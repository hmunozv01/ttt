using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inventario_des.Models
{
    public class oDestinatario
    {
        public int? EmpresaId { get; set; }
        [Display(Name = "Razón Social")]
        public string EmpresaRazonSocial { get; set; }                 
        [Display(Name = "Rut Empresa")]
        public string EmpresaRut { get; set; }

        [Display(Name = "Proyecto")]
        public string ProyectoNombre { get; set; }
        public int? ProyectoId { get; set; }

        [Display(Name = "Comuna")]
        public string ComunaNombre { get; set; }
        public int? ComunaId { get; set; }

        [Display(Name = "Ubicación")]
        public string UbicacionNombre { get; set; }
        public int? UbicacionId { get; set; }

        [Display(Name = "Centro Costo")]
        public string CentroCostoNombre { get; set; }
        public int? CentroCostoId { get; set; }

        [Display(Name = "GerenteId")]
        public int? GerenteId { get; set; }
        [Display(Name = "Gerente")]
        public string GerenteNombre { get; set; }
        [Display(Name = "Gerente (Id Red)")]
        public string GerenteIdRed { get; set; }
        [Display(Name = "Gerente (Email)")]
        public string GerenteEmail { get; set; }

        [Display(Name = "SolicitanteId")]
        public int? SolicitanteId { get; set; }
        [Display(Name = "Solicitante")]
        public string SolicitanteNombre { get; set; }
        [Display(Name = "Solicitante (Id Red)")]
        public string SolicitanteIdRed { get; set; }
        [Display(Name = "Solicitante (Email)")]
        public string SolicitanteEmail { get; set; }

        [Display(Name = "UsuarioFinalId")]
        public int? UsuarioFinalId { get; set; }
        [Display(Name = "Usuario Final")]
        public string UsuarioFinalNombre { get; set; }
        [Display(Name = "Usuario Final (Id Red)")]
        public string UsuarioFinalIdRed { get; set; }
        [Display(Name = "Usuario Final (Email)")]
        public string UsuarioFinalEmail { get; set; }
    }
}