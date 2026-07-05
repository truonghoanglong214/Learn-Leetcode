# Phase 10 — Heap / Priority Queue

> **Ngôn ngữ:** C#
> **Mục tiêu:** Giải các bài **Top-K**, **trộn nhiều nguồn sắp xếp**, **lập lịch theo ưu tiên**, và bài cần "luôn lấy phần tử nhỏ/lớn nhất hiện tại" hiệu quả.

---

## Bài học 1 — Nền Tảng

### 1.1 Heap là gì?

Heap là cây nhị phân hoàn chỉnh thỏa tính chất **heap**:
- **Min-Heap**: cha ≤ con → gốc luôn là phần tử nhỏ nhất.
- **Max-Heap**: cha ≥ con → gốc luôn là phần tử lớn nhất.

| Thao tác | Độ phức tạp |
|----------|------------|
| Push (thêm) | O(log n) |
| Pop (lấy cực trị) | O(log n) |
| Peek (nhìn đỉnh) | O(1) |
| Heapify (dựng từ mảng) | O(n) |

### 1.2 C# `PriorityQueue<TElement, TPriority>`

C# 6+ có `PriorityQueue` tích hợp sẵn (Min-Heap mặc định):

```csharp
// Min-Heap: phần tử có priority nhỏ hơn ra trước
var minHeap = new PriorityQueue<int, int>();
minHeap.Enqueue(3, 3);   // (element, priority)
minHeap.Enqueue(1, 1);
minHeap.Enqueue(5, 5);

minHeap.Dequeue();        // trả về 1 (priority nhỏ nhất)
minHeap.Peek();           // nhìn đỉnh không xóa
minHeap.Count;            // số phần tử

// Max-Heap: đảo ngược priority (dùng âm)
var maxHeap = new PriorityQueue<int, int>();
maxHeap.Enqueue(3, -3);
maxHeap.Enqueue(1, -1);
maxHeap.Enqueue(5, -5);
maxHeap.Dequeue();        // trả về 5 (priority = -5, nhỏ nhất)
```

> **Trick Max-Heap**: C# không có Max-Heap sẵn — đảo dấu priority: `Enqueue(x, -x)` rồi dùng như Min-Heap.

### 1.3 Khi nào dùng Heap vs Sort?

| Tình huống | Dùng |
|-----------|------|
| Cần top-k từ n phần tử (k << n) | Heap kích thước k |
| Cần toàn bộ sắp xếp, chỉ một lần | Sort |
| Dữ liệu đến dần (streaming), liên tục hỏi cực trị | Heap |
| Trộn k dãy đã sắp xếp | Min-Heap k nguồn |
| Tìm phần tử thứ k, không cần toàn bộ | Quickselect hoặc Heap |

---

## Bài học 2 — Pattern Top-K

**Ý tưởng:** Duy trì một heap kích thước k. Duyệt qua n phần tử, nếu heap chưa đủ k thì thêm vào, nếu phần tử mới "tốt hơn" phần tử tệ nhất trong heap thì đổi.

- **Top-K lớn nhất** → dùng **min-heap** kích thước k (đỉnh là phần tử nhỏ nhất trong k lớn nhất, dễ so sánh loại bỏ).
- **Top-K nhỏ nhất** → dùng **max-heap** kích thước k.

```csharp
// Tìm k phần tử lớn nhất trong mảng
int[] TopKLargest(int[] nums, int k)
{
    // Min-heap kích thước k
    var minHeap = new PriorityQueue<int, int>();
    foreach (int num in nums)
    {
        minHeap.Enqueue(num, num);
        if (minHeap.Count > k)
            minHeap.Dequeue();  // loại phần tử nhỏ nhất
    }
    return minHeap.UnorderedItems.Select(x => x.Element).ToArray();
}
// T: O(n log k)   S: O(k)
```

---

## Bài tập

---

### Bài 1 — Kth Largest Element in a Stream (LC 703) ⭐

**Đề:** Thiết kế class `KthLargest` nhận k và mảng khởi tạo. Mỗi lần gọi `Add(val)` thêm val vào stream và trả về phần tử lớn thứ k hiện tại.

**Ví dụ:**
```
Init: k = 3, nums = [4, 5, 8, 2]
Add(3)  → 4   // stream: [2,3,4,5,8] → lớn thứ 3 là 4
Add(5)  → 5   // stream: [2,3,4,5,5,8] → lớn thứ 3 là 5
Add(10) → 5   // stream: [2,3,4,5,5,8,10] → lớn thứ 3 là 5
Add(9)  → 8   // stream: [2,3,4,5,5,8,9,10] → lớn thứ 3 là 8
Add(4)  → 8   // lớn thứ 3 vẫn là 8

Edge case:
Init: k = 1, nums = []
Add(5) → 5   // k=1, lớn nhất là 5
```

