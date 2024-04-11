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
using DevExpress.XtraBars.Customization.Helpers;
using DevExpress.XtraEditors;
using System;
using System.Data;
using System.Windows.Forms;
using DevExpress.XtraGrid;
using QLNHANSU.CHAMCONG;

namespace QLNHANSU
{
    public partial class FormBangCong : DevExpress.XtraEditors.XtraForm
    {
        private MySQLConnector mySQLConnector;
        private bool checkbutton = false;
        public FormBangCong()
        {
            InitializeComponent();
            mySQLConnector = new MySQLConnector();
        }

        private void FormKyCong_Load(object sender, EventArgs e)
        {
            _showHide(true);
            gridView1.OptionsBehavior.Editable = false; // Chặn chỉnh sửa trực tiếp

            LoadData();
        }

        private void LoadData()
        {
            try
            {
                string query = "SELECT * FROM KYCONG";
                DataTable dataTable = mySQLConnector.Select(query);
                gridControl1.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load dữ liệu: " + ex.Message);
            }
        }

        private void _showHide(bool kt)
        {
            btnLuu.Enabled = !kt;
            btnHuy.Enabled = !kt;

            btnThem.Enabled = kt;
            btnSua.Enabled = kt;
            btnXoa.Enabled = kt;
            btnThoat.Enabled = kt;
            btnIn.Enabled = kt;

            cb_Thang.Enabled = !kt; // Enable combobox chọn tháng
            cb_Nam.Enabled = !kt; // Enable combobox chọn năm
            ckb_Khoa.Enabled = !kt; // Enable checkbox Khoa
            ckb_Trangthai.Enabled = !kt; // Enable checkbox Trangthai
        }

        private void btnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _showHide(false);
            checkbutton = true;
            ClearInputs();
        }

        private void btnSua_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowIndex = gridView1.FocusedRowHandle;
            if (rowIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn một dòng để sửa.");
                return;
            }

            DataRow row = gridView1.GetDataRow(rowIndex);
            int idKyCong = Convert.ToInt32(row["ID"]);
            checkbutton = false;
            _showHide(false);
        }

        private void btnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowIndex = gridView1.FocusedRowHandle;
            if (rowIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn một dòng để xóa.");
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa dòng này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DataRow row = gridView1.GetDataRow(rowIndex);
                int idKyCong = Convert.ToInt32(row["ID"]);
                string query = $"DELETE FROM KYCONG WHERE ID = {idKyCong}";
                mySQLConnector.ExecuteQuery(query);
                LoadData();
            }
        }

        private void btnLuu_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowIndex = gridView1.FocusedRowHandle;
                if (checkbutton)
                {
                    // Thêm mới
                    int thang = Convert.ToInt32(cb_Thang.SelectedItem); // Lấy tháng từ combobox
                    int nam = Convert.ToInt32(cb_Nam.SelectedItem); // Lấy năm từ combobox
                    bool khoa = ckb_Khoa.Checked ? true : false; // Lấy trạng thái khoá từ checkbox
                    bool trangthai = ckb_Trangthai.Checked ? true : false; // Lấy trạng thái từ checkbox
                    DateTime ngayTinhCong = DateTime.Now; // Ngày hiện tại khi thay đổi dữ liệu
                    float ngayCongTrongThang = CalculateNgayCongTrongThang(thang, nam); // Tính ngày công trong tháng
                    string maKyCong = $"{nam}{thang}"; // Tạo mã kỳ công
                    string query = $"INSERT INTO KYCONG (MAKYCONG, THANG, NAM, KHOA, NGAYTINHCONG, NGAYCONGTRONGTHANG, TRANGTHAI) " +
                                   $"VALUES ('{maKyCong}', {thang}, {nam}, {khoa}, '{ngayTinhCong.ToString("yyyy-MM-dd HH:mm:ss")}', {ngayCongTrongThang}, {trangthai})";
                    mySQLConnector.ExecuteQuery(query);
                }
                else
                {
                    // Sửa
                    DataRow row = gridView1.GetDataRow(rowIndex);
                    int idKyCong = Convert.ToInt32(row["ID"]);
                    int thang = Convert.ToInt32(cb_Thang.SelectedItem); // Lấy tháng từ combobox
                    int nam = Convert.ToInt32(cb_Nam.SelectedItem); // Lấy năm từ combobox
                    bool khoa = ckb_Khoa.Checked ? true : false; // Lấy trạng thái khoá từ checkbox
                    bool trangthai = ckb_Trangthai.Checked ? true : false; // Lấy trạng thái từ checkbox
                    DateTime ngayTinhCong = DateTime.Now; // Ngày hiện tại khi thay đổi dữ liệu
                    float ngayCongTrongThang = CalculateNgayCongTrongThang(thang, nam); // Tính ngày công trong tháng
                    string maKyCong = $"{nam}{thang}"; // Tạo mã kỳ công
                    string query = $"UPDATE KYCONG SET MAKYCONG = '{maKyCong}', THANG = {thang}, NAM = {nam}, KHOA = {khoa}, NGAYTINHCONG = '{ngayTinhCong.ToString("yyyy-MM-dd HH:mm:ss")}', " +
                                   $"NGAYCONGTRONGTHANG = {ngayCongTrongThang}, TRANGTHAI = {trangthai} WHERE ID = {idKyCong}";
                    mySQLConnector.ExecuteQuery(query);
                }

                LoadData();
                _showHide(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu dữ liệu: " + ex.Message);
            }
        }

        private float CalculateNgayCongTrongThang(int thang, int nam)
        {
            // Số ngày trong các tháng
            int[] daysInMonth = { 31, 28 + (nam % 4 == 0 && (nam % 100 != 0 || nam % 400 == 0) ? 1 : 0), 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

            // Tính tổng số ngày trong tháng
            int tongNgayTrongThang = daysInMonth[thang - 1];

            // Đếm số ngày chủ nhật trong tháng
            int soNgayChuNhat = 0;
            DateTime ngayDauThang = new DateTime(nam, thang, 1);
            for (int i = 0; i < tongNgayTrongThang; i++)
            {
                DateTime ngayHienTai = ngayDauThang.AddDays(i);
                if (ngayHienTai.DayOfWeek == DayOfWeek.Sunday)
                {
                    soNgayChuNhat++;
                }
            }

            // Số ngày công trong tháng là tổng số ngày trừ đi số ngày chủ nhật
            int ngayCongTrongThang = tongNgayTrongThang - soNgayChuNhat;

            return ngayCongTrongThang;
        }



        private void btnHuy_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _showHide(true);
        }

        private void btnThoat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void ClearInputs()
        {
            // Không cần xóa nội dung của textbox hay combobox, vì không có trường TENKC nữa
        }

        private void btnXemBangCong_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // Lấy dòng đang chọn từ gridView1
            int rowIndex = gridView1.FocusedRowHandle;
            if (rowIndex >= 0)
            {
                DataRow row = gridView1.GetDataRow(rowIndex);
                int makycong = Convert.ToInt32(row["MAKYCONG"]);
                int thang = Convert.ToInt32(row["THANG"]);
                int nam = Convert.ToInt32(row["NAM"]);

                // Khởi tạo form FormBangCongChiTiet và truyền giá trị makycong qua constructor
                FormBangCongChiTiet frm = new FormBangCongChiTiet(makycong,thang,nam);

                // Hiển thị form
                frm.ShowDialog();
            }
            else
            {
                // Hiển thị thông báo nếu không có dòng nào được chọn
                MessageBox.Show("Vui lòng chọn một dòng để xem chi tiết.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


    }
}
