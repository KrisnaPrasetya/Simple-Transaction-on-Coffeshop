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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Pembayaran_di_CoffeeShop
{
    public partial class HistoryTransaksi : Form
    {
        public HistoryTransaksi()
        {
            InitializeComponent();
            loadTransaksi();
        }

        private void HistoryTransaksi_Load(object sender, EventArgs e)
        {

        }

        public void loadTransaksi()
        {
            KoneksiSQL.buka();
            dataGridView1.Rows.Clear();

            // Ganti query dengan yang sesuai
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = $"SELECT c.nama_customer, t.id_transaksi, t.total_harga, t.tanggal_transaksi FROM transaksi t JOIN customer c ON t.id_customer = c.id_customer";
            cmd.Connection = KoneksiSQL.sqlConn;
            SqlDataReader rd = cmd.ExecuteReader();

            while (rd.Read())
            {
                int newindex = dataGridView1.Rows.Add();
                dataGridView1.Rows[newindex].Cells[0].Value = rd["id_transaksi"].ToString();
                dataGridView1.Rows[newindex].Cells[1].Value = rd["nama_customer"].ToString(); 
                dataGridView1.Rows[newindex].Cells[2].Value = rd["total_harga"].ToString();
                dataGridView1.Rows[newindex].Cells[3].Value = rd["tanggal_transaksi"].ToString(); 
            }

            cmd.Dispose();
            rd.Close();
            KoneksiSQL.tutup();
        }

        public void loadTransaksiTanggal()
        {
            KoneksiSQL.buka();
            dataGridView1.Rows.Clear();

            // Ganti query dengan yang sesuai
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = $"SELECT c.nama_customer, t.id_transaksi, t.total_harga, t.tanggal_transaksi FROM transaksi t JOIN customer c ON t.id_customer = c.id_customer WHERE t.tanggal_transaksi = @tanggal";
            cmd.Parameters.AddWithValue("@tanggal", dateTimePicker1.Value.Date);
            cmd.Connection = KoneksiSQL.sqlConn;
            SqlDataReader rd = cmd.ExecuteReader();

            while (rd.Read())
            {
                int newindex = dataGridView1.Rows.Add();
                dataGridView1.Rows[newindex].Cells[0].Value = rd["id_transaksi"].ToString();
                dataGridView1.Rows[newindex].Cells[1].Value = rd["nama_customer"].ToString(); 
                dataGridView1.Rows[newindex].Cells[2].Value = rd["total_harga"].ToString();
                dataGridView1.Rows[newindex].Cells[3].Value = rd["tanggal_transaksi"].ToString(); 
            }

            cmd.Dispose();
            rd.Close();
            KoneksiSQL.tutup();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            loadTransaksiTanggal();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            loadTransaksi();
        }
    }
}
