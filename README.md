# **Đồ Án Batch Rename**

## **Mục lục**
### [Cách chạy chương trình](#run)  
### [Thông tin nhóm](#team)  
### [Chức năng đã thực hiện](#done)  
### [Chức năng chưa thực hiện](#notdone)  
### [Chức năng cộng thêm](#other)
### [Điểm số mong muốn](#score)
### [Video demo](#video)

## **Cách chạy chương trình**<a name="run"></a>
1. **Chuẩn bị file DLL của các Luật**
    - Build toàn bộ solution để có được file dll của các luật
    - Copy các File DLL đặt vào folder DLL của folder chứa file exe hoặc chạy ứng dụng và sử dụng chức năng Browse để thêm các file dll muốn sử dụng.
    - Thầy có thể dùng các DLL nhóm em đã chuẩn bị sẵn trong folder Release
2. **Chạy chương trình Batch Rename**
    - Run project ở chế độ release hay debug đều được
3. **Release**:
    File exe của chương trình đi kèm với 4 folder:
    - DLL: Chức file dll của các luật và dll của Contract.
    - image: folder chứa các ảnh mà chương trình sử dụng.
    - LastProject: folder chứa thông tin về project cuối cùng mà người dùng mở trước khi đóng ứng dụng.
    - PRESET: folder chứa các preset
## **Thông tin nhóm**<a name="team"></a>
* * *
Nhóm gồm bốn thành viên
- **Nguyễn Hồ Diệu Hương, MSSV: 19120524**
- **Lê Trần Đăng Khoa, MSSV: 19120546**
- **Lê Nguyễn Thảo Mi, MSSV: 19120576**
- **Phạm Sơn Nam, MSSV: 19120596**

Khi có vấn đề về project, thầy có thể liên lạc với nhóm em thông qua Email: <19120546@student.hcmus.edu.vn>
## **Chức năng đã thực hiện**<a name="done"></a>
* * *
### Yêu cầu cốt lõi
1. Các rule được load từ file DLL.

2. Có thể chọn File, Folder muốn đổi tên.

3. Tạo được danh sách các Rule:
    1. Các rule được thêm vào từ một menu
    2. Mỗi rule có giao diện riêng để chỉnh sửa thông số
4. Các Rule được áp dụng để đổi tên theo thứ tự từ trên xuống dưới.
5. Có thể lưu các Rule thành các preset để có thể tái sử dụng nhanh chóng.
### Các luật cơ bản
1. Đổi extension của file

2. Thêm Couter vào tên file
    - Có thể tự chọn Giá trị bắt đầu, bước nhảy và số lượng chữ số của Couter
    
    - Ngoài ra nhóm em còn thêm một số chức năng:
        - Chọn thêm Couter vào đầu hoặc cuối file

        - Có thể tạo Seperator giữa tên file và Couter
3. Xóa khoảng trắng khỏi đầu hoặc cuối tên file:
    - Người dùng có thể chọn xóa ở đầu tên file, cuối tên file hoặc cả đầu và cuối tên file
4. Thay đổi một chuỗi kí tự thành chuỗi các kí tự khác
5. Thêm prefix vào đầu file
6. Thêm suffix vào cuối file
7. Chuyển các kí tự viết hoa thành viết thường, đồng thời xóa mọi khoảng trắng
8. Đổi tên file theo dạng PascalCase

## **Chức năng chưa thực hiện**<a name="notdone"></a>
* * *
**Không**
## **Chức năng cộng thêm**<a name="other"></a>
* * *
### **Tất cả improvement mà thầy gợi ý đều đã được thực hiện:** 
1. Kéo thả các File, Folder

2. Lưu trữ dữ liệu bằng file JSON.
3. Chỉ cần kéo thả một folder vào tab file, thì tất cả các file của folder đó sẽ được thêm vào danh sách files.
4. Có xử lý lỗi khi tên file, folder bị trùng.
5. Khi đóng ứng dụng, chương trình sẽ tự động lưu trạng thái cuối cùng của ứng dụng 
    1. Kích thước cửa sổ ứng dụng
   
    3. Vị trí cửa sổ ứng dựng trên màn hình
    4. Preset hoặc danh sách các rule cuối cùng được chọn
    5. Danh sách file, folder đang thao tác.
6. Có chức năng auto save lại project khi ứng dụng bị tắt đột ngột (ví dụ như mất điện)
    1. Kích thước cửa sổ ứng dụng

    2. Vị trí cửa sổ ứng dựng trên màn hình
    3. Preset hoặc danh sách các rule cuối cùng được chọn
    4. Danh sách file, folder đang thao tác.
    5. Project được auto-save mỗi 60 giây.
