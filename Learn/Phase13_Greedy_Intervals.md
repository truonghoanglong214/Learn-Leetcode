# Phase 13 — Greedy & Intervals

> **Ngôn ngữ:** C#
> **Mục tiêu:** Nhận ra khi nào **chọn cục bộ tối ưu** dẫn tới tối ưu toàn cục, và xử lý thành thạo họ bài **khoảng (intervals)**: gộp, chèn, đếm xung đột, lập lịch.

---

## Bài học 1 — Tư duy Greedy

### 1.1 Greedy là gì?

Greedy = "tham lam": **tại mỗi bước chọn phương án tốt nhất ngay lúc đó**, mà không quay lui xem xét lại.

Greedy đúng khi thoả **2 điều kiện**:
1. **Greedy choice property** — lựa chọn tham tại bước hiện tại không bao giờ làm hỏng đáp án tối ưu toàn cục.
2. **Optimal substructure** — bài toán con sau khi chọn cũng có cấu trúc tối ưu.

**Cách kiểm chứng:** thử tìm **phản ví dụ** (counter-example). Nếu không tìm được phản ví dụ nào → khả năng cao greedy đúng.

### 1.2 Phân biệt Greedy vs DP

| | Greedy | DP |
|---|---|---|
| Quyết định | Tại chỗ, không quay lại | Xét tất cả lựa chọn, ghi nhớ kết quả |
| Khi đúng | Có exchange argument / greedy stays ahead | Bài toán con chồng lặp + optimal substructure |
| Tốc độ | Thường O(n log n) | Thường O(n²) hoặc hơn |
| Nguy hiểm | Có thể sai nếu không kiểm chứng | Luôn đúng nếu state đúng |

> **Quy tắc:** Khi thấy bài trông "greedy", **luôn thử tìm phản ví dụ trước** rồi mới code.

### 1.3 Pattern tổng quát: Sort + Quét Tham

```csharp
// Bước 1: Sắp xếp theo tiêu chí phù hợp (bắt đầu / kết thúc / giá trị)
// Bước 2: Duyệt từ trái sang phải, tại mỗi bước chọn tham
// Bước 3: Cập nhật trạng thái và đáp án
```

---

## Bài học 2 — Interval Patterns

### 2.1 Ba kiểu bài khoảng

**Kiểu 1: Merge Intervals** — gộp các khoảng chồng lấn
- Sort theo **điểm bắt đầu**, duyệt và gộp khi chồng lấn.

**Kiểu 2: Interval Scheduling** — chọn nhiều nhất khoảng không chồng lấn
- Sort theo **điểm kết thúc**, tham lam chọn khoảng kết thúc sớm nhất.

**Kiểu 3: Meeting Rooms / Count Overlaps** — đếm số chồng lấn tối đa
- Dùng sweep line hoặc heap.

### 2.2 Template Merge Intervals

```csharp
int[][] MergeIntervals(int[][] intervals)
{
    Array.Sort(intervals, (a, b) => a[0].CompareTo(b[0])); // sort theo start

    var result = new List<int[]>();
    result.Add(intervals[0]);

    for (int i = 1; i < intervals.Length; i++)
    {
        int[] last = result[^1];
        if (intervals[i][0] <= last[1]) // chồng lấn (kể cả tiếp xúc)
            last[1] = Math.Max(last[1], intervals[i][1]); // mở rộng end
        else
            result.Add(intervals[i]); // không chồng → thêm mới
    }
    return result.ToArray();
    // T: O(n log n), S: O(n)
}
```

### 2.3 Template Interval Scheduling (không chồng lấn tối đa)

```csharp
int MaxNonOverlapping(int[][] intervals)
{
    Array.Sort(intervals, (a, b) => a[1].CompareTo(b[1])); // sort theo END

    int count = 0, lastEnd = int.MinValue;
    foreach (var iv in intervals)
    {
        if (iv[0] >= lastEnd) // không chồng với khoảng đã chọn
        {
            count++;
            lastEnd = iv[1];
        }
        // else: khoảng này chồng lấn → bỏ (khoảng kết thúc sau luôn kém hơn)
    }
    return count;
    // T: O(n log n)
}
```

