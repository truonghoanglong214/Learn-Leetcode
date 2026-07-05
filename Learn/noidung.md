# Tổng quan lộ trình

> **Ngôn ngữ học:** C# — tất cả code ví dụ và bài tập trong các phase đều viết bằng C#.

> **Chuẩn trình bày bài tập:** Mỗi bài tập trong file phase phải có đủ các mục sau, theo đúng thứ tự:
> 1. **Đề** — mô tả bài toán rõ ràng, đầy đủ điều kiện.
> 2. **Ví dụ** — tối thiểu 2–3 test case theo dạng:
>    ```
>    Input:  <giá trị cụ thể>
>    Output: <kết quả>
>    Vì: <giải thích ngắn nếu cần>
>    ```
>    Phải có ít nhất 1 **edge case** (mảng rỗng, một phần tử, số âm, trùng lặp, tràn biên...).
> 3. **Constraint** — giới hạn từ đề LeetCode (độ dài mảng, miền giá trị...).
> 4. **Phân tích 5 câu hỏi** (nếu là bài bản lề hoặc bài mới về pattern).
> 5. **Code C#** — brute force rồi optimal, kèm comment complexity.

> Mục tiêu của roadmap này **không phải** học thuộc lời giải, mà là xây *bộ não nhận diện pattern*: nhìn vào đề là phân loại được, biết tại sao một thuật toán phù hợp, và biết đường đi từ Brute Force → Better → Optimal.

## Triết lý xuyên suốt

Mỗi khi gặp một bài, bạn sẽ luôn chạy qua 5 câu hỏi (gọi là **vòng lặp tư duy**):

1. **Input nói gì?** — Mảng đã sắp xếp chưa? Có trùng lặp không? Kích thước n là bao nhiêu (n cho biết độ phức tạp mục tiêu)? Giá trị có giới hạn không?
2. **Output cần gì?** — Một số? Một mảng? Tất cả tổ hợp? Đúng/sai? Tối ưu hóa (min/max)?
3. **Brute Force là gì?** — Luôn nghĩ ra lời giải "ngu" nhất trước. Nó cho bạn baseline đúng và lộ ra chỗ lãng phí.
4. **Chỗ lãng phí nằm đâu?** — Đang tính lại cái gì? Đang quét lại cái gì? → Đây là nơi pattern xuất hiện (hash để nhớ, con trỏ để khỏi quét lại, prefix để khỏi cộng lại...).
5. **Có pattern nào khớp không?** — Đối chiếu "dấu hiệu nhận biết" của từng pattern.

> **Quy tắc vàng:** n ≤ 20 → nghĩ tới quay lui/exponential. n ≤ 500 → O(n³) ổn. n ≤ 5000 → O(n²). n ≤ 10⁶ → O(n log n) hoặc O(n). n ≤ 10⁹ → O(log n) hoặc O(1). Chính kích thước input gợi ý thuật toán.

## Bản đồ các Phase

| Phase | Chủ đề | Vai trò |
|------|--------|---------|
| 0 | Complexity & Tư duy nền | Biết "đo" lời giải, biết đọc đề |
| 1 | Arrays & Hashing | Nền tảng của mọi thứ; đánh đổi bộ nhớ lấy tốc độ |
| 2 | Two Pointers | Bỏ vòng lặp lồng nhau trên dữ liệu có thứ tự |
| 3 | Sliding Window | Đoạn con/chuỗi con liên tiếp |
| 4 | Prefix Sum / Hashing nâng cao | Truy vấn khoảng, đếm subarray |
| 5 | Binary Search | Tìm kiếm trên không gian sắp xếp / không gian đáp án |
| 6 | Stack & Monotonic Stack | Lồng nhau, "phần tử lớn/nhỏ kế tiếp" |
| 7 | Linked List | Thao tác con trỏ, fast/slow |
| 8 | Recursion & Backtracking | Tư duy đệ quy, sinh tổ hợp/hoán vị |
| 9 | Trees (Binary Tree, BST) | Đệ quy có cấu trúc, duyệt cây |
| 10 | Heap / Priority Queue | Top-K, trộn nhiều nguồn, lịch trình |
| 11 | Graphs (BFS/DFS) | Mô hình hóa quan hệ, duyệt đồ thị |
| 12 | Advanced Graph | Union-Find, Topo Sort, đường đi ngắn nhất |
| 13 | Greedy & Intervals | Chọn cục bộ tối ưu, xử lý khoảng |
| 14 | Dynamic Programming | Bài toán con chồng lặp, tối ưu hóa |
| 15 | Advanced (Trie, Bit, Segment Tree, Monotonic Queue, Advanced DP) | Vũ khí cho bài Hard |
| 16 | Mixed & Mô phỏng phỏng vấn | Trộn lẫn, phản xạ phân loại, giao tiếp |

**Tổng thời gian dự kiến:** ~5–7 tháng nếu học đều 1–2h/ngày (có thể nhanh/chậm tùy nền tảng).

---

# Phase 0 — Complexity & Tư duy nền

## Mục tiêu
Sau phase này bạn **chưa giải bài thuật toán khó**, nhưng sẽ: đọc đề và bóc tách input/output thành thạo, ước lượng được độ phức tạp một đoạn code, và dùng kích thước input để đoán thuật toán mục tiêu. Đây là "thước đo" bạn dùng cho mọi phase sau.

## Kiến thức cần học
- Big O, Big Θ, Big Ω (tập trung Big O là đủ cho phỏng vấn).
- Time complexity vs Space complexity.
- Cách tính độ phức tạp: vòng lặp đơn O(n), lồng O(n²), chia đôi O(log n), đệ quy nhị phân O(2ⁿ).
- Amortized complexity (ví dụ vì sao `append` vào mảng động trung bình là O(1)).
- Best / Average / Worst case.
- Cách đọc constraint trên LeetCode và suy ra thuật toán mục tiêu.

## Pattern quan trọng
- **"n quyết định thuật toán"**: dùng bảng quy tắc vàng ở trên.
- **Phân rã input → output → ràng buộc** trước khi nghĩ thuật toán.

## Dấu hiệu nhận biết
- Đề cho `n ≤ 10⁵` mà bạn định viết O(n²) → gần như chắc chắn sai hướng (10¹⁰ phép tính).
- Đề yêu cầu "đếm số cách / sinh tất cả" → thường exponential/combinatorial.
- Đề có "sắp xếp sẵn" hoặc "tìm trong không gian" → nghĩ tới log n.

## Lỗi tư duy thường gặp
- Bỏ qua hằng số nhưng lại tối ưu hằng số khi sai cả bậc độ phức tạp.
- Quên tính chi phí bộ nhớ (đặc biệt với hash map, đệ quy stack).
- Nhầm O(n log n) với O(n) khi có sort ẩn bên trong.
- Đọc đề vội, bỏ sót ràng buộc "mảng đã sắp xếp" / "có thể có số âm" / "có trùng lặp".

## Mức độ khó
⭐ (Nền tảng — không code nhiều, chủ yếu phân tích).

## Thời gian dự kiến
3–5 ngày.

## Bài tập
Phase này luyện *phân tích*, không cần submit nhiều:
- Lấy 5 bài Easy bất kỳ bạn đã biết cách giải, **chỉ làm bước phân tích**: viết ra input/output/constraint, brute force, độ phức tạp của brute force, và đoán độ phức tạp mục tiêu từ constraint. Chưa cần code tối ưu.
- Tự ước lượng Big O cho 10 đoạn code (vòng lặp đơn, lồng, lồng có break, đệ quy).

## Tiêu chí hoàn thành
- [ ] Nhìn một đoạn code bất kỳ, nói đúng Big O time & space trong < 30 giây.
- [ ] Nhìn constraint của một bài, đoán đúng độ phức tạp mục tiêu.
- [ ] Viết được brute force + phân tích cho 5 bài mà không nhìn lời giải.

---

# Phase 1 — Arrays & Hashing

