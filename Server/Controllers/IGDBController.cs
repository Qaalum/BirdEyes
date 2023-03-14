using IGDB;
using IGDB.Models;
using Microsoft.AspNetCore.Mvc;

namespace BirdEyes.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class IGDBController : ControllerBase
	{
		IGDBClient igdbClient = new IGDBClient("ooyem18exvjha2nkwwfrwhbei8nv0c", "b6qyv98cyvtz50nhizwip2zfmgxt7y"); //Secret should stay private 


		[HttpGet]
		public async Task<IActionResult> GetAllGames()
		{
			var allIGDBGames = await igdbClient.QueryAsync<Game>(IGDBClient.Endpoints.Games, query: "fields id,name;");
			return Ok(allIGDBGames);
		}

		/*[HttpGet("{id}")]
		public async Task<IActionResult> GetGame()
		{
			var IGDBGame = await igdbClient.QueryAsync<Game>(IGDBClient.Endpoints.Games, query: "fields id,name; where id = {id};");
			return Ok(IGDBGame);
		}*/
	}
}

