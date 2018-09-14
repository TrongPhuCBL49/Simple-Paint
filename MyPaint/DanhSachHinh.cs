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
    class DanhSachHinh
    {
        #region Thuộc tính
        public List<Hinh> danhSachHinh;
        #endregion

        #region Khởi tạo
        public DanhSachHinh()
        {
            danhSachHinh = new List<Hinh>();
        }
        #endregion

        #region Phương thức
        // Vẽ
        public void Ve(Graphics g)
        {
            foreach (Hinh hinh in danhSachHinh)
            {
                hinh.Ve(g);
            }
        }

        public void XoaHinhCuoi()
        {
            try
            {
                //for (int i = 0; i < listHinh.Count; i++)
                //    for (int j = i + 1; j < listHinh.Count; j++)
                //        if (listHinh[i].Equals(listHinh[j]))
                //            listHinh.RemoveAt(j);


                danhSachHinh.RemoveAt(danhSachHinh.Count - 1);
            }
            catch
            {
                MessageBox.Show("Khong the xoa!", "Canh bao");
            }
        }

        #endregion
    }
}
