# Phase 0 — Complexity & Tư duy nền

> **Ngôn ngữ:** C#
> **Mục tiêu:** Đây là "thước đo" dùng cho mọi phase sau. Chưa cần giải bài thuật toán khó — chỉ cần đọc đề và bóc tách input/output, ước lượng độ phức tạp, dùng kích thước input để đoán thuật toán mục tiêu.

---

## Vòng lặp tư duy 5 câu hỏi

Trước **mọi** bài, chạy qua 5 câu hỏi này:

```
1. Input nói gì?
   - Mảng đã sắp xếp chưa?
   - Có trùng lặp không?
   - n là bao nhiêu? (n gợi ý độ phức tạp mục tiêu)
   - Giá trị có giới hạn không? (nhỏ → counting array; lớn → Dictionary)

2. Output cần gì?
   - Một số? Mảng? Tất cả tổ hợp? Đúng/sai? Tối ưu (min/max)?

3. Brute Force là gì?
   - Lời giải "ngu" nhất. Luôn nghĩ ra baseline này trước.

4. Chỗ lãng phí nằm đâu?
   - Đang tính lại cái gì? Đang quét lại cái gì?

5. Có pattern nào khớp không?
   - Đối chiếu dấu hiệu nhận biết của từng pattern.
```

---

## Bài học 1 — Big O Notation

### 1.1 Big O là gì?

Big O mô tả **tốc độ tăng** của thời gian (hoặc bộ nhớ) khi kích thước đầu vào `n` tăng.

```
Big O KHÔNG đo thời gian tuyệt đối (giây, ms).
Big O đo TỐC ĐỘ TĂNG so với n.
```

Ba ký hiệu:

| Ký hiệu  | Ý nghĩa                    | Dùng khi          |
| -------- | -------------------------- | ----------------- |
| O(f(n))  | Cận trên — tệ nhất         | Phỏng vấn dùng cái này |
| Ω(f(n))  | Cận dưới — tốt nhất        | Ít dùng           |
| Θ(f(n))  | Chính xác cả hai chiều     | Học thuật         |

> **Quy tắc:** Trong phỏng vấn, chỉ nói Big O là đủ.

### 1.2 Bảng Big O từ nhanh đến chậm

```
O(1) < O(log n) < O(√n) < O(n) < O(n log n) < O(n²) < O(n³) < O(2ⁿ) < O(n!)
```

Minh họa bằng số khi n = 1,000:

| Độ phức tạp | Số phép tính     |
| ----------- | ---------------- |
| O(1)        | 1                |
| O(log n)    | ~10              |
| O(√n)       | ~32              |
| O(n)        | 1,000            |
| O(n log n)  | ~10,000          |
| O(n²)       | 1,000,000        |
| O(2ⁿ)       | 2^1000 ≈ ∞       |

### 1.3 Quy tắc rút gọn Big O

**Quy tắc 1: Bỏ hằng số**
```csharp
// O(2n) → O(n)
for (int i = 0; i < n; i++) Console.WriteLine(i);
for (int j = 0; j < n; j++) Console.WriteLine(j);
```

**Quy tắc 2: Bỏ số hạng nhỏ hơn**
```csharp
// O(n² + n) → O(n²)
for (int i = 0; i < n; i++)
    for (int j = 0; j < n; j++) { }  // n²

for (int k = 0; k < n; k++) { }      // n (bỏ đi)
```

**Quy tắc 3: Vòng lặp lồng nhau = nhân**
```csharp
// O(n * m)
for (int i = 0; i < n; i++)
    for (int j = 0; j < m; j++) { }
```

**Quy tắc 4: Tuần tự = cộng rồi lấy cái lớn hơn**
```csharp
// O(n) + O(m) = O(n + m)
// Không rút gọn nếu n và m độc lập nhau.
```

---

## Bài học 2 — Phân tích Time Complexity

### 2.1 Các dạng vòng lặp

**Vòng lặp đơn → O(n)**
```csharp
int SumArray(int[] arr)
{
    int total = 0;
    foreach (int x in arr)   // n lần
        total += x;
    return total;
}
// Time: O(n), Space: O(1)
```

