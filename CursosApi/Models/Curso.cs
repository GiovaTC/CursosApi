﻿namespace CursosApi.Models
{
    public class Curso
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public List<Inscripcion> Inscripciones { get; set; }
    }
}
