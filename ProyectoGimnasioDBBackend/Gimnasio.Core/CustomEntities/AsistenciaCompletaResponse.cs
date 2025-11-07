namespace Gimnasio.Core.CustomEntities
{
    public class AsistenciaCompletaResponse
    {
        public string Nombre { set; get; }
        public string DiaSemana { set; get; }
        public string HoraInicio { set; get; }
        public string HoraFin { set; get; }
        public string Sala { set; get; }        
        public string Descripcion { set; get; }
        public string FechaAsistencia { set; get; }
        public string Estado { set; get; }
    }
}