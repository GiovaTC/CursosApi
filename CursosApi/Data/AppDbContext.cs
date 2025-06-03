using Microsoft.EntityFrameworkCore;
using CursosApi.Models;

namespace CursosApi.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Estudiante> Estudiantes { get; set; }
        public DbSet<Inscripcion> Inscripciones { get; set; }




    }
}