7. Sử dụng Regular Expression để kiểm tra tính hợp lệ của tên file, tên folder và Regex còn được áp dụng để xử lý chuỗi trong một số Rule để kiểm tra tính hợp lệ của input từ người dùng.
8. Có chức năng kiểm tra tính hợp lệ của tên file, folder. Kiểm tra độ dài tên file/folder có vượt quá 255 kí tự và thông báo cho người dùng.
9. Có chức năng lưu trạng thái làm việc hiện tại vào các project(dưới dạng file json), người dùng có thể mở lại các project này để làm việc tiếp tục. 
10. Người dùng có thể xem trước được kết quả của việc đổi tên file, folder
11. Ngoài chức năng đổi tên trên chính file/folder gốc, thì có thể chọn chức năng copy toàn bộ file/folder sang nơi khác rồi đổi tên.
### **Các improvement khác:**
1. Sử dụng thư viện HandyControl (https://hosseini.ninja/handycontrol/) để giao diện của ứng dụng trở nên đẹp và thân thiện với người dùng hơn
2. Giao diện ứng dụng được thiết kế responsive, thích hợp với nhiều kích thước cửa sổ khác nhau, điều chỉnh số lượng hàng trên 1 trang theo kích thước cửa sổ.
3. Có các chức năng như new, save, save as, open để tạo mới, lưu, tạo bản sao và mở các project. 
4. Ngoài việc ấn các nút tương ứng để thực hiện các chức năng vừa kể, người dùng cỏ thể ấn ***Crtl + N***, ***Ctrl + S***, ***Ctrl + O***, để thực hiện các chức năng new, save, open.

5.  Có thêm một số luật đổi tên mới:
    1. **Strip**: Dùng để lược bỏ kí tự mong muốn ra khỏi tên 

    2. **Remove Accent**: Chuyển tiếng Việt có dấu thành không dấu
    3. **Add Alphabet Counter**: thêm Counter bằng chữ cái(a, b, c, ..., z, aa, ab, ..., zz...) vào tên file. Có thể lựa chọn thêm vị trí ở đầu hoặc cuối tên file, thêm chuỗi phân cách giữa counter và tên file.
    4. **Convert to uppercase**: Viết hoa tất cả các chữ cái trong tên, đồng thời loại bỏ mọi khoảng trắng
    5. **Convert to camelCase**: Chuyển tên file/folder theo dạng camelCase
    6. **Change File Name**: Đổi hoàn toàn tên file/folder
    7. **Replace Character**: Đổi kí tự bất kì trong tên file/folder thành kí tự khác
6. Có thể kéo thả **(dùng chuột phải)** các luật trong danh sách để thay đổi vị trí của chúng.
7. Nhấn **(nút x)** để xóa luật khỏi danh sách.
8. Nhấn vào luật để thu gọn hoặc mở rộng giao diện cài đặt của luật đó.
9. Ngoài việc load luật từ folder DLL sẵn có, có thêm chức năng Browse để người dùng chọn file dll muốn sử dụng từ máy tính, xử lí error DLL lỗi hoặc trùng.
10. Người dùng có thể áp dụng tất cả các luật đang xuất hiện trong danh sách cho các file/folder thông qua checkbox ***All rules***
11. Người dùng có thể dọn dẹp danh sách các luật thông qua nút ***Clear***. 
12. Người dùng có thể dọn dẹp danh sách các preset thông qua nút ***Clear***. 

13. Danh sách các file/folder đều được phân trang, có phần lựa chọn trang muốn nhảy đến.
14. Có thể kéo thả file/folder vào số trang để chuyển chúng qua trang đó.
15. Có thể kéo thả **(dùng chuột trái)** các file/folder trong danh sách để thay đổi vị trí của chúng
16. Ngăn chặn việc kéo thả file từ máy tính vào tab folder, ngăn chặn việc kéo thả cùng một file hoặc cùng một folder nhiều lần.
17. Có Context Menu cho mỗi file/folder trong danh sách trên giao diện với các chức năng:
    1. Đối với file:
        - Open this File: Mở file đó
        - Open in File explorer: Mở file đó trong file explorer
        - Delete Path: Xóa file khỏi danh sách file đang thao tác
    2. Đối với folder:
        - Open in File explorer: Mở folder đó trong file explorer
        - Delete Path: Xóa folder khỏi danh sách folder đang thao tác
18. Phần giao diện danh sách file/folder có cột Status để hiển thị trạng thái của file/folder sau khi đổi tên(thành công, thất bại, trùng tên, không tồn tại,...)
19. Hiển thị tổng sổ file/folder được rename thành công, rename thất bại, tổng số file và folder đang xử lí.
20. Icon của file được hiển thị phù hợp với định dạng file.
21. Có chức năng để xóa tất cả các file/folder không tồn tại trên máy ra khỏi danh sách file/folder đang làm việc.
22. Người dùng có thể dọn dẹp danh sách các file/folder thông qua nút ***clear***. 

## **Điểm số mong muốn** <a name="score"></a>
* * *
Điểm số mong muốn của từng thành viên:
- Nguyễn Hồ Diệu Hương (MSSV: 19120224): **10**

- Lê Trần Đăng Khoa (MSSV: 19120546): **10**
- Lê Nguyễn Thảo Mi (MSSV: 1912076): **10**
- Phạm Sơn Nam (MSSV: 19120596): **10**
## **Video demo** <a name="video"></a>
* * *
<https://youtube.com>
