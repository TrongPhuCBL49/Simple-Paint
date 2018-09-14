using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyPaint
{
    public partial class frmMain : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        #region Thuộc tính
        private DanhSachHinh DSHinh;          // List đối tượng
        private Hinh HinhHienTai;       // đối tượng hình hiện tại sẽ vẽ
        private int LoaiHinhHienTai;       // ID của đối tượng hình hiện tại

        private HinhDaGiac dg;
        private bool IsNewPolygon;

        private Color MauVe; // Màu vẽ
        private int DoDamNet; // Độ đậm
        private DashStyle KieuButVe;


        private Bitmap hinhNenChim;     //hình nền
        private Bitmap hinhNenNoi;

        bool IsDiChuyen;
        bool IsDaLuu = true;
        int LoaiHinhCuoi;

        String lastFileName = "";

        //string logFileName = "~\\ouptutLog.txt";
        // int type = 0;  //để lưu vào file log

        //  HinhVe hinhChinhSuaCuoiCung;

        Hinh HinhCopy;
        #endregion
        public frmMain()
        {
            InitializeComponent();
            KhoiTao();
        }

        #region Khởi tạo
        public void KhoiTao()
        {
            DSHinh = new DanhSachHinh();

            hinhNenChim = new Bitmap(picMain.Width, picMain.Height, picMain.CreateGraphics()); //tạo 1 hình bitmap
            Graphics g = Graphics.FromImage(hinhNenChim);   //lấy đối tượng Graphics từ bitmap
            g.Clear(Color.White);                           //xóa trắng bề mặt

            hinhNenNoi = new Bitmap(picMain.Width, picMain.Height, picMain.CreateGraphics());
            g = Graphics.FromImage(hinhNenNoi);
            g.Clear(Color.White);

            MauVe = Color.Black;
            DoDamNet = 1;
            KieuButVe = DashStyle.Solid;
            dg = new HinhDaGiac();
            IsNewPolygon = true;

            barColorPick.AutomaticColor = Color.Black;

            LoaiHinhCuoi = -1;                  //ID của nút bấm
            btnHand.Enabled = false;

            IsDiChuyen = false;

            HinhCopy = null;

            LoaiHinhHienTai = 0;
            DSHinh.danhSachHinh.Clear();

            picMain.Refresh();       //vẽ lại pictureBox-làm mới

        }
        #endregion

        Hinh LayHinhHienTai(int loaiHinhHienTai)
        {

            switch (loaiHinhHienTai)
            {

                //case -1: return new ConTro();
                case -1: return null;
                case 0: return new DuongThang(MauVe, DoDamNet, KieuButVe);
                case 1: return new DuongCong(MauVe, DoDamNet, KieuButVe);
                case 2: return new HinhChuNhat(MauVe, DoDamNet, KieuButVe);
                case 3: return new HinhOval(MauVe, DoDamNet, KieuButVe);
                case 4: return new HinhTamGiac(MauVe, DoDamNet, KieuButVe);
                case 5:
                    {
                        if (IsNewPolygon)
                            dg = new HinhDaGiac();
                        dg.MauVe = this.MauVe;
                        dg.DoDamNet = this.DoDamNet;
                        dg.KieuButVe = this.KieuButVe;
                        return dg;
                    }
                default: return new DuongThang(MauVe, DoDamNet, KieuButVe);
            }
        }
        private void Enabled_True_LoaiHinhCuoi(int ID) //Mở Enabled của hình trước 
        {
            switch (LoaiHinhCuoi)
            {
                case -1:
                    btnHand.Enabled = true;
                    break;
                case 0:
                    btnLine.Enabled = true;
                    break;
                case 1:
                    btnCurve.Enabled = true;
                    break;
                case 2:
                    btnRectangle.Enabled = true;
                    break;
                case 3:
                    btnOval.Enabled = true;
                    break;
                case 4:
                    btnTriangle.Enabled = true;
                    break;
                case 5:
                    btnPolygon.Enabled = true;
                    break;
                default: break;
            }


            LoaiHinhHienTai = ID;
            LoaiHinhCuoi = LoaiHinhHienTai;
            if (LoaiHinhHienTai == -1)
            {
                return;
            }
        }
        private void picMain_Paint(object sender, PaintEventArgs e)
        {
            hinhNenChim = hinhNenNoi.Clone(new Rectangle(0, 0, hinhNenNoi.Width, hinhNenNoi.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb); //sao chép hình nền

            Graphics g = Graphics.FromImage(hinhNenChim);

            if (DSHinh.danhSachHinh.Count > 0)
                DSHinh.Ve(g);                  //vẽ các hình có trong listHinh lên hình nền
            e.Graphics.DrawImageUnscaled(hinhNenChim, 0, 0);    //
        }

        #region Sự kiện chuột
        private void picMain_MouseDown(object sender, MouseEventArgs e)
        {
            IsDaLuu = false;  //khi mouse_down =>có thay đổi=>đánh dấu là chưa lưu
            
            //  label1.Text = "MouseDown trên";
            if (e.Button == MouseButtons.Left)
            {
                //   label2.Text = "MouseDown trên: "+IDhinhHienTai+" "+", isMoving: "+isMoving.ToString();
                if (IsDiChuyen)
                {
                    //đang di chuyển hình
                }
                else
                {
                    if (HinhHienTai == null || HinhHienTai.KiemTraViTri(e.Location) == -1)
                    {

                        HinhHienTai = LayHinhHienTai(LoaiHinhHienTai);
                        IsNewPolygon = false;

                    }
                    if (HinhHienTai != null)    //&& hinhHienTai.loaiHinh!=0)
                    {
                        HinhHienTai.Mouse_Down(e);          //gọi sự kiện mouse_down của hình
                        picMain.Refresh();               //làm mới
                        if (LoaiHinhHienTai != 5) HinhHienTai.VeKhung(picMain.CreateGraphics());      //vẽ 8 hình chữ nhật nhỏ (chấm vuông nhỏ) làm khung xung quanh   
                        DSHinh.danhSachHinh.Insert(DSHinh.danhSachHinh.Count, HinhHienTai);            //thêm hình mới vào list
                    }
                }
            }
            else
                if (e.Button == MouseButtons.Right && LoaiHinhHienTai == 5)
            {
                HinhHienTai.Mouse_Down(e);          //gọi sự kiện mouse_down của hình
                picMain.Refresh();               //làm mới
                HinhHienTai.VeKhung(picMain.CreateGraphics());      //vẽ 8 hình chữ nhật nhỏ (chấm vuông nhỏ) làm khung xung quanh   
                DSHinh.danhSachHinh.Insert(DSHinh.danhSachHinh.Count, HinhHienTai);            //thêm hình mới vào list
                IsNewPolygon = true;
            }
            else
            {
                HinhHienTai = null;

            }

        }

        private void picMain_MouseMove(object sender, MouseEventArgs e)
        {
            {
                //if (LoaiHinhHienTai == -1)
                //    label3.Text = "SizeMove";  //nút hiện tại là SizeMove
                //if (IDhinhHienTai == 0)
                //    label3.Text = "ConTro";     ////nút hiện tại là ConTro
                lblToaDoChuot.Caption = e.Location.X.ToString() + ", " + e.Location.Y.ToString();

                if (LoaiHinhHienTai == -1 && IsDiChuyen == false)  //nút "Vị trí và kích cỡ" đã được bấm
                {

                    if (IsDiChuyen == false)
                    {
                        for (int i = DSHinh.danhSachHinh.ToArray().Length - 1; i >= 0; i--)        //kiểm tra từng hình xem hình nào bị...
                        {

                            int vt = (DSHinh.danhSachHinh.ToArray())[i].KiemTraViTri(e.Location);
                            if (vt == 0)        //...chuột di chuyển trên bề mặt    
                            {
                                HinhHienTai = (DSHinh.danhSachHinh.ToArray())[i];
                                if (e.Button == MouseButtons.Left)      //=> sẽ di chuyển hình này
                                {
                                    Cursor = Cursors.Hand;
                                    HinhHienTai.IsDiChuyen = true;             //cho phép di chuyển
                                    HinhHienTai.IsThayDoiKichThuoc = false;   //không cho phép thay đổi kích thước
                                    IsDiChuyen = true;                        //bật cờ đang di chuyển

                                    //label1.Text = "MouseDown trong " + i + ", isMoVing: " + isMoving.ToString(); ;
                                    //label4.Text = "Đang di chuyển";
                                    //label5.Text = "Được đổi kích thước: " + hinhHienTai.thayDoiKichThuoc.ToString();

                                    btnHand.Enabled = false;        //mượn nút "Chuột" để di chuyển (bản thân nút "Vị trí và kích cỡ" sẽ không di chuyển hình
                                    Enabled_True_LoaiHinhCuoi(-1);

                                    picMain.Refresh();
                                    HinhHienTai.VeKhung(picMain.CreateGraphics());        //vẽ khung
                                    DSHinh.danhSachHinh.RemoveAt(i);                                           //sau khi di chuyển sẽ phát sinh hình mới tại vị trí mới=>xóa hình cũ
                                }
                                else // chuột đi qua mà không bấm
                                {
                                    IsDiChuyen = false;
                                    HinhHienTai.IsDiChuyen = false;
                                    //label4.Text = "Được di chuyển: " + hinhHienTai.diChuyen.ToString();
                                    //label5.Text = "Được đổi kích thước: " + hinhHienTai.thayDoiKichThuoc.ToString();
                                }

                                //label6.Text = "MouseMove trong "+i;

                                Cursor = Cursors.Hand;
                                picMain.Refresh();
                                HinhHienTai.VeKhung(picMain.CreateGraphics());
                                break;
                            }
                            else if (vt > 0) //...chuột chỉ đúng điểm điều khiển (1 trong 8 chấm vuông nhỏ làm khung)   => sẽ thay đổi kích thước hình này
                            {
                                HinhHienTai = (DSHinh.danhSachHinh.ToArray())[i];
                                if (e.Button == MouseButtons.Left)
                                {
                                    HinhHienTai.IsThayDoiKichThuoc = true;        //cho phép thay đổi kích thước
                                    HinhHienTai.IsDiChuyen = false;               //không cho phép di chuyển
                                    IsDiChuyen = true;

                                    //label1.Text = "MouseDown trên ĐK " + vt + " của " + i + ", isMoving: " + isMoving.ToString(); 
                                    //label4.Text = "Được di chuyển: " + hinhHienTai.diChuyen.ToString();
                                    //label5.Text = "Đang thay đổi kích thước";

                                    btnHand.Enabled = false;
                                    Enabled_True_LoaiHinhCuoi(-1);

                                    //label1.Text = hinhHienTai.khuVuc.ToString();

                                    picMain.Refresh();
                                    HinhHienTai.VeHCNDiemDieuKhien(picMain.CreateGraphics(), 5);
                                    DSHinh.danhSachHinh.RemoveAt(i);
                                }
                                else
                                {
                                    IsDiChuyen = false;
                                    HinhHienTai.IsThayDoiKichThuoc = false;

                                    //label4.Text = "Di chuyển: " + hinhHienTai.diChuyen.ToString();
                                    //label5.Text = "Được đổi kích thước: " + hinhHienTai.thayDoiKichThuoc.ToString();
                                }

                                //label6.Text = "MouseMove trên điểm Đk " + vt + " của " + i + ", isMoving: " + isMoving.ToString(); ;

                                Cursor = Cursors.Cross;
                                picMain.Refresh();
                                HinhHienTai.VeHCNDiemDieuKhien(picMain.CreateGraphics(), 5);
                                break;
                            }

                            else //tìm trong danh sách không có hình nào bị chuột đi qua
                            {
                                Cursor = Cursors.Default;
                                // label6.Text = "MouseMove ngoài";
                            }
                        }
                    }
                }
                else  //không phải nút "Vị trí và kích cỡ" => là nút vẽ hình hoặc nút "Chuột"
                {
                    if (HinhHienTai != null)
                    {

                        if (HinhHienTai.KiemTraViTri(e.Location) > 0)   //nếu chuột chỉ đúng 1 trong 8 chấm vuông nhỏ => đổi chuột thành hình dấu +
                            Cursor = Cursors.Cross;

                        else if (HinhHienTai.KiemTraViTri(e.Location) == 0)     //tương tự với lúc chuột nằm trong hình => chuột hình bàn tay
                            Cursor = Cursors.Hand;
                        else
                            Cursor = Cursors.Default;       //còn lại thì mặc định
                    }
                    if (LoaiHinhHienTai == 5)
                    {
                        HinhHienTai = LayHinhHienTai(LoaiHinhHienTai);
                        if (HinhHienTai != null)
                        {
                            //làm nổi hình mới nhất lên (hiện khung hình đó)
                            // label1.Text = "MouseDown ngoài";
                            HinhHienTai.Mouse_Move(e);
                            picMain.Refresh();
                            HinhHienTai.VeKhung(picMain.CreateGraphics());
                        }
                    }
                    if (e.Button == MouseButtons.Left)
                    {

                        if (HinhHienTai != null)
                        {
                            //làm nổi hình mới nhất lên (hiện khung hình đó)
                            // label1.Text = "MouseDown ngoài";
                            HinhHienTai.Mouse_Move(e);
                            picMain.Refresh();
                            HinhHienTai.VeKhung(picMain.CreateGraphics());
                        }
                    }
                }

            }
        }

        private void picMain_MouseUp(object sender, MouseEventArgs e)
        {

            if (HinhHienTai != null && IsDiChuyen == false)
            {
                DSHinh.danhSachHinh.Insert(DSHinh.danhSachHinh.Count, HinhHienTai); //thêm hình mới vào list
                HinhHienTai.Mouse_Up(sender);
                HinhHienTai.VeKhung(picMain.CreateGraphics());

                //string s = IDhinhHienTai + ", "+hinhHienTai.loaiHinh + ", " + hinhHienTai.diemBatDau.X + ", " + hinhHienTai.diemBatDau.Y + ", " + hinhHienTai.diemKetThuc.X + ", " + hinhHienTai.diemKetThuc.Y;
                //SaveLogFile(s);
            }
            if (IsDiChuyen)
            {

                HinhHienTai.Mouse_Up(sender);
                picMain.Refresh();
                btnHand.Enabled = false;
                Enabled_True_LoaiHinhCuoi(-1);
                IsDiChuyen = false;

                //string s = IDhinhHienTai + ", "+hinhHienTai.loaiHinh + ", " + hinhHienTai.diemBatDau.X + ", " + hinhHienTai.diemBatDau.Y + ", " + hinhHienTai.diemKetThuc.X + ", " + hinhHienTai.diemKetThuc.Y;
                //SaveLogFile(s);
            }


        }
        #endregion

        #region Xử lý sự kiện button hình vẽ
        private void btnLine_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            btnLine.Enabled = false;
            Enabled_True_LoaiHinhCuoi(0);
            btnLine.Enabled = false;
        }

        private void btnCurve_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            btnCurve.Enabled = false;
            Enabled_True_LoaiHinhCuoi(1);
            btnCurve.Enabled = false;
        }

        private void btnRectangle_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            btnRectangle.Enabled = false;
            Enabled_True_LoaiHinhCuoi(2);
            btnRectangle.Enabled = false;
        }

        private void btnOval_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            btnOval.Enabled = false;
            Enabled_True_LoaiHinhCuoi(3);
            btnOval.Enabled = false;
        }

        private void btnTriangle_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            btnTriangle.Enabled = false;
            Enabled_True_LoaiHinhCuoi(4);
            btnTriangle.Enabled = false;
        }

        private void btnPolygon_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            btnPolygon.Enabled = false;
            Enabled_True_LoaiHinhCuoi(5);
            btnPolygon.Enabled = false;
        }

        private void btnHand_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            btnHand.Enabled = false;
            Enabled_True_LoaiHinhCuoi(-1);
            btnHand.Enabled = false;
        }
        #endregion

        #region Màu vẽ
        private void barEditColor_EditValueChanged(object sender, EventArgs e)
        {
            MauVe = (Color)barEditColor.EditValue;
            if (HinhHienTai != null)
            {
                HinhHienTai.MauVe = MauVe;
                picMain.Refresh();
                HinhHienTai.VeKhung(picMain.CreateGraphics());
            }
        }

        #endregion

        #region Độ đậm nét
        private void btn1px_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DoDamNet = 1;
            if (HinhHienTai != null)
            {
                HinhHienTai.DoDamNet = DoDamNet;
                picMain.Refresh();
                HinhHienTai.VeKhung(picMain.CreateGraphics());
            }
        }

        private void btn3px_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DoDamNet = 3;
            if (HinhHienTai != null)
            {
                HinhHienTai.DoDamNet = DoDamNet;
                picMain.Refresh();
                HinhHienTai.VeKhung(picMain.CreateGraphics());
            }
        }

        private void btn5px_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DoDamNet = 5;
            if (HinhHienTai != null)
            {
                HinhHienTai.DoDamNet = DoDamNet;
                picMain.Refresh();
                HinhHienTai.VeKhung(picMain.CreateGraphics());
            }
        }

        private void btn8px_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DoDamNet = 8;
            if (HinhHienTai != null)
            {
                HinhHienTai.DoDamNet = DoDamNet;
                picMain.Refresh();
                HinhHienTai.VeKhung(picMain.CreateGraphics());
            }
        }

        #endregion

        #region Kiểu bút vẽ
        private void btnSoild_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            KieuButVe = DashStyle.Solid;
            if (HinhHienTai != null)
            {
                HinhHienTai.KieuButVe = KieuButVe;
                picMain.Refresh();
                HinhHienTai.VeKhung(picMain.CreateGraphics());
            }

        }

        private void btnDot_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            KieuButVe = DashStyle.Dot;
            if (HinhHienTai != null)
            {
                HinhHienTai.KieuButVe = KieuButVe;
                picMain.Refresh();
                HinhHienTai.VeKhung(picMain.CreateGraphics());
            }

        }

        private void btnDash_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            KieuButVe = DashStyle.Dash;
            if (HinhHienTai != null)
            {
                HinhHienTai.KieuButVe = KieuButVe;
                picMain.Refresh();
                HinhHienTai.VeKhung(picMain.CreateGraphics());
            }

        }

        private void btnDashDot_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            KieuButVe = DashStyle.DashDot;
            if (HinhHienTai != null)
            {
                HinhHienTai.KieuButVe = KieuButVe;
                picMain.Refresh();
                HinhHienTai.VeKhung(picMain.CreateGraphics());
            }

        }
        #endregion
    }
}