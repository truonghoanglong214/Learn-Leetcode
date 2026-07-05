# Phase 16 — Mixed & Mô phỏng phỏng vấn

> **Ngôn ngữ:** C#
> **Mục tiêu:** Chuyển từ "biết từng pattern" sang "**nhìn đề lạ là phân loại được**" và trình bày như trong phỏng vấn thật: nói to suy nghĩ, brute force trước, tối ưu dần, phân tích độ phức tạp, viết code sạch, test bằng tay.

---

## Phần A — Quy trình phỏng vấn chuẩn

### A.1 7 bước trong 45 phút

```
Bước 1 (2–3 phút)   — Làm rõ đề: hỏi về kiểu input, độ dài, giá trị đặc biệt.
Bước 2 (2–3 phút)   — Xây ví dụ: viết 2–3 test case bằng tay (không dùng ví dụ có sẵn).
Bước 3 (3–5 phút)   — Brute force: nêu lời giải "ngu" nhất, tính độ phức tạp.
Bước 4 (5–10 phút)  — Tối ưu: chạy "vòng lặp tư duy 5 câu hỏi", xác định pattern.
Bước 5 (15–20 phút) — Code: viết từ trên xuống, nói to từng dòng.
Bước 6 (3–5 phút)   — Test: chạy tay qua test case, kiểm tra edge case.
Bước 7 (2–3 phút)   — Phân tích: nêu time & space complexity, bàn về cải tiến.
```

> **Nguyên tắc vàng:** Không im lặng quá 30 giây. Người phỏng vấn chấm điểm *quá trình suy nghĩ*, không chỉ đáp án cuối.

---

### A.2 Vòng lặp tư duy 5 câu hỏi (nhắc lại)

Mỗi bài lạ, chạy đủ 5 câu này trước khi code:

```
1. Input nói gì?     Đã sắp xếp? Có trùng? Có số âm? n = bao nhiêu?
2. Output cần gì?    Một số? Mảng? Boolean? Đếm? Liệt kê tất cả?
3. Brute force?      Lời giải "ngu" O(n²) / O(2ⁿ) là gì?
4. Chỗ lãng phí?     Đang tính lại cái gì? Quét lại cái gì?
5. Pattern khớp?     So với bảng dấu hiệu nhận biết bên dưới.
```

---

### A.3 Bảng nhận diện pattern tổng hợp

| Dấu hiệu đề | Pattern đầu tiên thử |
|---|---|
| Có trùng? Tìm cặp? Đếm tần suất? | **Hash Map / Set** (Phase 1) |
| Mảng sắp xếp + tìm cặp/bộ tổng | **Two Pointers** (Phase 2) |
| Liên tiếp / subarray / substring dài nhất / ngắn nhất | **Sliding Window** (Phase 3) |
| Tổng khoảng / đếm subarray sum = k / có số âm | **Prefix Sum + Hash** (Phase 4) |
| Sắp xếp + tìm nhanh / "nhỏ nhất sao cho có thể" | **Binary Search** (Phase 5) |
| Ngoặc lồng / next greater / hình chữ nhật histogram | **Stack / Monotonic Stack** (Phase 6) |
| Node next / chu trình / đảo / k từ cuối | **Linked List** (Phase 7) |
| Liệt kê tất cả / n nhỏ / đặt từng cái thỏa ràng buộc | **Backtracking** (Phase 8) |
| Cây nhị phân / BST / duyệt theo tầng / LCA | **Trees DFS/BFS** (Phase 9) |
| Top-K / trộn nguồn sắp xếp / median / lập lịch | **Heap** (Phase 10) |
| Đảo / vùng / số bước ít nhất / lan từ nhiều nguồn | **Graph BFS/DFS** (Phase 11) |
| Kết nối nhóm động / thứ tự phụ thuộc / shortest có trọng số | **Advanced Graph** (Phase 12) |
| Số ít nhất không chồng lấn / gộp khoảng / lập lịch tham | **Greedy + Intervals** (Phase 13) |
| Đếm cách / tối ưu qua nhiều bước / bài con chồng lặp | **Dynamic Programming** (Phase 14) |
| Tiền tố chuỗi / bitmask n≤20 / cửa sổ max/min O(n) | **Trie / Bit / Monotonic Deque** (Phase 15) |

---

### A.4 Template giao tiếp

