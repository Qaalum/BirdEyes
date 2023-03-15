using BirdEyes.Shared;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;

namespace BirdEyes.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ITADController : Controller
	{

		private string ITADKey = "46594d518d6e4aedb823ecb4e6d00a54a10f1155";
		RestClient restClient = new RestClient("https://api.isthereanydeal.com/v01/game/"); //No wrapper ofcourse, using RestSharp 

		enum Shop { steam, gog, greenmangaming, indiegamestand, amazonus, humblestore, nuuvem, getgames, desura, indiegalastore, gamefly, origin, epic, fanatical, shinyloot, voidu, itchio, gamersgate, noctre, gamebillet, gamesplanetus, gamesplanetde, gamesplanetuk, wingamestore, allyouplay, etialmarket, joybuggy }
		public async Task<IActionResult> GetAllITADGames()
		{
			List<ITADGameModel> allITADGames = new List<ITADGameModel>();
			List<string> shopGames = new List<string>();
			List<double> gamePrices = new List<double>();

			foreach (var shop in (Shop[])Enum.GetValues(typeof(Shop)))
			{
				RestRequest shopGameRequest = new RestRequest("map/?key="+ITADKey+"&shop="+shop, Method.Get);
				var shopGameResponse = await restClient.GetAsync(shopGameRequest);

				shopGames.AddRange(JsonConvert.DeserializeObject<List<string>>(shopGameResponse.Content.ToString())); //Maybe use shopGameResponse.Content?


				foreach (var game in shopGames)
				{
					RestRequest shopPriceRequest = new RestRequest("prices/?key="+ITADKey+"&plains="+game+"&shops="+shop);
					var shopPriceResponse = await restClient.GetAsync(shopPriceRequest);

					gamePrices.Add(JsonConvert.DeserializeObject<double>(shopPriceResponse.Content.ToString()));
				}
			}

			for (int i = 0; i < shopGames.Count; i++)
				allITADGames.Add(new ITADGameModel(shopGames.ElementAt(i), gamePrices.ElementAt(i)));

			static void TestData(List<string> titles, List<double> prices)
			{
				if (titles.Count != prices.Count)
					throw new Exception();

			}

			try
			{
				TestData(shopGames, gamePrices);
			}
			catch (Exception gamePriceMismatch)
			{
				Console.WriteLine(gamePriceMismatch.Message);
			}

			if (allITADGames.Count > 0)
				return Ok(allITADGames);
			else
				return BadRequest("No games were found 🤔");
		}
	}
}
