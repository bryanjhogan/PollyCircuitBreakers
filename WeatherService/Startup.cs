using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using System;
using System.Net.Http;

namespace WeatherService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var basicCircuitBreakerPolicy = Policy
                .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .CircuitBreakerAsync(2, TimeSpan.FromSeconds(30), OnBreak, OnReset, OnHalfOpen);

            var advancedCircuitBreakerPolicy = Policy
                .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .AdvancedCircuitBreakerAsync(0.25, TimeSpan.FromSeconds(60), 7, TimeSpan.FromSeconds(30),
                    OnBreak, OnReset, OnHalfOpen);

            services.AddHttpClient("TemperatureService", client =>
            {
                client.BaseAddress = new Uri("http://localhost:6001/");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            }).AddPolicyHandler(basicCircuitBreakerPolicy);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        private void OnHalfOpen()
        {
            Console.WriteLine("Circuit in test mode, one request will be allowed.");
        }

        private void OnReset()
        {
            Console.WriteLine("Circuit closed, requests flow normally.");
        }

        private void OnBreak(DelegateResult<HttpResponseMessage> result, TimeSpan ts)
        {
            Console.WriteLine("Circuit cut, requests will not flow.");
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
