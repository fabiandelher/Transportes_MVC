using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Antlr.Runtime.Misc;
using DTO;
using Transportes_MVC.Models;

namespace Transportes_MVC.Controllers
{
    public class CamionesController : Controller
    {
        // GET: Camiones
        public ActionResult Index()
        {
            //crear una lista de camiones del modelo original 
            List<Camiones> lista_camiones = new List<Camiones>();
            //lleno la lista con elementos existentes dentro del contexto (BD) Utilizando EF y LinQ
            using(TransportesEntities context = new TransportesEntities())
            {
                //Llleno mi lista directamente usando LinQ
                //lista_camiones = (from camion in context.Camiones select camion ).ToList();
                //otra forma de usar LinQ es:
                lista_camiones = (from camion in context.Camiones select camion).ToList();
                //otra forma de hacerlo 
                //foreach (Camiones cam in context.Camiones)
            }
            //ViweBag (Forma parte de razor) se caracteriza por hacer uso de una propiedad arbitraria que sirve para pasar información desde el controladolr a la vista 
            ViewBag.Titulo = "Lista de Camiones";
            ViewBag.Subtitulo = "Utilizando ASP.NET MVC";

            //ViewData se caracteriza por hacer uso de un atributo arbitrario y tiene el mismo funcionamiento que el ViewBag
            ViewData["Ttitulo2"] = "Segundo Título";

            //TempData se caracteriza por permitir crear variables temporales que existen durante la ejecución del Runtime de ASP 
            //Además, los temdata me permite compartir información no solo del controlador a la Vista, sino también en otras vistas y otros controladores 
            //TempData.Add("Clave", "Valor");

            //Retorno la vista con los datos del modelo
            return View(lista_camiones);
        }

        //GET: Nuevo_Camion
        public ActionResult Nuevo_Camion()
        {
            ViewBag.Titulo = "Nuevo Camion";
            //cargar los DDL con las opciones del tipo camión
            cargarDDL();
            return View();
        }