**Vòng lặp lồng → O(n²)**
```csharp
bool HasPair(int[] arr, int target)
{
    int n = arr.Length;
    for (int i = 0; i < n; i++)           // n lần
        for (int j = i + 1; j < n; j++)   // ~n lần
            if (arr[i] + arr[j] == target)
                return true;
    return false;
}
// Time: O(n²), Space: O(1)
```

**Chia đôi mỗi bước → O(log n)**
```csharp
int BinarySearch(int[] arr, int target)
{
    int lo = 0, hi = arr.Length - 1;
    while (lo <= hi)
    {
        int mid = lo + (hi - lo) / 2;    // tránh tràn số
        if (arr[mid] == target) return mid;
        else if (arr[mid] < target) lo = mid + 1;
        else hi = mid - 1;
    }
    return -1;
}
// Time: O(log n), Space: O(1)
// Tại sao log n?
// Mỗi bước bỏ đi HALF số phần tử còn lại.
// n → n/2 → n/4 → ... → 1: mất log₂(n) bước.
```

**Đệ quy nhị phân → O(2ⁿ)**
```csharp
int Fib(int n)
{
    if (n <= 1) return n;
    return Fib(n - 1) + Fib(n - 2);  // 2 lời gọi mỗi bước
}
// Time: O(2ⁿ) — cây đệ quy có 2ⁿ nút
// Space: O(n) — call stack sâu n tầng
```

### 2.2 Phân tích từng dòng

```csharp
List<int> Example(int[] arr)
{
    int n = arr.Length;             // O(1)
    var result = new List<int>();   // O(1)

    for (int i = 0; i < n; i++)    // O(n) lần lặp
    {
        int val = arr[i] * 2;      // O(1) mỗi lần
        result.Add(val);            // O(1) amortized
    }

    result.Sort();                  // O(n log n) ← ẨN HÌNH!

    return result;
}
// Tổng: O(n) + O(n log n) = O(n log n)
// ⚠ Thường quên tính Sort() ẩn bên trong!
```

### 2.3 Bài tập phân tích Time Complexity

**Bài 1:**
```csharp
int Mystery1(int n)
{
    int count = 0;
    int i = 1;
    while (i < n)
    {
        count++;
        i *= 2;       // i tăng theo lũy thừa 2
    }
    return count;
}
```

<details>
<summary>Đáp án</summary>

`i` đi qua: 1, 2, 4, 8, ..., 2^k cho đến khi 2^k ≥ n.
Số bước = log₂(n) → **O(log n)**
</details>

**Bài 2:**
```csharp
bool Mystery2(int[] arr)
{
    int n = arr.Length;
    for (int i = 0; i < n; i++)
        for (int j = 0; j < n; j++)
            if (arr[i] == arr[j] && i != j)
                return true;
    return false;
}
```

<details>
<summary>Đáp án</summary>

Hai vòng lặp lồng, mỗi cái chạy n lần → **O(n²)** time, **O(1)** space
</details>

**Bài 3:**
```csharp
bool Mystery3(int[] arr)
{
    var seen = new HashSet<int>();
    foreach (int x in arr)          // O(n)
    {
        if (seen.Contains(x))       // O(1) trung bình
            return true;
        seen.Add(x);                // O(1) trung bình
    }
    return false;
}
```

<details>
<summary>Đáp án</summary>

Vòng lặp n lần, mỗi lần O(1) → **O(n)** time, **O(n)** space (seen chứa tối đa n phần tử)
</details>

**Bài 4:**
```csharp
int SumDigits(int n)
{
    if (n < 10) return n;
    return n % 10 + SumDigits(n / 10);
}
```

<details>
<summary>Đáp án</summary>

Mỗi lần gọi, n chia 10. Số lần gọi = số chữ số của n = log₁₀(n) → **O(log n)**
</details>

---

## Bài học 3 — Space Complexity

### 3.1 Space là gì?

Space complexity = bộ nhớ **thêm** mà thuật toán cần (không tính input).

```csharp
// O(1) Space — không tạo thêm gì đáng kể
int FindMax(int[] arr)
{
    int maxVal = arr[0];
    foreach (int x in arr)
        if (x > maxVal) maxVal = x;
    return maxVal;
}

// O(n) Space — tạo mảng mới kích thước n
int[] DuplicateArray(int[] arr) => (int[])arr.Clone();

// O(n) Space — call stack sâu n tầng
int Factorial(int n) => n == 0 ? 1 : n * Factorial(n - 1);
```