**Constraint:**
- `1 <= k <= 10^4`
- `0 <= nums.length <= 10^4`
- `-10^4 <= nums[i] <= 10^4`
- `-10^4 <= val <= 10^4`
- Gọi `add` tối đa `10^4` lần, và luôn có ít nhất k phần tử trước khi trả về.

**Phân tích 5 câu hỏi** (bài bản lề — pattern Top-K):
1. **Input nói gì?** Dữ liệu đến dần (streaming), không biết trước toàn bộ.
2. **Output cần gì?** Phần tử lớn thứ k sau mỗi lần thêm.
3. **Brute Force là gì?** Mỗi lần add, sort lại toàn bộ rồi trả nums[n-k]. T: O(n log n) mỗi lần add.
4. **Chỗ lãng phí?** Sort lại toàn bộ khi chỉ cần lớn thứ k. Nhận ra: nếu giữ min-heap kích thước k, đỉnh chính là phần tử lớn thứ k.
5. **Pattern?** **Top-K** — min-heap kích thước k, đỉnh = lớn thứ k.

**Code C#:**
```csharp
// Brute Force: mỗi add sort lại — T: O(n log n) mỗi add, S: O(n)
public class KthLargestBrute
{
    private List<int> data;
    private int k;
    public KthLargestBrute(int k, int[] nums) { this.k = k; data = new List<int>(nums); }
    public int Add(int val)
    {
        data.Add(val);
        data.Sort();
        return data[data.Count - k];
    }
}

// Optimal: min-heap kích thước k — T: O(log k) mỗi add, S: O(k)
public class KthLargest
{
    private PriorityQueue<int, int> minHeap;
    private int k;

    public KthLargest(int k, int[] nums)
    {
        this.k = k;
        minHeap = new PriorityQueue<int, int>();
        foreach (int n in nums)
            Add(n);
    }

    public int Add(int val)
    {
        minHeap.Enqueue(val, val);
        // Giữ heap đúng kích thước k
        if (minHeap.Count > k)
            minHeap.Dequeue();
        return minHeap.Peek();  // đỉnh min-heap = lớn thứ k
    }
}
```

---

### Bài 2 — Last Stone Weight (LC 1046) ⭐

**Đề:** Có mảng đá với trọng lượng `stones`. Mỗi lần chọn 2 đá nặng nhất (x ≤ y): nếu x == y thì cả hai vỡ; nếu x < y thì còn lại đá nặng y−x. Trả về trọng lượng đá cuối cùng (0 nếu không còn).

**Ví dụ:**
```
Input:  stones = [2,7,4,1,8,1]
Output: 1
Vì: lấy 8,7 → còn 1; lấy 4,2 → còn 2; lấy 2,1 → còn 1; lấy 1,1 → hết; còn 1

Input:  stones = [1]
Output: 1
Vì: chỉ 1 đá, trả về ngay

Input:  stones = [3,3]
Output: 0
Vì: 3==3 → cả hai vỡ
```

**Constraint:**
- `1 <= stones.length <= 30`
- `1 <= stones[i] <= 1000`

**Code C#:**
```csharp
// Optimal: Max-Heap (đảo dấu) — T: O(n log n), S: O(n)
public int LastStoneWeight(int[] stones)
{
    // Max-heap: đảo dấu priority
    var maxHeap = new PriorityQueue<int, int>();
    foreach (int s in stones)
        maxHeap.Enqueue(s, -s);

    while (maxHeap.Count > 1)
    {
        int y = maxHeap.Dequeue();
        int x = maxHeap.Dequeue();
        if (x != y)
            maxHeap.Enqueue(y - x, -(y - x));
    }

    return maxHeap.Count == 1 ? maxHeap.Dequeue() : 0;
}
```

---

### Bài 3 — Top K Frequent Elements (LC 347) ⭐⭐ (gặp lại từ Phase 1)

**Đề:** Cho mảng `nums`, trả về k phần tử xuất hiện nhiều nhất. Thứ tự kết quả không quan trọng.

**Ví dụ:**
```
Input:  nums = [1,1,1,2,2,3], k = 2
Output: [1,2]
Vì: 1 xuất hiện 3 lần, 2 xuất hiện 2 lần

Input:  nums = [1], k = 1
Output: [1]

Input:  nums = [4,4,4,5,5,6], k = 2
Output: [4,5]
```

