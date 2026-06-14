using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;  
namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        OleDbConnection baglan = new OleDbConnection
        ("Provider=Microsoft.Ace.Oledb.12.0; Data Source=bilgiler3.accdb");
        void yukle()
        {
            DataTable tablo = new DataTable();
            OleDbDataAdapter veriler = new OleDbDataAdapter("SELECT ADI,SOYADI,MEMLEKET,GELIR,TARIH FROM Tablo1 ", baglan);
            veriler.Fill(tablo);
            dataGridView1.DataSource = tablo;
            dataGridView1.ClearSelection();  
 
        }
        void hucre_kontrolu()
        {
            foreach (DataGridViewRow satır in dataGridView1.Rows)
            {
                foreach (DataGridViewCell hucre in satır.Cells)
                {
                    if (hucre.Value == null || string.IsNullOrWhiteSpace(hucre.Value.ToString()))
                    { hucre.Style.BackColor = Color.GreenYellow;
                    hucre.ToolTipText = "BURADA DEĞER YOKTUR!";
                    }
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            yukle();
            hesapla();
            hucre_kontrolu();
        }
        void hesapla()
        {

            int toplam = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(dataGridView1.Rows[i].Cells["Column4"].Value)))
                { toplam += Convert.ToInt32 (dataGridView1.Rows[i].Cells["Column4"].Value );  }
            }
            label1.Text = "Gelirler Toplamı : " +toplam.ToString("C2");  
        }
        private void button1_Click(object sender, EventArgs e)
        {
            baglan.Open();
            DateTime baslangic = dateTimePicker1.Value.Date;
            DateTime bitis = dateTimePicker2.Value.Date;
            string sorgu = "SELECT ADI,SOYADI,MEMLEKET,GELIR,TARIH FROM TABLO1 WHERE TARIH BETWEEN @baslangic AND @bitis";
            OleDbCommand cmd = new OleDbCommand(sorgu, baglan);
            cmd.Parameters.AddWithValue("@baslangic", baslangic);
            cmd.Parameters.AddWithValue("@bitis", bitis);
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            baglan.Close();
            hesapla();
            hucre_kontrolu();
        }
    }
}