### 3.2 Đánh đổi: Space ↔ Time

Đây là nguyên lý cốt lõi của Phase 1:

```
Dùng thêm bộ nhớ O(n) space → giảm thời gian O(n²) → O(n)
```

```csharp
// Brute Force: O(n²) time, O(1) space
bool HasDuplicateBrute(int[] arr)
{
    for (int i = 0; i < arr.Length; i++)
        for (int j = i + 1; j < arr.Length; j++)  // quét lại từ đầu
            if (arr[i] == arr[j]) return true;
    return false;
}

// Tối ưu: O(n) time, O(n) space
bool HasDuplicateOptimal(int[] arr)
{
    var seen = new HashSet<int>();    // dùng O(n) bộ nhớ
    foreach (int x in arr)
    {
        if (!seen.Add(x))            // Add trả về false nếu đã tồn tại
            return true;
    }
    return false;
}
```

---

## Bài học 4 — Amortized Complexity

### 4.1 List<T>.Add() — tại sao O(1) amortized?

```csharp
var list = new List<int>();
for (int i = 0; i < n; i++)
    list.Add(i);   // trung bình O(1) mỗi lần Add
```

Khi `List<T>` đầy, nó tạo mảng mới gấp đôi và copy toàn bộ:

```
Capacity: 1 → 2 → 4 → 8 → 16 → ...
Chi phí copy: 1 + 2 + 4 + ... + N = 2N - 1 ≈ 2N
```

→ N lần Add, tổng chi phí O(N) → mỗi Add trung bình **O(1) amortized**.

### 4.2 Dictionary / HashSet

- **Trường hợp thường:** O(1) tra cứu và thêm
- **Trường hợp xấu nhất:** O(n) nếu hash collision (hiếm gặp)
- **Amortized:** O(1) — trong phỏng vấn, nói O(1) là đúng

```csharp
var dict = new Dictionary<int, int>();
dict[key] = value;          // O(1) amortized
dict.ContainsKey(key);      // O(1) amortized
dict.TryGetValue(key, out var val); // O(1) amortized

var set = new HashSet<int>();
set.Add(x);                 // O(1) amortized
set.Contains(x);            // O(1) amortized
```

---

## Bài học 5 — Best / Average / Worst Case

```csharp
int LinearSearch(int[] arr, int target)
{
    for (int i = 0; i < arr.Length; i++)
        if (arr[i] == target) return i;
    return -1;
}
```

| Case    | Điều kiện                        | Complexity |
| ------- | -------------------------------- | ---------- |
| Best    | target là phần tử đầu tiên       | O(1)       |
| Average | target ở giữa mảng              | O(n/2) = O(n) |
| Worst   | target không có hoặc ở cuối      | O(n)       |

> **Phỏng vấn luôn hỏi Worst Case.** Khi nói "O(n)" là ngầm hiểu worst case.

### Ví dụ: Quick Sort trong C#

`Array.Sort()` dùng Introsort (kết hợp Quick Sort + Heap Sort + Insertion Sort):

| Case    | Khi nào                              | Complexity  |
| ------- | ------------------------------------ | ----------- |
| Best    | Pivot chia đều mỗi lần               | O(n log n)  |
| Average | Pivot ngẫu nhiên                     | O(n log n)  |
| Worst   | Introsort tự chuyển sang Heap Sort   | O(n log n)  |

---

## Bài học 6 — Quy tắc vàng: n → Thuật toán

### 6.1 Bảng quy tắc vàng

| Constraint n | Độ phức tạp mục tiêu | Thuật toán thường dùng         |
| ------------ | -------------------- | ------------------------------ |
| n ≤ 10       | O(n!)                | Permutation, brute force       |
| n ≤ 20       | O(2ⁿ)                | Backtracking, bitmask DP       |
| n ≤ 500      | O(n³)                | Triple loop, Floyd-Warshall    |
| n ≤ 5,000    | O(n²)                | DP 2D, nested loops            |
| n ≤ 10⁵     | O(n log n)           | Sort, BFS/DFS, Binary Search   |
| n ≤ 10⁶     | O(n)                 | Hash, Two Pointers, Sliding Window |
| n ≤ 10⁹     | O(log n) hoặc O(1)   | Binary Search, Math            |

