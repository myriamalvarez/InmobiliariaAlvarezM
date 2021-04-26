using InmobiliariaAlvarezM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaAlvarezM.Controllers
{
    [Authorize]
    public class InmuebleController : Controller
    {
        private readonly IRepositorioInmueble repositorio;
        private readonly IRepositorioPropietario repositorioPropietario;

        public InmuebleController(IRepositorioInmueble repositorio, IRepositorioPropietario repositorioPropietario)
        {
            this.repositorio = repositorio;
            this.repositorioPropietario = repositorioPropietario;
        }
        // GET: InmuebleController
        public ActionResult Index()
        {
            IList<Inmueble> lista = repositorio.ObtenerTodos();
            return View(lista);
        }

        // GET: InmuebleController/Details/5
        public ActionResult Details(int id)
        {
            var n = repositorio.ObtenerPorId(id);
            return View(n);
        }

        // GET: InmuebleController/Create
        public ActionResult Create()
        {
            ViewBag.Propietario = repositorioPropietario.ObtenerTodos();
            ViewBag.Estado = Inmueble.ObtenerEstado();
            return View();
        }

        // POST: InmuebleController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inmueble n)
        {
            try
            {
                repositorio.Alta(n);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: InmuebleController1/Edit/5
        public ActionResult Edit(int id)
        {
            var n = repositorio.ObtenerPorId(id);
            ViewBag.Propietario = repositorioPropietario.ObtenerTodos();
            ViewBag.Estado = Inmueble.ObtenerEstado();
            return View(n);
        }

        // POST: InmuebleController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Inmueble n)
        {
            try
            {
                n.IdInmueble = id;
                repositorio.Modificacion(n);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: InmuebleController1/Delete/5
        public ActionResult Delete(int id)
        {
            var n = repositorio.ObtenerPorId(id);
            return View(n);
        }

        // POST: InmuebleController1/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Inmueble n)
        {
            try
            {
                repositorio.Baja(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        public ActionResult MostrarInmueblesPorPropietario(int id)
        {
            try
            {
                var lista = repositorio.BuscarPorPropietario(id);
                ViewBag.IdPropietario = id;
                return View(lista);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }
    }
}
