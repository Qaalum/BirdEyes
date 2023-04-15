namespace BirdEyes.Client.Services.IGDBService
{
	public class ITADService : IITADService
	{
		public Dictionary<string, List<Application>> allShopGames { get; set; } = new Dictionary<string, List<Application>>();
		string[] shops = { "steam", "gog", "greenmangaming", "indiegamestand", "amazonus", "humblestore", "nuuvem", "getgames", "desura", "indiegalastore", "gamefly", "origin", "epic", "fanatical", "shinyloot", "voidu", "itchio", "gamersgate", "noctre", "gamebillet", "gamesplanetus", "gamesplanetde", "gamesplanetuk", "wingamestore", "allyouplay", "etailmarket", "joybuggy" };


		public static Dictionary<string, List<Application>> InterpretStringResponse(string response)
		{
			Dictionary<string, List<Application>> shopGamePairs = new Dictionary<string, List<Application>>();

			var shopSplitStrings = response.Split("|");
			for (int a = 0; a < shopSplitStrings.Count(); a++)
			{
				var gameSplitStrings = shopSplitStrings[a].Split(",");


				for (int b = 1; b < gameSplitStrings.Count()-1; b++)
				{
					List<Application> shopGames = new List<Application>();
					shopGames.Add(new Application(gameSplitStrings[b].TakeWhile(c => !Char.IsLetterOrDigit(c)).ToArray().ToString(), Double.Parse(gameSplitStrings[b].Remove(0, gameSplitStrings[b].IndexOf(":")))));
					shopGamePairs[gameSplitStrings[0]] = shopGames;
				}
			}

			return shopGamePairs;
		}

		private readonly HttpClient _http;

		public ITADService(HttpClient http)
		{
			_http = http;
		}


		public async Task GetAllGames()
		{
			allShopGames = InterpretStringResponse((await _http.GetAsync("api/itad")).ToString());
		}
	}
}