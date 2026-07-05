# Phase 5 — Binary Search

> **Ngôn ngữ:** C#
> **Mục tiêu:** Tìm kiếm O(log n) trên mảng sắp xếp, và — quan trọng hơn — **binary search trên không gian đáp án** (search the answer): nhìn ra tính đơn điệu của bài toán, viết hàm `Feasible(x)`, rồi binary search trên miền giá trị.

---

## Bài học 1 — Binary Search Chuẩn

### 1.1 Ý tưởng

```
Điều kiện bắt buộc: mảng (hoặc không gian tìm kiếm) đã sắp xếp.

Chiến lược:
  - Duy trì khoảng [lo, hi] còn khả năng chứa đáp án.
  - Mỗi bước kiểm tra mid = lo + (hi - lo) / 2.
  - Thu hẹp một nửa khoảng tùy kết quả so sánh.
  - Kết thúc khi lo > hi (không tìm thấy) hoặc tìm thấy.

Tại sao mid = lo + (hi - lo) / 2 thay vì (lo + hi) / 2?
  → Tránh tràn số int khi lo + hi > int.MaxValue.
```

### 1.2 Template — Tìm chính xác

```csharp
int lo = 0, hi = nums.Length - 1;

while (lo <= hi)
{
    int mid = lo + (hi - lo) / 2;
    if (nums[mid] == target)
        return mid;
    else if (nums[mid] < target)
        lo = mid + 1;
    else
        hi = mid - 1;
}
return -1; // không tìm thấy
```

---

## Bài học 2 — Lower Bound & Upper Bound

### 2.1 Lower Bound — vị trí đầu tiên ≥ target

```csharp
// Trả về chỉ số đầu tiên sao cho nums[i] >= target
// Nếu tất cả < target → trả về nums.Length
int LowerBound(int[] nums, int target)
{
    int lo = 0, hi = nums.Length; // hi = Length (không phải Length-1!)

    while (lo < hi)               // dừng khi lo == hi
    {
        int mid = lo + (hi - lo) / 2;
        if (nums[mid] < target)
            lo = mid + 1;
        else
            hi = mid;             // mid có thể là đáp án → không bỏ
    }
    return lo;
}
```

### 2.2 Upper Bound — vị trí đầu tiên > target

```csharp
// Trả về chỉ số đầu tiên sao cho nums[i] > target
int UpperBound(int[] nums, int target)
{
    int lo = 0, hi = nums.Length;

    while (lo < hi)
    {
        int mid = lo + (hi - lo) / 2;
        if (nums[mid] <= target)
            lo = mid + 1;
        else
            hi = mid;
    }
    return lo;
}
```

**Ứng dụng:** Số lần xuất hiện của `target` = `UpperBound(target) - LowerBound(target)`.

---

## Bài học 3 — Binary Search the Answer

### 3.1 Ý tưởng

```
Dùng khi:
  - Đáp án nằm trong một khoảng [lo, hi] liên tục.
  - Tồn tại hàm Feasible(x): "x có thỏa yêu cầu không?" — trả về bool.
  - Feasible có tính đơn điệu: nếu x thỏa thì mọi x' > x cũng thỏa
    (hoặc ngược lại — tùy bài).

Dấu hiệu nhận ra: đề hỏi "giá trị nhỏ nhất / lớn nhất sao cho có thể..."
  → Thử binary search trên miền giá trị đó, viết Feasible(x) kiểm tra.
```

### 3.2 Template — Tìm giá trị nhỏ nhất thỏa

```csharp
int lo = MIN_VALUE, hi = MAX_VALUE;

while (lo < hi)
{
    int mid = lo + (hi - lo) / 2;
    if (Feasible(mid))
        hi = mid;   // mid thỏa → thử tìm nhỏ hơn
    else
        lo = mid + 1; // mid không thỏa → phải lớn hơn
}
return lo; // lo == hi là đáp án nhỏ nhất thỏa
```

### 3.3 Template — Tìm giá trị lớn nhất thỏa

```csharp
while (lo < hi)
{
    int mid = lo + (hi - lo + 1) / 2; // +1 để tránh vòng lặp vô hạn khi lo+1==hi
    if (Feasible(mid))
        lo = mid;   // mid thỏa → thử tìm lớn hơn
    else
        hi = mid - 1;
}
return lo;
```

---

## Bài tập theo thứ tự

### Easy

---

