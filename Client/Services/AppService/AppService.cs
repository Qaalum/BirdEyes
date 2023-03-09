using System.Net.Http; // for HttpClient
using System.Net.Http.Json; // for HttpClientJsonExtensions

namespace BirdEyes.Client.Services.AppService
{
    public class AppService : IAppService
    {
        private readonly HttpClient _http;

        public AppService(HttpClient http)
        {
            _http = http;
        }

        public List<Application> AllMockApps { get; set; } = new List<Application>();
        public List<Publisher> AllMockPublishers { get; set; } = new List<Publisher>();
        public List<Developer> AllMockDevelopers { get; set; } = new List<Developer>();

        public async Task GetApps()
        {
            var result = await _http.GetFromJsonAsync<List<Application>>("api/app");
            if (result != null)
                AllMockApps = result;
        }

        public Task GetDevelopers()
        {
            throw new NotImplementedException();
        }

        public Task GetPublishers()
        {
            throw new NotImplementedException();
        }

        public Task<Application> GetMockApp(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Developer> GetMockDeveloper(string name)
        {
            throw new NotImplementedException();
        }

        public Task<Publisher> GetMockPublisher(string PubName)
        {
            /*            var pub = AllPublishers.FirstOrDefault(pubs => pubs.Name == PubName);
                        if (pub != null)
                            return pub;
                        else
                            throw new NullReferenceException("Couldn't find that publisher.");
            */
            throw new NotImplementedException();
        }
    }
}
