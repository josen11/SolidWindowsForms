using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolidWindowsForms
{
    public partial class UserWindow : Form
    {
        //Use Name of Entinty Container of our EF Data Model
        private readonly DBEdificioEntities Entities;

        public UserWindow()
        {
            InitializeComponent();
            Entities = new DBEdificioEntities();
        }
        private void retrieveRoles()
        {
            // Select * from Propietario con LINQ aprovechando el tema de concatear el nombres y apellidos desde la consulta
            var result = (from a in Entities.Roles.ToList()
                          select new { Id = a.id, Name = a.name}).ToList();

            // Genial una manera de asociar el index y el valor mostrado en el Combobox
            cbRol.DisplayMember = "Name";
            cbRol.ValueMember = "Id";
            cbRol.DataSource = result;

        }
        private void retrieveUsers()
        {
            // Podemos uar Lambda expression tambien
            var result = Entities.UserRoles.Select(a => new
            {
                Id = a.id,
                UserId = a.User.id,
                UserName = a.User.username,
                RolId = a.Role.id,
                RolName = a.Role.name,
                UserActive = a.User.isActive,
                Password = a.User.password
            })
            .ToList();

            gvUsuarios.AutoGenerateColumns = false;
            DataGridViewTextBoxColumn c1 = new DataGridViewTextBoxColumn();
            c1.DataPropertyName = "Id";
            c1.HeaderText = "Id";
            c1.Name = "Id";
            DataGridViewTextBoxColumn c2 = new DataGridViewTextBoxColumn();
            c2.DataPropertyName = "UserId";
            c2.HeaderText = "UserId";
            c2.Name = "UserId";
            DataGridViewTextBoxColumn c3 = new DataGridViewTextBoxColumn();
            c3.DataPropertyName = "UserName";
            c3.HeaderText = "UserName";
            c3.Name = "UserName";
            DataGridViewTextBoxColumn c4 = new DataGridViewTextBoxColumn();
            c4.DataPropertyName = "RolId";
            c4.HeaderText = "RolId";
            c4.Name = "RolId";
            DataGridViewTextBoxColumn c5 = new DataGridViewTextBoxColumn();
            c5.DataPropertyName = "RolName";
            c5.HeaderText = "RolName";
            c5.Name = "RolName";
            DataGridViewCheckBoxColumn c6 = new DataGridViewCheckBoxColumn();
            c6.DataPropertyName = "UserActive";
            c6.HeaderText = "UserActive";
            c6.Name = "UserActive";
            DataGridViewTextBoxColumn c7 = new DataGridViewTextBoxColumn();
            c7.DataPropertyName = "Password";
            c7.HeaderText = "Password";
            c7.Name = "Password";
            gvUsuarios.Columns.Add(c1);
            gvUsuarios.Columns.Add(c2);
            gvUsuarios.Columns.Add(c3);
            gvUsuarios.Columns.Add(c4);
            gvUsuarios.Columns.Add(c5);
            gvUsuarios.Columns.Add(c6);
            gvUsuarios.Columns.Add(c7);
            gvUsuarios.Columns[0].Visible = false;
            gvUsuarios.Columns[6].Visible = false;
            gvUsuarios.DataSource = result;
        }
        private void saveUser(User user, int idRole)
        {
            //Save new user
            Entities.Users.Add(user);
            Entities.SaveChanges();

            //Save new userRole
            // Get id of inserted entity
            // https://entityframework.net/retrieve-id-of-inserted-entity
            int idUser = user.id;
            UserRole item = new UserRole();
            item.userid = idUser;
            item.roleid = idRole;
            Entities.UserRoles.Add(item);
            Entities.SaveChanges();

            retrieveUsers();
            limpiar();
        }
        private void limpiar()
        {
            cbRol.SelectedIndex = 0;
            txtUsuario.Text = "";
            txtContraseaña.Text = "";
            chkActivo.Checked = false;
            gvUsuarios.ClearSelection();
        }

        private void UserWindow_Load(object sender, EventArgs e)
        {
            retrieveRoles();
            retrieveUsers();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                var pass = txtContraseaña.Text;
                var hashed_password = Utils.Hash256Password(pass);
                //Use model classes
                User user = new User();
                user.username = txtUsuario.Text.ToString();
                user.password = hashed_password;
                user.isActive = chkActivo.Checked;
              
                saveUser(user, int.Parse(cbRol.SelectedValue.ToString()));
                MessageBox.Show("Guardado correctamente");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                var pass = txtContraseaña.Text;
                var hashed_password = Utils.Hash256Password(pass);
                var id = int.Parse(gvUsuarios.SelectedRows[0].Cells["Id"].Value.ToString());
                var idUser = int.Parse(gvUsuarios.SelectedRows[0].Cells["UserId"].Value.ToString());
                //Retrieve item from table
                var itemUserRole = Entities.UserRoles.FirstOrDefault(a => a.id == id);
                var itemUser = Entities.Users.FirstOrDefault(a => a.id == idUser);

                itemUser.username = txtUsuario.Text.ToString();
                itemUser.password = hashed_password;
                itemUser.isActive = chkActivo.Checked;

                itemUserRole.userid = idUser;
                itemUserRole.roleid = int.Parse(cbRol.SelectedValue.ToString());

                Entities.SaveChanges();
                MessageBox.Show("Editado correctamente");
                retrieveUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                var id = int.Parse(gvUsuarios.SelectedRows[0].Cells["Id"].Value.ToString());
                var idUser = int.Parse(gvUsuarios.SelectedRows[0].Cells["UserId"].Value.ToString());
                var userName = int.Parse(gvUsuarios.SelectedRows[0].Cells["UserName"].Value.ToString());
                //Retrieve item from table
                var itemUserRole = Entities.UserRoles.FirstOrDefault(a => a.id == id);
                var itemUser = Entities.Users.FirstOrDefault(a => a.id == idUser);

                //DialogResult para confirmar eliminacion
                DialogResult dr = MessageBox.Show($"Esta seguro que desea eliminar el item con usuario {userName}",
                    "Delete", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                if (dr == DialogResult.Yes)
                {
                    Entities.UserRoles.Remove(itemUserRole);
                    Entities.Users.Remove(itemUser);
                    Entities.SaveChanges();
                    MessageBox.Show("Eliminado correctamente");
                    retrieveUsers();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void gvUsuarios_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                //https://stackoverflow.com/questions/7657137/datagridview-full-row-selection-but-get-single-cell-value
                //Con este evento manejamos adecuadamente al momento de seleccionar y borrar la seleccion en el DGV
                if (gvUsuarios.SelectedRows.Count > 0)
                {
                    txtUsuario.Text= gvUsuarios.SelectedRows[0].Cells["UserName"].Value.ToString();
                    txtContraseaña.Text = gvUsuarios.SelectedRows[0].Cells["UserName"].Value.ToString();
                    chkActivo.Checked = bool.Parse(gvUsuarios.SelectedRows[0].Cells["UserActive"].Value.ToString()) ? true : false;
                    cbRol.SelectedValue = gvUsuarios.SelectedRows[0].Cells["RolId"].Value;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
    }
}
