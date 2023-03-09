using System.Net.Http; // for HttpClient
using System.Net.Http; // for HttpClient
using System.Net.Http.Json; // for HttpClientJsonExtensions
using IGDB;
using IGDB.Models;


namespace BirdEyes.Client.Services.IGDBService
{
    public class IGDBService : BirdEyes.Client.Services.IGDBService.IIGDBService
    {
        private readonly HttpClient _http;

        public IGDBService(HttpClient http)
        {
            _http = http;
        }

        public List<Application> AllApps { get; set; } = new List<Application>();
        public List<Publisher> AllPublishers { get; set; } = new List<Publisher>();
        public List<Developer> AllDevelopers { get; set; } = new List<Developer>();

        IGDBClient igdbClient = new IGDBClient(
        Environment.GetEnvironmentVariable("IGDB_CLIENT_ID"),
        Environment.GetEnvironmentVariable("IGDB_CLIENT_SECRET")
);

        public async Task GetApps()
        {
            var result = await _http.GetFromJsonAsync<List<Application>>("api/app");
            if (result != null)
                AllApps = result;
        }

        public Task GetDevelopers()
        {
            throw new NotImplementedException();
        }

        public Task GetPublishers()
        {
            throw new NotImplementedException();
        }
    }
}
