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
using System.IO;
using System.Windows;

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
        public string Path { get; set; }
        public string FechaBD { get; set; }
        public string NombreDeHoja { get; set; }
        private string NombreArchivo = "bdeasybusiness";
        public static string email;
        public string eMail;
        public string valor1 { get; set; }
        public string valor2 { get; set; }
        public string valor3 { get; set; }
        public string grafico { get; set; }
        private MySqlConnection nCon;
        private void Conectar()
        {
            //string proovedor = "Server=" + "localhost" + ";" + "Database=" + NombreArchivo + ";" + "Uid=" + "root" + ";" + "Pwd=" + "ROOT" + ";";
            string proovedor = String.Format("Server={0};Database={1};Uid={2};Pwd={3};",
                                                "localhost",
                                                NombreArchivo,
                                                "root",
                                                "");
            nCon = new MySqlConnection();
            nCon.ConnectionString = proovedor;
            nCon.Open();

        }
        //Loguearse
        public void GuardarEmail()
        {
            eMail = Mail;
        }
        public String DevolverEmail()
        {
            return eMail;
        }

        public bool Login(ref string mens)
        {
            this.Mail = this.Mail.Replace('@', 'A');
            bool existe = false;
            string strSQL = "";
            try
            {
                Conectar();
                MySqlCommand Consulta = nCon.CreateCommand();
                Consulta.CommandType = CommandType.Text;
                strSQL += "SELECT Mail ";
                strSQL += "FROM usuarios";
                strSQL += " WHERE Mail = '" + this.Mail + "' AND Password = '" + Contraseña + "'";
                Consulta.CommandText = strSQL;
                MySqlDataReader drCon = Consulta.ExecuteReader();
                if (drCon.HasRows == true)
                {
                    existe = true;
                }

                nCon.Close();

            }
            catch (Exception exc)
            {
                mens = mens + "\n" + exc.Message;
            }
            return existe;
        }

        //Se Registra
        public string Registro(ref string mensaje)
        {
            this.Mail = this.Mail.Replace('@', 'A');
            int intRegsAffected = 0;
            string strSQL = "";
            try
            {
                Conectar();

                MySqlCommand Consulta = nCon.CreateCommand();
                Consulta.CommandType = CommandType.Text;
                //  VALUES ('pepe@pepe.comar', 'Nombre', 'Apellido', 'nombreempo', 'passwd', 'sss', '2', '2015-05-05');

                strSQL += "INSERT INTO usuarios";
                strSQL += " (Mail, NombreUsuario, ApellidoUsuario, NombreEmpresa, Password, BaseDeDatos, FechaBD, Path)";
                strSQL += " VALUES ";
                //strSQL += " ('"+ this.Mail + "', '"+ this.NombreUsuario + "', '"+ this.Apellido +"', '"+this.NombreEmpresa + "', '"+ this.Contraseña + "', '', ''); ";
                strSQL += String.Format(" ('{0}', '{1}', '{2}', '{3}', '{4}', '2' ,'2015-06-04', 'no hay'); ",
                   //                  mailx,
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
                mensaje = "\n" + Error.Message.ToString();
            }
            nCon.Close();
            return mensaje;
        }

        public bool HayDB(ref string mens)
        {
            bool existe = false;
            string strSQL = "";
            try
            {
                Conectar();

                MySqlCommand Consulta = nCon.CreateCommand();
                Consulta.CommandType = CommandType.Text;
                strSQL += "SELECT BaseDeDatos ";
                strSQL += "FROM usuarios";
                strSQL += " WHERE Mail = '" + Mail + "'";
                Consulta.CommandText = strSQL;
                MySqlDataReader drCon = Consulta.ExecuteReader();
                if (drCon.HasRows == true)
                {
                    while (drCon.Read())
                    {
                        if (drCon["BaseDeDatos"].ToString() != "2")
                        {
                            existe = true;
                        }
                    }
                }
                nCon.Close();

            }
            catch (Exception exc)
            {
                mens = "\n" + exc.Message;
            }
            return existe;
        }
        public void CrearBaseDeDatos(ref string mens)
        {
            String pa = this.Path;
            pa = pa.Replace("\\", ".Q.");
            int intRegsAffected = 0;
            string strSQL = "";
            try
            {
                Conectar();
                
                MySqlCommand Consulta = nCon.CreateCommand();
                Consulta.CommandType = CommandType.Text;
                strSQL += "UPDATE usuarios ";
                strSQL += "SET BaseDeDatos = '" + this.BaseDeDatos + "' , Path = '" + pa+"' ";
                strSQL += "WHERE Mail = '" + Mail + "';";
                Consulta.CommandText = strSQL;
                intRegsAffected = Consulta.ExecuteNonQuery();
            }
            catch (Exception exc)
            {
                mens = "\n" + exc.Message;
            }
        }
        public Usuario TraerUsuario(ref string mens)
        {
            this.Mail = Mail.Replace('@', 'A');
            Usuario NUsu = new Usuario();
            string strSQL = "";
            try
            {
                Conectar();
                MySqlCommand Consulta = nCon.CreateCommand();
                Consulta.CommandType = CommandType.Text;
                strSQL += "SELECT * ";
                strSQL += "FROM usuarios ";
                strSQL += " WHERE Mail = '" + this.Mail + "';";
                Consulta.CommandText = strSQL;
                MySqlDataReader DrConsulta = Consulta.ExecuteReader();
                while (DrConsulta.Read())
                {
                    NUsu.ID = Convert.ToInt32(DrConsulta["IdUsuario"].ToString());
                    NUsu.NombreUsuario = DrConsulta["NombreUsuario"].ToString();
                    NUsu.Apellido = DrConsulta["ApellidoUsuario"].ToString();
                    NUsu.Mail = DrConsulta["Mail"].ToString();
                    NUsu.NombreEmpresa = DrConsulta["NombreEmpresa"].ToString();
                    NUsu.Contraseña = DrConsulta["Password"].ToString();
                    NUsu.BaseDeDatos = DrConsulta["BaseDeDatos"].ToString();
                    NUsu.ID = Convert.ToInt32(DrConsulta["IdUsuario"].ToString());
                    NUsu.Path = DrConsulta["Path"].ToString();
                    
                }
                NUsu.Path = NUsu.Path.Replace(".Q.", "\\");
                nCon.Close();
            }
            catch (Exception exc)
            {
                mens = "\n" + exc.Message;
            }
            return NUsu;
        }
        public DataSet CargarExcelEnDataSet()
        {
            string connectionString = string.Format("provider=Microsoft.Jet.OLEDB.4.0; data source={0};Extended Properties=Excel 8.0;", this.Path);


            DataSet data = new DataSet();
                foreach (var sheetName in GetExcelSheetNames(connectionString))
                {
                    using (OleDbConnection con = new OleDbConnection(connectionString))
                    {
                        var dataTable = new DataTable();
                        string query = string.Format("SELECT * FROM [{0}]", sheetName);
                        con.Open();
                        OleDbDataAdapter adapter = new OleDbDataAdapter(query, con);
                        adapter.Fill(dataTable);
                        data.Tables.Add(dataTable);
                    }
                }
            return data;
        }

       
           private string[] GetExcelSheetNames(string connectionString)
        {
            OleDbConnection con = null;
            DataTable dt = null;
            con = new OleDbConnection(connectionString);
            con.Open();
            dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

            if (dt == null)
            {
                return null;
            }

            string[] excelSheetNames = new string[dt.Rows.Count];
            int i = 0;
            foreach (DataRow row in dt.Rows)
            {
                excelSheetNames[i] = row["TABLE_NAME"].ToString();
                i++;
            }

            return excelSheetNames;
        }
    }
}