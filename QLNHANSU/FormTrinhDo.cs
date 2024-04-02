using DevExpress.XtraBars.Customization.Helpers;
using DevExpress.XtraEditors;
using System;
using System.Data;
using System.Windows.Forms;

namespace QLNHANSU
{
    public partial class FormTrinhDo : DevExpress.XtraEditors.XtraForm
    {
        private MySQLConnector mySQLConnector;
        private bool checkbutton = false;

        public FormTrinhDo()
        {
            InitializeComponent();
            mySQLConnector = new MySQLConnector();
        }

        private void FormTrinhDo_Load(object sender, EventArgs e)
        {
            _showHide(true);
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                string query = "SELECT * FROM TRINHDO"; // Thay đổi từ TONGIAO thành TRINHDO
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

            txtTenTrinhDo.Enabled = !kt; // Đổi tên control txtTenTonGiao thành txtTenTrinhDo
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
            int idTrinhDo = Convert.ToInt32(row["IDTD"]);
            string tenTrinhDo = row["TENTD"].ToString(); // Thay đổi từ TENTONGIAO thành TENTRINHDO
            txtTenTrinhDo.Text = tenTrinhDo; // Đổi tên control txtTenTonGiao thành txtTenTrinhDo
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
                int idTrinhDo = Convert.ToInt32(row["IDTD"]);
                string query = $"DELETE FROM TRINHDO WHERE IDTD = {idTrinhDo}"; // Thay đổi từ TONGIAO thành TRINHDO
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
                    string tenTrinhDo = txtTenTrinhDo.Text.Trim(); // Đổi tên biến tenTonGiao thành tenTrinhDo
                    string query = $"INSERT INTO TRINHDO (TENTD) VALUES ('{tenTrinhDo}')"; // Thay đổi từ TONGIAO thành TRINHDO
                    mySQLConnector.ExecuteQuery(query);
                }
                else
                {
                    // Sửa
                    DataRow row = gridView1.GetDataRow(rowIndex);
                    int idTrinhDo = Convert.ToInt32(row["IDTD"]);
                    string tenTrinhDo = txtTenTrinhDo.Text.Trim(); // Đổi tên biến tenTonGiao thành tenTrinhDo
                    string query = $"UPDATE TRINHDO SET TENTD = '{tenTrinhDo}' WHERE IDTD = {idTrinhDo}"; // Thay đổi từ TONGIAO thành TRINHDO
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
            txtTenTrinhDo.Text = ""; // Đổi tên control txtTenTonGiao thành txtTenTrinhDo
        }
    }
}
