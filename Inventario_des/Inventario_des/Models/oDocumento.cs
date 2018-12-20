using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inventario_des.Models
{
    public class oDocumento
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "Guía Id")]
        public int? GuiaId { get; set; }
        [Display(Name = "Nro. Guía")]
        public string NumeroGuia { get; set; }

        [Display(Name = "Usuario")]
        public string DocumentoUsuario { get; set; }
        [Display(Name = "Tipo")]
        public string DocumentoTipo { get; set; }
        [Display(Name = "Fecha")]
        public DateTime DocumentoFecha { get; set; }
        [Display(Name = "Descripción")]
        public string DocumentoDescripcion { get; set; }

        [Display(Name = "Nombre")]
        public string DocumentoNombre { get; set; }

    }
}