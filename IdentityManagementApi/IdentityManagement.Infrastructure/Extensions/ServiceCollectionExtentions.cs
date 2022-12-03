using System;
using IdentityManagement.Infrastructure.Persistence;
using IdentityManagement.Infrastructure.Services;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityManagement.Infrastructure.Extensions
{
	public static class ServiceCollectionExtentions
	{
		public static IServiceCollection AddIdentityServerConfig(this IServiceCollection service, IConfiguration configuration)
		{
			service.AddIdentity<AppUser, AppRole>(options =>
			{
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 0;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.User.AllowedUserNameCharacters = "abcçdefghiıjklmnoöpqrsştuüvwxyzABCÇDEFGHIİJKLMNOÖPQRSŞTUÜVWXYZ0123456789-._@+'#!/^%{}*";
            }).AddEntityFrameworkStores<AppIdentityDbContext>()
            .AddDefaultTokenProviders();

            service.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder => builder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
                    options.EnableTokenCleanup = true;
                }).AddAspNetIdentity<AppUser>();

			return service;
		}

        public static IServiceCollection AddServices<TUser>(this IServiceCollection services)
        {
            services.AddTransient<IProfileService, IdentityClaimsProfileService>();
            return services;
        }

        public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AppIdentityDbContext>(options => options.UseNpgsql(connectionString));
            services.AddDbContext<AppPersistenceGrantDbContext>(options => options.UseNpgsql(connectionString));
            services.AddDbContext<AppConfigurationDbContext>(options => options.UseNpgsql(connectionString));
            return services;
        }
	}
}

