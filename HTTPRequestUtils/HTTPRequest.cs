using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HTTPRequestUtils
{
    public class HTTPRequest
    {
        private static readonly HTTPRequest instance = new HTTPRequest();
        private readonly HttpClient client;
        private HTTPRequest()
        {
            client = new HttpClient();
        }
        public static string Get(string url)
        {
            Uri endpoint = new Uri(url);

            HttpResponseMessage result = instance.client.GetAsync(endpoint).Result;
            Task<string> task = result.Content.ReadAsStringAsync();
            task.Wait();
            string json = task.Result;

            return json;
        }
        public static T Get<T>(string url) where T: new()
        {

            return JsonConvert.DeserializeObject<T>(Get(url)); ;
        }
    }
}