## Mục tiêu
Giải được các bài cần **tra cứu / đếm / nhóm nhanh**: tìm cặp tổng, tìm trùng lặp, đếm tần suất, nhóm anagram. Đây là phase quan trọng nhất vì hash là "phản xạ tối ưu" số một.

## Kiến thức cần học
- Mảng: truy cập, duyệt, hai chiều, in-place.
- Hash Set (kiểm tra tồn tại), Hash Map (key→value), đếm tần suất.
- Đánh đổi **không gian lấy thời gian**: lưu lại thứ đã thấy để khỏi quét lại.
- Mảng đếm (counting array) khi miền giá trị nhỏ.

## Pattern quan trọng
1. **Frequency counting** — đếm số lần xuất hiện bằng hash map.
2. **Seen-set / complement lookup** — "đã thấy gì rồi?" để biến O(n²) thành O(n) (vd Two Sum: tìm `target - x`).
3. **Grouping bằng key chuẩn hóa** — nhóm các phần tử theo một "chữ ký" (vd anagram → sort chữ cái hoặc đếm chữ làm key).

## Dấu hiệu nhận biết
- "Có tồn tại cặp / có trùng lặp không?" → seen-set.
- "Đếm số lần / phần tử xuất hiện nhiều nhất" → frequency map.
- "Nhóm các phần tử giống nhau theo tiêu chí nào đó" → grouping bằng key.
- Brute force của bạn là *hai vòng lặp lồng nhau để so từng cặp* → hỏi: "mình có thể nhớ lại cái đã duyệt bằng hash không?"

## Lỗi tư duy thường gặp
- Dùng hash map khi mảng đếm đơn giản hơn (miền giá trị nhỏ, cố định).
- Quên xử lý phần tử trùng / chính nó (Two Sum không được dùng lại cùng index).
- Chọn sai key khi grouping (key không phân biệt được các nhóm).
- Sửa mảng trong lúc đang duyệt gây lỗi index.

## Mức độ khó
⭐⭐

## Thời gian dự kiến
1–2 tuần.

## Bài tập (chọn lọc — làm theo thứ tự)
**Easy**
- *Contains Duplicate (217)* — phản xạ seen-set đầu tiên. Pattern: seen-set. Kỹ năng: đánh đổi bộ nhớ lấy tốc độ.
- *Two Sum (1)* — kinh điển complement lookup. Pattern: hash + bù. Kỹ năng: biến O(n²)→O(n).
- *Valid Anagram (242)* — frequency map / counting array. Pattern: đếm tần suất. Kỹ năng: chuẩn hóa so sánh.
- *Majority Element (169)* — đếm; sau đó học thêm Boyer-Moore. Kỹ năng: nhận ra giải pháp O(1) bộ nhớ.

**Medium**
- *Group Anagrams (49)* — grouping bằng key chuẩn hóa. Kỹ năng: thiết kế key.
- *Top K Frequent Elements (347)* — đếm + chọn top K (bucket sort / heap). Kỹ năng: kết hợp đếm với chọn lọc.
- *Product of Array Except Self (238)* — tiền/hậu tố, cấm dùng phép chia. Kỹ năng: nghĩ "tích trái × tích phải", bắc cầu sang Prefix Sum.
- *Longest Consecutive Sequence (128)* — set + chỉ bắt đầu đếm từ đầu chuỗi. Kỹ năng: dùng set để đạt O(n) thay vì sort.

**Hard** (sau khi đã chắc Medium)
- *First Missing Positive (41)* — index-as-hash, in-place. Kỹ năng: dùng chính mảng làm bảng băm.

## Tiêu chí hoàn thành
- [ ] Gặp bài "tìm cặp/trùng/đếm" là phản xạ nghĩ tới hash ngay.
- [ ] Tự giải ≥ 6/9 bài trên không nhìn lời giải.
- [ ] Giải thích được khi nào dùng hash map vs counting array.

---

# Phase 2 — Two Pointers

## Mục tiêu
Giải các bài trên mảng/chuỗi (thường **đã sắp xếp** hoặc có thể sắp xếp) bằng cách loại bỏ vòng lặp lồng nhau: tìm cặp/bộ ba có tổng, kiểm tra palindrome, dồn phần tử, chứa nước.

## Kiến thức cần học
- Two pointers **đối đầu** (trái–phải tiến vào giữa).
- Two pointers **cùng chiều** (slow–fast, đọc/ghi).
- Vì sao two pointers chỉ đúng khi dữ liệu có **tính đơn điệu** (sắp xếp / điều kiện tăng-giảm rõ ràng).

## Pattern quan trọng
1. **Opposite ends** — tổng quá lớn thì lùi phải, quá nhỏ thì tiến trái (cần mảng sắp xếp).
2. **Fast & slow / read-write** — dồn phần tử, xóa tại chỗ, loại trùng.
3. **Fix một + two pointers cho phần còn lại** — nền của 3Sum/4Sum.

## Dấu hiệu nhận biết
- Mảng **đã sắp xếp** và cần tìm cặp/bộ thỏa điều kiện tổng → opposite ends.
- "Sửa mảng tại chỗ", "xóa phần tử", "dồn về một phía" → read-write.
- "Bộ ba/bộ bốn có tổng = target" → cố định 1 phần tử + two pointers (nhớ sort trước).
- Brute force là hai vòng lặp và mảng có thể sort mà không mất tính đúng → thử two pointers.

## Lỗi tư duy thường gặp
- Dùng two pointers khi mảng **chưa sắp xếp** mà thứ tự lại quan trọng.
- Quên xử lý phần tử trùng trong 3Sum (bỏ qua duplicate khi di chuyển con trỏ).
- Di chuyển sai con trỏ → vòng lặp vô hạn hoặc bỏ sót đáp án.
- Nhầm two pointers với sliding window (window là tập con *liên tiếp*, two pointers không nhất thiết).

## Mức độ khó
⭐⭐

## Thời gian dự kiến
4–7 ngày.

## Bài tập (chọn lọc)
**Easy**
- *Valid Palindrome (125)* — opposite ends + chuẩn hóa ký tự. Kỹ năng: hai con trỏ cơ bản.
- *Two Sum II – Input Array Is Sorted (167)* — đối đầu trên mảng sắp xếp. Kỹ năng: tận dụng tính sắp xếp.
- *Move Zeroes (283)* — read-write pointer. Kỹ năng: thao tác in-place.

**Medium**
- *3Sum (15)* — fix + two pointers + bỏ trùng. Kỹ năng: kết hợp sort, loại duplicate. Đây là bài *bản lề*.
- *Container With Most Water (11)* — opposite ends, tham lam thu hẹp. Kỹ năng: lập luận tại sao bỏ cạnh thấp hơn.
- *Sort Colors (75)* — Dutch national flag, ba con trỏ. Kỹ năng: phân vùng tại chỗ.

**Hard**
- *Trapping Rain Water (42)* — two pointers với max trái/phải (so sánh với cách prefix ở Phase 4). Kỹ năng: tư duy "nút thắt thấp hơn quyết định".

## Tiêu chí hoàn thành
- [ ] Tự giải 3Sum không nhìn lời giải, xử lý đúng duplicate.
- [ ] Phân biệt rõ khi nào dùng opposite ends vs cùng chiều.
- [ ] Giải thích được vì sao two pointers cho ra O(n) thay vì O(n²).

---

# Phase 3 — Sliding Window

## Mục tiêu
Giải mọi bài về **đoạn con / chuỗi con liên tiếp**: dài nhất, ngắn nhất, có/không trùng, thỏa điều kiện tần suất. Đây là pattern xuất hiện cực nhiều trong phỏng vấn.

## Kiến thức cần học
- Cửa sổ **cố định kích thước**.
- Cửa sổ **co giãn** (mở rộng phải, thu hẹp trái khi vi phạm điều kiện).
- Kết hợp với hash map/đếm để theo dõi nội dung cửa sổ.

## Pattern quan trọng
1. **Fixed window** — kích thước k cho trước, trượt và cập nhật.
2. **Variable window (mở rộng/thu hẹp)** — mở rộng để tham, thu hẹp để hợp lệ.
3. **Window + frequency map** — đếm ký tự/số trong cửa sổ để biết khi nào hợp lệ.

