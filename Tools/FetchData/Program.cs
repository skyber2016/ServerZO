using Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FetchData
{
    class Program
    {
        static StringBuilder tableData = new StringBuilder();

        static HttpClient Http { get; set; }
        static void FetchData(Type type, string q)
        {
            //tableData.AppendLine($"DELETE FROM {type.Name};");
            var offset = 1000;
            var page = 0;
            var dataBK = new List<dynamic>();
            while (true)
            {
                var query = $"{q} LIMIT {offset} OFFSET {offset * page++}";
                Console.WriteLine(query);
                var dt = new Dictionary<string, string>()
                {
                    ["db"] = "jz",
                    ["q"] = query
                };
                var data = new FormUrlEncodedContent(dt);
                var result = Http.PostAsync("http://103.89.85.15/auth/fetch.php", data);
                result.Wait();
                var str = result.Result.Content.ReadAsStringAsync();
                str.Wait();
                List<cq_action> dataResult = new List<cq_action>();
                try
                {
                    dataResult = JsonConvert.DeserializeObject<List<cq_action>>(str.Result);
                }
                catch (Exception)
                {
                    using(var stream = new StreamWriter("error.log", true))
                    {
                        stream.WriteLine(type.Name);
                    }
                    break;
                }
                if (dataResult.Count == 0)
                {
                    break;
                }
                
                
                var queryInsert = new StringBuilder();
                
                var dataInsert = new StringBuilder();
                foreach (var item in dataResult)
                {
                    tableData.AppendLine($"UPDATE `JZ`.`cq_action` SET `id_next` = {item.id_next}, `id_nextfail` = {item.id_nextfail}, `type` = {item.type}, `data` = {item.data}, `param` = '{item.param.Replace("'", "`")}' WHERE `id` = {item.id}; ");
                }


            }
            File.WriteAllText(type.Name + ".sql", tableData.ToString());
        }
        static void Main(string[] args)
        {
            Http = new HttpClient();
            var ass = Assembly.GetExecutingAssembly().DefinedTypes.ToList();
            FetchData(typeof(cq_action), $"select * from cq_action where type = 101 or type = 102 or type = 126 or type = 1010");
            File.WriteAllText($"database.sql", tableData.ToString());
        }
    }
}
