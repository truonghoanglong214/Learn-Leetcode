# Phase 15 — Advanced (Trie, Bit, Segment Tree, Monotonic Queue, Advanced DP)

> **Ngôn ngữ:** C#
> **Mục tiêu:** Trang bị "vũ khí hạng nặng" cho bài Hard. Không cần thành thần — cần **nhận diện** khi nào dùng và biết cách triển khai cơ bản.

---

## Phần A — Trie (Prefix Tree)

### A.1 Trie là gì?

Trie là cây dùng để **lưu và tìm kiếm chuỗi theo tiền tố**. Mỗi node đại diện một ký tự; đường từ gốc đến node = tiền tố của các từ đã chèn.

**Độ phức tạp:**
- Chèn: O(L) với L = độ dài từ.
- Tìm kiếm: O(L).
- Không gian: O(tổng độ dài tất cả từ × size bảng chữ cái).

**Khi nào dùng Trie thay vì HashSet?**
- Cần tìm kiếm theo **tiền tố** (startsWith).
- Cần xử lý **nhiều từ** cùng lúc trên một chuỗi (Word Search II).
- Tìm cặp từ có XOR lớn nhất (Maximum XOR).

### A.2 Template Trie Node C#

```csharp
public class TrieNode
{
    public TrieNode[] Children = new TrieNode[26];
    public bool IsEnd = false;
}

public class Trie
{
    private TrieNode root = new TrieNode();

    public void Insert(string word)
    {
        TrieNode node = root;
        foreach (char c in word)
        {
            int idx = c - 'a';
            if (node.Children[idx] == null)
                node.Children[idx] = new TrieNode();
            node = node.Children[idx];
        }
        node.IsEnd = true;
    }

    public bool Search(string word)
    {
        TrieNode node = root;
        foreach (char c in word)
        {
            int idx = c - 'a';
            if (node.Children[idx] == null) return false;
            node = node.Children[idx];
        }
        return node.IsEnd;
    }

    public bool StartsWith(string prefix)
    {
        TrieNode node = root;
        foreach (char c in prefix)
        {
            int idx = c - 'a';
            if (node.Children[idx] == null) return false;
            node = node.Children[idx];
        }
        return true;
    }
}
```

---

### Bài A1 — Implement Trie (208) ⭐⭐ — Bản lề

**Đề:** Implement cấu trúc Trie với các thao tác:
- `insert(word)` — chèn từ.
- `search(word)` — tìm từ chính xác.
- `startsWith(prefix)` — tìm tiền tố.

**Ví dụ:**
```
Insert "apple"
Search "apple"   → true
Search "app"     → false (chưa insert "app")
StartsWith "app" → true
Insert "app"
Search "app"     → true

Search ""        → edge case: tùy định nghĩa, thường false
StartsWith ""    → true (mọi từ đều có tiền tố rỗng)
```

**Constraint:** `1 ≤ word.length, prefix.length ≤ 2000`, chỉ gồm chữ thường.

**Phân tích 5 câu hỏi:**
1. Chuỗi chỉ chữ thường → bảng 26 phần tử là đủ.
2. Output: boolean cho search/startsWith.
3. Brute force: HashSet → O(1) search, O(n) startsWith (phải duyệt tất cả). Trie → O(L) cả hai.
4. Chỗ lãng phí: startsWith bằng HashSet phải duyệt mọi từ → Trie chia sẻ tiền tố chung.
5. Pattern: Trie — cây con trỏ 26 nhánh.

```csharp
// Dùng template A.2 ở trên — đã đầy đủ
// Implement trong class Trie (208):
public class Trie208
{
    private TrieNode _root = new TrieNode();

    public void Insert(string word)
    {
        var node = _root;
        foreach (char c in word)
        {
            int i = c - 'a';
            node.Children[i] ??= new TrieNode();
            node = node.Children[i];
        }
        node.IsEnd = true;
    }

    public bool Search(string word)
    {
        var node = Find(word);
        return node != null && node.IsEnd;
    }

    public bool StartsWith(string prefix)
        => Find(prefix) != null;

    private TrieNode Find(string s)
    {
        var node = _root;
        foreach (char c in s)
        {
            int i = c - 'a';
            if (node.Children[i] == null) return null;
            node = node.Children[i];
        }
        return node;
        // T: O(L), S: O(1) mỗi thao tác
    }
}
```

---

