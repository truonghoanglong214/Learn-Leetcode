# Phase 4 — Prefix Sum & Hashing Nâng Cao

> **Ngôn ngữ:** C#
> **Mục tiêu:** Trả lời nhanh truy vấn tổng trên khoảng, đếm số subarray thỏa điều kiện tổng — kể cả khi có số âm (lúc này sliding window không dùng được).

---

## Bài học 1 — Prefix Sum 1D

### 1.1 Ý tưởng

```
Tiền xử lý một lần O(n): prefix[i] = tổng nums[0..i-1]
  → prefix[0] = 0  (sentinel quan trọng!)
  → prefix[i] = prefix[i-1] + nums[i-1]

Truy vấn tổng đoạn [l, r] (0-indexed) O(1):
  sum(l, r) = prefix[r+1] - prefix[l]
```

### 1.2 Template Prefix Sum

```csharp
int n = nums.Length;
int[] prefix = new int[n + 1];   // prefix[0] = 0
for (int i = 0; i < n; i++)
    prefix[i + 1] = prefix[i] + nums[i];

// Tổng đoạn [l, r] (0-indexed):
int rangeSum = prefix[r + 1] - prefix[l];
```

---

## Bài học 2 — Prefix Sum + HashMap (đếm subarray)

### 2.1 Ý tưởng

```
Tổng đoạn [i..j] = prefix[j+1] - prefix[i] = k
  ↔  prefix[j+1] - k = prefix[i]
  ↔  "đã từng thấy prefix[i] = (prefixHienTai - k) chưa?"

Duyệt từng vị trí j, dùng HashMap để đếm:
  số lần prefix đã xuất hiện → trả lời câu hỏi trên trong O(1).

Tại sao khởi tạo map với {0: 1}?
  → Để xử lý trường hợp chính đoạn [0..j] có tổng = k
    (prefix[0] = 0 đã "xuất hiện trước khi duyệt").
```

### 2.2 Template Prefix + HashMap

```csharp
var prefixCount = new Dictionary<int, int> { { 0, 1 } }; // {0:1} bắt buộc!
int prefixSum = 0;
int result = 0;

foreach (int num in nums)
{
    prefixSum += num;
    int complement = prefixSum - k;
    if (prefixCount.ContainsKey(complement))
        result += prefixCount[complement];
    prefixCount[prefixSum] = prefixCount.GetValueOrDefault(prefixSum, 0) + 1;
}
return result;
```

---

## Bài học 3 — Difference Array

### 3.1 Ý tưởng

```
Khi cần thực hiện nhiều thao tác "cộng v cho đoạn [l, r]" rồi hỏi kết quả:
  - Không làm từng ô O(n) mỗi lần → TLE nếu có q truy vấn.
  - Dùng diff[l] += v, diff[r+1] -= v → mỗi thao tác O(1).
  - Cuối cùng tính prefix sum của diff → kết quả tại mỗi vị trí O(n).
```

### 3.2 Template Difference Array

```csharp
int[] diff = new int[n + 1];

// Áp dụng thao tác: cộng v vào đoạn [l, r]
void RangeAdd(int l, int r, int v)
{
    diff[l] += v;
    diff[r + 1] -= v;
}

// Tính mảng kết quả
int[] result = new int[n];
int running = 0;
for (int i = 0; i < n; i++)
{
    running += diff[i];
    result[i] = running;
}
```

---

## Bài học 4 — Prefix Sum 2D

### 4.1 Công thức bao hàm–loại trừ

```
prefix[i][j] = tổng tất cả ô trong hình chữ nhật (0,0) → (i-1, j-1)

Xây dựng:
  prefix[i][j] = matrix[i-1][j-1]
               + prefix[i-1][j]      // hình chữ nhật bên trên
               + prefix[i][j-1]      // hình chữ nhật bên trái
               - prefix[i-1][j-1]    // bị cộng 2 lần

Truy vấn tổng hình chữ nhật (r1,c1) → (r2,c2):
  = prefix[r2+1][c2+1]
  - prefix[r1][c2+1]
  - prefix[r2+1][c1]
  + prefix[r1][c1]
```

---

## Bài tập theo thứ tự

### Easy

---

#### Bài 1: Range Sum Query – Immutable (LeetCode 303)

**Đề:** Cho `int[] nums`. Xây dựng một object `NumArray` với hàm `SumRange(int left, int right)` trả về tổng `nums[left] + ... + nums[right]`. Hàm này được gọi **nhiều lần**.

