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
        IGDBClient igdb = new IGDBClient("ooyem18exvjha2nkwwfrwhbei8nv0c", "b6qyv98cyvtz50nhizwip2zfmgxt7y");
        

        [HttpGet]
        public async Task<IActionResult> GetAllGames()
        {
            var AllGames = await igdb.QueryAsync<Game>(IGDBClient.Endpoints.Games, query: "fields id,name;");
            return Ok(AllGames);
        }

        [HttpGet("{id}")]
		public async Task<IActionResult> GetGame()
		{
			var AllGames = await igdb.QueryAsync<Game>(IGDBClient.Endpoints.Games, query: "fields id,name; where id = {id};");
			return Ok(AllGames);
		}
	}
}

