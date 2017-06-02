using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proyecto.Models;
using System.ComponentModel.DataAnnotations;

namespace Proyecto.Controllers
{
    public class RegistroController : Controller
    {
        // GET: Registro
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Usuario NUsuario)
        {
            Usuario MiUsser = new Usuario();
            MiUsser = NUsuario;
            string mens;
            bool Existe = MiUsser.Login(out mens);
            if (Existe == true)
            {
                ViewBag.Usuario = MiUsser;
                if (MiUsser.HayDB())
                {
                    TempData["Mail"] = MiUsser.Mail;
                    return View("Inicio");
                }
                else
                {
                    TempData["Mail"] = MiUsser.Mail;
                    return View("SubirArchivo");
                }
            }
            else
            {
                ViewBag.mensaje = mens;
                return View();
            }
        }
        public ActionResult Registrarse()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Registrarse(Usuario NUsuario2)
        {
            Usuario NUsuario = new Usuario();
            NUsuario = NUsuario2;
            string mensaje = NUsuario.Registro();
            if (mensaje != "")
            {
                ViewBag.mensaje = mensaje;
                return View();
            }
            else
            {
                if (NUsuario.Mail.Contains('@') == false && NUsuario.Mail.Contains('.') == false)
                {
                    ViewBag.mensaje = "Ingrese un Mail válido";
                    return View();
                }
                else
                {
                    if (NUsuario2.Contraseña == NUsuario2.ReContraseña)
                    {
                        ViewBag.mensaje = "Ingrese dos contraseñas iguales";
                        return View();
                    }
                     else
                    {
                       ViewBag.Usuario = NUsuario;
                       TempData["Mail"] = NUsuario.Mail;
                       return View("SubirArchivo");
            }
                }
            }
           
        }
        public ActionResult SubirArchivo()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SubirArchivo(HttpPostedFileBase file)
        {
            Usuario NUsuario = new Usuario();
            string mail = TempData["Mail"].ToString();
            NUsuario = NUsuario.TraerUsuario(mail);
            System.IO.File.Move(file.FileName,file.FileName + "-" + NUsuario.ID);
            string fileName = file.FileName;
            string FileExtension = fileName.Substring(fileName.LastIndexOf('.') + 1).ToLower();
                if (file.ContentLength > 0 && file != null && (FileExtension == "xlsx" || FileExtension == "xlsm" || FileExtension == "xltx" || FileExtension == "xltm" || FileExtension == "xlam"))
            {
                NUsuario.BaseDeDatos = file.FileName;
                var path = Server.MapPath("~/BD/") + file.FileName;
                file.SaveAs(path);
                NUsuario.CrearBaseDeDatos();
                return View("Inicio");
            }
            else
            {
                ViewBag.mensaje = "Ingresaste un Archivo invalido";
                return View();
            }
        }
        public ActionResult Inicio()
        {
            return View();
        }
        public ActionResult Principal()
        {
            return View();
        }
    }
}