**Ví dụ:**
```
Input:  nums = [-2, 0, 3, -5, 2, -1]
        sumRange(0, 2) → 1       (= -2 + 0 + 3)
        sumRange(2, 5) → -1      (= 3 + -5 + 2 + -1)
        sumRange(0, 5) → -3

Input:  nums = [5]
        sumRange(0, 0) → 5
(Edge case: mảng 1 phần tử, truy vấn chính nó)

Input:  nums = [1, -1, 1, -1]
        sumRange(1, 2) → 0
(Edge case: số âm, kết quả = 0)
```

**Constraint:** `1 <= nums.length <= 10^4`, `-10^5 <= nums[i] <= 10^5`, `0 <= left <= right < nums.length`, tối đa `10^4` lần gọi `sumRange`.

**Phân tích 5 câu hỏi:**
1. Input: n ≤ 10⁴, có số âm, truy vấn nhiều lần
2. Output: tổng đoạn (int) cho mỗi truy vấn
3. Brute: mỗi truy vấn cộng vòng lặp O(n) → O(n·q) tổng cộng, quá chậm
4. Lãng phí: tính lại tổng từ đầu mỗi lần → tiền xử lý prefix O(n) để mỗi truy vấn O(1)
5. Pattern: **Prefix Sum** — xây prefix một lần, trả lời O(1)

```csharp
// Brute — O(n) mỗi truy vấn
public class NumArrayBrute
{
    private int[] _nums;
    public NumArrayBrute(int[] nums) => _nums = nums;

    public int SumRange(int left, int right)
    {
        int sum = 0;
        for (int i = left; i <= right; i++)
            sum += _nums[i];
        return sum;
    }
}
// Time: O(n) per query, Space: O(1)

// Optimal — O(1) mỗi truy vấn
public class NumArray
{
    private int[] _prefix;

    public NumArray(int[] nums)
    {
        _prefix = new int[nums.Length + 1];
        for (int i = 0; i < nums.Length; i++)
            _prefix[i + 1] = _prefix[i] + nums[i];
    }

    public int SumRange(int left, int right)
        => _prefix[right + 1] - _prefix[left];
}
// Build: O(n), Query: O(1), Space: O(n)
```

---

#### Bài 2: Find Pivot Index (LeetCode 724)

**Đề:** Cho `int[] nums`. Tìm **pivot index** — vị trí `i` sao cho tổng tất cả phần tử bên trái bằng tổng tất cả phần tử bên phải. Trả về index nhỏ nhất thỏa, hoặc -1 nếu không tồn tại.

**Ví dụ:**
```
Input:  nums = [1, 7, 3, 6, 5, 6]
Output: 3
Vì: trái = 1+7+3 = 11, phải = 5+6 = 11 ✓

Input:  nums = [1, 2, 3]
Output: -1
Vì: không có vị trí nào thỏa.

Input:  nums = [2, 1, -1]
Output: 0
(Edge case: pivot ở đầu mảng — tổng bên trái = 0 (rỗng), bên phải = 1 + -1 = 0)

Input:  nums = [-1, -1, -1, 0, 1, 1]
Output: 0
(Edge case: số âm, pivot ở đầu)
```

**Constraint:** `1 <= nums.length <= 10^4`, `-1000 <= nums[i] <= 1000`.

**Phân tích 5 câu hỏi:**
1. Input: n ≤ 10⁴, có số âm
2. Output: pivot index (int), -1 nếu không có
3. Brute: với mỗi i, tính tổng trái và phải O(n) → O(n²) tổng
4. Lãng phí: tính đi tính lại tổng trái → dùng biến `leftSum` cộng dần; tổng phải = `total - leftSum - nums[i]`
5. Pattern: **Prefix Sum một chiều** — dùng tổng toàn bộ để suy tổng phải

```csharp
// Brute — O(n²)
public int PivotIndexBrute(int[] nums)
{
    for (int i = 0; i < nums.Length; i++)
    {
        int leftSum = 0, rightSum = 0;
        for (int j = 0; j < i; j++) leftSum += nums[j];
        for (int j = i + 1; j < nums.Length; j++) rightSum += nums[j];
        if (leftSum == rightSum) return i;
    }
    return -1;
}

// Optimal — O(n) time, O(1) space
public int PivotIndex(int[] nums)
{
    int total = 0;
    foreach (int n in nums) total += n;

    int leftSum = 0;
    for (int i = 0; i < nums.Length; i++)
    {
        // rightSum = total - leftSum - nums[i]
        if (leftSum == total - leftSum - nums[i])
            return i;
        leftSum += nums[i];
    }
    return -1;
}
// Time: O(n), Space: O(1)
```

---

### Medium

---

#### Bài 3: Subarray Sum Equals K (LeetCode 560) ⭐ Bài bản lề

