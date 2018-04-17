using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Data;

namespace DataConnect
{
    class Program
    {
        static string source = "server=(local);" + "integrated security=SSPI;" + "database=Northwind";
        static void Main(string[] args)
        {
            try
            {
                string select = "select top 1 regionid from Region order by RegionID desc ";
                string select = "select regionid,regiondescription from region";
                GetNameFromCustomers(source, select);
                GetEmployeeAndOrder();
                SqlConnection conn = new SqlConnection(source);
                SqlDataAdapter da = new SqlDataAdapter(select, conn);
                da.UpdateCommand = new SqlCommand("update region set regiondescription = @regiondescription where regionid = @regionid", conn);
                da.UpdateCommand.Parameters.Add("@regiondescription", SqlDbType.NChar, 50, "regiondescription");
                //SqlParameter pa = da.UpdateCommand.Parameters.Add("@regionid", SqlDbType.Int);
                //pa.SourceColumn = "regionid";
                //pa.SourceVersion = DataRowVersion.Original;
                //DataTable dt = new DataTable();
                //da.Fill(dt);
                //DataRow r = dt.Rows[12];
                //r[1] = "test";
                da.Update(dt);
                foreach (DataRow row in dt.Rows)
                {
                    Console.WriteLine($"RegionID = {row[0], -5} RegionDescription = {row[1]}");
                }
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error: {ex.Message}");
                Console.Read();
            }
        }
        public async static void GetEmployeeAndOrder()
        {
            var employeeCount = await GetEmployeeCount();
            var orderCount = await GetOrderCount();
            Console.WriteLine("employeeCount: {0},  orderCount: {1}", employeeCount, orderCount);
        }
        private static async Task<int> GetEmployeeCount()
        {
            using (SqlConnection conn = new SqlConnection(source))
            {
                SqlCommand cmd = new SqlCommand("awaitfor delay '0:0:5';select count(*) from employees", conn);
                conn.Open();
                return await cmd.ExecuteScalarAsync().ContinueWith(t => Convert.ToInt32(t.Result));
            }
        }
        private static async Task<int> GetOrderCount()
        {
            using (SqlConnection conn = new SqlConnection(source))
            {
                SqlCommand cmd = new SqlCommand("awaitfor delay '0:0:5';select count(*) from orders", conn);
                conn.Open();
                return await cmd.ExecuteScalarAsync().ContinueWith(t => Convert.ToInt32(t.Result));
            }
        }
        private static void GetNameFromCustomers(string source, string select)
        {
            using (SqlConnection conn = new SqlConnection(source))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(select, conn);
                //SqlCommand cmd1 = new SqlCommand(select, conn);
                //int id = (int)cmd1.ExecuteScalar() + 1;
                //string insert = "insert into region(regionid,regiondescription) values(" + id + ",'dada')";
                //SqlCommand cmd2 = new SqlCommand(insert, conn);
                //cmd2.ExecuteNonQuery();
                //while (reader.Read())
                //{
                //    Console.WriteLine("Contact: {0, -20} Company: {1}", reader.GetString(0), reader.GetString(1));
                //}
                //Console.WriteLine("获取了数据");
            }
        }

        private static void Proc(string source)
        {
            using (SqlConnection conn = new SqlConnection(source))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("RegionInsert", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@regionID", System.Data.SqlDbType.Int,
                    0, System.Data.ParameterDirection.Output, false, 0, 0, "regionID",
                    System.Data.DataRowVersion.Default, null));
                cmd.Parameters["@regionid"].Value = 4;
                cmd.Parameters.Add(new SqlParameter("@regiondescription", System.Data.SqlDbType.NChar, 50,
                    "regiondescription"));
                cmd.Parameters["@regiondescription"].Value = "avcd";
                // cmd.UpdatedRowSource = System.Data.UpdateRowSource.OutputParameters;
                cmd.ExecuteNonQuery();
                Console.WriteLine(cmd.Parameters["@regionID"].Value);
            }
        }
    }
}
