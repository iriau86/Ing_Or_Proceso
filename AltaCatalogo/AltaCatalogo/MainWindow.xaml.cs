using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace AltaCatalogo
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }



        private System.Windows.Media.Imaging.BitmapImage ObtenImagen(string cCod) {
           // Image img;

            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;

            try
            {

                using (SqlCommand cmd = cn.CreateCommand())
            {

                cn.Open();
                cmd.CommandText = "Select iImagen from  Producto Where cCod='"+ cCod + "'";
               // byte[] arrImg = (byte[])
                String arrImg=(String)cmd.ExecuteScalar();
                cn.Close();
                //MemoryStream ms = new MemoryStream(arrImg);
                    //img = Image.FromStream(ms);
                    //using (MemoryStream memory = new MemoryStream(arrImg))
                    //{
                    //    memory.Position = 0;
                       BitmapImage bitmapimage = new BitmapImage();
                    //    bitmapimage.BeginInit();
                    //    bitmapimage.StreamSource = memory;
                    //    bitmapimage.EndInit();
                        return bitmapimage;
                    //}
                //return img;

                }

            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        private void imagen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg";
            if (openFileDialog.ShowDialog() == true)
            {
                txt_nomimg.Text = openFileDialog.FileName.ToString();
                //txt_nomimg.Text = File.ReadAllText(openFileDialog.FileName.ToString());
            }


        }

        private void guardaFotoDB(string codigo , string filefoto) {
            
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;

            try {

                MemoryStream ms = new MemoryStream();
                FileStream fs = new FileStream(filefoto, FileMode.Open,FileAccess.Read,FileShare.ReadWrite);

                ms.SetLength(fs.Length);
                fs.Read(ms.GetBuffer(), 0, (int)fs.Length);
                byte[] arrImg = ms.GetBuffer();
                ms.Flush();
                ms.Close();


                using (SqlCommand cmd = cn.CreateCommand()) {


                    cn.Open();
                    cmd.CommandText = "insert into Producto ( cCod,cNomProd,cDescripcion ,dCostoPza ,dPeso,bActive ,cUnidad ,iImagen) values (@cCod,@cNomProd,@cDescripcion,@dCostoPza,@dPeso,@bActive ,@cUnidad ,@iImagen)";
                    cmd.Parameters.Add("@cCod",SqlDbType.VarChar).Value= codigo;
                    cmd.Parameters.Add("@cNomProd", SqlDbType.VarChar).Value = "Polvo de Nopal";
                    cmd.Parameters.Add("@cDescripcion", SqlDbType.VarChar).Value = "Polvo de nopal deshidratado";
                    cmd.Parameters.Add("@dCostoPza", SqlDbType.Float).Value = 12;
                    cmd.Parameters.Add("@dPeso", SqlDbType.Float).Value = 40;
                    cmd.Parameters.Add("@bActive", SqlDbType.Bit).Value = true;
                    cmd.Parameters.Add("@cUnidad", SqlDbType.VarChar).Value = "gr";
                    cmd.Parameters.Add("@iImagen", SqlDbType.VarBinary).Value = arrImg;

                    cmd.ExecuteNonQuery();
                    cn.Close();

                }

            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }

        }

        private void SaveImage_Click(object sender, RoutedEventArgs e)
        {

            guardaFotoDB("Fsdfsf-4353", txt_nomimg.Text.ToString());

        }

        private void muestraIm_Click(object sender, RoutedEventArgs e)
        {
            try {
                boxpic.Stretch = Stretch.Fill;
                boxpic.Source= ObtenImagen("Fsdfsf-4353");
               
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
