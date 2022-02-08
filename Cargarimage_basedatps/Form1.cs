using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;


namespace Cargarimage_basedatps
{
    public partial class Form1 : Form
    {

        
        
        
        public Form1()
        {
            InitializeComponent();
        }

            
       SqlConnection connection = new SqlConnection("Data Source=LAPTOP-8UITBRBH;Initial Catalog=imagenDB;Integrated Security=True");
        
        string imgLocation = "";
        SqlCommand com ;

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //cargar imagen
             OpenFileDialog dialogo = new OpenFileDialog();
            dialogo.Filter = "png files(*.png) |*.png|jpg files(*.jpg)|*.jpg|All files(*.*)|*.*";
            if (dialogo.ShowDialog() == DialogResult.OK) 
            {
                imgLocation = dialogo.FileName.ToString();
                pictureBox1.ImageLocation = imgLocation;

            }
        }

        private void button1_Click(object sender, EventArgs e) 
        {
            //BOTON GUARDAR

            byte[] images = null;
            FileStream fls = new FileStream(imgLocation, FileMode.Open, FileAccess.Read);
            BinaryReader reader =  new BinaryReader(fls);
            images = reader.ReadBytes((int)fls.Length);

            connection.Open();
          
            String sqlQuery = "Insert into [dbo].[imagedatab](Id,Image) values('" + textBox1.Text + "',@images)";
            com = new SqlCommand(sqlQuery, connection);
            com.Parameters.Add(new SqlParameter("@images", images));
            int N = com.ExecuteNonQuery();
            connection.Close();
            MessageBox.Show(N.ToString() + "Guardado en la base de datos");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection("Data Source=LAPTOP-8UITBRBH;Initial Catalog=imagenDB;Integrated Security=True");
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "SELECT [Image] FROM [dbo].[imagedatab] where Id = '" + textBox2.Text + "'";
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            if (ds.Tables[0].Rows.Count == 0) 
            {
                MessageBox.Show("imagen no existe");
            }
            else 
            {
            MemoryStream ms = new MemoryStream((byte[])ds.Tables[0].Rows[0][0]);
                pictureBox2.Image = new Bitmap(ms);
            }


        }
    }
}