## Dấu hiệu nhận biết
- Có cụm từ: **"liên tiếp"**, "subarray/substring", "dài nhất", "ngắn nhất", "đúng/ít nhất/nhiều nhất k loại".
- Brute force là *xét mọi đoạn con* O(n²) hoặc O(n³) → nghĩ window để về O(n).
- Điều kiện hợp lệ có tính **đơn điệu** (mở rộng làm điều kiện "tệ" hơn, thu hẹp làm "tốt" hơn) → variable window hoạt động.

## Lỗi tư duy thường gặp
- Dùng window khi đoạn con **không cần liên tiếp** (đó là bài tập con / DP / subsequence).
- Thu hẹp sai thời điểm hoặc quên cập nhật map khi co cửa sổ.
- Tính lại toàn bộ cửa sổ mỗi bước (mất O(n) mỗi lần) thay vì cập nhật incremental.
- Nhầm "đúng k" với "nhiều nhất k" (mẹo: "đúng k" = "nhiều nhất k" − "nhiều nhất k−1").

## Mức độ khó
⭐⭐–⭐⭐⭐

## Thời gian dự kiến
1 tuần.

## Bài tập (chọn lọc)
**Easy**
- *Best Time to Buy and Sell Stock (121)* — cửa sổ ngầm (min trái + lời nhuận). Kỹ năng: theo dõi giá trị nhỏ nhất đã thấy.
- *Maximum Average Subarray I (643)* — fixed window thuần. Kỹ năng: trượt cửa sổ cập nhật incremental.

**Medium**
- *Longest Substring Without Repeating Characters (3)* — variable window + set/map. Bài *bản lề* số một.
- *Longest Repeating Character Replacement (424)* — window + đếm + điều kiện (window − maxFreq ≤ k). Kỹ năng: định nghĩa "hợp lệ".
- *Permutation in String (567)* — fixed window + so khớp tần suất. Kỹ năng: window + map.
- *Fruit Into Baskets (904)* — variable window "nhiều nhất 2 loại". Kỹ năng: tổng quát hóa "≤ k loại".

**Hard**
- *Minimum Window Substring (76)* — variable window + map + bộ đếm "đủ điều kiện". Kỹ năng: master template của window co giãn.
- *Sliding Window Maximum (239)* — cần Monotonic Queue (sẽ gặp lại ở Phase 15).

## Tiêu chí hoàn thành
- [ ] Viết được "template" window co giãn từ đầu mà không cần nhớ.
- [ ] Tự giải bài 3 và 424 không nhìn lời giải.
- [ ] Phân biệt được window cố định vs co giãn ngay khi đọc đề.

---

# Phase 4 — Prefix Sum & Hashing nâng cao

## Mục tiêu
Trả lời nhanh các truy vấn **tổng/đếm trên một khoảng**, và đếm số subarray thỏa điều kiện tổng — kể cả khi có số âm (lúc này sliding window *không* dùng được).

## Kiến thức cần học
- Prefix sum 1D (và sơ lược 2D).
- Difference array (cập nhật khoảng nhanh).
- Kết hợp **prefix sum + hash map** để đếm subarray có tổng = k.
- Tư duy "tổng đoạn [i..j] = prefix[j] − prefix[i−1]".

## Pattern quan trọng
1. **Prefix sum cho range query** — tiền xử lý O(n), trả lời mỗi truy vấn O(1).
2. **Prefix + hashmap đếm** — "đã từng thấy prefix nào = (prefixHienTai − k)?".
3. **Difference array** — cộng dồn nhiều khoảng rồi tính một lần.

## Dấu hiệu nhận biết
- "Tổng của đoạn con", "đếm subarray có tổng = k", nhiều truy vấn tổng khoảng.
- Có **số âm** nên không dùng được sliding window → chuyển sang prefix + hash.
- "Cộng giá trị cho nhiều khoảng rồi hỏi kết quả cuối" → difference array.

## Lỗi tư duy thường gặp
- Off-by-one ở biên prefix (quên `prefix[0] = 0`).
- Cố dùng sliding window cho bài có số âm.
- Quên khởi tạo map với `{0: 1}` trong bài đếm subarray = k.
- Tràn số khi tổng lớn (chú ý kiểu dữ liệu).

## Mức độ khó
⭐⭐⭐

## Thời gian dự kiến
4–6 ngày.

## Bài tập (chọn lọc)
**Easy**
- *Range Sum Query – Immutable (303)* — prefix sum cơ bản. Kỹ năng: tiền xử lý cho truy vấn O(1).
- *Find Pivot Index (724)* — prefix trái/phải. Kỹ năng: tư duy hai phía.

**Medium**
- *Subarray Sum Equals K (560)* — prefix + hashmap. Bài *bản lề*. Kỹ năng: nhận ra "đếm prefix bù".
- *Contiguous Array (525)* — chuẩn hóa 0→−1 rồi prefix + map. Kỹ năng: biến đổi bài toán về dạng đã biết.
- *Product of Array Except Self (238)* — (gặp lại) tích tiền/hậu tố. Kỹ năng: prefix dạng tích.
- *Range Sum Query 2D – Immutable (304)* — prefix 2D. Kỹ năng: bao hàm–loại trừ.

**Hard**
- *Subarrays with K Different Integers (992)* — "đúng k" = "≤k" − "≤(k−1)", kết hợp window. Kỹ năng: kỹ thuật trừ.

## Tiêu chí hoàn thành
- [ ] Tự giải 560 và giải thích vai trò của `{0:1}`.
- [ ] Biết chọn giữa sliding window và prefix+hash dựa vào "có số âm hay không".
- [ ] Hiểu prefix 2D ở mức vẽ được công thức bao hàm–loại trừ.

---

# Phase 5 — Binary Search

## Mục tiêu
Tìm kiếm trong O(log n) trên mảng sắp xếp, **và** — quan trọng hơn — biết "binary search trên không gian đáp án" (binary search the answer), kỹ thuật xuất hiện rất nhiều ở bài Medium/Hard.

## Kiến thức cần học
- Binary search chuẩn (tìm chính xác).
- Tìm cận trái / cận phải (lower_bound / upper_bound).
- Binary search trên **mảng xoay** / dữ liệu có cấu trúc.
- **Binary search the answer**: khi đáp án nằm trong một khoảng và tồn tại hàm kiểm tra đơn điệu `feasible(x)`.

## Pattern quan trọng
1. **Tìm vị trí / giá trị** trong mảng sắp xếp.
2. **Cận trái/phải** khi có trùng lặp hoặc cần biên.
3. **Binary search trên đáp án** — "tìm giá trị nhỏ nhất sao cho điều kiện thỏa".

## Dấu hiệu nhận biết
- Dữ liệu **sắp xếp** + cần tìm nhanh → binary search.
- Bài hỏi "giá trị **nhỏ nhất/lớn nhất** sao cho có thể làm được X" và bạn có thể viết hàm `feasible(x)` đơn điệu (x lớn hơn thì dễ hơn / khó hơn một cách nhất quán) → binary search the answer.
- Constraint kiểu `n ≤ 10⁵` nhưng giá trị tới `10⁹` → gợi ý log trên miền giá trị.

## Lỗi tư duy thường gặp
- Vòng lặp vô hạn do cập nhật `lo/hi` sai hoặc chọn sai `mid` khi tìm biên.
- Off-by-one giữa `<` và `<=`, `mid` vs `mid±1`.
- Không nhận ra tính đơn điệu nên bỏ lỡ "search the answer".
- Tràn số khi tính `(lo+hi)` (dùng `lo + (hi−lo)/2`).

## Mức độ khó
⭐⭐⭐

## Thời gian dự kiến
1 tuần.

## Bài tập (chọn lọc)
**Easy**
- *Binary Search (704)* — bản chuẩn. Kỹ năng: viết đúng biên.
- *First Bad Version (278)* — tìm cận trái. Kỹ năng: lower_bound.