        //Post: Nuevo_Camion
        [HttpPost]
        public ActionResult Nuevo_Camion(CamionesDTO model, HttpPostedFileBase imagen)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (TransportesEntities context = new TransportesEntities())
                    {
                        var camion = new Camiones();

                        camion.Matricula = model.Matricula;
                        camion.Marca = model.Marca;
                        camion.Modelo = model.Modelo;
                        camion.Tipo_Camion = model.Tipo_Camion;
                        camion.Capacidad = model.Capacidad;
                        camion.Kilometraje = model.Kilometraje;
                        camion.Disponibilidad = model.Disponibilidad;

                        //valido si existe un imagen en la petición 
                        if (imagen != null && imagen.ContentLength > 0)
                        {
                            string filename = Path.GetFileName(imagen.FileName);//recupero el nombre de la imagen que viene de l apetición
                            string pathdir = Server.MapPath("~/Assets/Imagenes/Camiones/");//mapeo la ruta y el nombre del archivo para enviarlo a la BD


                            if (!Directory.Exists(pathdir))//si no existe el directorio lo niega para crearlo 
                            {
                                Directory.CreateDirectory(pathdir);
                            }
                            imagen.SaveAs(pathdir + filename);
                            camion.UrlFoto = "/Assets/Imagenes/Camiones/" + filename;

                            context.Camiones.Add(camion);
                            context.SaveChanges();

                                return RedirectToAction("Index");


                        }
                        else
                        {
                            cargarDDL();
                            return View(model);
                        }
                    }

                }
                else
                {
                    cargarDDL();
                    return View(model);
                }

            }
            catch (Exception ex)
            {
                {
                    //en caso de que ocurra una excepción, voy a mostrar un mensaje con un error (Sweet Alert), voy a decolver a la vista el modelo que causo el conflicto (return View (model)) y vuelvo a cargar el DDL para que estén disponibles esas opciones (cargarDDL())
                    cargarDDL();
                    return View(model);
                }
            }
        }


        //GET Editar 
        public ActionResult Editar_Camion(int id)
        {
            if(id > 0)
            {
                CamionesDTO camion = new CamionesDTO();//creo una instancia del tipo DTO para pasar información desde el contexto a la vista con ayuda de EF y LinQ
                using (TransportesEntities context = new TransportesEntities())//creo una instancia de un solo contexto 
                {
                    var camion_aux = context.Camiones.Where(x => x.ID_Camion == id).FirstOrDefault();
                    var camion_aux2 = context.Camiones.FirstOrDefault(x => x.ID_Camion == id);

                    camion.Matricula = camion_aux.Matricula;
                    camion.Marca = camion_aux.Marca;
                    camion.Modelo = camion_aux.Modelo;
                    camion.Capacidad = camion_aux.Capacidad;
                    camion.Kilometraje = camion_aux.Kilometraje;
                    camion.Tipo_Camion = camion_aux.Tipo_Camion;
                    camion.Disponibilidad = camion_aux.Disponibilidad;
                    camion.UrlFoto = camion_aux.UrlFoto;
                    camion.ID_Camion= camion_aux.ID_Camion;

                 camion = (from c in context .Camiones
                           where c.ID_Camion == id
                           select new CamionesDTO()
                           {
                               ID_Camion = c.ID_Camion,
                               Matricula = c.Matricula,
                               Marca = c.Marca,
                               Modelo = c.Modelo,
                               Capacidad = c.Capacidad,
                               Kilometraje = c.Kilometraje,
                               Tipo_Camion = c.Tipo_Camion,
                               Disponibilidad  = c.Disponibilidad,
                               UrlFoto = c.UrlFoto,


                    }).FirstOrDefault();
                }
                if (camion == null)
                {
                    return RedirectToAction("Index");
                }
                ViewBag.Titulo = $"Editar Camión °{camion.ID_Camion}";
                cargarDDL();
                return View(camion);
            }
            else
            {
                //Sweet Alert 
                return RedirectToAction("Index");
            }
        }
        //POST: Editar_Camion
        [HttpPost]
        public ActionResult Editar_Camion(CamionesDTO model, HttpPostedFileBase imagen)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (TransportesEntities context = new TransportesEntities())
                    {
                        var camion = new Camiones();

                        camion.ID_Camion = model.ID_Camion;
                        camion.Matricula = model.Matricula;
                        camion.Marca = model.Marca;
                        camion.Modelo = model.Modelo;
                        camion.Capacidad = model.Capacidad;
                        camion.Tipo_Camion = model.Tipo_Camion;
                        camion.Disponibilidad = model.Disponibilidad;
                        camion.Kilometraje = model.Kilometraje;

                        if (imagen != null && imagen.ContentLength > 0)
                        {
                            string filename = Path.GetFileName(imagen.FileName);
                            string pathdir = Server.MapPath("~/Assets/Imagenes/Camiones/");
                            if (model.UrlFoto.Length == 0)
                            {
                                //la imagen en la BD es null y hay que darle la imagen
                                if (!Directory.Exists(pathdir))
                                {
                                    Directory.CreateDirectory(pathdir);
                                }

                                imagen.SaveAs(pathdir + filename);
                                camion.UrlFoto = "/Assets/Imagenes/Camiones/" + filename;
                            }
                            else
                            {
                                //validar si es la misma o es nueva
                                if (model.UrlFoto.Contains(filename))
                                {
                                    //es la misma
                                    camion.UrlFoto = "/Assets/Imagenes/Camiones/" + filename;
                                }
                                else
                                {
                                    //es diferente
                                    if (!Directory.Exists(pathdir))
                                    {
                                        Directory.CreateDirectory(pathdir);
                                    }

                                    //Borro la imagen anterios
                                    //valido si existe

                                    try
                                    {
                                        string pathdir_old = Server.MapPath("~" + model.UrlFoto); //busco la imagen que catualmente tiene el camión
                                        if (System.IO.File.Exists(pathdir_old)) //valido si existe dicho archivo
                                        {
                                            //procedo a eliminarlo
                                            System.IO.File.Delete(pathdir_old);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        //Sweet Alert
                                    }

                                    imagen.SaveAs(pathdir + filename);
                                    camion.UrlFoto = "/Assets/Imagenes/Camiones/" + filename;
                                }
                            }
                        }
                        else //si no hya una nueva imagen, paso la misma
                        {
                            camion.UrlFoto = model.UrlFoto;
                        }

                        //Guardar cambios, validar excepciones, redirigir
                        //actualizar el estado de nuestro elemento
                        //.Entry() registrar la entrada de nueva información al contexto y notificar un cambio de estado usando System.Data.Entity.EntityState.Modified
                        context.Entry(camion).State = System.Data.Entity.EntityState.Modified;
                        //impactamos la BD
                        try
                        {
                            context.SaveChanges();
                        }
                        //agregar using desde 'using System.Data.Entity.Validation;'
                        catch (DbEntityValidationException ex)
                        {
                            string resp = "";
                            //recorro todos los posibles errores de la Entidad Referencial
                            foreach (var error in ex.EntityValidationErrors)
                            {
                                //recorro los detalles de cada error
                                foreach (var validationError in error.ValidationErrors)
                                {
                                    resp += "Error en la Entidad: " + error.Entry.Entity.GetType().Name;
                                    resp += validationError.PropertyName;
                                    resp += validationError.ErrorMessage;
                                }
                            }
                            //Sweet Alert
                        }
                        //Sweet Alert
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    //Sweet Alert
                    cargarDDL();
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                //Sweet Alert
                cargarDDL();
                return View(model);
            }
        }

        #region Auxiliares
        private class Opciones
        {
            public string Nuemro { get; set; }
            public string Descripcion { get; set; }
        }
        public void cargarDDL()
        {
            List<Opciones> lista_opciones = new List<Opciones>()
            {
                new Opciones(){Nuemro = "0", Descripcion="Seleccione una opción"},
                 new Opciones(){Nuemro = "1", Descripcion="Volteo"},
                  new Opciones(){Nuemro = "2", Descripcion="Redilas"},
                   new Opciones(){Nuemro = "3", Descripcion="Transporte"}
            };

            ViewBag.ListarTipos = lista_opciones; 
        }
        #endregion
    }
}