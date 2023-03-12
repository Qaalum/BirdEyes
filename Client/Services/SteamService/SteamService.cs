﻿using System.Net.Http.Json;

namespace BirdEyes.Client.Services.SteamService
{
	public class SteamService : ISteamService
	{
		private readonly HttpClient _http;

		public SteamService(HttpClient http)
		{
			_http = http;
		}

		public List<Application> AllGames { get; set; } = new List<Application>();

		public async Task GetAllGames()
		{
			var result = await _http.GetFromJsonAsync<List<Application>>("api/steam"); 
			if (result != null)
				AllGames = result;
		}
	}
}