using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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