**Constraint:**
- `1 <= nums.length <= 10^5`
- `-10^4 <= nums[i] <= 10^4`
- k nằm trong `[1, số phần tử phân biệt]`
- Đáp án là duy nhất.

**Code C#:**
```csharp
// Approach 1: Heap — T: O(n log k), S: O(n)
public int[] TopKFrequent(int[] nums, int k)
{
    // Đếm tần suất
    var freq = new Dictionary<int, int>();
    foreach (int n in nums)
        freq[n] = freq.GetValueOrDefault(n, 0) + 1;

    // Min-heap kích thước k theo tần suất
    var minHeap = new PriorityQueue<int, int>();
    foreach (var (num, count) in freq)
    {
        minHeap.Enqueue(num, count);
        if (minHeap.Count > k)
            minHeap.Dequeue();
    }

    return minHeap.UnorderedItems.Select(x => x.Element).ToArray();
    // T: O(n log k)   S: O(n)
}

// Approach 2: Bucket Sort — T: O(n), S: O(n)
public int[] TopKFrequentBucket(int[] nums, int k)
{
    var freq = new Dictionary<int, int>();
    foreach (int n in nums)
        freq[n] = freq.GetValueOrDefault(n, 0) + 1;

    // bucket[i] = danh sách số xuất hiện đúng i lần
    var bucket = new List<int>[nums.Length + 1];
    foreach (var (num, count) in freq)
    {
        bucket[count] ??= new List<int>();
        bucket[count].Add(num);
    }

    var result = new List<int>();
    for (int i = bucket.Length - 1; i >= 0 && result.Count < k; i--)
        if (bucket[i] != null)
            result.AddRange(bucket[i]);

    return result.Take(k).ToArray();
    // T: O(n)   S: O(n)
}
```

---

### Bài 4 — Kth Largest Element in an Array (LC 215) ⭐⭐

**Đề:** Tìm phần tử lớn thứ k trong mảng (không cần sắp xếp, tính theo thứ tự khi sort giảm dần).

**Ví dụ:**
```
Input:  nums = [3,2,1,5,6,4], k = 2
Output: 5
Vì: sắp xếp giảm: [6,5,4,3,2,1] → vị trí 2 là 5

Input:  nums = [3,2,3,1,2,4,5,5,6], k = 4
Output: 4

Input:  nums = [1], k = 1
Output: 1
```

**Constraint:**
- `1 <= k <= nums.length <= 10^5`
- `-10^4 <= nums[i] <= 10^4`

**Code C#:**
```csharp
// Approach 1: Min-Heap kích thước k — T: O(n log k), S: O(k)
public int FindKthLargest(int[] nums, int k)
{
    var minHeap = new PriorityQueue<int, int>();
    foreach (int n in nums)
    {
        minHeap.Enqueue(n, n);
        if (minHeap.Count > k)
            minHeap.Dequeue();
    }
    return minHeap.Peek();
    // T: O(n log k)   S: O(k)
}

// Approach 2: Quickselect — T: O(n) trung bình, O(n²) worst, S: O(1)
public int FindKthLargestQuickselect(int[] nums, int k)
{
    return Quickselect(nums, 0, nums.Length - 1, nums.Length - k);
}

private int Quickselect(int[] nums, int lo, int hi, int target)
{
    if (lo == hi) return nums[lo];
    int pivot = Partition(nums, lo, hi);
    if (pivot == target) return nums[pivot];
    if (pivot < target) return Quickselect(nums, pivot + 1, hi, target);
    return Quickselect(nums, lo, pivot - 1, target);
}

private int Partition(int[] nums, int lo, int hi)
{
    int pivot = nums[hi], i = lo;
    for (int j = lo; j < hi; j++)
        if (nums[j] <= pivot) { (nums[i], nums[j]) = (nums[j], nums[i]); i++; }
    (nums[i], nums[hi]) = (nums[hi], nums[i]);
    return i;
}
```

---

### Bài 5 — Task Scheduler (LC 621) ⭐⭐

**Đề:** Có danh sách task (ký tự A–Z). CPU cần thực thi đúng 1 task mỗi đơn vị thời gian, hoặc idle. Sau khi thực thi task X, phải chờ ít nhất n đơn vị trước khi thực thi X lại. Trả về thời gian tối thiểu để hoàn thành tất cả.

**Ví dụ:**
```
Input:  tasks = ["A","A","A","B","B","B"], n = 2
Output: 8
Vì: A → B → idle → A → B → idle → A → B

Input:  tasks = ["A","A","A","B","B","B"], n = 0
Output: 6
Vì: không cần chờ, chạy liên tục

Input:  tasks = ["A","A","A","A","A","A","B","C","D","E","F","G"], n = 2
Output: 16
```

