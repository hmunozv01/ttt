using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inventario_des.Models
{
    public class oEquipo
    {
        [Display(Name = "Id")]
        public int EquipoId { get; set; }


        [Display(Name = "Guía Id")]
        public int? GuiaId { get; set; }
        [Display(Name = "Nro. Guía")]
        public string NumeroGuia { get; set; }

        [Display(Name = "Orden de Compra")]
        public string OrdenCompra { get; set; }

        [Display(Name = "Proveedor Id")]
        public int? ProveedorId { get; set; }
        [Display(Name = "Proveedor")]
        public string ProveedorNombre { get; set; }

        [Display(Name = "Fecha Guía")]
        [DisplayFormat(DataFormatString = "{0:dd\\/MM\\/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FechaGuia { get; set; }


        [Display(Name = "Tipo Id")]
        public int? TipoId { get; set; }
        [Display(Name = "Tipo")]
        public string TipoNombre { get; set; }

        [Display(Name = "Marca Id")]
        public int? MarcaId { get; set; }
        [Display(Name = "Marca")]
        public string MarcaNombre { get; set; }

        [Display(Name = "Modelo")]
        public string ModeloNombre { get; set; }

        [Display(Name = "Nro. Serie")]
        public string NumeroSerie { get; set; }

        public bool Bolso { get; set; }
        public bool Mouse { get; set; }
        public bool Candado { get; set; }

        [Display(Name = "Comentario")]
        public string Comentario { get; set; }

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

        [Display(Name = "Estado Id")]
        public int? EstadoId { get; set; }
        [Display(Name = "Estado")]
        public string EstadoNombre { get; set; }

        [Display(Name = "Sub Estado Id")]
        public int? SubEstadoId { get; set; }
        [Display(Name = "Sub Estado")]
        public string SubEstadoNombre { get; set; }

        [Display(Name = "TecnicoPreId")]
        public int? TecnicoPreId { get; set; }
        [Display(Name = "Técnico Pre.")]
        public string TecnicoPreNombre { get; set; }
        [Display(Name = "Técnico Pre. (Id Red)")]
        public string TecnicoPreIdRed { get; set; }
        [Display(Name = "Técnico Pre. (Email)")]
        public string TecnicoPreEmail { get; set; }

        [Display(Name = "No requiere técnico")]
        public bool SinTecnicoPreId { get; set; }

       
        [Display(Name = "Actualizar")]
        public bool actualizar { get; set; }


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


        // Usuarios (destinatario)

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


        // Hardware

        [Display(Name = "Procesador")]
        public string Procesador { get; set; }

        [Display(Name = "Memoria")]
        public string Memoria { get; set; }

        [Display(Name = "Disco Duro")]
        public string DiscoDuro { get; set; }

        [Display(Name = "Dirección MAC")]
        public string DireccionMAC { get; set; }

        [Display(Name = "Dirección IP")]
        public string DireccionIP { get; set; }

        [Display(Name = "Nombre PC")]
        public string NombrePC { get; set; }

        [Display(Name = "Sistema Operativo Id")]
        public int? SistemaOperativoId { get; set; }
        [Display(Name = "Sistema Operativo")]
        public string SistemaOperativoNombre { get; set; }

        [Display(Name = "Key de Windows")]
        public string KeyWindows { get; set; }

        [Display(Name = "Listo para Entrega")]
        public bool ListoParaEntrega { get; set; }


        // Distribucion

        [Display(Name = "DistribucionId")]
        public int? DistribucionId { get; set; }
        [Display(Name = "Distribucion")]
        public string Distribucion { get; set; }
        [Display(Name = "Fecha Distribución")]
        [DisplayFormat(DataFormatString = "{0:dd\\/MM\\/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FechaDistribucion { get; set; }

        // Entrega

        [Display(Name = "TecnicoEntId")]
        public int? TecnicoEntId { get; set; }
        [Display(Name = "Técnico Ent.")]
        public string TecnicoEntNombre { get; set; }
        [Display(Name = "Técnico Ent. (Id Red)")]
        public string TecnicoEntIdRed { get; set; }
        [Display(Name = "Técnico Ent. (Email)")]
        public string TecnicoEntEmail { get; set; }

        [Display(Name = "Fecha Entrega")]
        //[DisplayFormat(DataFormatString = "{0:dd\\/MM\\/yyyy HH\\:mm}", ApplyFormatInEditMode = true)]
        [DisplayFormat(DataFormatString = "{0:dd\\/MM\\/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FechaEntrega { get; set; }

        [Display(Name = "Quién Recibe")]
        public string QuienRecibeNombre { get; set; }
        [Display(Name = "Quién Recibe (Email)")]
        public string QuienRecibeEmail { get; set; }

        [Display(Name = "No requiere técnico")]
        public bool SinTecnicoEntId { get; set; }

        [Display(Name = "Equipo Entregado")]
        public bool EquipoEntregado { get; set; }


        // Retiro

        [Display(Name = "Fecha Retira")]
        //[DisplayFormat(DataFormatString = "{0:dd\\/MM\\/yyyy HH\\:mm}", ApplyFormatInEditMode = true)]
        [DisplayFormat(DataFormatString = "{0:dd\\/MM\\/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FechaRetira { get; set; }

        [Display(Name = "Quién Retira (Nombre)")]
        public string QuienRetiraNombre { get; set; }
        [Display(Name = "Quién Retira (Email)")]
        public string QuienRetiraEmail { get; set; }
        [Display(Name = "Quién Retira (Rut)")]
        public string QuienRetiraRut { get; set; }


        [Display(Name = "Equipo Retirado")]
        public bool EquipoRetirado { get; set; }

    }
}