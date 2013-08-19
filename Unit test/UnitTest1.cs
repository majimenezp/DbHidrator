using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Data.Common;
using DbHidrator;
namespace Unit_test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var factories=DbProviderFactories.GetFactoryClasses();
            var factory=DbProviderFactories.GetFactory("System.Data.SqlClient");
            var conn = factory.CreateConnection();
            conn.ConnectionString = "data source=127.0.0.1;user id=testapp;password=testapp;initial catalog=testapp;MultipleActiveResultSets=true;";
            conn.Open();
            var tables=conn.GetSchema("Tables");
            var columns = conn.GetSchema("Columns");
            conn.Close();
        }

        [TestMethod]
        public void TestSchemaGeneration()
        {
            string conn="data source=127.0.0.1;user id=testapp;password=testapp;initial catalog=testapp;MultipleActiveResultSets=true;";
            string provider="System.Data.SqlClient";
            DbHidratorClient client = new DbHidratorClient(conn, provider);
            client.GenerateSchema();
        }

        [TestMethod]
        public void TestGenerateData()
        {
            string conn = "data source=127.0.0.1;user id=testapp;password=testapp;initial catalog=testapp;MultipleActiveResultSets=true;";
            string provider = "System.Data.SqlClient";
            DbHidratorClient client = new DbHidratorClient(conn, provider);
            client.GenerateSchema();
            string[] tables=new string[] {"Personas","TablaPruebas"};
            client.GenerateNRows(1000, tables);
        }
    }
}