### 6.2 Cách dùng bảng này

```
Đề cho: 2 <= nums.length <= 10^5, time limit 1–2 giây
→ Máy tính ~10⁸ phép tính/giây
→ O(n²) = 10¹⁰ phép tính → TLE chắc chắn
→ Cần O(n) hoặc O(n log n)
```

### 6.3 Ví dụ áp dụng

**Đề:** "Cho mảng n số nguyên (n ≤ 10⁵), tìm hai số có tổng bằng target."
- n ≤ 10⁵ → cần O(n) hoặc O(n log n)
- Brute force O(n²) = 10¹⁰ → TLE
- Nghĩ: dùng `Dictionary` để tra O(1) → đạt O(n)

**Đề:** "n ≤ 20, đếm tất cả tập con thỏa điều kiện."
- n ≤ 20 → O(2ⁿ) = 2²⁰ ≈ 10⁶ → ổn
- Dùng backtracking hoặc bitmask

---

## Bài học 7 — Đọc Constraint trên LeetCode

### 7.1 Các constraint thường gặp và ý nghĩa

```
1 <= nums.Length <= 10^5       → n lớn, cần O(n) hoặc O(n log n)
-10^9 <= nums[i] <= 10^9       → miền giá trị lớn → dùng Dictionary
0 <= nums[i] <= 10^4           → miền nhỏ → CÓ THỂ dùng int[] count = new int[10001]
All elements are distinct      → không cần xử lý trùng lặp
Sorted in non-decreasing order → có thể dùng Binary Search / Two Pointers
```

### 7.2 Template phân tích đề

```
Bài: [Tên bài]
Input: [Mô tả input, constraint]
Output: [Cần trả về gì]

--- 5 câu hỏi ---
1. Input nói gì?
   - Kích thước n: ___
   - Sắp xếp chưa? ___
   - Có trùng lặp? ___
   - Miền giá trị: ___

2. Output cần gì?
   - ___

3. Brute Force?
   - Làm gì? ___
   - Code sơ bộ: ___
   - Độ phức tạp: O(___)

4. Chỗ lãng phí?
   - Đang lặp lại: ___
   - Có thể tối ưu bằng: ___

5. Pattern?
   - Dấu hiệu nhận biết khớp với: ___
   - Độ phức tạp mục tiêu: O(___)
```

---

## Bài học 8 — Các lỗi tư duy thường gặp

### Lỗi 1: Quên Sort ẩn làm tăng complexity

```csharp
IList<IList<string>> GroupAnagrams(string[] strs)
{
    var map = new Dictionary<string, List<string>>();
    foreach (var s in strs)
    {
        char[] chars = s.ToCharArray();
        Array.Sort(chars);                    // ← O(k log k), k = độ dài chuỗi
        string key = new string(chars);
        // ...
    }
    // Đây KHÔNG phải O(n), mà là O(n * k log k)!
}
```

### Lỗi 2: Nhầm O(n log n) với O(n)

```csharp
Array.Sort(arr);              // O(n log n) — KHÔNG phải O(n)
// Nếu thêm phần tử vào List đã sort rồi tìm vị trí → O(n) mỗi lần
```

### Lỗi 3: Quên tính Space của call stack đệ quy

```csharp
void DFS(TreeNode node)
{
    if (node == null) return;
    DFS(node.left);
    DFS(node.right);
}
// Space: O(h), h = chiều cao cây
// Worst case (cây lệch): h = n → O(n) space
```

### Lỗi 4: Đọc đề vội, bỏ sót constraint

Ví dụ đề: `"Given a sorted array of distinct integers..."`
- **"sorted"** → có thể dùng Binary Search hoặc Two Pointers
- **"distinct"** → không cần xử lý trùng lặp

Bỏ sót "sorted" → nghĩ ra O(n) thay vì O(log n) là đã đủ, lãng phí cải tiến.

---

## Bài tập tổng hợp Phase 0

