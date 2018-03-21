namespace DotFlake
{
    using System;
    using Generators;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Timing;

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
            var generatorFactory = new IdGeneratorFactory();

            services.AddSingleton<IIdGeneratorFactory>(generatorFactory);
            services.AddSingleton<ISystemClock, SystemClock>();

            services
                .AddMvcCore()
                .AddJsonFormatters();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            RegisterGenerators(app.ApplicationServices);

            app.UseMvc();
        }

        private void RegisterGenerators(IServiceProvider services)
        {
            var generatorFactory = services.GetRequiredService<IIdGeneratorFactory>();
            var systemClock = services.GetRequiredService<ISystemClock>();

            var stopwatchTimeSource = new StopwatchTimeSource(systemClock, new StopwatchTimeSourceOptions());
            var flakeGenerator = new FlakeGenerator(stopwatchTimeSource, new FlakeGeneratorOptions());

            generatorFactory.AddGenerator("flake", flakeGenerator);
        }
    }
}
