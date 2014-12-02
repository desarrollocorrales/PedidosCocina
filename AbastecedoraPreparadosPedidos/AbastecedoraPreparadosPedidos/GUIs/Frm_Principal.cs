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
        bool exito;
        System.IO.StreamReader fileToPrint;
        System.Drawing.Font printFont;

        public Frm_Principal()
        {
            InitializeComponent();
        }
        
        private void btnEnviar_Click(object sender, EventArgs e)
        {
            string sFullPath = CrearArchivoPedidos();
            
            if (exito == true)
                Imprimir(sFullPath);                            
        }
        private string CrearArchivoPedidos()
        {
            exito = false;
            string sFullPath = string.Empty;

            try
            {
                FirebirdDAL DAL = new FirebirdDAL();
                List<Venta> lstPedidos = DAL.getPedidos();

                MysqlDAL myDAL = new MysqlDAL();
                List<Venta> lstPedidosNuevos = myDAL.ValidarRegistros(lstPedidos);

                if (lstPedidosNuevos.Count != 0)
                {
                    string sFileName = "Pedido" + DateTime.Now.ToString("ddMMyyyy HHmmss") + ".txt";
                    sFullPath = Environment.CurrentDirectory + "\\Pedidos\\" + sFileName;

                    StreamWriter swFile = new StreamWriter(sFullPath, false);
                    swFile.WriteLine("       NUEVO PEDIDO        ");
                    swFile.WriteLine("TICKET    CANTIDAD ARTICULO");

                    foreach (Venta pedido in lstPedidos)
                    {
                        swFile.Write(pedido.FolioTicket.PadLeft(9));
                        swFile.Write(" ");
                        swFile.Write(pedido.Cantidad.ToString().PadLeft(8));
                        swFile.Write(" ");
                        swFile.WriteLine(pedido.Articulo);
                    }

                    swFile.Close();
                    exito = true;

                    MessageBox.Show("Enviado a cocina con exito..."); 
                }                
            } 
            catch (Exception ex)
            {                
                MessageBox.Show(ex.Message, ex.GetType().ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return sFullPath;
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