```
Khi đọc đề:
"OK, để tôi xác nhận lại — input là [X], output cần [Y].
 Mảng có thể rỗng không? Giá trị có âm không?"

Khi brute force:
"Cách đơn giản nhất là [mô tả]. Độ phức tạp O([X]) — chắc chắn không đủ
 vì n = [Y]. Tôi sẽ tìm cách cải thiện."

Khi tìm pattern:
"Chỗ lãng phí ở đây là [Z]. Nếu tôi dùng [pattern] thì có thể tránh
 được bằng cách [giải thích]. Anh/chị thấy hướng này ổn không?"

Khi bí:
"Tôi đang nghĩ tới [hướng A] nhưng vẫn chưa thấy cách xử lý [vấn đề B].
 Anh/chị có muốn tôi thử viết brute force trước để làm rõ không?"

Khi test:
"Tôi chạy tay với input [ví dụ]. Bước 1: [X]. Bước 2: [Y]. Kết quả [Z].
 Khớp với expected. Giờ tôi thử edge case [rỗng / một phần tử / trùng]."
```

---

## Phần B — Bài tập thiết kế hệ thống (Design Problems)

Các bài này yêu cầu **kết hợp nhiều cấu trúc dữ liệu** — hay gặp nhất trong phỏng vấn.

---

### Bài B1 — LRU Cache (146) ⭐⭐⭐ — Bản lề thiết kế

**Đề:** Thiết kế cấu trúc LRU Cache với capacity cố định. Hỗ trợ:
- `Get(key)` — trả về giá trị nếu tồn tại (và đánh dấu recently used), ngược lại trả −1.
- `Put(key, value)` — chèn/cập nhật. Nếu đầy, xóa phần tử **ít được dùng nhất gần đây (LRU)**.
- Cả hai thao tác phải O(1).

**Ví dụ:**
```
capacity = 2
Put(1, 1)   → cache: {1=1}
Put(2, 2)   → cache: {1=1, 2=2}
Get(1)      → 1    (1 trở thành most recently used)
Put(3, 3)   → cache đầy, LRU là key=2 → xóa 2. Cache: {1=1, 3=3}
Get(2)      → -1   (đã bị xóa)
Put(4, 4)   → LRU là key=1 → xóa. Cache: {3=3, 4=4}
Get(1)      → -1
Get(3)      → 3
Get(4)      → 4

Edge case:
capacity = 1
Put(1, 1) → {1=1}
Put(2, 2) → xóa 1. {2=2}
Get(1)    → -1
```

**Constraint:** `1 ≤ capacity ≤ 3000`, `0 ≤ key, value ≤ 10⁴`, tối đa 2×10⁵ lời gọi.

**Phân tích 5 câu hỏi:**
1. Capacity cố định, thao tác get/put.
2. Cần O(1) cho cả hai thao tác.
3. Brute force: dùng `List<(key,value,time)>` + quét để tìm LRU — O(n) mỗi thao tác.
4. Chỗ lãng phí: quét để tìm LRU và để tra cứu theo key — cần tra O(1) **và** xóa O(1) bất kỳ vị trí.
5. Pattern: **HashMap + Doubly Linked List** — hash cho O(1) lookup, linked list giữ thứ tự recently used, doubly để xóa O(1) bất kỳ node.

```csharp
// O(1) get & put
public class LRUCache
{
    private class Node
    {
        public int Key, Val;
        public Node Prev, Next;
        public Node(int k, int v) { Key = k; Val = v; }
    }

    private int _cap;
    private Dictionary<int, Node> _map = new();
    private Node _head = new Node(0, 0); // dummy LRU end
    private Node _tail = new Node(0, 0); // dummy MRU end

    public LRUCache(int capacity)
    {
        _cap = capacity;
        _head.Next = _tail;
        _tail.Prev = _head;
    }

    public int Get(int key)
    {
        if (!_map.ContainsKey(key)) return -1;
        var node = _map[key];
        Remove(node);
        InsertMRU(node);
        return node.Val;
    }

    public void Put(int key, int value)
    {
        if (_map.ContainsKey(key))
            Remove(_map[key]);
        var node = new Node(key, value);
        _map[key] = node;
        InsertMRU(node);
        if (_map.Count > _cap)
        {
            var lru = _head.Next; // node ngay sau dummy head = LRU
            Remove(lru);
            _map.Remove(lru.Key);
        }
    }

    private void Remove(Node node)
    {
        node.Prev.Next = node.Next;
        node.Next.Prev = node.Prev;
    }

    private void InsertMRU(Node node)
    {
        // chèn ngay trước dummy tail = vị trí MRU
        node.Prev = _tail.Prev;
        node.Next = _tail;
        _tail.Prev.Next = node;
        _tail.Prev = node;
    }
}
// Time: O(1) get & put | Space: O(capacity)
```

