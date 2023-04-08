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
		public static List<string> TrimGameResponse(string trimString, string shop)
		{
			trimString.Trim("\"\\/:{}1234567890".ToCharArray()); //This is why names can have no numbers in them 

			List<string> resultantStrings = new List<string>();
			resultantStrings.AddRange(trimString.Split(','));

			foreach (string str in resultantStrings)
			{
				if (str.Contains("data"))
					str.Remove(0, 4+shop.Length);
				else if (str.Length>2)
				{
					Console.WriteLine(str);
					str.Remove(0, 3);
				}
				else
				{
					str.Remove(0, str.Length);
				}
			}

			return resultantStrings;
		}

		static double trimPriceResponse(string trimString)
		{
			double resultantDouble = new double();
			var match = Regex.Match(trimString, @"\d+(\.\d+)?");
			if (match.Success)
			{
				resultantDouble = double.Parse(match.Value, CultureInfo.InvariantCulture);
			}
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
					shopGames.AddRange(TrimGameResponse(shopGameResponse, shop.ToString()));
				else
				{
					return BadRequest("shopGameResponse is null");
				}


				foreach (var game in shopGames)
				{
					RestRequest shopPriceRequest = new RestRequest("prices/?key="+ITADKey+"&plains="+game+"&shops="+shop);
					var shopPriceResponse = restClient.ExecuteAsync(shopPriceRequest).Result.Content;

					//One game's price
					double shopGamePrice = trimPriceResponse(shopPriceResponse);


					for (int i = 0; i < shopGames.Count; i++)
						allITADGames.Add(new ITADGameModel(shopGames.ElementAt(i), shopGamePrice));
				}
			}


			if (allITADGames.Count > 0)
				return Ok(allITADGames);
			else
				return BadRequest("No games were found 🤔");
		}
	}
}
