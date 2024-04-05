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
using System.Windows.Forms;

namespace QLNHANSU
{
    public partial class FormDangNhap : DevExpress.XtraEditors.XtraForm
    {
        private MySQLConnector mySQLConnector;

        public FormDangNhap()
        {
            InitializeComponent();
            mySQLConnector = new MySQLConnector();
        }
        private void Form_Login_Load(object sender, EventArgs e)
        {
            // Ẩn mật khẩu ngay khi form được tải lên
            txt_PassWord.Properties.PasswordChar = '*';
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            string username = txt_UserName.Text.Trim();
            string password = txt_PassWord.Text.Trim();

            // Kiểm tra xem người dùng đã nhập tên người dùng và mật khẩu chưa
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                XtraMessageBox.Show("Vui lòng nhập tên người dùng và mật khẩu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Thực hiện truy vấn để kiểm tra xem tên người dùng và mật khẩu có tồn tại trong cơ sở dữ liệu không
            string query = $"SELECT COUNT(*) FROM Users WHERE Username = '{username}' AND Password = '{password}'";
            object result = mySQLConnector.ExecuteScalar(query);

            // Kiểm tra kết quả trả về
            if (result != null && Convert.ToInt32(result) > 0)
            {
                XtraMessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Sau khi đăng nhập thành công, bạn có thể mở form chính hoặc thực hiện hành động mong muốn ở đây.
                // Ví dụ: FormMain formMain = new FormMain();
                // formMain.Show();
            }
            else
            {
                XtraMessageBox.Show("Tên người dùng hoặc mật khẩu không đúng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void chkHienThiMatKhau_CheckedChanged(object sender, EventArgs e)
        {
            // Hiển thị hoặc ẩn mật khẩu khi nhấn vào checkbox
            txt_PassWord.Properties.PasswordChar = chk_HienThiMatKhau.Checked ? '\0' : '*';
        }
    }
}
