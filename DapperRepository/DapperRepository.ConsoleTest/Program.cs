using DapperRepository.Context;
using DapperRepository.Model;
using System;
using System.Collections.Generic;

namespace DapperRepository.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var before2 = GC.CollectionCount(2);
            var before1 = GC.CollectionCount(1);
            var before0 = GC.CollectionCount(0);

            var dataBase = new MysqlContext();
            var repo = new Repository<Supplier>(dataBase);

            //Insert 
            Supplier supplier1 = new Supplier();
            supplier1.Name = "SUPLIER 1";
            supplier1.Products = new List<Product>
            {
                new Product { Name = "PRODUCT TEST 1 ", Price = 10, Quantity = 1 },
                new Product { Name = "PRODUCT TEST 2 ", Price = 5, Quantity = 4 }
            };

            repo.Add(supplier1);

            repo.SaveChanges();

            Supplier supplier2 = new Supplier();
            supplier2.Name = "SUPLIER 2";
            supplier2.Products = new List<Product>
            {
                new Product { Name = "PRODUCT TEST 3", Price = 10, Quantity = 1 },
                new Product { Name = "PRODUCT TEST 4 ", Price = 5, Quantity = 4 }
            };

            repo.Add(supplier2);

            repo.SaveChanges();
            //End Insert 

            var SupplierGetAll = repo.GetAll();
            var SupplierGetId = repo.GetbyId(2);
            var SupplierGet = repo.Get(s => s.Name == "SUPLIER 1");

            watch.Stop();
            TimeSpan ts = watch.Elapsed;

            System.Console.WriteLine($"Tempo total: {watch.ElapsedMilliseconds}ms");
            System.Console.WriteLine($"GC Gen #2  : {GC.CollectionCount(2) - before2}");
            System.Console.WriteLine($"GC Gen #1  : {GC.CollectionCount(1) - before1}");
            System.Console.WriteLine($"GC Gen #0  : {GC.CollectionCount(0) - before0}");
            System.Console.WriteLine("Done!");

            System.Console.WriteLine("Tempo: " + ts.ToString("mm\\:ss\\.ff"));
            System.Console.WriteLine();


            System.Console.ReadKey();


        }
    }
}