### Bài tập 1: Ước lượng Big O (10 đoạn code C#)

```csharp
// 1.
for (int i = 0; i < n; i++) { }
// Time: ?  Space: ?

// 2.
for (int i = 0; i < n; i++)
    for (int j = 0; j < n; j++)
        for (int k = 0; k < n; k++) { }
// Time: ?  Space: ?

// 3.
int i = n;
while (i > 0) i /= 2;
// Time: ?  Space: ?

// 4.
int Rec(int n) => n <= 1 ? 1 : Rec(n - 1) + Rec(n - 1);
// Time: ?  Space: ?

// 5.
int Rec2(int n) => n <= 1 ? 1 : Rec2(n - 1) + 1;
// Time: ?  Space: ?

// 6.
int[] arr = { 5, 3, 1, 4, 2 };
Array.Sort(arr);
// Time: ?  Space: ?

// 7.
var seen = new HashSet<int>();
foreach (int x in arr) seen.Add(x);
// Time: ?  Space: ?

// 8.
for (int i = 0; i < n; i++)
    for (int j = i + 1; j < n; j++)
        if (arr[i] > arr[j]) (arr[i], arr[j]) = (arr[j], arr[i]);
// Time: ?  Space: ?

// 9.
var sorted = new List<int>();
foreach (int x in arr)
{
    int pos = sorted.BinarySearch(x);
    sorted.Insert(pos < 0 ? ~pos : pos, x);  // Insert dịch chuyển phần tử
}
// Time: ?  Space: ?

// 10. Merge Sort
int[] MergeSort(int[] arr)
{
    if (arr.Length <= 1) return arr;
    int mid = arr.Length / 2;
    var left  = MergeSort(arr[..mid]);    // n/2 phần tử
    var right = MergeSort(arr[mid..]);    // n/2 phần tử
    return Merge(left, right);            // O(n)
}
// Time: ?  Space: ?
```

<details>
<summary>Đáp án</summary>

1. O(n) time, O(1) space
2. O(n³) time, O(1) space
3. O(log n) time, O(1) space — i giảm đi nửa mỗi bước
4. O(2ⁿ) time, O(n) space — cây đệ quy 2 nhánh, sâu n tầng
5. O(n) time, O(n) space — chuỗi đệ quy thẳng, stack sâu n
6. O(n log n) time, O(log n) space (Introsort dùng stack đệ quy)
7. O(n) time, O(n) space
8. O(n²) time, O(1) space — bubble sort
9. O(n²) time, O(n) space — mỗi Insert mất O(n) để dịch chuyển phần tử
10. O(n log n) time, O(n) space — T(n)=2T(n/2)+O(n) → Master Theorem → O(n log n)
</details>

---

### Bài tập 2: Phân tích 5 bài Easy với template 5 câu hỏi

**Không cần code tối ưu — chỉ cần phân tích.**

**Bài 2.1 — Two Sum (LeetCode 1)**
```
Cho int[] nums và int target.
Trả về int[] gồm hai chỉ số [i, j] sao cho nums[i] + nums[j] == target.
Constraint: 2 <= n <= 10^4, -10^9 <= nums[i] <= 10^9
Đảm bảo có đúng một đáp án. Không dùng cùng một phần tử hai lần.
```

<details>
<summary>Phân tích mẫu</summary>

**1. Input nói gì?**
- n ≤ 10⁴ → O(n²) có thể chấp nhận, nhưng O(n) tốt hơn
- Không nói sorted → chưa sắp xếp
- Distinct không được đảm bảo → có thể có trùng

**2. Output cần gì?**
- Hai chỉ số (không phải giá trị)
- Không được dùng lại cùng index

**3. Brute Force?**
```csharp
for (int i = 0; i < n; i++)
    for (int j = i + 1; j < n; j++)   // O(n²)
        if (nums[i] + nums[j] == target)
            return new[] { i, j };
```
Độ phức tạp: O(n²)

**4. Chỗ lãng phí?**
- Với mỗi `nums[i]`, đang quét lại toàn bộ để tìm `target - nums[i]`
- Có thể nhớ lại các số đã thấy bằng `Dictionary<int, int>` (value → index)

