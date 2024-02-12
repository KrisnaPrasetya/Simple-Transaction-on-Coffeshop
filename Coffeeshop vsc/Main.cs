using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pembayaran_di_CoffeeShop
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }
       

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void daftarMemberToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Pelanggan pelanggan = new Pelanggan();
            pelanggan.WindowState = FormWindowState.Maximized;
            pelanggan.MdiParent = this;
            pelanggan.Show();
            
        }

        private void transaksiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Transaksi transaksi = new Transaksi();
            transaksi.WindowState = FormWindowState.Maximized;
            transaksi.MdiParent = this;
            transaksi.Show();
        }

        private void menuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Menu menu = new Menu();
            menu.WindowState = FormWindowState.Maximized;
            menu.MdiParent = this;
            menu.Show();
        }
        private void historiTransaksiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HistoryTransaksi histori = new HistoryTransaksi();
            histori.WindowState = FormWindowState.Maximized;
            histori.MdiParent = this;
            histori.Show();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

       
    }
}
