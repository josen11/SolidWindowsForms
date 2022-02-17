using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolidWindowsForms
{
    public partial class PropiedadWindow : Form
    {
        //Use Name of Entinty Container of our EF Data Model
        private readonly DBEdificioEntities Entities;

        public PropiedadWindow()
        {
            InitializeComponent();
            Entities = new DBEdificioEntities();
        }
        private void retrieveOwners()
        {
            // Select * from Propietario con LINQ aprovechando el tema de concatear el nombres y apellidos desde la consulta
            var result = (from a in Entities.Propietarios.ToList()
                          select new { Id = a.Id, Name = a.Nombres + " " + a.Apellidos }).ToList();

            // Genial una manera de asociar el index y el valor mostrado en el Combobox
            cbPropietario.DisplayMember = "Name";
            cbPropietario.ValueMember = "Id";
            cbPropietario.DataSource = result;

        }
        private void retrieveGoods()
        {
            // Podemos uar Lambda expression tambien
            var result = Entities.Propiedads.Select(a => new
            {
                Id = a.Id,
                Propietario = a.Propietario.Nombres + " " + a.Propietario.Apellidos,
                Tipo = a.Tipo,
                Nro = a.Nro,
                Piso = a.Piso,
                Area = a.Area,
                Porcentaje = a.Porcentaje,
                Habitado = a.Habitado,
                Descuento = a.Descuento,
                MontoDescuento = a.MontoDescuento,
                IdPropietario = a.IdPropietario,
            }).ToList();

            gvPropiedades.AutoGenerateColumns = false;
            DataGridViewTextBoxColumn c1 = new DataGridViewTextBoxColumn();
            c1.DataPropertyName = "Id";
            c1.HeaderText = "Id";
            c1.Name = "Id";
            DataGridViewTextBoxColumn c2 = new DataGridViewTextBoxColumn();
            c2.DataPropertyName = "Propietario";
            c2.HeaderText = "Propietario";
            c2.Name = "Propietario";
            DataGridViewTextBoxColumn c3 = new DataGridViewTextBoxColumn();
            c3.DataPropertyName = "Tipo";
            c3.HeaderText = "Tipo";
            c3.Name = "Tipo";
            DataGridViewTextBoxColumn c4 = new DataGridViewTextBoxColumn();
            c4.DataPropertyName = "Nro";
            c4.HeaderText = "Nro";
            c4.Name = "Nro";
            DataGridViewTextBoxColumn c5 = new DataGridViewTextBoxColumn();
            c5.DataPropertyName = "Piso";
            c5.HeaderText = "Piso";
            c5.Name = "Piso";
            DataGridViewTextBoxColumn c6 = new DataGridViewTextBoxColumn();
            c6.DataPropertyName = "Area";
            c6.HeaderText = "Area";
            c6.Name = "Area";
            DataGridViewTextBoxColumn c7 = new DataGridViewTextBoxColumn();
            c7.DataPropertyName = "Porcentaje";
            c7.HeaderText = "Porcentaje";
            c7.Name = "Porcentaje";
            DataGridViewCheckBoxColumn c8 = new DataGridViewCheckBoxColumn();
            c8.DataPropertyName = "Habitado";
            c8.HeaderText = "Habitado";
            c8.Name = "Habitado";
            DataGridViewCheckBoxColumn c9 = new DataGridViewCheckBoxColumn();
            c9.DataPropertyName = "Descuento";
            c9.HeaderText = "Descuento";
            c9.Name = "Descuento";
            DataGridViewTextBoxColumn c10 = new DataGridViewTextBoxColumn();
            c10.DataPropertyName = "MontoDescuento";
            c10.HeaderText = "MontoDescuento";
            c10.Name = "MontoDescuento";
            DataGridViewTextBoxColumn c11 = new DataGridViewTextBoxColumn();
            c11.DataPropertyName = "IdPropietario";
            c11.HeaderText = "IdPropietario";
            c11.Name = "IdPropietario";
            gvPropiedades.Columns.Add(c1);
            gvPropiedades.Columns.Add(c2);
            gvPropiedades.Columns.Add(c3);
            gvPropiedades.Columns.Add(c4);
            gvPropiedades.Columns.Add(c5);
            gvPropiedades.Columns.Add(c6);
            gvPropiedades.Columns.Add(c7);
            gvPropiedades.Columns.Add(c8);
            gvPropiedades.Columns.Add(c9);
            gvPropiedades.Columns.Add(c10);
            gvPropiedades.Columns.Add(c11);
            gvPropiedades.Columns[10].Visible = false;
            gvPropiedades.DataSource = result;
        }
        private void saveGood(Propiedad prop1)
        {
            //Save new Good
            Entities.Propiedads.Add(prop1);
            Entities.SaveChanges();
            retrieveGoods();
            limpiar();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            retrieveOwners();
            retrieveGoods();
        }


        private void limpiar()
        {
            cbPropietario.SelectedIndex = 0;
            txtArea.Text = "";
            cbTipo.SelectedIndex = 0;
            txtNro.Text = "";
            txtArea.Text = "";
            txtPorcentaje.Text = "";
            txtDescuento.Text = "";
            cbPiso.SelectedIndex = 0;
            chkDescuento.Checked = false;
            chkHabilitado.Checked = false;
            gvPropiedades.ClearSelection();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                //Use model classes
                Propiedad good = new Propiedad();
                good.Tipo = cbTipo.SelectedItem.ToString();
                good.Nro = txtNro.Text;
                good.Piso = cbPiso.SelectedItem.ToString();
                good.Area = decimal.Parse(txtArea.Text.ToString());
                good.Porcentaje = decimal.Parse(txtPorcentaje.Text.ToString());
                good.Descuento = chkDescuento.Checked;
                good.Habitado = chkHabilitado.Checked;
                good.MontoDescuento = decimal.Parse(txtDescuento.Text.ToString());
                good.IdPropietario = int.Parse(cbPropietario.SelectedValue.ToString());
                saveGood(good);
                MessageBox.Show("Guardado correctamente");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            try {
                var id = int.Parse(gvPropiedades.SelectedRows[0].Cells["Id"].Value.ToString());
                //Retrieve item from table
                var item = Entities.Propiedads.FirstOrDefault(a => a.Id == id);

                item.IdPropietario = int.Parse(cbPropietario.SelectedValue.ToString());
                item.Tipo = cbTipo.SelectedItem.ToString();
                item.Nro = txtNro.Text;
                item.Piso = cbPiso.SelectedItem.ToString();
                item.Area = decimal.Parse(txtArea.Text.ToString());
                item.Porcentaje = decimal.Parse(txtPorcentaje.Text.ToString());
                item.Descuento = chkDescuento.Checked;
                item.Habitado = chkHabilitado.Checked;
                item.MontoDescuento = decimal.Parse(txtDescuento.Text.ToString());

                Entities.SaveChanges();
                MessageBox.Show("Editado correctamente");
                retrieveGoods();
            }
            catch (Exception ex) {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                var id = int.Parse(gvPropiedades.SelectedRows[0].Cells["Id"].Value.ToString());
                //Retrieve item from table
                var item = Entities.Propiedads.FirstOrDefault(a => a.Id == id);

                //DialogResult para confirmar eliminacion
                DialogResult dr = MessageBox.Show($"Esta seguro que desea eliminar el item con ID {id}",
                    "Delete", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                if (dr == DialogResult.Yes)
                {
                    Entities.Propiedads.Remove(item);
                    Entities.SaveChanges();
                    MessageBox.Show("Eliminado correctamente");
                    retrieveGoods();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void gvPropiedades_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                //https://stackoverflow.com/questions/7657137/datagridview-full-row-selection-but-get-single-cell-value
                //Con este evento manejamos adecuadamente al momento de seleccionar y borrar la seleccion en el DGV
                if (gvPropiedades.SelectedRows.Count > 0)
                {
                    txtNro.Text = gvPropiedades.SelectedRows[0].Cells["Nro"].Value.ToString();
                    txtArea.Text = gvPropiedades.SelectedRows[0].Cells["Area"].Value.ToString();
                    txtPorcentaje.Text = gvPropiedades.SelectedRows[0].Cells["Porcentaje"].Value.ToString();
                    txtDescuento.Text = gvPropiedades.SelectedRows[0].Cells["MontoDescuento"].Value.ToString();
                    chkDescuento.Checked = bool.Parse(gvPropiedades.SelectedRows[0].Cells["Descuento"].Value.ToString()) ? true : false;
                    chkHabilitado.Checked = bool.Parse(gvPropiedades.SelectedRows[0].Cells["Habitado"].Value.ToString()) ? true : false;
                    cbTipo.SelectedItem = gvPropiedades.SelectedRows[0].Cells["Tipo"].Value;
                    cbPiso.SelectedItem = gvPropiedades.SelectedRows[0].Cells["Piso"].Value;
                    cbPropietario.SelectedValue = gvPropiedades.SelectedRows[0].Cells["IdPropietario"].Value;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
    }
}
