using BirdEyes.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Immutable;

namespace BirdEyes.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppController : ControllerBase
    {
        public static List<Application> AllMockApps = new List<Application>
        {
            new Application (0, "example", 60.00),
            new Application (1, "Heroes of Might and Magic 3: Complete", 9.99),
            new Application (2, "Minecraft: Java Edition", 29.99, false, 1.16, 100, "Microsoft", "Mojang", 69420), //(int id, string title, double price, bool onSale, int? version, int? rating, Publisher publisher, Developer developer, int downloads)
            new Application (3, "Apex Legends", 0)
        };
        
        [HttpGet]
        public async Task<IActionResult> GetApps()
        {
            return Ok(AllMockApps);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetApp(int id)
        {
            var app = AllMockApps.FirstOrDefault(apps => apps.Id == id);
            if (app == null) return NotFound("40404040404040404040404");
            return Ok(app);
        }
    };


    [Route("api/[controller]")]
    [ApiController]
    public class DevController : ControllerBase
    {

        public static List<Developer> AllMockDevelopers = new List<Developer>
        {
            new Developer ("Mojang"),
        };

        [HttpGet]
        public async Task<IActionResult> GetDevs()
        {
            return Ok(AllMockDevelopers);
        }

        [HttpGet("{DevName}")]
        public async Task<IActionResult> GetDev(string DevName)
        {
            var dev = AllMockDevelopers.FirstOrDefault(devs => devs.Name == DevName);
            if (dev == null) 
                return NotFound("It turns out, somehow, no game dev has that name!");
            else 
                return Ok(dev);
        }
        }

    [Route("api/[controller]")]
    [ApiController]
    public class PubController : ControllerBase
    {

        public static List<Developer> AllMockPublishers = new List<Developer>
        {
            new Developer ("Microsoft"),
        };

        [HttpGet]
        public async Task<IActionResult> GetPubs()
        {
            return Ok(AllMockPublishers);
        }

        [HttpGet("{PubName}")]
        public async Task<IActionResult> GetPub(string PubName)
        {
            var pub = AllMockPublishers.FirstOrDefault(pubs => pubs.Name == PubName);
            if (pub == null) 
                return NotFound("It turns out, somehow, no game dev has that name!");
            else 
                return Ok(pub);
        }
    }


}

