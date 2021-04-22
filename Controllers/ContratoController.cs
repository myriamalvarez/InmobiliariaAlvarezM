using InmobiliariaAlvarezM.Models;using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;using Microsoft.AspNetCore.Mvc;using Microsoft.Extensions.Configuration;using System;using System.Collections.Generic;using System.Linq;using System.Threading.Tasks;namespace InmobiliariaAlvarezM.Controllers{    [Authorize]    public class ContratoController : Controller    {        private readonly IRepositorioContrato repositorio;
        private readonly IRepositorioInmueble repositorioInmueble;
        private readonly IRepositorioPropietario repositorioPropietario;
        private readonly IRepositorioInquilino repositorioInquilino;        public ContratoController(IRepositorioContrato repositorio, IRepositorioInmueble repositorioInmueble, IRepositorioPropietario repositorioPropietario, IRepositorioInquilino repositorioInquilino)        {            this.repositorio = repositorio;            this.repositorioInmueble = repositorioInmueble;            this.repositorioPropietario = repositorioPropietario;            this.repositorioInquilino = repositorioInquilino;                   }        // GET: ContratoController                public ActionResult Index()        {            var lista = repositorio.ObtenerTodos();            if (TempData.ContainsKey("Id"))                ViewBag.Id = TempData["Id"];            if (TempData.ContainsKey("Mensaje"))                ViewBag.Mensaje = TempData["Mensaje"];            return View(lista);        }        // GET: ContratoController/Details/5                public ActionResult Details(int id)        {            ViewData["Error"] = TempData["Error"];            var c = repositorio.ObtenerPorId(id);            return View(c);        }        // GET: ContratoController/Create                public ActionResult Create()        {            ViewBag.Inmueble = repositorioInmueble.ObtenerTodos();            ViewBag.Inquilino = repositorioInquilino.ObtenerTodos();            return View();        }        // POST: ContratoController/Create                [HttpPost]        [ValidateAntiForgeryToken]        public ActionResult Create(Contrato c)        {            try
            {
                if (ModelState.IsValid)

                {
                    repositorio.Alta(c);
                    TempData["Id"] = c.IdContrato;
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.Inquilino = repositorioInquilino.ObtenerTodos();
                    ViewBag.Inmueble = repositorioInmueble.ObtenerTodos();
                    return View(c);
                }
            }            catch (Exception ex)            {                ViewBag.Error = ex.Message;                ViewBag.StackTrate = ex.StackTrace;                return View(c);            }        }        // GET: ContratoController/Edit/5                public ActionResult Edit(int id)        {            var n = repositorio.ObtenerPorId(id);            ViewBag.Inquilino = repositorioInquilino.ObtenerTodos();            ViewBag.Inmueble = repositorioInmueble.ObtenerTodos();
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];            return View(n);        }        // POST: ContratoController/Edit/5                [HttpPost]        [ValidateAntiForgeryToken]        public ActionResult Edit(int id, Contrato c)        {            try            {                c.IdContrato = id;                repositorio.Modificacion(c);                TempData["Mensaje"] = "Se actualizo coreectamente";                return RedirectToAction(nameof(Index));            }            catch (Exception ex)            {                ViewBag.Inquilino = repositorioInquilino.ObtenerTodos();
                ViewBag.Inmueble = repositorioInmueble.ObtenerTodos();
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;                return View();            }        }        // GET: ContratoController/Delete/5           public ActionResult Delete(int id)        {
                var c = repositorio.ObtenerPorId(id);
                if (TempData.ContainsKey("Mensaje"))
                    ViewBag.Mensaje = TempData["Mensaje"];
                if (TempData.ContainsKey("Error"))
                    ViewBag.Error = TempData["Error"];
                return View(c);
                    }        // POST: ContratoController/Delete/5        [HttpPost]        [ValidateAntiForgeryToken]            public ActionResult Delete(int id, Contrato c)        {            try            {                repositorio.Baja(id);                return RedirectToAction(nameof(Index));            }            catch (Exception ex)            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;                return View();            }        }    }}