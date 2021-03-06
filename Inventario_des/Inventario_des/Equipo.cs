//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Inventario_des
{
    using System;
    using System.Collections.Generic;
    
    public partial class Equipo
    {
        public int EquipoId { get; set; }
        public Nullable<int> TipoId { get; set; }
        public Nullable<int> MarcaId { get; set; }
        public string Modelo { get; set; }
        public string Serie { get; set; }
        public string Comentario { get; set; }
        public Nullable<int> GuiaId { get; set; }
        public Nullable<System.DateTime> FechaCreacion { get; set; }
        public Nullable<int> CreadorId { get; set; }
        public Nullable<int> EstadoId { get; set; }
        public Nullable<int> SubEstadoId { get; set; }
        public Nullable<int> TecnicoPreId { get; set; }
        public Nullable<int> Bolso { get; set; }
        public Nullable<int> Mouse { get; set; }
        public Nullable<int> Candado { get; set; }
        public Nullable<int> EmpresaId { get; set; }
        public Nullable<int> MacroEmpresaId { get; set; }
        public Nullable<int> ProyectoId { get; set; }
        public Nullable<int> ComunaId { get; set; }
        public Nullable<int> UbicacionId { get; set; }
        public Nullable<int> CentroCostoId { get; set; }
        public Nullable<int> GerenteAutorizaId { get; set; }
        public Nullable<int> SolicitanteId { get; set; }
        public Nullable<int> UsuarioFinalId { get; set; }
        public Nullable<int> DireccionId { get; set; }
        public string Procesador { get; set; }
        public string Memoria { get; set; }
        public string DiscoDuro { get; set; }
        public string DireccionMAC { get; set; }
        public string DireccionIP { get; set; }
        public string NombrePC { get; set; }
        public Nullable<int> SistemaOperativoId { get; set; }
        public string KeyWindows { get; set; }
        public Nullable<int> TecnicoEntId { get; set; }
        public Nullable<int> DistribucionId { get; set; }
        public Nullable<System.DateTime> FechaDistribucion { get; set; }
        public string EmailDistribucion { get; set; }
    
        public virtual CentroCosto CentroCosto { get; set; }
        public virtual Colaborador Colaborador { get; set; }
        public virtual Colaborador Colaborador1 { get; set; }
        public virtual Colaborador Colaborador2 { get; set; }
        public virtual Comuna Comuna { get; set; }
        public virtual Direccion Direccion { get; set; }
        public virtual Distribucion Distribucion { get; set; }
        public virtual Empresa Empresa { get; set; }
        public virtual EstadoEquipo EstadoEquipo { get; set; }
        public virtual Guia Guia { get; set; }
        public virtual MacroEmpresa MacroEmpresa { get; set; }
        public virtual Marca Marca { get; set; }
        public virtual Proyecto Proyecto { get; set; }
        public virtual SistemaOperativo SistemaOperativo { get; set; }
        public virtual SubEstadoEquipo SubEstadoEquipo { get; set; }
        public virtual TipoEquipo TipoEquipo { get; set; }
        public virtual Ubicacion Ubicacion { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual Usuario Usuario1 { get; set; }
        public virtual Usuario Usuario2 { get; set; }
    }
}
