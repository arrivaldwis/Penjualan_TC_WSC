using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Penjualan
{
    public partial class frmMain : Form
    {
        private db_penjualanEntities data = new db_penjualanEntities();
        public frmMain()
        {
            InitializeComponent();
            loadData();
        }

        private void loadData()
        {
            var transaksi = data.Transaksis.Select(x => new
            {
                NoFaktur = x.no_faktur,
                Customer = x.Customer.name,
                TglPenjualan = x.tgl_penjualan,
                TglDeadline = x.tgl_deadline,
                TotalBiaya = x.total_biaya,
                Bunga = x.bunga + "%"
            }).ToList();
            dataGridView1.DataSource = transaksi;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string no_faktur = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            getPembayaranData(no_faktur);
        }

        private void getPembayaranData(string no_faktur)
        {
            int total_bayar = 0;
            int no = int.Parse(no_faktur);
            var pembayaran = data.Pembayarans.Where(x => x.no_faktur == no);

            foreach(var a in pembayaran)
            {
                label6.Text = a.no_faktur.ToString();
                total_bayar += a.jumlah_pembayaran;
            }

            dataGridView2.DataSource = pembayaran.Select(x => new
            {
                NoFaktur = x.no_faktur,
                Customer = x.Customer.name,
                JumlahBayar = x.jumlah_pembayaran,
                TglPembayaran = x.tgl_pembayaran
            }).ToList();
            
            if(DateTime.Now.Date < pembayaran.First().Transaksi.tgl_deadline.Date)
            {
                label5.Text = "0";
            } else
            {
                int day = (DateTime.Now.Date - pembayaran.First().Transaksi.tgl_deadline.Date).Days;
                float calc = (((float)pembayaran.First().Transaksi.bunga / 100) * pembayaran.First().Transaksi.total_biaya) * day;
                label9.Text = day.ToString();
                label5.Text = calc.ToString();
            }

            label3.Text = (pembayaran.First().Transaksi.total_biaya - total_bayar).ToString();
        }
    }
}
