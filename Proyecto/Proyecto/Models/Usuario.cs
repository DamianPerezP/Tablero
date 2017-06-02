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
        public int ID { get; set; }
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
        [Required(ErrorMessage = "La confrimación de contraseña es incorrecta")]
        public string ReContraseña { get; set; }
        public HttpPostedFile BD { get; set; }
        public string BaseDeDatos { get; set; }
        private string NombreArchivo = "bdeasybusiness";
        private MySqlConnection nCon;
        private void Conectar()
        {
            //string proovedor = "Server=" + "localhost" + ";" + "Database=" + NombreArchivo + ";" + "Uid=" + "root" + ";" + "Pwd=" + "ROOT" + ";";
            string proovedor = String.Format ("Server={0};Database={1};Uid={2};Pwd={3};",
                                                "localhost",
                                                NombreArchivo, 
                                                "root", 
                                                "");
            nCon = new MySqlConnection();
        nCon.ConnectionString = proovedor;
            nCon.Open();
            
        }
        //Loguearse
        public bool Login()
        {
                bool existe = false;
                string strSQL = "";
                    Conectar();

                    MySqlCommand Consulta = nCon.CreateCommand();
                    Consulta.CommandType = CommandType.Text;
                    strSQL += "SELECT `Mail`";
                    strSQL += "FROM `USUARIOS`";
                    strSQL += " WHERE `Mail` = "+Mail+ "AND `Contraseña` = "+Contraseña+"";
                    Consulta.CommandText = strSQL;
                    MySqlDataReader drCon = Consulta.ExecuteReader();
                    if (drCon.HasRows == true)
                    {
                    existe = true;
                    }
                nCon.Close();
                return existe;
            }
           
        //Se Registra
        public string Registro()
        {
            int intRegsAffected = 0;
            string strSQL = "";
            string mensaje = "";
            try
            {
                Conectar();

                MySqlCommand Consulta = nCon.CreateCommand();
                Consulta.CommandType = CommandType.Text;
                strSQL += "INSERT INTO `bdeasybusiness`.`usuarios`";
                strSQL += " (`Mail`, `NombreUsuario`, `ApellidoUsuario`, `NombreEmpresa`, `Contraseña`, `BaseDeDatos`,  `FechaBD`)";
                strSQL += " VALUES";
                //strSQL += " ('"+ this.Mail + "', '"+ this.NombreUsuario + "', '"+ this.Apellido +"', '"+this.NombreEmpresa + "', '"+ this.Contraseña + "', 'aaa', '2012/05/02'); ";
                strSQL += String.Format( " ('{0}', '{1}', '{2}', '{3}', '{4}', '', ''); ",
                    this.Mail,
                    this.NombreUsuario,
                    this.Apellido,
                    this.NombreEmpresa,
                    this.Contraseña);

                Consulta.CommandText = strSQL;
                intRegsAffected = Consulta.ExecuteNonQuery();


                //MySqlCommand Consulta = nCon.CreateCommand();
                //Consulta.CommandType = CommandType.StoredProcedure;
                //Consulta.CommandText = "Registrar";
                //MySqlParameter ParNombreUsuario = new MySqlParameter("ParNombreUsuario", NombreUsuario);
                //Consulta.Parameters.Add(ParNombreUsuario);
                //MySqlParameter ParApellido = new MySqlParameter("ParApellido", Apellido);
                //Consulta.Parameters.Add(ParApellido);
                //MySqlParameter ParMail = new MySqlParameter("ParMail", Mail);
                //Consulta.Parameters.Add(ParMail);
                //MySqlParameter ParNombreEmpresa = new MySqlParameter("ParNombreEmpresa", NombreEmpresa);
                //Consulta.Parameters.Add(ParNombreEmpresa);
                //MySqlParameter ParContrasena = new MySqlParameter("ParContrasena", Contraseña);
                //Consulta.Parameters.Add(ParContrasena);
                //Consulta.ExecuteNonQuery();
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
            MySqlCommand Consulta = nCon.CreateCommand();
            Consulta.CommandType = CommandType.StoredProcedure;
            Consulta.CommandText = "TraerBaseDeDatos";
            MySqlParameter ParMail = new MySqlParameter("ParMail", Mail);
            Consulta.Parameters.Add(ParMail);
            MySqlDataReader drConsulta = Consulta.ExecuteReader();

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
                MySqlCommand Consulta = nCon.CreateCommand();
                Consulta.CommandType = CommandType.StoredProcedure;
                Consulta.CommandText = "CrearBD";
            MySqlParameter ParBaseDeDatos = new MySqlParameter("ParBaseDeDatos", BaseDeDatos);
                Consulta.Parameters.Add(ParBaseDeDatos);
            MySqlParameter ParMail = new MySqlParameter("ParMail", Mail);
                Consulta.Parameters.Add(ParMail);
                Consulta.ExecuteNonQuery();
                nCon.Close();
        }
        public Usuario TraerUsuario(string mail)
        {
            Conectar();
            Usuario NUsu = new Usuario();
            MySqlCommand consulta = nCon.CreateCommand();
            consulta.CommandType = CommandType.StoredProcedure;
            consulta.CommandText = "TraerUsuario";
            MySqlParameter ParMail = new MySqlParameter("ParMail", mail);
           consulta.Parameters.Add(ParMail);
            consulta.ExecuteNonQuery();
            MySqlDataReader DrConsulta;
            DrConsulta = consulta.ExecuteReader();
            while (DrConsulta.Read())
            {
                NUsu.ID = Convert.ToInt32(DrConsulta["IdUsuario"].ToString());
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