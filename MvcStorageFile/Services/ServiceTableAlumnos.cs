using Microsoft.Azure.Cosmos.Table;
using MvcStorage.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MvcStorage.Services
{
    public class ServiceTableAlumnos
    {
        //METODO ASYNC PARA PODER CONSUMIR EL TOKEN
        public async Task<String> GetTokenAsync(String curso)
        {
            String request = "api/access/token/" + curso;
            String url = "https://localhost:44342/";
            using (WebClient client = new WebClient())
            {
                client.Headers["content-type"] = "application/json";
                String content =
                    await client.DownloadStringTaskAsync(new Uri(url + request));
                JObject jobject = JObject.Parse(content);
                return jobject.GetValue("token").ToString();
            }
        }

        public List<Alumno> GetAlumnos(String token)
        {
            String uristorage = "http://127.0.0.1:10002/devstoreaccount1/";
            StorageCredentials credentials =
                new StorageCredentials(token);
            CloudTableClient client =
                new CloudTableClient(new Uri(uristorage), credentials);
            CloudTable tabla = client.GetTableReference("tablaalumnos");
            TableQuery<Alumno> query = new TableQuery<Alumno>();
            return tabla.ExecuteQuery(query).ToList();
        }
    }
}
