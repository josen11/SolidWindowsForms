using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolidWindowsForms
{
    public static class Utils
    {
        public static bool FormIsOpen(string name)
        {
            // Genial una buena manera de manejar la apertura de nuevos Forms cada vez que se hace click
            var OpenForms = Application.OpenForms.Cast<Form>();
            var isOpen = OpenForms.Any(q => q.Name == name);
            return isOpen;
        }
        public static string Hash256Password(string pass)
        {
            //Encryptar Password ingresado y compararlo con el password encryptado a nivel de BD
            SHA256 sha = SHA256.Create();
            //Convert the input string to a byte array and compute the hash
            byte[] data = sha.ComputeHash(Encoding.UTF8.GetBytes(pass));
            //Create a String Builder to collect the byte in a string
            StringBuilder sBuilder = new StringBuilder();
            //Loop through each byte of the hashed data and format each one as a hexadecmal string
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("X2"));
            }
            var hashed_password = sBuilder.ToString();
            return hashed_password;
        }
        public static string DeHash256Password(string hashpass)
        {
            //Encryptar Password ingresado y compararlo con el password encryptado a nivel de BD
            SHA256 sha = SHA256.Create();
            //Convert the input string to a byte array and compute the hash
            byte[] data = sha.ComputeHash(Encoding.UTF8.GetBytes(hashpass));
            //Create a String Builder to collect the byte in a string
            StringBuilder sBuilder = new StringBuilder();
            //Loop through each byte of the hashed data and format each one as a hexadecmal string
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("X2"));
            }
            return sBuilder.ToString();
        }
    }
}
