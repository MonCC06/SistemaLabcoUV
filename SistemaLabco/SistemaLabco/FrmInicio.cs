﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ET;
using BL;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SistemaLabco
{
    public partial class FrmInicio : Form
    {
        public FrmInicio()
        {
            InitializeComponent();
        }

        int EstadoGuarda = 1;
        int IDCliente = 0;

        
        private void FormatoCL()
        {
            if (DgvCliente.Columns.Count >= 6) // Verifica si hay al menos 6 columnas
            {
                DgvCliente.Columns[0].Width = 100;
                DgvCliente.Columns[0].HeaderText = "ID_Cliente";
                DgvCliente.Columns[1].Width = 100;
                DgvCliente.Columns[1].HeaderText = "Nombre";
                DgvCliente.Columns[2].Width = 100;
                DgvCliente.Columns[2].HeaderText = "Cedula";
                DgvCliente.Columns[3].Width = 100;
                DgvCliente.Columns[3].HeaderText = "Telefono";
                DgvCliente.Columns[4].Width = 100;
                DgvCliente.Columns[4].HeaderText = "Correo";
                DgvCliente.Columns[5].Width = 100;
                DgvCliente.Columns[5].HeaderText = "Estado";
            }
            
        }

        private void ListadoCL(string tTexto)
        {
            try
            {
                DgvCliente.DataSource = BLCliente.ListadoCL(tTexto);
                this.FormatoCL();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
            //datasource nos dice de donde vamos a consumir los datos
        }

        private void BotonesCliente(bool LEstado)
        {
            this.btnGuardarNuevoCliente.Enabled = LEstado;
            this.btnBuscarCliente.Enabled = LEstado;
            this.btnModficarCliente.Enabled = LEstado;
            this.btnCancelarNuevoCliente.Enabled = LEstado;
            this.btnEliminarCliente.Enabled = LEstado;
        }

        private void SeleccionaCliente()
        {
            //convertir dato de string a un valor 
            if (string.IsNullOrEmpty(Convert.ToString(DgvCliente.CurrentRow.Cells["IDCliente"].Value)))
            {
                MessageBox.Show("No hay datos que mostar", "Aviso del Sistema", MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
            }
            else
            {
                //convertir el id del cliente de la celda seleccionada de string a entero
                this.IDCliente = Convert.ToInt32(DgvCliente.CurrentRow.Cells["IDCliente"].Value);
                txtNuevoNombreCliente.Text = Convert.ToString(DgvCliente.CurrentRow.Cells["Nombre"].Value);
                txtNuevoCedulaCliente.Text = Convert.ToString(DgvCliente.CurrentRow.Cells["Cedula"].Value);
                txtNuevoCorreoCliente.Text = Convert.ToString(DgvCliente.CurrentRow.Cells["Correo"].Value);
                txtNuevoTelefonoCliente.Text = Convert.ToString(DgvCliente.CurrentRow.Cells["Telefono"].Value);
            }
        }


        private void ActualizarCliente()
        {
            // Aquí puedes utilizar el método GuardarCliente o crear un método específico para actualización.
            // Asumiendo que `GuardarCliente` se usa para ambos casos:
            GuardarCliente();
            this.ListadoCL("%");
        }

        private void ModificarCliente()
        {
            EstadoGuarda = 2; // Indica que se trata de una actualización

            // Verificar si hay una fila seleccionada en el DataGridView
            if (DgvCliente.CurrentRow != null)
            {
                // Obtener el IDCliente de la fila seleccionada
                this.IDCliente = Convert.ToInt32(DgvCliente.CurrentRow.Cells["IDCliente"].Value);

                // Poblar los campos con los datos actuales del cliente
                txtNuevoNombreCliente.Text = Convert.ToString(DgvCliente.CurrentRow.Cells["Nombre"].Value);
                txtNuevoCedulaCliente.Text = Convert.ToString(DgvCliente.CurrentRow.Cells["Cedula"].Value);
                txtNuevoCorreoCliente.Text = Convert.ToString(DgvCliente.CurrentRow.Cells["Correo"].Value);
                txtNuevoTelefonoCliente.Text = Convert.ToString(DgvCliente.CurrentRow.Cells["Telefono"].Value);


                // Establecer el enfoque en el primer campo editable
                txtNuevoNombreCliente.Focus();
            }
            else
            {
                MessageBox.Show("Seleccione un cliente para modificar.", "Aviso del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void GuardarCliente()
        {
            if (txtNuevoNombreCliente.Text == String.Empty)
            {
                MessageBox.Show("Nombre del Cliente requerido(*)", "Aviso del Sistema", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

            }
            if (txtNuevoCedulaCliente.Text == String.Empty)
            {
                MessageBox.Show("Cedula del Cliente requerida(*)", "Aviso del Sistema", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

            }
            if (txtNuevoCorreoCliente.Text == String.Empty)
            {
                MessageBox.Show("Correo del Cliente requerido(*)", "Aviso del Sistema", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

            }
            if (txtNuevoTelefonoCliente.Text == String.Empty)
            {
                MessageBox.Show("Telefono del  requerido(*)", "Aviso del Sistema", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

            }
            else
            {
                ETCliente eTCliente = new ETCliente();
                //Respuesta de la bl y la que recibe de dal cuando se hace el insert (cuando guardamos)
                //para saber si el proceso fue exitoso o no
                String Rpta = "";
                eTCliente.IDCliente = this.IDCliente;
                //lo que el usuario digite en ese campo se va a capturar y se va a enviar a la bd como propiedad
                eTCliente.Nombre = txtNuevoNombreCliente.Text.Trim();
                eTCliente.Cedula = txtNuevoCedulaCliente.Text.Trim();
                eTCliente.Correo = txtNuevoCorreoCliente.Text.Trim();
                eTCliente.Telefono = txtNuevoTelefonoCliente.Text.Trim();
                //respuesta igual a los que nos retorne, enviar parametros para saber si es nuevo o no
                try
                {
                    Rpta = BLCliente.GuardarCL(EstadoGuarda, eTCliente);

                    if (Rpta == "OK")
                    {
                       
                        MessageBox.Show("Los datos se han registrado", "Aviso del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        EstadoGuarda = 0;
                        this.BotonesCliente(true);
                        txtNuevoNombreCliente.Text = "";
                        txtNuevoCedulaCliente.Text = "";
                        txtNuevoCorreoCliente.Text = "";
                        txtNuevoTelefonoCliente.Text = "";
                        this.IDCliente = 0;
                    }
                    else
                    {
                        MessageBox.Show("No se logró registrar el dato: " + Rpta, "Aviso del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Aviso del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

        }

        private void FrmInicio_Load(object sender, EventArgs e)
        {
            
            this.ListadoCL("%");

        }

        private void btnGuardarNuevoCliente_Click(object sender, EventArgs e)
        {
            
            if (EstadoGuarda == 2)
            {
                // Actualización del cliente
                ActualizarCliente();
            }
            else if (EstadoGuarda == 1)
            {
                // Inserción de nuevo cliente
                GuardarCliente();
            }
            else
            {
                MessageBox.Show("El estado de guardado no está definido.", "Aviso del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void btnEliminarCliente_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(Convert.ToString(DgvCliente.CurrentRow.Cells["IDCliente"].Value)))
            {
                MessageBox.Show("No hay datos que mostar", "Aviso del Sistema", MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
            }
            else
            {
                DialogResult opcion;
                //preguntar si se quiere realizar procedimiento y a opcion se le asignael valor de la respuesta
                opcion = MessageBox.Show("¿Está seguro de eliminar el registro seleccionado?", "Aviso del Sistema", MessageBoxButtons.YesNoCancel,
                   MessageBoxIcon.Question);
                if (opcion == DialogResult.Yes)
                {
                    String Rpta = "";
                    //convertir a int
                    this.IDCliente = Convert.ToInt32(DgvCliente.CurrentRow.Cells["IDCliente"].Value);
                    Rpta = BLCliente.EliminaCL(this.IDCliente);

                    if (Rpta.Equals("OK"))
                    {
                        this.IDCliente = 0;
                        MessageBox.Show("Registro Eliminado", "Aviso del Sistema", MessageBoxButtons.YesNoCancel,
                         MessageBoxIcon.Exclamation);

                    }
                }

            }
        }

        private void btnModficarCliente_Click(object sender, EventArgs e)
        {
            ModificarCliente();
        }

        private void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            this.ListadoCL(TxtBuscarCliente.Text.Trim());
        }

        private void btnCancelarNuevoCliente_Click(object sender, EventArgs e)
        {
            this.BotonesCliente(true);
            txtNuevoNombreCliente.Text = "";
            txtNuevoCedulaCliente.Text = "";
            txtNuevoCorreoCliente.Text = "";
            txtNuevoTelefonoCliente.Text = "";
        }

        

        private void ckbNombreCliente_CheckedChanged(object sender, EventArgs e)
        {
            this.ListadoCL(TxtBuscarCliente.Text.Trim());
        }

        private void ckbCedulaCliente_CheckedChanged(object sender, EventArgs e)
        {
            this.ListadoCL(TxtBuscarCliente.Text.Trim());
        }
    }
}
