using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inventario_des.Models
{
    public class oEquipoDistribuido
    {
        public oEquipo equipo { get; set; }

        public IList<oEquipo> equipos { get; set; }
    }
}