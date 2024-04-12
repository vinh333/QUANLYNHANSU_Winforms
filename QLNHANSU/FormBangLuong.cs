using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using DevExpress.XtraEditors.Mask;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace QLNHANSU.CHAMCONG
{
    public partial class FormBangLuong : DevExpress.XtraEditors.XtraForm
    {
        private MySQLConnector mySQLConnector;
        public int makycong;
        public int _thang;
        public int _nam;

        public FormBangLuong()
        {
            InitializeComponent();
            mySQLConnector = new MySQLConnector();
        }

        public FormBangLuong(int makycong, int _thang, int _nam)
        {
            InitializeComponent();
            mySQLConnector = new MySQLConnector();
            this.makycong = makycong;
            this._thang = _thang;
            this._nam = _nam;
        }

        private void FormBangLuong_Load(object sender, EventArgs e)
        {
            cb_Thang.Text = _thang.ToString();
            cb_Nam.Text = _nam.ToString();
            LoadData();
        }

        private void LoadData()
        {
            int makycong = int.Parse(cb_Nam.Text + cb_Thang.Text);
            if (makycong == 0)
            {
                try
                {
                    string query = "SELECT MAKYCONG, MANV, HOTEN, CAST(NGAYCONGTRONGTHANG AS CHAR) AS NGAYCONGTRONGTHANG, CAST(NGAYPHEP AS CHAR) AS NGAYPHEP, CAST(KHONGPHEP AS CHAR) AS KHONGPHEP, CAST(THUCLANH AS CHAR) AS THUCLANH FROM bangluong ";
                    DataTable dataTable = mySQLConnector.Select(query);
                    gridControl1.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi load dữ liệu: " + ex.Message);
                }
            }
            else
            {
                try
                {
                    string query = "SELECT MAKYCONG, MANV, HOTEN, CAST(NGAYCONGTRONGTHANG AS CHAR) AS NGAYCONGTRONGTHANG, CAST(NGAYPHEP AS CHAR) AS NGAYPHEP, CAST(KHONGPHEP AS CHAR) AS KHONGPHEP, CAST(THUCLANH AS CHAR) AS THUCLANH FROM bangluong WHERE MAKYCONG = " + makycong;
                    DataTable dataTable = mySQLConnector.Select(query);
                    gridControl1.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi load dữ liệu: " + ex.Message);
                }
            }
            

        }

        public void tinhLuong(int makycong)
        {
            // Lấy dữ liệu từ bảng KYCONGCHITIET với điều kiện MAKYCONG = makycong
            DataTable dataTableKyCongChiTiet = mySQLConnector.Select($"SELECT MANV, HOTEN, NGAYCONG, NGAYPHEP, NGHIKHONGPHEP FROM kycongchitiet WHERE MAKYCONG = {makycong}");

            // Lấy dữ liệu từ bảng NHANVIEN
            DataTable dataTableNhanVien = mySQLConnector.Select("SELECT MANV, HOTEN, LUONGCOBAN FROM Nhanvien");

            // Kiểm tra xem cả hai bảng có dữ liệu không
            if (dataTableKyCongChiTiet == null || dataTableNhanVien == null)
            {
                MessageBox.Show("Không thể lấy dữ liệu từ cơ sở dữ liệu.");
                return;
            }

            // Duyệt qua từng dòng dữ liệu trong bảng KYCONGCHITIET
            foreach (DataRow rowKyCongChiTiet in dataTableKyCongChiTiet.Rows)
            {
                int manv = Convert.ToInt32(rowKyCongChiTiet["MANV"]);
                string hoten = rowKyCongChiTiet["HOTEN"].ToString();
                int ngayCong = Convert.ToInt32(rowKyCongChiTiet["NGAYCONG"]);
                int ngayPhep = Convert.ToInt32(rowKyCongChiTiet["NGAYPHEP"]);
                int nghiKhongPhep = Convert.ToInt32(rowKyCongChiTiet["NGHIKHONGPHEP"]);

                // Tìm dòng tương ứng trong bảng NHANVIEN
                DataRow[] matchingRows = dataTableNhanVien.Select($"MANV = {manv}");

                // Kiểm tra xem có dòng tương ứng không
                if (matchingRows.Length == 0)
                {
                    MessageBox.Show($"Không tìm thấy thông tin nhân viên có MANV = {manv} trong bảng NHANVIEN.");
                    continue;
                }

                // Lấy thông tin lương cơ bản từ dòng tương ứng trong bảng NHANVIEN
                int luongCoBan = Convert.ToInt32(matchingRows[0]["LUONGCOBAN"]);

                // Tính toán các khoản lương
                decimal ngayCongTrongThang = ngayCong * luongCoBan;
                decimal ngayPhepMoney = (ngayPhep * luongCoBan) * 0.3m;
                decimal khongPhepMoney = -(ngayPhep * luongCoBan) * 0.5m;
                decimal thuclanh = ngayPhepMoney + khongPhepMoney + ngayCongTrongThang;

                // Thêm dòng vào bảng BANGLUONG
                string insertQuery = $"INSERT INTO BANGLUONG (MAKYCONG, MANV, HOTEN, NGAYCONGTRONGTHANG, NGAYPHEP, KHONGPHEP, THUCLANH) " +
                                     $"VALUES ({makycong}, {manv}, '{hoten}', {ngayCongTrongThang}, {ngayPhepMoney}, {khongPhepMoney}, {thuclanh})";

                // Thực hiện truy vấn
                mySQLConnector.ExecuteQuery(insertQuery);
            }

            MessageBox.Show("Tính lương thành công và cập nhật vào bảng BANGLUONG.");
        }




        private void btnTinhLuong_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int makycong = int.Parse(cb_Nam.Text + cb_Thang.Text);

            tinhLuong(makycong);
            LoadData();
        }

        private void btnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadData();
        }

        private void btnPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // Điều chỉnh dữ liệu trước khi in (nếu cần)
        }

        private void btnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadData();
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {

        }
    }

   
}