> **Tại sao sort theo END?** Nếu có hai khoảng bắt đầu trùng nhau, khoảng kết thúc **sớm hơn** để lại nhiều "chỗ" hơn cho các khoảng sau — đây là exchange argument.

---

## Vòng lặp tư duy 5 câu hỏi cho Greedy & Intervals

1. **Input nói gì?** Mảng đã sắp xếp chưa? Dữ liệu là số đơn hay khoảng [start, end]?
2. **Output cần gì?** Đếm tối thiểu/tối đa? Danh sách khoảng sau gộp? Có thể đến đích không?
3. **Brute Force là gì?** Thử mọi tập con? Thử mọi thứ tự? O(2ⁿ) hoặc O(n²)?
4. **Có thể chọn tham không?** Tìm exchange argument: "nếu tôi đổi lựa chọn tham sang lựa chọn khác, kết quả có tốt hơn không?" → Nếu không → greedy đúng.
5. **Sort theo tiêu chí nào?** Đây là quyết định quan trọng nhất của greedy interval.

---

## Bài tập

### Bài 1 — Maximum Subarray (53) ⭐⭐ — Bản lề (Kadane)

**Đề:** Cho mảng số nguyên `nums`. Tìm **subarray liên tiếp** có **tổng lớn nhất**. Trả về tổng đó.

**Ví dụ:**
```
Input:  nums = [-2,1,-3,4,-1,2,1,-5,4]
Output: 6
Vì: [4,-1,2,1] có tổng = 6 là lớn nhất.

Input:  nums = [1]
Output: 1
Vì: edge case — một phần tử.

Input:  nums = [-1,-2,-3]
Output: -1
Vì: edge case — tất cả âm, phải chọn phần tử lớn nhất.
```

**Constraint:** `1 ≤ n ≤ 10⁵`, `-10⁴ ≤ nums[i] ≤ 10⁴`.

