using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inventario_des.Models
{
    public class oOtro
    {



        [Display(Name = "Guia Id")]
        public int GuiaId { get; set; }
        [Display(Name = "Guía")]
        public string GuiaNombre { get; set; }





        [Display(Name = "Técnico Id")]
        public int? TecnicoHabilitadorId { get; set; }
        [Display(Name = "Técnico")]
        public string TecnicoHabilitadorNombre { get; set; }
        [Display(Name = "Técnico (Email)")]
        public string TecnicoHabilitadorEmail { get; set; }
        [Display(Name = "Técnico (Id Red)")]
        public string TecnicoHabilitadorIdRed { get; set; }
        public string TecnicoHabilitadorCargo { get; set; }

        [Display(Name = "Técnico Id")]
        public int? TecnicoInstaladorId { get; set; }
        [Display(Name = "Técnico")]
        public string TecnicoInstaladorNombre { get; set; }
        [Display(Name = "Técnico (Email)")]
        public string TecnicoInstaladorEmail { get; set; }
        [Display(Name = "Técnico (Id Red)")]
        public string TecnicoInstaladorIdRed { get; set; }
        public string TecnicoInstaladorCargo { get; set; }

        [Display(Name = "Razón Social Id")]
        public int? RazonSocialId { get; set; }
        [Display(Name = "Razón Social")]
        public string RazonSocialNombre { get; set; }

        [Display(Name = "Centro de Costo")]
        public string CentroCostoNombre { get; set; }

        [Display(Name = "Gerente Autoriza Id")]
        public int? GerenteId { get; set; }
        [Display(Name = "Gerente Autoriza")]
        public string GerenteNombre { get; set; }

        [Display(Name = "Solicitante Id")]
        public int? SolicitanteId { get; set; }
        [Display(Name = "Solicitante")]
        public string SolicitanteNombre { get; set; }

        [Display(Name = "Usuario Final Id")]
        public int? UsuarioFinalId { get; set; }
        [Display(Name = "Usuario Final")]
        public string UsuarioFinalNombre { get; set; }
        [Display(Name = "Rut")]
        public string UsuarioFinalRut { get; set; }
        [Display(Name = "Cargo")]
        public string UsuarioFinalCargo { get; set; }

        [Display(Name = "Instalación Id")]
        public int? InstalacionId { get; set; }
        [Display(Name = "Instalación")]
        public string InstalacionNombre { get; set; }

        [Display(Name = "Proyecto Id")]
        public int? ProyectoId { get; set; }
        [Display(Name = "Proyecto")]
        public string ProyectoNombre { get; set; }
        [Display(Name = "Comuna")]
        public string ComunaNombre { get; set; }


    }
}