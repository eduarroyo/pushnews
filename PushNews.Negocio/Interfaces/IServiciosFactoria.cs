namespace PushNews.Negocio.Interfaces
{
    public interface IServiciosFactoria
    {
        IAplicacionesServicio AplicacionesServicio();
        IParametrosServicio ParametrosServicio();
        IUsuariosServicio UsuariosServicio();
        ICategoriasServicio CategoriasServicio();
        IComunicacionesServicio ComunicacionesServicio();
        IComunicacionesAccesosServicio ComunicacionesAccesosServicio();
        ITerminalesServicio TerminalesServicio();
        IDocumentosServicio DocumentosServicio();
        ITelefonosServicio TelefonosServicio();
        ILocalizacionesServicio LocalizacionesServicio();
        IAplicacionesCaracteristicasServicio AplicacionesCaracteristicasServicio();
        IEmpresasServicio EmpresasServicio();
        IRolesServicio RolesServicio();

        // Cuando se crea un nuevo servicio tenemos que declarar un método aquí para obtenerlo e
        // escribirlo en las implementaciones de esta interfaz.
    }
}