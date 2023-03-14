using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System.Text;

namespace BirdEyes.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class GOGController : ControllerBase
	{
		RestClient restClient = new RestClient("https://api.gog.com/"); //Could not find the wrapper's client method, used RestSharp instead.

		[HttpGet]
		public async Task<IActionResult> GetAllGOGGames()
		{
			for (byte i = 1; i < byte.MaxValue; i++) //This will break if GOG goes from its current 155 pages to over 256... too bad! 
			{
				var clientId = "46899977096215655";
				var clientSecret = "9d85c43b1482497dbbce61f6e4aa173a433796eeae2ca8c5f6129f2dc4de46d9";
				var authString = $"{clientId}:{clientSecret}";
				var encodedString = Convert.ToBase64String(Encoding.UTF8.GetBytes(authString));


				RestRequest request = new RestRequest("v2/games?sort=popularity&page={i}", Method.Get);
				request.AddHeader("Authorization", "Bearer " + encodedString); //NEED A BEARER KEY DAMMIT!! 
				var response = restClient.Execute(request);

				if (response.IsSuccessful)
				{
					// Deserialize the response JSON into a C# object
					if (response.Content!=null)
					{
						var games = JsonConvert.DeserializeObject<GogApi.GalaxyApi.ProductsResponse>(response.Content);


						if (games!=null)
							return Ok(games);
						else
							return BadRequest("Error getting from GOG API probably ours: " + response.ErrorMessage);
					}
					else
						return BadRequest("Search *YIKES* in the directory, you'll get it." + response.ErrorMessage);
				}
				else
				{
					Console.WriteLine("Error getting from GOG API probably theirs: " + response.ErrorMessage);

					return BadRequest("GOG API response unsuccesful! " + response.ErrorMessage);
				}
			}
			return BadRequest("If you see this message, I don't know how it happened!");
		}
	}
}

