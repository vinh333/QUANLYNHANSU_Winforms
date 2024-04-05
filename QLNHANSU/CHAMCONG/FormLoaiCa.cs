using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLNHANSU
{
    public partial class FormLoaiCa : DevExpress.XtraEditors.XtraForm
    {
        private MySQLConnector mySQLConnector;
        private bool checkbutton = false;
        public FormLoaiCa()
        {
            InitializeComponent();
            mySQLConnector = new MySQLConnector();
        }

        private void FormLoaiCa_Load(object sender, EventArgs e)
        {
            _showHide(true);
            gridView1.OptionsBehavior.Editable = false; // Chặn chỉnh sửa trực tiếp
            speHeSo.Properties.Increment = 0.5m; // Thiết lập bước tăng của SpinEdit thành 0.5

            LoadData();
        }

        private void LoadData()
        {
            try
            {
                string query = "SELECT * FROM LOAICA";
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

            txtTenLoaiCa.Enabled = !kt;
            speHeSo.Enabled = !kt; // Đảm bảo sự kích hoạt của SpinEdit
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
            int idLoaiCa = Convert.ToInt32(row["IDLOAICA"]);
            string tenLoaiCa = row["TENLOAICA"].ToString();
            decimal heSo = Convert.ToDecimal(row["HESO"]); // Lấy giá trị của HESO từ dòng được chọn

            txtTenLoaiCa.Text = tenLoaiCa;
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
                int idLoaiCa = Convert.ToInt32(row["IDLOAICA"]);
                string query = $"DELETE FROM LOAICA WHERE IDLOAICA = {idLoaiCa}";
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
                    string tenLoaiCa = txtTenLoaiCa.Text.Trim();
                    decimal heSo = Convert.ToDecimal(speHeSo.EditValue); // Lấy giá trị từ SpinEdit

                    string query = $"INSERT INTO LOAICA (TENLOAICA, HESO) VALUES ('{tenLoaiCa}', {heSo})";
                    mySQLConnector.ExecuteQuery(query);
                }
                else
                {
                    // Sửa
                    DataRow row = gridView1.GetDataRow(rowIndex);
                    int idLoaiCa = Convert.ToInt32(row["IDLOAICA"]);
                    string tenLoaiCa = txtTenLoaiCa.Text.Trim();
                    decimal heSo = Convert.ToDecimal(speHeSo.EditValue); // Lấy giá trị từ SpinEdit

                    string query = $"UPDATE LOAICA SET TENLOAICA = '{tenLoaiCa}', HESO = {heSo} WHERE IDLOAICA = {idLoaiCa}";
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
            txtTenLoaiCa.Text = "";
            speHeSo.EditValue = 0; // Đặt giá trị mặc định cho SpinEdit
        }
    }
}
