using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LocalizationExample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("de")
                };

                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;

                options.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(context =>
                {
                    var languages = context.Request.Headers["Accept-Language"].ToString();
                    var currentLanguage = languages.Split(',').FirstOrDefault();
                    var defaultLanguage = string.IsNullOrEmpty(currentLanguage) ? "en-US" : currentLanguage;

                    if (defaultLanguage != "de" && defaultLanguage != "en-US")
                    {
                        defaultLanguage = "en-US";
                    }

                    return Task.FromResult(new ProviderCultureResult(defaultLanguage, defaultLanguage));
                }));
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var localizationOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(localizationOptions.Value);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        //private void AddLocalizationSection(IServiceCollection services, IConfiguration configuration)
        //{
        //    services.AddLocalization(options =>
        //    {
        //        options.ResourcesPath = "Resources";
        //    });

        //    services.Configure<RequestLocalizationOptions>(options =>
        //    {
        //        var supportedCultures = new[]
        //        {
        //            new CultureInfo("en-US"),
        //            new CultureInfo("tr-TR")
        //        };

        //        options.DefaultRequestCulture = new RequestCulture(culture: "tr-TR");
        //        options.SupportedCultures = supportedCultures;
        //        options.SupportedUICultures = supportedCultures;

        //        options.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(context =>
        //        {
        //            var languages = context.Request.Headers["Accept-Language"].ToString();
        //            var currentLanguage = languages.Split(',').FirstOrDefault();
        //            var defaultLanguage = string.IsNullOrEmpty(currentLanguage) ? "tr-TR" : currentLanguage;

        //            if (defaultLanguage == "tr")
        //            {
        //                defaultLanguage = "tr-TR";
        //            }

        //            return Task.FromResult(new ProviderCultureResult(defaultLanguage, defaultLanguage));
        //        }));
        //    });
        //}
    }
}
