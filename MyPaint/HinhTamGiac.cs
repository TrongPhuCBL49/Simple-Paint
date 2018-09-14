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
    class HinhTamGiac : HinhChuNhat
    {
        #region Thuộc tính
        #endregion

        #region Khởi tạo
        public HinhTamGiac()
            : base()
        {
            LoaiHinh = 4;
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
        public HinhTamGiac(Color mauve, int dodamnet, DashStyle kieubutve)
            : base(mauve, dodamnet, kieubutve)
        {
            LoaiHinh = 4;
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
        public HinhTamGiac(Color mauve, int dodamnet, DashStyle kieubutve, Point diembatdau, Point diemketthuc, Point diemnhanchuot,
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
        public HinhTamGiac(SerializationInfo info, StreamingContext ctxt)
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
            if (DiemBatDau == DiemDieuKhien(6) || DiemKetThuc == DiemDieuKhien(3))
            {
                //Vẽ tam giác ngược khi kéo từ dưới lên
                Pen pen = new Pen(MauVe, DoDamNet);
                pen.DashStyle = KieuButVe;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.DrawLine(pen, DiemDieuKhien(3), DiemDieuKhien(7));
                g.DrawLine(pen, DiemDieuKhien(7), DiemDieuKhien(1));
                g.DrawLine(pen, DiemDieuKhien(1), DiemDieuKhien(3));
                pen.Dispose();
            }
            else
            {
                //Vẽ tam giác thuận khi kéo từ trên xuống
                Pen pen = new Pen(MauVe, DoDamNet);
                pen.DashStyle = KieuButVe;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.DrawLine(pen, DiemDieuKhien(2), DiemDieuKhien(6));
                g.DrawLine(pen, DiemDieuKhien(6), DiemDieuKhien(8));
                g.DrawLine(pen, DiemDieuKhien(8), DiemDieuKhien(2));
                pen.Dispose();
            }
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
