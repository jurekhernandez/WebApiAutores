using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

using WebApiAutores.Filtros;
using WebApiAutores.Middlewares;
using WebApiAutores.Servicios;

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

            services.AddTransient<IServicio, ServicioA>();

            // services.AddTransient<ServicioB>();

            services.AddTransient<ServicioTransient>();
            services.AddScoped<ServicioScoped>();
            services.AddSingleton<ServicioSingleton>();
            services.AddTransient<MiFiltroDeAccion>();
            services.AddHostedService<EscribirEnArchivo>();


            // esto nos permite manejar respuestas en cache , no olvidar agregar   app.UseResponseCaching(); en Configure
            services.AddResponseCaching(); 

            //
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            
            services.AddSwaggerGen();
        }


        // aca podemos configurar los middlewares,   el nombre contiene Use
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger) {

            /*
             * intercepta todas las rutas  y envia un texto
            app.Run(async contexto => {
                await contexto.Response.WriteAsync("Interceptado");
            });
            */

            // Esto nos permite crear una bifurcación  ante una solo ruta especifica
            /*app.Map("/ruta1", app => {
                app.Run(async contexto => {
                    await contexto.Response.WriteAsync(" Solo una ruta");
                });
            });*/


            // eser servicio registra todas las  respuesta enviadas al usuario


            // app.UseMiddleware<LoguearRespuestaHTTPMiddleware>();
            app.UseLoguearRespuestaHTTP();


            if (env.IsDevelopment()){
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            // esto nos permite manejar respuestas en cache , no olvidar agregar   services.AddResponseCaching();  en ConfigureServices
            app.UseResponseCaching();

            app.UseAuthorization();

           // app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
