﻿using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

using WebApiAutores.Servicios;

namespace WebApiAutores
{
    public class Startup{
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) {

            services.AddControllers().AddJsonOptions(x=> x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
            
            services.AddDbContext<AplicationDbContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("defaultConnection"))
            );

            services.AddTransient<IServicio, ServicioA>();

            // services.AddTransient<ServicioB>();

            services.AddTransient<ServicioTransient>();
            services.AddScoped<ServicioScoped>();
            services.AddSingleton<ServicioSingleton>();
            
            

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            
            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting(); 

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
