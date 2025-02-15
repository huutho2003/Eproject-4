﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using QLKS.Models;

namespace QLKS.Controllers.Home
{
    public class AccountController : Controller
    {
        private dataQLKSEntities db = new dataQLKSEntities();
        // GET: KhachHang
        public ActionResult Index()
        {
            return View(db.tblKhachHangs.ToList());
        }

        // GET: KhachHang/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblKhachHang tblKhachHang = db.tblKhachHangs.Find(id);
            if (tblKhachHang == null)
            {
                return HttpNotFound();
            }
            return View(tblKhachHang);
        }

        // GET: KhachHang/Create
        public ActionResult Register()
        {
            return View();
        }

        // POST: KhachHang/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "ma_kh,mat_khau,ho_ten,cmt,sdt,mail")] tblKhachHang tblKhachHang)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrWhiteSpace(tblKhachHang.ma_kh) ||
                    string.IsNullOrWhiteSpace(tblKhachHang.mat_khau) ||
                    string.IsNullOrWhiteSpace(tblKhachHang.ho_ten) ||
                    string.IsNullOrWhiteSpace(tblKhachHang.cmt) ||
                    string.IsNullOrWhiteSpace(tblKhachHang.sdt) ||
                    string.IsNullOrWhiteSpace(tblKhachHang.mail))
                {
                    ModelState.AddModelError("", "Vui lòng nhập đầy đủ thông tin.");
                    return View(tblKhachHang);
                }
                if (db.tblKhachHangs.Find(tblKhachHang.ma_kh) == null)
                {
                    // Check password length
                    if (tblKhachHang.mat_khau.Length < 6)
                    {
                        ModelState.AddModelError("mat_khau", "Mật khẩu phải chứa ít nhất 6 kí tự.");
                        return View(tblKhachHang);
                    }

                    // Check email format
                    if (!tblKhachHang.mail.EndsWith("@gmail.com"))
                    {
                        ModelState.AddModelError("mail", "Email phải kết thúc bằng '@gmail.com'.");
                        return View(tblKhachHang);
                    }

                    // Check phone number starts with 0 and has at least 9 digits
                    if (!Regex.IsMatch(tblKhachHang.sdt, "^0\\d{8,}$"))
                    {
                        ModelState.AddModelError("sdt", "Số điện thoại phải bắt đầu bằng số 0 và có ít nhất 9 chữ số.");
                        return View(tblKhachHang);
                    }

                    // Check CMT has at least 9 digits
                    if (!Regex.IsMatch(tblKhachHang.cmt, "^\\d{9,}$"))
                    {
                        ModelState.AddModelError("cmt", "Số CMT phải có ít nhất 9 chữ số.");
                        return View(tblKhachHang);
                    }

                    db.tblKhachHangs.Add(tblKhachHang);
                    db.SaveChanges();
                    Session["KH"] = tblKhachHang;
                    return RedirectToAction("BookRoom", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Tên tài khoản đã được sử dụng !");
                }
            }

            return View(tblKhachHang);
        }


        public ActionResult Add()
        {
            return View();
        }

        // POST: KhachHang/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add([Bind(Include = "ma_kh,mat_khau,ho_ten,cmt,sdt,mail")] tblKhachHang tblKhachHang)
        {
            if (ModelState.IsValid)
            {
                if (db.tblKhachHangs.Find(tblKhachHang.ma_kh) == null)
                {
                    db.tblKhachHangs.Add(tblKhachHang);
                    db.SaveChanges();
                    return RedirectToAction("FindRoom", "Admin");
                }
                else
                {
                    ModelState.AddModelError("", "Login data is incorrect!");
                }
            }

            return View(tblKhachHang);
        }

        // GET: KhachHang/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblKhachHang tblKhachHang = db.tblKhachHangs.Find(id);
            if (tblKhachHang == null)
            {
                return HttpNotFound();
            }
            return View(tblKhachHang);
        }

        // POST: KhachHang/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ma_kh,mat_khau,ho_ten,cmt,sdt,mail,diem")] tblKhachHang tblKhachHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblKhachHang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblKhachHang);
        }


        public ActionResult CaNhan()
        {
            tblKhachHang kh = new tblKhachHang();
            if (Session["KH"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                kh = (tblKhachHang)Session["KH"];
            }
            return View(kh);
        }

        // POST: KhachHang/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CaNhan([Bind(Include = "ma_kh,mat_khau,ho_ten,cmt,sdt,mail,diem")] tblKhachHang tblKhachHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblKhachHang).State = EntityState.Modified;
                db.SaveChanges();
                Session["KH"] = tblKhachHang;
                return RedirectToAction("Index", "Home");
            }
            return View(tblKhachHang);
        }

        // GET: KhachHang/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblKhachHang tblKhachHang = db.tblKhachHangs.Find(id);
            if (tblKhachHang == null)
            {
                return HttpNotFound();
            }
            return View(tblKhachHang);
        }

        // POST: KhachHang/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            try
            {
                tblKhachHang tblKhachHang = db.tblKhachHangs.Find(id);
                db.tblKhachHangs.Remove(tblKhachHang);
                db.SaveChanges();
            }
            catch
            {

            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(tblKhachHang objUser)
        {
            if (ModelState.IsValid)
            {
                var obj = db.tblKhachHangs.Where(a => a.ma_kh.Equals(objUser.ma_kh) && a.mat_khau.Equals(objUser.mat_khau)).FirstOrDefault();
                if (obj != null)
                {
                    Session["KH"] = obj;
                    return RedirectToAction("BookRoom", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Login data is incorrect!");
                }
            }
            return View(objUser);
        }
        [HttpGet]
        public ActionResult Login()
        {
            Session["KH"] = null;
            tblKhachHang kh = (tblKhachHang)Session["KH"];
            if (kh != null)
                return RedirectToAction("BookRoom", "Home");
            return View();
        }




        public ActionResult SuaPhieuDatPhong(int? id)
        {
            tblKhachHang kh = new tblKhachHang();
            if (Session["KH"] != null)
                kh = (tblKhachHang)Session["KH"];
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblPhieuDatPhong tblPhieuDatPhong = db.tblPhieuDatPhongs.Find(id);
            if (tblPhieuDatPhong == null)
            {
                return HttpNotFound();
            }
            if (tblPhieuDatPhong.ma_kh != kh.ma_kh)
                return RedirectToAction("Index", "Home");
            ViewBag.ma_kh = new SelectList(db.tblKhachHangs, "ma_kh", "mat_khau", tblPhieuDatPhong.ma_kh);
            ViewBag.ma_phong = new SelectList(db.tblPhongs, "ma_phong", "so_phong", tblPhieuDatPhong.ma_phong);
            ViewBag.ma_tinh_trang = new SelectList(db.tblTinhTrangPhieuDatPhongs, "ma_tinh_trang", "tinh_trang", tblPhieuDatPhong.ma_tinh_trang);
            return View(tblPhieuDatPhong);
        }

        // POST: PhieuDatPhong/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SuaPhieuDatPhong([Bind(Include = "ma_pdp,ma_kh,ngay_dat,ngay_vao,ngay_ra,ma_phong,ma_tinh_trang")] tblPhieuDatPhong tblPhieuDatPhong)
        {
            if (ModelState.IsValid)
            {
                tblPhieuDatPhong.ma_tinh_trang = 1;
                db.Entry(tblPhieuDatPhong).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("BookRoom", "Home");
            }
            ViewBag.ma_kh = new SelectList(db.tblKhachHangs, "ma_kh", "mat_khau", tblPhieuDatPhong.ma_kh);
            ViewBag.ma_phong = new SelectList(db.tblPhongs, "ma_phong", "so_phong", tblPhieuDatPhong.ma_phong);
            ViewBag.ma_tinh_trang = new SelectList(db.tblTinhTrangPhieuDatPhongs, "ma_tinh_trang", "tinh_trang", tblPhieuDatPhong.ma_tinh_trang);
            return RedirectToAction("BookRoom", "Home");
        }

        // GET: PhieuDatPhong/Delete/5
        public ActionResult XoaPhieuDatPhong(int? id)
        {
            tblKhachHang kh = new tblKhachHang();
            if (Session["KH"] != null)
                kh = (tblKhachHang)Session["KH"];
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            tblPhieuDatPhong tblPhieuDatPhong = db.tblPhieuDatPhongs.Find(id);
            if (tblPhieuDatPhong == null)
            {
                return HttpNotFound();
            }
            if (tblPhieuDatPhong.ma_kh != kh.ma_kh)
                return RedirectToAction("Index", "Home");
            return View(tblPhieuDatPhong);
        }

        // POST: PhieuDatPhong/Delete/5
        [HttpPost, ActionName("XoaPhieuDatPhong")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmXoaPhieuDatPhong(int id)
        {
            tblPhieuDatPhong tblPhieuDatPhong = db.tblPhieuDatPhongs.Find(id);
            tblPhieuDatPhong.ma_tinh_trang = 3;
            db.Entry(tblPhieuDatPhong).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("BookRoom", "Home");
        }

        public ActionResult Logout()
        {
            Session["KH"] = null;
            return RedirectToAction("Login", "Account");
        }






        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult HoaDon()
        {
            tblKhachHang kh = new tblKhachHang();
            if (Session["KH"] != null)
                kh = (tblKhachHang)Session["KH"];
            else
                return RedirectToAction("Index", "Home");

            var dsHoaDon = db.tblHoaDons.Where(t => t.tblPhieuDatPhong.ma_kh == kh.ma_kh).ToList();

            foreach (var hoaDon in dsHoaDon)
            {
                DateTime ngay_ra = DateTime.Now;
                DateTime ngay_vao = (DateTime)hoaDon.tblPhieuDatPhong.ngay_vao;
                DateTime ngay_du_kien = (DateTime)hoaDon.tblPhieuDatPhong.ngay_ra;

                DateTime dateS = new DateTime(ngay_vao.Year, ngay_vao.Month, ngay_vao.Day, 12, 0, 0);
                DateTime dateE = new DateTime(ngay_ra.Year, ngay_ra.Month, ngay_ra.Day, 12, 0, 0);

                double gia = (double)hoaDon.tblPhieuDatPhong.tblPhong.tblLoaiPhong.gia;

                var so_ngay = (dateE - dateS).TotalDays;
                if (dateS > ngay_vao)
                    so_ngay++;
                if (ngay_ra > dateE)
                    so_ngay++;

                var ti_le_phu_thu = hoaDon.tblPhieuDatPhong.tblPhong.tblLoaiPhong.ti_le_phu_thu;
                var so_ngay_phu_thu = (int)Math.Abs(Math.Ceiling((ngay_ra - ngay_du_kien).TotalDays));

                var phu_thu = so_ngay_phu_thu * gia * ti_le_phu_thu / 100;
                hoaDon.phu_thu = phu_thu;

                var tien_phong = so_ngay * gia;
                hoaDon.tien_phong = tien_phong;

                double tong_tien_dich_vu = 0;
                List<double> list_tt = new List<double>();
                foreach (var dichVu in hoaDon.tblDichVuDaDats)
                {
                    double tien_dich_vu = (double)(dichVu.so_luong * dichVu.tblDichVu.gia);
                    tong_tien_dich_vu += tien_dich_vu;
                    list_tt.Add(tien_dich_vu);
                }

                hoaDon.tien_dich_vu = tong_tien_dich_vu;
                hoaDon.tong_tien = tien_phong + tong_tien_dich_vu + phu_thu;
            }

            return View(dsHoaDon);
        }

        public ActionResult PhieuDatPhong()
        {
            AutoHuyPhieuDatPhong();
            tblKhachHang kh = new tblKhachHang();
            if (Session["KH"] != null)
                kh = (tblKhachHang)Session["KH"];
            else
                return RedirectToAction("Index", "Home");

            var dsPDP = db.tblPhieuDatPhongs.Where(t => t.ma_kh == kh.ma_kh).ToList();
            return View(dsPDP);
        }
        private void AutoHuyPhieuDatPhong()
        {
            var datenow = DateTime.Now;
            var tblPhieuDatPhongs = db.tblPhieuDatPhongs.Where(u => u.ma_tinh_trang == 1).Include(t => t.tblKhachHang).Include(t => t.tblPhong).Include(t => t.tblTinhTrangPhieuDatPhong).ToList();
            foreach (var item in tblPhieuDatPhongs)
            {
                System.Diagnostics.Debug.WriteLine((item.ngay_vao - datenow).Value.Days);
                if ((item.ngay_vao - datenow).Value.Days < 0)
                {
                    item.ma_tinh_trang = 3;
                    db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }
        public ActionResult ChiTietHoaDon(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblHoaDon tblHoaDon = db.tblHoaDons.Find(id);
            if (tblHoaDon == null)
            {
                return HttpNotFound();
            }
            DateTime ngay_ra = DateTime.Now;
            DateTime ngay_vao = (DateTime)tblHoaDon.tblPhieuDatPhong.ngay_vao;
            DateTime ngay_du_kien = (DateTime)tblHoaDon.tblPhieuDatPhong.ngay_ra;

            DateTime dateS = new DateTime(ngay_vao.Year, ngay_vao.Month, ngay_vao.Day, 12, 0, 0);
            DateTime dateE = new DateTime(ngay_ra.Year, ngay_ra.Month, ngay_ra.Day, 12, 0, 0);

            Double gia = (Double)tblHoaDon.tblPhieuDatPhong.tblPhong.tblLoaiPhong.gia;

            var songay = (dateE - dateS).TotalDays;
            if (dateS > ngay_vao)
                songay++;
            if (ngay_ra > dateE)
                songay++;
            var ti_le_phu_thu = tblHoaDon.tblPhieuDatPhong.tblPhong.tblLoaiPhong.ti_le_phu_thu;
            var so_ngay_phu_thu = Math.Abs(Math.Ceiling((ngay_ra - ngay_du_kien).TotalDays));

            System.Diagnostics.Debug.WriteLine("So ngay: " + so_ngay_phu_thu);
            System.Diagnostics.Debug.WriteLine("Gia: " + gia);
            System.Diagnostics.Debug.WriteLine("Ti le: :" + ti_le_phu_thu);

            var phuthu = so_ngay_phu_thu * gia * ti_le_phu_thu / 100;
            ViewBag.phu_thu = phuthu;

            System.Diagnostics.Debug.WriteLine("Phu thu:" + phuthu);

            ViewBag.so_ngay_phu_thu = so_ngay_phu_thu;
            var tien_phong = songay * gia;
            ViewBag.tien_phong = tien_phong;
            ViewBag.so_ngay = songay;

            tblNhanVien nv = (tblNhanVien)Session["NV"];
            if (nv != null)
            {
                ViewBag.ho_ten = nv.ho_ten;
            }
            List<tblDichVuDaDat> dsdv = db.tblDichVuDaDats.Where(u => u.ma_hd == id).ToList();
            ViewBag.list_dv = dsdv;
            double tongtiendv = 0;
            List<double> tt = new List<double>();
            foreach (var item in dsdv)
            {
                double t = (double)(item.so_luong * item.tblDichVu.gia);
                tongtiendv += t;
                tt.Add(t);
            }
            ViewBag.list_tt = tt;
            ViewBag.tien_dich_vu = tongtiendv;
            ViewBag.tong_tien = tien_phong + tongtiendv + phuthu;
            return View(tblHoaDon);
        }
    }
}