---

### Bài B2 — Insert Delete GetRandom O(1) (380) ⭐⭐⭐

**Đề:** Thiết kế cấu trúc với các thao tác trung bình O(1):
- `Insert(val)` — chèn val. Trả `true` nếu chưa có, `false` nếu đã có.
- `Remove(val)` — xóa val. Trả `true` nếu có, `false` nếu không.
- `GetRandom()` — trả một phần tử ngẫu nhiên với xác suất **đều nhau**.

**Ví dụ:**
```
Insert(1) → true.   Tập: {1}
Insert(2) → true.   Tập: {1,2}
Insert(2) → false.  Tập: {1,2}  (đã có)
GetRandom → 1 hoặc 2, mỗi cái 50%.
Remove(1) → true.   Tập: {2}
GetRandom → 2       (100%)

Edge case:
Remove phần tử không tồn tại → false
GetRandom khi chỉ có 1 phần tử → trả phần tử đó
```

**Constraint:** `-2³¹ ≤ val ≤ 2³¹ − 1`, tối đa 2×10⁵ lời gọi. GetRandom chỉ gọi khi tập không rỗng.

**Phân tích 5 câu hỏi:**
1. Giá trị bất kỳ (int32), không có thứ tự.
2. Insert/Remove O(1) trung bình + GetRandom O(1) đều.
3. Brute force: HashSet → Insert/Remove/Contains O(1) nhưng GetRandom O(n) (phải duyệt).
4. Chỗ lãng phí: không có index để chọn ngẫu nhiên O(1). Cần cả List (để random by index) **và** Map (để xóa O(1)).
5. Pattern: **List + HashMap** — khi xóa, hoán đổi với phần tử cuối để tránh shift.

```csharp
public class RandomizedSet
{
    private List<int> _list = new();
    private Dictionary<int, int> _map = new(); // val → index trong list
    private Random _rand = new Random();

    public bool Insert(int val)
    {
        if (_map.ContainsKey(val)) return false;
        _map[val] = _list.Count;
        _list.Add(val);
        return true;
    }

    public bool Remove(int val)
    {
        if (!_map.ContainsKey(val)) return false;
        int idx = _map[val];
        int last = _list[^1];
        _list[idx] = last;      // đưa phần tử cuối vào vị trí bị xóa
        _map[last] = idx;       // cập nhật index của last
        _list.RemoveAt(_list.Count - 1);
        _map.Remove(val);
        return true;
    }

    public int GetRandom() => _list[_rand.Next(_list.Count)];
}
// Time: O(1) trung bình | Space: O(n)
```

---

### Bài B3 — Time Based Key-Value Store (981) ⭐⭐⭐

**Đề:** Thiết kế key-value store hỗ trợ timestamp:
- `Set(key, value, timestamp)` — lưu (key, value) tại thời điểm timestamp. Đảm bảo timestamp tăng dần.
- `Get(key, timestamp)` — trả về value của key tại thời điểm `t ≤ timestamp` lớn nhất. Nếu không có, trả `""`.

**Ví dụ:**
```
Set("foo", "bar", 1)
Set("foo", "bar2", 4)
Get("foo", 4)  → "bar2"   (t=4 ≤ 4)
Get("foo", 5)  → "bar2"   (t=4 là lớn nhất ≤ 5)
Get("foo", 3)  → "bar"    (t=1 là lớn nhất ≤ 3)
Get("foo", 0)  → ""       (không có t ≤ 0)

Edge case:
Get key chưa từng Set → ""
```

**Constraint:** `1 ≤ key.length, value.length ≤ 100`, `1 ≤ timestamp ≤ 10⁷`, tối đa 2×10⁵ lời gọi.

**Phân tích 5 câu hỏi:**
1. Timestamp tăng dần (quan trọng!) → danh sách đã sắp xếp sẵn.
2. Tìm timestamp lớn nhất ≤ target → **upper bound − 1**.
3. Brute force: với mỗi Get, duyệt toàn bộ timestamp của key → O(n).
4. Chỗ lãng phí: timestamp đã sắp xếp → Binary Search.
5. Pattern: **HashMap của List + Binary Search (upper bound)**.

