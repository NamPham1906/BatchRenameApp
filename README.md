# **Đồ Án Batch Rename**
![Alt Text](https://github.com/NamPham1906/BatchRenameApp/blob/khoa/ReadmeImage/app.png?raw=true)


## **Nội dung**  
[**Điểm số mong muốn**](#score)  



## **Cách chạy chương trình**
1. Chuẩn bị file DLL của các Luật
- 
2. Chạy chương trình Batch Rename
- 
## **Thông tin nhóm**
* * *
Nhóm gồm bốn thành viên
- **Nguyễn Hồ Diệu Hương, MSSV: 19120524**
- **Lê Trần Đăng Khoa, MSSV: 19120546**
- **Lê Nguyễn Thảo Mi, MSSV: 19120576**
- **Phạm Sơn Nam, MSSV: 19120596**

Khi có vấn đề về project, thầy có thể liên lạc với nhóm em thông qua Email: <19120546@student.hcmus.edu.vn>
## **Chức năng đã thực hiện**
* * *
### Yêu cầu cốt lỗi
1. Các rule được load từ file DLL
2. Có thể chọn File, Folder muốn đổi tên
3. Tạo được danh sách các Rule 
    1. Các rule được thêm vào từ một menu
    2. Mỗi rule có giao diện riêng để chỉnh sửa thông số
4. Các Rule được áp dụng để đổi tên theo thứ tự từ trên xuống dưới
5. Có thể lưu các Rule thành các preset để có thể tái sử dụng nhanh chóng.

## **Chức năng chưa thực hiện**
* * *
**Không**
## **Chức năng cộng thêm**
* * *
### **Tất cả improvement mà thầy gọi ý đều đã được thực hiện:** 
1. Kéo thả các File, Folder
2. Lưu trữ dữ liệu bằng file JSON
3. Chỉ cần thêm một thư mục, thì tất cả các file của thư mục đó sẽ được thêm vào danh sách file
4. Có xử lý lỗi khi tên file, folder bị trùng
5. Khi đóng ứng dụng, chương trình sẽ tự động lưu trạng thái cuối cùng của ứng dụng 
    1. Kích thước cửa sổ ứng dụng
    2. Vị trí cửa sổ ứng dựng trên màn hình
    3. Preset hoặc danh sách các rule cuối cùng được chọn
    4. Danh sách file, folder đang thao tác.
6. Có chức năng auto save lại project khi ứng dụng bị tắt đột ngột(ví dụ như mất điện)
    1. Kích thước cửa sổ ứng dụng
    2. Vị trí cửa sổ ứng dựng trên màn hình
    3. Preset hoặc danh sách các rule cuối cùng được chọn
    4. Danh sách file, folder đang thao tác.
    5. Project được auto-save mỗi phút.
7. Sử dụng Regular Expression để kiểm tra tính hợp lệ của tên file, tên folder và Regex còn được áp dụng để xử lý chuỗi trong một số Rule
8. Có chức năng kiểm tra tính hợp lệ của tên file, thư mục
9. Có chức năng lưu trạng thái làm việc hiện tại vào các project(dưới dạng file json), người dùng có thể mở lại các project này để làm việc tiếp tục. 
10. Người dùng có thể xem trước được kết quả của việc đổi tên file, folder
11. Ngoài chức năng đổi tên trên chính file/folder gốc, thì có thể chọn chức năng copy toàn bộ file/folder sang nơi khác rồi đổi tên.
### **Các improvement khác:**
1. Có thêm một số luật đổi tên mới:
    1. **Strip**: Dùng để các các kí tự mong muốn ra khỏi tên 
    2. **Remove Accent**: Chuyển tiếng Việt có dấu thành không dấu
    3. **Add Alphabet Counter**: thêm Couter bằng chữ cái(a, b, c, ..., z, aa, bb, ..., zz...) vào cuối tên file
    4. **Convert to uppercase**: Viết hoa tất cả các chữ cái trong tên
    5. **Convert to camelCase**: Chuyển tên file/folder theo dạng camelCase
    6. **Change File Name**: Đổi hoàn toàn tên file/folder
    7. **Replace Character**: Đổi kí tự bất kì trong tên file/folder thành kí tự khác
2. Sử dụng thư viện HandyControl (https://hosseini.ninja/handycontrol/) để giao diện của ứng dụng trở nên đẹp và thân thiện với người dùng hơn
3. Có các chức năng như new, save, save as, open để tạo mới, lưu, tạo bản sao và mở các project.
4. Danh sách các file/folder đều được phân trang
5. Giao diện ứng dụng được thiết kế responsive
## **Điểm số mong muốn** <a name="score"></a>
* * *
Điểm số mong muốn của từng thành viên:
- Nguyễn Hồ Diệu Hương: **10**
- Lê Trần Đăng Khoa: **10**
- Lê Nguyễn Thảo Mi: **10**
- Phạm Sơn Nam: **10**
## **Video demo**
* * *
<https://youtube.com>
