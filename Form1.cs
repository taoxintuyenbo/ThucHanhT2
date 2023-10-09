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
namespace KTCSharp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string ckn = @"Data Source=LAPTOP-5Q8BC9UT\SQLEXPRESS;Initial Catalog=KTCSharp;Integrated Security=True";
        string sql;
        SqlConnection kn;
        SqlCommand th;
        SqlDataReader ddl;
        int i = 0;
        private void Form1_Load(object sender, EventArgs e)
        {
            kn = new SqlConnection(ckn);
            hienthi();
        }

        public void hienthi()
        {
            listView1.Items.Clear();
            kn.Open();
            sql = @"select MaNV,TenNV,Tuoi,GioiTinh from QLNV";
            th = new SqlCommand(sql, kn);
            ddl = th.ExecuteReader();
            i = 0;
            while(ddl.Read())
            {
                listView1.Items.Add(ddl[0].ToString());
                listView1.Items[i].SubItems.Add(ddl[1].ToString());
                listView1.Items[i].SubItems.Add(ddl[2].ToString());
                listView1.Items[i].SubItems.Add(ddl[3].ToString());
                i++;
            }
            kn.Close();

        }

        private bool kiemTraDuLieu()
        {
            if (string.IsNullOrWhiteSpace(txtMaNV.Text))
            {
                MessageBox.Show("Ma nhan vien khong the bo trong.", "Loi!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!txtMaNV.Text.Any(char.IsDigit))
            {
                MessageBox.Show("Ma nhan vien phai la mot so.", "Loi!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrEmpty(txtTenNV.Text))
            {
                MessageBox.Show("Ten nhan vien khong the bo trong.", "Loi!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
           
            if(txtTenNV.Text.Any(char.IsDigit))
            {
                MessageBox.Show("Ten nhan vien khong the co so.", "Loi!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!int.TryParse(txtTuoi.Text, out int tuoi) || tuoi <= 0)
            {
                MessageBox.Show("Tuoi khong hop le. Hay nhap tuoi la mot so nguyen duong.", "Loi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            string gioiTinh = txtGioiTinh.Text.Trim().ToLower();
            if (gioiTinh != "nam" && gioiTinh != "nu" && gioiTinh != "Nu" && gioiTinh != "Nam")
            {
                MessageBox.Show("Gioi tinh khong hop le. Nhap gioi tinh la \"Nam\" hoac \"Nu\".", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private bool MaNVDuyNhat(string MaNV)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                if (MaNV == item.SubItems[0].Text)
                {
                    MessageBox.Show("Ma nhan vien da ton tai.", "Loi!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            return true;
        }

        private void UpAnh(string MaNV)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                if (MaNV == item.SubItems[0].Text)
                {
                    pbImage.SizeMode = PictureBoxSizeMode.StretchImage;
                    string imagePath = @"d:\taoxintuyenbo\anh\"+MaNV+".jpg" ;
                    pbImage.ImageLocation = imagePath;
                }

            }
        }

        private void listView1_Click(object sender, EventArgs e)
        {
            txtMaNV.Text = listView1.SelectedItems[0].SubItems[0].Text;
            txtTenNV.Text = listView1.SelectedItems[0].SubItems[1].Text;
            txtTuoi.Text = listView1.SelectedItems[0].SubItems[2].Text;
            txtGioiTinh.Text = listView1.SelectedItems[0].SubItems[3].Text;
            UpAnh(txtMaNV.Text);
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if(kiemTraDuLieu()&& MaNVDuyNhat(txtMaNV.Text))
            {
                listView1.Items.Clear();
                kn.Open();
                sql = @"INSERT INTO QLNV(MaNV, TenNV, Tuoi, GioiTinh)
            VALUES(N'" + txtMaNV.Text + @"',
                    N'" + txtTenNV.Text + @"',
                    N'" + txtTuoi.Text + @"', 
                    N'" + txtGioiTinh.Text + @"')";
                th = new SqlCommand(sql, kn);
                th.ExecuteNonQuery();
                kn.Close();
                hienthi();
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
           if(kiemTraDuLieu())
            {
                listView1.Items.Clear();
                kn.Open();
                sql = @"UPDATE QLNV
                    SET TenNV = N'" + txtTenNV.Text + @"',
                        Tuoi = N'" + txtTuoi.Text + @"',
                        GioiTinh = N'" + txtGioiTinh.Text + @"'
                    WHERE (MaNV = N'" + txtMaNV.Text + @"')";
                th = new SqlCommand(sql, kn);
                th.ExecuteNonQuery();
                kn.Close();
                hienthi();
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            kn.Open();
            sql = @"Delete FROM QLNV Where (MaNV=N'"+txtMaNV.Text+@"')";
            th = new SqlCommand(sql, kn);
            th.ExecuteNonQuery();
            kn.Close();
            hienthi();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult f = MessageBox.Show("Ban co muon thoat khong?", "Thong bao", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if(f == DialogResult.Yes)
            {
                this.Close();
                Application.Exit();
            }
        }

        private void btnFile_Click(object sender, EventArgs e)
        {
            pbImage.SizeMode = PictureBoxSizeMode.StretchImage;
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Open Image";
            dlg.Filter = "JPEG files (*.jpg)|*.jpg";
            if(dlg.ShowDialog()==DialogResult.OK)
            {
                pbImage.ImageLocation = dlg.FileName;
            }
        }
    }
}
