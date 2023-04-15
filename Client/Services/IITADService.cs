namespace BirdEyes.Client.Services.IGDBService
{
	public interface IITADService
	{
		Dictionary<string, List<Application>> allShopGames { get; set; }

		Task GetAllGames();
	}
}