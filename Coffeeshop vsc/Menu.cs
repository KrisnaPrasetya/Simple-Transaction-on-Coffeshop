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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Pembayaran_di_CoffeeShop
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
            LoadMenu();
        }
        private void Menu_Load(object sender, EventArgs e)
        {

        }

        public void LoadMenu()
        {
            KoneksiSQL.buka();
            dataGridView1.Rows.Clear();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader rd;
            cmd.CommandText = $"select * FROM menu WHERE nama_menu LIKE '%{textBox1.Text}%'order by id_menu ";
            cmd.Connection = KoneksiSQL.sqlConn;
            rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                int newindex = dataGridView1.Rows.Add();
                dataGridView1.Rows[newindex].Cells[0].Value = rd["id_menu"].ToString();
                dataGridView1.Rows[newindex].Cells[1].Value = rd["nama_menu"].ToString();
                dataGridView1.Rows[newindex].Cells[2].Value = rd["harga"].ToString();
                dataGridView1.Rows[newindex].Cells[3].Value = "EDIT";
                dataGridView1.Rows[newindex].Cells[4].Value = "DELETE";
            }
            cmd.Dispose();
            rd.Close();
            KoneksiSQL.tutup();
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.ColumnIndex == 3)
            {
                TambahMenu frm = new TambahMenu();
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.Text = "Edit Menu";
                frm.id_menu_edit = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                frm.TextNamaMenu.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                frm.textHarga.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();

                if (frm.ShowDialog() == DialogResult.OK)
                {
                    LoadMenu();
                }

            }

            if (e.ColumnIndex == 4)
            {
                if (MessageBox.Show("Apakah kamu yakin ingin menghapus Menu?",
                    "Konfirmasi",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Warning) == DialogResult.Cancel)
                {
                    return;
                }

                KoneksiSQL.buka();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = KoneksiSQL.sqlConn;
                cmd.CommandText = "DELETE FROM menu WHERE id_menu = @pID";
                cmd.Parameters.AddWithValue("pID", dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                KoneksiSQL.tutup();
                LoadMenu();

            }

        }
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {


            if (e.KeyCode == Keys.Enter)
            {
                LoadMenu();
            }


        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            TambahMenu frm = new TambahMenu();
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.Text = "Tambah Menu Baru";
            if (frm.ShowDialog() == DialogResult.OK)
            {
                LoadMenu();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

    }
}
