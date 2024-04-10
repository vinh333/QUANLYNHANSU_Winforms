using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace QLNHANSU.CHAMCONG
{
    public partial class FormBangCongChiTiet : DevExpress.XtraEditors.XtraForm
    {
        private MySQLConnector mySQLConnector;

        public FormBangCongChiTiet()
        {
            InitializeComponent();
            mySQLConnector = new MySQLConnector();
        }
        private void FormKyCong_Load(object sender, EventArgs e)
        {
            gridView1.OptionsBehavior.Editable = false; // Chặn chỉnh sửa trực tiếp

            LoadData();
        }

        private void LoadData()
        {
            phatSinhKyCongChiTiet(4, 2024);
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
                          VALUES ({nam * 100 + thang}, {item.IDNV}, '{item.HOTEN}', {string.Join(", ", listDay)})";

                mySQLConnector.ExecuteQuery(query);
            }
        }






        private List<NhanVien> GetAllNhanVien()
        {
            List<NhanVien> lstNV = new List<NhanVien>();

            // Replace this code with your actual data retrieval logic
            // For demonstration purposes, a hardcoded list is provided here
            lstNV.Add(new NhanVien { IDNV = 2, HOTEN = "Employee 1" });
            lstNV.Add(new NhanVien { IDNV = 13, HOTEN = "Employee 2" });
      

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
    }

    public class NhanVien
    {
        public int IDNV { get; set; }
        public string HOTEN { get; set; }
    }
}