### Bài A2 — Design Add and Search Words (211) ⭐⭐⭐

**Đề:** Implement cấu trúc hỗ trợ:
- `addWord(word)` — thêm từ.
- `search(word)` — tìm từ, ký tự `.` khớp với bất kỳ chữ cái nào.

**Ví dụ:**
```
addWord("bad"), addWord("dad"), addWord("mad")
search("pad") → false
search("bad") → true
search(".ad") → true  (b/d/m + ad)
search("b..") → true  (b + any + any)

search("...") → true  nếu có từ dài 3
search("")    → edge case: false
```

**Constraint:** `1 ≤ word.length ≤ 25`, chỉ chữ thường và `.`.

```csharp
public class WordDictionary
{
    private TrieNode _root = new TrieNode();

    public void AddWord(string word)
    {
        var node = _root;
        foreach (char c in word)
        {
            int i = c - 'a';
            node.Children[i] ??= new TrieNode();
            node = node.Children[i];
        }
        node.IsEnd = true;
    }

    public bool Search(string word)
        => SearchFrom(_root, word, 0);

    private bool SearchFrom(TrieNode node, string word, int idx)
    {
        if (idx == word.Length) return node.IsEnd;
        char c = word[idx];

        if (c == '.')
        {
            // Thử tất cả 26 nhánh
            foreach (var child in node.Children)
                if (child != null && SearchFrom(child, word, idx + 1))
                    return true;
            return false;
        }
        else
        {
            int i = c - 'a';
            return node.Children[i] != null && SearchFrom(node.Children[i], word, idx + 1);
        }
        // T: O(26^L) worst case với toàn '.', thực tế O(L) cho từ thường
    }
}
```

---

### Bài A3 — Word Search II (212) ⭐⭐⭐⭐

**Đề:** Lưới ký tự `m × n`. Cho danh sách từ `words`. Tìm tất cả từ có thể tạo bằng cách đi từ ô kề nhau (4 hướng, không dùng lại ô).

**Ví dụ:**
```
Input:
board = [["o","a","a","n"],
         ["e","t","a","e"],
         ["i","h","k","r"],
         ["i","f","l","v"]]
words = ["oath","pea","eat","rain"]
Output: ["eat","oath"]

Input:  board = [["a"]], words = ["a"]
Output: ["a"]
Vì: edge case — lưới 1×1.

Input:  board = [["a","b"],["c","d"]], words = ["abdc","abcd"]
Output: ["abdc"]
Vì: "abcd" không thể đi liên tiếp kề nhau.
```

**Constraint:** `m, n ≤ 12`, `words.length ≤ 3 × 10⁴`, `word.length ≤ 10`.

```csharp
public IList<string> FindWords(char[][] board, string[] words)
{
    // Bước 1: Xây Trie từ danh sách từ
    var root = new TrieNode();
    foreach (string w in words)
    {
        var node = root;
        foreach (char c in w)
        {
            int i = c - 'a';
            node.Children[i] ??= new TrieNode();
            node = node.Children[i];
        }
        node.IsEnd = true; // lưu từ vào node cuối (có thể mở rộng lưu từ thật sự)
    }

    // Dùng TrieNode mở rộng lưu thêm từ
    // (Xem cách đơn giản: lưu từ dưới dạng chuỗi trong IsEnd flag hay field Word)

    var result = new List<string>();
    int m = board.Length, n = board[0].Length;
    var wordRoot = BuildTrie(words);

    for (int r = 0; r < m; r++)
        for (int c = 0; c < n; c++)
            DFS(board, r, c, wordRoot, result);

    return result;
}

// TrieNode mở rộng (có trường Word)
class TrieNodeEx
{
    public TrieNodeEx[] Children = new TrieNodeEx[26];
    public string Word = null; // null = chưa kết thúc từ
}

TrieNodeEx BuildTrie(string[] words)
{
    var root = new TrieNodeEx();
    foreach (string w in words)
    {
        var node = root;
        foreach (char c in w)
        {
            int i = c - 'a';
            node.Children[i] ??= new TrieNodeEx();
            node = node.Children[i];
        }
        node.Word = w;
    }
    return root;
}

void DFS(char[][] board, int r, int c, TrieNodeEx node, List<string> result)
{
    if (r < 0 || r >= board.Length || c < 0 || c >= board[0].Length) return;
    char ch = board[r][c];
    if (ch == '#' || node.Children[ch - 'a'] == null) return; // visited hoặc không tồn tại

    node = node.Children[ch - 'a'];
    if (node.Word != null)
    {
        result.Add(node.Word);
        node.Word = null; // tránh thêm trùng
    }

    board[r][c] = '#'; // đánh dấu đã dùng
    DFS(board, r + 1, c, node, result);
    DFS(board, r - 1, c, node, result);
    DFS(board, r, c + 1, node, result);
    DFS(board, r, c - 1, node, result);
    board[r][c] = ch;  // undo
    // T: O(m×n×4^L), S: O(W×L) cho Trie
}
```

