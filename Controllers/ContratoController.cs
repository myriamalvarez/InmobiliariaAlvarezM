﻿using InmobiliariaAlvarezM.Models;
using Microsoft.AspNetCore.Http;
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
            }
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
                ViewBag.Inmueble = repositorioInmueble.ObtenerTodos();
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                var c = repositorio.ObtenerPorId(id);
                if (TempData.ContainsKey("Mensaje"))
                    ViewBag.Mensaje = TempData["Mensaje"];
                if (TempData.ContainsKey("Error"))
                    ViewBag.Error = TempData["Error"];
                return View(c);
            
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;