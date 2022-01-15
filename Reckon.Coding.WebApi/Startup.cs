using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Polly;
using Polly.Extensions.Http;
using Reckon.Coding.Services.FindOccurrences;
using Reckon.Coding.Services.Helpers;
using Reckon.Coding.Services.ResultsConsumer;
using Reckon.Coding.Services.SubText;
using Reckon.Coding.Services.TextToSearch;
using System;
using System.Net.Http;

namespace Reckon.Coding.WebApi {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {

            services.AddControllers();
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Reckon.Coding.WebApi", Version = "v1" });
            });

            RegisterDependencies(services);
            RegisterHttpClients(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Reckon.Coding.WebApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }

        #region non-public
        private void RegisterDependencies(IServiceCollection services) {
            services.AddScoped<ITextToSearchService, TextToSearchService>();
            services.AddScoped<ISubTextService, SubTextService>();
            services.AddScoped<IResultsConsumerService, ResultsConsumerService>();
            services.AddScoped<IFindOccurrencesService, FindOccurrencesService>();
            
            services.AddScoped<AppConfig, AppConfig>();
        }

        private void RegisterHttpClients(IServiceCollection services) {
            var appConfig = services.BuildServiceProvider().GetRequiredService<AppConfig>();
            var retryPolicy = GetRetryPolicy(appConfig);

            services.AddHttpClient<ITextToSearchService, TextToSearchService>(client => {
                client.BaseAddress = new Uri(appConfig.TextToSearchUri);
            }).AddPolicyHandler(retryPolicy);

            services.AddHttpClient<ISubTextService, SubTextService>(client => {
                client.BaseAddress = new Uri(appConfig.SubTextUri);
            }).AddPolicyHandler(retryPolicy);

            services.AddHttpClient<IResultsConsumerService, ResultsConsumerService>(client => {
                client.BaseAddress = new Uri(appConfig.ResulsConsumerUri);
            }).AddPolicyHandler(retryPolicy);
        }

        private IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(AppConfig appConfig) {
            return HttpPolicyExtensions.HandleTransientHttpError()
                                       .WaitAndRetryAsync(appConfig.RetryPolicy.RetryCount, 
                                                          retryAttempt => TimeSpan.FromMilliseconds(appConfig.RetryPolicy.RetryWaitMs));
        }
        #endregion non-public
    }
}
