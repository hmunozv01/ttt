using Inventario_des.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Web.Security;
using System.Security.Principal;
using System.Globalization;

using System.Windows;


// PDF
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Configuration;
using System.Net;
using System.Diagnostics;
using System.Runtime.Serialization;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection;

namespace Inventario_des.Controllers
{
    public class ISSController : Controller
    {
        // FUNCIONES

        public static Font titleFontBlackBold = FontFactory.GetFont(FontFactory.HELVETICA, 14, Font.BOLD, BaseColor.BLACK);
        public static Font textFontBlackBold = FontFactory.GetFont(FontFactory.HELVETICA, 11, Font.BOLD, BaseColor.BLACK);
        public static Font textFontBlack = FontFactory.GetFont(FontFactory.HELVETICA, 11, BaseColor.BLACK);
        public static Font textFontRedBold = FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.BOLD, BaseColor.RED);
        public static Font titleFontBlackBoldUnderline = FontFactory.GetFont(FontFactory.HELVETICA, 14, Font.BOLD | Font.UNDERLINE, BaseColor.BLACK);

        public static Font ReSizeFont = FontFactory.GetFont(FontFactory.HELVETICA, 9);

        public static BaseColor colorcampoborrador = BaseColor.YELLOW;
        public static BaseColor colorcampoausente = BaseColor.RED;
        public static BaseColor colorcampoencabezado = BaseColor.GRAY;
        public static BaseColor colorcampotitulo = BaseColor.LIGHT_GRAY;

        

        public string formatearRut(string rut)
        {
            int cont = 0;
            string format;
            if (rut.Length == 0)
            {
                return "";
            }
            else
            {
                rut = rut.Replace(".", "");
                rut = rut.Replace("-", "");
                format = "-" + rut.Substring(rut.Length - 1);
                for (int i = rut.Length - 2; i >= 0; i--)
                {
                    format = rut.Substring(i, 1) + format;
                    cont++;
                    if (cont == 3 && i != 0)
                    {
                        format = "." + format;
                        cont = 0;
                    }
                }
                return format;
            }
        }


        public int ObtenerIdDistribucion(string email_rec, string rut, string email_ret)                   
        {
            int id_distribucion = 0;
            //string[] nn = email.ToString().Split('@');
            //string nickname = nn[0].Trim();

            using (InventarioSoporteSistemas_desEntities bd_iss = new InventarioSoporteSistemas_desEntities())
            {
                if (!string.IsNullOrEmpty(email_rec))
                {
                    // en tabla Distribucion se busca por EMAIL de quien recibe
                    Distribucion distribucion = bd_iss.Distribucion.Where(em => em.Email.Equals(email_rec) && em.Tipo.Equals("entrega")).FirstOrDefault();

                    if ((distribucion != null) && !distribucion.DistribucionId.Equals(null))
                    {
                        id_distribucion = distribucion.DistribucionId;
                    }
                    else
                    {
                    }
                }
                else if (!string.IsNullOrEmpty(rut))
                {
                    // en tabla Distribucion se busca por RUT
                    Distribucion distribucion = bd_iss.Distribucion.Where(r => r.Rut.Equals(rut) && r.Tipo.Equals("retira")).FirstOrDefault();

                    if ((distribucion != null) && !distribucion.DistribucionId.Equals(null))
                    {
                        id_distribucion = distribucion.DistribucionId;
                    }
                    else
                    {
                    }
                }
                else if (!string.IsNullOrEmpty(email_ret))
                {
                    // en tabla Distribucion se busca por EMAIL de quien retira
                    // pendiente
                }
            }

            return id_distribucion;
        }


        public Distribucion ConstruirDistribucion(oDistribucion dis_app, int accion)
        {
            Distribucion dis_bd = new Distribucion()
            {
                Nombre = dis_app.nombre,
                Email = dis_app.email,
                Rut = dis_app.rut,
                Tipo = dis_app.tipo,
                DistribucionId = accion
            };

            //if (dis_app.id > 0)
            //    dis_bd.DistribucionId = accion;

                return dis_bd;
        }


        public oDistribucion CrearModificarDistribucionEnBD(oDistribucion dis_app, int accion)
        {
            string messages_errors = string.Empty;
            oDistribucion dis_resultado = new oDistribucion();

            var errors = ModelState.Values.SelectMany(v => v.Errors);

            if (ModelState.IsValid)
            {
                Distribucion dis_bd = new Distribucion();
                dis_bd = ConstruirDistribucion(dis_app, accion);

                using (InventarioSoporteSistemas_desEntities bd_iss = new InventarioSoporteSistemas_desEntities())
                {
                    try
                    {
                        if (accion == 0)                                 // CREAR... 
                        {
                            bd_iss.Distribucion.Add(dis_bd);
                        }
                        else                                            // MODIFICAR... 
                        {
                            bd_iss.Entry(dis_bd).State = System.Data.Entity.EntityState.Modified;
                        }

                        
                        int resultado = bd_iss.SaveChanges();

                        if (resultado > 0)
                        {
                            messages_errors = "Distribución con Email " + dis_bd.Email + " o Rut " + dis_bd.Rut + " creada/modificada en Base de Datos";
                            dis_resultado.id = dis_bd.DistribucionId;
                            dis_resultado.error = messages_errors;
                        }
                        else
                        {
                            messages_errors = "No fue creada/modificada la Distribución con Email " + dis_bd.Email + " o Rut " + dis_bd.Rut;
                            dis_resultado.id = 0;
                            dis_resultado.error = messages_errors;
                        }
                    }
                    catch (DbEntityValidationException ex)
                    {
                        foreach (DbEntityValidationResult item in ex.EntityValidationErrors)
                        {
                            // Get entry

                            DbEntityEntry entry = item.Entry;
                            string entityTypeName = entry.Entity.GetType().Name;

                            // Display or log error messages

                            foreach (DbValidationError subItem in item.ValidationErrors)
                            {
                                messages_errors = messages_errors + string.Format("Error '{0}' occurrió en {1} a {2}" + "\r\n",
                                         subItem.ErrorMessage, entityTypeName, subItem.PropertyName);
                                //Console.WriteLine(message);
                            }
                        }

                        dis_resultado.id = 0;
                        dis_resultado.error = messages_errors;
                    }
                }
            }
            else
            {
                messages_errors = string.Join("; ", ModelState.Values
                                    .SelectMany(x => x.Errors)
                                    .Select(x => x.ErrorMessage));

                dis_resultado.id = 0;
                dis_resultado.error = messages_errors;
            }

            return dis_resultado;
        }



        public List<oEquipo> ObtenerEquiposPorNumeroGuia(int idguia)                             // PENDIENTE
        {
            List<oEquipo> listaEquipos = new List<oEquipo>();

            if (idguia > 0)
            {
                listaEquipos.Add(new oEquipo() { TipoNombre = "Notebook", MarcaNombre = "HP", ModeloNombre = "ProBook 440", NumeroSerie = "5CD81871KS", Comentario = "Prueba 1", Bolso = true, Candado = true, Mouse = false });
                listaEquipos.Add(new oEquipo() { TipoNombre = "Estación Acoplamiento", MarcaNombre = "HP", ModeloNombre = "US", NumeroSerie = "8CJ815B1T6", Comentario = "Prueba 2", Bolso = false, Candado = false, Mouse = true});
            }
            else
            {
            }

            //List<oEquipo> listaEquipos = new List<oEquipo>
            //{
            //    new oEquipo(){ TipoNombre = "Notebook", MarcaNombre = "HP", ModeloNombre = "ProBook 440", NumeroSerie = "5CD81871KS", Comentario = "Prueba 1" },
            //    new oEquipo(){ TipoNombre = "Estación Acoplamiento", MarcaNombre = "HP", ModeloNombre = "US", NumeroSerie = "8CJ815B1T6", Comentario = "Prueba 2" }
            //};

            return listaEquipos;
        }


        public List<Equipo> ObtenerEquiposPorNumeroSerie2(string NumeroSerie)
        {
            List<Equipo> equipos;

            using (InventarioSoporteSistemas_desEntities bd_iss = new InventarioSoporteSistemas_desEntities())
            {
                equipos = bd_iss.Equipo.Where(eq => eq.Serie.Equals(NumeroSerie)).ToList();
            }

            return equipos;
        }
        

        public oHito ObtenerIdEquipoPorNumeroSerie(string NumeroSerie)
        {
            oHito resultado = new oHito();
            List<Equipo> equipos_bd;


            if (!string.IsNullOrEmpty(NumeroSerie))
            {
                using (InventarioSoporteSistemas_desEntities bd_iss = new InventarioSoporteSistemas_desEntities())
                {
                    equipos_bd = bd_iss.Equipo.Where(eq => eq.Serie.Equals(NumeroSerie)).ToList();
                }


                if (equipos_bd.Count() == 0)
                {
                    resultado.error = "No existe un equipo con el número de serie igual a " + NumeroSerie.ToString();
                    resultado.id = 0;
                }
                else if (equipos_bd.Count() > 1)
                {
                    // PENDIENTE: ListarEquipos, mientras tanto, arroja mensaje de error

                    //messages_errors = "¡Equipos " + buscarequipo.ToString() + " encontrados!";
                    //TempData["exito"] = messages_errors;
                    //return RedirectToAction("ListarEquipos", "ISS", new { @equipos_con_numero_serie_repetido = equipos_bd });

                    resultado.error = equipos_bd.Count() + " equipos con número de serie " + NumeroSerie.ToString() + " repetido. ";
                    resultado.id = 0;
                }
                else
                {
                    resultado.error = "¡Equipo " + NumeroSerie.ToString() + " encontrado!";
                    resultado.id = equipos_bd[0].EquipoId;
                }
            }
            else
            {
                resultado.error = "No fue posible cargar el Equipo con número de serie " + NumeroSerie.ToString() + " ya que no ingresó un valor alfanumérico. ";
                resultado.id = 0;
            }
            
            return resultado;
        }


        public int ObtenerIdEquipo(string NumeroSerie)
        {
            int resultado = 0;

            //List<Equipo> equipos_bd;

            //if (!string.IsNullOrEmpty(NumeroSerie))
            //{
            //    using (InventarioSoporteSistemas_desEntities bd_iss = new InventarioSoporteSistemas_desEntities())
            //    {
            //        equipos_bd = bd_iss.Equipo.Where(eq => eq.Serie.Equals(NumeroSerie)).ToList();
            //    }

            //    if (equipos_bd.Count() == 0)
            //    {

            //    }
            //    else if (equipos_bd.Count() > 1)
            //    {

            //    }
            //    else
            //    {
            //        foreach (Equipo eq in equipos_bd)
            //            resultado = eq.EquipoId;
            //    }
            //}

            Equipo equipo_bd;

            if (!string.IsNullOrEmpty(NumeroSerie))
            {
                using (InventarioSoporteSistemas_desEntities bd_iss = new InventarioSoporteSistemas_desEntities())
                {
                    equipo_bd = bd_iss.Equipo.Where(eq => eq.Serie.Equals(NumeroSerie)).FirstOrDefault();

                    if ((equipo_bd != null) && !equipo_bd.EquipoId.Equals(null))
                    {
                        resultado = equipo_bd.EquipoId;
                    }
                }
            }

            return resultado;
        }


        public oGuia ObtenerGuia(int idguia)                                  // desde BD a la App. 
        {
            string aux = string.Empty;

            oGuia guia_app = IniciarGuia();


            using (InventarioSoporteSistemas_desEntities bd_iss = new InventarioSoporteSistemas_desEntities())
            {
                Guia guia_bd = bd_iss.Guia.Where(g => g.GuiaId == idguia).FirstOrDefault();

                if ((guia_bd != null) && !guia_bd.Equals(null))
                {

                    guia_app.GuiaId = guia_bd.GuiaId;

                    if (guia_bd.NumeroGuia.HasValue)
                    {
                        guia_app.NumeroGuia = guia_bd.NumeroGuia;
                    }

                    if (guia_bd.ProveedorId.HasValue)
                    {
                        guia_app.ProveedorId = guia_bd.ProveedorId;
                        guia_app.ProveedorNombre = guia_bd.Proveedor.Nombre;
                    }

                    if (!string.IsNullOrEmpty(guia_bd.OrdenCompra))
                        guia_app.OrdenCompra = guia_bd.OrdenCompra;

                    if (!string.IsNullOrEmpty(guia_bd.NotaVenta))
                        guia_app.NotaVenta = guia_bd.NotaVenta;

                    if (guia_bd.FechaGuia.HasValue)
                        guia_app.FechaGuia = guia_bd.FechaGuia.Value;

                    guia_app.FechaCreacion = guia_bd.FechaCreacion.Value;

                    if (guia_bd.CreadorId.HasValue)
                    {
                        guia_app.CreadorId = guia_bd.CreadorId;
                        guia_app.CreadorNombre = guia_bd.Colaborador.NombreCompleto;
                        guia_app.CreadorEmail = guia_bd.Colaborador.Email;
                        guia_app.CreadorIdRed = guia_bd.Colaborador.NickName;
                    }
                    
                    if (guia_bd.NumeroFactura.HasValue)
                    {
                        guia_app.NumeroFactura = guia_bd.NumeroFactura;
                    }

                }
                else
                {
                    guia_bd.GuiaId = 0;
                }
            }

            return guia_app;
        }

        
        public List<oDocumento> ObtenerDocumento(int guiaid)                                    // PENDIENTE
        {
            List<oDocumento> listaDocumentos = new List<oDocumento>
            {
                new oDocumento(){ Id = 1, GuiaId = guiaid, DocumentoDescripcion = "Diagrama", DocumentoNombre = "diagramaap.bpm", DocumentoTipo = "Bizagi", DocumentoFecha = DateTime.Today, DocumentoUsuario = "vsandoval"},
                new oDocumento(){ Id = 2, GuiaId = guiaid, DocumentoDescripcion = "Especificación de Requerimientos", DocumentoNombre = "requerimiento.doc", DocumentoTipo = "Word", DocumentoFecha = DateTime.Today, DocumentoUsuario = "vsandoval"}
            };

            return listaDocumentos;
        }


        public Usuario ConstruirUsuario(oUsuario usuario)                           // de usuario (App) a usuario_bd (BD)
        {
            Usuario usuario_bd = new Usuario()
            {
                Nombre = usuario.nombre,
                Apellido = usuario.apellido,
                NombreCompleto = usuario.nombrecompleto,
                Cargo = usuario.cargo,
                Email = usuario.email,
                NickName = usuario.nickname
            };

            return usuario_bd;
        }


        public oUsuario CrearUsuarioEnBD(oUsuario us)
        {
            string messages_errors = string.Empty;

            var errors = ModelState.Values.SelectMany(v => v.Errors);

            if (ModelState.IsValid)

            {
                Usuario usuario_bd = new Usuario();
                usuario_bd = ConstruirUsuario(us);

                usuario_bd.Equipo = null;
                usuario_bd.Equipo1 = null;
                usuario_bd.Equipo2 = null;

                using (InventarioSoporteSistemas_desEntities bd_iss = new InventarioSoporteSistemas_desEntities())
                {
                    try
                    {
                        bd_iss.Usuario.Add(usuario_bd);
                        int resultado = bd_iss.SaveChanges();

                        if (resultado > 0)
                        {
                            messages_errors = "Usuario " + usuario_bd.UsuarioId.ToString() + " creado en Base de Datos";
                            us.id = usuario_bd.UsuarioId;
                            us.error = messages_errors;
                        }
                        else
                        {
                            messages_errors = "No fue creado el Usuario " + us.nickname;
                            us.id = 0;
                            us.error = messages_errors;
                        }
                    }
                    catch (DbEntityValidationException ex)
                    {
                        foreach (DbEntityValidationResult item in ex.EntityValidationErrors)
                        {
                            // Get entry

                            DbEntityEntry entry = item.Entry;
                            string entityTypeName = entry.Entity.GetType().Name;

                            // Display or log error messages

                            foreach (DbValidationError subItem in item.ValidationErrors)
                            {
                                messages_errors = messages_errors + string.Format("Error '{0}' occurrió en {1} a {2}" + "\r\n",
                                         subItem.ErrorMessage, entityTypeName, subItem.PropertyName);
                                //Console.WriteLine(message);
                            }
                        }

                        us.id = 0;
                        us.error = messages_errors;
                    }
                }
            }
            else
            {
                messages_errors = string.Join("; ", ModelState.Values
                                    .SelectMany(x => x.Errors)
                                    .Select(x => x.ErrorMessage));

                us.id = 0;
                us.error = messages_errors;
            }

            return us;
        }


        public oUsuario ObtenerUsuarioDesdeActiveDirectory(string email)
        {
            oUsuario us_asociado = new oUsuario();
            string error = string.Empty;
            string[] nc;
            string[] nn = email.ToString().Split('@');
            string nickname = nn[0].Trim();

            string aux_nombre = string.Empty, aux_apellido = string.Empty;

            try
            {
                string constring = System.Configuration.ConfigurationManager.ConnectionStrings["usuarios_socovesa"].ConnectionString;
                SqlConnection sqlConnection = new SqlConnection(constring);
                DataSet objSet = new DataSet();

                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                //string Query = "select PersonalId,Nombre,Apellidopaterno,Cargo,NombreCompleto,email,nickname,Rol from USUARIOS_MAILS where nickname = '" + oportunidadid.ToString().ToUpper() + "'";
                string Query = "select FullName,Description,Dominio,EMAIL from USUARIOS_MAILS where nickname = '" + nickname.ToString().ToUpper() + "'";

                sqlCommand.CommandText = Query;

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                sqlDataAdapter.SelectCommand = sqlCommand;
                sqlDataAdapter.Fill(objSet);

                if (objSet.Tables.Count > 0)
                {
                    foreach (DataRow dr in objSet.Tables[0].Rows)
                    {
                        if (dr["FullName"].ToString().Contains(","))
                        {
                            nc = dr["FullName"].ToString().Split(',');
                            aux_nombre = nc[1].Trim();
                            aux_apellido = nc[0].Trim();
                        }
                        else if (dr["FullName"].ToString().Contains(" "))
                        {
                            nc = dr["FullName"].ToString().Split(' ');
                            aux_nombre = nc[0].Trim();
                            aux_apellido = nc[1].Trim();
                        }
                        else
                        {
                            aux_nombre = dr["FullName"].ToString();
                            aux_apellido = " ";
                        }
                            
                        us_asociado.id = 1;
                        us_asociado.nombre = aux_nombre;
                        us_asociado.apellido = aux_apellido;
                        us_asociado.nombrecompleto = aux_nombre + " " + aux_apellido;
                        us_asociado.cargo = dr["Description"].ToString();
                        us_asociado.email = dr["EMAIL"].ToString();
                        us_asociado.nickname = nickname.ToString();
                        us_asociado.error = "ok";
                        us_asociado.rolid = 2;                              // tecnico
                    }
                }
                else
                {
                    us_asociado.id = 0;
                    us_asociado.error = "No EXISTE el nickname " + nickname.ToString().ToUpper() + " en el dominio Socovesa";
                }
            }
            catch (Exception er)
            {
                us_asociado.id = 0;
                us_asociado.error = "ERROR al recuperar el nickname " + nickname.ToString().ToUpper() + " del dominio Socovesa: .- " + er.Message + "-. ";
            }

            return us_asociado;
        }


        public int ObtenerIdColaborador(string email)                               // obtener Id del colaborador desde el nickname
        {
            int idcolaborador = 1;
            string[] nn = email.ToString().Split('@');
            string nickname = nn[0].Trim();

            using (InventarioSoporteSistemas_desEntities bd_iss = new InventarioSoporteSistemas_desEntities())
            {
                // en tabla Colaborador se busca por nickname 
                Colaborador colaborador = bd_iss.Colaborador.Where(e => e.NickName.Equals(nickname)).FirstOrDefault();

                if ((colaborador != null) && !colaborador.Id.Equals(null))
                {
                    idcolaborador = colaborador.Id;
                }
                else
                {
                }
            }

            return idcolaborador;
        }


        public int ObtenerIdUsuario(string email)
        {
            int idusuario = 0;
            string[] nn = email.ToString().Split('@');
            string nickname = nn[0].Trim();

            oUsuario usuario = new oUsuario();
            oUsuario usuario_nuevo = new oUsuario();
            string messages_errors = string.Empty;

            if (!string.IsNullOrEmpty(email))
            {
                using (InventarioSoporteSistemas_desEntities bd_iss = new InventarioSoporteSistemas_desEntities())
                {
                    // Se busca NickName en tabla Usuario 
                    Usuario usuario_bd = bd_iss.Usuario.Where(e => e.NickName.Equals(nickname)).FirstOrDefault();

                    if ((usuario_bd != null) && !usuario_bd.UsuarioId.Equals(null))
                    {
                        idusuario = usuario_bd.UsuarioId;
                    }
                    else
                    {
                        usuario = ObtenerUsuarioDesdeActiveDirectory(email);

                        if (usuario.id == 0)
                        {
                            messages_errors = "Usuario no existe en el Active Directory";
                            idusuario = 1;                                                              // No Existe
                        }
                        else
                        {
                            // Crear en tabla USUARIO y devolver ID de nuevo usuario
                            usuario_nuevo = CrearUsuarioEnBD(usuario);

                            if (usuario_nuevo.id == 0)
                            {
                                messages_errors = "Error al crear el Usuario";
                                idusuario = 1;                                                          // No Existe
                            }
                            else
                            {
                                idusuario = usuario_nuevo.id;
                            }
                        }
                    }
                }
            }
            else
            {
                messages_errors = "String es NULL o está vacío";
                //idusuario = 1;                                                                    // No Existe
            }
            
            return idusuario;
        }


        public string ObtenerEmailColaborador(string email_original)                // obtener Email del colaborador desde el nickname
        {
            string[] nn = email_original.ToString().Split('@');
            string nickname = nn[0].Trim();
            string email_definitivo = string.Empty;

            using (InventarioSoporteSistemas_desEntities bd_iss = new InventarioSoporteSistemas_desEntities())
            {
                // en tabla Colaborador se busca por nickname 
                Colaborador colaborador = bd_iss.Colaborador.Where(e => e.NickName.Equals(nickname)).FirstOrDefault();

                if ((colaborador != null) && !colaborador.Id.Equals(null))
                {
                    email_definitivo = colaborador.Email;
                }
                else
                {
                }
            }

            return email_definitivo;
        }


        public string ObtenerStringDeCUENTASDesdeActiveDirectory()
        {
            string Cuentas = string.Empty;

            try
            {
                string constring = System.Configuration.ConfigurationManager.ConnectionStrings["usuarios_socovesa"].ConnectionString;
                SqlConnection sqlConnection = new SqlConnection(constring);
                DataSet objSet = new DataSet();

                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                //string Query = "select PersonalId,Nombre,Apellidopaterno,Cargo,NombreCompleto,email,nickname,Rol from USUARIOS_MAILS where nickname = '" + oportunidadid.ToString().ToUpper() + "'";
                string Query = "select FullName,Description,Dominio,EMAIL,NickName from USUARIOS_MAILS";

                sqlCommand.CommandText = Query;

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                sqlDataAdapter.SelectCommand = sqlCommand;
                sqlDataAdapter.Fill(objSet);

                if (objSet.Tables.Count > 0)
                {
                    foreach (DataRow dr in objSet.Tables[0].Rows)
                    {
                        //Cuentas = Cuentas + "'" + dr["EMAIL"].ToString() + "', ";
                        Cuentas = Cuentas + "{" + "'nickname':'" + dr["NickName"].ToString() + "'" + "," + "'email':'" + dr["EMAIL"].ToString() + "'" + "," + "'nombre':'" + dr["FullName"].ToString() + "'" + "}, ";
                    }

                    Cuentas = Cuentas.TrimEnd(' ');
                    Cuentas = Cuentas.TrimEnd(',');
                }
                else
                {
                    Cuentas = "No se generó listado de cuentas del dominio Socovesa";
                }
            }
            catch (Exception er)
            {
                Cuentas = "ERROR al recuperar cuentas del dominio Socovesa: .- " + er.Message + "-. ";
            }

            return Cuentas;
        }


        public string ObtenerStringProyectoConexion()                       // 
        {
            string aux = string.Empty;
            string ListaProyectoConexion = string.Empty;

            using (InventarioSoporteSistemas_desEntities bd_iss = new InventarioSoporteSistemas_desEntities())
            {
                List<Proyecto> proyectos = bd_iss.Proyecto.ToList();

                foreach (var proyecto in proyectos)
                {
                    List<ProyectoConexion> ProCon = bd_iss.ProyectoConexion.Where(p => p.ProyectoId == proyecto.ProyectoId).ToList();
                    
                    if (ProCon.Count() > 0)
                    {
                        foreach (var pro in ProCon)
                        {
                            ListaProyectoConexion = ListaProyectoConexion
                            + "{"
                            + "'proyecto_nombre':'" + pro.Proyecto.ProyectoNombre + "'" + ","
                            + "'macroempresa_id':'" + pro.MacroEmpresaId + "'" + ","
                            + "'razonsocial_nombre':'" + pro.Empresa.RazonSocial + "'" + ","
                            + "'centrocosto_nombre':'" + pro.CentroCosto.CCCodigo + "'" + ","
                            + "'ubicacion_id':'" + pro.UbicacionId + "'" + ","
                            + "'ubicacion_nombre':'" + pro.Ubicacion.UbicacionNombre + "'" + ","
                            + "'comuna_nombre':'" + pro.Comuna.ComunaNombre + "'" + ","
                            + "'direccion_nombre':'" + pro.Direccion.DireccionNombre + "'"
                            + "}, ";
                        }
                    }
                    else
                    {
                        ListaProyectoConexion = ListaProyectoConexion
                        + "{"
                        + "'proyecto_nombre':'" + proyecto.ProyectoNombre + "'" + ","
                        + "'macroempresa_id':'" + "" + "'" + ","
                        + "'razonsocial_nombre':'" + "" + "'" + ","
                        + "'centrocosto_nombre':'" + "" + "'" + ","
                        + "'ubicacion_id':'" + "" + "'" + ","
                        + "'ubicacion_nombre':'" + "" + "'" + ","
                        + "'comuna_nombre':'" + "" + "'" + ","
                        + "'direccion_nombre':'" + "" + "'"
                        + "}, ";
                    }
                }

                ListaProyectoConexion = ListaProyectoConexion.TrimEnd(' ');
                ListaProyectoConexion = ListaProyectoConexion.TrimEnd(',');
            }
            
            return ListaProyectoConexion;
        }