---

## Phần B — Bit Manipulation

### B.1 Các phép toán bit cơ bản

```csharp
// AND (&): chỉ 1 khi cả hai đều 1
// OR  (|): 1 khi ít nhất một = 1
// XOR (^): 1 khi khác nhau (0^0=0, 1^1=0, 0^1=1)
// NOT (~): đảo bit
// LEFT SHIFT  (<<): nhân 2^k
// RIGHT SHIFT (>>): chia 2^k (arithmetic, giữ dấu)
// RIGHT SHIFT (>>>): unsigned shift (C# dùng >> cho uint/int không âm)

// Thủ thuật quan trọng:
// Lấy bit thứ i:   (n >> i) & 1
// Bật bit thứ i:   n | (1 << i)
// Tắt bit thứ i:   n & ~(1 << i)
// Đảo bit thứ i:   n ^ (1 << i)
// Xóa bit thấp nhất đang bật: n & (n - 1)
// Tính x là lũy thừa 2:       x > 0 && (x & (x-1)) == 0
```

### B.2 Tính chất XOR

```
x ^ 0 = x       (XOR với 0 không đổi)
x ^ x = 0       (XOR với chính nó = 0)
x ^ y ^ x = y   (XOR có thể nghịch đảo)
```

Ứng dụng: tìm số xuất hiện lẻ lần, tìm số thiếu, hoán đổi không cần biến tạm.

---

### Bài B1 — Single Number (136) ⭐ — Bản lề

**Đề:** Mảng số nguyên, mọi phần tử xuất hiện **đúng 2 lần** ngoại trừ **một phần tử xuất hiện 1 lần**. Tìm phần tử đó. Yêu cầu O(n) time, O(1) space.

**Ví dụ:**
```
Input:  nums = [2,2,1]
Output: 1

Input:  nums = [4,1,2,1,2]
Output: 4

Input:  nums = [1]
Output: 1
Vì: edge case — một phần tử, chính nó.
```

**Constraint:** `1 ≤ n ≤ 3×10⁴`, mỗi phần tử trong `[-3×10⁴, 3×10⁴]`.

**Phân tích 5 câu hỏi:**
1. Mảng số nguyên, có trùng lặp (mỗi cái 2 lần trừ 1).
2. Tìm số xuất hiện 1 lần — số nguyên.
3. Brute force: hash map đếm tần suất → O(n) time, O(n) space.
4. Chỗ lãng phí: hash map tốn O(n) bộ nhớ. XOR tất cả phần tử → cặp triệt tiêu nhau, còn lại phần tử lẻ.
5. Pattern: **XOR trick**.

```csharp
public int SingleNumber(int[] nums)
{
    int result = 0;
    foreach (int n in nums)
        result ^= n;
    return result;
    // T: O(n), S: O(1)
    // Vì: mọi số xuất hiện 2 lần → XOR với chính nó = 0 → chỉ còn số lẻ
}
```

---

### Bài B2 — Number of 1 Bits (191) ⭐

**Đề:** Cho số nguyên không dấu `n`. Đếm số bit `1` (còn gọi là Hamming weight).

**Ví dụ:**
```
Input:  n = 11 (binary: 1011)
Output: 3

Input:  n = 128 (binary: 10000000)
Output: 1

Input:  n = 2147483645 (binary: 01111111111111111111111111111101)
Output: 30
Vì: edge case — số lớn.
```

**Constraint:** `0 ≤ n ≤ 2³¹ - 1`.

```csharp
public int HammingWeight(uint n)
{
    int count = 0;
    while (n != 0)
    {
        n &= (n - 1); // xóa bit 1 thấp nhất
        count++;
    }
    return count;
    // T: O(k) với k = số bit 1, S: O(1)
    // Tốt hơn O(32) vì chỉ lặp số lần bằng số bit 1
}
```

