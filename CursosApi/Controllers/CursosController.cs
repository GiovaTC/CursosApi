using CursosApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CursosApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CursosController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CursosController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> GetCursos() =>
            Ok(await _context.Cursos.ToListAsync());

        [HttpPost]
        public async Task<IActionResult> AddCurso(Curso curso)
        {
            _context.Cursos.Add(curso);
            await _context.SaveChangesAsync();
            return Ok(curso);
        }
    }
}