        public void IniciarViewbag()
        {
            using (InventarioSoporteSistemas_desEntities bd_iss = new InventarioSoporteSistemas_desEntities())
            {
                // para DropDown

                List<Proveedor> proveedores = bd_iss.Proveedor.OrderBy(a => a.Nombre).ToList();
                ViewBag.ListaProveedores = proveedores;

                List<MacroEmpresa> macroempresas = bd_iss.MacroEmpresa.OrderBy(a => a.MacroEmpresaNombre).ToList();
                ViewBag.ListaMacroEmpresas = macroempresas;

                //List<Comuna> comunas = bd_iss.Comuna.OrderBy(a => a.ComunaNombre).ToList();
                //ViewBag.ListaComunas = comunas;
                
                List<Ubicacion> ubicaciones = bd_iss.Ubicacion.OrderBy(a => a.UbicacionNombre).ToList();
                ViewBag.ListaUbicaciones = ubicaciones;

                int estado = 1;         // 1: nuevo, 2: producción
                List<SubEstadoEquipo> subestadoequipos = bd_iss.SubEstadoEquipo.Where(e => e.EstadoEquipoId != estado).OrderBy(a => a.Nombre).ToList();
                ViewBag.ListaSubEstadoEquipos = subestadoequipos;



                // para autocomplete


                // ************************************************************
                // ****************************************************** Marca
                // ************************************************************

                int contMar = 0;
                StringBuilder ListaMarcas = new StringBuilder(string.Empty);

                List<Marca> marcas = bd_iss.Marca.ToList();

                if (marcas.Count > 0)
                {
                    for (contMar = 0; contMar < marcas.Count; contMar++)
                    {
                        ListaMarcas.Append("'");
                        ListaMarcas.Append(marcas[contMar].Nombre);
                        ListaMarcas.Append("', ");
                    }

                    ViewBag.StringMarcas = ListaMarcas.Remove(ListaMarcas.Length - 2, 2).ToString();
                }
                else
                {
                    ViewBag.StringMarcas = string.Empty;
                }



                // ************************************************************
                // *********************************************** Centro Costo
                // ************************************************************

                int contCC = 0;
                StringBuilder ListaCC = new StringBuilder(string.Empty);

                List<CentroCosto> centrocosto = bd_iss.CentroCosto.ToList();

                if (centrocosto.Count > 0)
                {
                    for (contCC = 0; contCC < centrocosto.Count; contCC++)
                    {
                        ListaCC.Append("'");
                        ListaCC.Append(centrocosto[contCC].CCCodigo);
                        ListaCC.Append("', ");
                    }

                    ViewBag.StringCentroCosto = ListaCC.Remove(ListaCC.Length - 2, 2).ToString();
                }
                else
                {
                    ViewBag.StringCentroCosto = string.Empty;
                }



                // ************************************************************
                // ***************************************************** Comuna
                // ************************************************************

                int contCom = 0;
                StringBuilder ListaCom = new StringBuilder(string.Empty);

                List<Comuna> comuna = bd_iss.Comuna.ToList();

                if (comuna.Count > 0)
                {
                    for (contCom = 0; contCom < comuna.Count; contCom++)
                    {
                        ListaCom.Append("'");
                        ListaCom.Append(comuna[contCom].ComunaNombre);
                        ListaCom.Append("', ");
                    }

                    ViewBag.StringComunas = ListaCom.Remove(ListaCom.Length - 2, 2).ToString();
                }
                else
                {
                    ViewBag.StringComunas = string.Empty;
                }



                // ************************************************************
                // ************************************************** Direccion
                // ************************************************************

                int contDir = 0;
                StringBuilder ListaDir = new StringBuilder(string.Empty);

                List<Direccion> direccion = bd_iss.Direccion.ToList();

                if (direccion.Count > 0)
                {
                    for (contDir = 0; contDir < direccion.Count; contDir++)
                    {
                        ListaDir.Append("'");
                        ListaDir.Append(direccion[contDir].DireccionNombre);
                        ListaDir.Append("', ");
                    }

                    ViewBag.StringDirecciones = ListaDir.Remove(ListaDir.Length - 2, 2).ToString();
                }
                else
                {
                    ViewBag.StringDirecciones = string.Empty;
                }



                // ************************************************************
                // ****************************************** Sistema Operativo
                // ************************************************************

                int contSO = 0;
                StringBuilder ListaSO = new StringBuilder(string.Empty);

                List<SistemaOperativo> sistemaoperativo = bd_iss.SistemaOperativo.ToList();

                if (sistemaoperativo.Count > 0)
                {
                    for (contSO = 0; contSO < sistemaoperativo.Count; contSO++)
                    {
                        ListaSO.Append("'");
                        ListaSO.Append(sistemaoperativo[contSO].SONombre);
                        ListaSO.Append("', ");
                    }

                    ViewBag.StringSO = ListaSO.Remove(ListaSO.Length - 2, 2).ToString();
                }
                else
                {
                    ViewBag.StringSO = string.Empty;
                }



                // ************************************************************
                // ************************************************ Tipo Equipo
                // ************************************************************

                int contTE = 0;
                StringBuilder ListaTE = new StringBuilder(string.Empty);

                List<TipoEquipo> tipoequipo = bd_iss.TipoEquipo.ToList();

                if (tipoequipo.Count > 0)
                {
                    for (contTE = 0; contTE < tipoequipo.Count; contTE++)
                    {
                        ListaTE.Append("'");
                        ListaTE.Append(tipoequipo[contTE].Nombre);
                        ListaTE.Append("', ");
                    }

                    ViewBag.StringTipoEquipo = ListaTE.Remove(ListaTE.Length - 2, 2).ToString();
                }
                else
                {
                    ViewBag.StringTipoEquipo = string.Empty;
                }



                // ************************************************************
                // **************************************************** Empresa
                // ************************************************************

                int contEmp = 0;
                //StringBuilder ListaEmpRS = new StringBuilder(string.Empty);
                //StringBuilder ListaEmpRut = new StringBuilder(string.Empty);
                StringBuilder ListaEmpExt = new StringBuilder(string.Empty);

                List<Empresa> empresas = bd_iss.Empresa.ToList();

                if (empresas.Count > 0)
                {
                    for (contEmp = 0; contEmp < empresas.Count; contEmp++)
                    {
                        //ListaEmpRS.Append("'");
                        //ListaEmpRS.Append(empresas[contEmp].RazonSocial);
                        //ListaEmpRS.Append("', ");

                        //ListaEmpRut.Append("'");
                        //ListaEmpRut.Append(empresas[contEmp].Rut);
                        //ListaEmpRut.Append("', ");

                        ListaEmpExt.Append("{'rut':'");
                        ListaEmpExt.Append(empresas[contEmp].Rut);
                        ListaEmpExt.Append("','razonsocial':'");
                        ListaEmpExt.Append(empresas[contEmp].RazonSocial);

                        ListaEmpExt.Append("'}, ");
                    }

                    ViewBag.StringRSEmpresas = ListaEmpExt.Remove(ListaEmpExt.Length - 2, 2).ToString();
                    ViewBag.StringRutEmpresas = ListaEmpExt.Remove(ListaEmpExt.Length - 2, 2).ToString();
                }
                else
                {
                    ViewBag.StringRSEmpresas = string.Empty;
                    ViewBag.StringRutEmpresas = string.Empty;
                }



                // ************************************************************
                // *************************************************** Conexion
                // ************************************************************

                int contProyectos = 0;
                int contProCon = 0;
                StringBuilder ListaProyectoConexion = new StringBuilder(string.Empty);
                int p_aux = 0;

                List<Proyecto> proyectos = bd_iss.Proyecto.ToList();

                if (proyectos.Count > 0)
                {
                    for (contProyectos = 0; contProyectos < proyectos.Count; contProyectos++)
                    {
                        p_aux = proyectos[contProyectos].ProyectoId;
                        List<ProyectoConexion> ProCon = bd_iss.ProyectoConexion.Where(p => p.ProyectoId == p_aux).ToList();

                        if (ProCon.Count() > 0)
                        {
                            for (contProCon = 0; contProCon < ProCon.Count; contProCon++)
                            {
                                ListaProyectoConexion.Append("{'proyecto_nombre':'");
                                ListaProyectoConexion.Append(ProCon[contProCon].Proyecto.ProyectoNombre);
                                ListaProyectoConexion.Append("','macroempresa_id':'");
                                ListaProyectoConexion.Append(ProCon[contProCon].MacroEmpresaId);
                                ListaProyectoConexion.Append("','razonsocial_nombre':'");
                                ListaProyectoConexion.Append(ProCon[contProCon].Empresa.RazonSocial);
                                ListaProyectoConexion.Append("','centrocosto_nombre':'");
                                ListaProyectoConexion.Append(ProCon[contProCon].CentroCosto.CCCodigo);
                                ListaProyectoConexion.Append("','ubicacion_id':'");
                                ListaProyectoConexion.Append(ProCon[contProCon].UbicacionId);
                                ListaProyectoConexion.Append("','ubicacion_nombre':'");
                                ListaProyectoConexion.Append(ProCon[contProCon].Ubicacion.UbicacionNombre);
                                ListaProyectoConexion.Append("','comuna_nombre':'");
                                ListaProyectoConexion.Append(ProCon[contProCon].Comuna.ComunaNombre);
                                ListaProyectoConexion.Append("','direccion_nombre':'");
                                ListaProyectoConexion.Append(ProCon[contProCon].Direccion.DireccionNombre);
                                ListaProyectoConexion.Append("'}, ");
                            }
                        }
                        else
                        {
                            ListaProyectoConexion.Append("{'proyecto_nombre':'");
                            ListaProyectoConexion.Append(proyectos[contProyectos].ProyectoNombre);
                            ListaProyectoConexion.Append("','macroempresa_id':'','razonsocial_nombre':'','centrocosto_nombre':'','ubicacion_id':'','ubicacion_nombre':'','comuna_nombre':'','direccion_nombre':''}, ");
                        }
                    }

                    ViewBag.StringProyectos = ListaProyectoConexion.Remove(ListaProyectoConexion.Length - 2, 2).ToString();
                }
                else
                {
                    ViewBag.StringProyectos = string.Empty;
                }

                //ViewBag.StringProyectos = ObtenerStringProyectoConexion();



                // ************************************************************
                // **************************************************** Cuentas
                // ************************************************************

                int contCuentas = 0;
                StringBuilder ListaCuentas = new StringBuilder(string.Empty);

                List<oCuentaAD> cuentas = bd_iss.Database.SqlQuery<oCuentaAD>("SP_DB_Obtener_Cuentas").ToList();

                if (cuentas.Count > 0)
                {
                    for (contCuentas = 0; contCuentas < cuentas.Count; contCuentas++)
                    {
                        ListaCuentas.Append("{'nickname':'");
                        ListaCuentas.Append(cuentas[contCuentas].nickname);
                        ListaCuentas.Append("','email':'");
                        ListaCuentas.Append(cuentas[contCuentas].email);
                        ListaCuentas.Append("','nombre':'");
                        ListaCuentas.Append(cuentas[contCuentas].nombre);
                        ListaCuentas.Append("'}, ");
                    }

                    //ViewBag.StringSolicitantes = myString.Remove(myString.Length - 2, 2).ToString();
                    ViewBag.StringCuentasAD = ListaCuentas.Remove(ListaCuentas.Length - 2, 2).ToString();
                }
                else
                {
                    //ViewBag.StringSolicitantes = string.Empty;
                    ViewBag.StringCuentasAD = string.Empty;
                }
                
                //ViewBag.StringCuentasAD = ObtenerStringDeCUENTASDesdeActiveDirectory();

                

            }
        }


        public oGuia IniciarGuia()
        {
            string aux_cab = string.Empty, idred = string.Empty;

            if (!string.IsNullOrEmpty(@System.Environment.UserName))
                idred = @System.Environment.UserName;

            oGuia iniciar_guia = new oGuia()
            {
                GuiaId = 1,

                //NumeroGuia = 0,
                NumeroFactura = 0,

                OrdenCompra = aux_cab,
                NotaVenta = aux_cab,

                //ProveedorId = 5,                    // 5: IIA
                //ProveedorNombre = aux_cab,

                FechaCreacion = DateTime.Now,
                FechaGuia = DateTime.Now,           // Fecha sugerida pero que puede ser ajustada

                CreadorId = 1,                      // 1: sin asignar
                CreadorNombre = aux_cab,
                CreadorIdRed = idred,

                //CreadorId = ObtenerIdColaborador(idred + "@socovesa.cl"),
                //CreadorIdRed = idred,
                //CreadorEmail = ObtenerEmailColaborador(idred + "@socovesa.cl"),

                equipos = ObtenerEquiposPorNumeroGuia(0),
                documentos = ObtenerDocumento(0)
                
            };

            return iniciar_guia;
        }


        public oEquipo IniciarEquipo()
        {
            string aux = string.Empty, idred = string.Empty;

            if (!string.IsNullOrEmpty(@System.Environment.UserName))
                idred = @System.Environment.UserName;

            oEquipo iniciar_equipo = new oEquipo()
            {
                EquipoId = 1,
                GuiaId = 0,
                NumeroGuia = aux,

                TipoId = 0,                                     // 6: CPU
                MarcaId = 0,                                    // 17: HP
                ModeloNombre = aux,
                NumeroSerie = aux,

                Bolso = false,
                Mouse = false,
                Candado = false,

                Comentario = aux,
                
                FechaCreacion = DateTime.Now,
                CreadorId = 1,                                  // 1: sin asignar 
                EstadoId = 1,                                   // 1: Nuevo
                SubEstadoId = 1,                                // 1: Creado
                TecnicoPreId = 1,                               // 1: sin asignar 
                TecnicoEntId = 1                                // 1: sin asignar 
            };

            return iniciar_equipo;
        }