**Đề:** Cho `int[] nums` và `int k`. Đếm số **subarray liên tiếp** có tổng bằng k.

**Ví dụ:**
```
Input:  nums = [1, 1, 1], k = 2
Output: 2
Vì: [1,1] (index 0-1) và [1,1] (index 1-2).

Input:  nums = [1, 2, 3], k = 3
Output: 2
Vì: [3] (index 2) và [1, 2] (index 0-1).

Input:  nums = [-1, -1, 1], k = 0
Output: 1
(Edge case: số âm, subarray [-1, -1, 1] có tổng = -1)

Input:  nums = [1], k = 0
Output: 0
(Edge case: một phần tử, không có subarray nào tổng = 0)
```

**Constraint:** `1 <= nums.length <= 2 * 10^4`, `-1000 <= nums[i] <= 1000`, `-10^7 <= k <= 10^7`.

**Phân tích 5 câu hỏi:**
1. Input: n ≤ 2×10⁴, **có số âm** → sliding window không dùng được
2. Output: đếm số subarray (int)
3. Brute: duyệt mọi cặp (i, j), tính tổng O(n) → O(n³); dùng prefix thì O(n²)
4. Lãng phí: với mỗi prefix[j], cần đếm bao nhiêu prefix[i] đã xuất hiện sao cho `prefix[j] - prefix[i] = k` → dùng HashMap đếm prefix đã gặp
5. Pattern: **Prefix Sum + HashMap** — tra cứu "complement" trong O(1)

```csharp
// Brute — O(n²) (dùng prefix sum bên trong)
public int SubarraySumBrute(int[] nums, int k)
{
    int count = 0;
    for (int i = 0; i < nums.Length; i++)
    {
        int sum = 0;
        for (int j = i; j < nums.Length; j++)
        {
            sum += nums[j];
            if (sum == k) count++;
        }
    }
    return count;
}
// Time: O(n²), Space: O(1)

// Optimal — O(n) time, O(n) space
public int SubarraySum(int[] nums, int k)
{
    // {prefix_sum: số lần đã xuất hiện}
    var prefixCount = new Dictionary<int, int> { { 0, 1 } }; // {0:1} bắt buộc!
    int prefixSum = 0;
    int count = 0;

    foreach (int num in nums)
    {
        prefixSum += num;

        // Cần tìm: prefixSum - k đã xuất hiện bao nhiêu lần trước đây?
        if (prefixCount.TryGetValue(prefixSum - k, out int freq))
            count += freq;

        prefixCount[prefixSum] = prefixCount.GetValueOrDefault(prefixSum, 0) + 1;
    }
    return count;
}
// Time: O(n), Space: O(n)
```

**Tại sao `{0: 1}`?**
Nếu đoạn `[0..j]` đúng bằng k thì `prefix[j+1] - k = 0`. Phải có `0` trong map trước đó → khởi tạo `{0:1}`.

---

#### Bài 4: Contiguous Array (LeetCode 525)

**Đề:** Cho `int[] nums` chỉ chứa 0 và 1. Tìm **subarray liên tiếp dài nhất** có số lượng 0 bằng số lượng 1.

**Ví dụ:**
```
Input:  nums = [0, 1]
Output: 2
Vì: toàn bộ mảng có 1 số 0 và 1 số 1.

Input:  nums = [0, 1, 0]
Output: 2
Vì: [0, 1] hoặc [1, 0], không thể lấy cả 3 vì 2 số 0 ≠ 1 số 1.

Input:  nums = [0, 0, 1, 0, 0, 0, 1, 1]
Output: 6
(Edge case: phần tử 0 nhiều hơn)

Input:  nums = [0]
Output: 0
(Edge case: chỉ có 0, không thể cân bằng)
```

**Constraint:** `1 <= nums.length <= 10^5`, `nums[i]` chỉ là 0 hoặc 1.

**Phân tích 5 câu hỏi:**
1. Input: chỉ 0 và 1, n ≤ 10⁵
2. Output: độ dài dài nhất (int)
3. Brute: duyệt mọi cặp (i, j), đếm 0 và 1 trong đoạn → O(n³) hoặc O(n²)
4. Lãng phí: đang tính lại từ đầu → **biến đổi**: thay 0 → -1, bài trở thành "subarray dài nhất có tổng = 0" → dùng prefix + hashmap tìm vị trí đầu tiên thấy prefix đó
5. Pattern: **Chuẩn hóa + Prefix + HashMap** — biến đổi bài về dạng đã biết

