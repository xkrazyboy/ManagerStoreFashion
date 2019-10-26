# ManagerStoreFashion
Quản lý cửa hàng thời trang

WPF - MVVM 

Lê Doãn Cường 15520077

Lưu Nguyễn Khải Hoàn 15520249


-------------------------
B1: Cài đặt database:
    - Chạy file QuanLyKho by hand.sql trong thu mục Database
    - Execute tạo database

B2: Chạy chương trình trên Visual Studio:
    - Cài đặt các gói Package:
        Nhấn vào Tools -> Nuget Package Manager -> Package Manager Console -> Lần lượt gõ từng dòng dưới đây:
            
            Install-Package MaterialDesignThemes

            Install-Package System.Windows.Interactivity.WPF
    
    - Kết nối database:
        + Vào Server Explorer -> Connect to Database -> Change -> Microsoft MQL Server -> Server name là tên server bạn sử dụng ở B1 -> Chọn database bạn đã Excute ở B1 (QuanLyCuaHangThoiTrang)
        + Vào App.config thay đổi đường dẫn tới Database đã tạo
    
    - Rebuild Solution -> Run -> Nhập tài khoản và mật khẩu là "admin" 
