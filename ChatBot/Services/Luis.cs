using ChatBot.Serialization;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace ChatBot.Services
{
    public class Luis
    {
        public static async Task<Utterance> GetResponse(string message)
        {
            using (var client = new HttpClient())
            {
                //LUIS 的Programmantic API ID
                const string subscriptionkey = "subKey";

                //C# 6 called Interpolated Strings, {變數名稱} = > {authKey} {message}
                var url = $"https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/8de22ee1-619a-4c5a-816c-be0a0b8effc6?subscription-key={subscriptionkey}&timezoneOffset=0.0&verbose=true&q={message}";
                   
                client.DefaultRequestHeaders.Accept.Clear();
                //Header宣告成Json格式
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); 

                var response = await client.GetAsync(url); //呼叫LUIS Service

                if (!response.IsSuccessStatusCode) return null; //失敗直接返回

                var result = await response.Content.ReadAsStringAsync(); //讀取資料

                //將LUIS返回的Json 轉到 Utterance物件
                var js = new DataContractJsonSerializer(typeof(Utterance)); 
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
                var list = (Utterance)js.ReadObject(ms);

                return list;
            }
        }
    }
}