```csharp
public class TimeMap
{
    // key → list of (timestamp, value), đã sắp xếp tăng dần theo timestamp
    private Dictionary<string, List<(int ts, string val)>> _store = new();

    public void Set(string key, string value, int timestamp)
    {
        if (!_store.ContainsKey(key))
            _store[key] = new List<(int, string)>();
        _store[key].Add((timestamp, value));
    }

    public string Get(string key, int timestamp)
    {
        if (!_store.ContainsKey(key)) return "";
        var list = _store[key];
        // Binary search: tìm index cuối cùng có ts ≤ timestamp
        int lo = 0, hi = list.Count - 1, ans = -1;
        while (lo <= hi)
        {
            int mid = lo + (hi - lo) / 2;
            if (list[mid].ts <= timestamp)
            {
                ans = mid;
                lo = mid + 1; // thử tìm xa hơn bên phải
            }
            else
                hi = mid - 1;
        }
        return ans == -1 ? "" : list[ans].val;
    }
}
// Set: O(1) | Get: O(log n) | Space: O(n)
```

---

### Bài B4 — Design Twitter (355) ⭐⭐⭐

**Đề:** Thiết kế Twitter đơn giản:
- `PostTweet(userId, tweetId)` — user đăng tweet.
- `GetNewsFeed(userId)` — trả 10 tweet mới nhất từ user đó **và** những người họ follow (theo thứ tự mới → cũ).
- `Follow(followerId, followeeId)` — follow.
- `Unfollow(followerId, followeeId)` — unfollow.

**Ví dụ:**
```
PostTweet(1, 5)
GetNewsFeed(1)    → [5]
Follow(1, 2)
PostTweet(2, 6)
GetNewsFeed(1)    → [6, 5]   (6 mới hơn)
Unfollow(1, 2)
GetNewsFeed(1)    → [5]

Edge case:
GetNewsFeed của user chưa post và chưa follow ai → []
Follow chính mình → không ảnh hưởng (vẫn thấy tweet của mình)
```

**Constraint:** `1 ≤ userId, tweetId ≤ 500`, tối đa 3×10⁴ lời gọi.

**Phân tích 5 câu hỏi:**
1. Nhiều user, mỗi user có danh sách tweet và danh sách follow.
2. GetNewsFeed cần 10 tweet mới nhất từ nhiều nguồn → **trộn k danh sách sắp xếp**.
3. Brute force: gộp tất cả tweet vào một list, sort theo time, lấy 10 đầu.
4. Chỗ lãng phí: sort toàn bộ → dùng min-heap để trộn k nguồn (mỗi nguồn là danh sách tweet của 1 user).
5. Pattern: **HashMap + Heap (merge k sorted lists)**.

```csharp
public class Twitter
{
    private int _time = 0;
    // userId → list of (timestamp, tweetId)
    private Dictionary<int, List<(int ts, int tid)>> _tweets = new();
    // followerId → set of followeeId
    private Dictionary<int, HashSet<int>> _follows = new();

    private List<(int ts, int tid)> UserTweets(int userId)
    {
        if (!_tweets.ContainsKey(userId))
            _tweets[userId] = new();
        return _tweets[userId];
    }

    private HashSet<int> UserFollows(int userId)
    {
        if (!_follows.ContainsKey(userId))
            _follows[userId] = new();
        return _follows[userId];
    }

    public void PostTweet(int userId, int tweetId)
        => UserTweets(userId).Add((_time++, tweetId));

    public IList<int> GetNewsFeed(int userId)
    {
        // min-heap (ts, tweetId, listRef, index) — lấy tweet mới nhất trước dùng max-ts
        // dùng PriorityQueue<T, priority> với priority = -ts để giả lập max-heap
        var pq = new PriorityQueue<(int ts, int tid, List<(int,int)> src, int idx), int>();

        void Enqueue(int uid)
        {
            var list = UserTweets(uid);
            if (list.Count > 0)
            {
                int i = list.Count - 1; // tweet mới nhất ở cuối
                pq.Enqueue((list[i].ts, list[i].tid, list, i), -list[i].ts);
            }
        }

        Enqueue(userId);
        foreach (int fid in UserFollows(userId))
            Enqueue(fid);

        var result = new List<int>();
        while (pq.Count > 0 && result.Count < 10)
        {
            var (ts, tid, src, idx) = pq.Dequeue();
            result.Add(tid);
            if (idx > 0)
                pq.Enqueue((src[idx-1].ts, src[idx-1].tid, src, idx-1), -src[idx-1].ts);
        }
        return result;
    }

    public void Follow(int followerId, int followeeId)
        => UserFollows(followerId).Add(followeeId);

    public void Unfollow(int followerId, int followeeId)
        => UserFollows(followerId).Remove(followeeId);
}
// GetNewsFeed: O(k log k + 10 log k) với k = số nguồn | Space: O(tổng tweet + follows)
```

