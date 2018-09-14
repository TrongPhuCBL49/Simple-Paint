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
    class HinhDaGiac:HinhChuNhat
    {
        #region Thuộc tính
        public List<Point> DaGiac;
        Point Diem;
        int MinX, MinY, MaxX, MaxY;
        #endregion

        #region Khởi tạo
        public HinhDaGiac()
            : base()
        {
            LoaiHinh = 5;
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
            //DaGiac = new List<Point>();
            MinX = MinY = 5000;
            MaxX = MaxY = 0;
        }
        public HinhDaGiac(Color mauve, int dodamnet, DashStyle kieubutve)
            : base(mauve, dodamnet, kieubutve)
        {
            LoaiHinh = 5;
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
        public HinhDaGiac(Color mauve, int dodamnet, DashStyle kieubutve, Point diembatdau, Point diemketthuc, Point diemnhanchuot,
            int sodiemdieukhien, GraphicsPath graphicspath, Region khuvuc, int vitrisovoihinh,
            bool isdichuyen, bool isthaydoikichthuoc, int loaihinh, List<Point> dagiac)
            : base(mauve, dodamnet, kieubutve, diembatdau, diemketthuc, diemnhanchuot, sodiemdieukhien, graphicspath, khuvuc, vitrisovoihinh, isdichuyen, isthaydoikichthuoc, loaihinh)
        {
            LoaiHinh = loaihinh;
            DaGiac = dagiac;
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
            DaGiac = dagiac;
        }
        #endregion
        #region Tuần tự hóa và giải tuần tự hóa
        public HinhDaGiac(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        {
            KhuVuc = new Region(VeHCN(DiemBatDau, DiemKetThuc));
            DaGiac = new List<Point>();
        }
        public new void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            base.GetObjectData(info, ctxt);
            info.AddValue("DaGiac", DaGiac);
        }
        #endregion
        #region Phương thức

        public override void Ve(Graphics g)
        {
            try
            {
                if (DaGiac.Count == 2)
            {
                Pen pen = new Pen(MauVe, DoDamNet);
                pen.DashStyle = KieuButVe;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.DrawLine(pen, DaGiac[0], DaGiac[1]);
                pen.Dispose();
            }
                if (DaGiac.Count > 2)
                {
                    Pen pen = new Pen(MauVe, DoDamNet);
                    pen.DashStyle = KieuButVe;
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.DrawPolygon(pen, DaGiac.ToArray());
                    pen.Dispose();
                }
            }
            catch
            {

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

        public override void Mouse_Down(MouseEventArgs e)
        {
            ViTriSoVoiHinh = KiemTraViTri(e.Location);
            if (ViTriSoVoiHinh > 0)  //đánh dấu bắt đầu thay đổi kích thước
            {
                IsThayDoiKichThuoc = true;
                ThayDoiDiem(ViTriSoVoiHinh);
                DiemNhanChuot = e.Location;
            }
            else if (ViTriSoVoiHinh == 0)    //đánh dấu băt đầu di chuyển
            {
                IsDiChuyen = true;
                DiemNhanChuot = e.Location;
            }
            else //vẽ hình mới
            {
                if (DaGiac != null)
                {
                    // We are already drawing a polygon.
                    // If it's the right mouse button, finish this polygon.
                    if (e.Button == MouseButtons.Right)
                    {
                        // Finish this polygon.
                        if (DaGiac.Count > 2)
                        {
                            GraphicsPath = new GraphicsPath();
                            Pen pen = new Pen(MauVe, DoDamNet);
                            pen.DashStyle = KieuButVe;
                            GraphicsPath.AddPolygon(DaGiac.ToArray());
                            GraphicsPath.Widen(pen);
                            KhuVuc = new Region(VeHCN(DiemBatDau, DiemKetThuc));
                            KhuVuc.Union(GraphicsPath);
                            IsDiChuyen = false;
                            IsThayDoiKichThuoc = false;
                            ViTriSoVoiHinh = -1;
                        }
                    }
                    else
                    {
                        // Add a point to this polygon.
                        if (DaGiac[DaGiac.Count - 1] != e.Location)
                        {
                            DaGiac.Add(e.Location);
                            CapNhatKhung(e.Location);

                        }
                    }
                }
                else
                {
                    // Start a new polygon.
                    DaGiac = new List<Point>();
                    Diem = e.Location;
                    DaGiac.Add(e.Location);
                    CapNhatKhung(e.Location);
                }
            }
        }
        public void CapNhatKhung(Point e)
        {
            if (e.X < MinX) MinX = e.X;
            if (e.Y < MinY) MinY = e.Y;
            if (e.X > MaxX) MaxX = e.X;
            if (e.Y > MaxY) MaxY = e.Y;
            DiemBatDau.X = MinX; DiemBatDau.Y = MinY;
            DiemKetThuc.X = MaxX; DiemKetThuc.Y = MaxY;
        }
        public override void Mouse_Move(MouseEventArgs e)
        {
            if (IsThayDoiKichThuoc == true)
            {
                ThayDoiKichThuocHinh(ViTriSoVoiHinh, e.Location);
            }
            else if (IsDiChuyen == true)
            {
                int deltaX = e.X - DiemNhanChuot.X;
                int deltaY = e.Y - DiemNhanChuot.Y;
                DiemNhanChuot = e.Location;
                DiChuyenHinh(deltaX, deltaY);
            }
            else
            {
                if (DaGiac == null) return;
                Diem = e.Location;
            }
        }
        public override void DiChuyenHinh(int deltaX, int deltaY)
        {
            base.DiChuyenHinh(deltaX, deltaY);
            List<Point> DaGiac2 = new List<Point>();
            for (int i = 0; i < DaGiac.Count; i++)
            {
                Point point = new Point(DaGiac[i].X + deltaX, DaGiac[i].Y + deltaY);
                DaGiac2.Add(point);
            }
            DaGiac = DaGiac2;
        }

        protected override void ThayDoiKichThuocHinh(int ViTriDiemDieuKhien, Point newPoint)
        {
        }
        #endregion

    }
}
