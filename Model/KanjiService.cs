using System.Net.Http;
using System.Text.Json;

namespace SideApp.Model
{
    internal class KanjiService
    {
        private static readonly HttpClient client = new HttpClient();
        public async Task<Kanji> GetKanjiAsync(string character)
        {
            string url = $"https://kanjiapi.dev/v1/kanji/{character}";

            HttpResponseMessage responce = await client.GetAsync(url);

            responce.EnsureSuccessStatusCode();

            string json = await responce.Content.ReadAsStringAsync();

            Kanji info = JsonSerializer.Deserialize<Kanji>(json);

            return info;
        }

        // This will get the whole list 
        public async Task<Kanji[]> GetKanjiListAsync(string list)
        {
            string url = $"https://kanjiapi.dev/v1/kanji/{list}";

            HttpResponseMessage responce = await client.GetAsync(url);

            responce.EnsureSuccessStatusCode();

            string json = await responce.Content.ReadAsStringAsync();

            Kanji[] randkan = JsonSerializer.Deserialize<Kanji[]>(json);

            return randkan;
        }
        
    }
}
