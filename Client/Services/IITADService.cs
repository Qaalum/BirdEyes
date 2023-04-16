namespace BirdEyes.Client.Services.ITADService
{
    public interface IITADService
    {
        Task<List<Application>> GetAllGames();
    }
}