using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace weather_summary
{
    class Program
    {
        static async Task Main()
        {
            HttpClient client = new HttpClient();
            try
            {
                HttpResponseMessage response = await client.GetAsync("http://httpbin.org/get");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException caught");
                Console.WriteLine("Message: {0} ", e.Message);
            }
            client.Dispose();
        }
    }
}