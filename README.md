# CursosApi

# API REST en Visual Studio 2022 conectada a MySQL y Frontend en React

Vamos a dividir el proceso en dos partes:

---

## 🧩 Parte 1: API REST en .NET con MySQL

### 🛠️ Requisitos

- Visual Studio 2022  
- .NET 6 o superior  
- MySQL Server  
- MySQL Connector/NET  
- Paquetes NuGet:  
  - `MySql.Data` (si usas ADO.NET directamente)  
  - `Pomelo.EntityFrameworkCore.MySql` (si usas EF Core)

### 📁 Estructura de ejemplo (3 tablas)

Imaginemos un sistema de gestión de cursos:

- **Cursos** (Id, Nombre, Descripcion)  
- **Estudiantes** (Id, Nombre, Email)  
- **Inscripciones** (Id, CursoId, EstudianteId, FechaInscripcion)

---

### 1️⃣ Crear el proyecto

1. Abre Visual Studio 2022 > Crear nuevo proyecto.  
2. Selecciona **ASP.NET Core Web API**.  
3. Nómbralo `CursosApi`.  
4. Desmarca “Enable OpenAPI support” si no lo necesitas.  
5. Asegúrate de seleccionar .NET 6 o superior.

---

### 2️⃣ Instalar los paquetes necesarios

Ve a:

Herramientas > Administrador de paquetes NuGet > Consola del Administrador de paquetes
mathematica
Y ejecuta:

```bash
Install-Package Pomelo.EntityFrameworkCore.MySql
Install-Package Microsoft.EntityFrameworkCore.Design

3️⃣ Crear el modelo de datos y contexto
📄 Models/Curso.cs
csharp

public class Curso
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public List<Inscripcion> Inscripciones { get; set; }
}

📄 Models/Estudiante.cs
csharp

public class Estudiante
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Email { get; set; }
    public List<Inscripcion> Inscripciones { get; set; }
}

📄 Models/Inscripcion.cs
csharp

public class Inscripcion
{
    public int Id { get; set; }

    public int CursoId { get; set; }
    public Curso Curso { get; set; }

    public int EstudianteId { get; set; }
    public Estudiante Estudiante { get; set; }

    public DateTime FechaInscripcion { get; set; }
}

📄 Data/AppDbContext.cs
csharp

using Microsoft.EntityFrameworkCore;
using CursosApi.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<Curso> Cursos { get; set; }
    public DbSet<Estudiante> Estudiantes { get; set; }
    public DbSet<Inscripcion> Inscripciones { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Inscripcion>()
            .HasOne(i => i.Curso)
            .WithMany(c => c.Inscripciones)
            .HasForeignKey(i => i.CursoId);

        modelBuilder.Entity<Inscripcion>()
            .HasOne(i => i.Estudiante)
            .WithMany(e => e.Inscripciones)
            .HasForeignKey(i => i.EstudianteId);
    }
}

4️⃣ Configurar la cadena de conexión en appsettings.json
json

"ConnectionStrings": {
  "MySqlConnection": "server=localhost;database=cursosdb;user=root;password=tu_password"
}

5️⃣ Agregar el contexto en Program.cs
csharp

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("MySqlConnection"),
        new MySqlServerVersion(new Version(8, 0, 30))));

6️⃣ Crear controladores REST
📄 Controllers/CursosController.cs
csharp

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

Repite esto para EstudiantesController e InscripcionesController.

🎨 Parte 2: Frontend en React
🛠️ Requisitos
Node.js

Visual Studio Code (u otro editor)

Axios

1️⃣ Crear el proyecto React
bash

npx create-react-app frontend-cursos
cd frontend-cursos
npm install axios

2️⃣ Consumir la API desde React
📄 src/api.js
javascript

import axios from 'axios';

const API = axios.create({
  baseURL: 'https://localhost:5001/api'
});

export const getCursos = () => API.get('/cursos');
export const addCurso = (curso) => API.post('/cursos', curso);

📄 src/components/Cursos.js
jsx

import React, { useEffect, useState } from 'react';
import { getCursos, addCurso } from '../api';

export default function Cursos() {
  const [cursos, setCursos] = useState([]);
  const [nombre, setNombre] = useState('');
  const [descripcion, setDescripcion] = useState('');

  useEffect(() => {
    getCursos().then(res => setCursos(res.data));
  }, []);

  const handleSubmit = (e) => {
    e.preventDefault();
    addCurso({ nombre, descripcion }).then(() => {
      setNombre('');
      setDescripcion('');
      getCursos().then(res => setCursos(res.data));
    });
  };

  return (
    <div>
      <h2>Cursos</h2>
      <form onSubmit={handleSubmit}>
        <input value={nombre} onChange={e => setNombre(e.target.value)} placeholder="Nombre" />
        <input value={descripcion} onChange={e => setDescripcion(e.target.value)} placeholder="Descripción" />
        <button type="submit">Agregar</button>
      </form>
      <ul>
        {cursos.map(c => <li key={c.id}>{c.nombre} - {c.descripcion}</li>)}
      </ul>
    </div>
  );
}

📄 src/App.js
jsx

import React from 'react';
import Cursos from './components/Cursos';

function App() {
  return (
    <div className="App">
      <h1>Gestión de Cursos</h1>
      <Cursos />
    </div>
  );
}

export default App;

✅ Últimos pasos
Ejecutar la API en Visual Studio 2022
Asegúrate de que CORS esté habilitado para permitir peticiones desde React:

csharp

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});
app.UseCors();

Ejecutar la app de React
bash
npm start
css

