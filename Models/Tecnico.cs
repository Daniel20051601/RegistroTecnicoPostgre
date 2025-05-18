using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace RegistroTecnicoPostgre.Models
{
    public class Tecnico: DbContext
    {
        [Key]
        public int TecnicoId { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        public string Name { get; set; }

        [Range(1, 200000, ErrorMessage = "El salario debe estar entre 1 y 200000")]
        public double sueldo { get; set; }

    }
}