---

### Bài B3 — Counting Bits (338) ⭐

**Đề:** Cho số nguyên `n`. Trả về mảng `result[0..n]` trong đó `result[i]` = số bit `1` trong biểu diễn nhị phân của `i`.

**Ví dụ:**
```
Input:  n = 2
Output: [0,1,1]
Vì: 0→0b0, 1→0b1, 2→0b10.

Input:  n = 5
Output: [0,1,1,2,1,2]

Input:  n = 0
Output: [0]
Vì: edge case.
```

**Constraint:** `0 ≤ n ≤ 10⁵`.

```csharp
public int[] CountBits(int n)
{
    int[] dp = new int[n + 1];
    // dp[i] = dp[i >> 1] + (i & 1)
    // Tức là: số bit 1 của i = số bit 1 của (i/2) + bit cuối của i
    for (int i = 1; i <= n; i++)
        dp[i] = dp[i >> 1] + (i & 1);
    return dp;
    // T: O(n), S: O(n)
}
```

---

### Bài B4 — Sum of Two Integers (371) ⭐⭐

**Đề:** Tính `a + b` mà **không dùng toán tử `+` hoặc `-`**.

**Ví dụ:**
```
Input:  a = 1, b = 2
Output: 3

Input:  a = 2, b = 3
Output: 5

Input:  a = -2, b = 3
Output: 1
Vì: edge case — số âm (bù 2).
```

**Constraint:** `-1000 ≤ a, b ≤ 1000`.

```csharp
public int GetSum(int a, int b)
{
    while (b != 0)
    {
        int carry = (a & b) << 1;  // bit nhớ (carry) = AND rồi shift trái 1
        a = a ^ b;                  // tổng không nhớ = XOR
        b = carry;                  // lặp cho đến khi không còn nhớ
    }
    return a;
    // T: O(log(max)), S: O(1)
    // XOR cộng từng bit không nhớ; AND + shift tạo carry; lặp đến carry = 0
}
```

---

## Phần C — Monotonic Queue (Deque)

### C.1 Monotonic Queue là gì?

Deque duy trì tính đơn điệu (tăng hoặc giảm) giúp tìm **max/min của cửa sổ trượt trong O(n)**.

**Ý tưởng:** Deque lưu **chỉ số**. Trước khi thêm phần tử mới, loại bỏ các phần tử từ cuối deque mà nhỏ hơn (hoặc lớn hơn) phần tử hiện tại — vì chúng không bao giờ có thể là max/min trong tương lai.

### C.2 Template Sliding Window Maximum

```csharp
// Deque giảm dần → đầu deque luôn là MAX trong cửa sổ
var deque = new LinkedList<int>(); // lưu index

for (int i = 0; i < nums.Length; i++)
{
    // Loại phần tử ra khỏi cửa sổ (index quá cũ)
    while (deque.Count > 0 && deque.First.Value < i - k + 1)
        deque.RemoveFirst();

    // Loại phần tử nhỏ hơn nums[i] từ cuối (không bao giờ là max)
    while (deque.Count > 0 && nums[deque.Last.Value] < nums[i])
        deque.RemoveLast();

    deque.AddLast(i);

    if (i >= k - 1)
        result[i - k + 1] = nums[deque.First.Value]; // max = đầu deque
}
```

---

### Bài C1 — Sliding Window Maximum (239) ⭐⭐⭐⭐ — Bản lề

**Đề:** Cho mảng `nums` và cửa sổ kích thước `k`. Trả về mảng chứa **giá trị lớn nhất** trong mỗi cửa sổ.

**Ví dụ:**
```
Input:  nums = [1,3,-1,-3,5,3,6,7], k = 3
Output: [3,3,5,5,6,7]
Vì:
  [1,3,-1] → 3
  [3,-1,-3] → 3
  [-1,-3,5] → 5
  [-3,5,3] → 5
  [5,3,6] → 6
  [3,6,7] → 7

Input:  nums = [1], k = 1
Output: [1]
Vì: edge case — cửa sổ = toàn mảng.

Input:  nums = [1,-1], k = 1
Output: [1,-1]
Vì: k=1, max mỗi cửa sổ chính là phần tử đó.
```

**Constraint:** `1 ≤ n ≤ 10⁵`, `-10⁴ ≤ nums[i] ≤ 10⁴`, `1 ≤ k ≤ n`.