---

## Phần C — Bài tập tổng hợp trộn pattern

Phần này **không gợi ý pattern** — tự nhận diện rồi giải.

---

### Bài C1 — Subarray Sum Equals K với số âm ⭐⭐⭐ (tổng hợp Phase 1 + 4)

**Đề:** Cho mảng số nguyên `nums` (có thể âm) và số nguyên `k`. Đếm số subarray liên tiếp có tổng đúng bằng `k`.

**Ví dụ:**
```
Input:  nums = [1, 1, 1], k = 2
Output: 2
Vì: [1,1] (index 0–1) và [1,1] (index 1–2)

Input:  nums = [1, -1, 1], k = 1
Output: 3
Vì: [1] (i=0), [1,-1,1] (i=0..2), [1] (i=2)

Input:  nums = [3, 4, 7, 2, -3, 1, 4, 2], k = 7
Output: 4

Edge case:
Input:  nums = [0, 0, 0], k = 0
Output: 6   (mọi subarray đều có tổng 0)
```

**Constraint:** `1 ≤ nums.length ≤ 2×10⁴`, `-1000 ≤ nums[i] ≤ 1000`, `-10⁷ ≤ k ≤ 10⁷`.

**Phân tích 5 câu hỏi:**
1. Mảng có số âm → không dùng sliding window được.
2. Đếm số subarray có tổng = k.
3. Brute force: hai vòng lặp O(n²) tính mọi tổng đoạn con.
4. Chỗ lãng phí: tính lại tổng từng cặp (i,j) → dùng prefix sum. Tổng(i..j) = prefix[j] − prefix[i−1] = k → tìm "đã thấy prefix nào bằng prefix[j]−k?" → hash map.
5. Pattern: **Prefix Sum + Hash Map**, khởi tạo `{0: 1}`.

```csharp
public int SubarraySum(int[] nums, int k)
{
    // prefixCount[s] = số lần đã thấy prefix sum = s
    var prefixCount = new Dictionary<int, int> { [0] = 1 };
    int sum = 0, count = 0;
    foreach (int x in nums)
    {
        sum += x;
        // tìm prefix trước đó = sum - k → tổng đoạn = k
        if (prefixCount.TryGetValue(sum - k, out int c))
            count += c;
        prefixCount[sum] = prefixCount.GetValueOrDefault(sum) + 1;
    }
    return count;
    // Time: O(n) | Space: O(n)
}
```

---

### Bài C2 — Longest Consecutive Sequence (128) ⭐⭐⭐ (Phase 1 nâng cao)

**Đề:** Cho mảng số nguyên chưa sắp xếp `nums`. Tìm độ dài dãy số liên tiếp (consecutive sequence) dài nhất. Yêu cầu O(n).

**Ví dụ:**
```
Input:  nums = [100, 4, 200, 1, 3, 2]
Output: 4
Vì: [1, 2, 3, 4]

Input:  nums = [0, 3, 7, 2, 5, 8, 4, 6, 0, 1]
Output: 9
Vì: [0,1,2,3,4,5,6,7,8]

Edge case:
Input:  nums = []       → 0
Input:  nums = [1]      → 1
Input:  nums = [1,1,1]  → 1  (trùng lặp không tính thêm)
```

**Constraint:** `0 ≤ nums.length ≤ 10⁵`, `-10⁹ ≤ nums[i] ≤ 10⁹`.

**Phân tích 5 câu hỏi:**
1. Mảng không sắp xếp, có trùng lặp, n ≤ 10⁵ → cần O(n).
2. Độ dài dãy liên tiếp dài nhất.
3. Brute force: sort O(n log n) rồi quét → không thỏa O(n).
4. Chỗ lãng phí: sort → dùng HashSet để tra O(1). Với mỗi số x, **chỉ bắt đầu đếm nếu x−1 không có trong set** (tránh lặp lại).
5. Pattern: **Hash Set + chỉ bắt đầu đếm từ đầu chuỗi**.

