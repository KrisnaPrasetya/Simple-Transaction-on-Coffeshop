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
    public partial class Pelanggan : Form
    {
        public Pelanggan()
        {
            InitializeComponent();
            LoadDataMember();
        }

        private void Pelanggan_Load(object sender, EventArgs e)
        {

        }

        public void LoadDataMember()
        {
            KoneksiSQL.buka();
            dataGridView1.Rows.Clear();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader rd;
            cmd.CommandText = $"select * FROM customer WHERE nama_customer LIKE '%{textBox1.Text}%'order by id_customer ";
            cmd.Connection = KoneksiSQL.sqlConn;
            rd = cmd.ExecuteReader();
            while(rd.Read())
            {
                int newindex = dataGridView1.Rows.Add();
                dataGridView1.Rows[newindex].Cells[0].Value = rd["id_customer"].ToString();
                dataGridView1.Rows[newindex].Cells[1].Value = rd["nama_customer"].ToString();
                dataGridView1.Rows[newindex].Cells[2].Value = rd["nomor_telepon"].ToString();
                dataGridView1.Rows[newindex].Cells[3].Value = rd["jumlah_point"].ToString();
                dataGridView1.Rows[newindex].Cells[4].Value = "EDIT";
                dataGridView1.Rows[newindex].Cells[5].Value = "DELETE";
                dataGridView1.Rows[newindex].Cells[6].Value = "Pakai Point";

            }
            cmd.Dispose();
            rd.Close();
            KoneksiSQL.tutup();
        }



        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 4)
            {
                PelangganCreate frm = new PelangganCreate();
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.Text = "Edit Member";
                frm.id_member_edit = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                frm.txtNama.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                frm.txtNoTelephone.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();

                if(frm.ShowDialog() == DialogResult.OK)
                {
                    LoadDataMember();
                }

            }

            if (e.ColumnIndex == 5)
            {

                int customerID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);

                if (customerID == 1)
                {
                    MessageBox.Show("Data member dengan ID 1 tidak bisa dihapus.",
                        "Peringatan",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                if (MessageBox.Show("Apakah kamu yakin ingin menghapus member?",
                    "Konfirmasi",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Warning) == DialogResult.Cancel)
                {
                    return;
                }


                KoneksiSQL.buka();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = KoneksiSQL.sqlConn;
                cmd.CommandText = "DELETE FROM customer WHERE id_customer = @pID";
                cmd.Parameters.AddWithValue("pID", customerID);
                cmd.ExecuteNonQuery();


                cmd.Dispose();
                KoneksiSQL.tutup();
                LoadDataMember();
            }

            if (e.ColumnIndex == 6)
            {
                if (MessageBox.Show("Apakah kamu yakin ingin menggunakan Point?",
                    "Konfirmasi",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Information) == DialogResult.Cancel)
                {
                    return;
                }


                // Get the current point value from the DataGridView cell
                int currentPoint = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[3].Value);

                if(currentPoint < 10)
                {
                    if (MessageBox.Show("Point tidak Cukup !",
                    "Informasi",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Warning) == DialogResult.Cancel)
                    {
                        return;
                    }
                }
                else
                {
                    // Subtract 10 from the current point value
                    int newPoint = currentPoint - 10;
                    // Update the DataGridView cell with the new point value
                    dataGridView1.Rows[e.RowIndex].Cells[3].Value = newPoint;
                    // Update the database with the new point value
                    KoneksiSQL.buka();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = KoneksiSQL.sqlConn;
                    cmd.CommandText = "UPDATE customer SET jumlah_point = @newPoint WHERE id_customer = @pID";
                    cmd.Parameters.AddWithValue("newPoint", newPoint);
                    cmd.Parameters.AddWithValue("pID", dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    KoneksiSQL.tutup();

                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PelangganCreate frm = new PelangganCreate();
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.Text = "Tambah Member Baru";
            if(frm.ShowDialog() == DialogResult.OK)
            {
                LoadDataMember();
            }
        }
        private void Text_box_enter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadDataMember();
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