**Medium**
- *Search in Rotated Sorted Array (33)* — binary search có biến thể. Bài *bản lề*. Kỹ năng: lập luận nửa nào sắp xếp.
- *Find Minimum in Rotated Sorted Array (153)* — tìm điểm xoay. Kỹ năng: so sánh với biên phải.
- *Koko Eating Bananas (875)* — search the answer điển hình. Kỹ năng: viết `feasible(speed)`.
- *Find First and Last Position (34)* — lower & upper bound. Kỹ năng: hai lần binary search.

**Hard**
- *Median of Two Sorted Arrays (4)* — binary search trên phân hoạch. Kỹ năng: tư duy partition (khó, để cuối phase).
- *Split Array Largest Sum (410)* — search the answer + feasibility. Kỹ năng: tổng quát hóa Koko.

## Tiêu chí hoàn thành
- [ ] Viết binary search (cả lower/upper bound) đúng biên mà không debug lâu.
- [ ] Nhận ra "search the answer" qua dấu hiệu đơn điệu (giải được Koko).
- [ ] Giải 33 không nhìn lời giải.

---

# Phase 6 — Stack & Monotonic Stack

## Mục tiêu
Giải các bài có cấu trúc **lồng nhau** (ngoặc, biểu thức), mô phỏng theo trật tự LIFO, và đặc biệt là họ bài **"phần tử lớn/nhỏ kế tiếp"** bằng monotonic stack.

## Kiến thức cần học
- Stack: push/pop/peek, ứng dụng matching và mô phỏng.
- Monotonic stack (tăng/giảm) và lý do nó cho O(n).
- Liên hệ stack với đệ quy (đệ quy = stack ngầm).

## Pattern quan trọng
1. **Matching / nesting** — ngoặc hợp lệ, undo, biểu thức.
2. **Monotonic stack** — next greater/smaller element, span, "nhìn xa nhất".
3. **Stack mô phỏng** — xử lý theo thứ tự với khả năng "quay lại phần tử trước".

## Dấu hiệu nhận biết
- "Ngoặc hợp lệ", "lồng nhau", "khớp cặp", "undo gần nhất" → stack.
- "Phần tử **lớn hơn / nhỏ hơn kế tiếp**", "nhiệt độ ấm hơn", "giá cổ phiếu span", "diện tích hình chữ nhật lớn nhất" → monotonic stack.
- Brute force là "với mỗi i, quét sang phải tìm phần tử thỏa" O(n²) → monotonic stack hạ về O(n).

## Lỗi tư duy thường gặp
- Không nhận ra bài "next greater" thuộc họ monotonic (cứ quét O(n²)).
- Chọn sai chiều đơn điệu (tăng vs giảm) → kết quả sai.
- Quên xử lý phần tử còn lại trong stack sau vòng lặp.
- Lưu nhầm giá trị vs index trong stack.

## Mức độ khó
⭐⭐⭐

## Thời gian dự kiến
1 tuần.

## Bài tập (chọn lọc)
**Easy**
- *Valid Parentheses (20)* — matching cơ bản. Kỹ năng: ánh xạ cặp ngoặc.
- *Min Stack (155)* — stack phụ giữ min. Kỹ năng: thiết kế cấu trúc.

**Medium**
- *Daily Temperatures (739)* — monotonic stack. Bài *bản lề*. Kỹ năng: nhận ra họ "next greater".
- *Next Greater Element II (503)* — vòng tròn + monotonic. Kỹ năng: xử lý mảng vòng.
- *Evaluate Reverse Polish Notation (150)* — stack tính toán. Kỹ năng: mô phỏng.
- *Car Fleet (853)* — sort + stack. Kỹ năng: kết hợp sắp xếp với stack.

**Hard**
- *Largest Rectangle in Histogram (84)* — monotonic stack đỉnh cao. Kỹ năng: tư duy "cạnh nào giới hạn chiều rộng".
- *Trapping Rain Water (42)* — (gặp lại) cách dùng stack.

## Tiêu chí hoàn thành
- [ ] Phản xạ nghĩ tới monotonic stack khi gặp "next greater/smaller".
- [ ] Giải 739 và lập luận được hướng đơn điệu.
- [ ] Hiểu vì sao mỗi phần tử vào/ra stack đúng 1 lần → O(n).

---

# Phase 7 — Linked List

## Mục tiêu
Thành thạo **thao tác con trỏ**: đảo danh sách, phát hiện chu trình, tìm điểm giữa, trộn danh sách, và các bài cần thao tác tại chỗ không dùng thêm bộ nhớ.

## Kiến thức cần học
- Singly / doubly linked list, dummy node.
- **Fast & slow pointers** (Floyd): tìm giữa, phát hiện & tìm điểm bắt đầu chu trình.
- Đảo danh sách (lặp & đệ quy).
- Trộn, tách, xóa node.

## Pattern quan trọng
1. **Dummy head** — xử lý thống nhất biên đầu danh sách.
2. **Fast & slow** — giữa danh sách, chu trình, node thứ k từ cuối.
3. **In-place reversal** — đảo toàn bộ / một đoạn.

## Dấu hiệu nhận biết
- Đề cho cấu trúc node có `next` → gần như chắc chắn dùng con trỏ.
- "Tìm node giữa", "có chu trình không", "node thứ k từ cuối" → fast & slow.
- "Đảo", "đảo theo nhóm k", "kiểm tra palindrome list" → in-place reversal (+ fast/slow).
- Yêu cầu O(1) bộ nhớ → không được đổ sang mảng, phải thao tác con trỏ.

## Lỗi tư duy thường gặp
- Mất con trỏ `next` khi đảo (quên lưu tạm trước khi đổi hướng).
- Null pointer ở biên (danh sách rỗng, một phần tử).
- Không dùng dummy node nên phải xử lý nhiều case đặc biệt.
- Lệch một bước với fast/slow (giữa-trái vs giữa-phải).

## Mức độ khó
⭐⭐–⭐⭐⭐

## Thời gian dự kiến
4–6 ngày.

## Bài tập (chọn lọc)
**Easy**
- *Reverse Linked List (206)* — đảo cơ bản (làm cả lặp & đệ quy). Bài *bản lề*.
- *Merge Two Sorted Lists (21)* — dummy node + trộn. Kỹ năng: nối con trỏ.
- *Linked List Cycle (141)* — Floyd phát hiện chu trình. Kỹ năng: fast/slow.

**Medium**
- *Remove Nth Node From End (19)* — hai con trỏ cách k. Kỹ năng: khoảng cách cố định.
- *Reorder List (143)* — tìm giữa + đảo nửa sau + trộn. Kỹ năng: ghép nhiều kỹ thuật.
- *Add Two Numbers (2)* — mô phỏng cộng + nhớ. Kỹ năng: xử lý carry & độ dài lệch.
- *LRU Cache (146)* — doubly linked list + hashmap. Kỹ năng: thiết kế cấu trúc (rất hay hỏi phỏng vấn).

**Hard**
- *Merge k Sorted Lists (23)* — heap / chia để trị (liên hệ Phase 10). Kỹ năng: trộn nhiều nguồn.
- *Reverse Nodes in k-Group (25)* — đảo theo nhóm. Kỹ năng: thao tác con trỏ tinh vi.

## Tiêu chí hoàn thành
- [ ] Đảo danh sách (lặp + đệ quy) viết được ngay.
- [ ] Giải thích và dùng được Floyd cho cả phát hiện lẫn tìm điểm bắt đầu chu trình.
- [ ] Thiết kế được LRU Cache.

---

# Phase 8 — Recursion & Backtracking

## Mục tiêu
Xây **tư duy đệ quy** vững (tin vào "giả định đệ quy đúng") và làm chủ backtracking để sinh **mọi** hoán vị / tổ hợp / tập con, giải các bài tìm kiếm trên không gian trạng thái.

## Kiến thức cần học
- Đệ quy: base case, recursive case, "leap of faith".
- Cây đệ quy, không gian trạng thái.
- Backtracking: chọn → đệ quy → bỏ chọn (undo).
- Cắt nhánh (pruning).