**Phân tích 5 câu hỏi:**
1. Mảng số nguyên, cửa sổ kích thước k.
2. Mảng n-k+1 phần tử — giá trị max mỗi cửa sổ.
3. Brute force: mỗi cửa sổ quét k phần tử → O(n×k).
4. Chỗ lãng phí: phần tử nhỏ hơn phần tử mới thêm vào sẽ không bao giờ là max → loại bỏ ngay.
5. Pattern: **Monotonic Deque giảm dần**.

```csharp
public int[] MaxSlidingWindow(int[] nums, int k)
{
    int n = nums.Length;
    int[] result = new int[n - k + 1];
    var deque = new LinkedList<int>(); // lưu index, deque giảm theo giá trị

    for (int i = 0; i < n; i++)
    {
        // Loại index đã ra khỏi cửa sổ
        while (deque.Count > 0 && deque.First.Value <= i - k)
            deque.RemoveFirst();

        // Loại các phần tử nhỏ hơn nums[i] từ cuối (vô dụng)
        while (deque.Count > 0 && nums[deque.Last.Value] < nums[i])
            deque.RemoveLast();

        deque.AddLast(i);

        if (i >= k - 1)
            result[i - k + 1] = nums[deque.First.Value]; // max = đầu deque
    }
    return result;
    // T: O(n) — mỗi phần tử vào/ra deque đúng 1 lần
    // S: O(k) — deque tối đa k phần tử
}
```

> **Tại sao O(n)?** Mỗi phần tử được thêm vào deque **đúng 1 lần** và xóa khỏi deque **tối đa 1 lần** → tổng thao tác deque = O(2n) = O(n).

---

## Phần D — Segment Tree & Fenwick Tree (BIT)

### D.1 Khi nào cần Segment Tree / Fenwick?

| Bài toán | Cấu trúc phù hợp |
|---|---|
| Tổng khoảng, không có cập nhật | Prefix Sum — O(1) query |
| Tổng khoảng + cập nhật điểm | Fenwick Tree (BIT) — O(log n) cả hai |
| Min/max khoảng + cập nhật | Segment Tree — O(log n) cả hai |
| Cập nhật khoảng (range update) | Segment Tree + lazy propagation |

### D.2 Fenwick Tree (BIT) Template

```csharp
class BIT
{
    private int[] tree;
    private int n;

    public BIT(int n)
    {
        this.n = n;
        tree = new int[n + 1];
    }

    // Cập nhật vị trí i thêm delta
    public void Update(int i, int delta)
    {
        for (; i <= n; i += i & (-i)) // i & (-i) = bit thấp nhất đang bật
            tree[i] += delta;
    }

    // Tổng prefix [1..i]
    public int Query(int i)
    {
        int sum = 0;
        for (; i > 0; i -= i & (-i))
            sum += tree[i];
        return sum;
    }

    // Tổng khoảng [l..r]
    public int QueryRange(int l, int r)
        => Query(r) - Query(l - 1);
}
```

### D.3 Segment Tree Template (min/max/sum)

```csharp
class SegmentTree
{
    private int[] tree;
    private int n;

    public SegmentTree(int[] nums)
    {
        n = nums.Length;
        tree = new int[4 * n];
        Build(nums, 1, 0, n - 1);
    }

    private void Build(int[] nums, int node, int lo, int hi)
    {
        if (lo == hi) { tree[node] = nums[lo]; return; }
        int mid = (lo + hi) / 2;
        Build(nums, 2 * node, lo, mid);
        Build(nums, 2 * node + 1, mid + 1, hi);
        tree[node] = tree[2 * node] + tree[2 * node + 1]; // tổng (thay bằng Math.Min/Max cho min/max)
    }

    public void Update(int node, int lo, int hi, int idx, int val)
    {
        if (lo == hi) { tree[node] = val; return; }
        int mid = (lo + hi) / 2;
        if (idx <= mid) Update(2 * node, lo, mid, idx, val);
        else            Update(2 * node + 1, mid + 1, hi, idx, val);
        tree[node] = tree[2 * node] + tree[2 * node + 1];
    }

    public int Query(int node, int lo, int hi, int l, int r)
    {
        if (r < lo || hi < l) return 0;             // ngoài khoảng (trả 0 hoặc int.MaxValue cho min)
        if (l <= lo && hi <= r) return tree[node];  // trong khoảng hoàn toàn
        int mid = (lo + hi) / 2;
        return Query(2 * node, lo, mid, l, r)
             + Query(2 * node + 1, mid + 1, hi, l, r);
    }

    // Gọi ngoài: Update(1, 0, n-1, idx, val), Query(1, 0, n-1, l, r)
}
```

