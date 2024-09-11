using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebApplication1.Models;
using PagedList;
namespace WebApplication1.Areas.PrivateSite.Controllers
{
    public class BrandController : Controller
    {
        static ShoesShopEntities db = new ShoesShopEntities();
        static bool checkEdit = false;

        // GET: PrivateSite/Brand
        public ActionResult Index()
        {
            return View();
        }

        // Load brands and search by maThuongHieu
        [HttpGet]
        public JsonResult LoadData(int? keyword, int? page, int? pageSize)
        {
            var size = pageSize ?? 2;
            var pageIndex = page ?? 1;

            try
            {
                // Search by maThuongHieu if keyword is provided
                var brands = db.ThuongHieux
                    .Where(p => keyword == null || p.maThuongHieu == keyword)
                    .Select(d => new
                    {
                        d.maThuongHieu,
                        d.tenThuongHieu,
                        d.moTa
                    })
                    .ToList();

                // Pagination
                var totalPage = brands.Count;
                var numberPage = Math.Ceiling((float)totalPage / size);

                var start = (pageIndex - 1) * size;
                brands = brands.Skip(start).Take(size).ToList();

                return Json(new { status = true, Data = brands, CurrentPage = pageIndex, TotalItem = totalPage, NumberPage = numberPage, PageSize = size, message = "Đang load" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = false, CurrentPage = pageIndex, TotalItem = 0, NumberPage = 0, PageSize = size, message = "Tải dữ liệu lên thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }

        // Add or edit brand
        [HttpPost]
        public JsonResult SaveBrand(ThuongHieu thuongHieu)
        {
            if (!string.IsNullOrEmpty(thuongHieu.tenThuongHieu) && thuongHieu.maThuongHieu != 0)
            {
                if (checkEdit)
                {
                    // Update brand if editing
                    ThuongHieu existingBrand = db.ThuongHieux.Find(thuongHieu.maThuongHieu);
                    if (existingBrand != null)
                    {
                        existingBrand.tenThuongHieu = thuongHieu.tenThuongHieu;
                        existingBrand.moTa = thuongHieu.moTa;
                        checkEdit = false;
                        db.SaveChanges();
                        return Json(new { status = true, message = "Đã sửa thành công" });
                    }
                    return Json(new { status = false, message = "Không tìm thấy thương hiệu để sửa" });
                }
                else
                {
                    // Add new brand
                    db.ThuongHieux.Add(thuongHieu);
                    db.SaveChanges();
                    return Json(new { status = true, message = "Đã thêm thương hiệu thành công" });
                }
            }
            return Json(new { status = false, message = "Vui lòng nhập đầy đủ thông tin" });
        }

        // Get brand by maThuongHieu for editing
        [HttpGet]
        public JsonResult Edit(int id)
        {
            checkEdit = false;

            // Truy xuất dữ liệu từ cơ sở dữ liệu, kiểm tra thương hiệu với maThuongHieu tương ứng
            var brand = db.ThuongHieux
                          .Where(m => m.maThuongHieu == id)  // Kiểm tra điều kiện trước
                          .Select(d => new { d.tenThuongHieu, d.maThuongHieu, d.moTa })  // Lấy các trường cần thiết
                          .FirstOrDefault();

            // Nếu tìm thấy thương hiệu, trả về JSON với dữ liệu
            if (brand != null)
            {
                checkEdit = true;
                return Json(new { status = true, data = brand }, JsonRequestBehavior.AllowGet);
            }

            // Nếu không tìm thấy thương hiệu, trả về thông báo lỗi
            return Json(new { status = false, message = "Không tìm thấy thương hiệu" }, JsonRequestBehavior.AllowGet);
        }


        // Delete brand by maThuongHieu
        [HttpPost]
        public JsonResult Delete(int id)
        {
            ThuongHieu brand = db.ThuongHieux.FirstOrDefault(m => m.maThuongHieu == id);
            if (brand != null)
            {
                string name = brand.tenThuongHieu;
                db.ThuongHieux.Remove(brand);
                db.SaveChanges();
                return Json(new { status = true, message = "Đã xóa thành công thương hiệu: " + name });
            }
            return Json(new { status = false, message = "Không tìm thấy thương hiệu để xóa" });
        }
    }
}
