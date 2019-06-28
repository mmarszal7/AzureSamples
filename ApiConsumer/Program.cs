using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiConsumer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var httpClient = new HttpClient();
            var client = new ApiClient(httpClient);
            var values = await client.GetValues();
            Console.WriteLine(values);
        }
    }
}