```csharp
public int LongestConsecutive(int[] nums)
{
    var set = new HashSet<int>(nums);
    int best = 0;
    foreach (int x in set)
    {
        if (set.Contains(x - 1)) continue; // không phải đầu chuỗi
        int len = 1;
        while (set.Contains(x + len)) len++;
        best = Math.Max(best, len);
    }
    return best;
    // Time: O(n) — mỗi số vào/ra vòng while đúng 1 lần | Space: O(n)
}
```

---

### Bài C3 — Word Ladder (127) ⭐⭐⭐⭐ (Phase 5 + 11)

**Đề:** Cho `beginWord`, `endWord` và danh sách từ `wordList`. Mỗi bước được đổi đúng 1 ký tự, từ mới phải có trong `wordList`. Tìm số bước **ít nhất** để đến `endWord` từ `beginWord`. Nếu không đến được, trả 0.

**Ví dụ:**
```
Input:  beginWord = "hit", endWord = "cog"
        wordList = ["hot","dot","dog","lot","log","cog"]
Output: 5
Vì: "hit" → "hot" → "dot" → "dog" → "cog"

Input:  beginWord = "hit", endWord = "cog"
        wordList = ["hot","dot","dog","lot","log"]
Output: 0    (không có "cog" trong wordList)

Edge case:
beginWord = endWord = "hit", wordList = ["hit"] → 1 (đã ở đích)
```

**Constraint:** `1 ≤ beginWord.length ≤ 10`, từ chỉ gồm chữ thường, `1 ≤ wordList.length ≤ 5000`.

**Phân tích 5 câu hỏi:**
1. Đồ thị ẩn — cạnh giữa hai từ khác nhau đúng 1 ký tự.
2. Số bước ít nhất → BFS (không trọng số).
3. Brute force: BFS với mọi cặp từ, kiểm tra từng cặp O(L×N²).
4. Chỗ lãng phí: kiểm tra kề O(N) mỗi node → thay bằng thử 26 ký tự × L vị trí = O(26L) mỗi node.
5. Pattern: **BFS trên đồ thị ẩn + HashSet word list**.

```csharp
public int LadderLength(string beginWord, string endWord, IList<string> wordList)
{
    var wordSet = new HashSet<string>(wordList);
    if (!wordSet.Contains(endWord)) return 0;

    var queue = new Queue<string>();
    queue.Enqueue(beginWord);
    wordSet.Remove(beginWord); // đánh dấu visited bằng cách xóa khỏi set
    int steps = 1;

    while (queue.Count > 0)
    {
        int size = queue.Count;
        for (int i = 0; i < size; i++)
        {
            string word = queue.Dequeue();
            char[] arr = word.ToCharArray();
            for (int pos = 0; pos < arr.Length; pos++)
            {
                char orig = arr[pos];
                for (char c = 'a'; c <= 'z'; c++)
                {
                    if (c == orig) continue;
                    arr[pos] = c;
                    string next = new string(arr);
                    if (next == endWord) return steps + 1;
                    if (wordSet.Contains(next))
                    {
                        queue.Enqueue(next);
                        wordSet.Remove(next);
                    }
                }
                arr[pos] = orig;
            }
        }
        steps++;
    }
    return 0;
    // Time: O(N × L × 26) | Space: O(N × L)
}
```

---

### Bài C4 — Trapping Rain Water (42) ⭐⭐⭐⭐ (Phase 2 + 4 + 6)

**Đề:** Cho mảng `height` biểu diễn bản đồ chiều cao. Tính lượng nước mưa được giữ lại.

**Ví dụ:**
```
Input:  height = [0,1,0,2,1,0,1,3,2,1,2,1]
Output: 6

Input:  height = [4,2,0,3,2,5]
Output: 9

Edge case:
height = []         → 0
height = [3]        → 0   (một cột không giữ được nước)
height = [1,2,3]    → 0   (dốc lên, không có chỗ đọng)
height = [3,2,1]    → 0   (dốc xuống)
height = [3,0,3]    → 3
```

**Constraint:** `0 ≤ height.length ≤ 2×10⁴`, `0 ≤ height[i] ≤ 10⁵`.

**Phân tích 5 câu hỏi:**
1. Mảng không âm, không sắp xếp.
2. Tổng nước giữ lại.
3. Brute force: mỗi ô i, tìm maxLeft và maxRight rồi `min(maxL, maxR) − height[i]` → O(n²).
4. Chỗ lãng phí: tính lại maxLeft/maxRight mỗi lần → có 3 cách tối ưu: (a) prefix max array O(n) space, (b) two pointers O(1) space, (c) stack O(n).
5. Pattern: **Two Pointers (Phase 2) — "nút thắt thấp hơn quyết định"**.

