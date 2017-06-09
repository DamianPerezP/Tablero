﻿using System;
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
            Usuario MiUsser =  NUsuario;
            string mens = "";
            bool Existe = MiUsser.Login(ref mens);
            if (Existe == true)
            {
                ViewBag.Usuario = MiUsser;
                if (MiUsser.HayDB(ref mens))
                {
                    TempData["Mail"] = MiUsser.Mail;
                    return RedirectToAction("Registro","Inicio");
                }
                else
                {
                    TempData["Mail"] = MiUsser.Mail;
                    return RedirectToAction("Registro","SubirArchivo");
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
            string mensaje = "";
            Usuario NUsuario = new Usuario();
            NUsuario = NUsuario2;
            NUsuario.Registro(ref mensaje);
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
                    if (NUsuario2.Contraseña == NUsuario2.ReContraseña)
                    {
                        ViewBag.mensaje = "Ingrese dos contraseñas iguales";
                        return View();
                    }
                     else
                    {
                        
                       ViewBag.Usuario = NUsuario;
                       TempData["Mail"] = NUsuario.Mail;
                       return View("Registro","SubirArchivo");
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
            NUsuario.Mail = TempData["Mail"].ToString();
            NUsuario = NUsuario.TraerUsuario(ref mens);
            System.IO.File.Move(file.FileName,file.FileName + "-" + NUsuario.ID);
            string fileName = file.FileName;
            string FileExtension = fileName.Substring(fileName.LastIndexOf('.') + 1).ToLower();
                if (file.ContentLength > 0 && file != null && (FileExtension == "xlsx" || FileExtension == "xlsm" || FileExtension == "xltx" || FileExtension == "xltm" || FileExtension == "xlam"))
            {
                NUsuario.BaseDeDatos = file.FileName;
                var path = Server.MapPath("~/BD/") + file.FileName;
                file.SaveAs(path);
                NUsuario.CrearBaseDeDatos(ref mens);
                if (mens.Length == 0)
                {
                    return RedirectToAction("Registro","Inicio");
                }
                else
                {
                    ViewBag.mensaje = mens;
                    return RedirectToAction("Registro","SubirArchivo");                  
                }
            }
            else
            {
                ViewBag.mensaje = "\n" + "Ingresaste un Archivo invalido";
                return RedirectToAction("Registro", "SubirArchivo");
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