## Pattern quan trọng
1. **Subsets / Combinations / Permutations** — ba khuôn mẫu sinh tổ hợp.
2. **Decision tree + backtrack** — tại mỗi bước "chọn hay không chọn / chọn cái nào".
3. **Pruning** — bỏ nhánh chắc chắn không dẫn tới đáp án.

## Dấu hiệu nhận biết
- "Liệt kê **tất cả**", "tất cả tổ hợp/hoán vị/tập con/cách", "tất cả đường đi" → backtracking.
- `n` rất nhỏ (≤ ~20) và yêu cầu sinh nghiệm → exponential chấp nhận được → backtracking.
- Bài có cấu trúc "đặt từng cái một thỏa ràng buộc" (N-Queens, Sudoku, ghép chữ) → backtracking + pruning.

## Lỗi tư duy thường gặp
- Quên **undo** sau đệ quy (trạng thái rò rỉ giữa các nhánh).
- Sinh trùng do không xử lý duplicate (cần sort + bỏ qua phần tử trùng cùng cấp).
- Base case sai hoặc thiếu → vô hạn / sai kết quả.
- Sửa trực tiếp danh sách kết quả thay vì lưu **bản sao** của trạng thái hiện tại.

## Mức độ khó
⭐⭐⭐–⭐⭐⭐⭐

## Thời gian dự kiến
1–1.5 tuần.

## Bài tập (chọn lọc)
**Easy/Khởi động**
- *Fibonacci / Climbing Stairs (70)* — đệ quy → nhận ra chồng lặp (bắc cầu sang DP). Kỹ năng: cây đệ quy.

**Medium**
- *Subsets (78)* — khuôn mẫu tập con. Bài *bản lề* số một của backtracking.
- *Combination Sum (39)* — chọn lặp lại + pruning. Kỹ năng: kiểm soát nhánh.
- *Permutations (46)* — khuôn mẫu hoán vị. Kỹ năng: dùng mảng `used`.
- *Subsets II (90) / Combination Sum II (40)* — xử lý duplicate. Kỹ năng: bỏ trùng cùng cấp.
- *Word Search (79)* — backtracking trên lưới. Kỹ năng: trạng thái + undo trên grid.

**Hard**
- *N-Queens (51)* — backtracking + cắt nhánh kinh điển. Kỹ năng: kiểm tra ràng buộc hiệu quả.
- *Sudoku Solver (37)* — backtracking nặng + pruning. Kỹ năng: mô hình hóa ràng buộc.

## Tiêu chí hoàn thành
- [ ] Viết được khuôn mẫu subsets / permutations / combinations từ trí nhớ.
- [ ] Xử lý đúng duplicate trong cả ba khuôn mẫu.
- [ ] Tin và áp dụng được "leap of faith" khi thiết kế đệ quy.

---

# Phase 9 — Trees (Binary Tree & BST)

## Mục tiêu
Áp dụng đệ quy vào cấu trúc cây: duyệt (pre/in/post/level-order), tính chiều cao/đường kính, kiểm tra tính chất, khai thác **tính chất BST** (in-order = tăng dần).

## Kiến thức cần học
- Cây nhị phân, BST, các kiểu duyệt (DFS pre/in/post, BFS level-order).
- Đệ quy "trả về thông tin từ con lên cha".
- Tính chất BST và LCA (tổ tiên chung gần nhất).
- Serialize/deserialize cơ bản.

## Pattern quan trọng
1. **DFS đệ quy "bottom-up"** — mỗi node tổng hợp kết quả từ con.
2. **DFS "top-down"** truyền trạng thái xuống (vd kiểm tra hợp lệ với min/max).
3. **BFS level-order** — xử lý theo tầng (queue).
4. **Khai thác in-order của BST** — biến bài BST thành bài mảng sắp xếp.

## Dấu hiệu nhận biết
- Cấu trúc node có `left/right` → cây → nghĩ DFS đệ quy trước.
- "Theo tầng", "level", "trái-sang-phải mỗi hàng" → BFS + queue.
- Đề có **BST** → in-order cho dãy tăng → nhiều bài quy về "thao tác trên dãy sắp xếp".
- "Đường kính", "đường đi tổng lớn nhất", "chiều cao cân bằng" → DFS trả nhiều giá trị về cha.

## Lỗi tư duy thường gặp
- Nhầm validate BST (phải truyền khoảng min/max, không chỉ so cha–con).
- Quên null node trong base case.
- Dùng BFS khi DFS gọn hơn và ngược lại.
- Trả sai thứ "giá trị toàn cục" (đường kính) vs "giá trị trả về cho cha" (chiều cao).

## Mức độ khó
⭐⭐⭐

## Thời gian dự kiến
1–1.5 tuần.

## Bài tập (chọn lọc)
**Easy**
- *Maximum Depth of Binary Tree (104)* — DFS cơ bản. Kỹ năng: đệ quy trên cây.
- *Invert Binary Tree (226)* — đệ quy đối xứng. Kỹ năng: tư duy "swap rồi đệ quy".
- *Same Tree (100) / Subtree (572)* — so sánh cấu trúc. Kỹ năng: đệ quy song song.

**Medium**
- *Binary Tree Level Order Traversal (102)* — BFS. Bài *bản lề* cho level-order.
- *Validate BST (98)* — top-down với khoảng. Kỹ năng: truyền ràng buộc xuống.
- *Lowest Common Ancestor (236, và 235 cho BST)* — LCA. Kỹ năng: khai thác cấu trúc.
- *Construct Tree from Preorder & Inorder (105)* — dựng cây từ duyệt. Kỹ năng: phân hoạch đệ quy.
- *Kth Smallest in BST (230)* — in-order. Kỹ năng: dùng tính chất BST.

**Hard**
- *Binary Tree Maximum Path Sum (124)* — DFS trả về vs cập nhật toàn cục. Bài *bản lề* Hard.
- *Serialize and Deserialize Binary Tree (297)* — mã hóa cấu trúc. Kỹ năng: tuần tự hóa.

## Tiêu chí hoàn thành
- [ ] Viết cả 4 kiểu duyệt từ trí nhớ.
- [ ] Phân biệt rõ "trả về cho cha" và "cập nhật biến toàn cục".
- [ ] Giải 124 không nhìn lời giải.

---

# Phase 10 — Heap / Priority Queue

## Mục tiêu
Giải các bài **Top-K**, **trộn nhiều nguồn sắp xếp**, **lập lịch theo ưu tiên**, và bài cần "luôn lấy phần tử nhỏ/lớn nhất hiện tại" hiệu quả.

## Kiến thức cần học
- Heap (min/max), độ phức tạp push/pop O(log n), heapify O(n).
- Khi nào heap thắng sort (cần top-k liên tục, dữ liệu streaming).
- Two-heaps (median), heap + hashmap.

## Pattern quan trọng
1. **Top-K** — giữ heap kích thước k.
2. **Merge K sorted** — heap chứa "đầu" mỗi nguồn.
3. **Scheduling / greedy theo ưu tiên** — luôn lấy việc tốt nhất kế tiếp.
4. **Two heaps** — cân bằng nửa nhỏ / nửa lớn (median dòng dữ liệu).

## Dấu hiệu nhận biết
- "K phần tử lớn nhất/nhỏ nhất", "K thường gặp nhất", "K gần nhất" → heap kích thước k.
- "Trộn k danh sách/dòng sắp xếp" → min-heap k nguồn.
- "Liên tục lấy phần tử cực trị", lập lịch CPU, khoảng cách → heap.
- "Median của dòng dữ liệu chảy vào" → two heaps.

## Lỗi tư duy thường gặp
- Dùng max-heap khi cần min-heap (đảo ngược dấu sai chỗ).
- Giữ heap to bằng n thay vì giới hạn k (mất lợi thế).
- Quên heap chỉ cho cực trị, **không** cho truy vấn phần tử bất kỳ.
- Dùng heap khi sort một lần là đủ (k cố định, không streaming).

## Mức độ khó
⭐⭐⭐

## Thời gian dự kiến
1 tuần.

