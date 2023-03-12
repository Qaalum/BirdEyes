﻿using System.Net.Http.Json;

namespace BirdEyes.Client.Services.GOGService
{
	public class GOGService : IGOGService
	{
		private readonly HttpClient _http;

		public GOGService(HttpClient http)
		{
			_http = http;
		}

		public List<Application> AllGames { get; set; } = new List<Application>();

		public async Task GetAllGames()
		{
			var result = await _http.GetFromJsonAsync<List<Application>>("api/igdb"); // Get from "https://api.gog.com/api/products" 
			if (result != null)
				AllGames = result;
		}
	}
}