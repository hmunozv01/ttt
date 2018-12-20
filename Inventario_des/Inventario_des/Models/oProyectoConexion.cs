using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inventario_des.Models
{
    public class oProyectoConexion
    {
        // Proyecto o Área

        [Display(Name = "Macro Empresa Id")]
        public int? MacroEmpresaId { get; set; }
        [Display(Name = "Macro Empresa")]
        public string MacroEmpresaNombre { get; set; }

        public int? EmpresaId { get; set; }
        [Display(Name = "Razón Social")]
        public string EmpresaRazonSocial { get; set; }
        [Display(Name = "Rut Empresa")]
        public string EmpresaRut { get; set; }

        [Display(Name = "Centro Costo Id")]
        public int? CentroCostoId { get; set; }
        [Display(Name = "Centro Costo")]
        public string CentroCostoNombre { get; set; }

        [Display(Name = "Ubicación Id")]
        public int? UbicacionId { get; set; }
        [Display(Name = "Ubicación")]
        public string UbicacionNombre { get; set; }

        [Display(Name = "Proyecto")]
        public string ProyectoNombre { get; set; }
        public int? ProyectoId { get; set; }

        [Display(Name = "Comuna Id")]
        public int? ComunaId { get; set; }
        [Display(Name = "Comuna")]
        public string ComunaNombre { get; set; }

        [Display(Name = "Dirección Id")]
        public int? DireccionId { get; set; }
        [Display(Name = "Dirección")]
        public string DireccionNombre { get; set; }
    }
}