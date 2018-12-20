using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inventario_des.Models
{
    public class oGuia
    {
        [Display(Name = "Id")]
        public int GuiaId { get; set; }

        [Display(Name = "Nro. Guía")]
        public int? NumeroGuia { get; set; }

        [Display(Name = "Factura")]
        public int? NumeroFactura { get; set; }

        [Display(Name = "Orden de Compra")]
        public string OrdenCompra { get; set; }

        [Display(Name = "Nota de Venta")]
        public string NotaVenta { get; set; }

        [Display(Name = "Proveedor Id")]
        public int? ProveedorId { get; set; }
        [Display(Name = "Proveedor")]
        public string ProveedorNombre { get; set; }

        [Display(Name = "Fecha Guía")]
        [DisplayFormat(DataFormatString = "{0:dd\\/MM\\/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FechaGuia { get; set; }
        
        [Display(Name = "Fecha Creación")]
        [DisplayFormat(DataFormatString = "{0:dd\\/MM\\/yyyy HH\\:mm}", ApplyFormatInEditMode = true)]
        public DateTime? FechaCreacion { get; set; }

        [Display(Name = "UsuarioId")]
        public int? CreadorId { get; set; }
        [Display(Name = "Usuario")]
        public string CreadorNombre { get; set; }
        [Display(Name = "Usuario (Id Red)")]
        public string CreadorIdRed { get; set; }
        [Display(Name = "Usuario (Email)")]
        public string CreadorEmail { get; set; }

        public IList<oEquipo> equipos { get; set; }
        public IList<oDocumento> documentos { get; set; }
    }
}