**5. Pattern?**
- "Tìm cặp" + tra cứu nhanh → **Complement Lookup**
- Độ phức tạp mục tiêu: O(n) time, O(n) space
</details>

---

**Bài 2.2 — Valid Anagram (LeetCode 242)**
```
Cho string s và string t.
Trả về true nếu t là anagram của s.
Constraint: 1 <= s.length, t.length <= 5 * 10^4, lowercase English letters
```

<details>
<summary>Phân tích mẫu</summary>

**1. Input nói gì?**
- n ≤ 5 * 10⁴ → O(n) ổn
- Chỉ lowercase a-z → miền giá trị = 26 → có thể dùng `int[26]`

**2. Output cần gì?**
- Boolean

**3. Brute Force?**
- Sort cả hai rồi so sánh: O(n log n)

**4. Chỗ lãng phí?**
- Sort không cần thiết
- Chỉ cần đếm tần suất từng ký tự → `int[26]` thay Dictionary

**5. Pattern?**
- "So sánh tần suất ký tự" → **Frequency Counting**
- Miền nhỏ (26) → `int[26]` thay Dictionary → O(n) time, O(1) space
</details>

---

**Bài 2.3 — Contains Duplicate (LeetCode 217)**
```
Cho int[] nums.
Trả về true nếu có bất kỳ phần tử nào xuất hiện ≥ 2 lần.
Constraint: 1 <= n <= 10^5, -10^9 <= nums[i] <= 10^9
```

<details>
<summary>Phân tích mẫu</summary>

**1. Input nói gì?**
- n ≤ 10⁵ → cần O(n) hoặc O(n log n)
- Miền giá trị ±10⁹ → phải dùng HashSet<int>

**2. Output cần gì?**
- Boolean

**3. Brute Force?**
- Hai vòng lặp lồng: O(n²) → với n=10⁵ là TLE

**4. Chỗ lãng phí?**
- Với mỗi phần tử, đang quét lại toàn bộ phần trước
- Dùng `HashSet<int>` để tra cứu O(1)

**5. Pattern?**
- "Có trùng lặp không?" → **Seen-set**
- Độ phức tạp: O(n) time, O(n) space
</details>

---

### Bài tập 3: Dự đoán thuật toán từ constraint

| Đề | Constraint | Độ phức tạp mục tiêu | Gợi ý |
| -- | ---------- | -------------------- | ----- |
| Tìm cặp có tổng = target | n ≤ 10⁵ | ? | ? |
| Đường đi ngắn nhất (đồ thị có n đỉnh) | n ≤ 500 | ? | ? |
| Sinh tất cả tập con | n ≤ 20 | ? | ? |
| Sort mảng | n ≤ 10⁶ | ? | ? |
| Đếm cách phân hoạch tổng | n ≤ 1000 | ? | ? |
| Tìm trong mảng sorted | n ≤ 10⁹ (giá trị) | ? | ? |

<details>
<summary>Đáp án</summary>

| Đề | Độ phức tạp mục tiêu | Gợi ý |
| -- | -------------------- | ----- |
| Tìm cặp tổng = target | O(n) | Dictionary (Two Sum) |
| Đường đi ngắn nhất | O(n³) | Floyd-Warshall |
| Sinh tất cả tập con | O(2ⁿ) | Backtracking / Bitmask |
| Sort mảng | O(n log n) | Array.Sort() |
| Đếm cách phân hoạch | O(n²) | DP 2D |
| Tìm trong mảng sorted | O(log n) | Binary Search |
</details>

---

## Tiêu chí hoàn thành Phase 0

- [ ] Nhìn một đoạn code bất kỳ, nói đúng Big O time & space trong < 30 giây.
- [ ] Nhìn constraint của một bài, đoán đúng độ phức tạp mục tiêu.
- [ ] Viết được brute force + phân tích 5 câu hỏi cho 5 bài mà không nhìn lời giải.
- [ ] Phân biệt được khi nào `int[]` (counting array) vs `Dictionary` / `HashSet`.
- [ ] Giải thích được tại sao `List<T>.Add()` là O(1) amortized.

---

**Thời gian dự kiến:** 3–5 ngày
**Mức độ:** ⭐ (Nền tảng — không code nhiều, chủ yếu phân tích)
