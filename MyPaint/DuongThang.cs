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
    class DuongThang : Hinh
    {
        #region Thuộc tính
        #endregion

        #region Khởi tạo
        public DuongThang()
            : base()
        {
            LoaiHinh = 0;
            SoDiemDieuKhien = 2;
            DiemBatDau.X = 0; DiemBatDau.Y = 0;
            DiemKetThuc.X = 0; DiemKetThuc.Y = 1;
            Pen pen = new Pen(MauVe, DoDamNet);
            pen.DashStyle = KieuButVe;
            GraphicsPath = new GraphicsPath();
            GraphicsPath.AddLine(DiemBatDau, DiemKetThuc);
            GraphicsPath.Widen(pen);
            KhuVuc = new Region(GraphicsPath);
        }
        public DuongThang(Color mauve, int dodamnet, DashStyle kieubutve)
            : base(mauve, dodamnet, kieubutve)
        {
            LoaiHinh = 0;
            SoDiemDieuKhien = 2;
            DiemBatDau.X = 0; DiemBatDau.Y = 0;
            DiemKetThuc.X = 0; DiemKetThuc.Y = 1;
            Pen pen = new Pen(MauVe, DoDamNet);
            pen.DashStyle = KieuButVe;
            GraphicsPath = new GraphicsPath();
            GraphicsPath.AddLine(DiemBatDau, DiemKetThuc);
            GraphicsPath.Widen(pen);
            KhuVuc = new Region(GraphicsPath);
        }
        public DuongThang(Color mauve, int dodamnet, DashStyle kieubutve, Point diembatdau, Point diemketthuc, Point diemnhanchuot,
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
        public DuongThang(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        {
        }
        public new void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            base.GetObjectData(info, ctxt);
        }
        #endregion
        #region Phương thức

        //Vẽ
        public override void Ve(Graphics g)
        {
            Pen pen = new Pen(MauVe, DoDamNet);
            pen.DashStyle = KieuButVe;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.DrawLine(pen, DiemBatDau, DiemKetThuc);
            pen.Dispose();
        }

        // Tạo điểm điều khiển từ tọa độ của điểm bắt đầu và điểm kết thúc, lấy các giá trị trung bình để tạo các trung điểm
        protected override Point DiemDieuKhien(int ViTriDiemDieuKhien)
        {
            if (ViTriDiemDieuKhien == 1)
                return DiemBatDau;
            return DiemKetThuc;
        }

        // Chọn lại điểm bắt đầu, kết thúc khi bấm vào 1 điểm điều khiển nào đó
        protected override void ThayDoiDiem(int ViTriDiemDieuKhien)
        {
            if (ViTriDiemDieuKhien == 1)
            {
                Point point = DiemBatDau;
                DiemBatDau = DiemKetThuc;
                DiemKetThuc = point;
            }
        }

        // Thay đổi kích thước đối tượng khi biết 1 điểm điều khiển và điểm đến
        protected override void ThayDoiKichThuocHinh(int ViTriDiemDieuKhien, Point newPoint)
        {
            DiemKetThuc = newPoint;
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
                DiemBatDau = e.Location;
                DiemKetThuc.X = e.X; DiemKetThuc.Y = e.Y;
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
                DiemKetThuc = e.Location;
            }
        }
        #endregion

        #region Chiến thuật thi hành lệnh bắt đối tượng
        public override void Mouse_Up(Object sender)
        {
            GraphicsPath = new GraphicsPath();
            Pen pen = new Pen(MauVe, DoDamNet);
            pen.DashStyle = KieuButVe;
            GraphicsPath.AddLine(DiemBatDau, DiemKetThuc);
            //GraphicsPath.Widen(pen);
            //Tạo khu vực cho đường thẳng
            GraphicsPath gp = new GraphicsPath();
            gp.AddLine(new Point(DiemBatDau.X + 15, DiemBatDau.Y), new Point(DiemKetThuc.X + 15, DiemKetThuc.Y));
            gp.AddLine(new Point(DiemBatDau.X - 15, DiemBatDau.Y), new Point(DiemKetThuc.X - 15, DiemKetThuc.Y));
            gp.AddLine(new Point(DiemBatDau.X, DiemBatDau.Y + 15), new Point(DiemKetThuc.X, DiemKetThuc.Y + 15));
            gp.AddLine(new Point(DiemBatDau.X, DiemBatDau.Y - 15), new Point(DiemKetThuc.X, DiemKetThuc.Y - 15));
            KhuVuc = new Region(gp);

            IsDiChuyen = false;
            IsThayDoiKichThuoc = false;
            ViTriSoVoiHinh = -1;
        }
        #endregion
    }
}
