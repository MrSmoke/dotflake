namespace DotFlake
{
    using System;
    using System.Linq;
    using Generators;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Sources.MachineId;

    public class Startup
    {
        public Startup()
        {
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddJsonFile("config.json");
            Configuration = configBuilder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IIdGeneratorFactory, IdGeneratorFactory>();
            services.AddSingleton<ISystemClock, SystemClock>();

            services.AddSingleton(p => GetMachineIdSource(Configuration.GetSection("machineIdSource")));

            GeneratorsConfig.RegisterAll();

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

            RegisterGenerators(app.ApplicationServices, Configuration);

            app.UseMvc();
        }

        private static IMachineIdSource GetMachineIdSource(IConfiguration configuration)
        {
            var type = configuration.GetValue<string>("type");

            switch (type)
            {
                case "static":
                    return new StaticMachineSource(configuration.GetValue<long>("options:machineId"));
                default:
                    throw new Exception("Unknown machine id source");
            }
        }

        private static void RegisterGenerators(IServiceProvider services, IConfiguration config)
        {
            var generatorFactory = services.GetRequiredService<IIdGeneratorFactory>();

            var generatorsConfig = config.GetSection("generators").GetChildren();

            generatorFactory.CreateInstances(generatorsConfig);
        }
    }
}
