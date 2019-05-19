using MySql.Data.MySqlClient;

namespace DapperRepository.Context
{
    public class MysqlContext : DataBase
    {
        public MysqlContext() : base(new MySqlConnection("server=localhost;userid=root;password=test;database=test;Port=3306"))
        {

        }
    }
}
