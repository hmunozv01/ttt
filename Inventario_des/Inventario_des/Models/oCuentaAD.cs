using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inventario_des.Models
{
    public class oCuentaAD
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string nombrecompleto { get; set; }
        public string cargo { get; set; }

        [Display(Name = "De:")]
        public string email { get; set; }
        public string nickname { get; set; }
        public string rol { get; set; }

        public int? empresaid { get; set; }
        public string ubicacion { get; set; }
        public string telefono { get; set; }
        public string direccion { get; set; }

        public string error { get; set; }
    }
}