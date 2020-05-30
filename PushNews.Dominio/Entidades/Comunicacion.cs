using PushNews.Dominio.Enums;
using System;
using System.Collections.Generic;
namespace PushNews.Dominio.Entidades
{
    public class Comunicacion
    {
        public Comunicacion()
        {
            Accesos = new List<ComunicacionAcceso>(0);
        }

        public long ComunicacionID { get; set; }
        public long UsuarioID { get; set; }
        public long CategoriaID { get; set; }
        public bool Destacado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaPublicacion { get; set; }

        // Evitar el 0 porque las apps solicitan timestamps mayores que un dato.
        // El filtro inicial es timestamp > 0.
        public long TimeStamp { get; set; } = 1;

        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string Autor { get; set; }
        public string ImagenTitulo { get; set; }
        public long? ImagenDocumentoID { get; set; }
        public string AdjuntoTitulo { get; set; }
        public long? AdjuntoDocumentoID { get; set; }
        public string EnlaceTitulo { get; set; }
        public string Enlace { get; set; }
        public string YoutubeTitulo { get; set; }
        public string Youtube { get; set; }
        public string GeoPosicionTitulo { get; set; }
        public float? GeoPosicionLatitud { get; set; }
        public float? GeoPosicionLongitud { get; set; }
        public string GeoPosicionDireccion { get; set; }
        public string GeoPosicionLocalidad { get; set; }
        public string GeoPosicionProvincia { get; set; }
        public string GeoPosicionPais { get; set; }

        public string RecordatorioTitulo { get; set; }
        public DateTime? RecordatorioFecha { get; set; }
        public DateTime? PushRecordatorio { get; set; }

        public string UltimaEdicionIP { get; set; }
        public bool Activo { get; set; }
        public bool Instantanea { get; set; }
        public bool Borrado { get; set; }
        public DateTime? FechaBorrado { get; set; }

        public bool PushEnviada { get; set; }
        public DateTime? PushFecha { get; set; }

        public EstadosPublicacion EstadoPublicacion(double horasEnvio)
        {
            if(this.FechaPublicacion > DateTime.Now)
            {
                // Fecha de publicación no alcanzada
                return EstadosPublicacion.Planificado;
            }
            else if(!this.PushEnviada)
            {
                if (DateTime.Now.AddHours(-1 * horasEnvio) > this.FechaPublicacion)
                {
                    // La fecha de publicación se ha alcanzado y se ha superado el periodo de envío de push
                    return EstadosPublicacion.Caducado;
                }
                else
                {
                    // Fecha de publicación alcanzada y push no enviado,
                    // pero en periodo de reintento del envío
                    return EstadosPublicacion.Atencion;
                }
            }
            else if(!this.RecordatorioFecha.HasValue)
            {
                // Push enviado y no hay recordatorio programado.
                return EstadosPublicacion.Enviado;
            }
            else if (this.RecordatorioFecha > DateTime.Now)
            {
                // Push enviado y fecha recordatorio no alcanzada.
                return EstadosPublicacion.Recordar;
            }
            else if (this.PushRecordatorio.HasValue)
            {
                // Recordatorio enviado
                return EstadosPublicacion.Enviado;
            }
            else if (DateTime.Now.AddHours(-1 * horasEnvio) > this.RecordatorioFecha)
            {
                // La fecha de recordatorio se ha alcanzado y se ha superado el periodo de envío de push
                return EstadosPublicacion.Caducado;
            }
            else
            {
                // Fecha de publicación alcanzada y push no enviado,
                // pero en periodo de reintento del envío
                // Fecha de recordatorio alcanzada y push recordatorio no enviado.
                return EstadosPublicacion.Atencion;
            }
        }

        public virtual Categoria Categoria { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual Documento Adjunto { get; set; }
        public virtual Documento Imagen { get; set; }
        public virtual ICollection<ComunicacionAcceso> Accesos { get; set; }
    }
}