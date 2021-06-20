using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using Gridcoin.WebApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Gridcoin.WebApi
{
    public class Startup
    {
        private readonly List<string> _scopes = new() { "read:info", "create:address", "create:transaction" };

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Gridcoin.WebApi", Version = "v1" });
            });

            services.AddHttpClient("gridcoin", x =>
            {
                var gridcoinSettings = Configuration.GetSection("Gridcoin").Get<GridcoinSettings>();
                x.BaseAddress = gridcoinSettings.Uri;
                var bytes = Encoding.ASCII.GetBytes($"{gridcoinSettings.Username}:{gridcoinSettings.Password}");
                x.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(bytes));
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = Configuration.GetValue<string>("Authentication:Authority");
                options.Audience = "Authentication:Audience";
            });

            services.AddAuthorization(options => _scopes.ForEach(x => options.AddPolicy(x, p => p.RequireClaim("scope", x))));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gridcoin.WebApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
