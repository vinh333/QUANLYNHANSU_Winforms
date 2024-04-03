using DevExpress.XtraBars.Customization.Helpers;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using static Mysqlx.Datatypes.Scalar.Types;

namespace QLNHANSU
{
    public partial class FormNhanVien : DevExpress.XtraEditors.XtraForm
    {
        private MySQLConnector mySQLConnector;
        private bool checkbutton = false;

        public FormNhanVien()
        {
            InitializeComponent();
            mySQLConnector = new MySQLConnector();
        }

        private void FormNhanVien_Load(object sender, EventArgs e)
        {
            _showHide(true);
            gridView1.OptionsBehavior.Editable = false; // Chặn chỉnh sửa trực tiếp

            LoadData();
            LoadComboBoxData(); // Load dữ liệu cho các ComboBox
        }

        private void LoadComboBoxData()
        {
            // Load data for cbo_BoPhan, cbo_ChucVu, cbo_DanToc, cbo_PhongBan, cbo_TonGiao, cbo_TrinhDo
            try
            {
                string query;
                // Load data for cbo_BoPhan
                query = "SELECT TENBP FROM bophan"; // Chỉnh sửa query tương ứng
                DataTable dtBoPhan = mySQLConnector.Select(query);
                foreach (DataRow row in dtBoPhan.Rows)
                {
                    cbo_BoPhan.Items.Add(row["TENBP"].ToString());
                }

                // Load data for cbo_ChucVu
                query = "SELECT TENCV FROM chucvu"; // Chỉnh sửa query tương ứng
                DataTable dtChucVu = mySQLConnector.Select(query);
                foreach (DataRow row in dtChucVu.Rows)
                {
                    cbo_ChucVu.Items.Add(row["TENCV"].ToString());
                }

                // Load data for cbo_DanToc
                query = "SELECT TENDANTOC FROM dantoc"; // Chỉnh sửa query tương ứng
                DataTable dtDanToc = mySQLConnector.Select(query);
                foreach (DataRow row in dtDanToc.Rows)
                {
                    cbo_DanToc.Items.Add(row["TENDANTOC"].ToString());
                }

                // Load data for cbo_PhongBan
                query = "SELECT TENPB FROM phongban"; // Chỉnh sửa query tương ứng
                DataTable dtPhongBan = mySQLConnector.Select(query);
                foreach (DataRow row in dtPhongBan.Rows)
                {
                    cbo_PhongBan.Items.Add(row["TENPB"].ToString());
                }

                // Load data for cbo_TonGiao
                query = "SELECT TENTONGIAO FROM tongiao"; // Chỉnh sửa query tương ứng
                DataTable dtTonGiao = mySQLConnector.Select(query);
                foreach (DataRow row in dtTonGiao.Rows)
                {
                    cbo_TonGiao.Items.Add(row["TENTONGIAO"].ToString());
                }

                // Load data for cbo_TrinhDo
                query = "SELECT TENTD FROM trinhdo"; // Chỉnh sửa query tương ứng
                DataTable dtTrinhDo = mySQLConnector.Select(query);
                foreach (DataRow row in dtTrinhDo.Rows)
                {
                    cbo_TrinhDo.Items.Add(row["TENTD"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load dữ liệu ComboBox: " + ex.Message);
            }
        }

        private void LoadData()
        {
            try
            {
                string query = "SELECT * FROM nhanvien";
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
            splitContainer1.Panel1Collapsed = kt;

            btnLuu.Enabled = !kt;
            btnHuy.Enabled = !kt;

            btnThem.Enabled = kt;
            btnSua.Enabled = kt;
            btnXoa.Enabled = kt;
            btnThoat.Enabled = kt;
            btnIn.Enabled = kt;

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
            int maNV = Convert.ToInt32(row["MANV"]);
            string hoTen = row["HOTEN"].ToString();
            string gioiTinh = row["GIOITINH"].ToString();
            DateTime ngaySinh = Convert.ToDateTime(row["NGAYSINH"]);
            string sdt = row["DIENTHOAI"].ToString();
            string cccd = row["CCCD"].ToString();
            string diaChi = row["DIACHI"].ToString();
            byte[] hinhAnh = (byte[])row["HINHANH"]; // Đọc hình ảnh từ cột HINHANH

            txtHoTen.Text = hoTen;
            chkGioiTinh.Checked = gioiTinh == "True";
            dtpNgaySinh.Value = ngaySinh;
            txt_SoDienThoai.Text = sdt;
            txtCCCD.Text = cccd;
            txt_DiaChi.Text = diaChi;
            // Hiển thị hình ảnh lên pictureBox
            if (hinhAnh != null)
            {
                picHinhAnh.Image = Image.FromStream(new MemoryStream(hinhAnh));
            }

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
                int maNV = Convert.ToInt32(row["MANV"]);
                string query = $"DELETE FROM nhanvien WHERE MANV = {maNV}";
                mySQLConnector.ExecuteQuery(query);
                LoadData();
            }
        }

        private void btnLuu_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowIndex = gridView1.FocusedRowHandle;
                string hoTen = txtHoTen.Text.Trim();
                string gioiTinh = chkGioiTinh.Checked ? "1" : "0";
                DateTime ngaySinh = dtpNgaySinh.Value;
                string sdt = txt_SoDienThoai.Text.Trim();
                string cccd = txtCCCD.Text.Trim();
                string diaChi = txt_DiaChi.Text.Trim();
                // byte[] hinhAnh = ImageToByte(picHinhAnh.Image);
                string hinhAnh = ImageToHexString(picHinhAnh.Image, picHinhAnh.Image.RawFormat);

                int idBoPhan = GetIDByTen(cbo_BoPhan.SelectedItem.ToString(), "bophan", "TENBP", "IDBP");
                int idChucVu = GetIDByTen(cbo_ChucVu.SelectedItem.ToString(), "chucvu", "TENCV", "IDCV");
                int idDanToc = GetIDByTen(cbo_DanToc.SelectedItem.ToString(), "dantoc", "TENDANTOC", "IDDT");
                int idPhongBan = GetIDByTen(cbo_PhongBan.SelectedItem.ToString(), "phongban", "TENPB", "IDPB");
                int idTonGiao = GetIDByTen(cbo_TonGiao.SelectedItem.ToString(), "tongiao", "TENTONGIAO", "IDTG");
                int idTrinhDo = GetIDByTen(cbo_TrinhDo.SelectedItem.ToString(), "trinhdo", "TENTD", "IDTD");

                if (checkbutton)
                {
                    // Thêm mới
                    string query = $"INSERT INTO nhanvien (HOTEN, GIOITINH, NGAYSINH, DIENTHOAI, CCCD, DIACHI, HINHANH, IDBP, IDCV, IDDT, IDPB, IDTG, IDTD) " +
                                   $"VALUES ('{hoTen}', {gioiTinh}, '{ngaySinh.ToString("yyyy-MM-dd")}', '{sdt}', '{cccd}', '{diaChi}', {hinhAnh}, " +
                                   $"{idBoPhan}, {idChucVu}, {idDanToc}, {idPhongBan}, {idTonGiao}, {idTrinhDo})";
                    mySQLConnector.ExecuteQuery(query);
                }
                else
                {
                    // Sửa
                    DataRow row = gridView1.GetDataRow(rowIndex);
                    int maNV = Convert.ToInt32(row["MANV"]);
                    string query = $"UPDATE nhanvien SET HOTEN = '{hoTen}', GIOITINH = {gioiTinh}, NGAYSINH = '{ngaySinh.ToString("yyyy-MM-dd")}', " +
                                   $"DIENTHOAI = '{sdt}', CCCD = '{cccd}', DIACHI = '{diaChi}', HINHANH = '{hinhAnh}', IDBP = {idBoPhan}, IDCV = {idChucVu}, " +
                                   $"IDDT = {idDanToc}, IDPB = {idPhongBan}, IDTG = {idTonGiao}, IDTD = {idTrinhDo} WHERE MANV = {maNV}";
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
            txtHoTen.Text = "";
            chkGioiTinh.Checked = false;
            dtpNgaySinh.Value = DateTime.Now;
            txt_SoDienThoai.Text = "";
            txtCCCD.Text = "";
            txt_DiaChi.Text = "";
            picHinhAnh.Image = Properties.Resources.No_Image_Available;
        }

        public string ImageToHexString(Image image, System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Lưu hình ảnh vào MemoryStream
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();
                // Chuyển đổi sang chuỗi hex
                string hexString = BitConverter.ToString(imageBytes).Replace("-", "");
                return "0x" + hexString.ToLower();
            }
        }


        public Image Base64ToImage(string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);
            // Tạo MemoryStream từ dữ liệu base64
            using (MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                // Đọc hình ảnh từ MemoryStream
                Image image = Image.FromStream(ms, true);
                return image;
            }
        }



        private void picHinhAnh_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Picture file (.png, .jpg) | *.png; *.jpg";
            openFile.Title = "Chọn hình ảnh";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                picHinhAnh.Image = Image.FromFile(openFile.FileName);
                picHinhAnh.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        private int GetIDByTen(string ten, string tableName, string columnName, string idName)
        {
            int id = -1; // Giá trị mặc định nếu không tìm thấy ID

            try
            {
                string query = $"SELECT {idName} FROM {tableName} WHERE {columnName} = '{ten}'";
                object result = mySQLConnector.ExecuteScalar(query);
                if (result != null && result != DBNull.Value)
                {
                    id = Convert.ToInt32(result);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lấy ID từ bảng {tableName}: {ex.Message}");
            }

            return id;
        }




    }
}