#### Bài 1: Binary Search (LeetCode 704)

**Đề:** Cho `int[] nums` đã sắp xếp tăng dần (không trùng) và `int target`. Trả về chỉ số của target trong mảng, hoặc -1 nếu không tồn tại. Yêu cầu O(log n).

**Ví dụ:**
```
Input:  nums = [-1, 0, 3, 5, 9, 12], target = 9
Output: 4

Input:  nums = [-1, 0, 3, 5, 9, 12], target = 2
Output: -1

Input:  nums = [5], target = 5
Output: 0
(Edge case: mảng 1 phần tử, tìm thấy)

Input:  nums = [5], target = 3
Output: -1
(Edge case: mảng 1 phần tử, không tìm thấy)
```

**Constraint:** `1 <= nums.length <= 10^4`, `-10^4 <= nums[i], target <= 10^4`, tất cả phần tử phân biệt, mảng tăng dần.

**Phân tích 5 câu hỏi:**
1. Input: mảng **đã sắp xếp**, không trùng, n ≤ 10⁴
2. Output: chỉ số (int) hoặc -1
3. Brute: duyệt tuyến tính O(n) — luôn dùng được nhưng không tận dụng thứ tự
4. Lãng phí: không tận dụng thứ tự → mỗi lần so sánh có thể loại một nửa
5. Pattern: **Binary Search chuẩn**

```csharp
// Brute — O(n)
public int SearchBrute(int[] nums, int target)
{
    for (int i = 0; i < nums.Length; i++)
        if (nums[i] == target) return i;
    return -1;
}

// Optimal — O(log n) time, O(1) space
public int Search(int[] nums, int target)
{
    int lo = 0, hi = nums.Length - 1;

    while (lo <= hi)
    {
        int mid = lo + (hi - lo) / 2;
        if (nums[mid] == target)
            return mid;
        else if (nums[mid] < target)
            lo = mid + 1;
        else
            hi = mid - 1;
    }
    return -1;
}
// Time: O(log n), Space: O(1)
```

---

#### Bài 2: First Bad Version (LeetCode 278)

**Đề:** Có n phiên bản [1..n]. Từ một phiên bản bad nào đó trở đi tất cả đều bad. Hàm `IsBadVersion(version)` đã cho. Tìm phiên bản bad đầu tiên với số lần gọi API tối thiểu.

**Ví dụ:**
```
Input:  n = 5, bad = 4
Output: 4
Vì: IsBadVersion(3) = false, IsBadVersion(4) = true
    → phiên bản 4 là bad đầu tiên.

Input:  n = 1, bad = 1
Output: 1
(Edge case: chỉ có một phiên bản, nó là bad)

Input:  n = 10^9, bad = 1
Output: 1
(Edge case: bad đầu tiên ngay phiên bản 1 — lo = hi = 1 ngay từ đầu)
```

**Constraint:** `1 <= bad <= n <= 2^31 - 1`.

**Phân tích 5 câu hỏi:**
1. Input: n rất lớn (≤ 2³¹−1) → O(n) TLE chắc chắn; phải O(log n)
2. Output: phiên bản đầu tiên bad (int)
3. Brute: gọi IsBadVersion từ 1 đến n → O(n)
4. Lãng phí: không tận dụng tính đơn điệu (false...false, true...true) → binary search
5. Pattern: **Lower Bound** — tìm vị trí đầu tiên IsBadVersion = true

```csharp
// Brute — O(n) gọi API
public int FirstBadVersionBrute(int n)
{
    for (int i = 1; i <= n; i++)
        if (IsBadVersion(i)) return i;
    return n;
}

// Optimal — O(log n)
public int FirstBadVersion(int n)
{
    int lo = 1, hi = n; // hi = n vì phiên bản đánh từ 1

    while (lo < hi) // dừng khi lo == hi (đáp án duy nhất)
    {
        int mid = lo + (hi - lo) / 2;
        if (IsBadVersion(mid))
            hi = mid;     // mid có thể là đáp án → không bỏ
        else
            lo = mid + 1; // mid chắc chắn không phải → bỏ
    }
    return lo;
}
// Time: O(log n), Space: O(1)
```

---

### Medium

---

#### Bài 3: Search in Rotated Sorted Array (LeetCode 33) ⭐ Bài bản lề

**Đề:** Cho `int[] nums` — mảng đã sắp xếp tăng dần rồi bị **xoay** tại một vị trí không biết (phần tử phân biệt). Tìm `target`, trả về chỉ số hoặc -1.

