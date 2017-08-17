using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proyecto.Models;
using System.ComponentModel.DataAnnotations;
using System.Data;

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
            Usuario MiUsser = NUsuario;
            string mens = "";
            bool Existe = MiUsser.Login(ref mens);
            if (Existe == true)
            {
                ViewBag.Usuario = MiUsser;
                if (MiUsser.HayDB(ref mens))
                {
                    //TempData["email"] = MiUsser.Mail.ToString();
                    //HttpCookie cookie = new HttpCookie(MiUsser.Mail, "email");
                    //Response.Cookies.Add(cookie);
                    return RedirectToAction("Inicio", "Registro");
                }
                else
                {
                    //TempData["email"] = MiUsser.Mail.ToString();
                    //HttpCookie cookie = new HttpCookie(MiUsser.Mail, "email");
                    //Response.Cookies.Add(cookie);
                    return RedirectToAction("SubirArchivo", "Registro");
                }
            }
            else
            {
                mens = mens + "\n Mail o Contraseña incorrecta, inténtelo nuevamente";
                ViewBag.mensaje = mens;
                return View();
            }
        }
        public ActionResult Inicio()
        {
            string mens = "";
            Usuario usu = new Usuario();
            usu.Mail = "sebilernerAgmail.com";
            usu= usu.TraerUsuario(ref mens);
            string bdd = usu.BaseDeDatos;
            string asd = @"C:\Tablero\Proyecto\Proyecto\BD\Librow.xls";
            usu.BaseDeDatos = asd;  
            ViewBag.Nombres = usu.CargarExcelEnDataSet();
            ViewBag.mensaje = mens;         
            return View();
        }
        public ActionResult Registrarse()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Registrarse(Usuario NUsuario2)
        {
            string mensaje = "";
            Usuario NUsuario = new Usuario();
            NUsuario = NUsuario2;
            bool existe = NUsuario.Login(ref mensaje);
            if (mensaje != "" || existe)
            {
                if (existe)
                {
                    mensaje = mensaje + "\n" + "Ya existe un Usuario con ese Mail";
                }
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
                    if (NUsuario2.Contraseña != NUsuario2.ReContraseña)
                    {
                        ViewBag.mensaje = "Ingrese dos contraseñas iguales";
                        return View();
                    }
                    else
                    {
                        NUsuario.Registro(ref mensaje);
                        if (mensaje == "")
                        {
                            ViewBag.Usuario = NUsuario;
                            //TempData["email"] = NUsuario.Mail.ToString();
                            //HttpCookie cookie = new HttpCookie(NUsuario.Mail, "email");
                            //Response.Cookies.Add(cookie);
                            return RedirectToAction("SubirArchivo", "Registro");
                        }
                        else
                        {
                            return View();
                        }
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
            string mens = "";
            Usuario NUsuario = new Usuario();
            //NUsuario.Mail = TempData["email"].ToString();
            // string tempCookie = Request.Cookies["email"].Value;
            //NUsuario.Mail = tempCookie;
            NUsuario.Mail = "sebilernerAgmail.com";
            string fileName = file.FileName;
            string FileExtension = fileName.Substring(fileName.LastIndexOf('.') + 1).ToLower();
            if (file.ContentLength > 0 && file != null && (FileExtension == "xlsx" || FileExtension == "xlsm" || FileExtension == "xltx" || FileExtension == "xltm" || FileExtension == "xlam") || FileExtension == "xls")
            {
                try
                {
                    var path = Server.MapPath("~/BD/") + file.FileName;
                    var newpath = Server.MapPath("~/BD/") + NUsuario.ID + "-" + file.FileName;
                    file.SaveAs(path);
                    System.IO.File.Move(path, newpath);
                    NUsuario.Mail = "sebilernerAgmail.com";
                    NUsuario.BaseDeDatos = file.FileName;
                    NUsuario.CrearBaseDeDatos(ref mens);
                }
                catch(Exception exc)
                {
                    mens = mens + "\n" + exc.Message;
                }               
                if (mens.Length == 0)
                {
                    ViewBag.Usuario = NUsuario;
                    return RedirectToAction("Inicio", "Registro");
                }
                else
                {
                    ViewBag.mensaje = mens;
                    return View();
                }
            }
            else
            {
                ViewBag.mensaje = "\n" + "Ingresaste un Archivo invalido";
                return View();
            }
        }
        public ActionResult _TablaPartial()
        {
            String mens = "";
            Usuario usr = new Usuario();
            //usr = ViewBag.Usuario;
            usr.Mail = "sebilernerAgmail.com";
            usr = usr.TraerUsuario(ref mens);
            String bdd = usr.BaseDeDatos;
            usr.BaseDeDatos = @"C:\Tablero\Proyecto\BaseDeDatos\" + bdd;
            DataSet dsData = usr.CargarExcelEnDataSet();
            ViewBag.ElDataSet = dsData;
            return PartialView("_partialGrafico");
        }
        public ActionResult Principal()
        {
            return View();
        }
    }
}