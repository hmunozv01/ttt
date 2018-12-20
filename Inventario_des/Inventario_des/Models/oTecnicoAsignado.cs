using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inventario_des.Models
{
    public class oTecnicoAsignado
    {
        [Display(Name = "TecnicoId")]
        public int? TecnicoId { get; set; }
        [Display(Name = "Técnico")]
        public string TecnicoNombre { get; set; }
        [Display(Name = "Tecnico (Id Red)")]
        public string TecnicoIdRed { get; set; }
        [Display(Name = "Tecnico (Email)")]
        public string TecnicoEmail { get; set; }

        public IList<oEquipo> equipos { get; set; }
    }
}