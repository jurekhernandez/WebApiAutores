﻿namespace WebApiAutores.Servicios
{
    public interface IServicio {
        Guid ObtenerTransient();
        Guid ObtenerScoped();
        Guid ObtenerSingleton();
        void RealizarTarea();
    }


    public class ServicioA : IServicio {
        private readonly ILogger<ServicioA> logger;
        private readonly ServicioTransient servicioTransient;
        private readonly ServicioScoped servicioScoped;
        private readonly ServicioSingleton servicioSingleton;
        public ServicioA(ILogger<ServicioA> logger, 
            ServicioTransient servicioTransient,
            ServicioScoped servicioScoped, 
            ServicioSingleton servicioSingleton
            ) {

            this.logger= logger;
            this.servicioTransient = servicioTransient;
            this.servicioScoped = servicioScoped;
            this.servicioSingleton = servicioSingleton;
        }

        public Guid ObtenerTransient() => servicioTransient.Guid;
        public Guid ObtenerScoped() => servicioScoped.Guid; 
        public Guid ObtenerSingleton() =>servicioSingleton.Guid; 



        public void RealizarTarea() {
            throw new NotImplementedException();
        }
    }

    public class ServicioB : IServicio {
        public Guid ObtenerScoped()
        {
            throw new NotImplementedException();
        }

        public Guid ObtenerSingleton()
        {
            throw new NotImplementedException();
        }

        public Guid ObtenerTransient()
        {
            throw new NotImplementedException();
        }

        public void RealizarTarea() => throw new NotImplementedException();
    }

    public class ServicioTransient{
        public Guid Guid = Guid.NewGuid();
    }
    public class ServicioScoped {
        public Guid Guid = Guid.NewGuid();
    }
    public class ServicioSingleton{
        public Guid Guid = Guid.NewGuid();
    }
}
