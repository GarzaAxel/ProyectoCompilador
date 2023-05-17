using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoCompilador
{
    public partial class AgregarUsuario : Form
    {
        string server = "Data Source = LAPTOP-B2J62JB7; Initial Catalog = CompiladorDB; Integrated Security = True ";
        SqlConnection conectar = new SqlConnection();
        public AgregarUsuario()
        {
            InitializeComponent();
        }

        private void AgregarUsuario_Load(object sender, EventArgs e)
        {

        }
        public static string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Convertir la contraseña en un arreglo de bytes
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convertir el arreglo de bytes a una cadena hexadecimal
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            string contrasena = txtContrasena.Text;
            string hashedpassword = HashPassword(contrasena);

            conectar.ConnectionString = server;
            conectar.Open();
            SqlCommand cmd = new SqlCommand("AgregarUsuario", conectar);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id_Usuario", txtIDUSER.Text);
            cmd.Parameters.AddWithValue("@Usuario", txtUser.Text);           
            cmd.Parameters.AddWithValue("@Nombre", txtNombre.Text);
            cmd.Parameters.AddWithValue("@Appaterno", txtApPaterno.Text);
            cmd.Parameters.AddWithValue("@Apmaterno", txtApMaterno.Text);
            cmd.Parameters.AddWithValue("@Correo", txtCorreo.Text);
            cmd.Parameters.AddWithValue("@Telefono", txtTelefono.Text);
            cmd.Parameters.AddWithValue("Contrasena", hashedpassword);

            try
            {
                MessageBox.Show("Usuario Agregado Correctamente");
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString());               
            }
            conectar.Close();
        }
    }
}
