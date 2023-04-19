using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace adonett
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string baglanticumlesi = "Data Source=nahidesumer;Initial Catalog=Northwind; UserID= nahidesumer; Password=nahidesumer0798+";
        //UserID= sa; Password=Fbu123456
        //    integrated security () kullanıcı adı sifre yok. herkes benim sistemime connected olabilir 
        // odev- professional indir teams ögren

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                #region veri listeleme
                string komut = "select*from Employees";
                using (SqlConnection baglanti = new SqlConnection())
                {
                    baglanti.ConnectionString = baglanticumlesi;
                    using (SqlCommand listelemekomut = new SqlCommand(komut, baglanti))
                    {
                        baglanti.Open();
                        using (DataTable datatablosu = new DataTable())
                        {
                            datatablosu.Columns.Add("PersonelKimliNO");
                            datatablosu.Columns.Add("Adı");
                            datatablosu.Columns.Add("Soyadı");
                            using (SqlDataReader okuyucu = listelemekomut.ExecuteReader())
                            {
                                while (okuyucu.Read())
                                {
                                    DataRow row = datatablosu.NewRow();
                                    row["PersonelKimliNO"] = okuyucu["EmployeeID"];
                                    row["Adı"] = okuyucu["FirstName"];
                                    row["Soyadı"] = okuyucu["LastName"];
                                    datatablosu.Rows.Add(row);
                                }
                                dataGridView1.DataSource = datatablosu;
                            }
                        }
                        baglanti.Close();
                    }
                    if (baglanti.State == System.Data.ConnectionState.Closed)
                        toolStripStatusLabel1.Text = "Bağlantı kapalı";
                    else
                        toolStripStatusLabel1.Text = "bağlantı açık";
                }
                #endregion
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                #region veriekleme
               
                using (SqlConnection baglanti = new SqlConnection())
                {
                   
                    baglanti.ConnectionString = baglanticumlesi;
                    using (SqlCommand eklekomut = new SqlCommand())
                    {
                        baglanti.Open();
                        eklekomut.Connection = baglanti;
                        eklekomut.CommandType = CommandType.Text;
                        eklekomut.CommandText = "Insert into Employees(FirstName,LastName) values(@FirstName,@LastName)"; // property
                        eklekomut.Parameters.AddWithValue("@FirstName", textBox1.Text); //metot
                        eklekomut.Parameters.AddWithValue("@LastName", textBox2.Text);
                        bool karakter=false;
                        //charisletter ile harf mi kontrolü
                        for (int i = 0; i <(textBox1.Text).Length; i++)
                        {
                            if (char.IsLetter(Convert.ToChar((textBox1.Text).Substring(i, 1))))
                                karakter = true;
                            else
                            {
                                karakter= false;
                                break;
                            }
                        }
                        if(karakter)
                        {
                            eklekomut.ExecuteNonQuery();
                            MessageBox.Show("Kayıt başarılı");
                         
                        }
                        else
                        {
                            MessageBox.Show("Kayıt olmadı çünkü adı alanında nümerik değer var");
                        }
                        textBox1.Clear();
                        textBox2.Clear();
                        textBox1.Select();
                    }
                    baglanti.Close();
                }
                Form1_Load(this, null); // Form1 load çalışacak güncelleme yaptığımızda yeni kayıt eklenmesi için.

                #endregion
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
    }
}