```csharp
// Cách 1: Prefix max — O(n) time, O(n) space (dễ hiểu)
public int TrapPrefix(int[] height)
{
    int n = height.Length;
    if (n == 0) return 0;
    int[] maxL = new int[n], maxR = new int[n];
    maxL[0] = height[0];
    for (int i = 1; i < n; i++) maxL[i] = Math.Max(maxL[i-1], height[i]);
    maxR[n-1] = height[n-1];
    for (int i = n-2; i >= 0; i--) maxR[i] = Math.Max(maxR[i+1], height[i]);
    int water = 0;
    for (int i = 0; i < n; i++)
        water += Math.Min(maxL[i], maxR[i]) - height[i];
    return water;
}

// Cách 2: Two Pointers — O(n) time, O(1) space (tối ưu)
public int Trap(int[] height)
{
    int lo = 0, hi = height.Length - 1;
    int maxL = 0, maxR = 0, water = 0;
    while (lo < hi)
    {
        if (height[lo] <= height[hi])
        {
            // nút thắt bên trái: nước tại lo do maxL quyết định
            maxL = Math.Max(maxL, height[lo]);
            water += maxL - height[lo];
            lo++;
        }
        else
        {
            maxR = Math.Max(maxR, height[hi]);
            water += maxR - height[hi];
            hi--;
        }
    }
    return water;
    // Time: O(n) | Space: O(1)
}
```

---

### Bài C5 — Find Median from Data Stream (295) ⭐⭐⭐⭐ (Phase 10)

**Đề:** Thiết kế cấu trúc tính **median** của dòng số liên tục chảy vào:
- `AddNum(num)` — thêm số vào cấu trúc.
- `FindMedian()` — trả median hiện tại (double).

**Ví dụ:**
```
AddNum(1)    → stream: [1],    median = 1.0
AddNum(2)    → stream: [1,2],  median = 1.5
AddNum(3)    → stream: [1,2,3], median = 2.0
AddNum(0)    → stream: [0,1,2,3], median = 1.5

Edge case:
AddNum với số âm, số rất lớn, số trùng lặp → đều phải hoạt động đúng
```

**Constraint:** `-10⁵ ≤ num ≤ 10⁵`, tối đa 5×10⁴ lời gọi.

**Phân tích 5 câu hỏi:**
1. Dòng dữ liệu không biết trước, cần cập nhật liên tục.
2. Median = phần tử giữa (nếu lẻ) hoặc trung bình hai phần tử giữa (nếu chẵn).
3. Brute force: giữ sorted list, insert O(n), median O(1) → quá chậm.
4. Chỗ lãng phí: sort lại mỗi lần → dùng 2 heap: **max-heap chứa nửa nhỏ** (lo), **min-heap chứa nửa lớn** (hi). Đảm bảo `lo.Count − hi.Count ≤ 1` và `lo.Max ≤ hi.Min`.
5. Pattern: **Two Heaps** — median = đỉnh lo (nếu lẻ) hoặc (lo.Max + hi.Min) / 2 (nếu chẵn).

```csharp
public class MedianFinder
{
    // lo: max-heap chứa nửa nhỏ (dùng dấu âm để giả lập max-heap)
    private PriorityQueue<int, int> _lo = new();
    // hi: min-heap chứa nửa lớn
    private PriorityQueue<int, int> _hi = new();

    public void AddNum(int num)
    {
        // Luôn đẩy vào lo trước
        _lo.Enqueue(num, -num);

        // Đảm bảo lo.Max ≤ hi.Min
        if (_hi.Count > 0 && _lo.TryPeek(out int loMax, out _) &&
            _hi.TryPeek(out int hiMin, out _) && loMax > hiMin)
        {
            _lo.Dequeue();
            _hi.Enqueue(loMax, loMax);
        }

        // Cân bằng kích thước: lo nhiều hơn hi tối đa 1
        if (_lo.Count > _hi.Count + 1)
        {
            int top = _lo.Dequeue();
            _hi.Enqueue(top, top);
        }
        else if (_hi.Count > _lo.Count)
        {
            int top = _hi.Dequeue();
            _lo.Enqueue(top, -top);
        }
    }

    public double FindMedian()
    {
        if (_lo.Count > _hi.Count)
            return _lo.Peek();
        return (_lo.Peek() + _hi.Peek()) / 2.0;
    }
}
// AddNum: O(log n) | FindMedian: O(1) | Space: O(n)
```

