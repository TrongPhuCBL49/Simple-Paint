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
    class DuongCong : Hinh
    {
        #region Thuộc tính
        //Điểm 1 và 4 là điểm đầu cuối
        //Điểm 2 và 3 là điểm điều khiển
        public Point[] Diem;
        public int DiemDangVe = 1;
        #endregion

        #region Khởi tạo
        public DuongCong()
            : base()
        {
            LoaiHinh = 1;
            SoDiemDieuKhien = 4;
            Diem = new Point[5];
            Diem[1].X = 0; Diem[1].Y = 0;
            Diem[2].X = 0; Diem[2].Y = 1;
            Diem[3].X = 0; Diem[3].Y = 2;
            Diem[4].X = 0; Diem[4].Y = 3;
            Pen pen = new Pen(MauVe, DoDamNet);
            pen.DashStyle = KieuButVe;
            GraphicsPath = new GraphicsPath();
            GraphicsPath.AddBezier(Diem[1], Diem[2], Diem[3], Diem[4]);
            GraphicsPath.Widen(pen);
            KhuVuc = new Region(GraphicsPath);
        }
        public DuongCong(Color mauve, int dodamnet, DashStyle kieubutve)
            : base(mauve, dodamnet, kieubutve)
        {
            LoaiHinh = 1;
            SoDiemDieuKhien = 4;
            Diem = new Point[5];
            Diem[1].X = 0; Diem[1].Y = 0;
            Diem[2].X = 0; Diem[2].Y = 1;
            Diem[3].X = 0; Diem[3].Y = 2;
            Diem[4].X = 0; Diem[4].Y = 3;
            Pen pen = new Pen(MauVe, DoDamNet);
            pen.DashStyle = KieuButVe;
            GraphicsPath = new GraphicsPath();
            GraphicsPath.AddBezier(Diem[1], Diem[2], Diem[3], Diem[4]);
            GraphicsPath.Widen(pen);
            KhuVuc = new Region(GraphicsPath);
        }
        public DuongCong(Color mauve, int dodamnet, DashStyle kieubutve, Point diembatdau, Point diemketthuc, Point diemnhanchuot,
            int sodiemdieukhien, GraphicsPath graphicspath, Region khuvuc, int vitrisovoihinh,
            bool isdichuyen, bool isthaydoikichthuoc, int loaihinh, Point[] diem)
            : base(mauve, dodamnet, kieubutve, diembatdau, diemketthuc, diemnhanchuot, sodiemdieukhien, graphicspath, khuvuc, vitrisovoihinh, isdichuyen, isthaydoikichthuoc, loaihinh)
        {
            LoaiHinh = loaihinh;
            MauVe = mauve;
            DoDamNet = dodamnet;
            KieuButVe = kieubutve;
            DiemBatDau = diembatdau;
            DiemKetThuc = diemketthuc;
            Diem = diem;
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
        public DuongCong(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        {
        }
        public new void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            base.GetObjectData(info, ctxt);
        }
        #endregion
        #region
        //Vẽ
        public override void Ve(Graphics g)
        {
            Pen pen = new Pen(MauVe, DoDamNet);
            pen.DashStyle = KieuButVe;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.DrawBezier(pen, Diem[1], Diem[2], Diem[3], Diem[4]);
            pen.Dispose();
        }

        // Tạo điểm điều khiển từ tọa độ của điểm bắt đầu và điểm kết thúc, lấy các giá trị trung bình để tạo các trung điểm
        protected override Point DiemDieuKhien(int ViTriDiemDieuKhien)
        {
            switch (ViTriDiemDieuKhien)
            {
                case 1:
                    return Diem[1];
                case 2:
                    return Diem[2];
                case 3:
                    return Diem[3];
                case 4:
                    return Diem[4];
                default:
                    return new Point(0, 0);
            }
        }

        // Thay đổi kích thước đối tượng khi biết 1 điểm điều khiển và điểm đến
        protected override void ThayDoiKichThuocHinh(int ViTriDiemDieuKhien, Point newPoint)
        {
            Diem[ViTriDiemDieuKhien] = newPoint;
        }

        public override void VeKhung(Graphics g)
        {
            Pen pen = new Pen(Color.Blue, 1);
            for (int i = 1; i <= SoDiemDieuKhien; i++)
            {
                g.DrawRectangle(pen, VeChamVuong(i, 3));
                g.FillRectangle(new SolidBrush(Color.Blue), VeChamVuong(i, 6));
            }
            pen.Dispose();
        }

        // Sự kiện chuột
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
                Diem[1] = e.Location;
                Diem[2].X = e.X; Diem[2].Y = e.Y;
                Diem[3].X = e.X; Diem[3].Y = e.Y;
                Diem[4].X = e.X; Diem[4].Y = e.Y;
            }
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
                Diem[4] = e.Location;
                Diem[2].X = (2 * Diem[1].X + Diem[4].X) / 3;
                Diem[2].Y = (2 * Diem[1].Y + Diem[4].Y) / 3;
                Diem[3].X = (Diem[1].X + 2 * Diem[4].X) / 3;
                Diem[3].Y = (Diem[1].Y + 2 * Diem[4].Y) / 3;
            }
        }
        public override void DiChuyenHinh(int deltaX, int deltaY)
        {
            Diem[1].X += deltaX;
            Diem[1].Y += deltaY;
            Diem[2].X += deltaX;
            Diem[2].Y += deltaY;
            Diem[3].X += deltaX;
            Diem[3].Y += deltaY;
            Diem[4].X += deltaX;
            Diem[4].Y += deltaY;

        }
        #endregion

        #region Chiến thuật thi hành lệnh bắt đối tượng
        public override void Mouse_Up(Object sender)
        {
            GraphicsPath = new GraphicsPath();
            Pen pen = new Pen(MauVe, DoDamNet);
            pen.DashStyle = KieuButVe;
            GraphicsPath.AddBezier(Diem[1], Diem[2], Diem[3], Diem[4]);
            //GraphicsPath.Widen(pen);
            //Tạo khu vực cho đường thẳng
            GraphicsPath gp = new GraphicsPath();
            gp.AddLine(new Point(Diem[1].X + 15, Diem[1].Y), new Point(Diem[4].X + 15, Diem[4].Y));
            gp.AddLine(new Point(Diem[1].X - 15, Diem[1].Y), new Point(Diem[4].X - 15, Diem[4].Y));
            gp.AddLine(new Point(Diem[1].X, Diem[1].Y + 15), new Point(Diem[4].X, Diem[4].Y + 15));
            gp.AddLine(new Point(Diem[1].X, Diem[1].Y - 15), new Point(Diem[4].X, Diem[4].Y - 15));
            KhuVuc = new Region(gp);
            IsDiChuyen = false;
            IsThayDoiKichThuoc = false;
            ViTriSoVoiHinh = -1;
        }
        #endregion

    }
}