**Phân tích 5 câu hỏi:**
1. Mảng có số âm → sliding window không dùng được.
2. Một số — tổng subarray lớn nhất.
3. Brute force: mọi cặp (i,j) tính tổng → O(n²).
4. Chỗ lãng phí: nếu tổng hiện tại âm, thêm nó vào chỉ kéo xuống → **reset về 0** (bắt đầu subarray mới).
5. Pattern: **Greedy (Kadane's Algorithm)** — tại mỗi vị trí, quyết định "tiếp tục subarray cũ hay bắt đầu mới".

```csharp
// Kadane's Algorithm — O(n) time, O(1) space
public int MaxSubArray(int[] nums)
{
    int current = nums[0], best = nums[0];
    for (int i = 1; i < nums.Length; i++)
    {
        // Chọn: tiếp tục (current + nums[i]) hoặc bắt đầu lại (nums[i])
        current = Math.Max(nums[i], current + nums[i]);
        best = Math.Max(best, current);
    }
    return best;
}

// Tương đương với:
// current = (current < 0) ? nums[i] : current + nums[i];
// Nếu current âm → nó chỉ kéo tổng xuống → reset về nums[i]
```

> **Tại sao greedy đúng?** Exchange argument: giả sử đáp án tối ưu có subarray bắt đầu tại `i`. Nếu tổng các phần tử trước `i` (cùng subarray) âm → bỏ chúng đi tổng chỉ tăng → mâu thuẫn với giả sử → không thể bắt đầu từ vị trí âm.

---

### Bài 2 — Jump Game (55) ⭐⭐

**Đề:** Mảng `nums[i]` = số bước tối đa có thể nhảy từ vị trí `i`. Xuất phát từ index 0, hỏi có thể đến index cuối không?

**Ví dụ:**
```
Input:  nums = [2,3,1,1,4]
Output: true
Vì: 0→1→4 hoặc 0→2→3→4.

Input:  nums = [3,2,1,0,4]
Output: false
Vì: mọi đường đều kẹt tại index 3 (nums[3]=0).

Input:  nums = [0]
Output: true
Vì: edge case — đã đứng tại đích.
```

**Constraint:** `1 ≤ n ≤ 10⁴`, `0 ≤ nums[i] ≤ 10⁵`.

**Phân tích 5 câu hỏi:**
1. Mảng số không âm.
2. Boolean — đến được cuối không.
3. Brute force: BFS/DFS từ 0 thử mọi bước nhảy → O(n²).
4. Chỗ lãng phí: không cần thử từng đường, chỉ cần theo dõi **tầm với xa nhất** (maxReach).
5. Pattern: **Greedy — track maxReach**.

```csharp
public bool CanJump(int[] nums)
{
    int maxReach = 0;
    for (int i = 0; i < nums.Length; i++)
    {
        if (i > maxReach) return false; // không đến được vị trí i
        maxReach = Math.Max(maxReach, i + nums[i]);
    }
    return true;
    // T: O(n), S: O(1)
}
```

---

### Bài 3 — Merge Intervals (56) ⭐⭐ — Bản lề

**Đề:** Cho mảng `intervals[i] = [start, end]`. Gộp tất cả các khoảng chồng lấn, trả về mảng các khoảng rời nhau.

**Ví dụ:**
```
Input:  intervals = [[1,3],[2,6],[8,10],[15,18]]
Output: [[1,6],[8,10],[15,18]]
Vì: [1,3] và [2,6] chồng → gộp thành [1,6].

Input:  intervals = [[1,4],[4,5]]
Output: [[1,5]]
Vì: tiếp xúc tại 4 → cũng gộp (≤ không phải <).

Input:  intervals = [[1,4]]
Output: [[1,4]]
Vì: edge case — chỉ một khoảng.
```

**Constraint:** `1 ≤ intervals.length ≤ 10⁴`, `0 ≤ start ≤ end ≤ 10⁴`.

**Phân tích 5 câu hỏi:**
1. Mảng khoảng [start, end], chưa sắp xếp.
2. Mảng khoảng sau gộp.
3. Brute force: so từng cặp → O(n²).
4. Chỗ lãng phí: nếu sort theo start, chỉ cần so khoảng hiện tại với khoảng cuối cùng đã gộp.
5. Pattern: **Sort theo start + Merge**.

```csharp
public int[][] Merge(int[][] intervals)
{
    Array.Sort(intervals, (a, b) => a[0].CompareTo(b[0]));

    var result = new List<int[]> { intervals[0] };
    for (int i = 1; i < intervals.Length; i++)
    {
        int[] last = result[^1];
        if (intervals[i][0] <= last[1])          // chồng hoặc tiếp xúc
            last[1] = Math.Max(last[1], intervals[i][1]);
        else
            result.Add(intervals[i]);
    }
    return result.ToArray();
    // T: O(n log n), S: O(n)
}
```

---

### Bài 4 — Insert Interval (57) ⭐⭐

**Đề:** Cho mảng `intervals` **đã sắp xếp và không chồng**. Chèn `newInterval` vào và trả về mảng sau khi gộp.

**Ví dụ:**
```
Input:  intervals = [[1,3],[6,9]], newInterval = [2,5]
Output: [[1,5],[6,9]]
Vì: [2,5] chồng với [1,3] → gộp thành [1,5].

Input:  intervals = [[1,2],[3,5],[6,7],[8,10],[12,16]], newInterval = [4,8]
Output: [[1,2],[3,10],[12,16]]
Vì: [4,8] chồng với [3,5],[6,7],[8,10] → gộp thành [3,10].

Input:  intervals = [], newInterval = [5,7]
Output: [[5,7]]
Vì: edge case — mảng rỗng.
```

**Constraint:** `0 ≤ intervals.length ≤ 10⁴`, đã sắp xếp, newInterval hợp lệ.

```csharp
public int[][] Insert(int[][] intervals, int[] newInterval)
{
    var result = new List<int[]>();
    int i = 0, n = intervals.Length;

    // Giai đoạn 1: các khoảng kết thúc TRƯỚC newInterval — thêm thẳng
    while (i < n && intervals[i][1] < newInterval[0])
        result.Add(intervals[i++]);

    // Giai đoạn 2: các khoảng CHỒNG với newInterval — gộp vào newInterval
    while (i < n && intervals[i][0] <= newInterval[1])
    {
        newInterval[0] = Math.Min(newInterval[0], intervals[i][0]);
        newInterval[1] = Math.Max(newInterval[1], intervals[i][1]);
        i++;
    }
    result.Add(newInterval); // thêm khoảng đã gộp

    // Giai đoạn 3: các khoảng bắt đầu SAU newInterval — thêm thẳng
    while (i < n) result.Add(intervals[i++]);

    return result.ToArray();
    // T: O(n), S: O(n) — không cần sort vì đã sắp xếp sẵn
}
```

> **Điểm mấu chốt:** Ba giai đoạn rõ ràng — trước / giao / sau. Nhớ `<` ở giai đoạn 1 và `<=` ở giai đoạn 2 (để xử lý tiếp xúc đúng).

---

### Bài 5 — Non-overlapping Intervals (435) ⭐⭐

**Đề:** Tìm số khoảng **ít nhất** cần xóa để các khoảng còn lại không chồng lấn nhau.

**Ví dụ:**
```
Input:  intervals = [[1,2],[2,3],[3,4],[1,3]]
Output: 1
Vì: xóa [1,3] → còn [[1,2],[2,3],[3,4]] không chồng (tiếp xúc ổn).

Input:  intervals = [[1,2],[1,2],[1,2]]
Output: 2
Vì: giữ 1, xóa 2.

Input:  intervals = [[1,2],[2,3]]
Output: 0
Vì: edge case — tiếp xúc không phải chồng lấn.
```

**Constraint:** `1 ≤ intervals.length ≤ 10⁵`.

**Phân tích 5 câu hỏi:**
1. Mảng khoảng, không sắp xếp.
2. Số nguyên — số khoảng cần xóa ít nhất.
3. Brute force: thử mọi tập con giữ lại → O(2ⁿ).
4. Đảo bài: **xóa ít nhất** = **giữ nhiều nhất không chồng**. Bài toán giữ nhiều nhất là interval scheduling → sort theo end.
5. Pattern: **Interval Scheduling** (sort theo end + greedy).

```csharp
public int EraseOverlapIntervals(int[][] intervals)
{
    Array.Sort(intervals, (a, b) => a[1].CompareTo(b[1])); // sort theo END

    int kept = 0, lastEnd = int.MinValue;
    foreach (var iv in intervals)
    {
        if (iv[0] >= lastEnd) // không chồng → giữ lại
        {
            kept++;
            lastEnd = iv[1];
        }
        // chồng lấn → bỏ (greedy: khoảng kết thúc sớm hơn đã được giữ)
    }
    return intervals.Length - kept; // số phải xóa = tổng - số giữ
    // T: O(n log n)
}
```

---

### Bài 6 — Gas Station (134) ⭐⭐

**Đề:** `n` trạm xăng theo vòng tròn. Tại trạm `i`: nhận được `gas[i]` lít, đi đến trạm tiếp tốn `cost[i]` lít. Bắt đầu từ trạm nào để đi một vòng hoàn chỉnh? Trả về index đó, hoặc -1 nếu không thể.

**Ví dụ:**
```
Input:  gas = [1,2,3,4,5], cost = [3,4,5,1,2]
Output: 3
Vì: bắt đầu từ trạm 3: 4-1=3 → 3+5-2=6 → 6+1-3=4 → 4+2-4=2 → 2+3-5=0. Hoàn thành!

Input:  gas = [2,3,4], cost = [3,4,3]
Output: -1
Vì: tổng gas=9 < tổng cost=10 → không thể.

Input:  gas = [5], cost = [4]
Output: 0
Vì: edge case — một trạm, gas > cost.
```

**Constraint:** `n == gas.length == cost.length`, `1 ≤ n ≤ 10⁵`, `0 ≤ gas[i], cost[i] ≤ 10⁴`.

**Phân tích 5 câu hỏi:**
1. Hai mảng gas và cost, vòng tròn.
2. Index bắt đầu hoặc -1.
3. Brute force: thử từng điểm xuất phát → O(n²).
4. Chỗ lãng phí: nếu tổng(gas) < tổng(cost) → chắc chắn -1. Nếu tổng bị âm tại vị trí nào → điểm xuất phát hiện tại không ổn, thử tiếp.
5. Pattern: **Greedy một vòng** — track `tank` và `total`.

```csharp
public int CanCompleteCircuit(int[] gas, int[] cost)
{
    int total = 0, tank = 0, start = 0;
    for (int i = 0; i < gas.Length; i++)
    {
        int gain = gas[i] - cost[i];
        total += gain;
        tank  += gain;
        if (tank < 0) // không thể đến từ start → thử bắt đầu từ i+1
        {
            start = i + 1;
            tank  = 0;
        }
    }
    return total >= 0 ? start : -1;
    // T: O(n), S: O(1)
    // Tại sao đúng: nếu total >= 0, luôn tồn tại đúng 1 điểm xuất phát hợp lệ
    // và điểm start được greedy tìm chính là điểm đó.
}
```

---

### Bài 7 — Meeting Rooms II (253) ⭐⭐⭐

**Đề:** Cho `intervals[i] = [start, end]`. Tìm số **phòng họp ít nhất** cần để không cuộc họp nào bị chồng lấn.

**Ví dụ:**
```
Input:  intervals = [[0,30],[5,10],[15,20]]
Output: 2
Vì: [0,30] chồng cả [5,10] và [15,20]. Nhưng [5,10] và [15,20] không chồng nhau.
     Phòng 1: [0,30]. Phòng 2: [5,10] rồi [15,20].

Input:  intervals = [[7,10],[2,4]]
Output: 1
Vì: không chồng nhau, 1 phòng là đủ.

Input:  intervals = [[1,5],[1,5],[1,5]]
Output: 3
Vì: edge case — tất cả trùng nhau hoàn toàn.
```

**Constraint:** `1 ≤ intervals.length ≤ 10⁴`.

```csharp
// Cách 1: Min-heap (track thời gian kết thúc sớm nhất)
public int MinMeetingRooms(int[][] intervals)
{
    Array.Sort(intervals, (a, b) => a[0].CompareTo(b[0])); // sort theo start

    // Min-heap chứa thời gian kết thúc của các phòng đang dùng
    var pq = new PriorityQueue<int, int>();
    foreach (var iv in intervals)
    {
        // Nếu phòng kết thúc sớm nhất đã xong → tái sử dụng phòng đó
        if (pq.Count > 0 && pq.Peek() <= iv[0])
            pq.Dequeue();
        pq.Enqueue(iv[1], iv[1]); // phòng này dùng đến iv[1]
    }
    return pq.Count; // số phòng đang dùng = số phòng cần
    // T: O(n log n)
}

// Cách 2: Sweep line — đếm chồng lấn tối đa
public int MinMeetingRoomsSweep(int[][] intervals)
{
    int n = intervals.Length;
    int[] starts = new int[n], ends = new int[n];
    for (int i = 0; i < n; i++) { starts[i] = intervals[i][0]; ends[i] = intervals[i][1]; }
    Array.Sort(starts); Array.Sort(ends);

    int rooms = 0, maxRooms = 0, j = 0;
    for (int i = 0; i < n; i++)
    {
        if (starts[i] < ends[j]) rooms++;   // họp mới bắt đầu trước họp cũ kết thúc
        else { rooms--; j++; }              // tái dụng phòng (một họp vừa kết thúc)
        // LƯU Ý: dùng < (không <=) vì end == start là ổn (kết thúc rồi mới bắt đầu)
        maxRooms = Math.Max(maxRooms, rooms);
    }
    return maxRooms;
    // T: O(n log n)
}
```

---

### Bài 8 — Candy (135) ⭐⭐⭐

**Đề:** `n` trẻ em đứng thành hàng, mỗi em có điểm `ratings[i]`. Phân kẹo theo quy tắc: mỗi em ít nhất 1 cái; em nào điểm cao hơn láng giềng thì nhận nhiều hơn. Tìm số kẹo **ít nhất**.

**Ví dụ:**
```
Input:  ratings = [1,0,2]
Output: 5
Vì: [2,1,2] — em giữa ít hơn, hai bên cao hơn.

Input:  ratings = [1,2,2]
Output: 4
Vì: [1,2,1] — em cuối điểm bằng em giữa, không cần hơn.

Input:  ratings = [1]
Output: 1
Vì: edge case — một em.
```

**Constraint:** `1 ≤ n ≤ 2·10⁴`, `0 ≤ ratings[i] ≤ 2·10⁴`.

```csharp
// Greedy hai lượt — O(n) time, O(n) space
public int Candy(int[] ratings)
{
    int n = ratings.Length;
    int[] candy = new int[n];
    Array.Fill(candy, 1); // mỗi em ít nhất 1

    // Lượt 1 — trái sang phải: nếu ratings[i] > ratings[i-1] → candy[i] > candy[i-1]
    for (int i = 1; i < n; i++)
        if (ratings[i] > ratings[i - 1])
            candy[i] = candy[i - 1] + 1;

    // Lượt 2 — phải sang trái: nếu ratings[i] > ratings[i+1] → candy[i] ≥ candy[i+1]+1
    for (int i = n - 2; i >= 0; i--)
        if (ratings[i] > ratings[i + 1])
            candy[i] = Math.Max(candy[i], candy[i + 1] + 1);

    return candy.Sum();
    // T: O(n), S: O(n)
}
```

> **Tại sao hai lượt?** Ràng buộc tồn tại từ **cả hai phía**. Lượt trái→phải thỏa ràng buộc "cao hơn bên trái", lượt phải→trái thỏa "cao hơn bên phải". `Math.Max` đảm bảo cả hai đồng thời.

---

## Tổng kết & Dấu hiệu nhận biết

| Bài toán | Pattern | Tiêu chí sort |
|---|---|---|
| Subarray có tổng lớn nhất | Kadane | Không cần sort |
| Đến được cuối mảng không | Greedy reach | Không cần sort |
| Gộp khoảng chồng lấn | Merge intervals | Sort theo **start** |
| Chèn khoảng mới | Insert interval | Đã sắp xếp, O(n) |
| Xóa ít nhất để không chồng | Interval scheduling | Sort theo **end** |
| Đi vòng xăng | Greedy một vòng | Không cần sort |
| Số phòng họp ít nhất | Min-heap / Sweep line | Sort theo **start** |
| Phân kẹo | Greedy hai lượt | Không cần sort |

## Lỗi tư duy thường gặp

- Tin greedy mà không kiểm chứng → sai (nhiều bài trông greedy nhưng phải DP, ví dụ: coin change với mệnh giá không chuẩn).
- Sort sai tiêu chí: merge → sort theo **start**; scheduling → sort theo **end**.
- Quên `Math.Max` khi gộp: nếu khoảng lồng nhau hoàn toàn (`[1,10]` và `[2,5]`), phải giữ end lớn hơn.
- Nhầm `<` với `<=` khi kiểm tra chồng lấn: `[1,2]` và `[2,3]` — tiếp xúc tại 2, tùy bài mà chồng hay không.
- Kadane: quên rằng khi tất cả âm, kết quả là phần tử lớn nhất (không phải 0).

## Tiêu chí hoàn thành

- [ ] Giải Kadane và giải thích vì sao reset khi tổng âm bằng exchange argument.
- [ ] Master merge/insert/scheduling intervals — biết sort theo **start** hay **end** cho từng loại.
- [ ] Biết *kiểm chứng* một ý tưởng greedy bằng phản ví dụ trước khi code.
- [ ] Phân biệt bài greedy vs DP: coin change (mệnh giá tùy ý → DP, không phải greedy).
