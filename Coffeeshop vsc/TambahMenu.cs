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
    public partial class TambahMenu : Form
    {
        public int id_menu_edit = 0;
        public TambahMenu()
        {
            InitializeComponent();
        }

        private void TambahMenu_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            KoneksiSQL.buka();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = KoneksiSQL.sqlConn;

            if (id_menu_edit == 0)
            {
                cmd.CommandText = "INSERT INTO menu (nama_menu, harga) "
                    + "VALUES (@pMenu, @pHarga) ";
            }
            else
            {
                cmd.CommandText = "UPDATE menu SET "
                    + "nama_menu = @pMenu, "
                    + "harga = @pHarga "
                    + "WHERE id_menu = @pID";
                cmd.Parameters.AddWithValue("pID", id_menu_edit);
            }

            cmd.Parameters.AddWithValue("pMenu", TextNamaMenu.Text);
            cmd.Parameters.AddWithValue("pHarga", textHarga.Text);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            KoneksiSQL.tutup();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
