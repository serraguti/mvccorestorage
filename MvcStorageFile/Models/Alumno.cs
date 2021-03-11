using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcStorage.Models
{
    public class Alumno
    {
        public String IdAlumno { get; set; }
        public String Nombre { get; set; }
        public String Apellidos { get; set; }
        public String Curso { get; set; }
        public String Nota { get; set; }
    }
}
