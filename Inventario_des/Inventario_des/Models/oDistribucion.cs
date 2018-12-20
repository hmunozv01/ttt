using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inventario_des.Models
{
    public class oDistribucion
    {
        public int id { get; set; }
        public string email { get; set; }
        public string nombre { get; set; }
        public string rut { get; set; }
        public string tipo { get; set; }
        public string error { get; set; }

    }
}