using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using AzureFunctionsEcommerce.Models;
using System.Data.SQLite;
using System.Web.Http;
using System.Net;

namespace AzureFunctionsEcommerce
{
    public static class StockCheckFunction
    {
        [FunctionName("StockCheck")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "stock/{id}")]
             HttpRequest req,
             ILogger log, int id)
        {
            log.LogInformation("Requested stock for product id " + id);

            var sqlite_conn = new SQLiteConnection(@"Data Source=C:\NetCoreEcommerce.db; Version = 3; New = True; Compress = True; ");

            try
            {
                sqlite_conn.Open();
            }
            catch (Exception ex)
            {
                log.LogInformation("Error: " + ex.Message);
            }

            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();

            sqlite_cmd.CommandText = "SELECT InStock FROM products WHERE Id = '" + id + "'";

            sqlite_datareader = sqlite_cmd.ExecuteReader();

            var result = new Product { };
            var goodResult = false;

            if (sqlite_datareader.HasRows)
            {
                goodResult = true;

                while (sqlite_datareader.Read())
                {
                    result.StockCount = sqlite_datareader.GetInt32(0);
                }
            }
            sqlite_conn.Close();

            if (goodResult)
            {                
                return new OkObjectResult(result);
            }
            else
            {
                return new NotFoundResult();
            }
        }
    }
}
