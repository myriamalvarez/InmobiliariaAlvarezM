﻿using InmobiliariaAlvarezM.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace InmobiliariaAlvarezM.Controllers
{
    [Authorize(Policy = "Administrador")]
    public class UsuarioController : Controller
    {
        private readonly IConfiguration configuration;
        private RepositorioUsuario repositorio;
        public UsuarioController(IConfiguration configuration)
        {
            this.configuration = configuration;
            repositorio = new RepositorioUsuario(configuration);
        }
        // GET: UsuarioController
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(IFormCollection collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: collection["Clave"],
                        salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 400,
                        numBytesRequested: 256 / 8));

                    Usuario e = repositorio.ObtenerPorEmail(collection["Email"].ToString());
                    if (e == null || e.Clave != hashed)
                    {
                        ViewData["Error"] = "El email o la clave no son correctos";
                        return View();
                    }

                    List<Claim> claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, e.Email),
                        new Claim(ClaimTypes.Role, e.RolNombre),
                    };

                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    return RedirectToAction("Index", "Home");
                }
                throw new Exception("Error de validación");
            }
            catch (Exception ex)
            {
                ViewData["Error"] = ex.Message;
                return View();
            }
        }

        //[Route("salir", Name = "logout")] que es esto??
        [AllowAnonymous]
        [Authorize(Policy = "Empleado")]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        // GET: UsuarioController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UsuarioController/Create
        [AllowAnonymous]
        public ActionResult Create()
        {
            return View();
        }

        // POST: UsuarioController/Create
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Usuario e)
        {
            try
            {
                e.Clave = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: e.Clave,
                        salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 400,
                        numBytesRequested: 256 / 8));
                e.Rol = 3;
                e.Avatar = "";
                repositorio.Alta(e);
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                if (ex is SqlException && (ex as SqlException).Number == 2627)
                    msg = "Ya existe una cuenta asociada a ese Email.";
                ViewData["Error"] = msg;
                return View();
            }
        }

        // GET: UsuarioController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UsuarioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UsuarioController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UsuarioController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}