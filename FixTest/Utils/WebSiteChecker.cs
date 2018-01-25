using System.Net.Http;
using System.Threading.Tasks;

namespace FixTest.Utils
{
    internal static class WebSiteChecker
    {
        internal static async Task<bool> Check(string url)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    HttpResponseMessage response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Head, url));

                    response.EnsureSuccessStatusCode();

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}