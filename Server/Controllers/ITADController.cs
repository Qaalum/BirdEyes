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
		RestClient restClient = new RestClient("https://api.isthereanydeal.com/v01/game/"); //No wrapper, using RestSharp 

		//Deserializes the response's content into a list of strings 
		List<string> TrimGamesResponse(string inputString, string shop)
		{
			List<string> resultantSs = new List<string>();
			string[] trimStrings = inputString.Split(',');

			string s;
			foreach (string str in trimStrings)
			{
				Regex rgx = new Regex("[^a-zA-Z]");
				s = rgx.Replace(str, "");

				if (s.Length > 2)
				{
					if (str.Contains("data"))
						s = s.Remove(0, 4+shop.Length);
					s = s.Remove(0, 3);
				}
				else
					s.Remove(0, s.Length);
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
				throw new Exception("There's no price in the priceResponse" + trimString);

			return resultantDouble;
		}


		enum Shop { steam, gog, greenmangaming, indiegamestand, amazonus, humblestore, nuuvem, getgames, desura, indiegalastore, gamefly, origin, epic, fanatical, shinyloot, voidu, itchio, gamersgate, noctre, gamebillet, gamesplanetus, gamesplanetde, gamesplanetuk, wingamestore, allyouplay, etialmarket, joybuggy }
		public async Task<IActionResult> GetAllITADGames()
		{
			List<ITADGameModel> allITADGames = new List<ITADGameModel>();


			foreach (var shop in (Shop[])Enum.GetValues(typeof(Shop)))
			{
				RestRequest shopGameRequest = new RestRequest("plain/list/?key="+ITADKey+"&shop="+shop, Method.Get);
				var shopGameResponse = restClient.ExecuteAsync(shopGameRequest).Result.Content;


				List<string> shopGames = new List<string>();

				//All games' titles
				if (shopGameResponse != null)
					shopGames.AddRange(TrimGamesResponse(shopGameResponse, shop.ToString()));
				else
				{
					return BadRequest("shopGameResponse is null");
				}


				foreach (var game in shopGames)
				{
					RestRequest shopPriceRequest = new RestRequest("prices/?key="+ITADKey+"&plains="+game+"&shops="+shop);
					var shopPriceResponse = restClient.ExecuteAsync(shopPriceRequest).Result.Content;

					if (shopPriceResponse == "{\"error\":\"missing_params\",\"error_description\":\"Required parameter 'plains' is missing, refer to documentation\"}")
					{
						Console.WriteLine(game + " is not a plain ITAD recognizes!");
					}
					else
					{
						//One game's price
						double shopGamePrice = trimPriceResponse(shopPriceResponse);

						for (int i = 0; i < shopGames.Count; i++)
							allITADGames.Add(new ITADGameModel(shopGames.ElementAt(i), shopGamePrice));
					}

				}
			}


			if (allITADGames.Count > 0)
				return Ok(allITADGames);
			else
				return BadRequest("No games were found 🤔");
		}
	}
}
