using System;
using System.Text;

namespace LolBruteForceBypass
{
    public class Program
    {
         static public async Task Main(string[] args)
         {
            string username, password;

            Console.WriteLine("Username: ");
            username = Console.ReadLine();
            Console.WriteLine("Password: ");
            password = Console.ReadLine();

            var handler = new HttpClientHandler();
            handler.AutomaticDecompression = System.Net.DecompressionMethods.All;
            var client = new HttpClient(handler);
            
            string nonce = RandomString(22);

            string data1 = "{\"acr_values\":\"\",\"claims\":\"\",\"client_id\":\"riot-client\",\"code_challenge\":\"\",\"code_challenge_method\":\"\",\"nonce\":\""+nonce+"\",\"redirect_uri\":\"http://localhost/redirect\",\"response_type\":\"token id_token\",\"scope\":\"openid link ban lol_region account\"}";
            var content = new StringContent(data1.ToString(), Encoding.UTF8, "application/json");

            var url = "https://auth.riotgames.com/api/v1/authorization";

            client.DefaultRequestVersion = new Version(2, 0);
            client.DefaultRequestHeaders.Add("Host", "auth.riotgames.com");
            client.DefaultRequestHeaders.Add("Accept-Encoding", "deflate, gzip, zstd");
            client.DefaultRequestHeaders.Add("user-agent", "RiotClient/50.0.0.4396195.4381201 rso-auth (Windows;10;;Professional, x64)");
            client.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            var response = await client.PostAsync(url, content);

            string data2 = "{\"language\":\"en_GB\",\"password\":\"" + password + "\",\"region\":null,\"remember\":false,\"type\":\"auth\",\"username\":\"" + username + "\"}";
            var content2 = new StringContent(data2.ToString(), Encoding.UTF8, "application/json");
          
            url = "https://auth.riotgames.com/api/v1/authorization";

            response = await client.PutAsync(url, content2);

            Console.WriteLine(await response.Content.ReadAsStringAsync());
            Console.ReadKey();
         }

        static private Random random = new Random();
        static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}
