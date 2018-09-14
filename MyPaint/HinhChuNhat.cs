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
    class HinhChuNhat : Hinh
    {
        #region Thuộc tính
        #endregion

        #region Khởi tạo
        public HinhChuNhat()
            : base()
        {
            LoaiHinh = 2;
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
        public HinhChuNhat(Color mauve, int dodamnet, DashStyle kieubutve)
            : base(mauve, dodamnet, kieubutve)
        {
            LoaiHinh = 2;
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
        public HinhChuNhat(Color mauve, int dodamnet, DashStyle kieubutve, Point diembatdau, Point diemketthuc, Point diemnhanchuot,
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
        public HinhChuNhat(SerializationInfo info, StreamingContext ctxt)
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

        // Tạo hình chữ nhật từ tọa độ 2 điểm
        protected virtual Rectangle VeHCN(int x1, int y1, int x2, int y2)
        {
            if (x1 > x2)
            {
                int tam = x1;
                x1 = x2;
                x2 = tam;
            }
            if (y1 > y2)
            {
                int tam = y1;
                y1 = y2;
                y2 = tam;
            }
            return new Rectangle(x1, y1, x2 - x1, y2 - y1);
        }

        protected virtual Rectangle VeHCN(Point A, Point B)
        {
            return VeHCN(A.X, A.Y, B.X, B.Y);
        }

        //Vẽ
        public override void Ve(Graphics g)
        {
            Pen pen = new Pen(MauVe, DoDamNet);
            pen.DashStyle = KieuButVe;
            g.DrawRectangle(pen, VeHCN(DiemBatDau, DiemKetThuc));
            pen.Dispose();
        }

        // Tạo điểm điều khiển từ tọa độ của điểm bắt đầu và điểm kết thúc, lấy các giá trị trung bình để tạo các trung điểm
        protected override Point DiemDieuKhien(int ViTriDiemDieuKhien)
        {

            int xCenter = (DiemBatDau.X + DiemKetThuc.X) / 2;
            int yCenter = (DiemBatDau.Y + DiemKetThuc.Y) / 2;
            int x = 0, y = 0;
            switch (ViTriDiemDieuKhien)
            {
                case 1:
                    {
                        x = DiemBatDau.X;
                        y = DiemBatDau.Y;
                        break;
                    }
                case 2:
                    {
                        x = xCenter;
                        y = DiemBatDau.Y;
                        break;
                    }
                case 3:
                    {
                        x = DiemKetThuc.X;
                        y = DiemBatDau.Y;
                        break;
                    }
                case 4:
                    {
                        x = DiemBatDau.X;
                        y = yCenter;
                        break;
                    }
                case 5:
                    {
                        x = DiemKetThuc.X;
                        y = yCenter;
                        break;
                    }
                case 6:
                    {
                        x = DiemBatDau.X;
                        y = DiemKetThuc.Y;
                        break;
                    }
                case 7:
                    {
                        x = xCenter;
                        y = DiemKetThuc.Y;
                        break;
                    }
                case 8:
                    {
                        x = DiemKetThuc.X;
                        y = DiemKetThuc.Y;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            return new Point(x, y);
        }

        // Chọn lại điểm bắt đầu, kết thúc khi bấm vào 1 điểm điều khiển nào đó
        protected override void ThayDoiDiem(int ViTriDiemDieuKhien)
        {
            if (ViTriDiemDieuKhien == 1 || ViTriDiemDieuKhien == 2 || ViTriDiemDieuKhien == 4)
            {
                Point point = DiemBatDau;
                DiemBatDau = DiemKetThuc;
                DiemKetThuc = point;
            }
            if (ViTriDiemDieuKhien == 3)
            {
                DiemBatDau = DiemDieuKhien(6);
                DiemKetThuc = DiemDieuKhien(3);
            }
            if (ViTriDiemDieuKhien == 6)
            {
                DiemBatDau = DiemDieuKhien(3);
                DiemKetThuc = DiemDieuKhien(6);
            }
        }

        // Thay đổi kích thước đối tượng khi biết 1 điểm điều khiển và điểm đến
        protected override void ThayDoiKichThuocHinh(int ViTriDiemDieuKhien, Point newPoint)
        {
            int deltaX = newPoint.X - DiemNhanChuot.X;
            int deltaY = newPoint.Y - DiemNhanChuot.Y;
            DiemNhanChuot = newPoint;
            if (ViTriDiemDieuKhien == 2 || ViTriDiemDieuKhien == 7)
            {
                DiemKetThuc.Y += deltaY;    //2 cạnh nằm ngang tịnh tiến lên xuống
            }
            else if (ViTriDiemDieuKhien == 4 || ViTriDiemDieuKhien == 5)
            {
                DiemKetThuc.X += deltaX;    //2 cạnh đứng tịnh tiến trái phải
            }
            else
            {
                DiemKetThuc = newPoint;     //các góc di chuyển theo chuột
            }
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
            GraphicsPath.AddRectangle(VeHCN(DiemBatDau, DiemKetThuc));
            GraphicsPath.Widen(pen);
            KhuVuc = new Region(VeHCN(DiemBatDau, DiemKetThuc));
            KhuVuc.Union(GraphicsPath);
            IsDiChuyen = false;
            IsThayDoiKichThuoc = false;
            ViTriSoVoiHinh = -1;
        }
        #endregion
    }
}
