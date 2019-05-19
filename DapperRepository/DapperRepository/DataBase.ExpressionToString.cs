using DapperRepository.Interfaces;
using System;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace DapperRepository
{
    public abstract partial class DataBase : IDataBase
    {
        private static string ConvertExpressionToString(Expression body)
        {
            if (body == null)
            {
                return string.Empty;
            }
            if (body is ConstantExpression)
            {
                return ValueToString(((ConstantExpression)body).Value);
            }
            if (body is MemberExpression)
            {
                var member = ((MemberExpression)body);
                if (member.Member.MemberType == MemberTypes.Property)
                {
                    return member.Member.Name;
                }
                var value = GetValueOfMemberExpression(member);
                if (value is IEnumerable)
                {
                    var sb = new StringBuilder();
                    foreach (var item in value as IEnumerable)
                    {
                        sb.AppendFormat("{0},", ValueToString(item));
                    }
                    return sb.Remove(sb.Length - 1, 1).ToString();
                }
                return ValueToString(value);
            }
            if (body is UnaryExpression)
            {
                return ConvertExpressionToString(((UnaryExpression)body).Operand);
            }
            if (body is BinaryExpression)
            {
                var binary = body as BinaryExpression;
                return string.Format("({0}{1}{2})", ConvertExpressionToString(binary.Left),
                    ConvertExpressionTypeToString(binary.NodeType),
                    ConvertExpressionToString(binary.Right));
            }
            if (body is MethodCallExpression)
            {
                var method = body as MethodCallExpression;
                return string.Format("({0} IN ({1}))", ConvertExpressionToString(method.Arguments[0]),
                    ConvertExpressionToString(method.Object));
            }
            if (body is LambdaExpression)
            {
                return ConvertExpressionToString(((LambdaExpression)body).Body);
            }
            return "";
        }

        private static string ValueToString(object value)
        {
            if (value is string)
            {
                return string.Format("'{0}'", value);
            }
            if (value is DateTime)
            {
                return string.Format("'{0:yyyy-MM-dd HH:mm:ss}'", value);
            }
            return value.ToString();
        }

        private static object GetValueOfMemberExpression(MemberExpression member)
        {
            var objectMember = Expression.Convert(member, typeof(object));
            var getterLambda = Expression.Lambda<Func<object>>(objectMember);
            var getter = getterLambda.Compile();
            return getter();
        }

        private static string ConvertExpressionTypeToString(ExpressionType nodeType)
        {
            switch (nodeType)
            {
                case ExpressionType.And:
                    return " AND ";
                case ExpressionType.AndAlso:
                    return " AND ";
                case ExpressionType.Or:
                    return " OR ";
                case ExpressionType.OrElse:
                    return " OR ";
                case ExpressionType.Not:
                    return "NOT";
                case ExpressionType.NotEqual:
                    return "<>";
                case ExpressionType.Equal:
                    return "=";
                case ExpressionType.GreaterThan:
                    return ">";
                case ExpressionType.GreaterThanOrEqual:
                    return ">=";
                case ExpressionType.LessThan:
                    return "<";
                case ExpressionType.LessThanOrEqual:
                    return "<=";
                default:
                    return "";
            }
        }

    }
}
