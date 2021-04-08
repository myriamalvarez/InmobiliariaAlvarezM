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
    public class InquilinoController : Controller
    {
        private readonly RepositorioInquilino repositorio;
        private readonly IConfiguration configuration;

        public InquilinoController(IConfiguration configuration)
        {
            this.repositorio = new RepositorioInquilino(configuration);
            this.configuration = configuration;
        }
        // GET: InquilinoController
        public ActionResult Index()
        {
            IList<Inquilino> lista = repositorio.ObtenerTodos();
            return View(lista);
        }

        // GET: InquilinoController/Details/5
        public ActionResult Details(int id)
        {
            var e = repositorio.ObtenerPorId(id);
            return View(e);
        }

        // GET: InquilinoController/Create
        public ActionResult Create()
        {
            
            return View();
        }

        // POST: InquilinoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inquilino e)
        {
            try
            {
                int res = repositorio.Alta(e);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: InquilinoController/Edit/5
        public ActionResult Edit(int id)
        {
            var e = repositorio.ObtenerPorId(id);
            e.IdInquilino = id;
            return View(e);
        }

        // POST: InquilinoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            Inquilino e = null;
            try
            {
                e = repositorio.ObtenerPorId(id);
                e.Nombre = collection["Nombre"];
                e.Apellido = collection["Apellido"];
                e.Dni = collection["Dni"];
                e.Direccion = collection["Direccion"];
                e.Telefono = collection["Telefono"];
                e.DireccionLaboral = collection["DireccionLaboral"];
                e.NombreGarante = collection["NombreGarante"];
                e.ApellidoGarante = collection["ApellidoGarante"];
                e.DniGarante = collection["DniGarante"];

                repositorio.Modificacion(e);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return View(e);
            }
        }

        // GET: InquilinoController/Delete/5
        public ActionResult Delete(int id)
        {
            var e = repositorio.ObtenerPorId(id);
            return View(e);
        }

        // POST: InquilinoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
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
