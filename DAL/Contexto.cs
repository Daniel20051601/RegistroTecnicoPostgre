using Microsoft.EntityFrameworkCore;
using RegistroTecnicoPostgre.Models;

namespace RegistroTecnicoPostgre.DAL
{
    public class Contexto: DbContext
    {
        public Contexto(DbContextOptions<Contexto> options) : base(options){}

        public DbSet<Tecnico> Tecnicos { get; set; } = null!;
    }
}
