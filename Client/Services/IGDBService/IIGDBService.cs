namespace BirdEyes.Client.Services.IGDBService
{
    public interface IIGDBService
    {
        List<Application> AllApps { get; set; }
        List<Publisher> AllPublishers { get; set; }
        List<Developer> AllDevelopers { get; set; }

        Task GetApps();
        Task GetPublishers();
        Task GetDevelopers();
    }
}
