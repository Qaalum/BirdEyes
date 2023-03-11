using BirdEyes.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SteamWebAPI2.Interfaces;
using System.Collections.Immutable;
using Steam;
using SteamWebAPI2;
using SteamWebAPI2.Models;
using System.ComponentModel;
using SteamWebAPI2.Utilities;
using SteamKit2.Internal;
using Duende.IdentityServer.Models;
using Steam.Models;

namespace BirdEyes.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SteamController : ControllerBase
	{
		SteamWebInterfaceFactory webInterfaceFactory = new SteamWebInterfaceFactory("46ED8F983DCDFD3B9213903052CFED90"); //This key should stay private 
		SteamWebInterface steamInterface = webInterfaceFactory.CreateSteamWebInterface<SteamUser>(new HttpClient());


	}
}

