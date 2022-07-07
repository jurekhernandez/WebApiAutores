﻿namespace WebApiAutores.Servicios {
    public class EscribirEnArchivo: IHostedService {
        private readonly IWebHostEnvironment env;
        private readonly string nombreArchivo = "archivo_1.txt";
        private Timer timer;

        public EscribirEnArchivo(IWebHostEnvironment env) {
            this.env = env;
        }

        public Task StartAsync(CancellationToken cancellationToken) {
            timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            Escribir("Proceso inicado");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) {
            timer.Dispose();
            Escribir("Proceso finalizado");
            return Task.CompletedTask;
        }

        private void Escribir(string mensaje) {
            var ruta = $@"{env.ContentRootPath}\wwwroot\{nombreArchivo}";
            using(StreamWriter writer = new StreamWriter(ruta, append:true)){ 
                writer.WriteLine(mensaje);
            }
        }

        private void DoWork(object state) {
            Escribir("Proceso en ejecución:"+DateTime.Now.ToString("dd/MM/yyy hh:mm:ss"));
        }
    }
}
