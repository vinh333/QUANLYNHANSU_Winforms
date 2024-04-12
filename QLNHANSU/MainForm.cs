using QLNHANSU.CHAMCONG;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QLNHANSU
{
    public partial class MainForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public MainForm()
        {
            InitializeComponent();
        }
        private void FormMain_Load(object sender, EventArgs e)
        {
           ribbonControl1.SelectedPage = ribbonPage2 ;
        }
        void openform(Type typeForm)
        {
            foreach (var frm in MdiChildren)
            {
                if (frm.GetType() == typeForm)
                {
                    frm.Activate();
                    return;
                }
            }
            // Nếu không tìm thấy form đã mở, tạo một instance mới
            Form f = (Form)Activator.CreateInstance(typeForm);
            f.MdiParent = this;
            f.Show();
        }

        private void btnDanToc_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            openform(typeof(FormDanToc));
        }

        private void btnTonGiao_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            openform(typeof(FormTonGiao));
        }

        private void btnTrinhDo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            openform(typeof(FormTrinhDo));

        }

        private void btnPhongBan_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            openform(typeof(FormPhongBan));

        }

        private void btnChucVu_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            openform(typeof(FormChucVu));

        }

        private void btnBoPhan_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            openform(typeof(FormBoPhan));

        }

        private void btnNhanVien_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            openform(typeof(FormNhanVien));

        }

        private void btnLoaiCa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            openform(typeof(FormLoaiCa));
        }

        private void btnLoaiCong_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            openform(typeof(FormLoaiCong));
        }

        private void btnBangCong_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            openform(typeof(FormBangCong));
        }

        private void btnBangLuong_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            openform(typeof(FormBangLuong));
        }
    }
}