        public IList<DB_MyTE> FormarCadenaParaTablaNuevoEquipo(string sp)
        {
            using (InventarioSoporteSistemas_desEntities bd_iss = new InventarioSoporteSistemas_desEntities())
            {
                return bd_iss.Database.SqlQuery<DB_MyTE>(sp).ToList();
            }
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult Cargar_DB_MyTE()
        {
            //string aux_octs = FormarCadenaParaGraficoTorta("SP_DB_Obtener_Tipo_Solicitud", "Tipo", "Cant.");

            //return Json(aux_octs, JsonRequestBehavior.AllowGet);

            var modelList = FormarCadenaParaTablaNuevoEquipo("SP_DB_Obtener_MyTE");

            var modelData = modelList.Select(m => new SelectListItem()
            {                
                Text = m.Nombre.ToString(),
                Value = m.Origen.ToString()
            });

            return Json(modelData, JsonRequestBehavior.AllowGet);
        }


        public Guia ConstruirGuia(oGuia guia_original)                              // de guia_original (App) a guia_bd (BD)
        {
            string aux = string.Empty, idred = string.Empty, email_creador = string.Empty;
            int aux_creador = 0;

            Guia guia_bd = new Guia()
            {
                GuiaId = 1,
                NumeroGuia = 0,
                NumeroFactura = 0,
                OrdenCompra = aux,
                NotaVenta = aux,
                FechaCreacion = DateTime.Now,
                FechaGuia = DateTime.Now,
                //Descripción = aux,
                CreadorId = 1,                                  // 1: sin asignar 
                ProveedorId = 5,                                // 5: IIA
            };

            // para la creación, se asigna Id igual a 0 (cero)
            guia_bd.GuiaId = guia_original.GuiaId;

            if (guia_original.NumeroGuia.HasValue)
            {
                guia_bd.NumeroGuia = guia_original.NumeroGuia.Value;
            }

            if (guia_original.NumeroFactura.HasValue)
            {
                guia_bd.NumeroFactura = guia_original.NumeroFactura.Value;
            }

            if (!string.IsNullOrEmpty(guia_original.OrdenCompra))
            {
                guia_bd.OrdenCompra = guia_original.OrdenCompra.ToUpper();
            }

            if (!string.IsNullOrEmpty(guia_original.NotaVenta))
            {
                guia_bd.NotaVenta = guia_original.NotaVenta;
            }

            if (guia_original.FechaCreacion.HasValue)
                guia_bd.FechaCreacion = guia_original.FechaCreacion;

            if (guia_original.FechaGuia.HasValue)
                guia_bd.FechaGuia = guia_original.FechaGuia;

            if (!string.IsNullOrEmpty(@System.Environment.UserName))
            {
                idred = @System.Environment.UserName;
                email_creador = idred + "@socovesa.cl";
                aux_creador = ObtenerIdColaborador(email_creador);
                if (aux_creador > 0)
                    guia_bd.CreadorId = aux_creador;
            }
            
            using (InventarioSoporteSistemas_desEntities bd_iss = new InventarioSoporteSistemas_desEntities())
            {
                if (guia_original.ProveedorId.HasValue)
                {
                    guia_bd.ProveedorId = guia_original.ProveedorId;
                    guia_bd.Proveedor = bd_iss.Proveedor.Where(p => p.Id == guia_original.ProveedorId.Value).FirstOrDefault();
                }
            }

            return guia_bd;
        }


        public Equipo ConstruirEquipo(oEquipo equipo_original)                      // de equipo_original (App) a equipo_bd (BD)
        {
            string aux = string.Empty, idred = string.Empty, email_creador = string.Empty;
            int aux_creador = 0;
            int aux_estadoid = 1;


            Equipo equipo_bd = new Equipo()
            {
                EquipoId = 1,
                TipoId = 0,                                     // 6: CPU
                MarcaId = 0,                                    // 17: HP
                Modelo = aux,
                Serie = aux,
                Comentario = aux,
                GuiaId = 0,
                FechaCreacion = DateTime.Now,
                CreadorId = 1,                                  // 1: sin asignar 
                EstadoId = 1,                                   // 1: Nuevo
                SubEstadoId = 1,                                // 1: Creado
                TecnicoPreId = 1,                               // 1: sin asignar 
                TecnicoEntId = 1,                               // 1: sin asignar 
                Bolso = 0,
                Mouse = 0,
                Candado = 0,
                DistribucionId = 1
            };

            // para la creación, se asigna Id igual a 1 (cero)
            if(equipo_original.EquipoId > 0)
                equipo_bd.EquipoId = equipo_original.EquipoId;

            if (!string.IsNullOrEmpty(equipo_original.ModeloNombre))
            {
                equipo_bd.Modelo = equipo_original.ModeloNombre.ToUpper();
            }

            if (!string.IsNullOrEmpty(equipo_original.NumeroSerie))
            {
                equipo_bd.Serie = equipo_original.NumeroSerie.ToUpper();
            }

            if (!string.IsNullOrEmpty(equipo_original.Comentario))
            {
                equipo_bd.Comentario = equipo_original.Comentario;
            }

            if (equipo_original.FechaCreacion.HasValue)
                equipo_bd.FechaCreacion = equipo_original.FechaCreacion;
            
            if (!string.IsNullOrEmpty(@System.Environment.UserName))
            {
                idred = @System.Environment.UserName;
                email_creador = idred + "@socovesa.cl";
                aux_creador = ObtenerIdColaborador(email_creador);
                if (aux_creador > 0)
                    equipo_bd.CreadorId = aux_creador;
            }

            if (equipo_original.Bolso)
                equipo_bd.Bolso = Convert.ToInt32(equipo_original.Bolso);

            if (equipo_original.Mouse)
                equipo_bd.Mouse = Convert.ToInt32(equipo_original.Mouse);

            if (equipo_original.Candado)
                equipo_bd.Candado = Convert.ToInt32(equipo_original.Candado);
            

            if (!string.IsNullOrEmpty(equipo_original.GerenteEmail))
                equipo_bd.GerenteAutorizaId = ObtenerIdUsuario(equipo_original.GerenteEmail);

            if (!string.IsNullOrEmpty(equipo_original.SolicitanteEmail))
                equipo_bd.SolicitanteId = ObtenerIdUsuario(equipo_original.SolicitanteEmail);

            if (!string.IsNullOrEmpty(equipo_original.UsuarioFinalEmail))
                equipo_bd.UsuarioFinalId = ObtenerIdUsuario(equipo_original.UsuarioFinalEmail);

            if (!string.IsNullOrEmpty(equipo_original.TecnicoPreEmail))
                equipo_bd.TecnicoPreId = ObtenerIdColaborador(equipo_original.TecnicoPreEmail);
            else if (equipo_original.SinTecnicoPreId)
                equipo_bd.TecnicoPreId = 5;

            if (!string.IsNullOrEmpty(equipo_original.TecnicoEntEmail))            
                equipo_bd.TecnicoEntId = ObtenerIdColaborador(equipo_original.TecnicoEntEmail);
            else if (equipo_original.SinTecnicoEntId)
                equipo_bd.TecnicoEntId = 5;
            

            if (!string.IsNullOrEmpty(equipo_original.Distribucion))
            { }
            else
            {
                equipo_original.Distribucion = "pendiente";
            }


            using (InventarioSoporteSistemas_desEntities bd_iss = new InventarioSoporteSistemas_desEntities())
            {
                //if (equipo_original.TipoId.HasValue)
                //{
                //    equipo_bd.TipoId = equipo_original.TipoId;
                //    equipo_bd.TipoEquipo = bd_iss.TipoEquipo.Where(te => te.Id == equipo_original.TipoId.Value).Single();
                //}

                if (!string.IsNullOrEmpty(equipo_original.TipoNombre))
                {
                    //equipo_bd.TipoId = equipo_original.TipoId;
                    equipo_bd.TipoEquipo = bd_iss.TipoEquipo.Where(te => te.Nombre.Equals(equipo_original.TipoNombre)).FirstOrDefault();
                    if (equipo_bd.TipoEquipo == null)
                        equipo_bd.TipoId = 24;                       // Otro
                    else
                        equipo_bd.TipoId = equipo_bd.TipoEquipo.Id;
                }

                //if (equipo_original.MarcaId.HasValue)
                //{
                //    equipo_bd.MarcaId = equipo_original.MarcaId;
                //    equipo_bd.Marca = bd_iss.Marca.Where(m => m.Id == equipo_original.MarcaId.Value).Single();
                //}

                if (!string.IsNullOrEmpty(equipo_original.MarcaNombre))
                {
                   // equipo_bd.MarcaId = equipo_original.MarcaId;
                    equipo_bd.Marca = bd_iss.Marca.Where(m => m.Nombre.Equals(equipo_original.MarcaNombre)).FirstOrDefault();
                    if (equipo_bd.Marca == null)
                        equipo_bd.MarcaId = 16;                       // Generico
                    else
                        equipo_bd.MarcaId = equipo_bd.Marca.Id;
                }

                //if (equipo_original.EstadoId.HasValue)
                //{
                //    equipo_bd.EstadoId = equipo_original.EstadoId;
                //    equipo_bd.EstadoEquipo = bd_iss.EstadoEquipo.Where(e => e.Id == equipo_original.EstadoId.Value).FirstOrDefault();
                //}
                
                if (equipo_original.SubEstadoId.HasValue)
                {
                    // Asignar SUB-ESTADO
                    equipo_bd.SubEstadoId = equipo_original.SubEstadoId;
                    equipo_bd.SubEstadoEquipo = bd_iss.SubEstadoEquipo.Where(se => se.Id == equipo_original.SubEstadoId.Value).FirstOrDefault();

                    // Obtener el ESTADO a partir del SUB-ESTADO
                    aux_estadoid = equipo_bd.SubEstadoEquipo.EstadoEquipo.Id;
                    equipo_bd.EstadoId = aux_estadoid;     // equipo_original.EstadoId;
                    equipo_bd.EstadoEquipo = bd_iss.EstadoEquipo.Where(e => e.Id == aux_estadoid).FirstOrDefault();

                }

                if (equipo_original.GuiaId.HasValue)
                {
                    equipo_bd.GuiaId = equipo_original.GuiaId;
                    equipo_bd.Guia = bd_iss.Guia.Where(g => g.GuiaId == equipo_original.GuiaId.Value).FirstOrDefault();
                }
                

                if (equipo_original.MacroEmpresaId.HasValue)
                {
                    equipo_bd.MacroEmpresaId = equipo_original.MacroEmpresaId;
                    equipo_bd.MacroEmpresa = bd_iss.MacroEmpresa.Where(g => g.MacroEmpresaId == equipo_original.MacroEmpresaId.Value).FirstOrDefault();
                }

                if (!string.IsNullOrEmpty(equipo_original.EmpresaRazonSocial))
                {
                    equipo_bd.Empresa = bd_iss.Empresa.Where(m => m.RazonSocial.Equals(equipo_original.EmpresaRazonSocial)).FirstOrDefault();
                    if (equipo_bd.Empresa == null)
                    {
                        //equipo_bd.EmpresaId = 25;                       // Socovesa S.A. (94840000-6)
                    }
                    else
                        equipo_bd.EmpresaId = equipo_bd.Empresa.EmpresaId;
                }

                if (!string.IsNullOrEmpty(equipo_original.CentroCostoNombre))
                {
                    equipo_bd.CentroCosto = bd_iss.CentroCosto.Where(m => m.CCCodigo.Equals(equipo_original.CentroCostoNombre)).FirstOrDefault();
                    if (equipo_bd.CentroCosto == null)
                    { 
                        // Nada 
                    }
                    else
                        equipo_bd.CentroCostoId = equipo_bd.CentroCosto.CCId;
                }

                if (equipo_original.UbicacionId.HasValue)
                {
                    equipo_bd.UbicacionId = equipo_original.UbicacionId;
                    equipo_bd.Ubicacion = bd_iss.Ubicacion.Where(g => g.UbicacionId == equipo_original.UbicacionId.Value).FirstOrDefault();
                }

                if (!string.IsNullOrEmpty(equipo_original.ProyectoNombre))
                {
                    equipo_bd.Proyecto = bd_iss.Proyecto.Where(m => m.ProyectoNombre.Equals(equipo_original.ProyectoNombre)).FirstOrDefault();
                    if (equipo_bd.Proyecto == null)
                    {
                        // Nada 
                    }
                    else
                        equipo_bd.ProyectoId = equipo_bd.Proyecto.ProyectoId;
                }

                if (!string.IsNullOrEmpty(equipo_original.ComunaNombre))
                {
                    equipo_bd.Comuna = bd_iss.Comuna.Where(m => m.ComunaNombre.Equals(equipo_original.ComunaNombre)).FirstOrDefault();
                    if (equipo_bd.Comuna == null)
                    {
                        // Nada 
                    }
                    else
                        equipo_bd.ComunaId = equipo_bd.Comuna.ComunaId;
                }

                if (!string.IsNullOrEmpty(equipo_original.DireccionNombre))
                {
                    equipo_bd.Direccion = bd_iss.Direccion.Where(d => d.DireccionNombre.Equals(equipo_original.DireccionNombre)).FirstOrDefault();
                    if (equipo_bd.Direccion == null)
                    {
                        // Nada 
                    }
                    else
                        equipo_bd.DireccionId = equipo_bd.Direccion.DireccionId;
                }

                if (!string.IsNullOrEmpty(equipo_original.SistemaOperativoNombre))
                {
                    // equipo_bd.MarcaId = equipo_original.MarcaId;
                    equipo_bd.SistemaOperativo = bd_iss.SistemaOperativo.Where(so => so.SONombre.Equals(equipo_original.SistemaOperativoNombre)).FirstOrDefault();
                    if (equipo_bd.SistemaOperativo == null)
                    {
                        //equipo_bd.SistemaOperativoId = 1;
                    }
                    else
                        equipo_bd.SistemaOperativoId = equipo_bd.SistemaOperativo.SOId;
                }


                if((!string.IsNullOrEmpty(equipo_original.QuienRecibeEmail)) || (!string.IsNullOrEmpty(equipo_original.QuienRetiraRut)))
                {
                    int id_distribucion = ObtenerDistribucion(equipo_original);

                    equipo_bd.DistribucionId = id_distribucion;
                    equipo_bd.Distribucion = bd_iss.Distribucion.Where(di => di.DistribucionId == id_distribucion).FirstOrDefault();
                }
                else
                {
                    equipo_bd.DistribucionId = 1;
                    equipo_bd.Distribucion = bd_iss.Distribucion.Where(di => di.DistribucionId == 1).FirstOrDefault();
                }

            }


            if (!string.IsNullOrEmpty(equipo_original.Procesador))
                equipo_bd.Procesador = equipo_original.Procesador.ToUpper();

            if (!string.IsNullOrEmpty(equipo_original.Memoria))
                equipo_bd.Memoria = equipo_original.Memoria.ToUpper();

            if (!string.IsNullOrEmpty(equipo_original.DiscoDuro))
                equipo_bd.DiscoDuro = equipo_original.DiscoDuro.ToUpper();

            if (!string.IsNullOrEmpty(equipo_original.DireccionMAC))
                equipo_bd.DireccionMAC = equipo_original.DireccionMAC.ToUpper();

            if (!string.IsNullOrEmpty(equipo_original.DireccionIP))
                equipo_bd.DireccionIP = equipo_original.DireccionIP.ToUpper();

            if (!string.IsNullOrEmpty(equipo_original.NombrePC))
                equipo_bd.NombrePC = equipo_original.NombrePC.ToUpper();

            if (!string.IsNullOrEmpty(equipo_original.KeyWindows))
                equipo_bd.KeyWindows = equipo_original.KeyWindows.ToUpper();



            if (equipo_original.FechaEntrega.HasValue)
                equipo_bd.FechaDistribucion = equipo_original.FechaEntrega.Value;
            else if (equipo_original.FechaRetira.HasValue)
                equipo_bd.FechaDistribucion = equipo_original.FechaRetira.Value;

            if (!string.IsNullOrEmpty(equipo_original.QuienRecibeEmail))
                equipo_bd.EmailDistribucion = equipo_original.QuienRecibeEmail;
            else if (!string.IsNullOrEmpty(equipo_original.QuienRetiraEmail))
                equipo_bd.EmailDistribucion = equipo_original.QuienRetiraEmail;



            // Asignar sub-estado
            // Asumo que si existe ProyectoId, entonces tiene los 7 datos básicos 

            //if (equipo_bd.SubEstadoId == 7)
            //{

            //}
            //else
            //{ 



            if (equipo_original.SubEstadoId > 10)
            {
                // Se queda con el nuevo sub-estado asignado en ActualizarEquipo()
                //equipo_bd.SubEstadoId = equipo_original.SubEstadoId;
            }

            ////if ((equipo_bd.TecnicoEntId > 1) && (equipo_original.FechaEntrega.HasValue) && (!string.IsNullOrEmpty(equipo_original.QuienRecibeEmail)))
            //if ((equipo_bd.TecnicoEntId > 1) && (equipo_original.FechaEntrega.HasValue) && (equipo_bd.DistribucionId > 1))
            //{
            //    equipo_bd.SubEstadoId = 11;                                     // Equipo Entregado
            //}
            else if (equipo_original.EquipoEntregado)
            {
                equipo_bd.SubEstadoId = 11;
                equipo_bd.EstadoId = 2;                                            // Equipo en Produccion


                // Carga de campos "Fecha Entrega", "Tecnico Ent." y "Quien Recibe (email)"

                if (!equipo_original.FechaEntrega.HasValue)
                    equipo_bd.FechaDistribucion = DateTime.Now;

                // Tecnico Ent.
                if (equipo_bd.TecnicoEntId.HasValue)
                {
                    if ((equipo_bd.TecnicoEntId.Value == 1) || (equipo_bd.TecnicoEntId.Value == 5))
                    {
                        if ((equipo_bd.TecnicoPreId.Value == 1) || (equipo_bd.TecnicoPreId.Value == 5))
                        {
                            // No tiene tecnico de Preparación

                            //equipo_bd.TecnicoEntId = 21;            // msandovals, myriam sandoval
                        }
                        else
                        {
                            equipo_bd.TecnicoEntId = equipo_bd.TecnicoPreId;
                        }
                    }
                    else
                    {


                    }

                }

                // Quien Recibe (email)
                if (string.IsNullOrEmpty(equipo_original.QuienRecibeEmail))
                {
                    equipo_bd.EmailDistribucion = equipo_original.UsuarioFinalEmail;
                    equipo_original.QuienRecibeEmail = equipo_original.UsuarioFinalEmail;

                    equipo_bd.DistribucionId = ObtenerDistribucion(equipo_original);

                    //int id_distribucion = ObtenerDistribucion(equipo_original);

                    //equipo_bd.DistribucionId = id_distribucion;
                    //equipo_bd.Distribucion = bd_iss.Distribucion.Where(di => di.DistribucionId == id_distribucion).FirstOrDefault();
                }
            }

            ////else if ((equipo_original.FechaRetira.HasValue) && (!string.IsNullOrEmpty(equipo_original.QuienRetiraNombre)) && (!string.IsNullOrEmpty(equipo_original.QuienRetiraRut)))
            //else if ((equipo_original.FechaRetira.HasValue) && (equipo_bd.DistribucionId > 1))
            //{
            //    equipo_bd.SubEstadoId = 11;                                   // Equipo Entregado
            //    equipo_bd.TecnicoEntId = 5;                                   // 5: No requerido
            //}
            else if (equipo_original.EquipoRetirado)
            {
                equipo_bd.SubEstadoId = 11;
                equipo_bd.EstadoId = 2;                                            // Equipo en Produccion


                // Carga de campos "Fecha Retira", "Tecnico Ent." y "Quien Retira (email)"

                if (!equipo_original.FechaRetira.HasValue)
                    equipo_bd.FechaDistribucion = DateTime.Now;

                equipo_bd.TecnicoEntId = 5;                                     // 5: No requerido

                if (string.IsNullOrEmpty(equipo_original.QuienRetiraEmail))
                {
                    equipo_bd.EmailDistribucion = equipo_original.UsuarioFinalEmail;
                    equipo_original.QuienRetiraEmail = equipo_original.UsuarioFinalEmail;
                }
            }

            // POST Equipo Listo para Entrega
            //else if ((equipo_original.Distribucion.Equals("entrega")) || (equipo_bd.TecnicoEntId > 1))
            else if (equipo_original.Distribucion.Equals("entrega"))
            {
                equipo_bd.SubEstadoId = 10;                                 // Equipo para Entrega


                // Carga de campos "Tecnico Ent." y "Quien Recibe (email)"

                // Tecnico Ent.
                if (equipo_bd.TecnicoEntId.HasValue)
                {
                    if ((equipo_bd.TecnicoEntId.Value == 1) || (equipo_bd.TecnicoEntId.Value == 5))
                    {
                        if ((equipo_bd.TecnicoPreId.Value == 1) || (equipo_bd.TecnicoPreId.Value == 5))
                        {
                            // No tiene tecnico de Preparación

                            //equipo_bd.TecnicoEntId = 21;            // msandovals, myriam sandoval
                        }
                        else
                        {
                            equipo_bd.TecnicoEntId = equipo_bd.TecnicoPreId;
                        }
                    }
                }

                // Quien Recibe (email)
                if (string.IsNullOrEmpty(equipo_original.QuienRecibeEmail))
                {
                    equipo_bd.EmailDistribucion = equipo_original.UsuarioFinalEmail;
                    equipo_original.QuienRecibeEmail = equipo_original.UsuarioFinalEmail;

                    equipo_bd.DistribucionId = ObtenerDistribucion(equipo_original);

                    //int id_distribucion = ObtenerDistribucion(equipo_original);

                    //equipo_bd.DistribucionId = id_distribucion;
                    //equipo_bd.Distribucion = bd_iss.Distribucion.Where(di => di.DistribucionId == id_distribucion).FirstOrDefault();
                }
            }


            else if (equipo_original.Distribucion.Equals("retira"))
            {
                equipo_bd.SubEstadoId = 9;                                  // Equipo para Retiro


                // Carga de campos "Tecnico Ent." y "Quien Retira (email)"

                equipo_bd.TecnicoEntId = 5;                                 // 5: No requerido

                if (string.IsNullOrEmpty(equipo_original.QuienRetiraEmail))
                {
                    equipo_bd.EmailDistribucion = equipo_original.UsuarioFinalEmail;
                    equipo_original.QuienRetiraEmail = equipo_original.UsuarioFinalEmail;
                }
            }


            else if ((equipo_bd.ProyectoId > 0) && (equipo_bd.UsuarioFinalId > 1) && (equipo_original.SinTecnicoPreId))
            {
                equipo_bd.SubEstadoId = 7;                                      // Equipo Preparado
                //equipo_bd.SubEstadoId = 9;                                    // Equipo para Retiro
                equipo_bd.TecnicoPreId = 5;                                     // 5: No requerido
                //equipo_bd.TecnicoEntId = 5;                                     // 5: No requerido
                //equipo_original.TecnicoPreId = 5;                             // 5: No requerido
            }
            else if ((equipo_bd.ProyectoId > 0) && (equipo_original.SinTecnicoPreId))
            {
                equipo_bd.SubEstadoId = 8;                                      // Equipo sin Usuario Final
                equipo_bd.TecnicoPreId = 5;                                     // 5: No requerido
                //equipo_original.SubEstadoId = 8;                              // Equipo sin Usuario Final
                //equipo_original.TecnicoPreId = 5;                             // 5: No requerido
            }

            // 4 -> 7
            else if ((equipo_bd.ProyectoId > 0) && (equipo_bd.TecnicoPreId > 1) && (equipo_bd.UsuarioFinalId > 1) && (equipo_original.ListoParaEntrega))
            {
                // En ModificarEquipo no puede seleccionar check ListoParaEntrega sin tener UF (usuario final)
                equipo_bd.SubEstadoId = 7;                                      // Equipo Preparado
                //equipo_original.SubEstadoId = 4;                              // Equipo en Preparación
            }
            else if ((equipo_bd.ProyectoId > 0) && (equipo_bd.TecnicoPreId > 1))
            {
                equipo_bd.SubEstadoId = 4;                                      // Equipo en Preparación
                //equipo_original.SubEstadoId = 4;                              // Equipo en Preparación
            }

            else if ((equipo_bd.ProyectoId > 0) && (equipo_bd.UsuarioFinalId > 1))
            {
                equipo_bd.SubEstadoId = 6;                                // Equipo sin Técnico
                //equipo_original.SubEstadoId = 6;                            // Equipo sin Técnico
            }
            else if (equipo_bd.ProyectoId > 0)
            {
                equipo_bd.SubEstadoId = 5;                                // Equipo con Datos Básicos
                //equipo_original.SubEstadoId = 5;                            // Equipo con Datos Básicos
            }
            else
            {
                equipo_bd.SubEstadoId = 1;                            // Equipos Ingresados
            }
            //}

            return equipo_bd;
        }


        public ActionResult CrearModificarGuia(oGuia guia_original, int accion)
        {
            // Accion ======> 1: crear; 2: modificar

            var errors = ModelState.Values.SelectMany(v => v.Errors);
            string messages_errors = "Error al Crear/Modificar Guia: ";            
            //string nombreProveedor = string.Empty, NombreMarca = string.Empty, NumeroGuia = string.Empty, NombreEstadoEquipo = string.Empty, NombreSubEstadoEquipo = string.Empty;
            int resultadoGuia = 0, resultadoEquipo = 0;

            string NombreTipoEquipo = string.Empty;
            
            if (ModelState.IsValid)
            {
                Guia guia_bd = new Guia();
                guia_bd = ConstruirGuia(guia_original);

                //if (guia_bd.Proveedor != null)
                //    nombreProveedor = guia_bd.Proveedor.Nombre;

                guia_bd.Proveedor = null;

                // objetounicoeneluniverso.ColaboradorEmail = emailColaborador;

                using (InventarioSoporteSistemas_desEntities bd_iss = new InventarioSoporteSistemas_desEntities())
                {
                    try
                    {
                        if (accion == 1)                                 // CREAR... 
                        {
                            // Crear la GUÍA
                            bd_iss.Guia.Add(guia_bd);
                        }
                        else if (accion == 2)                            // MODIFICAR... 
                        {
                            // Sin modificacion de guia por el momento
                            // bd_iss.Entry(guia_bd).State = System.Data.Entity.EntityState.Modified;
                        }
                        else
                        {
                            // otra opcion no considerada por el momento
                        }

                        resultadoGuia = bd_iss.SaveChanges();
                        
                        if (resultadoGuia > 0)
                        {
                            guia_original.GuiaId = guia_bd.GuiaId;
                            Equipo equipo_bd;

                            // Crear EQUIPOS

                            foreach (oEquipo equipo_original in guia_original.equipos)
                            {
                                if (!string.IsNullOrEmpty(equipo_original.NumeroSerie) && !string.IsNullOrEmpty(equipo_original.ModeloNombre))
                                {
                                    equipo_bd = new Equipo();

                                    equipo_original.GuiaId = guia_bd.GuiaId;

                                    equipo_bd = ConstruirEquipo(equipo_original);

                                    //if (equipo_bd.TipoEquipo != null)
                                    //    NombreTipoEquipo = equipo_bd.TipoEquipo.Nombre;
                                    //if (equipo_bd.Marca != null)
                                    //    NombreMarca = equipo_bd.Marca.Nombre;
                                    //if (equipo_bd.EstadoEquipo != null)
                                    //    NombreEstadoEquipo = equipo_bd.EstadoEquipo.Nombre;
                                    //if (equipo_bd.SubEstadoEquipo != null)
                                    //    NombreSubEstadoEquipo = equipo_bd.SubEstadoEquipo.Nombre;
                                    //if (equipo_bd.Guia != null)
                                    //    NumeroGuia = equipo_bd.Guia.NumeroGuia.ToString();

                                    equipo_bd.TipoEquipo = null;
                                    equipo_bd.Marca = null;
                                    equipo_bd.EstadoEquipo = null;
                                    equipo_bd.SubEstadoEquipo = null;
                                    equipo_bd.Guia = null;
                                    equipo_bd.MacroEmpresa = null;
                                    equipo_bd.Empresa = null;
                                    equipo_bd.CentroCosto = null;
                                    equipo_bd.Ubicacion = null;
                                    equipo_bd.CentroCosto = null;
                                    equipo_bd.Comuna = null;
                                    equipo_bd.Direccion = null;
                                    equipo_bd.Proyecto = null;
                                    equipo_bd.SistemaOperativo = null;
                                    equipo_bd.Distribucion = null;

                                    // Grabar Equipo en la BD

                                    bd_iss.Equipo.Add(equipo_bd);
                                    resultadoEquipo = bd_iss.SaveChanges();
                                    
                                    if (resultadoEquipo > 0)
                                    {
                                        equipo_original.EquipoId = equipo_bd.EquipoId;

                                        TempData["exito"] = "Equipo " + equipo_original.NumeroSerie + " (" + equipo_original.EquipoId.ToString() + ") de guía " + guia_original.NumeroGuia.ToString() + " (" + guia_original.GuiaId.ToString() + ") creado. ";
                                    }
                                    else
                                    {
                                        TempData["error"] = messages_errors + " NO fue creado el Equipo " + equipo_original.NumeroSerie + " de guía " + guia_original.NumeroGuia.ToString() + " (" + guia_original.GuiaId.ToString() + "). ";

                                        // PENDIENTE: que hacer en caso que no se grabe uno de los equipos?
                                    }
                                    
                                    equipo_bd = null;
                                }
                                else
                                {

                                }
                            }
                            
                            TempData["exito"] = "Guía " + guia_original.GuiaId.ToString() + " creada";
                            return RedirectToAction("EquiposIngresados", "ISS", new { @id = guia_original.GuiaId });
                        }
                        else
                        {
                            TempData["error"] = messages_errors + "No fue creado/modificado el registro";
                            return RedirectToAction("Error", "ISS");
                        }
                    }
                    catch (DbEntityValidationException ex)
                    {
                        foreach (DbEntityValidationResult item in ex.EntityValidationErrors)
                        {
                            // Get entry

                            DbEntityEntry entry = item.Entry;
                            string entityTypeName = entry.Entity.GetType().Name;

                            // Display or log error messages

                            foreach (DbValidationError subItem in item.ValidationErrors)
                            {
                                messages_errors = messages_errors + string.Format("Error '{0}' occurrió en {1} a {2}" + "\r\n",
                                         subItem.ErrorMessage, entityTypeName, subItem.PropertyName);
                            }
                        }

                        TempData["error"] = messages_errors;
                        return RedirectToAction("Error", "ISS");
                    }
                }
            }
            else
            {
                TempData["error"] = messages_errors + string.Join("; ", ModelState.Values
                                    .SelectMany(x => x.Errors)
                                    .Select(x => x.ErrorMessage));
                return RedirectToAction("Error", "ISS");
            }
        }


        public int ObtenerDistribucion(oEquipo equipo_original)
        {
            oDistribucion dis_original = new oDistribucion();
            oDistribucion dis_nueva = new oDistribucion();

            // Obtener (1) y construir (2) registro

            int id_distribucion = 1;

            if (!string.IsNullOrEmpty(equipo_original.QuienRecibeEmail))
            {
                id_distribucion = ObtenerIdDistribucion(equipo_original.QuienRecibeEmail, null, null);

                dis_original.tipo = "entrega";
                dis_original.email = equipo_original.QuienRecibeEmail;
            }
            else if (!string.IsNullOrEmpty(equipo_original.QuienRetiraRut))
            {
                id_distribucion = ObtenerIdDistribucion(null, equipo_original.QuienRetiraRut, null);

                dis_original.tipo = "retira";
                dis_original.email = equipo_original.QuienRetiraEmail;
                dis_original.rut = equipo_original.QuienRetiraRut;
                dis_original.nombre = equipo_original.QuienRetiraNombre;
                dis_original.id = id_distribucion;
            }
            else if (!string.IsNullOrEmpty(equipo_original.QuienRetiraEmail))
            {
                // pendiente
            }

            if (id_distribucion == 0)
            {
                // 3.- crear registro en tabla DISTRIBUCION y asignar ID 

                dis_nueva = CrearModificarDistribucionEnBD(dis_original, id_distribucion);

                if (dis_nueva.id == 0)
                {
                    // Error al crear la Distribucion
                    id_distribucion = 1;                                 // 1: Sin Distribución
                }
                else
                {
                    id_distribucion = dis_nueva.id;
                }

                //equipo_bd.DistribucionId = id_distribucion;
                //equipo_bd.Distribucion = bd_iss.Distribucion.Where(di => di.DistribucionId == id_distribucion).FirstOrDefault();
                return id_distribucion;
            }
            else
            {
                dis_nueva = CrearModificarDistribucionEnBD(dis_original, id_distribucion);

                if (dis_nueva.id == 0)
                {
                    // Error al modificar la Distribucion, seguirá con los valores originales
                }
                else
                {
                    id_distribucion = dis_nueva.id;
                }

                //equipo_bd.DistribucionId = id_distribucion;
                //equipo_bd.Distribucion = bd_iss.Distribucion.Where(di => di.DistribucionId == id_distribucion).FirstOrDefault();

                return id_distribucion;
            }
        }


        public oEquipo ObtenerEquipo(int idequipo)                                  // desde BD a la App. 
        {
            string aux = string.Empty;

            oEquipo equipo_app = IniciarEquipo();

            using (InventarioSoporteSistemas_desEntities bd_iss = new InventarioSoporteSistemas_desEntities())
            {
                Equipo equipo_bd = bd_iss.Equipo.Where(e => e.EquipoId == idequipo).FirstOrDefault();

                if ((equipo_bd != null) && !equipo_bd.EquipoId.Equals(null))
                {
                    equipo_app.EquipoId = equipo_bd.EquipoId;

                    if(equipo_bd.GuiaId.HasValue)
                    {
                        equipo_app.GuiaId = equipo_bd.GuiaId;
                        equipo_app.NumeroGuia = equipo_bd.Guia.NumeroGuia.ToString();
                        equipo_app.FechaGuia = equipo_bd.Guia.FechaGuia;
                        equipo_app.OrdenCompra = equipo_bd.Guia.OrdenCompra;
                        equipo_app.ProveedorId = equipo_bd.Guia.ProveedorId;
                        equipo_app.ProveedorNombre = equipo_bd.Guia.Proveedor.Nombre;
                    }

                    if (equipo_bd.TipoId.HasValue)
                    {
                        equipo_app.TipoId = equipo_bd.TipoId;
                        equipo_app.TipoNombre = equipo_bd.TipoEquipo.Nombre;
                    }

                    if (equipo_bd.MarcaId.HasValue)
                    {
                        equipo_app.MarcaId = equipo_bd.MarcaId;
                        equipo_app.MarcaNombre = equipo_bd.Marca.Nombre;
                    }

                    if (!string.IsNullOrEmpty(equipo_bd.Modelo))
                        equipo_app.ModeloNombre = equipo_bd.Modelo;

                    if (!string.IsNullOrEmpty(equipo_bd.Serie))
                        equipo_app.NumeroSerie = equipo_bd.Serie;

                    if(equipo_bd.Bolso.HasValue)
                        equipo_app.Bolso = Convert.ToBoolean(equipo_bd.Bolso.Value);

                    if (equipo_bd.Mouse.HasValue)
                        equipo_app.Mouse = Convert.ToBoolean(equipo_bd.Mouse.Value);

                    if (equipo_bd.Candado.HasValue)
                        equipo_app.Candado = Convert.ToBoolean(equipo_bd.Candado.Value);

                    if (!string.IsNullOrEmpty(equipo_bd.Comentario))
                        equipo_app.Comentario = equipo_bd.Comentario;

                    equipo_app.FechaCreacion = equipo_bd.FechaCreacion.Value;

                    if (equipo_bd.CreadorId.HasValue)
                    {
                        equipo_app.CreadorId = equipo_bd.CreadorId;
                        equipo_app.CreadorNombre = equipo_bd.Colaborador.NombreCompleto;
                        equipo_app.CreadorEmail = equipo_bd.Colaborador.Email;
                        equipo_app.CreadorIdRed = equipo_bd.Colaborador.NickName;
                    }

                    if (equipo_bd.TecnicoPreId.HasValue)
                    {
                        equipo_app.TecnicoPreId = equipo_bd.TecnicoPreId;
                        equipo_app.TecnicoPreNombre = equipo_bd.Colaborador2.NombreCompleto;
                        equipo_app.TecnicoPreIdRed = equipo_bd.Colaborador2.NickName;

                        if (equipo_bd.TecnicoPreId == 1)
                        {
                            equipo_app.TecnicoPreEmail = string.Empty;
                            equipo_app.SinTecnicoPreId = false;
                        }
                        else if (equipo_bd.TecnicoPreId == 5)
                        {
                            equipo_app.TecnicoPreEmail = string.Empty;
                            equipo_app.SinTecnicoPreId = true;
                        }
                        else
                        {
                            equipo_app.TecnicoPreEmail = equipo_bd.Colaborador2.Email;
                            equipo_app.SinTecnicoPreId = false;
                        }
                    }

                    if (equipo_bd.TecnicoEntId.HasValue)
                    {
                        equipo_app.TecnicoEntId = equipo_bd.TecnicoEntId;
                        equipo_app.TecnicoEntNombre = equipo_bd.Colaborador1.NombreCompleto;
                        equipo_app.TecnicoEntIdRed = equipo_bd.Colaborador1.NickName;

                        if (equipo_bd.TecnicoEntId == 1)
                        {
                            equipo_app.TecnicoEntEmail = string.Empty;
                            equipo_app.SinTecnicoEntId = false;
                            equipo_app.Distribucion = "pendiente";
                        }
                        else if (equipo_bd.TecnicoEntId == 5)
                        {
                            equipo_app.TecnicoEntEmail = string.Empty;
                            equipo_app.SinTecnicoEntId = true;
                            equipo_app.Distribucion = "retira";
                        }
                        else
                        {
                            equipo_app.TecnicoEntEmail = equipo_bd.Colaborador1.Email;
                            equipo_app.SinTecnicoEntId = false;
                            equipo_app.Distribucion = "entrega";
                        }
                    }
                    
                    if (equipo_bd.EstadoId.HasValue)
                    {
                        equipo_app.EstadoId = equipo_bd.EstadoId;
                        equipo_app.EstadoNombre = equipo_bd.EstadoEquipo.Nombre;
                    }

                    if (equipo_bd.SubEstadoId.HasValue)
                    {
                        equipo_app.SubEstadoId = equipo_bd.SubEstadoId;
                        equipo_app.SubEstadoNombre = equipo_bd.SubEstadoEquipo.Nombre;
                    }


                    if ((equipo_bd.SubEstadoId.HasValue) && (equipo_bd.TecnicoPreId.HasValue))
                    {
                        if ((equipo_bd.TecnicoPreId > 1) && (equipo_bd.TecnicoPreId != 5))
                            if ((equipo_bd.SubEstadoId.Value == 7) || (equipo_bd.SubEstadoId.Value == 9) || (equipo_bd.SubEstadoId.Value == 10) || (equipo_bd.SubEstadoId.Value == 11))
                            {
                                equipo_app.ListoParaEntrega = true;
                            }
                    }
                    

                    if ((equipo_bd.SubEstadoId.HasValue) && (equipo_bd.TecnicoEntId.HasValue))
                    {
                        if (equipo_bd.SubEstadoId.Value == 11)
                            if ((equipo_bd.TecnicoEntId > 1) && (equipo_bd.TecnicoEntId != 5))
                            {
                                equipo_app.EquipoEntregado = true;
                            }
                            else if (equipo_bd.TecnicoEntId == 5)
                            {       
                                equipo_app.EquipoRetirado = true;
                            }
                    }


                    if (equipo_bd.MacroEmpresaId.HasValue)
                    {
                        equipo_app.MacroEmpresaId = equipo_bd.MacroEmpresaId;
                        equipo_app.MacroEmpresaNombre = equipo_bd.MacroEmpresa.MacroEmpresaNombre;
                    }

                    if (equipo_bd.EmpresaId.HasValue)
                    {
                        equipo_app.EmpresaId = equipo_bd.EmpresaId;
                        equipo_app.EmpresaRazonSocial = equipo_bd.Empresa.RazonSocial;
                        equipo_app.EmpresaRut = equipo_bd.Empresa.Rut;
                    }

                    if (equipo_bd.CentroCostoId.HasValue)
                    {
                        equipo_app.CentroCostoId = equipo_bd.CentroCostoId;
                        equipo_app.CentroCostoNombre = equipo_bd.CentroCosto.CCCodigo;
                    }

                    if (equipo_bd.UbicacionId.HasValue)
                    {
                        equipo_app.UbicacionId = equipo_bd.UbicacionId;
                        equipo_app.UbicacionNombre = equipo_bd.Ubicacion.UbicacionNombre;
                    }

                    if (equipo_bd.ProyectoId.HasValue)
                    {
                        equipo_app.ProyectoId = equipo_bd.ProyectoId;
                        equipo_app.ProyectoNombre = equipo_bd.Proyecto.ProyectoNombre;
                    }

                    if (equipo_bd.ComunaId.HasValue)
                    {
                        equipo_app.ComunaId = equipo_bd.ComunaId;
                        equipo_app.ComunaNombre = equipo_bd.Comuna.ComunaNombre;
                    }

                    if (equipo_bd.DireccionId.HasValue)
                    {
                        equipo_app.DireccionId = equipo_bd.DireccionId;
                        equipo_app.DireccionNombre = equipo_bd.Direccion.DireccionNombre;
                    }
                    

                    if (equipo_bd.GerenteAutorizaId.HasValue)
                    {
                        equipo_app.GerenteId = equipo_bd.GerenteAutorizaId;
                        equipo_app.GerenteNombre = equipo_bd.Usuario.NombreCompleto;
                        equipo_app.GerenteEmail = equipo_bd.Usuario.Email;
                        equipo_app.GerenteIdRed = equipo_bd.Usuario.NickName;
                    }

                    if (equipo_bd.SolicitanteId.HasValue)
                    {
                        equipo_app.SolicitanteId = equipo_bd.SolicitanteId;
                        equipo_app.SolicitanteNombre = equipo_bd.Usuario1.NombreCompleto;
                        equipo_app.SolicitanteEmail = equipo_bd.Usuario1.Email;
                        equipo_app.SolicitanteIdRed = equipo_bd.Usuario1.NickName;
                    }

                    if (equipo_bd.UsuarioFinalId.HasValue)
                    {
                        equipo_app.UsuarioFinalId = equipo_bd.UsuarioFinalId;
                        equipo_app.UsuarioFinalNombre = equipo_bd.Usuario2.NombreCompleto;
                        equipo_app.UsuarioFinalEmail = equipo_bd.Usuario2.Email;
                        equipo_app.UsuarioFinalIdRed = equipo_bd.Usuario2.NickName;
                    }


                    if (!string.IsNullOrEmpty(equipo_bd.Procesador))
                        equipo_app.Procesador = equipo_bd.Procesador;

                    if (!string.IsNullOrEmpty(equipo_bd.Memoria))
                        equipo_app.Memoria = equipo_bd.Memoria;

                    if (!string.IsNullOrEmpty(equipo_bd.DiscoDuro))
                        equipo_app.DiscoDuro = equipo_bd.DiscoDuro;

                    if (!string.IsNullOrEmpty(equipo_bd.DireccionMAC))
                        equipo_app.DireccionMAC = equipo_bd.DireccionMAC;

                    if (!string.IsNullOrEmpty(equipo_bd.DireccionIP))
                        equipo_app.DireccionIP = equipo_bd.DireccionIP;

                    if (!string.IsNullOrEmpty(equipo_bd.NombrePC))
                        equipo_app.NombrePC = equipo_bd.NombrePC;

                    if (!string.IsNullOrEmpty(equipo_bd.KeyWindows))
                        equipo_app.KeyWindows = equipo_bd.KeyWindows;

                    if (equipo_bd.SistemaOperativoId.HasValue)
                    {
                        equipo_app.SistemaOperativoId = equipo_bd.SistemaOperativoId;
                        equipo_app.SistemaOperativoNombre = equipo_bd.SistemaOperativo.SONombre;
                    }


                    if (equipo_bd.DistribucionId.HasValue)
                    {
                        equipo_app.DistribucionId = equipo_bd.DistribucionId;

                        //if(equipo_app.DistribucionId > 1)
                        //{

                        if (equipo_app.Distribucion.Equals("entrega"))
                        {
                            // carga la entrega

                            equipo_app.FechaDistribucion = equipo_bd.FechaDistribucion;
                            equipo_app.FechaEntrega = equipo_bd.FechaDistribucion;
                            equipo_app.QuienRecibeEmail = equipo_bd.EmailDistribucion;

                            if (equipo_app.DistribucionId > 1)
                                equipo_app.QuienRecibeEmail = equipo_bd.Distribucion.Email;

                            // limpia el retiro

                            equipo_app.FechaRetira = null;
                            equipo_app.QuienRetiraEmail = null;
                            equipo_app.QuienRetiraNombre = null;
                            equipo_app.QuienRetiraRut = null;
                        }
                        else if (equipo_app.Distribucion.Equals("retira"))
                        {
                            // carga el retiro

                            equipo_app.FechaDistribucion = equipo_bd.FechaDistribucion;
                            equipo_app.FechaRetira = equipo_bd.FechaDistribucion;
                            equipo_app.QuienRetiraEmail = equipo_bd.EmailDistribucion;

                            if (equipo_app.DistribucionId > 1)
                            {
                                equipo_app.QuienRetiraEmail = equipo_bd.Distribucion.Email;
                                equipo_app.QuienRetiraNombre = equipo_bd.Distribucion.Nombre;
                                equipo_app.QuienRetiraRut = equipo_bd.Distribucion.Rut;
                            }

                            // limpia la entrega

                            equipo_app.TecnicoEntEmail = null;
                            equipo_app.SinTecnicoEntId = false;
                            equipo_app.TecnicoEntId = 5;

                            equipo_app.FechaEntrega = null;
                            equipo_app.QuienRecibeEmail = null;
                        }
                        else
                        {

                        }

                        //}
                    }

                }
                else
                {
                    equipo_bd.EquipoId = 0;
                }
            }

            return equipo_app;
        }


        public oEquipo ActualizarEquipo(oEquipo equipo_original, oEquipo equipo_nuevo)            // En la App. 
        {
            if (equipo_nuevo.MacroEmpresaId.HasValue)
            {
                equipo_original.MacroEmpresaId = equipo_nuevo.MacroEmpresaId;
            }

            if (!string.IsNullOrEmpty(equipo_nuevo.EmpresaRazonSocial))
            {
                equipo_original.EmpresaRazonSocial = equipo_nuevo.EmpresaRazonSocial;
            }

            if (!string.IsNullOrEmpty(equipo_nuevo.CentroCostoNombre))
            {
                equipo_original.CentroCostoNombre = equipo_nuevo.CentroCostoNombre;
            }

            if (equipo_nuevo.UbicacionId.HasValue)
            {
                equipo_original.UbicacionId = equipo_nuevo.UbicacionId;
            }

            if (!string.IsNullOrEmpty(equipo_nuevo.ProyectoNombre))
            {
                equipo_original.ProyectoNombre = equipo_nuevo.ProyectoNombre;
            }

            if (!string.IsNullOrEmpty(equipo_nuevo.ComunaNombre))
            {
                equipo_original.ComunaNombre = equipo_nuevo.ComunaNombre;
            }

            if (!string.IsNullOrEmpty(equipo_nuevo.DireccionNombre))
            {
                equipo_original.DireccionNombre = equipo_nuevo.DireccionNombre;
            }


            if (!string.IsNullOrEmpty(equipo_nuevo.GerenteEmail))
                //equipo_original.GerenteId = ObtenerIdUsuario(equipo_nuevo.GerenteEmail);
                equipo_original.GerenteEmail = equipo_nuevo.GerenteEmail;

            if (!string.IsNullOrEmpty(equipo_nuevo.SolicitanteEmail))
                //equipo_original.SolicitanteId = ObtenerIdUsuario(equipo_nuevo.SolicitanteEmail);
                equipo_original.SolicitanteEmail = equipo_nuevo.SolicitanteEmail;

            if (!string.IsNullOrEmpty(equipo_nuevo.UsuarioFinalEmail))
                //equipo_original.UsuarioFinalId = ObtenerIdUsuario(equipo_nuevo.UsuarioFinalEmail);
                equipo_original.UsuarioFinalEmail = equipo_nuevo.UsuarioFinalEmail;

            //if (!string.IsNullOrEmpty(equipo_nuevo.TecnicoPreEmail))
            //    //equipo_original.TecnicoPreId = ObtenerIdColaborador(equipo_nuevo.TecnicoPreEmail);
            //    equipo_original.TecnicoPreEmail = equipo_nuevo.TecnicoPreEmail;

            //if (equipo_nuevo.TecnicoPreId.HasValue)
            //    if (equipo_nuevo.TecnicoPreId != 5)
            //        equipo_original.SinTecnicoPreId = equipo_nuevo.SinTecnicoPreId;


            // Para Técnico de Preparación existe botón SinTecnicoPreId

            if (equipo_nuevo.SinTecnicoPreId)
            {
                equipo_original.SinTecnicoPreId = equipo_nuevo.SinTecnicoPreId;
                equipo_original.TecnicoPreId = 5;
            }
            else
            {
                if (!string.IsNullOrEmpty(equipo_nuevo.TecnicoPreEmail))
                    //equipo_original.TecnicoPreId = ObtenerIdColaborador(equipo_nuevo.TecnicoPreEmail);
                    equipo_original.TecnicoPreEmail = equipo_nuevo.TecnicoPreEmail;
            }

            if (!string.IsNullOrEmpty(equipo_nuevo.Distribucion))
            {
                equipo_original.Distribucion = equipo_nuevo.Distribucion;
            }


            // Entrega

            if (!string.IsNullOrEmpty(equipo_nuevo.TecnicoEntEmail))
            {
                equipo_original.TecnicoEntEmail = equipo_nuevo.TecnicoEntEmail;
            }

            if (equipo_nuevo.FechaEntrega.HasValue)
            {
                equipo_original.FechaEntrega = equipo_nuevo.FechaEntrega;
            }

            if (!string.IsNullOrEmpty(equipo_nuevo.QuienRecibeEmail))
            {
                equipo_original.QuienRecibeEmail = equipo_nuevo.QuienRecibeEmail;
            }


            // Retira

            if (equipo_nuevo.FechaRetira.HasValue)
            {
                equipo_original.FechaRetira = equipo_nuevo.FechaRetira;
            }

            if (!string.IsNullOrEmpty(equipo_nuevo.QuienRetiraEmail))
            {
                equipo_original.QuienRetiraEmail = equipo_nuevo.QuienRetiraEmail;
            }

            if (!string.IsNullOrEmpty(equipo_nuevo.QuienRetiraRut))
            {
                equipo_original.QuienRetiraRut = equipo_nuevo.QuienRetiraRut;
            }

            if (!string.IsNullOrEmpty(equipo_nuevo.QuienRetiraNombre))
            {
                equipo_original.QuienRetiraNombre = equipo_nuevo.QuienRetiraNombre;
            }



            //if (equipo_nuevo.Distribucion.Equals("entrega"))
            //{
            //    equipo_original.SinTecnicoEntId = false;
            //    equipo_original.Distribucion = equipo_nuevo.Distribucion;
            //}
            //else if (equipo_nuevo.Distribucion.Equals("retira"))
            //{
            //    equipo_original.TecnicoEntId = 5;
            //    equipo_original.SinTecnicoEntId = true;
            //    equipo_original.Distribucion = equipo_nuevo.Distribucion;
            //}
            //else 
            //{
            //    equipo_original.TecnicoEntId = 1;
            //    equipo_original.SinTecnicoEntId = false;                
            //    equipo_original.Distribucion = equipo_nuevo.Distribucion;
            //}


            //if (equipo_nuevo.SinTecnicoEntId)
            //{
            //    equipo_original.SinTecnicoEntId = equipo_nuevo.SinTecnicoEntId;
            //    equipo_original.TecnicoEntId = 5;
            //    equipo_original.Distribucion = "retira";
            //}
            //else
            //{
            //    if (!string.IsNullOrEmpty(equipo_nuevo.TecnicoEntEmail))
            //    {
            //        equipo_original.TecnicoEntEmail = equipo_nuevo.TecnicoEntEmail;
            //        equipo_original.Distribucion = "entrega";
            //    }
            //}


            if (equipo_nuevo.EquipoEntregado)
            {
                equipo_original.EquipoEntregado = equipo_nuevo.EquipoEntregado;
            }
            else if (equipo_nuevo.EquipoRetirado)
            {
                equipo_original.EquipoRetirado = equipo_nuevo.EquipoRetirado;
            }


            if (equipo_nuevo.SubEstadoId.HasValue)
            {
                equipo_original.SubEstadoId = equipo_nuevo.SubEstadoId;
            }


            return equipo_original;
        }



        public ActionResult Generar_PDF(string numeroserie, string tipo)
        {
            // tipo     =>      C: contrato, H: hoja de entrega
            
            string error = string.Empty;
            string revision1 = string.Empty;
            StringBuilder revision2 = new StringBuilder(string.Empty);
            string pdf_sin_nro_pagina = string.Empty;
            string resultado = string.Empty;
            byte[] bytes;
            oHito existeequipo = new oHito();

            //string directorio_inicial = @"C:\inetpub\Inventario_des\Contratos\";
            //string directorio_inicial = @"C:\Users\vsandoval\Documents\Codigo\VS2017\Inventario\Inventario_des\Contratos\";
            string directorio_inicial = Server.MapPath("~/Documentos/");
            string imagepath = Server.MapPath("~/Content/Images/");

            iTextSharp.text.Image LogoEmpresasSocovesaJPG = iTextSharp.text.Image.GetInstance(imagepath + "LogoEmpresasSocovesa.jpg");
            LogoEmpresasSocovesaJPG.ScalePercent(25f);


            //string stringstrAttachment0 = Server.MapPath("/");
            //string stringstrAttachment1 = Server.MapPath(".");
            //string stringstrAttachment2 = Server.MapPath("..");
            //string stringstrAttachment3 = Server.MapPath("~/Contratos/");
            //string stringstrAttachment4 = Server.MapPath("~");
            //string stringstrAttachment5 = Server.MapPath("../Contratos/");
            //string stringstrAttachment7 = HttpContext.Request.PhysicalApplicationPath;

            revision2.Append("Tipo Documento: ");
            revision2.Append(tipo);
            revision2.Append("\r\n");

            string extension = DateTime.Now.ToString("dd'-'MM'-'yyyy'H'HH':'mm':'ss");
            extension = extension.Replace(":", "");
            extension = extension.Replace("-", "");
            extension = "-" + tipo + extension;

            resultado = directorio_inicial + numeroserie + extension;

            string filein = resultado + ".pdf";
            string directorio_revision = resultado + "-revision.txt";

            if (!string.IsNullOrEmpty(numeroserie))
            {
                revision2.Append("Numero de Serie: ");
                revision2.Append(numeroserie);
                revision2.Append("\r\n");
                //revision = revision + "Numero de Serie: " + numeroserie + "\r\n";

                existeequipo = ObtenerIdEquipoPorNumeroSerie(numeroserie);

                if (existeequipo.id == 0)
                {
                    revision2.Append(existeequipo.error);
                    revision2.Append("\r\n");
                    //revision = revision + existeequipo.error + "\r\n";
                    System.IO.File.WriteAllText(directorio_revision, revision2.ToString());
                    return File(revision2.ToString(), "text/plain", directorio_revision);
                }
                else
                {
                    revision2.Append(existeequipo.error);
                    revision2.Append("\r\n");
                    //revision = revision + existeequipo.error + "\r\n";

                    oEquipo equipo = ObtenerEquipo(existeequipo.id);

                    revision2.Append("Id Equipo: ");
                    revision2.Append(equipo.EquipoId);
                    revision2.Append("\r\n");
                    //revision = revision + "Id Equipo: " + equipo.EquipoId + "\r\n";

                    //PdfPTable tabla_ingresos = cs_funciones.ConstruirTablaIngresos(Ingresos, borrador);

                    //oFormato formatear = new oFormato();

                    //PdfPTable tabla_firmas = cs_funciones.ConstruirTablaFirmas(Encabezado_inicial);
                    //revision = revision + "Tabla Firmas construida -" + "\r\n";

                    //Encabezado_inicial.nroclausula = 0;

                    //string directorio_aux1 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    //string directorio_aux2 = HttpRuntime.AppDomainAppPath;
                    //string directorio_aux3 = System.AppDomain.CurrentDomain.BaseDirectory;
                    //string directorio_aux5 = System.Environment.CurrentDirectory;
                    //string directorio_aux6 = System.IO.Directory.GetCurrentDirectory();
                    //string directorio_aux7 = Environment.CurrentDirectory;

                    revision2.Append("Directorio: ");
                    revision2.Append(directorio_inicial);
                    revision2.Append("\r\n");
                    //revision = revision + "Directorio: " + directorio_inicial + "\r\n";

                    Paragraph pTitulo = new Paragraph();
                    Paragraph pDocumento = new Paragraph();
                    Paragraph pRastro = new Paragraph();

                    if (tipo.Equals("C"))
                    {
                        pTitulo = ConstruirTituloC();
                        pDocumento = ConstruirContrato(equipo);
                        
                        //pdf_sin_nro_pagina = Crear_PDF(filein, pTituloC, pDocumentoC, LogoEmpresasSocovesaJPG);
                    }
                    else if (tipo.Equals("H"))
                    {
                        pTitulo = ConstruirTituloH();
                        pDocumento = ConstruirHojaEntrega(equipo);
                        
                        //pdf_sin_nro_pagina = Crear_PDF(filein, pTituloH, pDocumentoH, LogoEmpresasSocovesaJPG);
                    }
                    else
                    {
                        revision2.Append("No se reconoce el tipo de documento: ");
                        revision2.Append(tipo);
                        revision2.Append("\r\n");
                        //revision = revision + "Sin Número de Serie " + "\r\n";
                        System.IO.File.WriteAllText(directorio_revision, revision2.ToString());

                        return File(revision2.ToString(), "text/plain", directorio_revision);
                    }
                    
                    
                    pRastro = ConstruirRastro();

                    pTitulo.Alignment = Element.ALIGN_CENTER;
                    pTitulo.SpacingAfter = 9f;
                    pDocumento.Alignment = Element.ALIGN_JUSTIFIED;
                    pDocumento.SpacingAfter = 9f;
                    pRastro.Alignment = Element.ALIGN_RIGHT;

                    pdf_sin_nro_pagina = Crear_PDF(filein, pTitulo, pDocumento, pRastro, LogoEmpresasSocovesaJPG);


                    revision2.Append("PDF generado!");
                    revision2.Append("\r\n");
                    //revision = revision + "PDF generado!" + "\r\n";

                    System.IO.File.WriteAllText(directorio_revision, revision2.ToString());

                    bytes = Crear_BYTE(pdf_sin_nro_pagina, out error);

                    return File(filein, "application/pdf", numeroserie + extension + ".pdf");
                }
            }
            else
            {
                revision2.Append("Sin Número de Serie. ");
                revision2.Append("\r\n");
                //revision = revision + "Sin Número de Serie " + "\r\n";
                System.IO.File.WriteAllText(directorio_revision, revision2.ToString());

                return File(revision2.ToString(), "text/plain", directorio_revision);
            }

        }




        // ACTIONRESULT

        [HttpGet]
        public ActionResult AutoLogin()
        {
            ViewBag.Current = "AutoLogin";

            string idred = System.Security.Principal.WindowsIdentity.GetCurrent().Name;             // Formato idred: Socovesa\\vsandoval

            string error = string.Empty, messages_errors = string.Empty;
            string[] ir;
            oUsuario usuario_sesion = new oUsuario();
            //Colaborador colaborador = new Colaborador();

            if (!string.IsNullOrEmpty(idred))
            {
                ir = idred.ToString().Split('\\');
                string us_ir = ir[1].Trim();
                string do_ir = ir[0].Trim();
                string email_auxiliar = ir[1].Trim() + "@socovesa.cl";

                // 1.- validar que usuario este en el Active Directory
                //usuario_sesion = ObtenerUsuarioDesdeActiveDirectory(email);
                usuario_sesion = ObtenerUsuarioDesdeActiveDirectory(email_auxiliar);

                if (usuario_sesion.id == 0)
                {
                    //error = "Usuario no existe en el Active Directory";
                    error = "Usuario IdRed" + idred + " no existe en el Active Directory (" + usuario_sesion.error + ")";

                    Session["RolId"] = 0;
                    Session["Mensaje"] = error;

                    messages_errors = error;
                    TempData["error"] = messages_errors;
                    return RedirectToAction("LoginError");
                }
                else
                {
                    // 2.- validar si usuario existe en tabla Colaborador de la BD atencionproveedores

                    using (InventarioSoporteSistemas_desEntities bd_iss = new InventarioSoporteSistemas_desEntities())
                    {
                        //Colaborador colaborador = bd_ap.Colaborador.Where(e => e.Nickname.Equals(us_ir) && (!e.RolId.Equals(5))).FirstOrDefault();
                        Colaborador colaborador = bd_iss.Colaborador.Where(e => e.NickName.Equals(us_ir)).FirstOrDefault();

                        if ((colaborador != null) && !colaborador.Id.Equals(null))
                        {
                            // 3.- validar si usuario está ACTIVO

                            if (colaborador.RolId == 5)
                            {
                                error = "Usuario " + colaborador.NickName + " NO está ACTIVO en la app.";

                                Session["RolId"] = 0;
                                Session["Mensaje"] = error;

                                messages_errors = error;
                                TempData["error"] = messages_errors;
                                return RedirectToAction("LoginError");
                            }
                            else
                            {
                                Session["RolId"] = colaborador.RolId;

                                return RedirectToAction("EquiposIngresados", "ISS", new { @idred = idred });
                            }
                        }
                        else
                        {
                            error = "Usuario " + colaborador.NickName + " NO existe en tabla Colaborador de la BD Inventario";

                            Session["RolId"] = 0;
                            Session["Mensaje"] = error;

                            messages_errors = error;
                            TempData["error"] = messages_errors;
                            return RedirectToAction("LoginError");
                        }
                    }
                }
            }
            else
            {
                error = "No fue posible obtener el Id Red " + idred + " del usuario al cargar la aplicación";

                // No tiene idred
                Session["RolId"] = 0;
                Session["Supervisor"] = false;
                Session["Mensaje"] = error;

                messages_errors = error;
                TempData["error"] = messages_errors;
                //return RedirectToAction("ErrorSSS", "SS", new { @id = messages_errors });
                //return RedirectToAction("ErrorSSS", "SS");
                return RedirectToAction("LoginError");
            }
        }
        

        [HttpGet]
        public ActionResult LoginError()
        {
            ViewBag.Current = "LoginError";

            string error = string.Empty, messages_errors = string.Empty;

            if (Session["RolId"] == null)
            {
                if (TempData.ContainsKey("error"))
                {
                    error = TempData["error"].ToString();
                }
                else
                {
                    error = "Accediendo a esta página sin pasar por función AutoLogin";
                }
            }
            else
            {
                if (Session["Mensaje"] == null)
                {
                    error = "Rol Id: " + Session["RolId"].ToString();
                }
                else
                {
                    error = "Error: " + Session["Mensaje"].ToString() + "\r\n - " + " Rol Id: " + Session["RolId"].ToString();
                }
            }

            ViewBag.MensajeError = error;

            return View();
        }

        
        [HttpGet]
        public ActionResult CrearGuia()
        {
            ViewBag.Current = "CrearGuia";

            string messages_errors = string.Empty;

            if (Session["RolId"] == null)
            {
                // Accediendo a esta página sin pasar por función AutoLoginSSS
                messages_errors = "Accediendo a esta página sin pasar por función AutoLogin";

                Session["RolId"] = 0;
                Session["Mensaje"] = messages_errors;
                TempData["error"] = messages_errors;

                return RedirectToAction("LoginError");
            }
            else
            {
                oGuia guia = IniciarGuia();

                IniciarViewbag();
                
                return View(guia);
            }
        }


        [HttpPost]
        public ActionResult CrearGuia(oGuia guia)
        {
            // PENDIENTE: validar que: existan equipos y sus campos 

            if (guia.equipos != null)
            {
                if (guia.equipos.Count > 0)
                {
                    bool validacion_equipo = true; 

                    // Validar que cada equipo tenga tipo, marca, nro. serie y modelo
                    foreach (oEquipo eq in guia.equipos)
                    {
                        if (string.IsNullOrEmpty(eq.TipoNombre) || string.IsNullOrEmpty(eq.MarcaNombre) || string.IsNullOrEmpty(eq.NumeroSerie) || string.IsNullOrEmpty(eq.ModeloNombre))
                        {
                            // Faltan datos
                            validacion_equipo = false;
                        }                        
                    }
                    
                    if(validacion_equipo == true)
                    {
                        return CrearModificarGuia(guia, 1);
                    }
                    else
                    {
                        // Algún equipo NO tiene tipo, marca, nro. serie y/o modelo

                        IniciarViewbag();

                        //return View(guia);
                        return RedirectToAction("CrearGuia", "ISS");
                    }
                }
                else
                {
                    // Guía sin Equipos

                    TempData["error"] = "Está tratando de crear una Guía sin Equipos asociados (0)";
                    return RedirectToAction("Error", "ISS");
                }
            }
            else
            {
                // Guía sin Equipos

                TempData["error"] = "Está tratando de crear una Guía sin Equipos asociados (null)";
                return RedirectToAction("Error", "ISS");
            }
        }
        

        [HttpGet]
        public ActionResult EquiposIngresados2()
        {
            ViewBag.Current = "EquiposIngresados";

            string messages_errors = string.Empty;

            if (Session["RolId"] == null)
            {
                // Accediendo a esta página sin pasar por función AutoLoginSSS
                messages_errors = "Accediendo a esta página sin pasar por función AutoLogin";

                Session["RolId"] = 0;
                Session["Mensaje"] = messages_errors;
                TempData["error"] = messages_errors;

                return RedirectToAction("LoginError");
            }
            else
            {
                List<oEquipo> lista_auxiliar = new List<oEquipo>();
                oTecnicoAsignado tecnico = new oTecnicoAsignado();

                // buscar equipos cuyo tenico preparacion (TecnicoPreId) sea "Sin Asignar" (1)

                using (InventarioSoporteSistemas_desEntities bd_iss = new InventarioSoporteSistemas_desEntities())
                {
                    var listaAuxiliarDesdeBD = bd_iss.Equipo.
                        Where(eq1 => eq1.TecnicoPreId == 1).                                            // 1: sin asignar
                        Join(bd_iss.SubEstadoEquipo, eq2 => eq2.SubEstadoId, se => se.Id, (eq2, se) => new
                        {
                            eq2.EquipoId,
                            eq2.Guia.ProveedorId,
                            eq2.Guia.NumeroGuia,
                            eq2.Guia.OrdenCompra,
                            eq2.TipoId,
                            eq2.MarcaId,
                            eq2.Modelo,
                            eq2.Serie,
                            //se.Nombre,
                            se.Id
                        }).Where(se => se.Id == 1).ToList();                                            // 1: Ingresado

                    if (listaAuxiliarDesdeBD.Count() > 0)
                    {
                        foreach (var msp in listaAuxiliarDesdeBD)
                        {
                            lista_auxiliar.Add(ObtenerEquipo(msp.EquipoId));
                        }
                    }
                    else
                    {
                    }

                    //ViewBag.TotalCabeceras = lista_auxiliar_misssspendientes.Count();
                    //ViewData["cabeceras_MisSSSPendientes"] = lista_auxiliar_misssspendientes;

                    tecnico.equipos = lista_auxiliar;

                    return View(tecnico);
                }
            }
        }


        [HttpPost]
        public ActionResult EquiposIngresados2(oTecnicoAsignado ta)
        {

            bool equipos_actualizados = true;
            string messages_errors = string.Empty;
            oEquipo equipo_aux = new oEquipo();
            Equipo equipo_bd = new Equipo();

            using (InventarioSoporteSistemas_desEntities bd_iss = new InventarioSoporteSistemas_desEntities())
            {
                foreach (oEquipo eq in ta.equipos)
                {
                    if (eq.actualizar)
                    {
                        //equipo_aux = ObtenerEquipo(eq.EquipoId);
                        //equipo_aux.TecnicoPreId = ObtenerIdColaborador(ta.TecnicoEmail);
                        //equipo_aux.SubEstadoId = 4;

                        equipo_bd = ConstruirEquipo(ObtenerEquipo(eq.EquipoId));
                        equipo_bd.TecnicoPreId = ObtenerIdColaborador(ta.TecnicoEmail);
                        equipo_bd.SubEstadoId = 4;

                        equipo_bd.TipoEquipo = null;
                        equipo_bd.Marca = null;
                        equipo_bd.EstadoEquipo = null;
                        equipo_bd.SubEstadoEquipo = null;
                        equipo_bd.Guia = null;
                        equipo_bd.MacroEmpresa = null;
                        equipo_bd.Empresa = null;
                        equipo_bd.CentroCosto = null;
                        equipo_bd.Ubicacion = null;
                        equipo_bd.CentroCosto = null;
                        equipo_bd.Comuna = null;
                        equipo_bd.Direccion = null;
                        equipo_bd.Proyecto = null;
                        equipo_bd.SistemaOperativo = null;
                        equipo_bd.Distribucion = null;

                        bd_iss.Entry(equipo_bd).State = System.Data.Entity.EntityState.Modified;

                        int resultado = bd_iss.SaveChanges();

                        if (resultado > 0)
                        {
                            TempData["exito"] = "Equipo " + eq.NumeroSerie + " (" + eq.EquipoId.ToString() + ") actualizado. ";
                        }
                        else
                        {
                            equipos_actualizados = false;
                            TempData["error"] = messages_errors + " NO fue actualizado el Equipo " + eq.NumeroSerie + " (" + eq.EquipoId.ToString() + "). ";
                        }
                    }
                }
            }

            if (equipos_actualizados)
            {
                TempData["exito"] = "Equipos asignados a " + ta.TecnicoEmail;
                return RedirectToAction("EquiposEnPreparacion", "ISS");
            }
            else
            {
                TempData["error"] = "Algún equipo no actualizado" + messages_errors;
                return RedirectToAction("Error", "ISS");
            }

        }


        [HttpGet]
        public ActionResult ModificarEquipo(string numeroserie)
        {
            ViewBag.Current = "ModificarEquipo";

            string messages_errors = string.Empty;

            if (Session["RolId"] == null)
            {
                // Accediendo a esta página sin pasar por función AutoLoginSSS
                messages_errors = "Accediendo a esta página sin pasar por función AutoLogin";

                Session["RolId"] = 0;
                Session["Mensaje"] = messages_errors;
                TempData["error"] = messages_errors;

                return RedirectToAction("LoginError");
            }
            else
            {
                if (!string.IsNullOrEmpty(numeroserie))
                {
                    //int aux_idequipo = 0;
                    IniciarViewbag();

                    oHito resultado = ObtenerIdEquipoPorNumeroSerie(numeroserie);

                    //aux_idequipo = ObtenerIdEquipo(numeroserie);

                    if (resultado.id > 0)
                    {
                        oEquipo equipo = ObtenerEquipo(resultado.id);

                        return View(equipo);
                    }
                    else
                    {
                        messages_errors = "No fue posible cargar el Equipo con número de serie " + numeroserie.ToString() + ": " + resultado.error;
                        TempData["error"] = messages_errors;
                        return RedirectToAction("Error", "ISS");
                    }
                }
                else
                {
                    messages_errors = "No fue posible cargar el Equipo con número de serie " + numeroserie.ToString() + ", ya que IsNullOrEmpty. ";
                    TempData["error"] = messages_errors;
                    return RedirectToAction("Error", "ISS");
                }
            }
        }


        [HttpPost]
        public ActionResult ModificarEquipo(oEquipo equipo_app)
        {
            string messages_errors = string.Empty;
            Equipo equipo_bd;
            int resultado;

            if (ModelState.IsValid)
            {
                using (InventarioSoporteSistemas_desEntities bd_iss = new InventarioSoporteSistemas_desEntities())
                {
                    try
                    {
                        //equipo_bd = ConstruirEquipo(ObtenerEquipo(eq.EquipoId));
                        equipo_bd = ConstruirEquipo(equipo_app);

                        equipo_bd.TipoEquipo = null;
                        equipo_bd.Marca = null;
                        equipo_bd.EstadoEquipo = null;
                        equipo_bd.SubEstadoEquipo = null;
                        equipo_bd.Guia = null;
                        equipo_bd.MacroEmpresa = null;
                        equipo_bd.Empresa = null;
                        equipo_bd.CentroCosto = null;
                        equipo_bd.Ubicacion = null;
                        equipo_bd.CentroCosto = null;
                        equipo_bd.Comuna = null;
                        equipo_bd.Direccion = null;
                        equipo_bd.Proyecto = null;
                        equipo_bd.SistemaOperativo = null;
                        equipo_bd.Distribucion = null;

                        bd_iss.Entry(equipo_bd).State = System.Data.Entity.EntityState.Modified;
                        resultado = bd_iss.SaveChanges();


                        if (resultado > 0)
                        {
                            messages_errors = "¡Equipo " + equipo_app.NumeroSerie.ToString() + " actualizado!";
                            TempData["exito"] = messages_errors;
                            return RedirectToAction("ModificarEquipo", "ISS", new { @numeroserie = equipo_app.NumeroSerie.ToString() });
                        }
                        else
                        {
                            TempData["error"] = messages_errors + "No fue modificado el registro";
                            return RedirectToAction("Error", "ISS");
                        }
                        

                    }
                    catch (DbEntityValidationException ex)
                    {
                        foreach (DbEntityValidationResult item in ex.EntityValidationErrors)
                        {
                            // Get entry

                            DbEntityEntry entry = item.Entry;
                            string entityTypeName = entry.Entity.GetType().Name;

                            // Display or log error messages

                            foreach (DbValidationError subItem in item.ValidationErrors)
                            {
                                messages_errors = messages_errors + string.Format("Error '{0}' occurrió en {1} a {2}" + "\r\n",
                                         subItem.ErrorMessage, entityTypeName, subItem.PropertyName);
                            }
                        }

                        TempData["error"] = messages_errors;
                        return RedirectToAction("Error", "ISS");
                    }
                }
            }
            else
            {
                TempData["error"] = messages_errors + string.Join("; ", ModelState.Values
                                    .SelectMany(x => x.Errors)
                                    .Select(x => x.ErrorMessage));
                return RedirectToAction("Error", "ISS");
            }
        }


        [HttpGet]
        public ActionResult EquiposIngresados()
        {
            ViewBag.Current = "EquiposIngresados";

            string messages_errors = string.Empty;

            if (Session["RolId"] == null)
            {
                // Accediendo a esta página sin pasar por función AutoLoginSSS
                messages_errors = "Accediendo a esta página sin pasar por función AutoLogin";

                Session["RolId"] = 0;
                Session["Mensaje"] = messages_errors;
                TempData["error"] = messages_errors;

                return RedirectToAction("LoginError");
            }
            else
            {
                List<oEquipo> lista_auxiliar = new List<oEquipo>();
                oEquipoDistribuido equipo_distribuido = new oEquipoDistribuido();
                oEquipo equipo;

                IniciarViewbag();

                //oTecnicoAsignado tecnico = new oTecnicoAsignado();

                // buscar equipos cuyo tenico preparacion (TecnicoPreId) sea "Sin Asignar" (1)

                using (InventarioSoporteSistemas_desEntities bd_iss = new InventarioSoporteSistemas_desEntities())
                {
                    var listaAuxiliarDesdeBD = bd_iss.Equipo.
                        Where(eq1 => eq1.TecnicoPreId == 1).                                            // 1: sin asignar
                        Join(bd_iss.SubEstadoEquipo, eq2 => eq2.SubEstadoId, se => se.Id, (eq2, se) => new
                        {
                            eq2.EquipoId,
                            eq2.Guia.ProveedorId,
                            eq2.Guia.Proveedor.Nombre,
                            eq2.Guia.NumeroGuia,
                            eq2.Guia.OrdenCompra,
                            eq2.TipoId,
                            eq2.MarcaId,
                            eq2.Modelo,
                            eq2.Serie,
                            //se.Nombre,
                            se.Id
                        }).Where(se => se.Id == 1).ToList();                                            // 1: Ingresado

                    if (listaAuxiliarDesdeBD.Count() > 0)
                    {
                        foreach (var msp in listaAuxiliarDesdeBD)
                        {
                            equipo = new oEquipo();
                            equipo = ObtenerEquipo(msp.EquipoId);

                            //lista_auxiliar.Add(ObtenerEquipo(msp.EquipoId));
                            lista_auxiliar.Add(equipo);
                        }
                    }
                    else
                    {
                    }

                    //ViewBag.TotalCabeceras = lista_auxiliar_misssspendientes.Count();
                    //ViewData["cabeceras_MisSSSPendientes"] = lista_auxiliar_misssspendientes;

                    //tecnico.equipos = lista_auxiliar;
                    equipo_distribuido.equipos = lista_auxiliar;

                    return View(equipo_distribuido);
                }
            }
        }


        [HttpPost]
        public ActionResult EquiposIngresados(oEquipoDistribuido ed)
        {
            if(ed.equipos.Count > 0)
            {
                bool algun_equipo_por_actualizar = false, algun_equipos_con_error = false;
                string messages_errors = string.Empty;
                oEquipo equipo_aux = new oEquipo();
                Equipo equipo_bd;

                //int gerenteid = 0, solicitanteid = 0, usuariofinalid = 0, tecnicoid = 0;

                //if (!string.IsNullOrEmpty(ed.equipo.GerenteEmail))
                //    gerenteid = ObtenerIdUsuario(ed.equipo.GerenteEmail);

                //if (!string.IsNullOrEmpty(ed.equipo.SolicitanteEmail))
                //    solicitanteid = ObtenerIdUsuario(ed.equipo.SolicitanteEmail);

                //if (!string.IsNullOrEmpty(ed.equipo.UsuarioFinalEmail))
                //    usuariofinalid = ObtenerIdUsuario(ed.equipo.UsuarioFinalEmail);

                //if (!string.IsNullOrEmpty(ed.equipo.TecnicoPreEmail))
                //    tecnicoid = ObtenerIdColaborador(ed.equipo.TecnicoPreEmail);

                using (InventarioSoporteSistemas_desEntities bd_iss = new InventarioSoporteSistemas_desEntities())
                {
                    foreach (oEquipo eq in ed.equipos)
                    {
                        equipo_bd = new Equipo();

                        if (eq.actualizar)
                        {
                            algun_equipo_por_actualizar = true;

                            equipo_aux = ObtenerEquipo(eq.EquipoId);
                            equipo_aux = ActualizarEquipo(equipo_aux, ed.equipo);

                            //equipo_bd = ConstruirEquipo(ObtenerEquipo(eq.EquipoId));
                            equipo_bd = ConstruirEquipo(equipo_aux);

                            equipo_bd.TipoEquipo = null;
                            equipo_bd.Marca = null;
                            equipo_bd.EstadoEquipo = null;
                            equipo_bd.SubEstadoEquipo = null;
                            equipo_bd.Guia = null;
                            equipo_bd.MacroEmpresa = null;
                            equipo_bd.Empresa = null;
                            equipo_bd.CentroCosto = null;
                            equipo_bd.Ubicacion = null;
                            equipo_bd.CentroCosto = null;
                            equipo_bd.Comuna = null;
                            equipo_bd.Direccion = null;
                            equipo_bd.Proyecto = null;
                            equipo_bd.SistemaOperativo = null;
                            equipo_bd.Distribucion = null;

                            //if (gerenteid > 0)
                            //    equipo_bd.GerenteAutorizaId = gerenteid;

                            //if (solicitanteid > 0)
                            //    equipo_bd.SolicitanteId = solicitanteid;

                            //if (usuariofinalid > 0)
                            //    equipo_bd.UsuarioFinalId = usuariofinalid;

                            //if (tecnicoid > 0)
                            //    equipo_bd.TecnicoPreId = tecnicoid;
                            //else
                            //    equipo_bd.TecnicoPreId = 1;                                 // 1: Sin Asignar


                            //if ((equipo_bd.UsuarioFinalId > 1) && (equipo_aux.SinTecnicoPreId))
                            //{
                            //    equipo_bd.SubEstadoId = 7;                                  // Equipo listo para Entrega
                            //    equipo_bd.TecnicoPreId = 5;                                 // 5: No requerido
                            //}
                            //else if (equipo_aux.SinTecnicoPreId)
                            //{
                            //    equipo_bd.SubEstadoId = 8;                                  // Equipo sin Usuario Final
                            //    equipo_bd.TecnicoPreId = 5;                                 // 5: No requerido
                            //}
                            //else if (equipo_bd.TecnicoPreId > 1)
                            //{
                            //    equipo_bd.SubEstadoId = 4;                                  // Equipo en Preparación
                            //}
                            //else if (equipo_bd.UsuarioFinalId > 1)
                            //{
                            //    equipo_bd.SubEstadoId = 6;                                  // Equipo sin Técnico
                            //}
                            //else
                            //{
                            //    equipo_bd.SubEstadoId = 5;                                  // Equipo con Datos Básicos
                            //}

                            // Liberar
                            bd_iss.Entry(equipo_bd).State = System.Data.Entity.EntityState.Detached;

                            // Actualizar
                            bd_iss.Entry(equipo_bd).State = System.Data.Entity.EntityState.Modified;
                            int resultado = bd_iss.SaveChanges();

                            if (resultado > 0)
                            {
                                TempData["exito"] = "Equipo " + eq.NumeroSerie + " (" + eq.EquipoId.ToString() + ") actualizado en EquiposIngresados (Post). ";
                            }
                            else
                            {
                                algun_equipos_con_error = true;
                                TempData["error"] = messages_errors + " NO fue actualizado el Equipo " + eq.NumeroSerie + " (" + eq.EquipoId.ToString() + ") en EquiposIngresados (Post). ";
                            }
                        }

                        equipo_bd = null;
                    }
                }

                if (algun_equipos_con_error)
                {
                    TempData["error"] = "Algún equipo NO actualizado en EquiposIngresados (Post) debido a: -" + messages_errors + "- ";
                    return RedirectToAction("Error", "ISS");
                }
                else if (algun_equipo_por_actualizar == false)
                {
                    TempData["error"] = "No seleccionó ningún equipo en EquiposIngresados (Post). ";
                    return RedirectToAction("Error", "ISS");
                }
                else
                {
                    TempData["exito"] = "Equipos actualizados en EquiposIngresados (Post). ";
                    return RedirectToAction("EquiposIngresados", "ISS");
                }
            }
            else
            {
                return View(ed);
            }
        }


        [HttpGet]
        public ActionResult Error()
        {
            ViewBag.Current = "Error";

            string messages_errors = string.Empty;

            if (Session["RolId"] == null)
            {
                // Accediendo a esta página sin pasar por función AutoLoginSSS
                messages_errors = "Accediendo a esta página sin pasar por función AutoLogin";

                Session["RolId"] = 0;
                Session["Mensaje"] = messages_errors;

                TempData["error"] = messages_errors;
                return RedirectToAction("LoginError");
            }
            else
            {
                string error = string.Empty;

                if (TempData.ContainsKey("error"))
                    error = TempData["error"].ToString();

                ViewBag.MensajeError = error;

                return View();
            }
        }


        [HttpPost]
        public ActionResult BuscarEquipo2(string buscarequipo)
        {
            ViewBag.Current = "BuscarEquipo";

            buscarequipo = buscarequipo.ToUpper();
            string messages_errors = string.Empty;

            if (Session["RolId"] == null)
            {
                // Accediendo a esta página sin pasar por función AutoLogin
                messages_errors = "Accediendo a esta página sin pasar por función AutoLogin";

                Session["RolId"] = 0;
                Session["Mensaje"] = messages_errors;
                TempData["error"] = messages_errors;

                return RedirectToAction("LoginError");
            }
            else
            {
                if (!string.IsNullOrEmpty(buscarequipo))
                {
                    List<Equipo> equipos_bd = ObtenerEquiposPorNumeroSerie2(buscarequipo);

                    if (equipos_bd.Count() == 0)
                    {
                        messages_errors = "No existe un equipo con el número de serie igual a " + buscarequipo.ToString();
                        TempData["error"] = messages_errors;
                        return RedirectToAction("Error", "ISS");
                    }
                    else if (equipos_bd.Count() > 1)
                    {
                        // PENDIENTE: ListarEquipos, mientras tanto, arroja mensaje de error

                        //messages_errors = "¡Equipos " + buscarequipo.ToString() + " encontrados!";
                        //TempData["exito"] = messages_errors;
                        //return RedirectToAction("ListarEquipos", "ISS", new { @equipos_con_numero_serie_repetido = equipos_bd });

                        messages_errors = equipos_bd.Count() + " equipos con número de serie " + buscarequipo.ToString() + " repetido. ";
                        TempData["error"] = messages_errors;
                        return RedirectToAction("Error", "ISS");
                    }
                    else
                    {
                        messages_errors = "¡Equipo " + buscarequipo.ToString() + " encontrado!";
                        TempData["exito"] = messages_errors;
                        return RedirectToAction("ModificarEquipo", "ISS", new { @numeroserie = buscarequipo });
                    }
                }
                else
                {
                    messages_errors = "No fue posible cargar el Equipo con número de serie " + buscarequipo.ToString() + " ya que no ingresó un valor alfanumérico. ";
                    TempData["error"] = messages_errors;
                    return RedirectToAction("Error", "ISS");
                }
            }
        }


        [HttpPost]
        public ActionResult BuscarEquipo(string buscarequipo)
        {
            ViewBag.Current = "BuscarEquipo";

            buscarequipo = buscarequipo.ToUpper();
            string messages_errors = string.Empty;

            if (Session["RolId"] == null)
            {
                // Accediendo a esta página sin pasar por función AutoLogin
                messages_errors = "Accediendo a esta página sin pasar por función AutoLogin";

                Session["RolId"] = 0;
                Session["Mensaje"] = messages_errors;
                TempData["error"] = messages_errors;

                return RedirectToAction("LoginError");
            }
            else
            {
                oHito resultado = ObtenerIdEquipoPorNumeroSerie(buscarequipo);

                if (resultado.id == 0)
                {
                    TempData["error"] = resultado.error;
                    return RedirectToAction("Error", "ISS");
                }
                else
                {
                    TempData["exito"] = resultado.error;
                    return RedirectToAction("ModificarEquipo", "ISS", new { @numeroserie = buscarequipo });
                }
                
            }
        }



        [HttpGet]
        public ActionResult ListarEquipos(List<Equipo> equipos_con_numero_serie_repetido)
        {
            ViewBag.Current = "ListarEquipos";

            string messages_errors = string.Empty;

            if (Session["RolId"] == null)
            {
                // Accediendo a esta página sin pasar por función AutoLoginSSS
                messages_errors = "Accediendo a esta página sin pasar por función AutoLogin";

                Session["RolId"] = 0;
                Session["Mensaje"] = messages_errors;
                TempData["error"] = messages_errors;

                return RedirectToAction("LoginError");
            }
            else
            {
                List<oEquipo> lista_auxiliar = new List<oEquipo>();
                oEquipoDistribuido equipo_distribuido = new oEquipoDistribuido();
                oEquipo equipo;

                IniciarViewbag();

                if (equipos_con_numero_serie_repetido.Count() > 0)
                {
                    foreach (Equipo eq in equipos_con_numero_serie_repetido)
                    {
                        equipo = new oEquipo();
                        equipo = ObtenerEquipo(eq.EquipoId);

                        lista_auxiliar.Add(equipo);
                    }
                }
                else
                {
                }

                equipo_distribuido.equipos = lista_auxiliar;

                return View(equipo_distribuido);
                
            }
        }
            
        
        [HttpGet]
        public ActionResult EquiposListosParaEntrega()
        {
            ViewBag.Current = "EquiposListosParaEntrega";

            string messages_errors = string.Empty;

            if (Session["RolId"] == null)
            {
                // Accediendo a esta página sin pasar por función AutoLoginSSS
                messages_errors = "Accediendo a esta página sin pasar por función AutoLogin";

                Session["RolId"] = 0;
                Session["Mensaje"] = messages_errors;
                TempData["error"] = messages_errors;

                return RedirectToAction("LoginError");
            }
            else
            {
                List<oEquipo> lista_auxiliar = new List<oEquipo>();
                oEquipoDistribuido equipo_distribuido = new oEquipoDistribuido();
                oEquipo equipo;

                IniciarViewbag();

                // buscar equipos con sub-estado = 7

                using (InventarioSoporteSistemas_desEntities bd_iss = new InventarioSoporteSistemas_desEntities())
                {
                    List<Equipo> equipos_bd = bd_iss.Equipo.Where(e => e.SubEstadoId == 7).ToList();

                    if (equipos_bd.Count() > 0)
                    {
                        foreach (Equipo eq in equipos_bd)
                        {
                            equipo = new oEquipo();
                            equipo = ObtenerEquipo(eq.EquipoId);

                            lista_auxiliar.Add(equipo);
                        }
                    }
                    else
                    {
                    }

                    equipo_distribuido.equipos = lista_auxiliar;

                    return View(equipo_distribuido);
                }
            }
        }


        [HttpPost]
        public ActionResult EquiposListosParaEntrega(oEquipoDistribuido ed)
        {
            if (ed.equipos.Count > 0)
            {
                bool algun_equipo_por_actualizar = false, algun_equipo_con_error = false;
                string messages_errors = string.Empty;
                oEquipo equipo_aux = new oEquipo();
                Equipo equipo_bd;

                using (InventarioSoporteSistemas_desEntities bd_iss = new InventarioSoporteSistemas_desEntities())
                {
                    foreach (oEquipo eq in ed.equipos)
                    {
                        equipo_bd = new Equipo();

                        if (eq.actualizar)
                        {
                            algun_equipo_por_actualizar = true;

                            equipo_aux = ObtenerEquipo(eq.EquipoId);
                            equipo_aux = ActualizarEquipo(equipo_aux, ed.equipo);

                            //equipo_bd = ConstruirEquipo(ObtenerEquipo(eq.EquipoId));
                            equipo_bd = ConstruirEquipo(equipo_aux);

                            equipo_bd.TipoEquipo = null;
                            equipo_bd.Marca = null;
                            equipo_bd.EstadoEquipo = null;
                            equipo_bd.SubEstadoEquipo = null;
                            equipo_bd.Guia = null;
                            equipo_bd.MacroEmpresa = null;
                            equipo_bd.Empresa = null;
                            equipo_bd.CentroCosto = null;
                            equipo_bd.Ubicacion = null;
                            equipo_bd.CentroCosto = null;
                            equipo_bd.Comuna = null;
                            equipo_bd.Direccion = null;
                            equipo_bd.Proyecto = null;
                            equipo_bd.SistemaOperativo = null;
                            equipo_bd.Distribucion = null;

                            // Liberar
                            bd_iss.Entry(equipo_bd).State = System.Data.Entity.EntityState.Detached;

                            // Actualizar
                            bd_iss.Entry(equipo_bd).State = System.Data.Entity.EntityState.Modified;
                            int resultado = bd_iss.SaveChanges();

                            if (resultado > 0)
                            {
                                TempData["exito"] = "Equipo " + eq.NumeroSerie + " (" + eq.EquipoId.ToString() + ") actualizado en EquiposListosParaEntrega (Post). ";
                            }
                            else
                            {
                                algun_equipo_con_error = true;
                                TempData["error"] = messages_errors + " NO fue actualizado el Equipo " + eq.NumeroSerie + " (" + eq.EquipoId.ToString() + ") en EquiposListosParaEntrega (Post). ";
                            }
                        }

                        equipo_bd = null;
                    }
                }

                if (algun_equipo_con_error)
                {
                    TempData["error"] = "Algún equipo NO actualizado en EquiposListosParaEntrega (Post) debido a: -" + messages_errors + "- ";
                    return RedirectToAction("Error", "ISS");
                }
                else if (algun_equipo_por_actualizar == false)
                {
                    TempData["error"] = "No seleccionó ningún equipo en EquiposListosParaEntrega (Post). ";
                    return RedirectToAction("Error", "ISS");
                }
                else
                {
                    TempData["exito"] = "Equipos actualizados en EquiposListosParaEntrega (Post). ";
                    return RedirectToAction("EquiposListosParaEntrega", "ISS");
                }
            }
            else
            {
                return View(ed);
            }
        }



        [HttpGet]
        public ActionResult EquiposSinTecnico()
        {
            ViewBag.Current = "EquiposSinTecnico";

            string messages_errors = string.Empty;

            if (Session["RolId"] == null)
            {
                // Accediendo a esta página sin pasar por función AutoLoginSSS
                messages_errors = "Accediendo a esta página sin pasar por función AutoLogin";

                Session["RolId"] = 0;
                Session["Mensaje"] = messages_errors;
                TempData["error"] = messages_errors;

                return RedirectToAction("LoginError");
            }
            else
            {
                List<oEquipo> lista_auxiliar = new List<oEquipo>();
                oEquipoDistribuido equipo_distribuido = new oEquipoDistribuido();
                oEquipo equipo;

                IniciarViewbag();

                // buscar equipos con sub-estado = 6

                using (InventarioSoporteSistemas_desEntities bd_iss = new InventarioSoporteSistemas_desEntities())
                {
                    List<Equipo> equipos_bd = bd_iss.Equipo.Where(e => e.SubEstadoId == 6).ToList();

                    if (equipos_bd.Count() > 0)
                    {
                        foreach (Equipo eq in equipos_bd)
                        {
                            equipo = new oEquipo();
                            equipo = ObtenerEquipo(eq.EquipoId);

                            lista_auxiliar.Add(equipo);
                        }
                    }
                    else
                    {
                    }

                    equipo_distribuido.equipos = lista_auxiliar;

                    return View(equipo_distribuido);
                }
            }
        }


        [HttpPost]
        public ActionResult EquiposSinTecnico(oEquipoDistribuido ed)
        {
            if (ed.equipos.Count > 0)
            {
                bool algun_equipo_por_actualizar = false, algun_equipos_con_error = false;
                string messages_errors = string.Empty;
                oEquipo equipo_aux = new oEquipo();
                Equipo equipo_bd;
                
                using (InventarioSoporteSistemas_desEntities bd_iss = new InventarioSoporteSistemas_desEntities())
                {
                    foreach (oEquipo eq in ed.equipos)
                    {
                        equipo_bd = new Equipo();

                        if (eq.actualizar)
                        {
                            algun_equipo_por_actualizar = true;

                            equipo_aux = ObtenerEquipo(eq.EquipoId);
                            equipo_aux = ActualizarEquipo(equipo_aux, ed.equipo);

                            //equipo_bd = ConstruirEquipo(ObtenerEquipo(eq.EquipoId));
                            equipo_bd = ConstruirEquipo(equipo_aux);

                            equipo_bd.TipoEquipo = null;
                            equipo_bd.Marca = null;
                            equipo_bd.EstadoEquipo = null;
                            equipo_bd.SubEstadoEquipo = null;
                            equipo_bd.Guia = null;
                            equipo_bd.MacroEmpresa = null;
                            equipo_bd.Empresa = null;
                            equipo_bd.CentroCosto = null;
                            equipo_bd.Ubicacion = null;
                            equipo_bd.CentroCosto = null;
                            equipo_bd.Comuna = null;
                            equipo_bd.Direccion = null;
                            equipo_bd.Proyecto = null;
                            equipo_bd.SistemaOperativo = null;
                            equipo_bd.Distribucion = null;

                            // Liberar
                            bd_iss.Entry(equipo_bd).State = System.Data.Entity.EntityState.Detached;

                            // Actualizar
                            bd_iss.Entry(equipo_bd).State = System.Data.Entity.EntityState.Modified;
                            int resultado = bd_iss.SaveChanges();

                            if (resultado > 0)
                            {
                                TempData["exito"] = "Equipo " + eq.NumeroSerie + " (" + eq.EquipoId.ToString() + ") actualizado en EquiposSinTecnico (Post). ";
                            }
                            else
                            {
                                algun_equipos_con_error = true;
                                TempData["error"] = messages_errors + " NO fue actualizado el Equipo " + eq.NumeroSerie + " (" + eq.EquipoId.ToString() + ") en EquiposSinTecnico (Post). ";
                            }
                        }

                        equipo_bd = null;
                    }
                }

                if (algun_equipos_con_error)
                {
                    TempData["error"] = "Algún equipo NO actualizado en EquiposSinTecnico (Post) debido a: -" + messages_errors + "- ";
                    return RedirectToAction("Error", "ISS");
                }
                else if (algun_equipo_por_actualizar == false)
                {
                    TempData["error"] = "No seleccionó ningún equipo en EquiposSinTecnico (Post). ";
                    return RedirectToAction("Error", "ISS");
                }
                else
                {
                    TempData["exito"] = "Equipos actualizados en EquiposSinTecnico (Post). ";
                    return RedirectToAction("EquiposSinTecnico", "ISS");
                }
            }
            else
            {
                return View(ed);
            }
        }


        [HttpGet]
        public ActionResult EquiposConDatosBasicos()
        {
            ViewBag.Current = "EquiposConDatosBasicos";

            string messages_errors = string.Empty;

            if (Session["RolId"] == null)
            {
                // Accediendo a esta página sin pasar por función AutoLoginSSS
                messages_errors = "Accediendo a esta página sin pasar por función AutoLogin";

                Session["RolId"] = 0;
                Session["Mensaje"] = messages_errors;
                TempData["error"] = messages_errors;

                return RedirectToAction("LoginError");
            }
            else
            {
                List<oEquipo> lista_auxiliar = new List<oEquipo>();
                oEquipoDistribuido equipo_distribuido = new oEquipoDistribuido();
                oEquipo equipo;

                IniciarViewbag();

                // buscar equipos con sub-estado = 5

                using (InventarioSoporteSistemas_desEntities bd_iss = new InventarioSoporteSistemas_desEntities())
                {
                    List<Equipo> equipos_bd = bd_iss.Equipo.Where(e => e.SubEstadoId == 5).ToList();

                    if (equipos_bd.Count() > 0)
                    {
                        foreach (Equipo eq in equipos_bd)
                        {
                            equipo = new oEquipo();
                            equipo = ObtenerEquipo(eq.EquipoId);

                            lista_auxiliar.Add(equipo);
                        }
                    }
                    else
                    {
                    }

                    equipo_distribuido.equipos = lista_auxiliar;

                    return View(equipo_distribuido);
                }
            }
        }


        [HttpPost]
        public ActionResult EquiposConDatosBasicos(oEquipoDistribuido ed)
        {
            if (ed.equipos.Count > 0)
            {
                bool algun_equipo_por_actualizar = false, algun_equipos_con_error = false;
                string messages_errors = string.Empty;
                oEquipo equipo_aux = new oEquipo();
                Equipo equipo_bd;

                using (InventarioSoporteSistemas_desEntities bd_iss = new InventarioSoporteSistemas_desEntities())
                {
                    foreach (oEquipo eq in ed.equipos)
                    {
                        equipo_bd = new Equipo();

                        if (eq.actualizar)
                        {
                            algun_equipo_por_actualizar = true;

                            equipo_aux = ObtenerEquipo(eq.EquipoId);
                            equipo_aux = ActualizarEquipo(equipo_aux, ed.equipo);

                            //equipo_bd = ConstruirEquipo(ObtenerEquipo(eq.EquipoId));
                            equipo_bd = ConstruirEquipo(equipo_aux);

                            equipo_bd.TipoEquipo = null;
                            equipo_bd.Marca = null;
                            equipo_bd.EstadoEquipo = null;
                            equipo_bd.SubEstadoEquipo = null;
                            equipo_bd.Guia = null;
                            equipo_bd.MacroEmpresa = null;
                            equipo_bd.Empresa = null;
                            equipo_bd.CentroCosto = null;
                            equipo_bd.Ubicacion = null;
                            equipo_bd.CentroCosto = null;
                            equipo_bd.Comuna = null;
                            equipo_bd.Direccion = null;
                            equipo_bd.Proyecto = null;
                            equipo_bd.SistemaOperativo = null;
                            equipo_bd.Distribucion = null;

                            // Liberar
                            bd_iss.Entry(equipo_bd).State = System.Data.Entity.EntityState.Detached;

                            // Actualizar
                            bd_iss.Entry(equipo_bd).State = System.Data.Entity.EntityState.Modified;
                            int resultado = bd_iss.SaveChanges();

                            if (resultado > 0)
                            {
                                TempData["exito"] = "Equipo " + eq.NumeroSerie + " (" + eq.EquipoId.ToString() + ") actualizado en EquiposConDatosBasicos (Post). ";
                            }
                            else
                            {
                                algun_equipos_con_error = true;
                                TempData["error"] = messages_errors + " NO fue actualizado el Equipo " + eq.NumeroSerie + " (" + eq.EquipoId.ToString() + ") en EquiposConDatosBasicos (Post). ";
                            }
                        }

                        equipo_bd = null;
                    }
                }

                if (algun_equipos_con_error)
                {
                    TempData["error"] = "Algún equipo NO actualizado en EquiposConDatosBasicos (Post) debido a: -" + messages_errors + "- ";
                    return RedirectToAction("Error", "ISS");
                }
                else if (algun_equipo_por_actualizar == false)
                {
                    TempData["error"] = "No seleccionó ningún equipo en EquiposConDatosBasicos (Post). ";
                    return RedirectToAction("Error", "ISS");
                }
                else
                {
                    TempData["exito"] = "Equipos actualizados en EquiposConDatosBasicos (Post). ";
                    return RedirectToAction("EquiposConDatosBasicos", "ISS");
                }
            }
            else
            {
                return View(ed);
            }
        }


        [HttpGet]
        public ActionResult EquiposEnPreparacion()
        {
            ViewBag.Current = "EquiposEnPreparacion";

            string messages_errors = string.Empty;

            if (Session["RolId"] == null)
            {
                // Accediendo a esta página sin pasar por función AutoLoginSSS
                messages_errors = "Accediendo a esta página sin pasar por función AutoLogin";

                Session["RolId"] = 0;
                Session["Mensaje"] = messages_errors;
                TempData["error"] = messages_errors;

                return RedirectToAction("LoginError");
            }
            else
            {
                List<oEquipo> lista_auxiliar = new List<oEquipo>();
                oEquipoDistribuido equipo_distribuido = new oEquipoDistribuido();
                oEquipo equipo;

                IniciarViewbag();

                // buscar equipos con sub-estado = 4

                using (InventarioSoporteSistemas_desEntities bd_iss = new InventarioSoporteSistemas_desEntities())
                {
                    List<Equipo> equipos_bd = bd_iss.Equipo.Where(e => e.SubEstadoId == 4).ToList();

                    if (equipos_bd.Count() > 0)
                    {
                        foreach (Equipo eq in equipos_bd)
                        {
                            equipo = new oEquipo();
                            equipo = ObtenerEquipo(eq.EquipoId);

                            lista_auxiliar.Add(equipo);
                        }
                    }
                    else
                    {
                    }

                    equipo_distribuido.equipos = lista_auxiliar;

                    return View(equipo_distribuido);
                }
            }
        }


        [HttpPost]
        public ActionResult EquiposEnPreparacion(oEquipoDistribuido ed)
        {
            if (ed.equipos.Count > 0)
            {
                bool algun_equipo_por_actualizar = false, algun_equipos_con_error = false;
                string messages_errors = string.Empty;
                oEquipo equipo_aux = new oEquipo();
                Equipo equipo_bd;

                using (InventarioSoporteSistemas_desEntities bd_iss = new InventarioSoporteSistemas_desEntities())
                {
                    foreach (oEquipo eq in ed.equipos)
                    {
                        equipo_bd = new Equipo();

                        if (eq.actualizar)
                        {
                            algun_equipo_por_actualizar = true;

                            equipo_aux = ObtenerEquipo(eq.EquipoId);
                            equipo_aux = ActualizarEquipo(equipo_aux, ed.equipo);

                            //equipo_bd = ConstruirEquipo(ObtenerEquipo(eq.EquipoId));
                            equipo_bd = ConstruirEquipo(equipo_aux);

                            equipo_bd.TipoEquipo = null;
                            equipo_bd.Marca = null;
                            equipo_bd.EstadoEquipo = null;
                            equipo_bd.SubEstadoEquipo = null;
                            equipo_bd.Guia = null;
                            equipo_bd.MacroEmpresa = null;
                            equipo_bd.Empresa = null;
                            equipo_bd.CentroCosto = null;
                            equipo_bd.Ubicacion = null;
                            equipo_bd.CentroCosto = null;
                            equipo_bd.Comuna = null;
                            equipo_bd.Direccion = null;
                            equipo_bd.Proyecto = null;
                            equipo_bd.SistemaOperativo = null;
                            equipo_bd.Distribucion = null;

                            // Liberar
                            bd_iss.Entry(equipo_bd).State = System.Data.Entity.EntityState.Detached;

                            // Actualizar
                            bd_iss.Entry(equipo_bd).State = System.Data.Entity.EntityState.Modified;
                            int resultado = bd_iss.SaveChanges();

                            if (resultado > 0)
                            {
                                TempData["exito"] = "Equipo " + eq.NumeroSerie + " (" + eq.EquipoId.ToString() + ") actualizado en EquiposEnPreparacion (Post). ";
                            }
                            else
                            {
                                algun_equipos_con_error = true;
                                TempData["error"] = messages_errors + " NO fue actualizado el Equipo " + eq.NumeroSerie + " (" + eq.EquipoId.ToString() + ") en EquiposEnPreparacion (Post). ";
                            }
                        }

                        equipo_bd = null;
                    }
                }

                if (algun_equipos_con_error)
                {
                    TempData["error"] = "Algún equipo NO actualizado en EquiposEnPreparacion (Post) debido a: -" + messages_errors + "- ";
                    return RedirectToAction("Error", "ISS");
                }
                else if (algun_equipo_por_actualizar == false)
                {
                    TempData["error"] = "No seleccionó ningún equipo en EquiposEnPreparacion (Post). ";
                    return RedirectToAction("Error", "ISS");
                }
                else
                {
                    TempData["exito"] = "Equipos actualizados en EquiposEnPreparacion (Post). ";
                    return RedirectToAction("EquiposEnPreparacion", "ISS");
                }
            }
            else
            {
                return View(ed);
            }
        }


        [HttpGet]
        public ActionResult EquiposSinUsuarioFinal()
        {
            ViewBag.Current = "EquiposSinUsuarioFinal";

            string messages_errors = string.Empty;

            if (Session["RolId"] == null)
            {
                // Accediendo a esta página sin pasar por función AutoLoginSSS
                messages_errors = "Accediendo a esta página sin pasar por función AutoLogin";

                Session["RolId"] = 0;
                Session["Mensaje"] = messages_errors;
                TempData["error"] = messages_errors;

                return RedirectToAction("LoginError");
            }
            else
            {
                List<oEquipo> lista_auxiliar = new List<oEquipo>();
                oEquipoDistribuido equipo_distribuido = new oEquipoDistribuido();
                oEquipo equipo;

                IniciarViewbag();

                // buscar equipos con sub-estado = 8

                using (InventarioSoporteSistemas_desEntities bd_iss = new InventarioSoporteSistemas_desEntities())
                {
                    List<Equipo> equipos_bd = bd_iss.Equipo.Where(e => e.SubEstadoId == 8).ToList();

                    if (equipos_bd.Count() > 0)
                    {
                        foreach (Equipo eq in equipos_bd)
                        {
                            equipo = new oEquipo();
                            equipo = ObtenerEquipo(eq.EquipoId);

                            lista_auxiliar.Add(equipo);
                        }
                    }
                    else
                    {
                    }

                    equipo_distribuido.equipos = lista_auxiliar;

                    return View(equipo_distribuido);
                }
            }
        }


        [HttpPost]
        public ActionResult EquiposSinUsuarioFinal(oEquipoDistribuido ed)
        {
            if (ed.equipos.Count > 0)
            {
                bool algun_equipo_por_actualizar = false, algun_equipos_con_error = false;
                string messages_errors = string.Empty;
                oEquipo equipo_aux = new oEquipo();
                Equipo equipo_bd;

                using (InventarioSoporteSistemas_desEntities bd_iss = new InventarioSoporteSistemas_desEntities())
                {
                    foreach (oEquipo eq in ed.equipos)
                    {
                        equipo_bd = new Equipo();

                        if (eq.actualizar)
                        {
                            algun_equipo_por_actualizar = true;

                            equipo_aux = ObtenerEquipo(eq.EquipoId);
                            equipo_aux = ActualizarEquipo(equipo_aux, ed.equipo);

                            //equipo_bd = ConstruirEquipo(ObtenerEquipo(eq.EquipoId));
                            equipo_bd = ConstruirEquipo(equipo_aux);

                            equipo_bd.TipoEquipo = null;
                            equipo_bd.Marca = null;
                            equipo_bd.EstadoEquipo = null;
                            equipo_bd.SubEstadoEquipo = null;
                            equipo_bd.Guia = null;
                            equipo_bd.MacroEmpresa = null;
                            equipo_bd.Empresa = null;
                            equipo_bd.CentroCosto = null;
                            equipo_bd.Ubicacion = null;
                            equipo_bd.CentroCosto = null;
                            equipo_bd.Comuna = null;
                            equipo_bd.Direccion = null;
                            equipo_bd.Proyecto = null;
                            equipo_bd.SistemaOperativo = null;
                            equipo_bd.Distribucion = null;

                            // Liberar
                            bd_iss.Entry(equipo_bd).State = System.Data.Entity.EntityState.Detached;

                            // Actualizar
                            bd_iss.Entry(equipo_bd).State = System.Data.Entity.EntityState.Modified;
                            int resultado = bd_iss.SaveChanges();

                            if (resultado > 0)
                            {
                                TempData["exito"] = "Equipo " + eq.NumeroSerie + " (" + eq.EquipoId.ToString() + ") actualizado en EquiposSinUsuarioFinal (Post). ";
                            }
                            else
                            {
                                algun_equipos_con_error = true;
                                TempData["error"] = messages_errors + " NO fue actualizado el Equipo " + eq.NumeroSerie + " (" + eq.EquipoId.ToString() + ") en EquiposSinUsuarioFinal (Post). ";
                            }
                        }

                        equipo_bd = null;
                    }
                }

                if (algun_equipos_con_error)
                {
                    TempData["error"] = "Algún equipo NO actualizado en EquiposSinUsuarioFinal (Post) debido a: -" + messages_errors + "- ";
                    return RedirectToAction("Error", "ISS");
                }
                else if (algun_equipo_por_actualizar == false)
                {
                    TempData["error"] = "No seleccionó ningún equipo en EquiposSinUsuarioFinal (Post). ";
                    return RedirectToAction("Error", "ISS");
                }
                else
                {
                    TempData["exito"] = "Equipos actualizados en EquiposSinUsuarioFinal (Post). ";
                    return RedirectToAction("EquiposSinUsuarioFinal", "ISS");
                }
            }
            else
            {
                return View(ed);
            }
        }


        [HttpGet]
        public ActionResult EquiposParaEntrega()
        {
            ViewBag.Current = "EquiposParaEntrega";

            string messages_errors = string.Empty;

            if (Session["RolId"] == null)
            {
                // Accediendo a esta página sin pasar por función AutoLoginSSS
                messages_errors = "Accediendo a esta página sin pasar por función AutoLogin";

                Session["RolId"] = 0;
                Session["Mensaje"] = messages_errors;
                TempData["error"] = messages_errors;

                return RedirectToAction("LoginError");
            }
            else
            {
                List<oEquipo> lista_auxiliar = new List<oEquipo>();
                oEquipoDistribuido equipo_distribuido = new oEquipoDistribuido();
                oEquipo equipo;

                IniciarViewbag();

                // buscar equipos con sub-estado = 10

                using (InventarioSoporteSistemas_desEntities bd_iss = new InventarioSoporteSistemas_desEntities())
                {
                    List<Equipo> equipos_bd = bd_iss.Equipo.Where(e => e.SubEstadoId == 10).ToList();

                    if (equipos_bd.Count() > 0)
                    {
                        foreach (Equipo eq in equipos_bd)
                        {
                            equipo = new oEquipo();
                            equipo = ObtenerEquipo(eq.EquipoId);

                            lista_auxiliar.Add(equipo);
                        }
                    }
                    else
                    {
                    }

                    equipo_distribuido.equipos = lista_auxiliar;

                    return View(equipo_distribuido);
                }
            }
        }


        [HttpPost]
        public ActionResult EquiposParaEntrega(oEquipoDistribuido ed)
        {
            if (ed.equipos.Count > 0)
            {
                bool algun_equipo_por_actualizar = false, algun_equipos_con_error = false;
                string messages_errors = string.Empty;
                oEquipo equipo_aux = new oEquipo();
                Equipo equipo_bd;

                using (InventarioSoporteSistemas_desEntities bd_iss = new InventarioSoporteSistemas_desEntities())
                {
                    foreach (oEquipo eq in ed.equipos)
                    {
                        equipo_bd = new Equipo();

                        if (eq.actualizar)
                        {
                            algun_equipo_por_actualizar = true;

                            equipo_aux = ObtenerEquipo(eq.EquipoId);
                            equipo_aux = ActualizarEquipo(equipo_aux, ed.equipo);

                            //equipo_bd = ConstruirEquipo(ObtenerEquipo(eq.EquipoId));
                            equipo_bd = ConstruirEquipo(equipo_aux);

                            equipo_bd.TipoEquipo = null;
                            equipo_bd.Marca = null;
                            equipo_bd.EstadoEquipo = null;
                            equipo_bd.SubEstadoEquipo = null;
                            equipo_bd.Guia = null;
                            equipo_bd.MacroEmpresa = null;
                            equipo_bd.Empresa = null;
                            equipo_bd.CentroCosto = null;
                            equipo_bd.Ubicacion = null;
                            equipo_bd.CentroCosto = null;
                            equipo_bd.Comuna = null;
                            equipo_bd.Direccion = null;
                            equipo_bd.Proyecto = null;
                            equipo_bd.SistemaOperativo = null;
                            equipo_bd.Distribucion = null;

                            // Liberar
                            bd_iss.Entry(equipo_bd).State = System.Data.Entity.EntityState.Detached;

                            // Actualizar
                            bd_iss.Entry(equipo_bd).State = System.Data.Entity.EntityState.Modified;

                            int resultado = bd_iss.SaveChanges();

                            if (resultado > 0)
                            {
                                TempData["exito"] = "Equipo " + eq.NumeroSerie + " (" + eq.EquipoId.ToString() + ") actualizado en EquiposParaEntrega (Post). ";
                            }
                            else
                            {
                                algun_equipos_con_error = true;
                                TempData["error"] = messages_errors + " NO fue actualizado el Equipo " + eq.NumeroSerie + " (" + eq.EquipoId.ToString() + ") en EquiposParaEntrega (Post). ";
                            }
                        }

                        equipo_bd = null;
                    }
                }

                if (algun_equipos_con_error)
                {
                    TempData["error"] = "Algún equipo NO actualizado en EquiposParaEntrega (Post) debido a: -" + messages_errors + "- ";
                    return RedirectToAction("Error", "ISS");
                }
                else if (algun_equipo_por_actualizar == false)
                {
                    TempData["error"] = "No seleccionó ningún equipo en EquiposParaEntrega (Post). ";
                    return RedirectToAction("Error", "ISS");
                }
                else
                {
                    TempData["exito"] = "Equipos actualizados en EquiposParaEntrega (Post). ";
                    return RedirectToAction("EquiposParaEntrega", "ISS");
                }
            }
            else
            {
                return View(ed);
            }
        }


        [HttpGet]
        public ActionResult EquiposParaRetiro()
        {
            ViewBag.Current = "EquiposParaRetiro";

            string messages_errors = string.Empty;

            if (Session["RolId"] == null)
            {
                // Accediendo a esta página sin pasar por función AutoLoginSSS
                messages_errors = "Accediendo a esta página sin pasar por función AutoLogin";

                Session["RolId"] = 0;
                Session["Mensaje"] = messages_errors;
                TempData["error"] = messages_errors;

                return RedirectToAction("LoginError");
            }
            else
            {
                List<oEquipo> lista_auxiliar = new List<oEquipo>();
                oEquipoDistribuido equipo_distribuido = new oEquipoDistribuido();
                oEquipo equipo;

                IniciarViewbag();

                // buscar equipos con sub-estado = 9

                using (InventarioSoporteSistemas_desEntities bd_iss = new InventarioSoporteSistemas_desEntities())
                {
                    List<Equipo> equipos_bd = bd_iss.Equipo.Where(e => e.SubEstadoId == 9).ToList();

                    if (equipos_bd.Count() > 0)
                    {
                        foreach (Equipo eq in equipos_bd)
                        {
                            equipo = new oEquipo();
                            equipo = ObtenerEquipo(eq.EquipoId);

                            lista_auxiliar.Add(equipo);
                        }
                    }
                    else
                    {
                    }

                    equipo_distribuido.equipos = lista_auxiliar;

                    return View(equipo_distribuido);
                }
            }
        }


        [HttpPost]
        public ActionResult EquiposParaRetiro(oEquipoDistribuido ed)
        {
            if (ed.equipos.Count > 0)
            {
                bool algun_equipo_por_actualizar = false, algun_equipos_con_error = false;
                string messages_errors = string.Empty;
                oEquipo equipo_aux = new oEquipo();
                Equipo equipo_bd;

                using (InventarioSoporteSistemas_desEntities bd_iss = new InventarioSoporteSistemas_desEntities())
                {
                    foreach (oEquipo eq in ed.equipos)
                    {
                        equipo_bd = new Equipo();

                        if (eq.actualizar)
                        {
                            algun_equipo_por_actualizar = true;

                            equipo_aux = ObtenerEquipo(eq.EquipoId);
                            equipo_aux = ActualizarEquipo(equipo_aux, ed.equipo);

                            //equipo_bd = ConstruirEquipo(ObtenerEquipo(eq.EquipoId));
                            equipo_bd = ConstruirEquipo(equipo_aux);

                            equipo_bd.TipoEquipo = null;
                            equipo_bd.Marca = null;
                            equipo_bd.EstadoEquipo = null;
                            equipo_bd.SubEstadoEquipo = null;
                            equipo_bd.Guia = null;
                            equipo_bd.MacroEmpresa = null;
                            equipo_bd.Empresa = null;
                            equipo_bd.CentroCosto = null;
                            equipo_bd.Ubicacion = null;
                            equipo_bd.CentroCosto = null;
                            equipo_bd.Comuna = null;
                            equipo_bd.Direccion = null;
                            equipo_bd.Proyecto = null;
                            equipo_bd.SistemaOperativo = null;
                            equipo_bd.Distribucion = null;

                            // Liberar
                            bd_iss.Entry(equipo_bd).State = System.Data.Entity.EntityState.Detached;

                            // Actualizar
                            bd_iss.Entry(equipo_bd).State = System.Data.Entity.EntityState.Modified;
                            int resultado = bd_iss.SaveChanges();

                            if (resultado > 0)
                            {
                                TempData["exito"] = "Equipo " + eq.NumeroSerie + " (" + eq.EquipoId.ToString() + ") actualizado en EquiposParaRetiro (Post). ";
                            }
                            else
                            {
                                algun_equipos_con_error = true;
                                TempData["error"] = messages_errors + " NO fue actualizado el Equipo " + eq.NumeroSerie + " (" + eq.EquipoId.ToString() + ") en EquiposParaRetiro (Post). ";
                            }
                        }

                        equipo_bd = null;
                    }
                }

                if (algun_equipos_con_error)
                {
                    TempData["error"] = "Algún equipo NO actualizado en EquiposParaRetiro (Post) debido a: -" + messages_errors + "- ";
                    return RedirectToAction("Error", "ISS");
                }
                else if (algun_equipo_por_actualizar == false)
                {
                    TempData["error"] = "No seleccionó ningún equipo en EquiposParaRetiro (Post). ";
                    return RedirectToAction("Error", "ISS");
                }
                else
                {
                    TempData["exito"] = "Equipos actualizados en EquiposParaRetiro (Post). ";
                    return RedirectToAction("EquiposParaRetiro", "ISS");
                }
            }
            else
            {
                return View(ed);
            }
        }
        

        [HttpGet]
        public ActionResult EquiposEnProduccion()
        {
            ViewBag.Current = "EquiposEnProduccion";

            string messages_errors = string.Empty;

            if (Session["RolId"] == null)
            {
                // Accediendo a esta página sin pasar por función AutoLoginSSS
                messages_errors = "Accediendo a esta página sin pasar por función AutoLogin";

                Session["RolId"] = 0;
                Session["Mensaje"] = messages_errors;
                TempData["error"] = messages_errors;

                return RedirectToAction("LoginError");
            }
            else
            {
                List<oEquipo> lista_auxiliar = new List<oEquipo>();
                oEquipoDistribuido equipo_distribuido = new oEquipoDistribuido();
                oEquipo equipo;

                IniciarViewbag();


                using (InventarioSoporteSistemas_desEntities bd_iss = new InventarioSoporteSistemas_desEntities())
                {
                    // buscar equipos con estado = 2 -> En Producción

                    //List<Equipo> equipos_bd = bd_iss.Equipo.Where(e => e.SubEstadoId == 11).ToList();
                    List<Equipo> equipos_bd = bd_iss.Equipo.Where(e => e.SubEstadoEquipo.EstadoEquipo.Id == 2).ToList();

                    if (equipos_bd.Count() > 0)
                    {
                        foreach (Equipo eq in equipos_bd)
                        {
                            equipo = new oEquipo();
                            equipo = ObtenerEquipo(eq.EquipoId);

                            lista_auxiliar.Add(equipo);
                        }
                    }
                    else
                    {
                    }

                    equipo_distribuido.equipos = lista_auxiliar;

                    return View(equipo_distribuido);
                }
            }
        }


        [HttpPost]
        public ActionResult EquiposEnProduccion(oEquipoDistribuido ed)
        {
            if (ed.equipos.Count > 0)
            {
                bool algun_equipo_por_actualizar = false, algun_equipos_con_error = false;
                string messages_errors = string.Empty;
                oEquipo equipo_aux = new oEquipo();
                Equipo equipo_bd;

                using (InventarioSoporteSistemas_desEntities bd_iss = new InventarioSoporteSistemas_desEntities())
                {
                    foreach (oEquipo eq in ed.equipos)
                    {
                        equipo_bd = new Equipo();

                        if (eq.actualizar)
                        {
                            algun_equipo_por_actualizar = true;

                            equipo_aux = ObtenerEquipo(eq.EquipoId);
                            equipo_aux = ActualizarEquipo(equipo_aux, ed.equipo);

                            //equipo_bd = ConstruirEquipo(ObtenerEquipo(eq.EquipoId));
                            equipo_bd = ConstruirEquipo(equipo_aux);

                            equipo_bd.TipoEquipo = null;
                            equipo_bd.Marca = null;
                            equipo_bd.EstadoEquipo = null;
                            equipo_bd.SubEstadoEquipo = null;
                            equipo_bd.Guia = null;
                            equipo_bd.MacroEmpresa = null;
                            equipo_bd.Empresa = null;
                            equipo_bd.CentroCosto = null;
                            equipo_bd.Ubicacion = null;
                            equipo_bd.CentroCosto = null;
                            equipo_bd.Comuna = null;
                            equipo_bd.Direccion = null;
                            equipo_bd.Proyecto = null;
                            equipo_bd.SistemaOperativo = null;
                            equipo_bd.Distribucion = null;

                            // Liberar
                            bd_iss.Entry(equipo_bd).State = System.Data.Entity.EntityState.Detached;

                            // Actualizar
                            bd_iss.Entry(equipo_bd).State = System.Data.Entity.EntityState.Modified;
                            int resultado = bd_iss.SaveChanges();

                            if (resultado > 0)
                            {
                                TempData["exito"] = "Equipo " + eq.NumeroSerie + " (" + eq.EquipoId.ToString() + ") actualizado en EquiposEnProduccion (Post). ";
                            }
                            else
                            {
                                algun_equipos_con_error = true;
                                TempData["error"] = messages_errors + " NO fue actualizado el Equipo " + eq.NumeroSerie + " (" + eq.EquipoId.ToString() + ") en EquiposEnProduccion (Post). ";
                            }
                        }

                        equipo_bd = null;
                    }
                }

                if (algun_equipos_con_error)
                {
                    TempData["error"] = "Algún equipo NO actualizado en EquiposEnProduccion (Post) debido a: -" + messages_errors + "- ";
                    return RedirectToAction("Error", "ISS");
                }
                else if (algun_equipo_por_actualizar == false)
                {
                    TempData["error"] = "No seleccionó ningún equipo en EquiposEnProduccion (Post). ";
                    return RedirectToAction("Error", "ISS");
                }
                else
                {
                    TempData["exito"] = "Equipos actualizados en EquiposEnProduccion (Post). ";
                    return RedirectToAction("EquiposEnProduccion", "ISS");
                }
            }
            else
            {
                return View(ed);
            }
        }


        [HttpGet]
        public ActionResult EquiposPostProduccion()
        {
            ViewBag.Current = "EquiposPostProduccion";

            string messages_errors = string.Empty;

            if (Session["RolId"] == null)
            {
                // Accediendo a esta página sin pasar por función AutoLoginSSS
                messages_errors = "Accediendo a esta página sin pasar por función AutoLogin";

                Session["RolId"] = 0;
                Session["Mensaje"] = messages_errors;
                TempData["error"] = messages_errors;

                return RedirectToAction("LoginError");
            }
            else
            {
                List<oEquipo> lista_auxiliar = new List<oEquipo>();
                oEquipoDistribuido equipo_distribuido = new oEquipoDistribuido();
                oEquipo equipo;

                IniciarViewbag();

                using (InventarioSoporteSistemas_desEntities bd_iss = new InventarioSoporteSistemas_desEntities())
                {
                    // Equipos en Dados de Baja o en Robo
                    
                    List<Equipo> equipos_bd = bd_iss.Equipo.Where(e => e.SubEstadoId == 19 || e.SubEstadoId == 20).ToList();

                    if (equipos_bd.Count() > 0)
                    {
                        foreach (Equipo eq in equipos_bd)
                        {
                            equipo = new oEquipo();
                            equipo = ObtenerEquipo(eq.EquipoId);

                            lista_auxiliar.Add(equipo);
                        }
                    }
                    else
                    {
                    }

                    equipo_distribuido.equipos = lista_auxiliar;

                    return View(equipo_distribuido);
                }
            }
        }


        [HttpPost]
        public ActionResult EquiposPostProduccion(oEquipoDistribuido ed)
        {
            if (ed.equipos.Count > 0)
            {
                bool algun_equipo_por_actualizar = false, algun_equipos_con_error = false;
                string messages_errors = string.Empty;
                oEquipo equipo_aux = new oEquipo();
                Equipo equipo_bd;

                using (InventarioSoporteSistemas_desEntities bd_iss = new InventarioSoporteSistemas_desEntities())
                {
                    foreach (oEquipo eq in ed.equipos)
                    {
                        equipo_bd = new Equipo();

                        if (eq.actualizar)
                        {
                            algun_equipo_por_actualizar = true;

                            equipo_aux = ObtenerEquipo(eq.EquipoId);
                            equipo_aux = ActualizarEquipo(equipo_aux, ed.equipo);

                            //equipo_bd = ConstruirEquipo(ObtenerEquipo(eq.EquipoId));
                            equipo_bd = ConstruirEquipo(equipo_aux);

                            equipo_bd.TipoEquipo = null;
                            equipo_bd.Marca = null;
                            equipo_bd.EstadoEquipo = null;
                            equipo_bd.SubEstadoEquipo = null;
                            equipo_bd.Guia = null;
                            equipo_bd.MacroEmpresa = null;
                            equipo_bd.Empresa = null;
                            equipo_bd.CentroCosto = null;
                            equipo_bd.Ubicacion = null;
                            equipo_bd.CentroCosto = null;
                            equipo_bd.Comuna = null;
                            equipo_bd.Direccion = null;
                            equipo_bd.Proyecto = null;
                            equipo_bd.SistemaOperativo = null;
                            equipo_bd.Distribucion = null;

                            // Liberar
                            bd_iss.Entry(equipo_bd).State = System.Data.Entity.EntityState.Detached;

                            // Actualizar
                            bd_iss.Entry(equipo_bd).State = System.Data.Entity.EntityState.Modified;
                            int resultado = bd_iss.SaveChanges();

                            if (resultado > 0)
                            {
                                TempData["exito"] = "Equipo " + eq.NumeroSerie + " (" + eq.EquipoId.ToString() + ") actualizado en EquiposPostProduccion (Post). ";
                            }
                            else
                            {
                                algun_equipos_con_error = true;
                                TempData["error"] = messages_errors + " NO fue actualizado el Equipo " + eq.NumeroSerie + " (" + eq.EquipoId.ToString() + ") en EquiposPostProduccion (Post). ";
                            }
                        }

                        equipo_bd = null;
                    }
                }

                if (algun_equipos_con_error)
                {
                    TempData["error"] = "Algún equipo NO actualizado en EquiposPostProduccion (Post) debido a: -" + messages_errors + "- ";
                    return RedirectToAction("Error", "ISS");
                }
                else if (algun_equipo_por_actualizar == false)
                {
                    TempData["error"] = "No seleccionó ningún equipo en EquiposPostProduccion (Post). ";
                    return RedirectToAction("Error", "ISS");
                }
                else
                {
                    TempData["exito"] = "Equipos actualizados en EquiposPostProduccion (Post). ";
                    return RedirectToAction("EquiposPostProduccion", "ISS");
                }
            }
            else
            {
                return View(ed);
            }
        }
        

        [HttpGet]
        public ActionResult EquiposPreparados()
        {
            ViewBag.Current = "EquiposPreparados";

            string messages_errors = string.Empty;

            if (Session["RolId"] == null)
            {
                // Accediendo a esta página sin pasar por función AutoLoginSSS
                messages_errors = "Accediendo a esta página sin pasar por función AutoLogin";

                Session["RolId"] = 0;
                Session["Mensaje"] = messages_errors;
                TempData["error"] = messages_errors;

                return RedirectToAction("LoginError");
            }
            else
            {
                List<oEquipo> lista_auxiliar = new List<oEquipo>();
                oEquipoDistribuido equipo_distribuido = new oEquipoDistribuido();
                oEquipo equipo;

                IniciarViewbag();

                // buscar equipos con sub-estado = 7

                using (InventarioSoporteSistemas_desEntities bd_iss = new InventarioSoporteSistemas_desEntities())
                {
                    List<Equipo> equipos_bd = bd_iss.Equipo.Where(e => e.SubEstadoId == 7).ToList();

                    if (equipos_bd.Count() > 0)
                    {
                        foreach (Equipo eq in equipos_bd)
                        {
                            equipo = new oEquipo();
                            equipo = ObtenerEquipo(eq.EquipoId);

                            lista_auxiliar.Add(equipo);
                        }
                    }
                    else
                    {
                    }

                    equipo_distribuido.equipos = lista_auxiliar;

                    return View(equipo_distribuido);
                }
            }
        }


        [HttpPost]
        public ActionResult EquiposPreparados(oEquipoDistribuido ed)
        {
            if (ed.equipos.Count > 0)
            {
                bool algun_equipo_por_actualizar = false, algun_equipo_con_error = false;
                string messages_errors = string.Empty;
                oEquipo equipo_aux = new oEquipo();
                Equipo equipo_bd;

                using (InventarioSoporteSistemas_desEntities bd_iss = new InventarioSoporteSistemas_desEntities())
                {
                    foreach (oEquipo eq in ed.equipos)
                    {
                        equipo_bd = new Equipo();

                        if (eq.actualizar)
                        {
                            algun_equipo_por_actualizar = true;

                            equipo_aux = ObtenerEquipo(eq.EquipoId);
                            equipo_aux = ActualizarEquipo(equipo_aux, ed.equipo);

                            //equipo_bd = ConstruirEquipo(ObtenerEquipo(eq.EquipoId));
                            equipo_bd = ConstruirEquipo(equipo_aux);

                            equipo_bd.TipoEquipo = null;
                            equipo_bd.Marca = null;
                            equipo_bd.EstadoEquipo = null;
                            equipo_bd.SubEstadoEquipo = null;
                            equipo_bd.Guia = null;
                            equipo_bd.MacroEmpresa = null;
                            equipo_bd.Empresa = null;
                            equipo_bd.CentroCosto = null;
                            equipo_bd.Ubicacion = null;
                            equipo_bd.CentroCosto = null;
                            equipo_bd.Comuna = null;
                            equipo_bd.Direccion = null;
                            equipo_bd.Proyecto = null;
                            equipo_bd.SistemaOperativo = null;
                            equipo_bd.Distribucion = null;

                            // Liberar
                            bd_iss.Entry(equipo_bd).State = System.Data.Entity.EntityState.Detached;

                            // Actualizar
                            bd_iss.Entry(equipo_bd).State = System.Data.Entity.EntityState.Modified;
                            int resultado = bd_iss.SaveChanges();

                            if (resultado > 0)
                            {
                                TempData["exito"] = "Equipo " + eq.NumeroSerie + " (" + eq.EquipoId.ToString() + ") actualizado en EquiposPreparados (Post). ";
                            }
                            else
                            {
                                algun_equipo_con_error = true;
                                TempData["error"] = messages_errors + " NO fue actualizado el Equipo " + eq.NumeroSerie + " (" + eq.EquipoId.ToString() + ") en EquiposPreparados (Post). ";
                            }
                        }

                        equipo_bd = null;
                    }
                }

                if (algun_equipo_con_error)
                {
                    TempData["error"] = "Algún equipo NO actualizado en EquiposPreparados (Post) debido a: -" + messages_errors + "- ";
                    return RedirectToAction("Error", "ISS");
                }
                else if (algun_equipo_por_actualizar == false)
                {
                    TempData["error"] = "No seleccionó ningún equipo en EquiposPreparados (Post). ";
                    return RedirectToAction("Error", "ISS");
                }
                else
                {
                    TempData["exito"] = "Equipos actualizados en EquiposPreparados (Post). ";
                    return RedirectToAction("EquiposPreparados", "ISS");
                }
            }
            else
            {
                return View(ed);
            }
        }


        [HttpGet]
        public ActionResult Dashboard()
        {
            ViewBag.Current = "Dashboard";

            string messages_errors = string.Empty;

            if (Session["RolId"] == null)
            {
                // Accediendo a esta página sin pasar por función AutoLoginSSS
                messages_errors = "Accediendo a esta página sin pasar por función AutoLogin";

                Session["RolId"] = 0;
                Session["Mensaje"] = messages_errors;
                TempData["error"] = messages_errors;

                return RedirectToAction("LoginError");
            }
            else
            {
                //string res = FormarCadenaParaGraficoTorta("SP_DB_Obtener_Tipo_Solicitud", "Tipo", "Cant.");

                //ViewBag.tipoSolicitudConcatenado = res;

                return View();
            }
        }


        [HttpGet]
        public ActionResult Generar_PDF_Contrato(string numeroserie)
        {
            return Generar_PDF(numeroserie, "C");
        }
        

        public static Paragraph ConstruirTituloC() 
        {
            Phrase f0 = new Phrase("\r\n" + "Responsabilidad asociada al préstamo de" + "\r\n", titleFontBlackBold);
            Phrase f1 = new Phrase("Computadores Portátiles o Notebooks" + "\r\n", titleFontBlackBold);

            Paragraph p = new Paragraph();
            p.Add(f0);
            p.Add(f1);

            return p;
        }


        public static Paragraph ConstruirTituloH()
        {
            Phrase f0 = new Phrase("\r\n" + "Hoja de Entrega de Equipo" + "\r\n" + "\r\n", titleFontBlackBold);

            Paragraph p = new Paragraph();
            p.Add(f0);

            return p;
        }
        

        public static Paragraph ConstruirContrato(oEquipo equipo) // 0
        {
            //Phrase f0 = new Phrase("Responsabilidad asociada al préstamo de Computadores Portátiles o Notebooks" + "\r\n" + "\r\n", titleFontBlackBold);

            Chunk c1 = new Chunk("Por este acto, don/a ", textFontBlack);
            Chunk c2 = new Chunk(equipo.UsuarioFinalNombre, textFontBlackBold);
            Chunk c3 = new Chunk(", en adelante el “Empleado”, declara haber recibido en préstamo, por parte de su empleador, (sociedad empleadora) ", textFontBlack);
            Chunk c4 = new Chunk(equipo.EmpresaRazonSocial, textFontBlackBold);
            Chunk c5 = new Chunk(" en adelante la “Empresa” un equipo computador portátil o notebook (individualizar): " + "\r\n", textFontBlack);
            
            if (string.IsNullOrEmpty(equipo.UsuarioFinalNombre)) 
            {
                c2 = new Chunk("     ", textFontBlack);
                c2.SetBackground(colorcampoausente);
            }

            if (string.IsNullOrEmpty(equipo.EmpresaRazonSocial))
            {
                c4 = new Chunk("     ", textFontBlack);
                c4.SetBackground(colorcampoausente);
            }

            Phrase f1 = new Phrase();

            f1.Add(c1);
            f1.Add(c2);
            f1.Add(c3);
            f1.Add(c4);
            f1.Add(c5);



            PdfPTable tabla_detalle_equipo = new PdfPTable(2);
            tabla_detalle_equipo.WidthPercentage = 50f;

            float[] widths1 = new float[] { 25f, 25f };
            tabla_detalle_equipo.SetWidths(widths1);

            tabla_detalle_equipo.AddCell(new Phrase("Marca: ", textFontBlack));
            tabla_detalle_equipo.AddCell(new Phrase(equipo.MarcaNombre, textFontBlackBold));
            tabla_detalle_equipo.AddCell(new Phrase("Modelo: ", textFontBlack));
            tabla_detalle_equipo.AddCell(new Phrase(equipo.ModeloNombre, textFontBlackBold));
            tabla_detalle_equipo.AddCell(new Phrase("Nro. Serie:", textFontBlack));
            tabla_detalle_equipo.AddCell(new Phrase(equipo.NumeroSerie, textFontBlackBold));
            
            
            // Tabla con caracteristicas del equipo
            Phrase f2 = new Phrase();
            f2.Add(tabla_detalle_equipo);

            Chunk c6 = new Chunk("\r\n" + "en adelante el “Equipo”. Se deja constancia que la entrega material del Equipo se produjo con fecha ", textFontBlack);

            string dia_e, mes_e, anio_e = string.Empty;
            Chunk c7 = new Chunk();


            if (equipo.FechaDistribucion.HasValue)
            {
                dia_e = equipo.FechaDistribucion.Value.Day.ToString();
                mes_e = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(equipo.FechaDistribucion.Value.ToString("MMMM"));
                anio_e = equipo.FechaDistribucion.Value.Year.ToString();

                c7 = new Chunk(dia_e + " de " + mes_e + " de " + anio_e, textFontBlackBold);
            }
            else
            {
                c7 = new Chunk("     ", textFontBlack);
                c7.SetBackground(colorcampoausente);
            }

            Chunk c10 = new Chunk(". " + "\r\n" + "\r\n", textFontBlack);

            Phrase f3 = new Phrase();

            f3.Add(c6);
            f3.Add(c7);
            f3.Add(c10);



            Phrase f4 = new Phrase();

            if ((equipo.Mouse == true) || (equipo.Bolso == true) || (equipo.Candado == true))
            {
                //f4 = new Phrase("Además se incluyen los siguientes accesorios:" + "\r\n");

                string accesorios = string.Empty;

                Chunk c8 = new Chunk("Además se incluyen los siguientes accesorios: ", textFontBlack);

                if (equipo.Mouse == true)
                    accesorios = accesorios + "Mouse, "; //c9 = new Chunk("Mouse, ");
                if (equipo.Bolso == true)
                    accesorios = accesorios + "Bolso, ";  //c10 = new Chunk("Bolso, ");
                if (equipo.Candado == true)
                    accesorios = accesorios + "Candado, ";  //c11 = new Chunk("Candado, ");

                accesorios = accesorios.Remove(accesorios.Length - 2, 2);

                Chunk c9 = new Chunk(accesorios, textFontBlackBold);

                f4.Add(c8);
                f4.Add(c9);
                f4.Add(c10);

                //PdfPTable tabla_accesorios = new PdfPTable(1);
                //tabla_accesorios.WidthPercentage = 25f;

                //float[] widths2 = new float[] { 25f };
                //tabla_accesorios.SetWidths(widths2);

                //if (equipo.Mouse == true)
                //    tabla_accesorios.AddCell("Mouse");
                //if (equipo.Bolso == true)
                //    tabla_accesorios.AddCell("Bolso");
                //if (equipo.Candado == true)
                //    tabla_accesorios.AddCell("Candado");

                //// Tabla con accesorios
                //Phrase f8 = new Phrase();
                //f8.Add(tabla_accesorios);
                //f8.Add("\r\n");
            }


            Phrase f9 = new Phrase("La empresa ha dado en préstamo el equipo al Empleado como una herramienta de trabajo, que es de la Empresa, y que en ningún caso podrá ser considerada de propiedad del Empleado. Dado lo anterior, el Empleado declara y se compromete con la Empresa a: " + "\r\n" + "\r\n", textFontBlack);
            Phrase f10 = new Phrase("a) Que reconoce a la Empresa la propiedad sobre el Equipo." + "\r\n" + "\r\n", textFontBlack);
            Phrase f11 = new Phrase("b) Que se obliga a cuidar y proteger el Equipo de cualquier posible daño, pérdida o deterioro. " + "\r\n" + "\r\n", textFontBlack);
            Phrase f12 = new Phrase("c) Que conoce y acepta que el uso del Equipo queda restringido única y  exclusivamente a tareas encomendadas y/o permitidas por la Empresa, y que no podrá ser usado para otros fines distintos a los anteriores. " + "\r\n" + "\r\n", textFontBlack);
            Phrase f13 = new Phrase("d) Que le está prohibido instalar software no autorizados por la Empresa y/o que no cuenten con la licencia respectiva y de propiedad de la empresa. " + "\r\n" + "\r\n", textFontBlack);
            Phrase f14 = new Phrase("e) En caso de pérdida del equipo, ya sea por hurto, robo u otros y; en caso de cualquier daño al Equipo que produzca su deterioro en forma considerable, y que dicha pérdida o deterioro provenga de un uso irresponsable del mismo por parte del Empleado, éste se obliga a pagar a la Empresa, el valor del Equipo, al valor que el Equipo tenga en el mercado. Se entenderá por uso irresponsable del Equipo por parte del Empleado, cualquier utilización del mismo que no haya sido autorizada expresamente por la empresa, o que habiendo sido autorizada, haya sido realizado en condiciones de riesgo para el equipo. Asimismo, se considera uso irresponsable del Empleado, cualquier acción descuidada del mismo que exponga a riesgo de pérdida o deterioro el equipo. " + "\r\n" + "\r\n", textFontBlack);


            Phrase f15 = new Phrase();

            PdfPTable tabla_firmas = new PdfPTable(5);
            tabla_firmas.HorizontalAlignment = 0;
            tabla_firmas.WidthPercentage = 100f;
            tabla_firmas.DefaultCell.BorderWidth = 0;
            tabla_firmas.DefaultCell.Border = Rectangle.NO_BORDER;

            float[] widths3 = new float[] { 30f, 5f, 30f, 5f, 30f};
            tabla_firmas.SetWidths(widths3);
            
            Phrase f16 = new Phrase("R.U.T.", textFontBlack);
            Phrase f17 = new Phrase("Firma", textFontBlack);
            Phrase f18 = new Phrase("Fecha", textFontBlack);

            PdfPCell cell1 = new PdfPCell();
            cell1.HorizontalAlignment = 1;
            cell1.FixedHeight = 40f;
            cell1.Border = 0;

            PdfPCell cell2 = new PdfPCell();
            cell2.HorizontalAlignment = 1;
            cell2.FixedHeight = 40f;
            cell2.Border = 0;

            PdfPCell cell3 = new PdfPCell();
            cell3.HorizontalAlignment = 1;
            cell3.FixedHeight = 40f;
            cell3.Border = 0;

            PdfPCell cell4 = new PdfPCell();
            cell4.HorizontalAlignment = 1;
            cell4.FixedHeight = 40f;
            cell4.Border = 0;

            PdfPCell cell5 = new PdfPCell();
            cell5.HorizontalAlignment = 1;
            cell5.FixedHeight = 40f;
            cell5.Border = 0;

            PdfPCell cell6 = new PdfPCell(f16);
            cell6.HorizontalAlignment = 1;
            cell6.FixedHeight = 0;
            cell6.Border = PdfPCell.TOP_BORDER;

            PdfPCell cell7 = new PdfPCell();
            cell7.HorizontalAlignment = 1;
            cell7.FixedHeight = 0;
            cell7.Border = 0;

            PdfPCell cell8 = new PdfPCell(f17);
            cell8.HorizontalAlignment = 1;
            cell8.FixedHeight = 0;
            cell8.Border = PdfPCell.TOP_BORDER;

            PdfPCell cell9 = new PdfPCell();
            cell9.HorizontalAlignment = 1;
            cell9.FixedHeight = 0;
            cell9.Border = 0;

            PdfPCell cell10 = new PdfPCell(f18);
            cell10.HorizontalAlignment = 1;
            cell10.FixedHeight = 0;
            cell10.Border = PdfPCell.TOP_BORDER;


            tabla_firmas.AddCell(cell1);
            tabla_firmas.AddCell(cell2);
            tabla_firmas.AddCell(cell3);
            tabla_firmas.AddCell(cell4);
            tabla_firmas.AddCell(cell5);
            tabla_firmas.AddCell(cell6);
            tabla_firmas.AddCell(cell7);
            tabla_firmas.AddCell(cell8);
            tabla_firmas.AddCell(cell9);
            tabla_firmas.AddCell(cell10);


            // Tabla con accesorios
            f15.Add(tabla_firmas);
            //f15.Add("\r\n");
            

            Paragraph p = new Paragraph();
            //p.Add(f0); 
            p.Add(f1);
            p.Add(f2);
            p.Add(f3);
            p.Add(f4);
            //p.Add(f8);
            p.Add(f9);
            p.Add(f10);
            p.Add(f11);
            p.Add(f12);
            p.Add(f13);
            p.Add(f14);
            p.Add(f15);

            return p;
        }


        public static Paragraph ConstruirHojaEntrega(oEquipo equipo) // 0
        {
            string dia_e = string.Empty, mes_e = string.Empty, anio_e = string.Empty, fechaD = string.Empty;

            Chunk c1 = new Chunk();

            if (equipo.FechaDistribucion.HasValue)
            {
                dia_e = equipo.FechaDistribucion.Value.Day.ToString();
                //mes_e = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(equipo.FechaDistribucion.Value.ToString("MMMM"));
                mes_e = equipo.FechaDistribucion.Value.Month.ToString();
                anio_e = equipo.FechaDistribucion.Value.Year.ToString();

                c1 = new Chunk(dia_e + "-" + mes_e + "-" + anio_e);
            }
            else
            {
                c1 = new Chunk("     ");
                c1.SetBackground(colorcampoausente);
            }


            PdfPTable tabla_proyecto = new PdfPTable(2);
            tabla_proyecto.WidthPercentage = 100f;

            float[] widths1 = new float[] { 25f, 75f };
            tabla_proyecto.SetWidths(widths1);

            tabla_proyecto.AddCell("Fecha: ");
            tabla_proyecto.AddCell(new Phrase(c1));

            PdfPCell cell000 = new PdfPCell(new Phrase("Empresa"));
            cell000.Colspan = 2; // colspan 
            cell000.HorizontalAlignment = 1;
            cell000.BackgroundColor = colorcampotitulo;
            tabla_proyecto.AddCell(cell000);

            tabla_proyecto.AddCell("Empresa: ");

            Chunk c55 = new Chunk();
            if (!string.IsNullOrEmpty(equipo.MacroEmpresaNombre))
                c55 = new Chunk(equipo.MacroEmpresaNombre.ToUpper());
            else
            {
                c55 = new Chunk("     ");
                c55.SetBackground(colorcampoausente);
            }
            tabla_proyecto.AddCell(new Phrase(c55));

            tabla_proyecto.AddCell("Razón Social:");
            //tabla_proyecto.AddCell(equipo.EmpresaRazonSocial);
            Chunk c56 = new Chunk();
            if (!string.IsNullOrEmpty(equipo.EmpresaRazonSocial))
                c56 = new Chunk(equipo.EmpresaRazonSocial.ToUpper());
            else
            {
                c56 = new Chunk("     ");
                c56.SetBackground(colorcampoausente);
            }
            tabla_proyecto.AddCell(new Phrase(c56));

            tabla_proyecto.AddCell("R.U.T. Razón Social: ");
            //tabla_proyecto.AddCell(equipo.EmpresaRut);
            Chunk c57 = new Chunk();
            if (!string.IsNullOrEmpty(equipo.EmpresaRut))
                c57 = new Chunk(equipo.EmpresaRut.ToUpper());
            else
            {
                c57 = new Chunk("     ");
                c57.SetBackground(colorcampoausente);
            }
            tabla_proyecto.AddCell(new Phrase(c57));

            PdfPCell cell001 = new PdfPCell(new Phrase("Proyecto"));
            cell001.Colspan = 2; // colspan 
            cell001.HorizontalAlignment = 1;
            cell001.BackgroundColor = colorcampotitulo;
            tabla_proyecto.AddCell(cell001);

            tabla_proyecto.AddCell("Proyecto: ");
            //tabla_proyecto.AddCell(equipo.ProyectoNombre);
            Chunk c58 = new Chunk();
            if (!string.IsNullOrEmpty(equipo.ProyectoNombre))
                c58 = new Chunk(equipo.ProyectoNombre.ToUpper());
            else
            {
                c58 = new Chunk("     ");
                c58.SetBackground(colorcampoausente);
            }
            tabla_proyecto.AddCell(new Phrase(c58));

            tabla_proyecto.AddCell("Comuna: ");
            //tabla_proyecto.AddCell(equipo.ComunaNombre);
            Chunk c59 = new Chunk();
            if (!string.IsNullOrEmpty(equipo.ComunaNombre))
                c59 = new Chunk(equipo.ComunaNombre.ToUpper());
            else
            {
                c59 = new Chunk("     ");
                c59.SetBackground(colorcampoausente);
            }
            tabla_proyecto.AddCell(new Phrase(c59));

            tabla_proyecto.AddCell("Centro Costo: ");
            //tabla_proyecto.AddCell(equipo.CentroCostoNombre);
            Chunk c60 = new Chunk();
            if (!string.IsNullOrEmpty(equipo.CentroCostoNombre))
                c60 = new Chunk(equipo.CentroCostoNombre.ToUpper());
            else
            {
                c60 = new Chunk("     ");
                c60.SetBackground(colorcampoausente);
            }
            tabla_proyecto.AddCell(new Phrase(c60));

            tabla_proyecto.AddCell("Instalación: ");
            //tabla_proyecto.AddCell(equipo.UbicacionNombre.ToUpper());
            Chunk c61 = new Chunk();
            if (!string.IsNullOrEmpty(equipo.UbicacionNombre))
                c61 = new Chunk(equipo.UbicacionNombre.ToUpper());
            else
            {
                c61 = new Chunk("     ");
                c61.SetBackground(colorcampoausente);
            }
            tabla_proyecto.AddCell(new Phrase(c61));

            PdfPCell cell002 = new PdfPCell(new Phrase("Colaboradores"));
            cell002.Colspan = 2; // colspan 
            cell002.HorizontalAlignment = 1;
            cell002.BackgroundColor = colorcampotitulo;
            tabla_proyecto.AddCell(cell002);

            tabla_proyecto.AddCell("Gerente: ");
            //tabla_proyecto.AddCell(equipo.GerenteEmail.ToUpper());
            Chunk c63 = new Chunk();
            if (!string.IsNullOrEmpty(equipo.GerenteEmail))
                c63 = new Chunk(equipo.GerenteEmail.ToUpper());
            else
            {
                c63 = new Chunk("     ");
                c63.SetBackground(colorcampoausente);
            }
            tabla_proyecto.AddCell(new Phrase(c63));

            tabla_proyecto.AddCell("Solicitante: ");
            Chunk c64 = new Chunk();
            if (!string.IsNullOrEmpty(equipo.SolicitanteEmail))
                c64 = new Chunk(equipo.SolicitanteEmail.ToUpper());
            else
            {
                // campo opcional
                c64 = new Chunk("     ");
                //c64.SetBackground(colorcampoausente);
            }
            tabla_proyecto.AddCell(new Phrase(c64));

            tabla_proyecto.AddCell("Usuario Final: ");
            //tabla_proyecto.AddCell(equipo.UsuarioFinalEmail.ToUpper());
            Chunk c65 = new Chunk();
            if (!string.IsNullOrEmpty(equipo.UsuarioFinalEmail))
                c65 = new Chunk(equipo.UsuarioFinalEmail.ToUpper());
            else
            {
                c65 = new Chunk("     ");
                c65.SetBackground(colorcampoausente);
            }
            tabla_proyecto.AddCell(new Phrase(c65));

            // Tabla con caracteristicas del equipo
            Phrase f2 = new Phrase();
            f2.Add(tabla_proyecto);
            f2.Add("\r\n");



            //PdfPTable tabla_usuarios = new PdfPTable(2);
            //tabla_proyecto.WidthPercentage = 100f;

            //float[] widths4 = new float[] { 25f, 75f };
            //tabla_usuarios.SetWidths(widths4);

            //tabla_usuarios.AddCell("Gerente: ");
            //tabla_usuarios.AddCell(equipo.GerenteEmail);
            //tabla_usuarios.AddCell("Solicitante: ");
            //tabla_usuarios.AddCell(equipo.SolicitanteEmail);
            //tabla_usuarios.AddCell("Usuario Final: ");
            //tabla_usuarios.AddCell(equipo.UsuarioFinalEmail);

            //// Tabla con caracteristicas del equipo
            //Phrase f5 = new Phrase();
            //f5.Add(tabla_usuarios);
            //f5.Add("\r\n");



            Chunk c9 = new Chunk();

            if ((equipo.Mouse == true) || (equipo.Bolso == true) || (equipo.Candado == true))
            {
                //f4 = new Phrase("Además se incluyen los siguientes accesorios:" + "\r\n");

                string accesorios = string.Empty;

                if (equipo.Mouse == true)
                    accesorios = accesorios + "Mouse, "; //c9 = new Chunk("Mouse, ");
                if (equipo.Bolso == true)
                    accesorios = accesorios + "Bolso, ";  //c10 = new Chunk("Bolso, ");
                if (equipo.Candado == true)
                    accesorios = accesorios + "Candado, ";  //c11 = new Chunk("Candado, ");

                accesorios = accesorios.Remove(accesorios.Length - 2, 2);

                c9 = new Chunk(accesorios, ReSizeFont);
            }
            else
                c9 = new Chunk("");




            PdfPTable tabla_detalle_equipo = new PdfPTable(8);
            tabla_detalle_equipo.WidthPercentage = 100f;

            float[] widths2 = new float[] { 12f, 12f , 12f, 12f , 12f, 12f , 12f, 12f };
            tabla_detalle_equipo.SetWidths(widths2);
            
            PdfPCell cell003 = new PdfPCell(new Phrase("Equipo(s)"));
            cell003.Colspan = 8; // colspan 
            cell003.HorizontalAlignment = 1;
            cell003.BackgroundColor = colorcampotitulo;
            tabla_detalle_equipo.AddCell(cell003);

            PdfPCell cell11 = new PdfPCell(new Phrase("Tipo", ReSizeFont));
            cell11.BackgroundColor = colorcampoencabezado;
            tabla_detalle_equipo.AddCell(cell11);

            PdfPCell cell12 = new PdfPCell(new Phrase("Marca", ReSizeFont));
            cell12.BackgroundColor = colorcampoencabezado;
            tabla_detalle_equipo.AddCell(cell12);

            PdfPCell cell13 = new PdfPCell(new Phrase("Modelo", ReSizeFont));
            cell13.BackgroundColor = colorcampoencabezado;
            tabla_detalle_equipo.AddCell(cell13);

            PdfPCell cell14 = new PdfPCell(new Phrase("Nro. Serie", ReSizeFont));
            cell14.BackgroundColor = colorcampoencabezado;
            tabla_detalle_equipo.AddCell(cell14);

            PdfPCell cell15 = new PdfPCell(new Phrase("Procesador", ReSizeFont));
            cell15.BackgroundColor = colorcampoencabezado;
            tabla_detalle_equipo.AddCell(cell15);

            PdfPCell cell16 = new PdfPCell(new Phrase("Memoria", ReSizeFont));
            cell16.BackgroundColor = colorcampoencabezado;
            tabla_detalle_equipo.AddCell(cell16);

            PdfPCell cell17 = new PdfPCell(new Phrase("Disco Duro", ReSizeFont));
            cell17.BackgroundColor = colorcampoencabezado;
            tabla_detalle_equipo.AddCell(cell17);

            PdfPCell cell18 = new PdfPCell(new Phrase("Accesorios", ReSizeFont));
            cell18.BackgroundColor = colorcampoencabezado;
            tabla_detalle_equipo.AddCell(cell18);

            tabla_detalle_equipo.AddCell(new Phrase(equipo.TipoNombre, ReSizeFont));
            tabla_detalle_equipo.AddCell(new Phrase(equipo.MarcaNombre, ReSizeFont));
            tabla_detalle_equipo.AddCell(new Phrase(equipo.ModeloNombre, ReSizeFont));
            tabla_detalle_equipo.AddCell(new Phrase(equipo.NumeroSerie, ReSizeFont));
            tabla_detalle_equipo.AddCell(new Phrase(equipo.Procesador, ReSizeFont));
            tabla_detalle_equipo.AddCell(new Phrase(equipo.Memoria, ReSizeFont));
            tabla_detalle_equipo.AddCell(new Phrase(equipo.DiscoDuro, ReSizeFont));
            tabla_detalle_equipo.AddCell(new Phrase(c9));

            //tabla_detalle_equipo.AddCell("Tipo: ");
            //tabla_detalle_equipo.AddCell(equipo.TipoNombre);
            //tabla_detalle_equipo.AddCell("Marca: ");
            //tabla_detalle_equipo.AddCell(equipo.MarcaNombre);
            //tabla_detalle_equipo.AddCell("Modelo: ");
            //tabla_detalle_equipo.AddCell(equipo.ModeloNombre);
            //tabla_detalle_equipo.AddCell("Nro. Serie: ");
            //tabla_detalle_equipo.AddCell(equipo.NumeroSerie);

            //tabla_detalle_equipo.AddCell("Procesador: ");
            //tabla_detalle_equipo.AddCell(equipo.NumeroSerie);
            //tabla_detalle_equipo.AddCell("Memoria: ");
            //tabla_detalle_equipo.AddCell(equipo.NumeroSerie);
            //tabla_detalle_equipo.AddCell("Disco Duro: ");
            //tabla_detalle_equipo.AddCell(equipo.NumeroSerie);
            //tabla_detalle_equipo.AddCell("Accesorios: ");
            //tabla_detalle_equipo.AddCell(equipo.NumeroSerie);


            // Tabla con caracteristicas del equipo
            Phrase f3 = new Phrase();
            f3.Add(tabla_detalle_equipo);
            f3.Add("\r\n");
            

            Phrase f15 = new Phrase();

            PdfPTable tabla_firmas = new PdfPTable(5);
            tabla_firmas.HorizontalAlignment = 0;
            tabla_firmas.WidthPercentage = 100f;
            tabla_firmas.DefaultCell.BorderWidth = 0;
            tabla_firmas.DefaultCell.Border = Rectangle.NO_BORDER;

            float[] widths3 = new float[] { 30f, 5f, 30f, 5f, 30f };
            tabla_firmas.SetWidths(widths3);

            Phrase f16 = new Phrase("R.U.T.");
            Phrase f17 = new Phrase("Nombre");
            Phrase f18 = new Phrase("Firma");

            PdfPCell cell1 = new PdfPCell();
            cell1.HorizontalAlignment = 1;
            cell1.FixedHeight = 40f;
            cell1.Border = 0;

            PdfPCell cell2 = new PdfPCell();
            cell2.HorizontalAlignment = 1;
            cell2.FixedHeight = 40f;
            cell2.Border = 0;

            PdfPCell cell3 = new PdfPCell();
            cell3.HorizontalAlignment = 1;
            cell3.FixedHeight = 40f;
            cell3.Border = 0;

            PdfPCell cell4 = new PdfPCell();
            cell4.HorizontalAlignment = 1;
            cell4.FixedHeight = 40f;
            cell4.Border = 0;

            PdfPCell cell5 = new PdfPCell();
            cell5.HorizontalAlignment = 1;
            cell5.FixedHeight = 40f;
            cell5.Border = 0;

            PdfPCell cell6 = new PdfPCell(f16);
            cell6.HorizontalAlignment = 1;
            cell6.FixedHeight = 0;
            cell6.Border = PdfPCell.TOP_BORDER;

            PdfPCell cell7 = new PdfPCell();
            cell7.HorizontalAlignment = 1;
            cell7.FixedHeight = 0;
            cell7.Border = 0;

            PdfPCell cell8 = new PdfPCell(f17);
            cell8.HorizontalAlignment = 1;
            cell8.FixedHeight = 0;
            cell8.Border = PdfPCell.TOP_BORDER;

            PdfPCell cell9 = new PdfPCell();
            cell9.HorizontalAlignment = 1;
            cell9.FixedHeight = 0;
            cell9.Border = 0;

            PdfPCell cell10 = new PdfPCell(f18);
            cell10.HorizontalAlignment = 1;
            cell10.FixedHeight = 0;
            cell10.Border = PdfPCell.TOP_BORDER;


            tabla_firmas.AddCell(cell1);
            tabla_firmas.AddCell(cell2);
            tabla_firmas.AddCell(cell3);
            tabla_firmas.AddCell(cell4);
            tabla_firmas.AddCell(cell5);
            tabla_firmas.AddCell(cell6);
            tabla_firmas.AddCell(cell7);
            tabla_firmas.AddCell(cell8);
            tabla_firmas.AddCell(cell9);
            tabla_firmas.AddCell(cell10);


            // Tabla con accesorios
            f15.Add(tabla_firmas);
            f15.Add("\r\n");


            Paragraph p = new Paragraph();
            //p.Add(f0); 
            //p.Add(f1);
            p.Add(f2);
            //p.Add(f5);
            p.Add(f3);

            p.Add(f15);

            return p;
        }


        public static Paragraph ConstruirRastro()
        {
            string idred = string.Empty, fecha = string.Empty;

            if (!string.IsNullOrEmpty(@System.Environment.UserName))
                idred = @System.Environment.UserName.ToString();

            fecha = DateTime.Now.ToString("dd'-'MM'-'yyyy' 'HH':'mm");
            //fecha = DateTime.Now.ToString("dd'-'MM'-'yyyy'H'HH':'mm':'ss");

            Phrase f0 = new Phrase("[" + idred + " " + fecha + "]", textFontBlack);

            Paragraph p = new Paragraph();
            p.Add(f0);

            return p;
        }



        public static string Crear_PDF(string filein, Paragraph titulo, Paragraph documento, Paragraph rastro, iTextSharp.text.Image logo)
        {

            ////string extension = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");
            //string extension = DateTime.Now.ToString("'F'" + "dd'-'MM'-'yyyy'H'HH':'mm':'ss");
            //extension = extension.Replace(":", "");
            //extension = extension.Replace("-", "");
            //extension = "-" + extension;
            //string fileName = @"C:\Windows\Temp\test1.pdf";
            //string fileName = @"C:\Windows\Temp\" + Encabezado.numerooperacion + Encabezado.extension + ".pdf";
            FileStream fs = new FileStream(filein, FileMode.Create, FileAccess.Write, FileShare.None);
            //FileStream fs = new FileStream("Chapter1_Example1.pdf", FileMode.Create, FileAccess.Write, FileShare.None);
            Document doc = new Document();
            PdfWriter writer = PdfWriter.GetInstance(doc, fs);

            doc.Open();

            doc.AddTitle("Inventario Soporte Sistemas");
            doc.AddSubject("Generación Automática de Documentos");
            doc.AddKeywords("Contrato, Responsabilidad, Hoja, Entrega, Equipo");
            doc.AddCreator("iTextSharp 5.4.4");
            doc.AddAuthor("Victor Sandoval N.");
            doc.AddHeader("Nothing", " ");

            doc.Add(logo);
            doc.Add(titulo);
            doc.Add(documento);
            doc.Add(rastro);

            doc.Close();

            //byte[] bytes = System.IO.File.ReadAllBytes("Chapter1_Example1.pdf");
            //byte[] bytes = System.IO.File.ReadAllBytes(fileName);
            //return fileName;
            return filein;
        }


        public static byte[] Crear_BYTE(string PDF, out string error)
        {
            //string fileName = @"C:\Windows\Temp\test1.pdf";
            //FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
            ////FileStream fs = new FileStream("Chapter1_Example1.pdf", FileMode.Create, FileAccess.Write, FileShare.None);
            //Document doc = new Document();
            //PdfWriter writer = PdfWriter.GetInstance(doc, fs);
            //doc.Open();

            //doc.AddTitle("Hello World example");
            //doc.AddSubject("This is an Example 4 of Chapter 1 of Book 'iText in Action'");
            //doc.AddKeywords("Metadata, iTextSharp 5.4.4, Chapter 1, Tutorial");
            //doc.AddCreator("iTextSharp 5.4.4");
            //doc.AddAuthor("Debopam Pal");
            //doc.AddHeader("Nothing", "No Header");

            //doc.Add(new Paragraph("PROMESA DE COMPRAVENTA"));
            //doc.Add(new Paragraph(Encabezado));
            //doc.Close();

            //byte[] bytes = System.IO.File.ReadAllBytes("Chapter1_Example1.pdf");

            byte[] bytes;

            try
            {
                bytes = System.IO.File.ReadAllBytes(PDF);
                error = string.Empty;
            }
            catch (Exception er)
            {
                bytes = null;
                error = er.Message;
            }

            return bytes;
        }


        [HttpGet]
        public ActionResult Generar_PDF_Hoja_Entrega(string numeroserie)
        {
            return Generar_PDF(numeroserie, "H");
        }

        


    }
}