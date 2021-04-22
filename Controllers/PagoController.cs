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
        private readonly IWebHostEnvironment environment;
        private readonly IRepositorioPago repositorio;
        private readonly IRepositorioContrato repositorioContrato;
        public PagoController(IConfiguration configuration, IWebHostEnvironment environment, IRepositorioPago repositorio, IRepositorioContrato repositorioContrato)
        {
            this.configuration = configuration;
            this.environment = environment;
            this.repositorio = repositorio;
            this.repositorioContrato = repositorioContrato;
        }

        // GET: PagoController
        public ActionResult Index(int id)
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

        // GET: PagoController/Create
        public ActionResult Create()
        {

            ViewBag.Contrato = repositorioContrato.ObtenerTodos();
            ViewBag.Pago = repositorio.ObtenerTodos();
            return View();
        }

        // POST: PagoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Pago pago)
        {
            try
            {
                repositorio.Alta(pago);
                TempData["Id"] = "Se creo el Pago";
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ViewBag.Contrato = repositorioContrato.ObtenerTodos();
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(pago);
            }
        }

        // GET: PagoController/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var e = repositorio.ObtenerPorId(id);
                return View(e);
            }
            catch(Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }

        }

        // POST: PagoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Pago e)
        {
            try
            {
                int res = repositorio.Modificacion(e);
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
            try
            {
                var pago = repositorio.ObtenerPorId(id);
                return View(pago);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        // POST: PagoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, Pago entidad)
        {
            try
            {
                int res = repositorio.Baja(id);
                TempData["Mensaje"] = "Eliminación realizada correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(entidad);
            }
        }

        public ActionResult PorContrato(int id)
        {
            var lista = repositorio.BuscarPorContrato(id);
            ViewBag.IdContrato = id;
            return View(lista);
        }
    }

}
