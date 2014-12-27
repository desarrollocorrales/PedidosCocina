using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MySql.Data.MySqlClient;
using AbastecedoraPreparadosPedidos.Modelos;

namespace AbastecedoraPreparadosPedidos.DAL
{
    public class MysqlDAL
    {
        private MySqlConnection Conexion;
        private MySqlCommand Comando;

        public MysqlDAL()
        {
            Conexion = new MySqlConnection();
            Comando = new MySqlCommand();
        }

        private string getStringConnection()
        {
            return ConfigurationManager.AppSettings["mysql"];
        }

        public bool ValidarRegistros(Venta pedido)
        {
            bool exito = false;
            Conexion.ConnectionString = getStringConnection();
            Conexion.Open();
            MySqlTransaction Transaccion = Conexion.BeginTransaction();

            try
            {
                Comando.Connection = Conexion;

                Comando.CommandText =
                    string.Format(@"INSERT INTO ventas 
                                             (folio_ticket, fecha_ticket, hora_ticket, 
                                              cantidad_ticket, clave_articulo) 
                                         VALUES ('{0}', '{1}', '{2}', 
                                                  {3}, '{4}')",
                                         pedido.FolioTicket, pedido.Fecha,
                                         pedido.Hora, pedido.Cantidad,
                                         pedido.Clave);
                try
                {
                    Comando.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                Transaccion.Commit();

                exito = true;
            }
            catch
            {
                Transaccion.Rollback();
                
                exito = false;
            }
            finally
            {
                if (Conexion.State != System.Data.ConnectionState.Closed)
                    Conexion.Close();
            }

            return exito;
        }

        public void InstalarBaseDeDatos()
        {
            string sCommandText =
                File.ReadAllText(string.Format(@"{0}\Data\ControlTortas.sql", Environment.CurrentDirectory));

            Conexion.ConnectionString = getStringConnection();
            Conexion.ConnectionString = Conexion.ConnectionString.Replace("database=control_tortas;", string.Empty);

            Conexion.Open();

            Comando.Connection = Conexion;
            Comando.CommandText = sCommandText;

            Comando.ExecuteNonQuery();

            if (Conexion.State != System.Data.ConnectionState.Closed)
            {
                Conexion.Close();
            }
        }

        public bool ExisteBaseDeDatos()
        {
            try
            {
                Conexion.ConnectionString = getStringConnection();

                Conexion.Open();
                Conexion.Close();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
