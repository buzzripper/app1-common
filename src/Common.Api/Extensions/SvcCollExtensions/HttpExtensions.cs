using Dyvenix.App1.Common.Api.Services;
using Dyvenix.App1.Common.Shared.Contracts;

namespace Dyvenix.App1.Common.Api.Extensions.SvcCollExtensions;

public static class HttpExtensions
{
	public static IServiceCollection AddCurrentUserServices(this IServiceCollection services)
	{
		services.ConfigureHttpClientDefaults(http =>
		{
			// Turn on resilience by default
			http.AddStandardResilienceHandler();

			// Turn on service discovery by default
			http.AddServiceDiscovery();
		});

		services.AddHttpContextAccessor();
		services.AddScoped<ICurrentUserService, HttpContextCurrentUserService>();

		return services;
	}
}