**Constraint:**
- `1 <= tasks.length <= 10^4`
- `tasks[i]` là chữ hoa A–Z
- `0 <= n <= 100`

**Code C#:**
```csharp
// Optimal: Greedy + Max-Heap — T: O(n log 26) = O(n), S: O(26) = O(1)
public int LeastInterval(char[] tasks, int n)
{
    // Đếm tần suất từng loại task
    int[] freq = new int[26];
    foreach (char t in tasks)
        freq[t - 'A']++;

    // Max-heap theo tần suất
    var maxHeap = new PriorityQueue<int, int>();
    foreach (int f in freq)
        if (f > 0)
            maxHeap.Enqueue(f, -f);

    int time = 0;
    // Queue lưu (tần suất còn lại, thời điểm có thể dùng lại)
    var cooldown = new Queue<(int freq, int readyAt)>();

    while (maxHeap.Count > 0 || cooldown.Count > 0)
    {
        time++;

        if (maxHeap.Count > 0)
        {
            int f = maxHeap.Dequeue() - 1;  // thực thi 1 lần
            if (f > 0)
                cooldown.Enqueue((f, time + n));
        }

        // Task nào hết cooldown thì đưa lại heap
        if (cooldown.Count > 0 && cooldown.Peek().readyAt == time)
        {
            var (f, _) = cooldown.Dequeue();
            maxHeap.Enqueue(f, -f);
        }
    }

    return time;
    // T: O(n)   S: O(1) vì chỉ 26 ký tự
}
```

---

### Bài 6 — K Closest Points to Origin (LC 973) ⭐⭐

**Đề:** Cho mảng điểm 2D, tìm k điểm gần gốc (0,0) nhất. Khoảng cách Euclid, không cần căn bậc hai (so bình phương).

**Ví dụ:**
```
Input:  points = [[1,3],[-2,2]], k = 1
Output: [[-2,2]]
Vì: sqrt(1²+3²) = sqrt(10), sqrt((-2)²+2²) = sqrt(8) → [-2,2] gần hơn

Input:  points = [[3,3],[5,-1],[-2,4]], k = 2
Output: [[3,3],[-2,4]]

Input:  points = [[0,0]], k = 1
Output: [[0,0]]
```

**Constraint:**
- `1 <= k <= points.length <= 10^4`
- `-10^4 <= x, y <= 10^4`

**Code C#:**
```csharp
// Optimal: Max-Heap kích thước k — T: O(n log k), S: O(k)
public int[][] KClosest(int[][] points, int k)
{
    // Max-heap theo khoảng cách (loại điểm xa nhất khi vượt k)
    var maxHeap = new PriorityQueue<int[], int>();

    foreach (var p in points)
    {
        int dist = p[0] * p[0] + p[1] * p[1];
        maxHeap.Enqueue(p, -dist);  // đảo dấu → max-heap
        if (maxHeap.Count > k)
            maxHeap.Dequeue();
    }

    return maxHeap.UnorderedItems.Select(x => x.Element).ToArray();
    // T: O(n log k)   S: O(k)
}
```

---

### Bài 7 — Find Median from Data Stream (LC 295) ⭐⭐⭐ (bản lề Hard)

**Đề:** Thiết kế `MedianFinder`:
- `AddNum(int num)` — thêm số vào stream.
- `FindMedian()` — trả về trung vị hiện tại (số lẻ phần tử → phần tử giữa; số chẵn → trung bình 2 phần tử giữa).

**Ví dụ:**
```
AddNum(1) → stream: [1]
FindMedian() → 1.0

AddNum(2) → stream: [1,2]
FindMedian() → 1.5

AddNum(3) → stream: [1,2,3]
FindMedian() → 2.0

Edge case:
AddNum(-1), AddNum(-2) → stream: [-2,-1]
FindMedian() → -1.5
```

**Constraint:**
- `-10^5 <= num <= 10^5`
- `FindMedian` chỉ gọi khi có ít nhất 1 phần tử.
- Tối đa `5 * 10^4` lần gọi.