---

## Phần D — Format mock interview tự luyện

### D.1 Quy trình 45 phút một mình

```
0:00 — Đọc đề, không code. Viết ra:
         - Input/Output type
         - Constraints
         - 2 test case tự nghĩ (không dùng ví dụ có sẵn)
         - 1 edge case

0:05 — Brute force bằng lời (không code). Tính độ phức tạp.

0:10 — Tối ưu: chạy 5 câu hỏi. Viết tên pattern nếu nhận ra.

0:15 — Bắt đầu code. Nói to từng bước.

0:35 — Test tay với test case đã viết. Sửa nếu sai.

0:40 — Phân tích time & space. Bàn về cải tiến có thể.

0:45 — Nhìn lại: mình đã nhận ra pattern lúc nào? Có bỏ lỡ edge case nào không?
```

### D.2 Thang điểm tự đánh giá

| Tiêu chí | 3 điểm | 2 điểm | 1 điểm |
|---|---|---|---|
| Nhận diện pattern | Ngay khi đọc đề | Sau khi brute force | Sau khi gợi ý |
| Xử lý edge case | Tự phát hiện hết | Bỏ sót 1 | Bỏ sót nhiều |
| Code đúng ngay | Đúng lần đầu | Sửa 1–2 lỗi nhỏ | Cần debug nhiều |
| Phân tích complexity | Chính xác, tự tin | Đúng nhưng chậm | Sai hoặc không làm |
| **Tổng** | **12** — sẵn sàng phỏng vấn | **8–11** — cần thêm luyện | **<8** — ôn lại pattern |

---

### D.3 Danh sách 30 bài luyện trộn (không gợi ý pattern)

Làm ngẫu nhiên — đọc đề, tự phân loại, rồi kiểm tra lại sau.

| # | Bài | Phase gốc |
|---|---|---|
| 1 | Two Sum (1) | 1 |
| 2 | Best Time to Buy and Sell Stock (121) | 3 |
| 3 | Valid Parentheses (20) | 6 |
| 4 | Merge Intervals (56) | 13 |
| 5 | Coin Change (322) | 14 |
| 6 | Number of Islands (200) | 11 |
| 7 | Find Minimum in Rotated Sorted Array (153) | 5 |
| 8 | Maximum Subarray (53) | 13 |
| 9 | Climbing Stairs (70) | 14 |
| 10 | Reverse Linked List (206) | 7 |
| 11 | Longest Substring Without Repeating Characters (3) | 3 |
| 12 | 3Sum (15) | 2 |
| 13 | Binary Tree Level Order Traversal (102) | 9 |
| 14 | Course Schedule (207) | 11 |
| 15 | Subarray Sum Equals K (560) | 4 |
| 16 | Top K Frequent Elements (347) | 1+10 |
| 17 | Search in Rotated Sorted Array (33) | 5 |
| 18 | House Robber (198) | 14 |
| 19 | Daily Temperatures (739) | 6 |
| 20 | Reorder List (143) | 7 |
| 21 | Lowest Common Ancestor (236) | 9 |
| 22 | Longest Increasing Subsequence (300) | 14 |
| 23 | Graph Valid Tree (261) | 12 |
| 24 | Pacific Atlantic Water Flow (417) | 11 |
| 25 | Binary Tree Maximum Path Sum (124) | 9 |
| 26 | Word Break (139) | 14 |
| 27 | Kth Largest Element in an Array (215) | 10 |
| 28 | Rotting Oranges (994) | 11 |
| 29 | Non-overlapping Intervals (435) | 13 |
| 30 | Merge k Sorted Lists (23) | 7+10 |

---

## Tiêu chí hoàn thành Phase 16

- [ ] Phân loại đúng pattern của một bài lạ trong < 2 phút ở ≥ 80% trường hợp.
- [ ] Hoàn thành bài Medium trong ~25–30 phút có nói to suy nghĩ (tự bấm giờ).
- [ ] Tự giải được LRU Cache, RandomizedSet, TimeMap không nhìn lời giải.
- [ ] Đạt ≥ 10 điểm theo thang D.2 trong 3 buổi mock liên tiếp.
- [ ] Trình bày được mạch brute force → optimal + phân tích complexity trôi chảy.
