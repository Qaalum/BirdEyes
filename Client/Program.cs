global using BirdEyes.Shared;
using BirdEyes.Client;
using BirdEyes.Client.Services.ITADService;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("BirdEyes.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
	.AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("BirdsEyes.ServerAPI")); // Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped<IITADService, ITADService>();
builder.Services.AddApiAuthorization();

await builder.Build().RunAsync();