```csharp
// Brute — O(n²)
public int FindMaxLengthBrute(int[] nums)
{
    int maxLen = 0;
    for (int i = 0; i < nums.Length; i++)
    {
        int count = 0;
        for (int j = i; j < nums.Length; j++)
        {
            count += nums[j] == 1 ? 1 : -1;
            if (count == 0)
                maxLen = Math.Max(maxLen, j - i + 1);
        }
    }
    return maxLen;
}

// Optimal — O(n) time, O(n) space
public int FindMaxLength(int[] nums)
{
    // Thay 0 → -1: bài quy về "subarray tổng = 0 dài nhất"
    // HashMap lưu: {prefix_sum: chỉ số đầu tiên thấy prefix đó}
    var firstSeen = new Dictionary<int, int> { { 0, -1 } }; // prefix=0 trước index 0
    int prefixSum = 0;
    int maxLen = 0;

    for (int i = 0; i < nums.Length; i++)
    {
        prefixSum += nums[i] == 1 ? 1 : -1;

        if (firstSeen.TryGetValue(prefixSum, out int prevIndex))
            maxLen = Math.Max(maxLen, i - prevIndex);
        else
            firstSeen[prefixSum] = i; // chỉ lưu lần ĐẦU TIÊN thấy
    }
    return maxLen;
}
// Time: O(n), Space: O(n)
```

**Kỹ năng then chốt:** Khởi tạo `{0: -1}` (không phải `{0: 1}` như bài đếm!) vì ở đây cần **index** để tính độ dài, không phải **đếm**.

---

#### Bài 5: Product of Array Except Self (LeetCode 238)

**Đề:** Cho `int[] nums`. Trả về mảng `answer` sao cho `answer[i]` bằng tích tất cả phần tử trong `nums` **ngoại trừ** `nums[i]`. Không được dùng phép chia. Yêu cầu O(n) time.

**Ví dụ:**
```
Input:  nums = [1, 2, 3, 4]
Output: [24, 12, 8, 6]
Vì: answer[0]=2*3*4=24, answer[1]=1*3*4=12, ...

Input:  nums = [-1, 1, 0, -3, 3]
Output: [0, 0, 9, 0, 0]
(Edge case: có số 0 → tất cả ngoại trừ vị trí của 0 đều = 0)

Input:  nums = [0, 0]
Output: [0, 0]
(Edge case: hai số 0)

Input:  nums = [2, -1]
Output: [-1, 2]
(Edge case: số âm, mảng nhỏ)
```

**Constraint:** `2 <= nums.length <= 10^5`, `-30 <= nums[i] <= 30`, tích bất kỳ đoạn con đảm bảo khớp 32-bit integer.

```csharp
// Brute — O(n²)
public int[] ProductExceptSelfBrute(int[] nums)
{
    int n = nums.Length;
    int[] result = new int[n];
    for (int i = 0; i < n; i++)
    {
        result[i] = 1;
        for (int j = 0; j < n; j++)
            if (j != i) result[i] *= nums[j];
    }
    return result;
}

// Optimal — O(n) time, O(1) extra space (ngoài mảng output)
public int[] ProductExceptSelf(int[] nums)
{
    int n = nums.Length;
    int[] answer = new int[n];

    // Bước 1: prefix product (tích trái)
    // answer[i] = tích tất cả phần tử bên trái nums[i]
    answer[0] = 1;
    for (int i = 1; i < n; i++)
        answer[i] = answer[i - 1] * nums[i - 1];

    // Bước 2: nhân thêm suffix product (tích phải) từ phải sang
    int rightProduct = 1;
    for (int i = n - 1; i >= 0; i--)
    {
        answer[i] *= rightProduct;
        rightProduct *= nums[i];
    }

    return answer;
}
// Time: O(n), Space: O(1) extra (mảng output không tính)
```

---

#### Bài 6: Range Sum Query 2D – Immutable (LeetCode 304)

**Đề:** Cho `int[][] matrix`. Xây dựng object `NumMatrix` với hàm `SumRegion(int row1, int col1, int row2, int col2)` trả về tổng tất cả phần tử trong hình chữ nhật góc trên-trái `(row1, col1)` và góc dưới-phải `(row2, col2)`.

**Ví dụ:**
```
Input:  matrix = [
          [3, 0, 1, 4, 2],
          [5, 6, 3, 2, 1],
          [1, 2, 0, 1, 5],
          [4, 1, 0, 1, 7],
          [1, 0, 3, 0, 5]
        ]
        sumRegion(2, 1, 4, 3) → 8
        sumRegion(1, 1, 2, 2) → 11
        sumRegion(1, 2, 2, 4) → 12

(Edge case: ô đơn lẻ)
        sumRegion(0, 0, 0, 0) → 3
```

