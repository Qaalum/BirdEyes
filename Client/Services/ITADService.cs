namespace BirdEyes.Client.Services.IGDBService
{
	public class ITADService : IITADService
	{
		public static List<Application> allGames { get; set; } = new();
		string[] shops = { "steam", "gog", "greenmangaming", "indiegamestand", "amazonus", "humblestore", "nuuvem", "getgames", "desura", "indiegalastore", "gamefly", "origin", "epic", "fanatical", "shinyloot", "voidu", "itchio", "gamersgate", "noctre", "gamebillet", "gamesplanetus", "gamesplanetde", "gamesplanetuk", "wingamestore", "allyouplay", "etailmarket", "joybuggy" };

		public static List<Application> InterpretStringResponse(string response)
		{
			var shopSplitStrings = response.Split("|");
			for (int a = 0; a < shopSplitStrings.Count(); a++)
			{
				var gameSplitStrings = shopSplitStrings[a].Split(",");

				for (int b = 1; b < gameSplitStrings.Count()-1; b++)
				{
					allGames.Add(new Application(gameSplitStrings[b].TakeWhile(c => !Char.IsLetterOrDigit(c)).ToArray().ToString(), gameSplitStrings[0], Double.Parse(gameSplitStrings[b].Remove(0, gameSplitStrings[b].IndexOf(":")))));
				}
			}

			return allGames;
		}

		private readonly HttpClient _http;

		public ITADService()
		{
			_http = new();
		}


		public async Task GetAllGames()
		{
			var result = InterpretStringResponse((await _http.GetAsync("api/itad")).ToString());
		}
	}
}