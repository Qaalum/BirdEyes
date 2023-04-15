using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System.Globalization;
using System.Text.RegularExpressions;

namespace BirdEyes.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ITADController : Controller
	{
		string[] shops = { "steam", "gog", "greenmangaming", "indiegamestand", "amazonus", "humblestore", "nuuvem", "getgames", "desura", "indiegalastore", "gamefly", "origin", "epic", "fanatical", "shinyloot", "voidu", "itchio", "gamersgate", "noctre", "gamebillet", "gamesplanetus", "gamesplanetde", "gamesplanetuk", "wingamestore", "allyouplay", "etailmarket", "joybuggy" };
		private string ITADKey = "46594d518d6e4aedb823ecb4e6d00a54a10f1155";
		RestClient restClient = new RestClient("https://api.isthereanydeal.com/v01/game/");

		List<string> TrimGamesResponse(string inputString, string shop)
		{
			List<string> resultantSs = new List<string>();
			string[] trimStrings = inputString.Split(",");

			for (int i = 0; i<trimStrings.Count(); i++)
			{
				string str = trimStrings[i];
				string s = new string(str.Where(c => char.IsLetterOrDigit(c)).ToArray());

				if (s.Length > 2)
				{
					if (str == trimStrings[0])
						s = s.Remove(0, 7+shop.Length);
					else if (str.Contains("app\\") || str.Contains("sub\\"))
						s = s.Remove(0, 3);
					else
						s = s.Remove(0, 6);
				}
				else
					s = null;
				resultantSs.Add(s);
			}
			return resultantSs;
		}

		double trimPriceResponse(string trimString)
		{
			double resultantDouble;
			var match = Regex.Match(trimString, @"\d+(\.\d+)?");
			if (match.Success)
				resultantDouble = double.Parse(match.Value, CultureInfo.InvariantCulture);
			else
				resultantDouble = 0;

			return resultantDouble;
		}


		public async Task<IActionResult> GetAllITADGames()
		{
			string allITADGames = String.Empty;
			List<string> shopGames = new List<string>();


			for (int i = 0; i < shops.Count(); i++)
			{
				string shop = shops[i];
				RestRequest shopGamesRequest = new RestRequest("plain/list/?key="+ITADKey+"&shops="+shop, Method.Get);
				var shopGamesResponse = restClient.ExecuteAsync(shopGamesRequest).Result.Content;

				shopGames.AddRange(TrimGamesResponse(shopGamesResponse, shop));

				for (int b = 0; b<shopGames.Count; b++)
				{
					string game = shopGames[b];

					RestRequest shopPriceRequest = new RestRequest("prices/?key="+ITADKey+"&plains="+game+"&shop="+shop);
					var shopPriceResponse = restClient.ExecuteAsync(shopPriceRequest).Result.Content;

					double shopGamePrice = trimPriceResponse(shopPriceResponse);

					allITADGames = String.Join(allITADGames, "|"+shop+","+game+":"+shopGamePrice+","); //Easily fixed
				}
			}

			return Ok(allITADGames);
		}
	}
}
