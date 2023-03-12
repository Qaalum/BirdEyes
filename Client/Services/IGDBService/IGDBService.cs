using System.Net.Http.Json; // for HttpClientJsonExtensions

namespace BirdEyes.Client.Services.IGDBService
{
    public class IGDBService : IIGDBService
    {
        private readonly HttpClient _http;

        public IGDBService(HttpClient http)
        {
            _http = http;
        }

        public List<Application> AllGames { get; set; } = new List<Application>();

        public async Task GetAllGames()
        {
            var result = await _http.GetFromJsonAsync<List<Application>>("api/igdb");
            if (result != null)
                AllGames = result;
        }
    }
}
