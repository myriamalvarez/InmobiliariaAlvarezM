using InmobiliariaAlvarezM.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaAlvarezM.Controllers
{
    public class InmuebleController : Controller
    {
        private readonly RepositorioInmueble repositorio;
        private RepositorioPropietario repositorioPropietario;
        private readonly IConfiguration configuration;

        public InmuebleController(IConfiguration configuration)
        {
            this.repositorio = new RepositorioInmueble(configuration);
            this.repositorioPropietario = new RepositorioPropietario(configuration);
            this.configuration = configuration;
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
    }
}
