using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OleDb;
using Proyecto.Models;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

using MySql.Data.MySqlClient;


namespace Proyecto.Models
{
    public class Usuario
    {
        [Required(ErrorMessage = "El Nombre de Usuario es incorrecto")]
        public string NombreUsuario { get; set; }
        [Required(ErrorMessage = "El Apellido es incorrecto")]
        public string Apellido { get; set; }
        [Required(ErrorMessage = "El Mail es incorrecto"), EmailAddress(ErrorMessage = "Ingrese una dirección de correo electrónico")]
        public string Mail { get; set; }
        [Required(ErrorMessage = "El Nombre de Empresa es incorrecto")]
        public string NombreEmpresa { get; set; }
        [Required(ErrorMessage = "La contraseña es incorrecta")]
        public string Contraseña { get; set; }
        public HttpPostedFile BD { get; set; }
        public string BaseDeDatos { get; set; }
        private string NombreArchivo = "UsuarioASPN.mdb ";
        private OleDbConnection nCon;
        private void Conectar()
        {
            string proovedor = @"Provider=Microsoft.Jet.OLEDB.4.0;  Data Source = |DataDirectory|" + NombreArchivo;
            nCon = new OleDbConnection();
        nCon.ConnectionString = proovedor;
            nCon.Open();
            
        }
        //Loguearse
        public bool Login(out string mensaje)
        {
            try
            {
                bool existe = false;
                Conectar();
                OleDbCommand Consulta = nCon.CreateCommand();
                Consulta.CommandType = CommandType.StoredProcedure;
                Consulta.CommandText = "Login";
                OleDbParameter ParMail = new OleDbParameter("ParMail", Mail);
                OleDbParameter ParContraseña = new OleDbParameter("ParContraseña", Contraseña);
                Consulta.Parameters.Add(ParMail);
                Consulta.Parameters.Add(ParContraseña);
                OleDbDataReader drConsulta = Consulta.ExecuteReader();

                if (drConsulta.HasRows == true)
                {
                    existe = true;
                }
                mensaje = "";
                nCon.Close();
                return existe;
            }
            catch (Exception exc)
            {
                mensaje = exc.Message;
                return false;
            }
            }
           
        //Se Registra
        public string Registro()
        {
            string mensaje = "";
            try
            {
                Conectar();
                OleDbCommand Consulta = nCon.CreateCommand();
                Consulta.CommandType = CommandType.StoredProcedure;
                Consulta.CommandText = "Registrar";
                OleDbParameter ParNombreUsuario = new OleDbParameter("ParNombreUsuario", NombreUsuario);
                Consulta.Parameters.Add(ParNombreUsuario);
                OleDbParameter ParApellido = new OleDbParameter("ParApellido", Apellido);
                Consulta.Parameters.Add(ParApellido);
                OleDbParameter ParMail = new OleDbParameter("ParMail", Mail);
                Consulta.Parameters.Add(ParMail);
                OleDbParameter ParNombreEmpresa = new OleDbParameter("ParNombreEmpresa", NombreEmpresa);
                Consulta.Parameters.Add(ParNombreEmpresa);
                OleDbParameter ParContraseña = new OleDbParameter("ParContraseña", Contraseña);
                Consulta.Parameters.Add(ParContraseña);
                Consulta.ExecuteNonQuery();
                nCon.Close();
            }
            catch (Exception Error)
            {
                mensaje = Error.Message.ToString();
            }
            nCon.Close();
            return mensaje;
        }
        public bool HayDB()
        {
            bool existe = false;
            Conectar();
            OleDbCommand Consulta = nCon.CreateCommand();
            Consulta.CommandType = CommandType.StoredProcedure;
            Consulta.CommandText = "TraerBaseDeDatos";
            OleDbParameter ParMail = new OleDbParameter("ParMail", Mail);
            Consulta.Parameters.Add(ParMail);
            OleDbDataReader drConsulta = Consulta.ExecuteReader();

            while (drConsulta.Read())
            {
                string Bd = drConsulta["BaseDeDatos"].ToString();
                if (Bd.Length > 1)
                {
                    existe = true;
                }
            }

            nCon.Close();
            return existe;
        }
        public void CrearBaseDeDatos()
        {
                Conectar();
                OleDbCommand Consulta = nCon.CreateCommand();
                Consulta.CommandType = CommandType.StoredProcedure;
                Consulta.CommandText = "CrearBD";
                OleDbParameter ParBaseDeDatos = new OleDbParameter("ParBaseDeDatos", BaseDeDatos);
                Consulta.Parameters.Add(ParBaseDeDatos);
                OleDbParameter ParMail = new OleDbParameter("ParMail", Mail);
                Consulta.Parameters.Add(ParMail);
                Consulta.ExecuteNonQuery();
                nCon.Close();
        }
        public Usuario TraerUsuario(string mail)
        {
            Conectar();
            Usuario NUsu = new Usuario();
            OleDbCommand consulta = nCon.CreateCommand();
            consulta.CommandType = CommandType.StoredProcedure;
            consulta.CommandText = "TraerUsuario";
            OleDbParameter ParMail = new OleDbParameter("ParMail", mail);
           consulta.Parameters.Add(ParMail);
            consulta.ExecuteNonQuery();
            OleDbDataReader DrConsulta;
            DrConsulta = consulta.ExecuteReader();
            while (DrConsulta.Read())
            {
                NUsu.NombreUsuario = DrConsulta["NombreUsuario"].ToString();
                NUsu.Apellido = DrConsulta["Apellido"].ToString();
                NUsu.Mail = DrConsulta["Mail"].ToString();
                NUsu.NombreEmpresa = DrConsulta["NombreEmpresa"].ToString();
                NUsu.Contraseña = DrConsulta["Contraseña"].ToString();
                NUsu.BaseDeDatos = DrConsulta["BaseDeDatos"].ToString();
            }
            nCon.Close();
            return NUsu;
        }

    }
}