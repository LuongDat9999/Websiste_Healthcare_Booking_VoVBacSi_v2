# VoVBacSi v2 — Hệ thống Đặt lịch & Tư vấn Sức khỏe (ASP.NET Core MVC + AI NLP)

Ứng dụng web hỗ trợ **đặt lịch khám**, **quản lý bác sĩ/bệnh nhân**, **tra cứu thông tin**, và có **mô-đun AI (NLP)** dự đoán bệnh từ mô tả triệu chứng tiếng Việt (tích hợp từ service Python chạy bằng FastAPI).
deploy in: [vovbacsisv.somee.com](http://vovbacsisv.somee.com)
---

## Giới thiệu
**VoVBacSi v2** là hệ thống đặt lịch khám trực tuyến, tối ưu trải nghiệm người dùng và tích hợp phần **NLP dự đoán bệnh** từ mô tả triệu chứng. Ứng dụng đáp ứng các vai trò:
- **Bệnh nhân**: Tìm bác sĩ, đặt lịch, quản lý lịch hẹn, nhận gợi ý bệnh tham khảo.
- **Bác sĩ**: Quản lý lịch khám, hồ sơ bệnh nhân, cập nhật thông tin chuyên khoa.
- **Quản trị**: Quản lý người dùng, chuyên khoa, phòng ban, cấu hình hệ thống.

---

## Kiến trúc & Công nghệ
- **Backend**: ASP.NET Core MVC (.NET 8, C#)
- **View layer**: Razor Views (HTML/CSS/JS)
- **CSDL**: SQL Server (script: `sql_vov.sql`)
- **AI/NLP**: Service Python chạy bằng **FastAPI** (microservice) — dự đoán bệnh tiếng Việt
- **Static assets**: `wwwroot/` (CSS/JS/Images)
- **Triển khai**: IIS / Docker / Azure App Service (tùy chọn)

---

## Tính năng chính
- Đăng ký/đăng nhập, phân quyền (bệnh nhân/bác sĩ/quản trị)
- Tìm kiếm & đặt lịch khám theo chuyên khoa/bác sĩ/thời gian
- Quản lý hồ sơ, lịch hẹn, thông báo
- Quản lý danh mục (bác sĩ, khoa, lịch làm việc, phòng khám)
- Import/Export dữ liệu cơ bản (tùy chọn)
- Quản lý, thống kê, xác nhận lịch hẹn khám phía bác sĩ
- Quản lý, thống kê, xác nhận đăng ký bác sĩ, quản lý thông tin website ở phía admin
- **Dự đoán bệnh** từ mô tả triệu chứng tiếng Việt (sử dụng Pre-Train VihealthBert)
- 
--- 

## Mô-đun AI dự đoán bệnh (NLP)

- **Kiểu**: Microservice Python với FastAPI
- **Chức năng**: Nhận mô tả triệu chứng tiếng Việt, trả về danh sách bệnh khả dĩ + xác suất
- **Mô hình được huấn luyện từ**: [Pretrain_VihealthBert_DiseaseClassifier](https://github.com/LuongDat9999/Pretrain_VihealthBert_DiseaseClassifier)


---

## Nhược điểm
- Giao diện chưa tối ưu cho thiết bị di động.
- Chưa có tính năng nhắc lịch qua email/SMS.
- Mô hình NLP còn phụ thuộc vào dữ liệu huấn luyện ban đầu, chưa liên tục cập nhật.
- Việc triển khai NLP tách service đòi hỏi cấu hình mạng & bảo mật bổ sung.
- Chưa hỗ trợ đa ngôn ngữ (chỉ tiếng Việt).

---
## Hướng phát triển trong tương lai
- **Cải thiện giao diện**: Responsive hoàn toàn trên mobile/tablet.
- **Thông báo & nhắc lịch**: Email/SMS/Push notification.
- **NLP nâng cao**: Huấn luyện thêm dữ liệu y tế mới, cải thiện độ chính xác.
- **Đa ngôn ngữ**: Hỗ trợ tiếng Anh, tiếng Pháp...
- **Phân tích dữ liệu**: Dashboard thống kê, báo cáo cho quản trị.
- **Tích hợp thanh toán trực tuyến**: Thanh toán phí khám/bảo hiểm ngay trên hệ thống.
- **Tích hợp lịch làm việc thực tế**: Đồng bộ với Google Calendar / Outlook.

---

## Yêu cầu hệ thống
- **.NET SDK**: .NET 8
- **SQL Server**: 2019+ (Express/Developer)
- **Node.js** (nếu build asset front-end)
- **Python 3.8+** (nếu chạy NLP service riêng)
- Hệ điều hành: Windows / Linux / macOS

---
## Cấu trúc thư mục

```
Controllers/
    AppointmentController.cs     // Quản lý lịch hẹn
    DoctorController.cs          // Quản lý bác sĩ
    PatientController.cs         // Quản lý bệnh nhân
    HomeController.cs            // Trang chủ
    AccountController.cs         // Đăng nhập, đăng ký

Models/
    Appointment.cs               // Entity lịch hẹn
    Doctor.cs                    // Entity bác sĩ
    Patient.cs                   // Entity bệnh nhân
    Specialty.cs                 // Entity chuyên khoa
    User.cs                      // Entity người dùng

Views/
    Appointment/
        Index.cshtml              // Danh sách lịch hẹn
        Create.cshtml             // Form tạo lịch hẹn
        Edit.cshtml               // Form chỉnh sửa lịch hẹn
    Doctor/
        Index.cshtml              // Danh sách bác sĩ
        Details.cshtml            // Chi tiết bác sĩ
    Patient/
        Index.cshtml              // Danh sách bệnh nhân
        Details.cshtml            // Chi tiết bệnh nhân
    Shared/
        _Layout.cshtml            // Layout chung
        _ValidationScriptsPartial.cshtml // Script validation

wwwroot/
    css/
        site.css                  // CSS mặc định
        custom.css                // CSS tùy chỉnh
    js/
        site.js                   // Script chung
        booking.js                // Script đặt lịch
    images/
        logo.png                   // Logo trang
        banner.jpg                 // Banner trang chủ

Properties/
    launchSettings.json           // Cấu hình khi debug

.vscode/
    settings.json                 // Cấu hình VSCode

DoAnCNPM.sln                      // Solution file
DoAnCNPM.csproj                   // Project file
Program.cs                        // Khởi tạo ứng dụng
appsettings.json                  // Cấu hình kết nối và thông số
appsettings.Development.json      // Cấu hình môi trường dev
sql_vov.sql                       // Script dựng cơ sở dữ liệu
```
---


