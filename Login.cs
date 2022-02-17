using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace SolidWindowsForms
{
    public partial class Login : Form
    {
        private readonly DBEdificioEntities Entities;
        public Login()
        {
            InitializeComponent();
            Entities = new DBEdificioEntities();
        }

        //Parte de la funcionalidad de arrastrar el formulario
        [DllImport("user32.dll", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int lparam);

        private void txtUser_Enter(object sender, EventArgs e)
        {
            //Evento cuanto el cursor del mouse esta dentro
            if (txtUser.Text == "Usuario") {
                txtUser.Text = "";
            }
        }

        private void txtUser_Leave(object sender, EventArgs e)
        {
            //Evento cuando el cursor del mouse sale
            if (txtUser.Text == "")
            {
                txtUser.Text = "Usuario";
            }
        }

        private void txtPass_Enter(object sender, EventArgs e)
        {
            if (txtPass.Text == "Contraseña")
            {
                txtPass.Text = "";
                txtPass.UseSystemPasswordChar = true;
            }
        }

        private void txtPass_Leave(object sender, EventArgs e)
        {
            if (txtPass.Text == "")
            {
                txtPass.Text = "Contraseña";
                txtPass.UseSystemPasswordChar = false;
            }
        }

        //Evento donde arrastramos el formulario con el mouse
        private void Login_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUser.Text != "Usuario")
            {
                if (txtPass.Text != "Contraseña")
                {
                    try
                    {
                        var user = txtUser.Text.Trim();
                        var pass = txtPass.Text;
                         //admin = {8C6976E5B5410415BDE908BD4DEE15DFB167A9C873FC4BB8A81F6F2AB448A918}
                        var hashed_password = Utils.Hash256Password(pass);

                        var itemUser = Entities.Users.FirstOrDefault(a => a.username == user && a.password == hashed_password && a.isActive ==true);
                        if (itemUser == null)
                        {
                            txtPass.Text = "Contraseña";
                            txtUser.Focus();
                            MostrarMsgError($"Error: Usuario o contraseña incorrecta");
                        }   
                        else
                        {
                            var role = itemUser.UserRoles.FirstOrDefault();
                            var roleName = role.Role.name;
                            // Crear un constructor que tenga el valor del login y del  User (esto para saber quien se logueo y que Rol tiene)
                            MainWindow main = new MainWindow(this, itemUser);
                            main.Show();
                            Hide();
                        }
                    }
                    catch (Exception ex)
                    {
                        MostrarMsgError($"Error:{ex.Message}");
                    }
                }
                else
                {
                    MostrarMsgError("Por favor ingresar contraseña");
                }
            }
            else
            {
                MostrarMsgError("Por favor ingresar usuario");
            }
        }

        private void MostrarMsgError(string msg)
        {
            lbMsgError.Text = "       " + msg;
            lbMsgError.Visible = true;
        }

        //Con este metodo cerramos sesion
        private void CerrarSesion(object sender, FormClosedEventArgs e) {
            txtPass.Text="Contraseña";
            txtPass.UseSystemPasswordChar = false;
            txtUser.Text = "Usuario";
            lbMsgError.Visible = false;
            this.Show();
        }

        private void lkRecuperar_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //var RecoveryForm = new PasswordRecovery();
            //RecoveryForm.ShowDialog();
        }
    }
}
