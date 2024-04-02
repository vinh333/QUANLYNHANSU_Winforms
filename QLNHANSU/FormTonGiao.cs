using DevExpress.XtraBars.Customization.Helpers;
using DevExpress.XtraEditors;
using System;
using System.Data;
using System.Windows.Forms;

namespace QLNHANSU
{
    public partial class FormTonGiao : DevExpress.XtraEditors.XtraForm
    {
        private MySQLConnector mySQLConnector;
        private bool checkbutton = false;

        public FormTonGiao()
        {
            InitializeComponent();
            mySQLConnector = new MySQLConnector();
        }

        private void FormTonGiao_Load(object sender, EventArgs e)
        {
            _showHide(true);
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                string query = "SELECT * FROM TONGIAO"; // Thay đổi từ DANTOC thành TONGIAO
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

            txtTenTonGiao.Enabled = !kt; // Đổi tên control txtTenDanToc thành txtTenTonGiao
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
            int idTonGiao = Convert.ToInt32(row["IDTG"]);
            string tenTonGiao = row["TENTONGIAO"].ToString(); // Thay đổi từ TENDANTOC thành TENTONGIAO
            txtTenTonGiao.Text = tenTonGiao; // Đổi tên control txtTenDanToc thành txtTenTonGiao
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
                int idTonGiao = Convert.ToInt32(row["IDTG"]);
                string query = $"DELETE FROM TONGIAO WHERE IDTG = {idTonGiao}"; // Thay đổi từ DANTOC thành TONGIAO
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
                    string tenTonGiao = txtTenTonGiao.Text.Trim(); // Đổi tên biến tenDanToc thành tenTonGiao
                    string query = $"INSERT INTO TONGIAO (TENTONGIAO) VALUES ('{tenTonGiao}')"; // Thay đổi từ DANTOC thành TONGIAO
                    mySQLConnector.ExecuteQuery(query);
                }
                else
                {
                    // Sửa
                    DataRow row = gridView1.GetDataRow(rowIndex);
                    int idTonGiao = Convert.ToInt32(row["IDTG"]);
                    string tenTonGiao = txtTenTonGiao.Text.Trim(); // Đổi tên biến tenDanToc thành tenTonGiao
                    string query = $"UPDATE TONGIAO SET TENTONGIAO = '{tenTonGiao}' WHERE IDTG = {idTonGiao}"; // Thay đổi từ DANTOC thành TONGIAO
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
            txtTenTonGiao.Text = ""; // Đổi tên control txtTenDanToc thành txtTenTonGiao
        }
    }
}
