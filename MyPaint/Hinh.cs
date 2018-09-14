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
    class Hinh : ISerializable
    {
        #region Thuộc tính
        //Thuộc tính đặc trưng từng loại hình
        public int LoaiHinh;                  //Mã loại hình vẽ.
        protected int SoDiemDieuKhien;
        public Point DiemBatDau;
        public Point DiemKetThuc;

        //Các thuộc tính cho Pen
        public Color MauVe;
        public int DoDamNet;
        public DashStyle KieuButVe;

        //Một số thuộc tính khác
        protected GraphicsPath GraphicsPath;  // Công cụ vẽ
        public Region KhuVuc;
        protected Point DiemNhanChuot;
        protected int ViTriSoVoiHinh;  // Vị trí tương đối của 1 điểm và đối tượng
        public bool IsDiChuyen;
        public bool IsThayDoiKichThuoc;
        #endregion

        #region Khởi tạo
        public Hinh()
        {
            MauVe = Color.Black;
            DoDamNet = 1;
            KieuButVe = DashStyle.Solid;
        }
        public Hinh(Color mauve, int dodamnet, DashStyle kieubutve)
        {
            MauVe = mauve;
            DoDamNet = dodamnet;
            KieuButVe = kieubutve;
        }
        public Hinh(Color mauve, int dodamnet, DashStyle kieubutve, Point diembatdau, Point diemketthuc, Point diemnhanchuot,
            int sodiemdieukhien, GraphicsPath graphicspath, Region khuvuc, int vitrisovoihinh,
            bool isdichuyen, bool isthaydoikichthuoc, int loaihinh)
        {
            LoaiHinh = loaihinh;
            SoDiemDieuKhien = sodiemdieukhien;
            DiemBatDau = diembatdau;
            DiemKetThuc = diemketthuc;
            MauVe = mauve;
            DoDamNet = dodamnet;
            KieuButVe = kieubutve;
            GraphicsPath = graphicspath;
            KhuVuc = khuvuc;
            DiemNhanChuot = diemnhanchuot;
            ViTriSoVoiHinh = vitrisovoihinh;
            IsDiChuyen = isdichuyen;
            IsThayDoiKichThuoc = isthaydoikichthuoc;
        }
        #endregion

        #region Giải tuần tự
        public Hinh(SerializationInfo info, StreamingContext ctxt)
        {
            LoaiHinh = (int)info.GetValue("LoaiHinh", typeof(int));
            SoDiemDieuKhien = (int)info.GetValue("SoDiemDieuKhien", typeof(int));
            DiemBatDau = (Point)info.GetValue("DiemBatDau", typeof(Point));
            DiemKetThuc = (Point)info.GetValue("DiemKetThuc", typeof(Point));
            MauVe = (Color)info.GetValue("MauVe", typeof(Color));
            DoDamNet = (int)info.GetValue("DoDamNet", typeof(int));
            KieuButVe = (DashStyle)info.GetValue("KieuButVe", typeof(DashStyle));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("LoaiHinh", LoaiHinh);
            info.AddValue("SoDiemDieuKhien", SoDiemDieuKhien);
            info.AddValue("DiemBatDau", DiemBatDau);
            info.AddValue("DiemKetThuc", DiemKetThuc);
            info.AddValue("MauVe", MauVe);
            info.AddValue("DoDamNet", DoDamNet);
            info.AddValue("KieuButVe", KieuButVe);
        }
        #endregion

        #region Phương thức
        public virtual void ThayDoi()
        {
            if (DiemBatDau.X > DiemKetThuc.X)
            {
                int tam = DiemKetThuc.X;
                DiemKetThuc.X = DiemBatDau.X;
                DiemBatDau.X = tam;
            }
            if (DiemBatDau.Y > DiemKetThuc.Y)
            {
                int tam = DiemKetThuc.Y;
                DiemKetThuc.Y = DiemBatDau.Y;
                DiemBatDau.Y = tam;
            }
        }

        // Vẽ
        public virtual void Ve(Graphics g)
        {
        }

        // Tạo điểm điều khiển
        protected virtual Point DiemDieuKhien(int ViTriDiemDieuKhien)
        {
            return new Point(0, 0);
        }

        // Tạo HCN quanh điểm điều khiển (8 chấm vuông)
        protected virtual Rectangle VeChamVuong(int ViTriDiemDieuKhien)
        {
            Point point = DiemDieuKhien(ViTriDiemDieuKhien);
            return new Rectangle(point.X - 5, point.Y - 5, 10, 10);
        }
        protected virtual Rectangle VeChamVuong(int ViTriDiemDieuKhien, int DoRongHCN)
        {
            Point point = DiemDieuKhien(ViTriDiemDieuKhien);
            return new Rectangle(point.X - DoRongHCN / 2, point.Y - DoRongHCN / 2, DoRongHCN, DoRongHCN);
        }
        // Vẽ điểm điều khiển
        public virtual void VeKhung(Graphics g)
        {

            for (int i = 1; i <= SoDiemDieuKhien; i++)
            {
                Pen pen = new Pen(Color.Blue, 1);
                if (i == 1)
                    pen = new Pen(Color.Red, 1);        //vẽ điểm số 1 màu đỏ cho đặc biệt :v
                g.DrawRectangle(pen, VeChamVuong(i, 3));
                g.FillRectangle(new SolidBrush(pen.Color), VeChamVuong(i, 6));   //tô chấm vuông
                pen.Dispose();
            }
        }

        public virtual void VeHCNDiemDieuKhien(Graphics g, int DoDamNet)
        {
            for (int i = 1; i <= SoDiemDieuKhien; i++)
            {
                Pen pen = new Pen(Color.Blue, DoDamNet);
                if (i == 1)
                    pen = new Pen(Color.Red, DoDamNet);
                g.DrawRectangle(pen, VeChamVuong(i, 5));
                g.FillRectangle(Brushes.Blue, VeChamVuong(i, 4));
                pen.Dispose();
            }
        }

        // Kiểm tra xem 1 điểm có thuộc khu vực chiếm giữ đối tượng này hay không
        protected virtual bool KiemTraThuoc(Point point)
        {
            if (KhuVuc.IsVisible(point) == true)
                return true;
            return false;
        }

        // Kiểm tra vị trí tương đối của 1 điểm và 1 đối tượng
        // - 1 : Nằm ngoài đối tượng
        // =0   : Trong đối tượng
        // >= 1 : Điểm điều khiển 
        public virtual int KiemTraViTri(Point point)
        {
            for (int i = 1; i <= SoDiemDieuKhien; i++)
            {
                if (VeChamVuong(i).Contains(point) == true) //điểm đó nằm trên hình chữ nhật bao quanh 1 điểm điều khiển (8 chấm vuông nhỏ)
                    return i;
            }
            if (KiemTraThuoc(point) == true)    // điểm đó thuộc khu vực bên trong hình bao quanh
                return 0;
            return -1;  //điểm và hình tách biệt nhau
        }

        // Xác định lại điểm Start và End khi Click vào 1 điểm điều khiển
        protected virtual void ThayDoiDiem(int ViTriDiemDieuKhien)
        {
        }

        // Di chuyển đối tượng khi move = true
        public virtual void DiChuyenHinh(int deltaX, int deltaY)
        {
            DiemBatDau.X += deltaX;
            DiemBatDau.Y += deltaY;
            DiemKetThuc.X += deltaX;
            DiemKetThuc.Y += deltaY;
        }

        // Thay đổi kích thước đối tượng khi biết điểm điều khiển và điểm đến, resize = true
        protected virtual void ThayDoiKichThuocHinh(int ViTriDiemDieuKhien, Point point)
        {
        }

        // Sự kiện chuột
        public virtual void Mouse_Down(MouseEventArgs e)
        {
        }

        public virtual void Mouse_Move(MouseEventArgs e)
        {
        }
        public virtual void Mouse_Up(Object sender)
        {
        }
        #endregion
    }
}
