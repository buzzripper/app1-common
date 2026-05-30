using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Asp.Versioning;
using Dyvenix.App1.Common.Shared.Contracts;
using Dyvenix.App1.Common.Shared.Services;

namespace Dyvenix.App1.Common.Api.Extensions;

/// <summary>
/// Common extensions for API server hosts.
/// </summary>
public static class Extensions
{
	/// <summary>
	/// Configures JWT Bearer authentication using the OpenIddict authority.
	/// Reads settings from the "Authentication" configuration section.
	/// </summary>
	public static IServiceCollection AddJwtBearerAuthentication(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer(options =>
			{
				options.Authority = configuration["Authentication:Authority"];
				options.Audience = configuration["Authentication:Audience"];

				options.TokenValidationParameters.NameClaimType = "name";
				options.TokenValidationParameters.RoleClaimType = "role";
			});

		services.AddAuthorization();

		services.AddScoped<ITenantAccessService, TenantAccessService>();

		return services;
	}

	/// <summary>
	/// Configures standard API versioning with default version 1.0.
	/// </summary>
	public static IServiceCollection AddStandardApiVersioning(this IServiceCollection services)
	{
		services.AddApiVersioning(options =>
		{
			options.DefaultApiVersion = new ApiVersion(1, 0);
			options.AssumeDefaultVersionWhenUnspecified = true;
			options.ReportApiVersions = true;
		}).AddApiExplorer(options =>
		{
			options.GroupNameFormat = "'v'VVV";
			options.SubstituteApiVersionInUrl = true;
		});

		return services;
	}

	/// <summary>
	/// Configures the standard API middleware pipeline.
	/// </summary>
	public static WebApplication UseStandardApiPipeline(this WebApplication app)
	{
		app.UseHttpsRedirection();
		app.UseAuthentication();
		app.UseAuthorization();

		return app;
	}
}
