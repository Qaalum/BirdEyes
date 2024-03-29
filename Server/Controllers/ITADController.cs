﻿using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System.Globalization;
using System.Text.RegularExpressions;

namespace BirdEyes.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ITADController : Controller
	{
		//Timer timer = new Timer(new TimerCallback(GetAllITADGames), null, 40000, Timeout.Infinite);
		string[] shops = { "steam", "gog", "greenmangaming", "indiegamestand", "amazonus", "humblestore", "nuuvem", "getgames", "desura", "indiegalastore", "gamefly", "origin", "epic", "fanatical", "shinyloot", "voidu", "itchio", "gamersgate", "noctre", "gamebillet", "gamesplanetus", "gamesplanetde", "gamesplanetuk", "wingamestore", "allyouplay", "etailmarket", "joybuggy" };
		private string ITADKey = "46594d518d6e4aedb823ecb4e6d00a54a10f1155";
		RestClient restClient = new RestClient("https://api.isthereanydeal.com/v01/game/");

		List<string> InterpretGamesResponse(string inputString, string shop)
		{
			List<string> resultantSs = new List<string>();
			if (inputString != null)
			{
				string[] trimStrings = inputString.Split(",");

				for (int i = 0; i<trimStrings.Count(); i++)
				{
					string str = trimStrings[i];
					string s = new string(str.Where(c => char.IsLetterOrDigit(c)).ToArray());

					if (s.Length > 2)
					{
						if (str == trimStrings[0])
							s = s.Remove(0, 7+shop.Length);
						else if (str.Contains("app\\") || str.Contains("sub\\"))
							s = s.Remove(0, 3);
						else
							s = s.Remove(0, 6);
					}
					else
						s = null;
					resultantSs.Add(s);
				}
				return resultantSs;
			}
			else
				return resultantSs;

		}

		double trimPriceResponse(string trimString)
		{
			double resultantDouble;
			var match = Regex.Match(trimString, @"\d+(\.\d+)?");
			if (match.Success)
				resultantDouble = double.Parse(match.Value, CultureInfo.InvariantCulture);
			else
				resultantDouble = 0;

			return resultantDouble;
		}


		public async Task<IActionResult> GetAllITADGames()
		{
			List<string> allITADGames = new();


			for (int a = 0; a < shops.Count(); a++)
			{
				List<string> shopGames = new();
				string shop = shops[a];
				RestRequest shopGamesRequest = new RestRequest("plain/list/?key="+ITADKey+"&shops="+shop, Method.Get);
				var shopGamesResponse = restClient.ExecuteAsync(shopGamesRequest).Result.Content;

				if (shopGamesResponse == null)
					return BadRequest("No shop games (╯°□°）╯︵ ┻━┻ [Check Github issue board]");

				shopGames.AddRange(InterpretGamesResponse(shopGamesResponse, shop));


				for (int b = 0; b<shopGames.Count; b++)
				{
					string game = shopGames[b];

					RestRequest shopPriceRequest = new RestRequest("prices/?key="+ITADKey+"&plains="+game+"&shop="+shop);
					var shopPriceResponse = restClient.ExecuteAsync(shopPriceRequest).Result.Content;

					if (shopPriceResponse == null)
						return BadRequest("No game price 😑");

					double shopGamePrice = trimPriceResponse(shopPriceResponse);


					allITADGames.Add(String.Join("", "|"+shop+","+game+":"+shopGamePrice+","));
				}
			}

			//timer.Change(40000, Timeout.Infinite);
			return Ok(String.Join("", allITADGames));
		}
	}
}
