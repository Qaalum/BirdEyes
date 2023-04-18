namespace BirdEyes.Client.Services.ITADService
{
	public class ITADService : IITADService
	{
		public static List<Game> allGames { get; set; } = new();
		string[] shops = { "steam", "gog", "greenmangaming", "indiegamestand", "amazonus", "humblestore", "nuuvem", "getgames", "desura", "indiegalastore", "gamefly", "origin", "epic", "fanatical", "shinyloot", "voidu", "itchio", "gamersgate", "noctre", "gamebillet", "gamesplanetus", "gamesplanetde", "gamesplanetuk", "wingamestore", "allyouplay", "etailmarket", "joybuggy" };

		public static List<Game> InterpretStringResponse(string response)
		{
			var shopSplitStrings = response.Split("|");
			for (int a = 0; a < shopSplitStrings.Count(); a++)
			{
				var gameSplitStrings = shopSplitStrings[a].Split(",");

				for (int b = 1; b < gameSplitStrings.Count()-1; b++)
				{
					double price = 0;
					if (Char.IsDigit(gameSplitStrings[b][gameSplitStrings[b].IndexOf(":")+1]))
					{
						price = Double.Parse(gameSplitStrings[b].Remove(0, gameSplitStrings[b].IndexOf(":")));
						allGames.Add(new Game(gameSplitStrings[b].TakeWhile(c => !Char.IsLetterOrDigit(c)).ToArray().ToString(), gameSplitStrings[0], price));
					}
					else
					{
						allGames.Add(new Game(gameSplitStrings[b].TakeWhile(c => !Char.IsLetterOrDigit(c)).ToArray().ToString(), gameSplitStrings[0], price));
					}
				}
			}

			return allGames;
		}

		private readonly HttpClient _http = new();

		public ITADService(HttpClient http)
		{
			_http = http;
		}

		public async Task<List<Game>> GetAllGames()
		{
			var result = InterpretStringResponse((await _http.GetAsync("https://localhost:7196/api/itad")).ToString());
			return result;
		}
	}
}