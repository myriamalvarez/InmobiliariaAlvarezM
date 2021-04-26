using InmobiliariaAlvarezM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaAlvarezM.Controllers
{
    public class PagoController : Controller
    {
        private readonly IConfiguration configuration;
        //private readonly IWebHostEnvironment environment;
        private readonly IRepositorioPago repositorio;
        private readonly IRepositorioContrato repositorioContrato;
        private readonly IRepositorioInmueble repositorioInmueble;
        private readonly IRepositorioInquilino repositorioInquilino;
        public PagoController(IConfiguration configuration, IRepositorioPago repositorio, IRepositorioContrato repositorioContrato, IRepositorioInmueble repositorioInmueble, IRepositorioInquilino repositorioInquilino)
        {
            this.configuration = configuration;
            //this.environment = environment;
            this.repositorio = repositorio;
            this.repositorioContrato = repositorioContrato;
            this.repositorioInmueble = repositorioInmueble;
            this.repositorioInquilino = repositorioInquilino;
        }

        // GET: PagoController
        
        public ActionResult Index()
        {
            var lista = repositorio.ObtenerTodos();
            return View(lista);
        }
      
        // GET: PagoController/Details/5
        public ActionResult Details(int id)
        {
            var p = repositorio.ObtenerPorId(id);
            return View(p);
        }

        [Authorize(Policy = "Administrador")]
        public ActionResult Create(int id)  
            {
                ViewBag.Contrato = repositorioContrato.ObtenerTodos();
                return View();
            }

        [Authorize(Policy = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int id, Pago pago)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    repositorio.Alta(pago);
                    var lista = repositorio.ObtenerTodos();
                    return View("Index", lista);
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }


        // GET: PagoController/Edit/5
        public ActionResult Edit(int id)
        {

            var entidad = repositorio.ObtenerPorId(id);
            ViewBag.Inquilino = repositorioInquilino.ObtenerTodos();
            ViewBag.Inmueble = repositorioInmueble.ObtenerTodos();
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return View(entidad);
        }

        // POST: PagoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Pago entidad)
        {
            try
            {
                entidad.IdPago = id;
                repositorio.Modificacion(entidad);
                TempData["Mensaje"] = "Datos guardados correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View();
            }
        }

        // GET: PagoController/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            var entidad = repositorio.ObtenerPorId(id);
            ViewBag.Inquilino = repositorioInquilino.ObtenerTodos();
            ViewBag.Inmueble = repositorioInmueble.ObtenerTodos();
            TempData["IdContrato"] = entidad.IdContrato;
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return View(entidad);
        }


        // POST: PagoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, Pago entidad)
        {
            try
            {
                entidad.IdPago = id;
                repositorio.Baja(id);
                TempData["Mensaje"] = "Eliminacion realizada correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View();
            }
        }
        public ActionResult PorContrato(int id)
        {
            var lista = repositorio.BuscarPorContrato(id);
            if (TempData.ContainsKey("Id"))
                ViewBag.Id = TempData["Id"];
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            ViewBag.Contrato = repositorioContrato.ObtenerPorId(id);
            return View(lista);
        }

    }

}
