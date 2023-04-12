using BirdEyes.Shared;
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
		private string ITADKey = "46594d518d6e4aedb823ecb4e6d00a54a10f1155";
		RestClient restClient = new RestClient("https://api.isthereanydeal.com/v01/game/");

		List<string> TrimGamesResponse(string inputString, string shop)
		{
			List<string> resultantSs = new List<string>();
			string[] trimStrings = inputString.Split(",");

			string s;
			foreach (string str in trimStrings)
			{
				Regex rgx = new Regex("[^a-zA-Z]");
				s = rgx.Replace(str, "");

				if (s.Length > 2)
				{
					if (str == trimStrings.ElementAt(0))
						s = s.Remove(0, 7+shop.Length);
					else if (str.Contains("app\\/") || str.Contains("sub\\/"))
						s = s.Remove(0, 3);
					else if (str.Contains("bundle\\/"))
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
				throw new Exception("There's no price in the priceResponse: " + trimString);

			return resultantDouble;
		}


		enum Shop { steam, gog, greenmangaming, indiegamestand, amazonus, humblestore, nuuvem, getgames, desura, indiegalastore, gamefly, origin, epic, fanatical, shinyloot, voidu, itchio, gamersgate, noctre, gamebillet, gamesplanetus, gamesplanetde, gamesplanetuk, wingamestore, allyouplay, etialmarket, joybuggy }
		public async Task<IActionResult> GetAllITADGames()
		{
			List<ITADGameModel> allITADGames = new List<ITADGameModel>();
			List<string> shopGames = new List<string>();


			foreach (var shop in (Shop[])Enum.GetValues(typeof(Shop)))
			{
				RestRequest shopGamesRequest = new RestRequest("plain/list/?key="+ITADKey+"&shops="+shop.ToString(), Method.Get);
				var shopGamesResponse = restClient.ExecuteAsync(shopGamesRequest).Result.Content;

				if (shopGamesResponse != null || shopGamesResponse != "")
					shopGames.AddRange(TrimGamesResponse(shopGamesResponse, shop.ToString()));
				else
					return BadRequest("shopGamesResponse is empty");

				foreach (var game in shopGames)
				{
					RestRequest shopPriceRequest = new RestRequest("prices/?key="+ITADKey+"&plains="+game+"&shop="+shop);
					var shopPriceResponse = restClient.ExecuteAsync(shopPriceRequest).Result.Content;

					if (shopPriceResponse == "{\"error\":\"missing_params\",\"error_description\":\"Required parameter 'plains' is missing, refer to documentation\"}")
					{
						return BadRequest(game + " is not a plain ITAD recognizes!");
					}
					else
					{
						double shopGamePrice = trimPriceResponse(shopPriceResponse);

						for (int i = 0; i < shopGames.Count; i++)
							allITADGames.Add(new ITADGameModel(shopGames.ElementAt(i), shopGamePrice));
					}
				}
			}

			return BadRequest("Bad");
			if (allITADGames.Count > 0)
				return Ok(allITADGames);
			else
				return BadRequest("No games were found 🤔");
		}
	}
}
