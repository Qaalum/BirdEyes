namespace BirdEyes.Client.Services.GOGService
{
	public interface IGOGService
	{
		List<Application> AllGames { get; set; }

		Task GetAllGames();
	}
}