---

### Bài D1 — Range Sum Query – Mutable (307) ⭐⭐⭐ — Bản lề

**Đề:** Cho mảng `nums`. Hỗ trợ:
- `update(index, val)` — cập nhật `nums[index] = val`.
- `sumRange(left, right)` — tổng `nums[left..right]`.

**Ví dụ:**
```
nums = [1,3,5]
sumRange(0,2) → 9
update(1, 2)  → nums = [1,2,5]
sumRange(0,2) → 8

nums = [0]
update(0, 1) → [1]
sumRange(0,0) → 1
Vì: edge case — mảng 1 phần tử.
```

**Constraint:** `1 ≤ n ≤ 3×10⁴`, số thao tác ≤ 3×10⁴.

**Phân tích 5 câu hỏi:**
1. Mảng số, có cập nhật động.
2. Tổng khoảng — số nguyên.
3. Brute force: mảng thường → update O(1), query O(n).
4. Prefix sum tĩnh → update O(n), query O(1). Cần cân bằng cả hai → Fenwick/Segment Tree.
5. Pattern: **Fenwick Tree (BIT)** — cả update và query đều O(log n).

```csharp
public class NumArray
{
    private int[] _nums;
    private BIT _bit;

    public NumArray(int[] nums)
    {
        _nums = nums.ToArray();
        _bit = new BIT(nums.Length);
        for (int i = 0; i < nums.Length; i++)
            _bit.Update(i + 1, nums[i]); // BIT dùng index 1-based
    }

    public void Update(int index, int val)
    {
        _bit.Update(index + 1, val - _nums[index]); // thêm delta
        _nums[index] = val;
    }

    public int SumRange(int left, int right)
        => _bit.QueryRange(left + 1, right + 1);
    // T: O(log n) mỗi thao tác, S: O(n)
}
```

---

### Bài D2 — Count of Smaller Numbers After Self (315) ⭐⭐⭐⭐

**Đề:** Cho mảng `nums`. Với mỗi `nums[i]`, đếm số phần tử **nhỏ hơn** `nums[i]` nằm **bên phải** `i`.

**Ví dụ:**
```
Input:  nums = [5,2,6,1]
Output: [2,1,1,0]
Vì: 5→{2,1}=2; 2→{1}=1; 6→{1}=1; 1→{}=0.

Input:  nums = [-1]
Output: [0]
Vì: edge case — một phần tử.

Input:  nums = [-1,-1]
Output: [0,0]
Vì: phần tử bằng nhau không đếm.
```

**Constraint:** `1 ≤ n ≤ 10⁵`, `-10⁴ ≤ nums[i] ≤ 10⁴`.

```csharp
public IList<int> CountSmaller(int[] nums)
{
    // Rời rạc hóa giá trị về [1..range]
    int offset = 10001; // để xử lý số âm
    int size = 20002;
    int[] bit = new int[size + 1];
    int n = nums.Length;
    int[] result = new int[n];

    // Duyệt từ phải sang trái
    for (int i = n - 1; i >= 0; i--)
    {
        int val = nums[i] + offset; // [1..20001]
        // Đếm các số đã thấy (bên phải) nhỏ hơn nums[i] → query [1..val-1]
        result[i] = QueryBIT(bit, val - 1);
        // Cập nhật: đánh dấu nums[i] đã xuất hiện
        UpdateBIT(bit, val, 1, size);
    }
    return result;
    // T: O(n log V), S: O(V) với V = miền giá trị
}

void UpdateBIT(int[] bit, int i, int delta, int n)
{
    for (; i <= n; i += i & (-i))
        bit[i] += delta;
}

int QueryBIT(int[] bit, int i)
{
    int sum = 0;
    for (; i > 0; i -= i & (-i))
        sum += bit[i];
    return sum;
}
```

---

## Phần E — Advanced DP

### E.1 Bitmask DP

Dùng khi `n ≤ 20` và trạng thái là **tập con** của n phần tử. Mỗi tập con biểu diễn bằng bitmask (số nguyên). Số trạng thái = 2ⁿ.

