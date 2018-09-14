using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Runtime.Serialization;

namespace MyPaint
{
    [Serializable()]
    class HinhOval : HinhChuNhat
    {
        #region Thuộc tính
        #endregion

        #region Khởi tạo
        public HinhOval()
            : base()
        {
            LoaiHinh = 3;
            SoDiemDieuKhien = 8;
            DiemBatDau.X = 0; DiemBatDau.Y = 0;
            DiemKetThuc.X = 0; DiemKetThuc.Y = 1;
            Pen pen = new Pen(MauVe, DoDamNet);
            pen.DashStyle = KieuButVe;
            GraphicsPath = new GraphicsPath();
            GraphicsPath.AddRectangle(new Rectangle(0, 0, 0, 1));
            GraphicsPath.Widen(pen);
            KhuVuc = new Region(new Rectangle(0, 0, 0, 1));
            KhuVuc.Union(GraphicsPath);
        }
        public HinhOval(Color mauve, int dodamnet, DashStyle kieubutve)
            : base(mauve, dodamnet, kieubutve)
        {
            LoaiHinh = 3;
            SoDiemDieuKhien = 8;
            DiemBatDau.X = 0; DiemBatDau.Y = 0;
            DiemKetThuc.X = 0; DiemKetThuc.Y = 1;
            Pen pen = new Pen(MauVe, DoDamNet);
            pen.DashStyle = KieuButVe;
            GraphicsPath = new GraphicsPath();
            GraphicsPath.AddRectangle(new Rectangle(0, 0, 0, 1));
            GraphicsPath.Widen(pen);
            KhuVuc = new Region(new Rectangle(0, 0, 0, 1));
            KhuVuc.Union(GraphicsPath);
        }
        public HinhOval(Color mauve, int dodamnet, DashStyle kieubutve, Point diembatdau, Point diemketthuc, Point diemnhanchuot,
            int sodiemdieukhien, GraphicsPath graphicspath, Region khuvuc, int vitrisovoihinh,
            bool isdichuyen, bool isthaydoikichthuoc, int loaihinh)
            : base(mauve, dodamnet, kieubutve, diembatdau, diemketthuc, diemnhanchuot, sodiemdieukhien, graphicspath, khuvuc, vitrisovoihinh, isdichuyen, isthaydoikichthuoc, loaihinh)
        {
            LoaiHinh = loaihinh;
            MauVe = mauve;
            DoDamNet = dodamnet;
            KieuButVe = kieubutve;
            DiemBatDau = diembatdau;
            DiemKetThuc = diemketthuc;
            DiemNhanChuot = diemnhanchuot;
            SoDiemDieuKhien = sodiemdieukhien;
            GraphicsPath = graphicspath;
            KhuVuc = khuvuc;
            ViTriSoVoiHinh = vitrisovoihinh;
            IsDiChuyen = isdichuyen;
            IsThayDoiKichThuoc = isthaydoikichthuoc;
        }
        #endregion
        #region Tuần tự hóa và giải tuần tự hóa
        public HinhOval(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        {
            KhuVuc = new Region(VeHCN(DiemBatDau, DiemKetThuc));
        }
        public new void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            base.GetObjectData(info, ctxt);
        }
        #endregion
        #region Phương thức

        public override void Ve(Graphics g)
        {
            Pen pen = new Pen(MauVe, DoDamNet);
            pen.DashStyle = KieuButVe;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.DrawEllipse(pen, VeHCN(DiemBatDau, DiemKetThuc));
            pen.Dispose();
        }

        public override void VeKhung(Graphics g)
        {
            base.VeKhung(g);
            Pen pen = new Pen(Color.Blue, 1);
            pen.DashStyle = DashStyle.Dash;
            g.DrawRectangle(pen, VeHCN(DiemBatDau, DiemKetThuc));
            pen.Dispose();
        }
        #endregion
    }
}
