using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;

namespace BirdEyes.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class GOGController : ControllerBase
	{
		RestClient restClient = new RestClient("https://api.gog.com/");

		[HttpGet]
		public async Task<IActionResult> GetAllGOGGames()
		{
			for (int i = 1; i < byte.MaxValue; i++) //This will break if GOG goes from its current 155 pages to over 256... too bad! 
			{
				RestRequest request = new RestRequest("v2/games?sort=popularity&page={i}", Method.Get);
				//request.AddHeader("Authorization", "Bearer <YOUR_API_KEY>");
				var response = restClient.Execute(request);


				if (response.IsSuccessful)
				{
					// Deserialize the response JSON into a C# object
					var games = JsonConvert.DeserializeObject<GameResponse>(response.Content);

					// Get all the games from the response
					var allGOGGames = games.Products;

					return Ok(allGOGGames);
				}
				else
				{
					Console.WriteLine("Error getting from GOG API (theirs not mine): " + response.ErrorMessage);

					return BadRequest("Lol, no games for you.");
				}
			}
		}
	}
}