**Ví dụ:**
```
Input:  nums = [4, 5, 6, 7, 0, 1, 2], target = 0
Output: 4

Input:  nums = [4, 5, 6, 7, 0, 1, 2], target = 3
Output: -1

Input:  nums = [1], target = 0
Output: -1
(Edge case: mảng 1 phần tử, không tìm thấy)

Input:  nums = [3, 1], target = 1
Output: 1
(Edge case: mảng 2 phần tử bị xoay, tìm ở nửa phải)
```

**Constraint:** `1 <= nums.length <= 5000`, `-10^4 <= nums[i], target <= 10^4`, phần tử phân biệt.

**Phân tích 5 câu hỏi:**
1. Input: mảng đã sắp xếp nhưng bị **xoay** → không thể binary search ngây thơ
2. Output: chỉ số (int) hoặc -1
3. Brute: duyệt tuyến tính O(n)
4. Lãng phí: không tận dụng cấu trúc → key insight: **luôn có một nửa là sắp xếp thẳng**; xác định nửa nào sắp xếp, kiểm tra target có thuộc nửa đó không
5. Pattern: **Binary Search có biến thể** — lập luận nửa nào sắp xếp

```csharp
// Brute — O(n)
public int SearchBrute(int[] nums, int target)
{
    for (int i = 0; i < nums.Length; i++)
        if (nums[i] == target) return i;
    return -1;
}

// Optimal — O(log n)
public int Search(int[] nums, int target)
{
    int lo = 0, hi = nums.Length - 1;

    while (lo <= hi)
    {
        int mid = lo + (hi - lo) / 2;

        if (nums[mid] == target) return mid;

        // Xác định nửa nào đang sắp xếp thẳng
        if (nums[lo] <= nums[mid]) // nửa trái [lo..mid] sắp xếp thẳng
        {
            // Target có nằm trong nửa trái không?
            if (nums[lo] <= target && target < nums[mid])
                hi = mid - 1; // nằm trong nửa trái
            else
                lo = mid + 1; // nằm ở nửa phải (hoặc không tồn tại)
        }
        else // nửa phải [mid..hi] sắp xếp thẳng
        {
            if (nums[mid] < target && target <= nums[hi])
                lo = mid + 1; // nằm trong nửa phải
            else
                hi = mid - 1;
        }
    }
    return -1;
}
// Time: O(log n), Space: O(1)
```

**Lập luận cốt lõi:** Mảng bị xoay có điểm gãy. Khi cắt tại mid, **một trong hai nửa luôn sắp xếp thẳng**. Dùng điều kiện `nums[lo] <= nums[mid]` để xác định nửa nào. Sau đó hỏi: "target có nằm trong nửa sắp xếp không?" → quyết định bỏ nửa nào.

---

#### Bài 4: Find Minimum in Rotated Sorted Array (LeetCode 153)

**Đề:** Cho `int[] nums` đã sắp xếp tăng dần rồi bị xoay tại vị trí không biết. Tìm phần tử **nhỏ nhất**.

**Ví dụ:**
```
Input:  nums = [3, 4, 5, 1, 2]
Output: 1

Input:  nums = [4, 5, 6, 7, 0, 1, 2]
Output: 0

Input:  nums = [11, 13, 15, 17]
Output: 11
(Edge case: không bị xoay)

Input:  nums = [2, 1]
Output: 1
(Edge case: xoay 1 vị trí)
```

**Constraint:** `1 <= nums.length <= 5000`, `-5000 <= nums[i] <= 5000`, phần tử phân biệt.

```csharp
// Brute — O(n)
public int FindMinBrute(int[] nums)
    => nums.Min();

// Optimal — O(log n)
public int FindMin(int[] nums)
{
    int lo = 0, hi = nums.Length - 1;

    while (lo < hi)
    {
        int mid = lo + (hi - lo) / 2;

        if (nums[mid] > nums[hi])
            // mid nằm trong phần "cao" (trước điểm xoay) → min ở phải
            lo = mid + 1;
        else
            // mid nằm trong phần "thấp" (sau điểm xoay) → min ở đây hoặc trái
            hi = mid;
    }
    return nums[lo];
}
// Time: O(log n), Space: O(1)
```