## Bài tập (chọn lọc)
**Easy**
- *Kth Largest Element in a Stream (703)* — heap kích thước k. Kỹ năng: duy trì top-k.
- *Last Stone Weight (1046)* — max-heap mô phỏng. Kỹ năng: lấy cực trị lặp.

**Medium**
- *Top K Frequent Elements (347)* — (gặp lại) heap / bucket. Kỹ năng: kết hợp đếm + heap.
- *Kth Largest Element in an Array (215)* — heap hoặc quickselect. Kỹ năng: so sánh hai cách.
- *Task Scheduler (621)* — greedy + heap/đếm. Kỹ năng: lập lịch.
- *K Closest Points to Origin (973)* — heap theo khoảng cách. Kỹ năng: top-k với comparator.

**Hard**
- *Find Median from Data Stream (295)* — two heaps. Bài *bản lề* Hard.
- *Merge k Sorted Lists (23)* — (gặp lại) heap. Kỹ năng: trộn nhiều nguồn.

## Tiêu chí hoàn thành
- [ ] Biết chọn min-heap vs max-heap và giới hạn kích thước k đúng.
- [ ] Giải được Find Median from Data Stream (two heaps).
- [ ] Phân biệt khi nào heap, khi nào sort, khi nào quickselect.

---

# Phase 11 — Graphs (BFS / DFS cơ bản)

## Mục tiêu
Mô hình hóa quan hệ thành đồ thị và **duyệt** thành thạo: thành phần liên thông, đường đi ngắn nhất trên đồ thị không trọng số, lan tỏa (flood fill / multi-source BFS), phát hiện chu trình.

## Kiến thức cần học
- Biểu diễn: danh sách kề, ma trận kề, lưới (grid as graph).
- DFS (đệ quy/stack), BFS (queue), mảng `visited`.
- BFS = đường đi ngắn nhất trên đồ thị **không trọng số**.
- Multi-source BFS; phát hiện chu trình (có hướng/vô hướng).

## Pattern quan trọng
1. **Flood fill / connected components** — lan từ một ô/đỉnh ra cả vùng.
2. **BFS shortest path (unweighted)** — số bước ít nhất.
3. **Multi-source BFS** — nhiều điểm xuất phát cùng lúc (vd loang đồng thời).
4. **Grid traversal** — coi ô lưới là đỉnh, ô kề là cạnh.

## Dấu hiệu nhận biết
- "Đảo / vùng / island", "ô kề nhau", "lan ra" → DFS/BFS trên grid.
- "Số bước **ít nhất**" trên đồ thị không trọng số → BFS.
- "Nhiều nguồn cùng lan" (cam thối, lửa cháy) → multi-source BFS.
- Quan hệ "A nối B" / phụ thuộc → mô hình hóa thành đồ thị.

## Lỗi tư duy thường gặp
- Quên `visited` → lặp vô hạn / đếm trùng.
- Dùng DFS để tìm đường ngắn nhất không trọng số (nên BFS).
- Sai khi đánh dấu visited (đánh dấu lúc enqueue vs lúc duyệt).
- Bỏ sót biên grid (ra ngoài mảng).

## Mức độ khó
⭐⭐⭐

## Thời gian dự kiến
1–1.5 tuần.

## Bài tập (chọn lọc)
**Easy/Khởi động**
- *Flood Fill (733)* — DFS/BFS lan vùng. Kỹ năng: khuôn mẫu lan tỏa.

**Medium**
- *Number of Islands (200)* — connected components trên grid. Bài *bản lề*.
- *Rotting Oranges (994)* — multi-source BFS. Kỹ năng: BFS nhiều nguồn theo tầng.
- *Clone Graph (133)* — DFS/BFS + hashmap ánh xạ. Kỹ năng: sao chép cấu trúc.
- *Pacific Atlantic Water Flow (417)* — BFS/DFS từ biên. Kỹ năng: đảo chiều tư duy.
- *Course Schedule (207)* — phát hiện chu trình (mở đường sang topo sort). Kỹ năng: đồ thị phụ thuộc.

**Hard**
- *Word Ladder (127)* — BFS trên đồ thị ẩn. Kỹ năng: xây cạnh ngầm + BFS shortest.

## Tiêu chí hoàn thành
- [ ] Viết DFS và BFS trên cả grid lẫn danh sách kề từ trí nhớ.
- [ ] Nhận ra "shortest unweighted → BFS" ngay.
- [ ] Giải Number of Islands và Rotting Oranges không nhìn lời giải.

---

# Phase 12 — Advanced Graph

## Mục tiêu
Xử lý các bài đồ thị "có cấu trúc": gộp nhóm/kết nối động (Union-Find), sắp xếp phụ thuộc (Topological Sort), và đường đi ngắn nhất **có trọng số** (Dijkstra), cùng làm quen MST.

## Kiến thức cần học
- **Union-Find (DSU)** + nén đường + union theo hạng.
- **Topological Sort** (Kahn/BFS & DFS).
- **Dijkstra** (heap) cho trọng số không âm; sơ lược Bellman-Ford.
- Sơ lược MST (Kruskal/Prim).

## Pattern quan trọng
1. **DSU cho kết nối / nhóm** — đếm thành phần, phát hiện chu trình vô hướng, gộp động.
2. **Topo sort cho phụ thuộc** — thứ tự khóa học, build order, phát hiện chu trình có hướng.
3. **Dijkstra cho shortest path có trọng số ≥ 0**.

## Dấu hiệu nhận biết
- "Có bao nhiêu nhóm/tỉnh kết nối", "có tạo thành chu trình khi thêm cạnh không", gộp tập động → Union-Find.
- "Thứ tự để hoàn thành", "điều kiện tiên quyết", "build phụ thuộc" → topo sort (đồ thị có hướng phi chu trình).
- "Đường đi **ngắn nhất** với **trọng số/chi phí**" → Dijkstra.
- "Nối tất cả với chi phí nhỏ nhất" → MST.

## Lỗi tư duy thường gặp
- DSU thiếu nén đường / union theo hạng → chậm.
- Topo sort không kiểm tra chu trình (đồ thị có chu trình thì vô nghiệm).
- Dùng Dijkstra khi có **trọng số âm** (phải Bellman-Ford).
- Nhầm "shortest unweighted" (BFS) với "weighted" (Dijkstra).

## Mức độ khó
⭐⭐⭐⭐

## Thời gian dự kiến
1.5–2 tuần.

## Bài tập (chọn lọc)
**Medium**
- *Number of Provinces (547)* — Union-Find. Kỹ năng: đếm thành phần bằng DSU.
- *Course Schedule II (210)* — topo sort trả về thứ tự. Bài *bản lề*.
- *Redundant Connection (684)* — DSU phát hiện cạnh tạo chu trình. Kỹ năng: union + detect.
- *Network Delay Time (743)* — Dijkstra cơ bản. Bài *bản lề* shortest path có trọng số.
- *Graph Valid Tree (261)* — DSU/đếm cạnh. Kỹ năng: điều kiện cây.

**Hard**
- *Cheapest Flights Within K Stops (787)* — Bellman-Ford / Dijkstra biến thể. Kỹ năng: ràng buộc số chặng.
- *Alien Dictionary (269)* — dựng đồ thị + topo sort. Kỹ năng: mô hình hóa thứ tự ẩn.
- *Min Cost to Connect All Points (1584)* — MST (Prim/Kruskal). Kỹ năng: cây khung nhỏ nhất.

## Tiêu chí hoàn thành
- [ ] Viết DSU (có nén đường) từ trí nhớ.
- [ ] Viết topo sort (Kahn) và phát hiện chu trình.
- [ ] Viết Dijkstra bằng heap; biết khi nào dùng Dijkstra vs BFS vs Bellman-Ford.

---

# Phase 13 — Greedy & Intervals

## Mục tiêu
Nhận ra khi nào **chọn cục bộ tối ưu** dẫn tới tối ưu toàn cục, và xử lý thành thạo họ bài **khoảng (intervals)**: gộp, chèn, đếm xung đột, lập lịch.

