using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolidWindowsForms
{
    public partial class MainWindow : Form
    {
        private Login _login;
        public string _roleName;
        public User _user;
        private PropiedadWindow propiedadwindow;
        public MainWindow()
        {
            InitializeComponent();
        }
        public MainWindow(Login login, User user)
        {
            InitializeComponent();
            _login = login;
            _user = user;
            //Interesante forma de hacer un Innerjoin con LINQ
            _roleName = user.UserRoles.FirstOrDefault().Role.name;
        }

        private void propiedadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var isOpen = Utils.FormIsOpen("PropiedadWindow");
            if (!isOpen)
            {
                // https://www.youtube.com/embed/rxsZc7KSwdE
                propiedadwindow = new PropiedadWindow();
                propiedadwindow.MdiParent = this;
                propiedadwindow.Dock = DockStyle.Fill;
                propiedadwindow.Show();
            }
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            if (_roleName != "admin")
            {
                administrarUsuariosToolStripMenuItem.Visible = false;
            }
            tssUser.Text = $"Logged in as :{_user.username}";
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Cerrar form login recordemos que solo lo ocultamos
            _login.Close();
        }
    }
}
