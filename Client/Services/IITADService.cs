namespace BirdEyes.Client.Services.ITADService
{
	public interface IITADService
	{
		Task<List<Game>> GetAllGames();
	}
}