**Constraint:** `m, n = 1..200`, `-10^4 <= matrix[i][j] <= 10^4`, tối đa `10^4` lần gọi.

```csharp
public class NumMatrix
{
    private int[][] _prefix;

    public NumMatrix(int[][] matrix)
    {
        int m = matrix.Length, n = matrix[0].Length;
        _prefix = new int[m + 1][];
        for (int i = 0; i <= m; i++)
            _prefix[i] = new int[n + 1];

        for (int i = 1; i <= m; i++)
            for (int j = 1; j <= n; j++)
                _prefix[i][j] = matrix[i-1][j-1]
                               + _prefix[i-1][j]
                               + _prefix[i][j-1]
                               - _prefix[i-1][j-1];
    }

    public int SumRegion(int r1, int c1, int r2, int c2)
        => _prefix[r2+1][c2+1]
         - _prefix[r1][c2+1]
         - _prefix[r2+1][c1]
         + _prefix[r1][c1];
}
// Build: O(m*n), Query: O(1), Space: O(m*n)
```

---

### Hard

---

#### Bài 7: Subarrays with K Different Integers (LeetCode 992)

**Đề:** Cho `int[] nums` và `int k`. Đếm số subarray liên tiếp có **đúng k** số nguyên khác nhau.

**Ví dụ:**
```
Input:  nums = [1, 2, 1, 2, 3], k = 2
Output: 7
Vì: [1,2],[2,1],[1,2],[2,3],[1,2,1],[2,1,2],[1,2,1,2]

Input:  nums = [1, 2, 1, 3, 4], k = 3
Output: 3
Vì: [1,2,1,3],[2,1,3],[1,3,4]

Input:  nums = [1], k = 1
Output: 1
(Edge case: một phần tử)

Input:  nums = [1, 2], k = 3
Output: 0
(Edge case: k lớn hơn số loại phần tử)
```

**Constraint:** `1 <= nums.length <= 2 * 10^4`, `1 <= nums[i] <= nums.length`, `1 <= k <= nums.length`.

**Phân tích 5 câu hỏi:**
1. Input: n ≤ 2×10⁴, `nums[i]` trong [1, n]
2. Output: đếm (int)
3. Brute: duyệt mọi cặp (i, j), dùng HashSet đếm distinct → O(n²)
4. Lãng phí: không thể làm variable window trực tiếp cho "đúng k" (điều kiện không đơn điệu) → dùng mẹo "đúng k = AtMost(k) − AtMost(k−1)"
5. Pattern: **Kỹ thuật trừ** + variable window đếm distinct ≤ k

```csharp
// AtMost(k): số subarray có nhiều nhất k loại khác nhau
private int AtMost(int[] nums, int k)
{
    var count = new Dictionary<int, int>();
    int left = 0, result = 0;

    for (int right = 0; right < nums.Length; right++)
    {
        // Thêm nums[right] vào cửa sổ
        count[nums[right]] = count.GetValueOrDefault(nums[right], 0) + 1;

        // Thu hẹp khi có > k loại
        while (count.Count > k)
        {
            int leftVal = nums[left];
            count[leftVal]--;
            if (count[leftVal] == 0) count.Remove(leftVal);
            left++;
        }

        // Mọi subarray kết thúc tại right, bắt đầu từ left đến right đều hợp lệ
        result += right - left + 1;
    }
    return result;
}

// Optimal — "đúng k" = AtMost(k) − AtMost(k−1)
public int SubarraysWithKDistinct(int[] nums, int k)
    => AtMost(nums, k) - AtMost(nums, k - 1);
// Time: O(n), Space: O(n)
```

---

## Tổng kết Pattern

| Dấu hiệu đề | Pattern |
|-------------|---------|
| "Tổng đoạn [l,r]", nhiều truy vấn | Prefix Sum 1D |
| "Tổng hình chữ nhật", nhiều truy vấn | Prefix Sum 2D |
| "Đếm subarray tổng = k", có số âm | Prefix + HashMap `{0:1}` |
| "Subarray dài nhất có tổng = 0 / cân bằng" | Prefix + HashMap lưu index đầu tiên `{0:-1}` |
| "Cộng giá trị cho nhiều khoảng" | Difference Array |
| "Đúng k loại" | AtMost(k) − AtMost(k−1) |

**Điểm phân biệt Sliding Window vs Prefix+HashMap:**
- Sliding window: điều kiện **đơn điệu** (số dương → tổng tăng đều) → thu hẹp cửa sổ có nghĩa.
- Prefix + HashMap: có **số âm** → tổng không đơn điệu → sliding window thất bại, phải dùng prefix.