## Kiến thức cần học
- Tư duy greedy & cách *chứng minh* (exchange argument, "greedy stays ahead").
- Sắp xếp theo tiêu chí (bắt đầu / kết thúc) làm bước tiền xử lý.
- Xử lý khoảng: gộp, giao, lập lịch không xung đột.

## Pattern quan trọng
1. **Sort + quét tham** — sắp xếp rồi quyết định tại chỗ.
2. **Interval scheduling** — sort theo điểm kết thúc, chọn không chồng lấn.
3. **Merge intervals** — sort theo điểm bắt đầu, gộp khi chồng lấn.

## Dấu hiệu nhận biết
- "Số ít nhất / nhiều nhất ... không chồng lấn", "tối thiểu số lần", "lịch không trùng" → greedy interval.
- "Gộp/chèn khoảng", "phòng họp", "điểm giao nhau" → interval pattern.
- Có cảm giác "cứ chọn cái tốt nhất ngay bây giờ là ổn" → thử greedy, **nhưng phải kiểm chứng** bằng phản ví dụ trước khi tin.

## Lỗi tư duy thường gặp
- Tin greedy mà không kiểm chứng → sai (nhiều bài trông greedy nhưng phải DP).
- Sort sai tiêu chí (theo start vs end) → kết quả sai.
- Quên cập nhật biên khi gộp khoảng chồng lấn lồng nhau.
- Bỏ sót trường hợp khoảng tiếp xúc tại đúng một điểm.

## Mức độ khó
⭐⭐⭐

## Thời gian dự kiến
1 tuần.

## Bài tập (chọn lọc)
**Easy/Medium**
- *Maximum Subarray (53)* — Kadane (greedy/DP). Bài *bản lề*. Kỹ năng: "reset khi tổng âm".
- *Jump Game (55)* — greedy reach. Kỹ năng: theo dõi tầm với xa nhất.
- *Merge Intervals (56)* — sort + gộp. Bài *bản lề* interval.
- *Insert Interval (57)* — chèn + gộp. Kỹ năng: ba giai đoạn trước/giao/sau.
- *Non-overlapping Intervals (435)* — sort theo end + đếm bỏ. Kỹ năng: interval scheduling.
- *Gas Station (134)* — greedy một vòng. Kỹ năng: lập luận điểm xuất phát.

**Hard**
- *Meeting Rooms II (253)* — sweep line / heap. Kỹ năng: đếm chồng lấn tối đa.
- *Candy (135)* — greedy hai lượt. Kỹ năng: quét trái rồi phải.

## Tiêu chí hoàn thành
- [ ] Giải Kadane và giải thích vì sao reset khi tổng âm.
- [ ] Master merge/insert/scheduling intervals.
- [ ] Biết *kiểm chứng* một ý tưởng greedy bằng phản ví dụ trước khi code.

---

# Phase 14 — Dynamic Programming

## Mục tiêu
Phase quan trọng nhất ở mức Medium/Hard. Mục tiêu: nhận ra **bài toán con chồng lặp**, định nghĩa được **trạng thái** và **công thức truy hồi**, đi từ đệ quy → memo (top-down) → bảng (bottom-up) → tối ưu bộ nhớ.

## Kiến thức cần học
- Phân biệt DP vs greedy vs backtracking.
- Quy trình: định nghĩa state → truy hồi → base case → thứ tự tính → tối ưu không gian.
- Họ bài: 1D (Fibonacci-like), chuỗi/lưới 2D, knapsack (0/1 & unbounded), LCS/edit distance, DP trên dãy con tăng, DP trên khoảng, DP trên cây.

## Pattern quan trọng
1. **DP 1D** — `dp[i]` phụ thuộc vài trạng thái trước (leo cầu thang, house robber).
2. **DP 2D lưới** — `dp[i][j]` từ trên/trái (đường đi, min path).
3. **Knapsack** — chọn/không chọn dưới ràng buộc dung lượng.
4. **DP trên hai chuỗi** — LCS, edit distance.
5. **DP dãy con** — LIS.
6. **DP khoảng / DP cây** — (nâng cao).

## Dấu hiệu nhận biết
- "Đếm số cách", "tối đa/tối thiểu ... qua nhiều bước lựa chọn", "có thể đạt được không".
- Backtracking của bạn **tính lại cùng một trạng thái nhiều lần** → thêm memo → DP.
- Bài có "lựa chọn ở mỗi bước ảnh hưởng bước sau" và **kết quả tối ưu của bài lớn = tổ hợp tối ưu của bài con** (optimal substructure).
- `n` vừa phải (vài trăm–vài nghìn) cho phép O(n²)/O(n·k).

## Lỗi tư duy thường gặp
- Định nghĩa state thiếu thông tin → truy hồi sai.
- Sai base case / thứ tự duyệt (bottom-up tính trước cái phụ thuộc).
- Dùng greedy cho bài thực ra cần DP (vd coin change min, không phải mệnh giá chuẩn).
- Không nhận ra "đây chỉ là backtracking + memo".
- Tối ưu bộ nhớ quá sớm khiến khó debug — hãy làm đúng (bảng đầy đủ) trước.

## Mức độ khó
⭐⭐⭐⭐ (cần nhiều thời gian, kiên nhẫn).

## Thời gian dự kiến
2.5–3.5 tuần (phase dài nhất — đừng vội).

## Bài tập (chọn lọc — theo nhóm)
**Khởi động 1D**
- *Climbing Stairs (70)* — DP 1D đầu tiên (từ đệ quy → memo → mảng). Bài *bản lề*.
- *House Robber (198) & II (213)* — chọn/không chọn liền kề. Kỹ năng: định nghĩa state.
- *Coin Change (322)* — unbounded knapsack min. Kỹ năng: truy hồi "thử mọi đồng".

**Chuỗi / dãy con**
- *Longest Increasing Subsequence (300)* — DP O(n²) rồi O(n log n). Kỹ năng: state theo "kết thúc tại i".
- *Longest Common Subsequence (1143)* — DP 2D hai chuỗi. Bài *bản lề*.
- *Edit Distance (72)* — DP 2D với 3 thao tác. Kỹ năng: truy hồi nhiều lựa chọn.
- *Word Break (139)* — DP trên chuỗi + từ điển. Kỹ năng: state "tách được tới i".

**Lưới 2D**
- *Unique Paths (62) / Minimum Path Sum (64)* — DP lưới. Kỹ năng: phụ thuộc trên/trái.

**Knapsack & phân hoạch**
- *Partition Equal Subset Sum (416)* — 0/1 knapsack dạng boolean. Kỹ năng: subset-sum.
- *Coin Change II (518)* — đếm cách (unbounded). Kỹ năng: thứ tự vòng lặp tránh trùng.

**Hard**
- *Best Time to Buy/Sell with Cooldown (309)* — DP trạng thái. Kỹ năng: máy trạng thái.
- *Regular Expression Matching (10) / Wildcard (44)* — DP 2D khó. Kỹ năng: truy hồi với ký tự đại diện.
- *Burst Balloons (312)* — DP khoảng. Kỹ năng: nghĩ "quả vỡ cuối cùng".

## Tiêu chí hoàn thành
- [ ] Đi được mạch: đệ quy → memo → bảng cho ít nhất 5 bài khác nhau.
- [ ] Tự định nghĩa state + truy hồi cho bài DP Medium mới mà không nhìn lời giải.
- [ ] Nhận ra ngay 1D vs 2D vs knapsack vs LCS-like qua dấu hiệu đề.

---

# Phase 15 — Advanced (Trie, Bit, Segment Tree, Monotonic Queue, Advanced DP)

## Mục tiêu
Trang bị "vũ khí hạng nặng" cho bài Hard và một số câu hỏi phỏng vấn nâng cao. Không cần thành thần, nhưng cần *nhận diện được* khi nào dùng.

## Kiến thức cần học
- **Trie**: tìm kiếm theo tiền tố, autocomplete, đếm từ.
- **Bit manipulation**: AND/OR/XOR/shift, mặt nạ bit, đếm bit, bitmask DP cơ bản.
- **Monotonic Queue (deque)**: cực trị trong cửa sổ trượt O(n).
- **Segment Tree / BIT (Fenwick)**: truy vấn + cập nhật khoảng/điểm O(log n).
- **Advanced DP**: bitmask DP, DP trên cây, digit DP (giới thiệu).

