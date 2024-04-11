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
    public partial class FormBangCongChiTiet : DevExpress.XtraEditors.XtraForm
    {
        private MySQLConnector mySQLConnector;
        public int makycong;
        public int _thang;
        public int _nam;

        public FormBangCongChiTiet()
        {
            InitializeComponent();
            mySQLConnector = new MySQLConnector();

          
        }
        // Thêm một constructor chấp nhận một đối số để truyền giá trị makycong
        public FormBangCongChiTiet(int makycong, int _thang, int _nam)
        {
            InitializeComponent();
            mySQLConnector = new MySQLConnector();

            this.makycong = makycong;
            this._thang = _thang;
            this._nam = _nam;
        }
        private void FormKyCong_Load(object sender, EventArgs e)
        {
            //gridView1.OptionsBehavior.Editable = false; // Chặn chỉnh sửa trực tiếp
            cb_Thang.Text = _thang.ToString();
            cb_Nam.Text = _nam.ToString();
            gridView1.CellValueChanged += gridView1_CellValueChanged;
            LoadData();
        }

        private void LoadData()
        {
            //CustomView(_thang, _nam);
            int result = int.Parse(cb_Nam.Text + cb_Thang.Text);


            loadBangCongChiTiet(result);
            CustomView(int.Parse(cb_Thang.Text) , int.Parse(cb_Nam.Text));
            //phatSinhKyCongChiTiet(4, 2024);
        }
        public void phatSinhKyCongChiTiet(int thang, int nam)
        {
            List<NhanVien> lstNV = GetAllNhanVien();
            if (lstNV.Count == 0) return;

            foreach (var item in lstNV)
            {
                List<string> listDay = new List<string>();

                for (int j = 1; j <= GetDayNumber(thang, nam); j++)
                {
                    DateTime newDate = new DateTime(nam, thang, j);

                    switch (newDate.DayOfWeek)
                    {
                        case DayOfWeek.Sunday:
                            listDay.Add("'CN'");
                            break;
                        case DayOfWeek.Saturday:
                            listDay.Add("'T7'");
                            break;
                        default:
                            listDay.Add("'X'");
                            break;
                    }
                }

                // Thêm các giá trị mặc định nếu cần
                while (listDay.Count < 31)
                {
                    listDay.Add("''");
                }

                string query = $@"INSERT INTO kycongchitiet (MAKYCONG, MANV, HOTEN, D1, D2, D3, D4, D5, D6, D7, D8, D9, D10, D11, D12, D13, D14, D15, D16, D17, D18, D19, D20, D21, D22, D23, D24, D25, D26, D27, D28, D29, D30, D31)
                          VALUES ({nam * 10 + thang}, {item.IDNV}, '{item.HOTEN}', {string.Join(", ", listDay)})";

                mySQLConnector.ExecuteQuery(query);
            }
        }






        private List<NhanVien> GetAllNhanVien()
        {
            List<NhanVien> lstNV = new List<NhanVien>();

            // Tạo câu truy vấn SQL
            string query = "SELECT * FROM nhanvien";

            // Thực thi truy vấn và lấy dữ liệu từ cơ sở dữ liệu
            DataTable dataTable = mySQLConnector.Select(query);

            // Kiểm tra xem truy vấn có thành công hay không
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                // Duyệt qua từng dòng dữ liệu
                foreach (DataRow row in dataTable.Rows)
                {
                    // Đọc giá trị từ cột IDNV và HOTEN
                    int id = Convert.ToInt32(row["MANV"]);
                    string name = row["HOTEN"].ToString();

                    // Tạo đối tượng NhanVien và thêm vào danh sách
                    lstNV.Add(new NhanVien { IDNV = id, HOTEN = name });
                }
            }

            return lstNV;
        }



        private int GetDayNumber(int thang, int nam)
        {
            int dayNumber = 0;
            switch (thang)
            {
                case 2:
                    dayNumber = (nam % 4 == 0 && nam % 100 != 0) || nam % 400 == 0 ? 29 : 28;
                    break;
                case 4:
                case 6:
                case 9:
                case 11:
                    dayNumber = 30;
                    break;
                default:
                    dayNumber = 31;
                    break;
            }
            return dayNumber;
        }

        private void loadBangCongChiTiet(int makycong)
        {

            //gcBangCongChiTiet.DataSource = _kcct.getList(int.Parse(cboNam.Text) * 100 + int.Parse(cboThang.Text));
            //CustomView(int.Parse(cboThang.Text), int.Parse(cboNam.Text));

            try
            {
                string query = "SELECT * FROM KYCONGCHITIET WHERE MAKYCONG = " + makycong;
                DataTable dataTable = mySQLConnector.Select(query);
                gridControl1.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load dữ liệu: " + ex.Message);
            }

        }

        private void btnPhatSinhKyCong_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            phatSinhKyCongChiTiet(int.Parse(cb_Thang.Text), int.Parse(cb_Nam.Text));

            LoadData();
        }

       

        private void btnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadData();
        }

        private void btnPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void btnDong_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void CustomView(int thang, int nam)
        {
            gridView1.RestoreLayoutFromXml(Application.StartupPath + @"\BangCong_Layout.xml");

            string[] dayHeaders = { "CN", "T.Hai", "T.Ba", "T.Tư", "T.Năm", "T.Sáu", "T.Bảy" };

            foreach (GridColumn gridColumn in gridView1.Columns)
            {
                if (gridColumn.FieldName == "HOTEN") continue;

                RepositoryItemTextEdit textEdit = new RepositoryItemTextEdit();
                textEdit.Mask.MaskType = MaskType.RegEx;
                textEdit.Mask.EditMask = @"\p{Lu}+";
                gridColumn.ColumnEdit = textEdit;
            }

            int dayNumber = GetDayNumber(thang, nam);

            for (int i = 1; i <= dayNumber; i++)
            {
                DateTime newDate = new DateTime(nam, thang, i);

                GridColumn column = gridView1.Columns["D" + i];
                column.AppearanceHeader.Font = new Font("Tahoma", 8, FontStyle.Regular);
                int dayIndex = (int)newDate.DayOfWeek;

                column.Caption = $"{dayHeaders[dayIndex]} {Environment.NewLine} {i}";

                if (dayIndex == (int)DayOfWeek.Saturday)
                {
                    column.AppearanceHeader.ForeColor = Color.Red;
                    column.AppearanceHeader.BackColor = Color.Violet;
                    column.AppearanceHeader.BackColor2 = Color.Violet;
                    column.AppearanceCell.BackColor = Color.Khaki;
                }
                else if (dayIndex == (int)DayOfWeek.Sunday)
                {
                    column.OptionsColumn.AllowEdit = false;
                    column.AppearanceHeader.ForeColor = Color.Red;
                    column.AppearanceHeader.BackColor = Color.GreenYellow;
                    column.AppearanceHeader.BackColor2 = Color.GreenYellow;
                    column.AppearanceCell.BackColor = Color.Orange;
                }
                else
                {
                    column.AppearanceHeader.ForeColor = Color.Blue;
                    column.AppearanceHeader.BackColor = Color.Transparent;
                    column.AppearanceHeader.BackColor2 = Color.Transparent;
                    column.AppearanceCell.BackColor = Color.Transparent;
                }

                column.OptionsColumn.AllowEdit = (dayIndex != (int)DayOfWeek.Sunday);
                column.OptionsColumn.AllowFocus = true;
            }

            for (int i = dayNumber + 2; i <= 31; i++)
            {
                gridView1.Columns["D" + i].Visible = false;
            }
        }

        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            // Lấy giá trị cột và dòng hiện tại
            int rowIndex = e.RowHandle;
            int colIndex = e.Column.VisibleIndex;
            string fieldName = $"D{colIndex -1}";

            // Lấy giá trị mới từ ô chỉnh sửa
            string newValue = gridView1.GetRowCellValue(rowIndex, e.Column).ToString();

            // Lấy khóa chính (primary key) của dòng hiện tại từ cột MAKYCONG
            int makycong = Convert.ToInt32(gridView1.GetRowCellValue(rowIndex, "MAKYCONG"));
            int manv = Convert.ToInt32(gridView1.GetRowCellValue(rowIndex, "MANV"));

            // Xây dựng truy vấn SQL để cập nhật dữ liệu tương ứng trong cơ sở dữ liệu XAMPP
            string query = $"UPDATE KYCONGCHITIET SET {fieldName} = '{newValue}' WHERE MAKYCONG = {makycong} AND MANV = {manv}";

            // Thực thi truy vấn SQL
            mySQLConnector.ExecuteQuery(query);
        }




    }

    public class NhanVien
    {
        public int IDNV { get; set; }
        public string HOTEN { get; set; }
    }



    
}
