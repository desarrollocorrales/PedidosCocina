using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FirebirdSql.Data.FirebirdClient;
using System.Configuration;
using AbastecedoraPreparadosPedidos.Modelos;

namespace AbastecedoraPreparadosPedidos.DAL
{
    public class FirebirdDAL
    {
        FbConnection Conexion;
        FbCommand Comando;
        FbDataAdapter Adapter;

        public FirebirdDAL()
        {
            Conexion = new FbConnection();
            Comando = new FbCommand();
            Adapter = new FbDataAdapter();

            Conexion.ConnectionString = getConnectionString();
        }

        private string getConnectionString()
        {
            FbConnectionStringBuilder fbcsb = new FbConnectionStringBuilder();
            fbcsb.DataSource = Properties.Settings.Default.Servidor;
            fbcsb.Database = Properties.Settings.Default.Path;
            fbcsb.UserID = Properties.Settings.Default.Usuario;
            fbcsb.Password = Properties.Settings.Default.Password;
            fbcsb.Port = Properties.Settings.Default.Puerto;

            return fbcsb.ConnectionString;
        }

        public List<string> getUltimosFolios(string sFecha)
        {
            string sSerie = Properties.Settings.Default.Serie;
            string Consulta =
                string.Format(@"  SELECT FIRST 200 FOLIO
                                    FROM DOCTOS_PV 
                                   WHERE FECHA >= '{0}' AND DOCTOS_PV.TIPO_DOCTO = 'V' AND
                                         FOLIO LIKE '{1}%' 
                                ORDER BY FOLIO", 
                                sFecha, sSerie.ToUpper());

            /* Acceso a base de datos */
            Conexion.Open();
            Comando.Connection = Conexion;
            Comando.CommandText = Consulta;
            Adapter.SelectCommand = Comando;

            DataTable tablaResultado = new DataTable();
            Adapter.Fill(tablaResultado);

            List<string> lstFolios = new List<string>();
            foreach (DataRow row in tablaResultado.Rows)
            {
                lstFolios.Add(row["FOLIO"].ToString());               
            }

            Conexion.Close();

            return lstFolios;
        }
        private string getClaves()
        {
            string sClaves = ConfigurationManager.AppSettings["Claves"];

            return sClaves;
        }

        public List<Venta> getPedidos(string sFecha, string sFolio)
        {            
            string clavesArticulos = getClaves();

            string Consulta =
                string.Format(@"SELECT 
                                      DOCTOS_PV.FOLIO,
                                      DOCTOS_PV.FECHA,
                                      DOCTOS_PV.HORA,
                                      DOCTOS_PV_DET.CLAVE_ARTICULO,
                                      ARTICULOS.NOMBRE,
                                      DOCTOS_PV_DET.UNIDADES
                                FROM
                                      DOCTOS_PV_DET
                                      INNER JOIN DOCTOS_PV ON (DOCTOS_PV_DET.DOCTO_PV_ID = DOCTOS_PV.DOCTO_PV_ID)
                                      INNER JOIN ARTICULOS ON (DOCTOS_PV_DET.ARTICULO_ID = ARTICULOS.ARTICULO_ID)
                                WHERE
                                      DOCTOS_PV.FECHA >= '{0}' AND 
                                      FOLIO = '{1}' AND
                                      DOCTOS_PV_DET.CLAVE_ARTICULO IN ({2})
                                ORDER BY
                                      FOLIO ASC",
                                      sFecha, 
                                      sFolio, 
                                      clavesArticulos);

            /* Acceso a base de datos */
            Conexion.Open();
            Comando.Connection = Conexion;
            Comando.CommandText = Consulta;
            Adapter.SelectCommand = Comando;

            DataTable tablaResultado = new DataTable();
            Adapter.Fill(tablaResultado);

            Venta venta;
            List<Venta> lstVentas = new List<Venta>();
            foreach (DataRow row in tablaResultado.Rows)
            {
                venta = new Venta();
                venta.FolioTicket = Convert.ToString(row["FOLIO"]);
                venta.Fecha = (Convert.ToDateTime(row["FECHA"])).ToString("yyyy-MM-dd");
                venta.Hora = Convert.ToString(row["HORA"]);
                venta.Clave = Convert.ToString(row["CLAVE_ARTICULO"]);
                venta.Articulo = Convert.ToString(row["NOMBRE"]);
                venta.Cantidad = Convert.ToUInt16(row["UNIDADES"]);
                lstVentas.Add(venta);
            }

            Conexion.Close();
            /**************************/

            return lstVentas;
        }
    }
}
