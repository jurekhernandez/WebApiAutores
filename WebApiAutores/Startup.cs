using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

using System.Text.Json.Serialization;

using WebApiAutores.Filtros;
using WebApiAutores.Middlewares;

namespace WebApiAutores
{
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // si un servicio es requerido de forma constante podemos iniciarlo aca, para usarlo de forma más rapida
        // ejemplo el AddDbContext queda listo para ser usado por los controladores,
        // cada controlador lo usa, pero no necesitan indicar configuraciones adicionales
        public void ConfigureServices(IServiceCollection services) {

            services.AddControllers(opciones => {
                opciones.Filters.Add(typeof(FiltroDeExcepcion));
            }).AddJsonOptions(x=> x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
            
            services.AddDbContext<AplicationDbContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("defaultConnection"))
            );
        
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            
            services.AddSwaggerGen();

            services.AddAutoMapper(typeof(Startup));


        }


        // aca podemos configurar los middlewares,   el nombre contiene Use
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger) {

           
            app.UseLoguearRespuestaHTTP();

            if (env.IsDevelopment()){
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
