using Microsoft.AspNetCore.Mvc;
using SteamWebAPI2.Interfaces;
using SteamWebAPI2.Utilities;

namespace BirdEyes.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SteamController : ControllerBase
	{
		SteamWebInterfaceFactory webInterfaceFactory = new SteamWebInterfaceFactory("46ED8F983DCDFD3B9213903052CFED90"); //This key should stay private 

		[HttpGet]
		public async Task<IActionResult> GetAllGames()
		{
			var steamInterface = webInterfaceFactory.CreateSteamWebInterface<SteamApps>(/* ISteamApps ??? */);

			var allSteamGames = await steamInterface.GetAppListAsync();
			if (allSteamGames != null)
				return Ok(allSteamGames);
			else
				return BadRequest("🤷‍♂️");
		}
	}
}

