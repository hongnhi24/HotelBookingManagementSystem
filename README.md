# Hệ thống quản lý đặt phòng khách sạn

Đây là đồ án nhóm được thực hiện nhằm phân tích, thiết kế và xây dựng hệ thống hỗ trợ hoạt động đặt phòng và quản lý khách sạn. Giải pháp gồm website dành cho khách hàng, Web API xử lý dữ liệu và ứng dụng Windows dành cho nhân viên, quản lý khách sạn.

## Chức năng chính

- Quản lý khách hàng, phòng và loại phòng.
- Tìm kiếm phòng trống theo ngày nhận phòng và ngày trả phòng.
- Tạo, xác nhận và theo dõi đơn đặt phòng.
- Hỗ trợ quy trình nhận phòng và trả phòng.
- Ghi nhận các dịch vụ khách hàng sử dụng trong thời gian lưu trú.
- Lập hóa đơn và tổng hợp báo cáo doanh thu.
- Quản lý nhân viên, vai trò, chấm công và tiền lương trên ứng dụng nội bộ.

## Kiến trúc hệ thống

| Thành phần | Mục đích | Công nghệ |
| --- | --- | --- |
| `HotelAPI` | Cung cấp API cho phòng, khách hàng và đơn đặt phòng | ASP.NET Core Web API, .NET 8, Entity Framework Core |
| `WebAppCustomer` | Website dành cho khách hàng tìm kiếm và đặt phòng | ASP.NET Core MVC, .NET 8, Bootstrap |
| `HotelManagementDesktop` | Ứng dụng nội bộ dành cho nhân viên và quản lý | C# Windows Forms, .NET Framework 4.7.2, Entity Framework 6 |
| Cơ sở dữ liệu `HotelManagement` | Lưu trữ dữ liệu nghiệp vụ của hệ thống | Microsoft SQL Server |

## Cấu trúc thư mục

```text
HotelBookingManagement/
|-- HotelAPI/
|-- WebAppCustomer/
|-- HotelManagementDesktop/
|-- Database/
|-- Documentation/
|-- .gitignore
`-- README.md
```

## Yêu cầu môi trường

- Visual Studio 2022 với workload ASP.NET and web development và .NET desktop development.
- .NET 8 SDK.
- .NET Framework 4.7.2 Developer Pack.
- Microsoft SQL Server.
- SQL Server Management Studio.

## Thiết lập cơ sở dữ liệu

Gói mã nguồn ban đầu chưa có file tạo cấu trúc và dữ liệu mẫu cho SQL Server. Trước khi chạy hệ thống, cần tạo hoặc khôi phục cơ sở dữ liệu có tên `HotelManagement`.

Đối với repository công khai, nên bổ sung một file `.sql` đã loại bỏ dữ liệu cá nhân vào thư mục `Database`.

Connection string mặc định dùng cho môi trường phát triển:

```text
Data Source=.;Initial Catalog=HotelManagement;Integrated Security=True;TrustServerCertificate=True
```

Nếu sử dụng tên SQL Server instance khác, cần cập nhật connection string tại:

- `HotelAPI/appsettings.json`
- `HotelManagementDesktop/HotelManagement/App.config`
- `HotelManagementDesktop/HotelManagement/Properties/Settings.settings`

## Chạy Hotel API

Mở Terminal tại thư mục gốc và thực hiện:

```bash
dotnet restore HotelAPI/HotelAPI.csproj
dotnet run --project HotelAPI/HotelAPI.csproj
```

Khi API khởi động thành công, địa chỉ Swagger sẽ được hiển thị trong Terminal hoặc trong file cấu hình chạy của dự án.

## Chạy website khách hàng

Khởi động `HotelAPI` trước, sau đó mở một Terminal khác và chạy:

```bash
dotnet restore WebAppCustomer/WebAppCustomer.csproj
dotnet run --project WebAppCustomer/WebAppCustomer.csproj
```

Nếu địa chỉ hoặc cổng của API thay đổi, cần cập nhật API base URL đang được sử dụng trong các controller của `WebAppCustomer`.

## Chạy ứng dụng quản lý khách sạn

1. Mở file `HotelManagementDesktop/HotelManagement.sln` bằng Visual Studio 2022.
2. Chọn **Restore NuGet Packages** để tải các thư viện cần thiết.
3. Kiểm tra lại connection string kết nối SQL Server.
4. Chọn project `HotelManagement` làm Startup Project.
5. Build và chạy ứng dụng.

## Tài liệu dự án

Báo cáo đồ án được lưu tại:

```text
Documentation/Project_Report.docx
```

## Phần việc cá nhân

Phần đóng góp chính của tôi tập trung vào phân tích nghiệp vụ và module quản lý dịch vụ. Tôi tham gia phân tích quy trình khách sạn, xây dựng use case và các sơ đồ quy trình, xác định quan hệ dữ liệu, triển khai các chức năng liên quan đến dịch vụ, hỗ trợ phân chia công việc và tổng hợp báo cáo nhóm.

## Trạng thái dự án

Đây là đồ án học tập phục vụ mục đích nghiên cứu và xây dựng portfolio. Trước khi sử dụng trong môi trường thực tế, hệ thống cần được hoàn thiện thêm về bảo mật mật khẩu, migration cơ sở dữ liệu, kiểm thử tự động, xử lý lỗi và cấu hình triển khai.

