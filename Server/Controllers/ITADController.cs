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

			for (int i = 0; i<trimStrings.Count(); i++)
			{
				string str = trimStrings[i];

				char[] arr = str.Where(c => (char.IsLetterOrDigit(c) || char.IsWhiteSpace(c))).ToArray();
				string s = new string(arr);

				if (s.Length > 2)
				{
					if (str == trimStrings.ElementAt(0))
						s = s.Remove(0, 7+shop.Length);
					else if (str.Contains("app\\/") || str.Contains("sub\\/"))
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
				resultantDouble= double.NaN;

			return resultantDouble;
		}


		//enum Shop { steam, gog, greenmangaming, indiegamestand, amazonus, humblestore, nuuvem, getgames, desura, indiegalastore, gamefly, origin, epic, fanatical, shinyloot, voidu, itchio, gamersgate, noctre, gamebillet, gamesplanetus, gamesplanetde, gamesplanetuk, wingamestore, allyouplay, etialmarket, joybuggy }
		enum Shop { joybuggy }
		public async Task<IActionResult> GetAllITADGames()
		{
			List<ITADGameModel> allITADGames = new List<ITADGameModel>();
			List<string> shopGames = new List<string>();


			foreach (var shop in (Shop[])Enum.GetValues(typeof(Shop)))
			{
				RestRequest shopGamesRequest = new RestRequest("plain/list/?key="+ITADKey+"&shops="+shop.ToString(), Method.Get);
				var shopGamesResponse = restClient.ExecuteAsync(shopGamesRequest).Result.Content;

				shopGames.AddRange(TrimGamesResponse(shopGamesResponse, shop.ToString()));

				for (int b = 0; b<shopGames.Count; b++)
				{
					string game = shopGames[b];

					RestRequest shopPriceRequest = new RestRequest("prices/?key="+ITADKey+"&plains="+game+"&shop="+shop);
					var shopPriceResponse = restClient.ExecuteAsync(shopPriceRequest).Result.Content;

					double shopGamePrice = trimPriceResponse(shopPriceResponse);

					for (int c = 0; c < shopGames.Count; c++)
						allITADGames.Add(new ITADGameModel(shopGames.ElementAt(c), shopGamePrice));
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
