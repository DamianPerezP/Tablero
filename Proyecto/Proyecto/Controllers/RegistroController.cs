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
            Usuario MiUsser = new Usuario();
            MiUsser = NUsuario;
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
                    Usuario NUser = new Usuario();
                    NUser = MiUsser.TraerUsuario(ref mens);
                    return RedirectToAction("Inicio", "Registro", new { mail = NUser.Mail, NPath = NUser.Path });
                }
                else
                {
                    //TempData["email"] = MiUsser.Mail.ToString();
                    //HttpCookie cookie = new HttpCookie(MiUsser.Mail, "email");
                    //Response.Cookies.Add(cookie);
                    return RedirectToAction("SubirArchivo", "Registro", new { mail = MiUsser.Mail });
                }
            }
            else
            {
                mens = mens + "\n Mail o Contraseña incorrecta, inténtelo nuevamente";
                ViewBag.mensaje = mens;
                return View();
            }
        }
<<<<<<< HEAD
        public ActionResult Inicio()
        {
            string mens = "";
        //    ViewBag.ElDataSet = TempData["nTabla"];
=======
        public ActionResult Inicio( DataSet ds)
        {
            string mens = "";
            ViewBag.ElDataSet = ds;
>>>>>>> bfaf50b6ce162429c7491b40038df1323e7b97d7
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
                            return RedirectToAction("SubirArchivo", "Registro", new { mail = NUsuario.Mail });
                        }
                        else
                        {
                            return View();
                        }
                    }
                }
            }

        }
        public ActionResult SubirArchivo(string mail)
        {
            TempData["Mail"] = mail;
            return View();
        }
        [HttpPost]
        public ActionResult SubirArchivo(HttpPostedFileBase file)
        {
            string mens = "";
            Usuario NUsuario2 = new Usuario();
            Usuario NUsuario = new Usuario();
            //NUsuario.Mail = TempData["email"].ToString();
            // string tempCookie = Request.Cookies["email"].Value;
            //NUsuario.Mail = tempCookie;
            //NUsuario.Mail = "sebilernerAgmail.com";
            NUsuario2.Mail = TempData["Mail"].ToString();
            NUsuario = NUsuario2.TraerUsuario(ref mens);
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
                    //NUsuario.Mail = "sebilernerAgmail.com";
                    NUsuario.BaseDeDatos = file.FileName;
                    NUsuario.Path = newpath;
                    NUsuario.CrearBaseDeDatos(ref mens);
                }
                catch(Exception exc)
                {
                    mens = mens + "\n" + exc.Message;
                }               
                if (mens.Length == 0)
                {
                    ViewBag.Usuario = NUsuario;
                    return RedirectToAction("ElegirExc", "Registro", new { mail = NUsuario.Mail, NPath = NUsuario.Path.ToString() });
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
        public ActionResult ElegirExc(string mail, string NPath)
        {
            string mens = "";
            Usuario usu = new Usuario();
            usu.Mail = mail;
            //usu.Mail = "sebilernerAgmail.com";
            usu = usu.TraerUsuario(ref mens);
            usu.Path = NPath;
            //string bdd = usu.BaseDeDatos;
            //string asd = @"C:\Tablero\Proyecto\Proyecto\BD\0-Librow.xls";
            //usu.BaseDeDatos = asd;  
            ViewBag.ElDataSet = usu.CargarExcelEnDataSet();
            TempData["dataset"] = ViewBag.ElDataSet;
            ViewBag.mensaje = mens;
            return View();

            //string mens = "";
            //Usuario usr = new Usuario();
            ////usr = ViewBag.Usuario;
            //usr.Mail = "sebilernerAgmail.com";
            //usr = usr.TraerUsuario(ref mens);
            //string bdd = usr.BaseDeDatos;
            //usr.BaseDeDatos = @"C:\Tablero\Proyecto\BaseDeDatos\" + bdd;
            //DataSet dsData = usr.CargarExcelEnDataSet();
            //ViewBag.ElDataSet = dsData;
            //return PartialView("_partialGrafico");
        }
        [HttpPost]
        public ActionResult ElegirExc(Usuario usu)
        {
            ViewBag.mensaje = "";
            if (usu.valor1 == null || usu.valor2 == null || usu.valor3 == null || usu.valor1 == usu.valor2 || usu.valor2 == usu.valor3 || usu.valor1 == usu.valor3)
            {
                ViewBag.mensaje = "Alguno de los contenidos de las columnas es incorrecto, vuelva a intentarlo";
                return View();
            }
            else
            {
                DataSet ds = new DataSet();
                ViewBag.DataSet = TempData["dataset"];
                DataSet dsViejo = ViewBag.DataSet;
<<<<<<< HEAD
                DataSet dsviejo2 = dsViejo;
                ds.Tables.Add(new DataTable());
                string titulo;
                try
                {
                    for (int i = 0; i < dsviejo2.Tables[0].Columns.Count; i++)
                    {
                        titulo = dsviejo2.Tables[0].Rows[0][i].ToString();
                        if (titulo != usu.valor1 && titulo != usu.valor2 && titulo != usu.valor3)
                        {
                            dsViejo.Tables[0].Columns.RemoveAt(i);
=======
                string titulo;
                try
                {
                    for (int i = 0; i < dsViejo.Tables[0].Columns.Count; i++)
                    {
                        titulo = dsViejo.Tables[0].Rows[0][i].ToString();
                        if (titulo == usu.valor1 || titulo == usu.valor2 || titulo == usu.valor3)
                        {
                            ds.Tables[0].Columns.Add(dsViejo.Tables[0].Columns[i]);
>>>>>>> bfaf50b6ce162429c7491b40038df1323e7b97d7
                        }
                    }
                }
                catch(Exception exc)
                {
                    ViewBag.mensaje = exc.Message;
                }
                if (ViewBag.mensaje == "")
                {
<<<<<<< HEAD
                    TempData["nTabla"] = dsViejo;
                    ViewBag.ElDataSet = dsViejo;
                    return View("Inicio");
=======
                    return RedirectToAction("Inicio", "Registro", new { nTabla = ds });
>>>>>>> bfaf50b6ce162429c7491b40038df1323e7b97d7
                }
                else
                {
                    return View();
                }
            }
        }
        public ActionResult Principal()
        {
            return View();
        }
    }
}