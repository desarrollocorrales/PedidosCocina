using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using AbastecedoraPreparadosPedidos.DAL;
using AbastecedoraPreparadosPedidos.Modelos;

namespace AbastecedoraPreparadosPedidos.GUIs
{
    public partial class Frm_Principal : Form
    {
        string sFullPath;
        System.IO.StreamReader fileToPrint;
        System.Drawing.Font printFont;

        public Frm_Principal()
        {
            InitializeComponent();
        }
        
        private void btnEnviar_Click(object sender, EventArgs e)
        {
            CrearArchivoPedidos();                                      
        }
        private void CrearArchivoPedidos()
        {
            try
            {
                string sFecha = DateTime.Today.ToString("yyyy-MM-dd");
                //string sFecha = "2014-11-28";
                FirebirdDAL FirebirdDAL = new FirebirdDAL();
                MysqlDAL MySqlDAL = new MysqlDAL();

                List<string> lstFolios = FirebirdDAL.getUltimosFolios(sFecha);                

                foreach (string folio in lstFolios)
                {                    
                    List<Venta> lstVentas = FirebirdDAL.getPedidos(sFecha,folio);
                    if (lstVentas.Count != 0)
                    {
                        string sFileName = "Pedido.txt";
                        sFullPath = Environment.CurrentDirectory + "\\Pedidos\\" + sFileName;

                        StreamWriter swFile = new StreamWriter(sFullPath, false);
                        StringBuilder sb = new StringBuilder();
                        //            "1234567890123456789012345678901234567890"
                        sb.AppendLine("  Abastecedora de Carnes Los Corrales   ");
                        sb.AppendLine();
                        sb.AppendLine("              Nuevo Pedido              ");
                        sb.AppendLine();
                        sb.AppendLine("  TICKET: " + lstVentas[0].FolioTicket);
                        sb.AppendLine("   FECHA: " + lstVentas[0].Fecha);
                        sb.AppendLine("    HORA: " + lstVentas[0].Hora.Substring(0, 8));
                        sb.AppendLine();
                        sb.AppendLine("CANT  ARTICULO");

                        ushort contador = 0;
                        foreach (Venta pedido in lstVentas)
                        {
                            if (MySqlDAL.ValidarRegistros(pedido) == true)
                            {
                                sb.Append(pedido.Cantidad.ToString().PadLeft(4));
                                sb.Append("  ");
                                sb.AppendLine(pedido.Articulo);
                                contador++;
                            }
                        }
                        sb.AppendLine();
                        //            "1234567890123456789012345678901234567890"
                        sb.AppendLine("       Gracias por su preferencia       ");

                        swFile.Write(sb.ToString());
                        swFile.Close();

                        if (contador != 0)
                        {
                            string sFileSavePath = 
                                string.Format(@"{0}\Pedidos\{1}.txt",
                                                Environment.CurrentDirectory, DateTime.Now.ToString("ddMMyyyy"));
                            
                            using (StreamWriter swFileSave = new StreamWriter(sFileSavePath, true))
                            {
                                swFileSave.WriteLine("****************************************");
                                swFileSave.WriteLine(sb.ToString());
                                swFileSave.WriteLine("****************************************");
                                swFileSave.WriteLine();
                                swFileSave.Close();
                            }

                            Imprimir(sFullPath);
                        }
                    }                                                                                                                          
                }                
            } 
            catch (Exception ex)
            {                
                MessageBox.Show(ex.Message, ex.GetType().ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void Imprimir(string sFullPath)
        {
            try
            {
                fileToPrint = new StreamReader(sFullPath);
                printFont = new System.Drawing.Font("Lucida Console", 8);
                DocumentoPedido.PrinterSettings.PrinterName = Properties.Settings.Default.Impresora;                
                DocumentoPedido.Print();
                fileToPrint.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void DocumentoPedido_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            int count = 0;
            float yPos = 0f;
            float topMargin = 0f;
            float leftMargin = 0f;
            string line = null;
            float linesPerPage = e.MarginBounds.Height / printFont.GetHeight(e.Graphics);

            while (count < linesPerPage)
            {
                line = fileToPrint.ReadLine();
                if (line == null)
                {
                    break;
                }

                yPos = topMargin + count * printFont.GetHeight(e.Graphics);
                e.Graphics.DrawString(line, printFont, Brushes.Black, leftMargin, yPos, new StringFormat());
                count++;
            }
            if (line != null)
            {
                e.HasMorePages = true;
            }
        }

        private void label1_DoubleClick(object sender, EventArgs e)
        {
            new Frm_Configuracion().ShowDialog();
        }

    }
}