## Pattern quan trọng
1. **Trie** cho bài "nhiều từ, truy vấn tiền tố / autocomplete".
2. **XOR tricks** ("số xuất hiện một lần", "thiếu số").
3. **Bitmask DP** khi `n ≤ ~20` và trạng thái là tập con.
4. **Monotonic deque** cho max/min của cửa sổ trượt.
5. **Segment Tree / Fenwick** cho range query + update động.

## Dấu hiệu nhận biết
- "Tiền tố", "autocomplete", "tập hợp nhiều từ" → Trie.
- "Số xuất hiện lẻ lần / một lần", thao tác trên bit, tập con biểu diễn bằng số → bit.
- `n ≤ 20` + "ghé thăm mọi tập con / hoán vị có nhớ" → bitmask DP.
- "Max/min của mọi cửa sổ kích thước k" → monotonic deque.
- "Vừa cập nhật phần tử vừa hỏi tổng/min khoảng nhiều lần" → segment tree / Fenwick.

## Lỗi tư duy thường gặp
- Dùng segment tree khi prefix sum tĩnh là đủ (vẽ vời quá mức).
- Sai dấu / ưu tiên toán tử bit; nhầm bit thứ 0.
- Monotonic deque quên loại phần tử ra khỏi cửa sổ theo index.
- Bitmask DP nổ trạng thái khi n quá lớn (chỉ dùng n nhỏ).

## Mức độ khó
⭐⭐⭐⭐–⭐⭐⭐⭐⭐

## Thời gian dự kiến
2–3 tuần (học để *nhận diện* + làm vài bài mỗi chủ đề, không cần cày sâu hết).

## Bài tập (chọn lọc)
**Trie**
- *Implement Trie (208)* — dựng Trie. Bài *bản lề*.
- *Word Search II (212)* — Trie + backtracking trên grid. Kỹ năng: ghép hai công cụ.
- *Design Add and Search Words (211)* — Trie + ký tự đại diện.

**Bit**
- *Single Number (136)* — XOR. Kỹ năng: tính chất XOR.
- *Number of 1 Bits (191) / Counting Bits (338)* — đếm bit. Kỹ năng: thao tác bit cơ bản.
- *Sum of Two Integers (371)* — cộng không dùng +. Kỹ năng: carry bằng bit.

**Monotonic Queue**
- *Sliding Window Maximum (239)* — deque đơn điệu. Bài *bản lề*.

**Segment Tree / Fenwick**
- *Range Sum Query – Mutable (307)* — Fenwick/segment tree. Kỹ năng: update + query động.
- *Count of Smaller Numbers After Self (315)* — Fenwick / merge sort. Kỹ năng: đếm nghịch thế.

**Advanced DP**
- *Partition to K Equal Sum Subsets (698)* — bitmask DP. Kỹ năng: state là tập con.

## Tiêu chí hoàn thành
- [ ] Dựng được Trie và giải Word Search II.
- [ ] Dùng XOR/bitmask cho ít nhất 3 bài.
- [ ] Nhận diện đúng khi nào cần segment tree vs prefix sum; khi nào monotonic deque.

---

# Phase 16 — Mixed & Mô phỏng phỏng vấn

## Mục tiêu
Chuyển từ "biết từng pattern" sang "**nhìn đề lạ là phân loại được**" và trình bày như trong phỏng vấn thật: nói to suy nghĩ, brute force trước, tối ưu dần, phân tích độ phức tạp, viết code sạch, test bằng tay.

## Kiến thức cần học
- Quy trình phỏng vấn: làm rõ đề → ví dụ → brute force → tối ưu → code → test → phân tích.
- Trộn ngẫu nhiên các chủ đề (không biết trước pattern).
- System design cơ bản (cho Backend SE — học song song, ngoài phạm vi DSA).

## Pattern quan trọng
- **Pattern matching tổng hợp**: chạy "vòng lặp tư duy 5 câu hỏi" + đối chiếu toàn bộ dấu hiệu đã học.
- **Giao tiếp**: dẫn dắt người phỏng vấn qua suy nghĩ của mình.

## Dấu hiệu nhận biết
- Đây là phase *không* gợi ý pattern — mục tiêu là tự nhận diện. Hãy luôn bắt đầu bằng phân loại: "bài này về mảng/chuỗi? đồ thị? tối ưu hóa? sinh nghiệm?".

## Lỗi tư duy thường gặp
- Nhảy vào code trước khi làm rõ đề và ví dụ.
- Im lặng suy nghĩ (người phỏng vấn không thấy quá trình).
- Bỏ qua brute force → kẹt khi không ra ngay lời giải tối ưu.
- Không test edge case (rỗng, một phần tử, trùng, âm, tràn số).

## Mức độ khó
⭐⭐⭐–⭐⭐⭐⭐⭐ (trộn lẫn).

## Thời gian dự kiến
Liên tục — duy trì đến khi đi phỏng vấn (3–6 tuần tập trung).

## Bài tập
- **Random practice**: dùng chế độ ngẫu nhiên / danh sách trộn chủ đề, mỗi ngày 2–4 bài không biết trước pattern.
- **Mock interview**: tự bấm giờ 45 phút/bài, nói to, hoặc luyện với bạn/peer.
- **Bài tổng hợp / hệ thống thiết kế**: LRU Cache (146), Design Twitter (355), Insert/Delete/GetRandom O(1) (380), Time-Based Key-Value Store (981).
- **Company-tagged**: chọn theo công ty mục tiêu khi đã có lịch phỏng vấn.

## Tiêu chí hoàn thành
- [ ] Phân loại đúng pattern của một bài lạ trong < 2 phút ở đa số trường hợp.
- [ ] Hoàn thành một bài Medium trong ~25–30 phút có nói to suy nghĩ.
- [ ] Trình bày được mạch brute force → optimal + phân tích độ phức tạp trôi chảy.

---

# Cách dùng roadmap này (gợi ý nhịp học)

- **Mỗi pattern → học một template chuẩn, rồi luyện đến khi viết lại được từ trí nhớ.** Không thuộc lời giải, thuộc *khuôn tư duy*.
- **Quy tắc với mỗi bài:** dành 20–40 phút tự nghĩ (chạy vòng lặp 5 câu hỏi) trước khi xem gợi ý. Nếu phải xem lời giải, hôm sau **làm lại bài đó từ đầu** không nhìn.
- **Spaced repetition:** đánh dấu bài "phải xem lời giải" → ôn lại sau 1 ngày, 3 ngày, 1 tuần.
- **Nhật ký pattern:** ghi lại "dấu hiệu đề → pattern" mỗi khi gặp bài mới; đây chính là tài sản giúp nhận diện nhanh.
- **Đừng bỏ Phase 14 (DP) vội** — đây là chỗ phân loại ứng viên trong phỏng vấn.

---

## Những phần còn lại (làm tiếp khi bạn muốn)

Theo yêu cầu của bạn, tôi mới dựng phần roadmap theo phase. Khi bạn sẵn sàng, tôi có thể bổ sung:

- **Danh sách bài tập đầy đủ** từng phase (Top 100 bắt buộc / Top 200 nên làm / Top 300 cho phỏng vấn mạnh) — kèm lý do chọn, pattern, kỹ năng.
- **Hệ thống luyện tập chi tiết** mỗi phase: bài học → bài có hướng dẫn → tự làm → bài kiểm tra → bài tổng hợp → mini contest.
- **Kế hoạch theo tuần và theo tháng** (lịch cụ thể, số bài/ngày, mốc kiểm tra).
- **Sổ tay "Cách suy nghĩ" cho từng dạng bài**: cách phân tích input, tìm pattern, thiết kế thuật toán, tối ưu, debug, các biến thể & bài tương tự.
- **Interview Preparation Roadmap** riêng (behavioral + system design cơ bản cho Backend SE).