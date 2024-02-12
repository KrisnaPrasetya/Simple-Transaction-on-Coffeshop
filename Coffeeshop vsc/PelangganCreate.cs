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

namespace Pembayaran_di_CoffeeShop
{
    public partial class PelangganCreate : Form
    {
        public int id_member_edit = 0;

        public PelangganCreate()
        {
            InitializeComponent();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            KoneksiSQL.buka();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = KoneksiSQL.sqlConn;

            if (id_member_edit == 0 )
            {
                cmd.CommandText = "INSERT INTO customer (nama_customer, nomor_telepon, jumlah_point) " 
                    + "VALUES (@pNamaCustomer, @pNomorTelephone, 0) ";
            }
            else
            {
                cmd.CommandText = "UPDATE customer SET "
                    + "nama_customer = @pNamaCustomer, "
                    + "nomor_telepon = @pNomorTelephone "
                    + "WHERE id_customer = @pID";
                cmd.Parameters.AddWithValue("pID", id_member_edit);
            }

            cmd.Parameters.AddWithValue("pNamaCustomer", txtNama.Text);
            cmd.Parameters.AddWithValue("pNomorTelephone", txtNoTelephone.Text);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            KoneksiSQL.tutup();

        }

        private void PelangganCreate_Load(object sender, EventArgs e)
        {

        }
    }
}
