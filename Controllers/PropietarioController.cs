﻿using InmobiliariaAlvarezM.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaAlvarezM.Controllers
{
    public class PropietarioController : Controller
    {
        private readonly RepositorioPropietario repositorio;
        private readonly IConfiguration configuration;

        public PropietarioController(IConfiguration configuration)
        {
            this.repositorio = new RepositorioPropietario(configuration);
            this.configuration = configuration;
        }
        // GET: PropietarioController
        public ActionResult Index()
        {
            IList<Propietario> lista = repositorio.ObtenerTodos();
            return View(lista);
        }

        // GET: PropietarioController/Details/5
        public ActionResult Details(int id)
        {
            var p = repositorio.ObtenerPorId(id);
            return View(p);
        }

        // GET: PropietarioController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PropietarioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Propietario p)
        {
            try
            {
                int res = repositorio.Alta(p);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PropietarioController/Edit/5
        public ActionResult Edit(int id)
        {
            var p = repositorio.ObtenerPorId(id);
            p.IdPropietario = id;
            return View(p);
        }

        // POST: PropietarioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            Propietario p = null;
            try
            {
                p = repositorio.ObtenerPorId(id);
                p.Nombre = collection["Nombre"];
                p.Apellido = collection["Apellido"];
                p.Dni = collection["Dni"];
                p.Email = collection["Email"];
                p.Telefono = collection["Telefono"];
             
                repositorio.Modificacion(p);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return View(p);
            }
        }

        // GET: PropietarioController/Delete/5
        public ActionResult Delete(int id)
        {
            var p = repositorio.ObtenerPorId(id);
            return View(p);
        }

        // POST: PropietarioController/Delete/5
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
