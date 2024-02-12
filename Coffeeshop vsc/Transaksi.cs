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

namespace Pembayaran_di_CoffeeShop
{
    public partial class Transaksi : Form
    {

        public decimal total_harga { get; set; }
        public string total_harga_text { get; set; }


        public Transaksi()
        {
            InitializeComponent();
            LoadMenu();
            total_harga = 0;
            total_harga_text = "";
            comboBox1.Enabled = false;
            comboBox1.Text = "Bukan Member";
            textBox1.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy");
        }

        public void LoadMenu()
        { 
            KoneksiSQL.buka();
            dataGridView1.Rows.Clear();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader rd;
            cmd.CommandText = $"select * FROM menu WHERE nama_menu LIKE '%{textBox2.Text}%'order by id_menu ";
            cmd.Connection = KoneksiSQL.sqlConn;
            rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                int newindex = dataGridView1.Rows.Add();
                dataGridView1.Rows[newindex].Cells[0].Value = rd["nama_menu"].ToString();
                dataGridView1.Rows[newindex].Cells[1].Value = rd["harga"].ToString();
                dataGridView1.Rows[newindex].Cells[2].Value = 0;
                dataGridView1.Rows[newindex].Cells[3].Value = "Hapus";
                dataGridView1.Rows[newindex].Cells[4].Value = "Pilih";
            }
            cmd.Dispose();
            rd.Close();
            KoneksiSQL.tutup();

        }
        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                LoadMenu();
            }

        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 4) // "Pilih" button clicked
            {
                int rowIndex = e.RowIndex;
                int currentQuantity = Convert.ToInt32(dataGridView1.Rows[rowIndex].Cells[2].Value);
                dataGridView1.Rows[rowIndex].Cells[2].Value = currentQuantity + 1;

                // Calculate total price
                decimal harga = Convert.ToDecimal(dataGridView1.Rows[rowIndex].Cells[1].Value);
                decimal total_harga = harga * currentQuantity;
                this.total_harga_text = total_harga.ToString();

                // Set total price text
                HitungTotalHarga();
            }
            else if (e.ColumnIndex == 3) // "Hapus" button clicked
            {
                int rowIndex = e.RowIndex;
                dataGridView1.Rows[rowIndex].Cells[2].Value = 0;

                // Calculate total price
                decimal harga = Convert.ToDecimal(dataGridView1.Rows[rowIndex].Cells[1].Value);
                decimal total_harga = harga * 0;
                this.total_harga_text = total_harga.ToString();

                // Set total price text
                HitungTotalHarga();
            }
        }
        private void HitungTotalHarga()
        {
            decimal totalHarga = 0;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                decimal harga = Convert.ToDecimal(row.Cells[1].Value);
                int quantity = Convert.ToInt32(row.Cells[2].Value);
                decimal subTotal = harga * quantity;
                totalHarga += subTotal;
            }

            textBox3.Text = totalHarga.ToString();
            this.total_harga = totalHarga;
            this.total_harga_text = totalHarga.ToString();
        }
     
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //ini default checkbox
            

            if (checkBox1.Checked)
            {
                comboBox1.Enabled = true; // Aktifkan combobox
                FillComboBox(); // Isi combobox dengan nama member sesuai abjad huruf
            }
            else
            {
                comboBox1.Enabled = false; // Nonaktifkan combobox jika checklist tidak dicentang
                comboBox1.Items.Clear(); // Hapus isi combobox
                comboBox1.Text = "Bukan Member";
            }
        }


        private void FillComboBox()
        {
            // Buat koneksi ke database
            KoneksiSQL.buka();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader rd;
            cmd.CommandText = "SELECT nama_customer FROM customer ORDER BY id_customer";
            cmd.Connection = KoneksiSQL.sqlConn;
            rd = cmd.ExecuteReader();


            // Tambahkan nama-nama member ke dalam combobox
            while (rd.Read())
            {
                comboBox1.Items.Add(rd["nama_customer"].ToString());
            }

            // Tutup koneksi dan reader
            cmd.Dispose();
            rd.Close();
            KoneksiSQL.tutup();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string memberName = comboBox1.SelectedItem == null ? " " : comboBox1.SelectedItem.ToString();

            // Get the total price from the textbox
            decimal totalPrice = Convert.ToDecimal(textBox3.Text);

            // Get the current date and time
            DateTime transactionDate = DateTime.Now;

            // Save the transaction to the database
            KoneksiSQL.buka();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = KoneksiSQL.sqlConn;
            

            if (memberName == " "){

                int bukanMember = 1 ;

                cmd.CommandText = "INSERT INTO transaksi (id_customer, total_harga, tanggal_transaksi) "
                    + "VALUES (@idCustomer, @totalHarga, @tanggalTrans) ";
                cmd.Parameters.AddWithValue("idCustomer",bukanMember);


                // Set the other 
                cmd.Parameters.AddWithValue("totalHarga", totalPrice);
                cmd.Parameters.AddWithValue("tanggalTrans", transactionDate);


            }
            else
            {
                cmd.CommandText = "SELECT id_customer FROM customer WHERE nama_customer = @nama_customer";
                cmd.Parameters.AddWithValue("nama_customer", memberName);
                int idCustomer = (int)cmd.ExecuteScalar();
                cmd.Parameters.Clear();

                cmd.CommandText = "INSERT INTO transaksi (id_customer, total_harga, tanggal_transaksi) "
                        + "VALUES (@idCustomer, @totalHarga, @tanggalTrans) ";
                cmd.Parameters.AddWithValue("idCustomer", idCustomer);


                // Set the other parameters
                cmd.Parameters.AddWithValue("totalHarga", totalPrice);
                cmd.Parameters.AddWithValue("tanggalTrans", transactionDate);
            }

            

            cmd.ExecuteNonQuery();
            cmd.Dispose();
            KoneksiSQL.tutup();

            // Update reward points for the member
            if (memberName != "Bukan Member")
            {
                UpdateRewardPoints(memberName);
            }

            if (MessageBox.Show("Pembayaran Berhasil",
                    "Informasi",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Information) == DialogResult.Cancel)
            {
                return;
            }

            // Clear the form
            LoadMenu();
            textBox3.Clear();
            comboBox1.Enabled = false;
            comboBox1.Text = "Bukan Member";
            checkBox1.Checked = false;
        }

    

        private void UpdateRewardPoints(string namaMember)
        {
            // Lakukan query untuk mendapatkan jumlah poin reward saat ini dari anggota
            int poinSaatIni = 0;
            KoneksiSQL.buka();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT jumlah_point FROM customer WHERE nama_customer = @nama_customer";
            cmd.Parameters.AddWithValue("@nama_customer", namaMember);
            cmd.Connection = KoneksiSQL.sqlConn;
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                poinSaatIni = Convert.ToInt32(reader["jumlah_point"]);
            }
            reader.Close();

            // Tambahkan 1 poin reward
            poinSaatIni += 1;

            // Update poin reward di database
            cmd.CommandText = "UPDATE customer SET jumlah_point = @jumlah_point WHERE nama_customer = @nama_customer";
            cmd.Parameters.AddWithValue("@jumlah_point", poinSaatIni);
            cmd.ExecuteNonQuery();
            KoneksiSQL.tutup();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Transaksi_Load(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

       
        
    }
}