**So sánh với bài 33:** Bài 33 so sánh `nums[lo]` với `nums[mid]` để tìm nửa sắp xếp; bài này so sánh `nums[mid]` với `nums[hi]` để tìm phía có phần tử nhỏ nhất.

---

#### Bài 5: Koko Eating Bananas (LeetCode 875) ⭐ Search the Answer điển hình

**Đề:** Có `int[] piles` (n đống chuối). Koko ăn `k` quả mỗi giờ. Mỗi giờ chỉ ăn từ một đống (ăn không hết thì bỏ đó, sang giờ sau mới ăn tiếp). Cần ăn hết trong tối đa `h` giờ. Tìm tốc độ k **nhỏ nhất** để đủ thời gian.

**Ví dụ:**
```
Input:  piles = [3, 6, 7, 11], h = 8
Output: 4
Vì: k=4 → giờ cần: ceil(3/4)+ceil(6/4)+ceil(7/4)+ceil(11/4) = 1+2+2+3 = 8 ≤ 8 ✓
    k=3 → 1+2+3+4 = 10 > 8 ✗

Input:  piles = [30, 11, 23, 4, 20], h = 5
Output: 30
Vì: h = n = 5, mỗi giờ ăn một đống → phải ăn được đống lớn nhất = 30.

Input:  piles = [1000000000], h = 2
Output: 500000000
(Edge case: một đống rất lớn)

Input:  piles = [1, 1, 1, 1], h = 4
Output: 1
(Edge case: h = n, mỗi đống chỉ 1 quả)
```

**Constraint:** `1 <= piles.length <= 10^4`, `piles.length <= h <= 10^9`, `1 <= piles[i] <= 10^9`.

**Phân tích 5 câu hỏi:**
1. Input: n ≤ 10⁴ đống, giá trị lên tới 10⁹ → không thể thử từng k từ 1
2. Output: k nhỏ nhất (int) — đáp án nằm trong [1, max(piles)]
3. Brute: thử từng k từ 1 đến max(piles), mỗi lần tính tổng giờ O(n) → O(n·max)
4. Lãng phí: duyệt từng k — nhưng Feasible(k) là hàm **đơn điệu** (k lớn hơn → dễ hơn) → binary search trên k
5. Pattern: **Binary Search the Answer** — tìm k nhỏ nhất sao cho Feasible(k) = true

```csharp
// Brute — O(max * n), TLE với n,piles[i] = 10^9
public int MinEatingSpeedBrute(int[] piles, int h)
{
    for (int k = 1; ; k++)
        if (CanFinish(piles, k, h)) return k;
}

// Kiểm tra: với tốc độ k, có ăn hết trong h giờ không?
private bool CanFinish(int[] piles, int k, int h)
{
    long hours = 0;
    foreach (int pile in piles)
        hours += (pile + k - 1) / k; // ceil(pile / k)
    return hours <= h;
}

// Optimal — Binary Search the Answer — O(n * log(max))
public int MinEatingSpeed(int[] piles, int h)
{
    int lo = 1;
    int hi = piles.Max(); // k tối đa cần là đống lớn nhất

    while (lo < hi)
    {
        int mid = lo + (hi - lo) / 2;
        if (CanFinish(piles, mid, h))
            hi = mid;     // mid thỏa → thử nhỏ hơn
        else
            lo = mid + 1; // mid không thỏa → cần nhanh hơn
    }
    return lo;
}
// Time: O(n * log(max(piles))), Space: O(1)
```

**Lập luận đơn điệu:** Nếu k=x đủ ăn trong h giờ, thì k=x+1 chắc chắn cũng đủ (ăn nhanh hơn). → Feasible tạo thành chuỗi F...F T T T T... → tìm T đầu tiên = lower bound.

---

#### Bài 6: Find First and Last Position of Element in Sorted Array (LeetCode 34)

**Đề:** Cho `int[] nums` đã sắp xếp tăng (có thể trùng) và `int target`. Tìm vị trí đầu tiên và cuối cùng của target. Trả về `[-1, -1]` nếu không tìm thấy. Yêu cầu O(log n).

**Ví dụ:**
```
Input:  nums = [5, 7, 7, 8, 8, 10], target = 8
Output: [3, 4]

Input:  nums = [5, 7, 7, 8, 8, 10], target = 6
Output: [-1, -1]

Input:  nums = [], target = 0
Output: [-1, -1]
(Edge case: mảng rỗng)

Input:  nums = [1], target = 1
Output: [0, 0]
(Edge case: mảng 1 phần tử, tìm thấy)
```

