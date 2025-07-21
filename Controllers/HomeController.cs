using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DoAnCNPM.Models;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace DoAnCNPM.Controllers;
using System.Collections;
using System.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;


public class HomeController : Controller
{
    
    private readonly ILogger<HomeController> _logger;
    private readonly ISession _session;

    public HomeController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _session = httpContextAccessor.HttpContext.Session;
    }

    public IActionResult LayoutShare(){
        var taikhoan = _session.GetString("taikhoan");
        ViewData["TaiKhoan"] = taikhoan;

        DataModel db = new DataModel();
        ViewBag.listKB = db.get("EXEC getAllKhoaBenh");

        if (!string.IsNullOrEmpty(taikhoan))
        {
            var MaND = taikhoan;
            ViewBag.ListTB = db.get($"EXEC sp_GetThongBaoByMaND @MaND = {MaND}");
        }
        return View();
    }

    public IActionResult Index()
    {
        LayoutShare();
        DataModel db =new DataModel();
        ViewBag.ListBV = db.get("EXEC getAllBenhVien");
        ViewBag.ListBS5 = db.get("EXEC GetTop7Doctors");
                
        return View();
    }
    public IActionResult Article(string MaBV)
    {
        LayoutShare();

        DataModel db = new DataModel();

        // Sử dụng tham số hóa để tránh lỗi
        ViewBag.ListBV = db.get($"EXEC getBaiVietByIDMaKhoaBenh {MaBV}");

        return View();
    }
    public IActionResult ListArticleLoaiBV(string MaLBV)
    {
        LayoutShare();

        DataModel db = new DataModel();

        // Sử dụng tham số hóa để tránh lỗi
        ViewBag.ListLBV = db.get($"EXEC getBaiVietByIDMaLoai {MaLBV}");

        return View();
    }
    public IActionResult ArticleLoaiBV(string MaBV)
    {
        LayoutShare();

        DataModel db = new DataModel();

        // Sử dụng tham số hóa để tránh lỗi
        ViewBag.ListBV = db.get($"EXEC getBaiVietByID {MaBV}");

        return View();
    }

    
    // ---------- ĐĂNG NHẬP ------------//
    public IActionResult Login()
    {
        LayoutShare();
        return View();
    }
    [HttpPost]
    public IActionResult LoginProcess(string sdt, string password)
    {
        DataModel db = new DataModel();
        var list = db.get("EXEC CheckLogin '" + sdt + "','" + password + "'");

        if (list.Count > 0 && list[0] is ArrayList arrayList && arrayList.Count > 1)
        {
            var userInfo = arrayList[0]?.ToString() ?? "Unknown";
            _session.SetString("taikhoan", userInfo);
            return RedirectToAction("Index", "Home");
        }
        else
        {
            return RedirectToAction("Login", "Home");
        }
    }

     // -------- REGISTER ----------//
    public IActionResult Register()
    {
        LayoutShare();
        return View();
    }
    [HttpPost]
    public IActionResult RegisterProcess(string TenND, string Password, string sdt)
    {
        DataModel db = new DataModel();

        var list = db.get($"EXEC REGISTER N'{TenND}', '{Password}', '{sdt}'");

        if (list.Count > 0 && list[0] is ArrayList arrayList && arrayList.Count >= 2)
        {
            string userName = arrayList[0]?.ToString() ?? "Unknown";
            HttpContext.Session.SetString("taikhoan", userName);

            return RedirectToAction("Index", "Home");
        }
        else
        {
            ViewBag.Error = "Đăng ký không thành công. Vui lòng thử lại.";
            return RedirectToAction("Register", "Home");
        }
    }
    // ------- Action đăng xuất------- //
    public IActionResult Logout()
    {
        // Xóa toàn bộ session
        HttpContext.Session.Clear();

        return RedirectToAction("Index", "Home");
    }
    public IActionResult FillterDoctor()
    {
        LayoutShare();
        DataModel db = new DataModel();
        ViewBag.ListNameDoc = db.get("SELECT MaBS, nd.TenND FROM BACSI bs, NGUOIDUNG nd where bs.MaND = nd.MaND");
        ViewBag.ListDoc = db.get("Exec getAllBacSi");
        
        return View();
    }
    public IActionResult FillterDoctorList(string khuvuc, string phikham, string khoabenh, string hocham)
    {
        LayoutShare();
        DataModel db = new DataModel();
        if(khuvuc != "Null"){
            khuvuc = "N'"+ khuvuc +"'";
        }
        if(hocham != "Null" ){
            hocham = "N'"+ hocham +"'";
        }
        ViewBag.ListDocFill = db.get($"EXEC FILTER_BACSI {khuvuc}, {phikham}, {khoabenh},  {hocham};");
        
        return View();
    }
    public IActionResult DetailDoctor(string MaBS)
    {
        LayoutShare();

        DataModel db = new DataModel();

   
        // Sử dụng tham số hóa để tránh lỗi
        ViewBag.ListDBS = db.get($"EXEC DetDETAILlBACSI {MaBS}");

        ViewBag.ListComment = db.get($"EXEC GetCommentBACSI {MaBS}");
        return View();
    }
    public IActionResult ListDoctor()
    {
        LayoutShare();
        
        return View();
    }

    // ----- PERSONAL PAGE ------//
     public IActionResult PersonalPage()
    {
        LayoutShare();
        DataModel db = new DataModel();
        var taikhoan = HttpContext.Session.GetString("taikhoan");
        ViewData["TaiKhoan"] = taikhoan;

        if (!string.IsNullOrEmpty(taikhoan))
        {
            var result = db.get("SELECT * from NGUOIDUNG where manD=" + taikhoan);
            if (result != null && result.Count > 0)
            {
                ViewBag.UserInfo = result[0]; // Lấy dòng đầu tiên của kết quả
            }
        }
        return View();
    }

    [HttpPost]
    public IActionResult UpdateUserInfo(string MaND, string TenND, string Email, 
                                        string NamSinh, string GioiTinh, 
                                        string DiaChi, IFormFile Hinhcanhan) 
    {
        DataModel db = new DataModel();
        int manD = int.Parse(MaND);
        DateTime parsedDate = DateTime.Parse(NamSinh); 
        string formattedDate = parsedDate.ToString("yyyy-MM-dd");

        string nameFile = "NULL";
        if(Hinhcanhan != null){
            // lấy tên tệp
            nameFile = Path.GetFileName(Hinhcanhan.FileName);

            // Đường dẫn để lưu tệp
            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads");
            Directory.CreateDirectory(uploadsFolder); // Tạo thư mục nếu chưa tồn tại
            string filePath = Path.Combine(uploadsFolder, nameFile);
            // Lưu tệp
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                Hinhcanhan.CopyTo(stream);
            }
        }
 
        db.get($"EXEC UpdateUserInfo {manD}, N'{TenND}', '{Email}', '{formattedDate}', N'{GioiTinh}', N'{DiaChi}', '{nameFile}' ");

        return RedirectToAction("PersonalPage", "Home");
    }

    public IActionResult BookExamine(string MaBS)
    {
        LayoutShare();

        DataModel db = new DataModel();
        var taikhoan = HttpContext.Session.GetString("taikhoan");
        ViewData["TaiKhoan"] = taikhoan;

        if (!string.IsNullOrEmpty(taikhoan))
        {
            var result = db.get("SELECT * from NGUOIDUNG where manD=" + taikhoan);
            if (result != null && result.Count > 0)
            {
                ViewBag.UserInfo = result[0]; // Lấy dòng đầu tiên của kết quả
            }
        }

        ViewBag.ListDBS = db.get($"EXEC DetDETAILlBACSI1 {MaBS}");

        return View();
    }
   [HttpPost]
    public IActionResult BookExamineProcess(string MaBS, string MoTaBenh, List<IFormFile> HinhAnhBenhs)
    {
        DataModel db = new DataModel();

        var taikhoan = HttpContext.Session.GetString("taikhoan");
        ViewData["TaiKhoan"] = taikhoan;
        var MaND = taikhoan;

        var result = db.get($"DECLARE @MaHS INT; EXEC SAVEHOSO {MaND}, N'{MoTaBenh}', @MaHS = @MaHS OUTPUT; SELECT @MaHS;");
        if (result != null && result.Count > 0)
        {
            ViewBag.MaHS = result[0]; // Lấy dòng đầu tiên của kết quả
        }
        var MaHS = int.Parse(ViewBag.MaHS[0].ToString());
        
        foreach (var file in HinhAnhBenhs)
        {
             // lấy tên tệp
            string nameFile = Path.GetFileName(file.FileName);

            // Đường dẫn để lưu tệp
            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads");
            Directory.CreateDirectory(uploadsFolder); // Tạo thư mục nếu chưa tồn tại
            string filePath = Path.Combine(uploadsFolder, nameFile);
            // Lưu tệp
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            
            db.get($"EXEC SAVEHINHANHBENH {MaHS}, '{nameFile}'");
        }

        db.get($"EXEC SAVECUOCHENKHAM {MaND}, {MaBS}, {MaHS}, null");

        return RedirectToAction("Index", "Home");
    }


    public IActionResult ExamineHistory()
    {
        LayoutShare();
        
        var taikhoan = HttpContext.Session.GetString("taikhoan");
        ViewData["TaiKhoan"] = taikhoan;
        var MaND = taikhoan;

        DataModel db = new DataModel();
        ViewBag.ListLichKham = db.get($"EXEC GetLichKhamInfoByMaND {MaND}");


        return View();
    }
    public IActionResult Cancel(string maCHK)
    {
        DataModel db = new DataModel();
        ViewBag.List = db.get("EXEC DeleteCHKbyID " + maCHK);
        
        return RedirectToAction("ExamineHistory", "Home");
    }

    public IActionResult AccountBank()
    {
        LayoutShare();
        DataModel db = new DataModel();
        var taikhoan = HttpContext.Session.GetString("taikhoan");
        ViewData["TaiKhoan"] = taikhoan;

        if (!string.IsNullOrEmpty(taikhoan))
        {
            var result = db.get("SELECT * from NGUOIDUNG where manD=" + taikhoan);
            if (result != null && result.Count > 0)
            {
                ViewBag.UserInfo = result[0]; // Lấy dòng đầu tiên của kết quả
            }
        }
        return View();
    }
    [HttpPost]
    public IActionResult AccountBankProcess(string SoDuTK)
    {
        var taikhoan = HttpContext.Session.GetString("taikhoan");
        ViewData["TaiKhoan"] = taikhoan;
        var MaND = taikhoan;

        DataModel db = new DataModel();
        db.get($"EXEC NapTien {MaND}, {SoDuTK}");

        return RedirectToAction("AccountBank", "Home");
    }
    
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}