**Phân tích 5 câu hỏi** (bài bản lề — pattern Two Heaps):
1. **Input nói gì?** Dữ liệu đến dần, cần trung vị sau mỗi lần thêm.
2. **Output cần gì?** Trung vị — nghĩa là cần biết phần tử giữa (hoặc 2 phần tử giữa).
3. **Brute Force là gì?** Giữ mảng, mỗi lần sort rồi lấy giữa. T: O(n log n) mỗi lần.
4. **Chỗ lãng phí?** Sort lại toàn bộ. Nhận ra: chỉ cần max của nửa nhỏ và min của nửa lớn — dùng 2 heap!
5. **Pattern?** **Two Heaps** — max-heap nửa trái + min-heap nửa phải. Trung vị = đỉnh heap lớn hơn (hoặc trung bình 2 đỉnh).

**Code C#:**
```csharp
// Optimal: Two Heaps — T: O(log n) mỗi add, O(1) mỗi findMedian, S: O(n)
public class MedianFinder
{
    // maxHeap: nửa nhỏ (lưu âm để giả lập max-heap)
    private PriorityQueue<int, int> maxHeap;
    // minHeap: nửa lớn
    private PriorityQueue<int, int> minHeap;

    public MedianFinder()
    {
        maxHeap = new PriorityQueue<int, int>();
        minHeap = new PriorityQueue<int, int>();
    }

    public void AddNum(int num)
    {
        // Mặc định đẩy vào nửa nhỏ
        maxHeap.Enqueue(num, -num);

        // Đảm bảo max(nửa nhỏ) <= min(nửa lớn)
        if (minHeap.Count > 0 && maxHeap.Peek() > minHeap.Peek())
        {
            int val = maxHeap.Dequeue();
            minHeap.Enqueue(val, val);
        }

        // Cân bằng kích thước: maxHeap nhiều hơn minHeap tối đa 1
        if (maxHeap.Count > minHeap.Count + 1)
        {
            int val = maxHeap.Dequeue();
            minHeap.Enqueue(val, val);
        }
        else if (minHeap.Count > maxHeap.Count)
        {
            int val = minHeap.Dequeue();
            maxHeap.Enqueue(val, -val);
        }
    }

    public double FindMedian()
    {
        if (maxHeap.Count > minHeap.Count)
            return maxHeap.Peek();
        return (maxHeap.Peek() + minHeap.Peek()) / 2.0;
    }
}
```

---

### Bài 8 — Merge k Sorted Lists (LC 23) ⭐⭐⭐ (gặp lại từ Phase 7)

**Đề:** Cho k danh sách liên kết đã sắp xếp tăng dần, trộn thành một danh sách sắp xếp.

**Ví dụ:**
```
Input:  lists = [[1,4,5],[1,3,4],[2,6]]
Output: [1,1,2,3,4,4,5,6]

Input:  lists = []
Output: []

Input:  lists = [[]]
Output: []
```

**Constraint:**
- `0 <= k <= 10^4` (số danh sách)
- `0 <= tổng nodes <= 10^4`
- `-10^4 <= node.val <= 10^4`
- Mỗi danh sách đã sắp xếp tăng dần.

**Code C#:**
```csharp
// Optimal: Min-Heap k nguồn — T: O(n log k), S: O(k)
public ListNode MergeKLists(ListNode[] lists)
{
    // Min-heap: (giá trị, index danh sách, node hiện tại)
    var minHeap = new PriorityQueue<ListNode, int>();

    foreach (var head in lists)
        if (head != null)
            minHeap.Enqueue(head, head.val);

    var dummy = new ListNode(0);
    var cur = dummy;

    while (minHeap.Count > 0)
    {
        var node = minHeap.Dequeue();
        cur.next = node;
        cur = cur.next;
        if (node.next != null)
            minHeap.Enqueue(node.next, node.next.val);
    }

    return dummy.next;
    // T: O(n log k) với n = tổng nodes, k = số danh sách   S: O(k)
}
```

---

## Tổng kết Pattern

| Pattern | Dấu hiệu | Heap dùng | Độ phức tạp |
|---------|----------|-----------|------------|
| Top-K lớn nhất | "k phần tử lớn nhất" | Min-heap kích thước k | O(n log k) |
| Top-K nhỏ nhất | "k phần tử nhỏ nhất" | Max-heap kích thước k | O(n log k) |
| Trộn k nguồn | "k danh sách sắp xếp" | Min-heap k đầu | O(n log k) |
| Lập lịch | "luôn xử lý ưu tiên cao nhất" | Max-heap | O(n log n) |
| Two Heaps | "median liên tục" | Max-heap + Min-heap | O(log n) mỗi add |

## Tiêu chí hoàn thành Phase 10

- [ ] Biết chọn min-heap vs max-heap và giới hạn kích thước k đúng.
- [ ] Giải được Find Median from Data Stream (two heaps).
- [ ] Phân biệt khi nào heap, khi nào sort, khi nào quickselect.
