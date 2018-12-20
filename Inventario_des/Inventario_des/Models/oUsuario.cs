using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inventario_des.Models
{
    public class oUsuario
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string nombrecompleto { get; set; }
        public string cargo { get; set; }

        public string telefono { get; set; }
        public string direccion { get; set; }

        [Display(Name = "De:")]
        public string email { get; set; }
        public string nickname { get; set; }
        public int rolid { get; set; }
        public string rolnombre { get; set; }

        public string error { get; set; }
    }
}