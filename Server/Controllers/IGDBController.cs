using BirdEyes.Shared;
using IGDB.Models;
using IGDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Immutable;

namespace BirdEyes.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IGDBController : ControllerBase
    {
        IGDBClient igdbClient = new IGDBClient("ooyem18exvjha2nkwwfrwhbei8nv0c", "b6qyv98cyvtz50nhizwip2zfmgxt7y"); //Secret shoudl stay private 
        

        [HttpGet]
        public async Task<IActionResult> GetAllGames()
        {
            var AllIGDBGames = await igdbClient.QueryAsync<Game>(IGDBClient.Endpoints.Games, query: "fields id,name;");
            return Ok(AllIGDBGames);
        }

        [HttpGet("{id}")]
		public async Task<IActionResult> GetGame()
		{
			var IGDBGame = await igdbClient.QueryAsync<Game>(IGDBClient.Endpoints.Games, query: "fields id,name; where id = {id};");
			return Ok(IGDBGame);
		}
	}
}

