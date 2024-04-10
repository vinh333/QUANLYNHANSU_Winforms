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

using DevExpress.XtraEditors;
using System;
using System.Data;
using System.Windows.Forms;

namespace QLNHANSU
{
    public partial class FormLoaiCong : DevExpress.XtraEditors.XtraForm
    {
        private MySQLConnector mySQLConnector;
        private bool checkbutton = false;

        public FormLoaiCong()
        {
            InitializeComponent();
            mySQLConnector = new MySQLConnector();
        }

        private void FormLoaiCong_Load(object sender, EventArgs e)
        {
            _showHide(true);
            gridView1.OptionsBehavior.Editable = false; // Vô hiệu hóa chỉnh sửa trực tiếp
            speHeSo.Properties.Increment = 0.5m; // Đặt bước tăng của SpinEdit thành 0.5

            LoadData();
        }

        private void LoadData()
        {
            try
            {
                string query = "SELECT * FROM LOAICONG";
                DataTable dataTable = mySQLConnector.Select(query);
                gridControl1.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message);
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

            txtTenLoaiCong.Enabled = !kt;
            speHeSo.Enabled = !kt; // Đảm bảo kích hoạt SpinEdit
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
            int idLoaiCong = Convert.ToInt32(row["IDLC"]);
            string tenLoaiCong = row["TENLC"].ToString();
            decimal heSo = Convert.ToDecimal(row["HESO"]); // Lấy giá trị của HESO từ dòng được chọn

            txtTenLoaiCong.Text = tenLoaiCong;
            speHeSo.EditValue = heSo; // Đặt giá trị cho SpinEdit
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
                int idLoaiCong = Convert.ToInt32(row["IDLC"]);
                string query = $"DELETE FROM LOAICONG WHERE IDLC = {idLoaiCong}";
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
                    string tenLoaiCong = txtTenLoaiCong.Text.Trim();
                    decimal heSo = Convert.ToDecimal(speHeSo.EditValue); // Lấy giá trị từ SpinEdit

                    string query = $"INSERT INTO LOAICONG (TENLC, HESO) VALUES ('{tenLoaiCong}', {heSo})";
                    mySQLConnector.ExecuteQuery(query);
                }
                else
                {
                    // Sửa
                    DataRow row = gridView1.GetDataRow(rowIndex);
                    int idLoaiCong = Convert.ToInt32(row["IDLC"]);
                    string tenLoaiCong = txtTenLoaiCong.Text.Trim();
                    decimal heSo = Convert.ToDecimal(speHeSo.EditValue); // Lấy giá trị từ SpinEdit

                    string query = $"UPDATE LOAICONG SET TENLC = '{tenLoaiCong}', HESO = {heSo} WHERE IDLC = {idLoaiCong}";
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
            txtTenLoaiCong.Text = "";
            speHeSo.EditValue = 0; // Đặt giá trị mặc định cho SpinEdit
        }
    }
}
