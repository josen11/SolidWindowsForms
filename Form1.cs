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
    public partial class Form1 : Form
    {
        //Use Name of Entinty Container of our EF Data Model
        private readonly DBEdificioEntities Entities;

        public Form1()
        {
            InitializeComponent();
            Entities = new DBEdificioEntities();
        }
        private void retrieveOwners()
        {         
            // Select * from Propietario con LINQ aprovechando el tema de concatear el nombres y apellidos desde la consulta
            var result = (from a in Entities.Propietarios.ToList()
                          select new { Id = a.Id, Name = a.Nombres +" "+ a.Apellidos}).ToList();
            
            // Genial una manera de asociar el index y el valor mostrado en el Combobox
            cbPropietario.DisplayMember= "Name";
            cbPropietario.ValueMember = "Id";
            cbPropietario.DataSource = result;
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            retrieveOwners();
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }
    }
}
