namespace CursosApi.Models
{
    public class Estudiante
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public List<Inscripcion> Inscripciones { get; set; }
    }
}
