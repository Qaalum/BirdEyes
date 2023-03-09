namespace BirdEyes.Client.Services.IGDBService
{
    public interface IIGDBService
    {
        List<Application> AllGames { get; set; }

        Task GetAllGames();
    }
}