```
dp[mask] = kết quả tối ưu khi đã thực hiện tập con các bước trong mask
```

### Bài E1 — Partition to K Equal Sum Subsets (698) ⭐⭐⭐⭐

**Đề:** Cho mảng `nums` và số `k`. Hỏi có thể chia `nums` thành `k` tập con có **tổng bằng nhau** không?

**Ví dụ:**
```
Input:  nums = [4,3,2,3,5,2,1], k = 4
Output: true
Vì: [5],[4,1],[3,2],[3,2].

Input:  nums = [1,2,3,4], k = 3
Output: false

Input:  nums = [1,1], k = 2
Output: true
Vì: edge case — chia thành [1],[1].
```

**Constraint:** `1 ≤ k ≤ n ≤ 16`, `0 < nums[i] < 10⁴`.

**Phân tích 5 câu hỏi:**
1. Mảng số dương, n ≤ 16 (gợi ý bitmask).
2. Boolean — có chia được không.
3. Brute force: backtracking thử mọi cách gán → O(k^n).
4. n ≤ 16 → 2¹⁶ = 65536 trạng thái bitmask → DP bitmask O(2ⁿ × n).
5. Pattern: **Bitmask DP — dp[mask] = tổng hiện tại trong nhóm đang xây**.

```csharp
public bool CanPartitionKSubsets(int[] nums, int k)
{
    int total = nums.Sum();
    if (total % k != 0) return false;
    int target = total / k;
    if (nums.Any(x => x > target)) return false;

    int n = nums.Length;
    int[] dp = new int[1 << n]; // dp[mask] = tổng phần tử đã gán trong nhóm hiện tại
    Array.Fill(dp, -1);
    dp[0] = 0;

    for (int mask = 0; mask < (1 << n); mask++)
    {
        if (dp[mask] == -1) continue; // trạng thái không hợp lệ
        for (int i = 0; i < n; i++)
        {
            if ((mask & (1 << i)) != 0) continue; // nums[i] đã dùng
            int next = mask | (1 << i);
            if (dp[next] != -1) continue;          // đã tính

            int newSum = (dp[mask] + nums[i]) % target;
            // Nếu (dp[mask] + nums[i]) <= target, chuyển sang next
            if (dp[mask] + nums[i] <= target)
                dp[next] = newSum;
        }
    }
    return dp[(1 << n) - 1] == 0; // tất cả phần tử đã dùng và nhóm cuối vừa đúng target
    // T: O(2ⁿ × n), S: O(2ⁿ)
}
```

> **Cách đọc dp[mask]:** Tổng hiện tại **trong nhóm đang xây** (đã mod target). Khi `dp[mask] + nums[i]` vượt `target` → không thêm vào nhóm này; khi đúng `target` → nhóm hoàn thành, `newSum = 0` → bắt đầu nhóm mới.

---

## Tổng kết Pattern — Phase 15

| Công cụ | Dấu hiệu nhận biết | Độ phức tạp |
|---|---|---|
| **Trie** | Nhiều từ, tìm theo tiền tố, autocomplete | O(L) mỗi thao tác |
| **XOR** | Số xuất hiện lẻ lần, thiếu số, hoán đổi | O(n), O(1) bộ nhớ |
| **Bitmask** | n ≤ 20, trạng thái là tập con | O(2ⁿ × n) |
| **Mono Deque** | Max/min cửa sổ trượt kích thước k | O(n) tổng |
| **Fenwick/BIT** | Cập nhật điểm + tổng khoảng | O(log n) mỗi thao tác |
| **Segment Tree** | Cập nhật điểm/khoảng + min/max/sum khoảng | O(log n) mỗi thao tác |
| **Bitmask DP** | n ≤ 16, dp trên tập con | O(2ⁿ × n) |

---

## Tiêu chí hoàn thành Phase 15

- [ ] Dựng được Trie từ trí nhớ và giải Word Search II.
- [ ] Dùng XOR/bitmask cho ít nhất 3 bài.
- [ ] Viết được Monotonic Deque cho Sliding Window Maximum và giải thích tại sao O(n).
- [ ] Hiểu khi nào dùng Fenwick Tree vs Segment Tree vs Prefix Sum (tĩnh).
- [ ] Nhận diện bài Bitmask DP qua dấu hiệu `n ≤ 16` + "tập con".
