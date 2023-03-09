namespace BirdEyes.Client.Services.SteamService
{
	public interface ISteamService
	{
		List<Application> AllGames { get; set; }

		Task GetAllGames();
	}
}
