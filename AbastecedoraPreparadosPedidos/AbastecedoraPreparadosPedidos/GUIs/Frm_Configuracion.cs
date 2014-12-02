using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AbastecedoraPreparadosPedidos.GUIs
{
    public partial class Frm_Configuracion : Form
    {
        public Frm_Configuracion()
        {
            InitializeComponent();
        }
       
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            Guardar();
        }
        private void Guardar()
        {
            try
            {
                Properties.Settings.Default.Serie = txbSerie.Text;
                Properties.Settings.Default.Servidor = txbServidor.Text;
                Properties.Settings.Default.Path = txbPath.Text;
                Properties.Settings.Default.Usuario = txbUsuario.Text;
                Properties.Settings.Default.Password = txbPassword.Text;
                Properties.Settings.Default.Puerto = Convert.ToInt32(txbPuerto.Text);
                Properties.Settings.Default.Impresora = Convert.ToString(cmbImpresora.SelectedItem);

                Properties.Settings.Default.Save();

                MessageBox.Show("Los datos fueron guardados con exito!!!", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se guardaron los datos" + Environment.NewLine + ex.Message, 
                                 ex.GetType().ToString(), 
                                 MessageBoxButtons.OK, 
                                 MessageBoxIcon.Error);
            }
        }

        private void Frm_Configuracion_Load(object sender, EventArgs e)
        {
            CargarImpresoras();
            CargarDatos();
        }

        private void CargarDatos()
        {
            txbSerie.Text = Properties.Settings.Default.Serie;
            txbServidor.Text = Properties.Settings.Default.Servidor;
            txbPath.Text = Properties.Settings.Default.Path;
            txbUsuario.Text = Properties.Settings.Default.Usuario;
            txbPassword.Text = Properties.Settings.Default.Password;
            txbPuerto.Text = Properties.Settings.Default.Puerto.ToString();
        }

        private void CargarImpresoras()
        {
            try
            {
                foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                {
                    cmbImpresora.Items.Add(printer);
                }

                cmbImpresora.SelectedIndex = 0;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