**Constraint:** `0 <= nums.length <= 10^5`, `-10^9 <= nums[i], target <= 10^9`, mảng tăng không giảm.

```csharp
// Optimal — hai lần binary search O(log n)
public int[] SearchRange(int[] nums, int target)
{
    int first = LowerBound(nums, target);

    // Kiểm tra target có thực sự tồn tại không
    if (first == nums.Length || nums[first] != target)
        return [-1, -1];

    int last = UpperBound(nums, target) - 1;
    return [first, last];
}

// Vị trí đầu tiên >= target
private int LowerBound(int[] nums, int target)
{
    int lo = 0, hi = nums.Length;
    while (lo < hi)
    {
        int mid = lo + (hi - lo) / 2;
        if (nums[mid] < target) lo = mid + 1;
        else hi = mid;
    }
    return lo;
}

// Vị trí đầu tiên > target
private int UpperBound(int[] nums, int target)
{
    int lo = 0, hi = nums.Length;
    while (lo < hi)
    {
        int mid = lo + (hi - lo) / 2;
        if (nums[mid] <= target) lo = mid + 1;
        else hi = mid;
    }
    return lo;
}
// Time: O(log n), Space: O(1)
```

---

### Hard

---

#### Bài 7: Split Array Largest Sum (LeetCode 410)

**Đề:** Cho `int[] nums` và `int k`. Chia mảng thành đúng k phần liên tiếp không rỗng. Tối thiểu hóa **tổng lớn nhất** trong số các phần.

**Ví dụ:**
```
Input:  nums = [7, 2, 5, 10, 8], k = 2
Output: 18
Vì: chia [7,2,5] và [10,8] → max(14,18)=18; hoặc [7,2,5,10] và [8] → max(24,8)=24.
    18 là tốt nhất.

Input:  nums = [1, 2, 3, 4, 5], k = 2
Output: 9
Vì: chia [1,2,3,4] và [5] → max(10,5)=10; [1,2,3] và [4,5] → max(6,9)=9 ✓

Input:  nums = [1, 4, 4], k = 3
Output: 4
(Edge case: mỗi phần tử là một phần riêng)

Input:  nums = [1, 2, 3, 4, 5], k = 5
Output: 5
(Edge case: k = n, mỗi phần một phần tử → max = max(nums))
```

**Constraint:** `1 <= nums.length <= 1000`, `0 <= nums[i] <= 10^6`, `1 <= k <= min(50, nums.length)`.

**Phân tích 5 câu hỏi:**
1. Input: n ≤ 1000, k ≤ 50
2. Output: tổng lớn nhất tối thiểu (int)
3. Brute: DP O(n²·k) hoặc backtracking → chậm với n lớn
4. Lãng phí: không tận dụng tính đơn điệu của đáp án
5. Pattern: **Binary Search the Answer** — đáp án nằm trong [max(nums), sum(nums)]; Feasible(x) = "có thể chia thành ≤ k phần mỗi phần tổng ≤ x?"

```csharp
// Feasible: với giới hạn tổng mỗi phần là 'limit', có dùng ≤ k phần không?
private bool CanSplit(int[] nums, int k, long limit)
{
    int parts = 1;
    long currentSum = 0;
    foreach (int num in nums)
    {
        if (currentSum + num > limit)
        {
            parts++;           // bắt đầu phần mới
            currentSum = num;
            if (parts > k) return false;
        }
        else
            currentSum += num;
    }
    return true;
}

// Optimal — Binary Search the Answer
public int SplitArray(int[] nums, int k)
{
    long lo = nums.Max();        // tối thiểu: phần tử lớn nhất
    long hi = 0;
    foreach (int n in nums) hi += n; // tối đa: toàn bộ mảng = 1 phần

    while (lo < hi)
    {
        long mid = lo + (hi - lo) / 2;
        if (CanSplit(nums, k, mid))
            hi = mid;     // mid thỏa → thử nhỏ hơn
        else
            lo = mid + 1;
    }
    return (int)lo;
}
// Time: O(n * log(sum)), Space: O(1)
```

**Liên hệ với Koko:** Cùng một khuôn mẫu — binary search trên đáp án, viết Feasible bằng greedy scan. Koko tìm tốc độ ăn, bài này tìm ngưỡng tổng mỗi phần.

---

