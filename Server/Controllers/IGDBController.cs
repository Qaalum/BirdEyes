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
        IGDBClient igdb = new IGDBClient(
        Environment.GetEnvironmentVariable("ooyem18exvjha2nkwwfrwhbei8nv0c"),
        Environment.GetEnvironmentVariable("b6qyv98cyvtz50nhizwip2zfmgxt7y")
        );



        [HttpGet]
        public async Task<IActionResult> ListAllApps()
        {
            var AllGames = await igdb.QueryAsync<Game>(IGDBClient.Endpoints.Games, query: "fields id,name,; where id > 0;");
            return Ok(AllGames);
        }    

        /*public async Task<IActionResult> GetApps()
        {
            return Ok(AllApps);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetApp(int id)
        {
            var app = AllApps.FirstOrDefault(apps => apps.Id == id);
            if (app == null) return NotFound("40404040404040404040404");
            return Ok(app);
        }*/
    
    }
}

