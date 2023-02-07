namespace BirdEyes.Client.Services.AppService
{
    public interface IAppService
    {
        List<Application> AllMockApps { get; set; }
        List<Publisher> AllMockPublishers { get; set; }
        List<Developer> AllMockDevelopers { get; set; }

        Task GetApps();
        Task GetPublishers();
        Task GetDevelopers();
        Task<Application> GetMockApp(int id);
        Task<Publisher> GetMockPublisher(string name);
        Task<Developer> GetMockDeveloper(string name);


    }
}
