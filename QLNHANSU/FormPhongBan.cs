using DevExpress.XtraBars.Customization.Helpers;
using DevExpress.XtraEditors;
using System;
using System.Data;
using System.Windows.Forms;

namespace QLNHANSU
{
    public partial class FormPhongBan : DevExpress.XtraEditors.XtraForm
    {
        private MySQLConnector mySQLConnector;
        private bool checkbutton = false;

        public FormPhongBan()
        {
            InitializeComponent();
            mySQLConnector = new MySQLConnector();
        }

        private void FormPhongBan_Load(object sender, EventArgs e)
        {
            _showHide(true);
            gridView1.OptionsBehavior.Editable = false; // Chặn chỉnh sửa trực tiếp

            LoadData();
        }

        private void LoadData()
        {
            try
            {
                string query = "SELECT * FROM PHONGBAN"; // Thay đổi từ DANTOC thành PHONGBAN
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

            txtTenPhongBan.Enabled = !kt; // Đổi tên control txtTenDanToc thành txtTenPhongBan
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
            int idPhongBan = Convert.ToInt32(row["IDPB"]);
            string tenPhongBan = row["TENPB"].ToString(); // Thay đổi từ TENDANTOC thành TENPHONGBAN
            txtTenPhongBan.Text = tenPhongBan; // Đổi tên control txtTenDanToc thành txtTenPhongBan
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
                int idPhongBan = Convert.ToInt32(row["IDPB"]);
                string query = $"DELETE FROM PHONGBAN WHERE IDPB = {idPhongBan}"; // Thay đổi từ DANTOC thành PHONGBAN
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
                    string tenPhongBan = txtTenPhongBan.Text.Trim(); // Đổi tên biến tenDanToc thành tenPhongBan
                    string query = $"INSERT INTO PHONGBAN (TENPB) VALUES ('{tenPhongBan}')"; // Thay đổi từ DANTOC thành PHONGBAN
                    mySQLConnector.ExecuteQuery(query);
                }
                else
                {
                    // Sửa
                    DataRow row = gridView1.GetDataRow(rowIndex);
                    int idPhongBan = Convert.ToInt32(row["IDPB"]);
                    string tenPhongBan = txtTenPhongBan.Text.Trim(); // Đổi tên biến tenDanToc thành tenPhongBan
                    string query = $"UPDATE PHONGBAN SET TENPB = '{tenPhongBan}' WHERE IDPB = {idPhongBan}"; // Thay đổi từ DANTOC thành PHONGBAN
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
            txtTenPhongBan.Text = ""; // Đổi tên control txtTenDanToc thành txtTenPhongBan
        }
    }
}