#### Bài 8: Median of Two Sorted Arrays (LeetCode 4)

**Đề:** Cho hai mảng đã sắp xếp `int[] nums1` và `int[] nums2`. Tìm median của mảng hợp nhất. Yêu cầu O(log(m+n)).

**Ví dụ:**
```
Input:  nums1 = [1, 3], nums2 = [2]
Output: 2.0
Vì: hợp nhất = [1, 2, 3], median = 2.

Input:  nums1 = [1, 2], nums2 = [3, 4]
Output: 2.5
Vì: hợp nhất = [1, 2, 3, 4], median = (2+3)/2 = 2.5.

Input:  nums1 = [], nums2 = [1]
Output: 1.0
(Edge case: mảng rỗng)

Input:  nums1 = [2], nums2 = []
Output: 2.0
(Edge case: một mảng rỗng)
```

**Constraint:** `m, n = 0..1000`, `-10^6 <= nums1[i], nums2[i] <= 10^6`.

```csharp
// Brute — O((m+n) log(m+n)): trộn rồi tìm median
public double FindMedianSortedArraysBrute(int[] nums1, int[] nums2)
{
    int[] merged = nums1.Concat(nums2).OrderBy(x => x).ToArray();
    int n = merged.Length;
    return n % 2 == 0
        ? (merged[n/2 - 1] + merged[n/2]) / 2.0
        : merged[n/2];
}

// Optimal — Binary Search trên phân hoạch, O(log(min(m,n)))
public double FindMedianSortedArrays(int[] nums1, int[] nums2)
{
    // Đảm bảo nums1 là mảng ngắn hơn
    if (nums1.Length > nums2.Length)
        return FindMedianSortedArrays(nums2, nums1);

    int m = nums1.Length, n = nums2.Length;
    int lo = 0, hi = m;
    int half = (m + n + 1) / 2; // số phần tử ở nửa trái

    while (lo <= hi)
    {
        int i = lo + (hi - lo) / 2; // số phần tử lấy từ nums1 cho nửa trái
        int j = half - i;            // số phần tử lấy từ nums2 cho nửa trái

        int maxLeft1  = i == 0 ? int.MinValue : nums1[i - 1];
        int minRight1 = i == m ? int.MaxValue : nums1[i];
        int maxLeft2  = j == 0 ? int.MinValue : nums2[j - 1];
        int minRight2 = j == n ? int.MaxValue : nums2[j];

        if (maxLeft1 <= minRight2 && maxLeft2 <= minRight1)
        {
            // Tìm được phân hoạch đúng
            if ((m + n) % 2 == 1)
                return Math.Max(maxLeft1, maxLeft2);
            else
                return (Math.Max(maxLeft1, maxLeft2) + Math.Min(minRight1, minRight2)) / 2.0;
        }
        else if (maxLeft1 > minRight2)
            hi = i - 1; // i quá lớn → lấy ít hơn từ nums1
        else
            lo = i + 1; // i quá nhỏ → lấy nhiều hơn từ nums1
    }
    throw new InvalidOperationException();
}
// Time: O(log(min(m,n))), Space: O(1)
```

---

## Tổng kết Pattern

| Dấu hiệu đề | Template |
|-------------|----------|
| Mảng sắp xếp, tìm chính xác | Binary search chuẩn (`lo <= hi`, trả về mid hoặc -1) |
| Tìm vị trí đầu tiên thỏa điều kiện | Lower bound (`lo < hi`, `hi = mid`) |
| Tìm vị trí cuối cùng thỏa điều kiện | Upper bound − 1 |
| Mảng xoay, tìm phần tử | So sánh với `nums[lo]` để xác định nửa sắp xếp |
| "Nhỏ nhất/lớn nhất sao cho có thể..." | Search the answer: xác định [lo,hi], viết Feasible |
| Hai mảng sắp xếp, tìm median | Binary search trên phân hoạch |

**Quy tắc biên:**
- `while (lo <= hi)`: khi cần tìm một phần tử cụ thể (trả về -1 nếu không có).
- `while (lo < hi)`: khi tìm biên (lower/upper bound, search the answer) — luôn có đáp án.
- Tính mid khi tìm min: `mid = lo + (hi - lo) / 2` (về phía trái).
- Tính mid khi tìm max: `mid = lo + (hi - lo + 1) / 2` (về phía phải, tránh deadlock khi `lo+1==hi`).
