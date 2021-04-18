using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using InmobiliariaAlvarezM.Models;

namespace InmobiliariaAlvarezM.Controllers

{
    [Authorize]
    public class UsuarioController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly RepositorioUsuario repositorioUsuario;
        public UsuarioController(IConfiguration configuration)
        {
            this.configuration = configuration;
            repositorioUsuario = new RepositorioUsuario(configuration);
        }
        // GET: Admin
        public ActionResult Index()
        {
            var lista = repositorioUsuario.ObtenerTodos();
            return View(lista);
        }

        // GET: Admin/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Admin/Create
        [Authorize(Policy = "Administrador")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Create(Usuario i)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Usuario us = repositorioUsuario.ObtenerPorEmail(i.Email);
                    if (us == null)
                    {

                        i.Clave = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                            password: i.Clave,
                            salt: System.Text.Encoding.ASCII.GetBytes("Salt"),
                            prf: KeyDerivationPrf.HMACSHA1,
                            iterationCount: 1000,
                            numBytesRequested: 256 / 8));
                        int res = repositorioUsuario.Alta(i);
                        TempData["Mensaje"] = "Usuario Creado Con Exito";

                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["Mensaje"] = "El correo ya se encuentra registrado";
                        return View();
                    }
                }
                else
                {
                    TempData["Mensaje"] = "Uno o mas datos son incorrectos";
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Edit/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Edit(int id)
        {
            var i = repositorioUsuario.ObtenerPorId(id);
            return View(i);
        }

        // POST: Admin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Edit(int id, Usuario i)
        {
            try
            {
                if (ModelState.IsValid)
                {


                    i.Clave = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: i.Clave,
                        salt: System.Text.Encoding.ASCII.GetBytes("Salt"),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));
                    int res = repositorioUsuario.Modificacion(i);
                    TempData["Mensaje"] = "Usuario Editado";
                    return RedirectToAction(nameof(Index));

                }
                else
                {
                    TempData["Mensaje"] = "Error al Editar, Verifique si los datos son correctos";
                    return View();
                }


            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            var i = repositorioUsuario.ObtenerPorId(id);
            return View(i);
        }

        // POST: Admin/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, Usuario i)
        {
            try
            {
                int res = repositorioUsuario.Baja(id);
                TempData["Mensaje"] = "Usuario Eliminado";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginView login)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                     password: login.Clave,
                     salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                     prf: KeyDerivationPrf.HMACSHA1,
                     iterationCount: 1000,
                     numBytesRequested: 256 / 8));

                    var e = repositorioUsuario.ObtenerPorEmail(login.Email);
                    if (e == null /*|| e.Clave != hashed*/)
                    {
                        ModelState.AddModelError("", "El email o la clave no son correctos");
                        return View();
                    }
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, e.Email),
                       // new Claim("FullName", e.Nombre + " " + e.Apellido),
                        new Claim(ClaimTypes.Role, e.RolNombre),
                    };
                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity));


                    return RedirectToAction(nameof(Index), "Home");
                }
                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }
        

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }


    }
}