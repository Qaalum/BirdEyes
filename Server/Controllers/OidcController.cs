﻿using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Mvc;

namespace BirdEyes.Server.Controllers
{
	public class OidcController : Microsoft.AspNetCore.Mvc.Controller
	{
		private readonly ILogger<OidcController> _logger;

		public OidcController(IClientRequestParametersProvider clientRequestParametersProvider, ILogger<OidcController> logger)
		{
			ClientRequestParametersProvider = clientRequestParametersProvider;
			_logger = logger;
		}

		public IClientRequestParametersProvider ClientRequestParametersProvider { get; }

		[HttpGet("_configuration/{clientId}")]
		public IActionResult GetClientRequestParameters([FromRoute] string clientId)
		{
			var parameters = ClientRequestParametersProvider.GetClientParameters(HttpContext, clientId);
			return Ok(parameters);
		}
	}
}