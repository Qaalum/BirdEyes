using BirdEyes.Shared;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

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
				else
					str.Remove(0, 3);
			}

			return resultantStrings;
		}

		enum Shop { steam, gog, greenmangaming, indiegamestand, amazonus, humblestore, nuuvem, getgames, desura, indiegalastore, gamefly, origin, epic, fanatical, shinyloot, voidu, itchio, gamersgate, noctre, gamebillet, gamesplanetus, gamesplanetde, gamesplanetuk, wingamestore, allyouplay, etialmarket, joybuggy }
		public async Task<IActionResult> GetAllITADGames()
		{
			List<ITADGameModel> allITADGames = new List<ITADGameModel>();


			foreach (var shop in (Shop[])Enum.GetValues(typeof(Shop)))
			{
				RestRequest shopGameRequest = new RestRequest("plain/list/?key="+ITADKey+"&shop="+shop, Method.Get);
				var shopGameResponse = restClient.ExecuteAsync(shopGameRequest).Result;


				List<string> shopGames = new List<string>();

				//All games' titles
				if (shopGameResponse.Content != null)
					shopGames.AddRange(TrimGameResponse(shopGameResponse.Content, shop.ToString()));
				else
				{
					return BadRequest("shopGameResponse.Content is null");
					throw new Exception();
				}


				foreach (var game in shopGames)
				{
					RestRequest shopPriceRequest = new RestRequest("prices/?key="+ITADKey+"&plains="+game+"&shops="+shop);
					var shopPriceResponse = restClient.ExecuteAsync(shopPriceRequest).Result;

					//One game's price
					double shopGamePrice = new double();
					shopPriceResponse.Content.Remove(0, shopPriceResponse.Content.IndexOf("\"price_new\":")+12);
					shopGamePrice = Double.Parse((string)shopPriceResponse.Content.TakeWhile(Char.IsDigit));

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
