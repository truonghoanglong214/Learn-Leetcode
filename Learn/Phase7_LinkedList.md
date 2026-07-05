# Phase 7 — Linked List

> **Ngôn ngữ:** C#
> **Mục tiêu:** Thành thạo **thao tác con trỏ** trên danh sách liên kết: đảo, dò chu trình, tìm giữa, trộn — tất cả O(1) bộ nhớ khi có thể. Kỹ năng cốt lõi: vẽ sơ đồ, thao tác trước khi code.

---

## Bài học 1 — Các Kỹ Thuật Nền

### 1.1 Định nghĩa node (dùng xuyên suốt)

```csharp
public class ListNode
{
    public int val;
    public ListNode next;
    public ListNode(int val = 0, ListNode next = null)
    {
        this.val = val;
        this.next = next;
    }
}
```

### 1.2 Dummy Head

```
Khi nào dùng: thao tác có thể ảnh hưởng đến đầu danh sách (xóa node đầu, trộn).
Lợi ích: xử lý thống nhất, không cần case đặc biệt cho head.

ListNode dummy = new ListNode(0);
dummy.next = head;
// ... thao tác trên dummy.next ...
return dummy.next; // head mới
```

### 1.3 Fast & Slow Pointers (Floyd)

```
Dùng để:
  1. Tìm node giữa: fast đi 2 bước, slow đi 1 bước → khi fast hết, slow ở giữa.
  2. Phát hiện chu trình: nếu có chu trình, fast và slow sẽ gặp nhau.
  3. Tìm điểm bắt đầu chu trình: sau khi gặp nhau, đặt một con trỏ về head,
     cả hai đi 1 bước mỗi lần → gặp nhau tại điểm bắt đầu chu trình.
  4. Node thứ k từ cuối: fast đi k bước trước, rồi cả hai cùng tiến → slow đến đích.
```

### 1.4 Đảo Danh Sách — Template In-Place

```csharp
// Đảo toàn bộ — lặp
ListNode Reverse(ListNode head)
{
    ListNode prev = null;
    ListNode curr = head;
    while (curr != null)
    {
        ListNode next = curr.next; // lưu trước khi đứt liên kết!
        curr.next = prev;
        prev = curr;
        curr = next;
    }
    return prev; // prev là head mới
}
```

---

## Bài tập theo thứ tự

### Easy

---

#### Bài 1: Reverse Linked List (LeetCode 206) ⭐ Bài bản lề

**Đề:** Cho `head` của danh sách liên kết đơn. Đảo danh sách, trả về head mới.

**Ví dụ:**
```
Input:  1 → 2 → 3 → 4 → 5 → null
Output: 5 → 4 → 3 → 2 → 1 → null

Input:  1 → 2 → null
Output: 2 → 1 → null

Input:  null
Output: null
(Edge case: danh sách rỗng)

Input:  1 → null
Output: 1 → null
(Edge case: một phần tử)
```

**Constraint:** `0 <= n <= 5000`, `-5000 <= val <= 5000`.

**Phân tích 5 câu hỏi:**
1. Input: linked list, n ≤ 5000
2. Output: head của danh sách đảo ngược
3. Brute: đổ ra mảng, đảo mảng, dựng lại list → O(n) time, O(n) space
4. Lãng phí: tốn thêm O(n) bộ nhớ — có thể đảo tại chỗ bằng cách chỉnh lại con trỏ
5. Pattern: **In-place reversal** — duyệt một lần, đổi hướng từng con trỏ next

```csharp
// Brute — O(n) time, O(n) space
public ListNode ReverseBrute(ListNode head)
{
    var vals = new List<int>();
    for (var curr = head; curr != null; curr = curr.next)
        vals.Add(curr.val);

    var curr2 = head;
    for (int i = vals.Count - 1; i >= 0; i--)
    {
        curr2.val = vals[i];
        curr2 = curr2.next;
    }
    return head;
}

// Optimal Lặp — O(n) time, O(1) space
public ListNode ReverseList(ListNode head)
{
    ListNode prev = null;
    ListNode curr = head;

    while (curr != null)
    {
        ListNode next = curr.next; // 1. lưu node kế trước khi đứt liên kết
        curr.next = prev;          // 2. đổi hướng con trỏ
        prev = curr;               // 3. tiến prev
        curr = next;               // 4. tiến curr
    }
    return prev; // prev là node cuối cùng (head mới)
}
// Time: O(n), Space: O(1)

// Optimal Đệ quy — O(n) time, O(n) space (stack đệ quy)
public ListNode ReverseListRecursive(ListNode head)
{
    // Base case: rỗng hoặc một node
    if (head == null || head.next == null) return head;

    // "Tin" đệ quy đảo được phần tail, newHead là head mới
    ListNode newHead = ReverseListRecursive(head.next);

    // head.next hiện đang là đuôi của phần đã đảo → làm nó trỏ về head
    head.next.next = head;
    head.next = null;

    return newHead;
}
// Time: O(n), Space: O(n) — stack đệ quy
```

**Sơ đồ lặp (3 bước mỗi vòng):**
```
Ban đầu: prev=null, curr=1→2→3

Vòng 1: next=2, 1→null, prev=1, curr=2
Vòng 2: next=3, 2→1→null, prev=2, curr=3
Vòng 3: next=null, 3→2→1→null, prev=3, curr=null → kết thúc
Trả về prev = 3 (head mới)
```

---

#### Bài 2: Merge Two Sorted Lists (LeetCode 21)

**Đề:** Cho `head1` và `head2` là hai danh sách đã sắp xếp tăng dần. Trộn thành một danh sách sắp xếp, trả về head. Trộn **tại chỗ** (không tạo node mới).

**Ví dụ:**
```
Input:  1→2→4,  1→3→4
Output: 1→1→2→3→4→4

Input:  null, null
Output: null
(Edge case: cả hai rỗng)

Input:  null, 0→null
Output: 0→null
(Edge case: một danh sách rỗng)

Input:  1→3→5, 2→4→6
Output: 1→2→3→4→5→6
```

**Constraint:** `0 <= n, m <= 50`, `-100 <= val <= 100`, cả hai đã sắp xếp.

**Phân tích 5 câu hỏi:**
1. Input: hai linked list đã sắp xếp
2. Output: một linked list đã sắp xếp (tại chỗ)
3. Brute: đổ cả hai ra mảng, sort, dựng lại → O((n+m) log(n+m)) space O(n+m)
4. Lãng phí: tốn bộ nhớ và sort không cần thiết khi đã có thứ tự
5. Pattern: **Dummy head + trộn hai con trỏ** — chọn node nhỏ hơn mỗi bước

```csharp
// Brute — O((n+m) log(n+m)) sort
public ListNode MergeTwoListsBrute(ListNode list1, ListNode list2)
{
    var vals = new List<int>();
    for (var c = list1; c != null; c = c.next) vals.Add(c.val);
    for (var c = list2; c != null; c = c.next) vals.Add(c.val);
    vals.Sort();

    ListNode dummy = new ListNode(0);
    var curr = dummy;
    foreach (int v in vals) { curr.next = new ListNode(v); curr = curr.next; }
    return dummy.next;
}

// Optimal — Dummy head + hai con trỏ, O(n+m) time, O(1) space
public ListNode MergeTwoLists(ListNode list1, ListNode list2)
{
    ListNode dummy = new ListNode(0); // node giả để tránh xử lý head đặc biệt
    ListNode curr = dummy;

    while (list1 != null && list2 != null)
    {
        if (list1.val <= list2.val)
        {
            curr.next = list1;
            list1 = list1.next;
        }
        else
        {
            curr.next = list2;
            list2 = list2.next;
        }
        curr = curr.next;
    }
    // Nối phần còn lại (một trong hai đã hết)
    curr.next = list1 ?? list2;

    return dummy.next;
}
// Time: O(n+m), Space: O(1)
```

---

#### Bài 3: Linked List Cycle (LeetCode 141)

**Đề:** Cho `head`. Danh sách có chu trình không? (Có node `next` trỏ ngược về node trước đó trong list.)

**Ví dụ:**
```
Input:  3→2→0→-4, và node -4.next = node 2 (pos=1)
Output: true
Vì: có chu trình 2→0→-4→2→...

Input:  1→2, và node 2.next = node 1 (pos=0)
Output: true

Input:  1→null
Output: false
(Edge case: một node, không chu trình)

Input:  null
Output: false
(Edge case: rỗng)
```

**Constraint:** `0 <= n <= 10^4`, `-10^5 <= val <= 10^5`.

**Phân tích 5 câu hỏi:**
1. Input: linked list (có thể có chu trình)
2. Output: bool — có chu trình hay không
3. Brute: dùng HashSet lưu các node đã thấy → O(n) space
4. Lãng phí: tốn O(n) bộ nhớ — Floyd dùng O(1)
5. Pattern: **Fast & Slow (Floyd)** — nếu có chu trình, hai con trỏ sẽ gặp nhau

```csharp
// Brute — HashSet, O(n) time, O(n) space
public bool HasCycleBrute(ListNode head)
{
    var seen = new HashSet<ListNode>();
    for (var curr = head; curr != null; curr = curr.next)
    {
        if (seen.Contains(curr)) return true;
        seen.Add(curr);
    }
    return false;
}

// Optimal — Floyd's Algorithm, O(n) time, O(1) space
public bool HasCycle(ListNode head)
{
    ListNode slow = head;
    ListNode fast = head;

    while (fast != null && fast.next != null)
    {
        slow = slow.next;       // 1 bước
        fast = fast.next.next;  // 2 bước

        if (slow == fast) return true; // gặp nhau trong chu trình
    }
    return false; // fast chạm null → không có chu trình
}
// Time: O(n), Space: O(1)
```

**Tại sao Floyd đúng?** Nếu có chu trình, fast "đuổi" slow trong vòng lặp. Mỗi vòng fast tiến thêm 1 bước so với slow (trong chu trình), sau tối đa `L` bước (độ dài chu trình) chúng gặp nhau. Nếu không có chu trình, fast chạm null.

---

### Medium

---

#### Bài 4: Remove Nth Node From End of List (LeetCode 19)

**Đề:** Cho `head` và `n`. Xóa node thứ `n` tính từ **cuối** danh sách. Trả về head. Làm trong một lần duyệt.

**Ví dụ:**
```
Input:  1→2→3→4→5, n = 2
Output: 1→2→3→5
Vì: node thứ 2 từ cuối là 4 → xóa 4.

Input:  1, n = 1
Output: null
(Edge case: danh sách 1 node, xóa node duy nhất)

Input:  1→2, n = 1
Output: 1
(Edge case: xóa node cuối)

Input:  1→2, n = 2
Output: 2
(Edge case: xóa node đầu)
```

**Constraint:** `1 <= n <= sz`, `1 <= sz <= 30`, `-100 <= val <= 100`.

**Phân tích 5 câu hỏi:**
1. Input: linked list + n
2. Output: head sau khi xóa
3. Brute: duyệt để tính độ dài L, xóa node thứ (L−n) từ đầu → 2 lần duyệt
4. Lãng phí: duyệt hai lần — có thể làm một lần bằng hai con trỏ cách nhau n bước
5. Pattern: **Two pointers cách khoảng cố định** — fast đi trước n bước, rồi cả hai cùng tiến; khi fast hết, slow ở đúng node cần xóa

```csharp
// Brute — 2 lần duyệt, O(n) time, O(1) space
public ListNode RemoveNthFromEndBrute(ListNode head, int n)
{
    int len = 0;
    for (var curr = head; curr != null; curr = curr.next) len++;

    ListNode dummy = new ListNode(0, head);
    ListNode curr2 = dummy;
    for (int i = 0; i < len - n; i++) curr2 = curr2.next;

    curr2.next = curr2.next.next; // xóa
    return dummy.next;
}

// Optimal — 1 lần duyệt, Two Pointers, O(n) time, O(1) space
public ListNode RemoveNthFromEnd(ListNode head, int n)
{
    ListNode dummy = new ListNode(0, head);
    ListNode fast = dummy;
    ListNode slow = dummy;

    // fast đi trước n+1 bước (để slow dừng ở node TRƯỚC node cần xóa)
    for (int i = 0; i <= n; i++)
        fast = fast.next;

    // Cùng tiến cho đến khi fast == null
    while (fast != null)
    {
        fast = fast.next;
        slow = slow.next;
    }

    slow.next = slow.next.next; // xóa node kế tiếp slow
    return dummy.next;
}
// Time: O(n), Space: O(1)
```

**Sơ đồ (n=2, list=[1,2,3,4,5]):**
```
Sau khi fast đi 3 bước: fast=3, slow=dummy(0)
Tiến cùng nhau: fast=4, slow=1 → fast=5, slow=2 → fast=null, slow=3
slow.next (=4) bị xóa → 1→2→3→5 ✓
```

---

#### Bài 5: Reorder List (LeetCode 143)

**Đề:** Cho `head` của list `L0→L1→…→Ln`. Sắp xếp lại thành `L0→Ln→L1→Ln-1→L2→Ln-2→…`. **Thực hiện tại chỗ**.

**Ví dụ:**
```
Input:  1→2→3→4
Output: 1→4→2→3

Input:  1→2→3→4→5
Output: 1→5→2→4→3

Input:  1
Output: 1
(Edge case: một node)

Input:  1→2
Output: 1→2
(Edge case: hai node, không thay đổi)
```

**Constraint:** `1 <= n <= 5×10^4`, `1 <= val <= 1000`.

**Phân tích 5 câu hỏi:**
1. Input: linked list, n ≤ 5×10⁴
2. Output: in-place (không trả về, sửa trực tiếp)
3. Brute: đổ ra mảng, dùng hai con trỏ trái/phải → O(n) space
4. Lãng phí: tốn O(n) — có thể làm tại chỗ bằng 3 bước
5. Pattern: **Ghép ba kỹ thuật** — (1) tìm giữa (fast/slow) → (2) đảo nửa sau → (3) trộn xen kẽ

```csharp
// Brute — O(n) space
public void ReorderListBrute(ListNode head)
{
    var nodes = new List<ListNode>();
    for (var c = head; c != null; c = c.next) nodes.Add(c);

    int lo = 0, hi = nodes.Count - 1;
    while (lo < hi)
    {
        nodes[lo].next = nodes[hi];
        lo++;
        if (lo == hi) break;
        nodes[hi].next = nodes[lo];
        hi--;
    }
    nodes[lo].next = null;
}

// Optimal — 3 bước tại chỗ, O(n) time, O(1) space
public void ReorderList(ListNode head)
{
    if (head?.next == null) return;

    // Bước 1: Tìm node giữa (fast/slow)
    ListNode slow = head, fast = head;
    while (fast.next != null && fast.next.next != null)
    {
        slow = slow.next;
        fast = fast.next.next;
    }
    // slow = node giữa (nửa trái kết thúc tại slow)

    // Bước 2: Đảo nửa sau (từ slow.next)
    ListNode secondHalf = slow.next;
    slow.next = null; // cắt đứt nửa trái
    secondHalf = ReverseList(secondHalf);

    // Bước 3: Trộn xen kẽ nửa trái và nửa phải (đã đảo)
    ListNode first = head, second = secondHalf;
    while (second != null)
    {
        ListNode tmp1 = first.next;
        ListNode tmp2 = second.next;
        first.next = second;
        second.next = tmp1;
        first = tmp1;
        second = tmp2;
    }
}

private ListNode ReverseList(ListNode head)
{
    ListNode prev = null, curr = head;
    while (curr != null)
    {
        ListNode next = curr.next;
        curr.next = prev;
        prev = curr;
        curr = next;
    }
    return prev;
}
// Time: O(n), Space: O(1)
```

---

#### Bài 6: Add Two Numbers (LeetCode 2)

**Đề:** Hai số không âm lưu trong linked list theo thứ tự **ngược** (chữ số hàng đơn vị ở đầu). Cộng hai số, trả về kết quả cũng dạng danh sách đảo ngược.

**Ví dụ:**
```
Input:  l1 = 2→4→3  (342),  l2 = 5→6→4  (465)
Output: 7→0→8  (807)
Vì: 342 + 465 = 807.

Input:  l1 = 0,  l2 = 0
Output: 0

Input:  l1 = 9→9→9→9→9→9→9,  l2 = 9→9→9→9
Output: 8→9→9→9→0→0→0→1
(Edge case: carry lan qua cả khi một số đã hết; kết quả dài hơn số dài nhất)
```

**Constraint:** `1 <= n, m <= 100`, `0 <= val <= 9`, không có số 0 đứng đầu trừ số 0.

**Phân tích 5 câu hỏi:**
1. Input: hai linked list, giá trị 0–9 mỗi node (ngược)
2. Output: linked list kết quả (ngược)
3. Brute: đọc cả hai số ra → cộng → chia thành chữ số → tốn bộ nhớ, tràn số nếu list dài
4. Lãng phí: tốn bộ nhớ và dễ tràn với n=100 chữ số
5. Pattern: **Mô phỏng cộng từng chữ số** — xử lý carry, đồng bộ hai list dù độ dài khác nhau

```csharp
// Optimal — Mô phỏng cộng, O(max(n,m)) time, O(max(n,m)) space
public ListNode AddTwoNumbers(ListNode l1, ListNode l2)
{
    ListNode dummy = new ListNode(0);
    ListNode curr = dummy;
    int carry = 0;

    while (l1 != null || l2 != null || carry != 0)
    {
        int sum = carry;
        if (l1 != null) { sum += l1.val; l1 = l1.next; }
        if (l2 != null) { sum += l2.val; l2 = l2.next; }

        carry = sum / 10;           // nhớ cho chữ số tiếp theo
        curr.next = new ListNode(sum % 10);
        curr = curr.next;
    }
    return dummy.next;
}
// Time: O(max(n,m)), Space: O(max(n,m)) — kết quả
```

**Edge case carry cuối:** Điều kiện vòng lặp là `l1 != null || l2 != null || carry != 0` — khi cả hai list hết nhưng còn carry (ví dụ 999+1=1000), vòng lặp tạo thêm node.

---

#### Bài 7: LRU Cache (LeetCode 146)

**Đề:** Thiết kế **LRU Cache** (Least Recently Used) với dung lượng `capacity`. Hỗ trợ:
- `Get(key)` → trả về value nếu tồn tại (và đánh dấu "recently used"), hoặc -1.
- `Put(key, value)` → chèn/cập nhật. Nếu đầy, xóa entry ít dùng nhất.
- Cả hai O(1) trung bình.

**Ví dụ:**
```
LRUCache cache = new LRUCache(2);
cache.Put(1, 1);   // cache: {1=1}
cache.Put(2, 2);   // cache: {1=1, 2=2}
cache.Get(1);      // → 1, cache: {2=2, 1=1} (1 vừa dùng → dùng gần nhất)
cache.Put(3, 3);   // đầy → xóa 2 (ít dùng nhất), cache: {1=1, 3=3}
cache.Get(2);      // → -1 (đã bị xóa)
cache.Put(4, 4);   // đầy → xóa 1 (ít dùng nhất), cache: {3=3, 4=4}
cache.Get(1);      // → -1
cache.Get(3);      // → 3
cache.Get(4);      // → 4
```

**Constraint:** `1 <= capacity <= 3000`, `0 <= key, value <= 10^4`, tối đa `2×10^5` thao tác.

**Phân tích 5 câu hỏi:**
1. Input: chuỗi Get/Put với capacity
2. Output: Get trả về value hoặc -1; Put không trả về gì
3. Brute: dùng danh sách + quét để tìm LRU → O(n) mỗi thao tác
4. Lãng phí: quét toàn bộ để tìm LRU — cần cấu trúc biết ngay "ít dùng nhất"
5. Pattern: **Doubly Linked List + HashMap** — list giữ thứ tự dùng (head=most recent, tail=LRU); hashmap cho O(1) lookup theo key

```csharp
// Optimal — Doubly Linked List + Dictionary, O(1) amortized
public class LRUCache
{
    private class Node
    {
        public int key, val;
        public Node prev, next;
        public Node(int k, int v) { key = k; val = v; }
    }

    private int capacity;
    private Dictionary<int, Node> map; // key → node
    private Node head, tail;            // sentinel nodes (dummy)

    public LRUCache(int capacity)
    {
        this.capacity = capacity;
        map = new Dictionary<int, Node>();

        // Sentinel head (most recent) và tail (least recent)
        head = new Node(0, 0);
        tail = new Node(0, 0);
        head.next = tail;
        tail.prev = head;
    }

    public int Get(int key)
    {
        if (!map.ContainsKey(key)) return -1;

        Node node = map[key];
        MoveToFront(node); // vừa dùng → chuyển lên đầu
        return node.val;
    }

    public void Put(int key, int value)
    {
        if (map.ContainsKey(key))
        {
            map[key].val = value;
            MoveToFront(map[key]);
        }
        else
        {
            if (map.Count == capacity)
            {
                // Xóa LRU (node ngay trước tail)
                Node lru = tail.prev;
                Remove(lru);
                map.Remove(lru.key);
            }
            Node newNode = new Node(key, value);
            AddToFront(newNode);
            map[key] = newNode;
        }
    }

    // Xóa node khỏi doubly linked list (O(1) vì có prev/next)
    private void Remove(Node node)
    {
        node.prev.next = node.next;
        node.next.prev = node.prev;
    }

    // Chèn node ngay sau head (most recently used)
    private void AddToFront(Node node)
    {
        node.next = head.next;
        node.prev = head;
        head.next.prev = node;
        head.next = node;
    }

    private void MoveToFront(Node node)
    {
        Remove(node);
        AddToFront(node);
    }
}
// Time: O(1) mọi thao tác, Space: O(capacity)
```

**Tại sao Doubly Linked List?** Cần `Remove(node)` O(1) → phải có `prev`. Nếu chỉ singly linked list thì remove O(n). Dummy head+tail loại bỏ hoàn toàn edge case khi list rỗng hoặc thao tác ở hai đầu.

---

### Hard

---

#### Bài 8: Merge k Sorted Lists (LeetCode 23)

**Đề:** Cho `ListNode[] lists` gồm k danh sách đã sắp xếp. Trộn thành một danh sách sắp xếp, trả về head.

**Ví dụ:**
```
Input:  lists = [[1,4,5], [1,3,4], [2,6]]
Output: 1→1→2→3→4→4→5→6

Input:  lists = []
Output: null
(Edge case: không có list nào)

Input:  lists = [[]]
Output: null
(Edge case: list rỗng duy nhất)

Input:  lists = [[1], [0]]
Output: 0→1
```

**Constraint:** `k == lists.length`, `0 <= k <= 10^4`, `0 <= n_i <= 500`, `-10^4 <= val <= 10^4`, tổng số node ≤ 10^4.

**Phân tích 5 câu hỏi:**
1. Input: k linked list đã sắp xếp
2. Output: một linked list sắp xếp
3. Brute: gộp tuần tự từng cặp → O(kN) với N = tổng số node
4. Lãng phí: không tận dụng tính sắp xếp của từng list — luôn cần phần tử nhỏ nhất trong số k đầu danh sách
5. Pattern: **Min-Heap** — heap kích thước k lưu node đầu mỗi list, mỗi lần pop phần tử nhỏ nhất rồi push node kế tiếp của list đó

```csharp
// Brute — gộp tuần tự từng cặp, O(kN)
public ListNode MergeKListsBrute(ListNode[] lists)
{
    ListNode result = null;
    foreach (var list in lists)
        result = MergeTwoSorted(result, list);
    return result;
}

// Optimal — Min-Heap, O(N log k) time, O(k) space
public ListNode MergeKLists(ListNode[] lists)
{
    // PriorityQueue<TElement, TPriority> (C# 6+)
    var pq = new PriorityQueue<ListNode, int>();

    // Nạp node đầu của mỗi list không rỗng
    foreach (var node in lists)
        if (node != null) pq.Enqueue(node, node.val);

    ListNode dummy = new ListNode(0);
    ListNode curr = dummy;

    while (pq.Count > 0)
    {
        ListNode node = pq.Dequeue(); // node nhỏ nhất trong k đầu
        curr.next = node;
        curr = curr.next;

        // Đẩy node kế tiếp của list đó vào heap
        if (node.next != null)
            pq.Enqueue(node.next, node.next.val);
    }
    return dummy.next;
}

// Hàm phụ cho brute
private ListNode MergeTwoSorted(ListNode a, ListNode b)
{
    ListNode dummy = new ListNode(0), curr = dummy;
    while (a != null && b != null)
    {
        if (a.val <= b.val) { curr.next = a; a = a.next; }
        else { curr.next = b; b = b.next; }
        curr = curr.next;
    }
    curr.next = a ?? b;
    return dummy.next;
}
// Time: O(N log k) — N thao tác heap, mỗi thao tác O(log k)
// Space: O(k) — heap
```

---

#### Bài 9: Reverse Nodes in k-Group (LeetCode 25)

**Đề:** Cho `head` và `k`. Đảo mỗi nhóm `k` node liên tiếp. Nếu số node còn lại < k, giữ nguyên. Không dùng thêm bộ nhớ ngoài O(1).

**Ví dụ:**
```
Input:  1→2→3→4→5, k = 2
Output: 2→1→4→3→5
Vì: [1,2] đảo thành [2,1]; [3,4] đảo thành [4,3]; [5] giữ nguyên.

Input:  1→2→3→4→5, k = 3
Output: 3→2→1→4→5
Vì: [1,2,3] đảo thành [3,2,1]; [4,5] < 3 node → giữ nguyên.

Input:  1→2→3→4, k = 4
Output: 4→3→2→1
(Edge case: k = n, đảo toàn bộ)

Input:  1, k = 1
Output: 1
(Edge case: k = 1, không thay đổi gì)
```

**Constraint:** `1 <= k <= n <= 5000`, `0 <= val <= 1000`.

```csharp
// Optimal — Thao tác con trỏ tại chỗ, O(n) time, O(1) space
public ListNode ReverseKGroup(ListNode head, int k)
{
    ListNode dummy = new ListNode(0, head);
    ListNode groupPrev = dummy; // node ngay trước nhóm hiện tại

    while (true)
    {
        // Tìm node thứ k trong nhóm hiện tại (bắt đầu từ groupPrev.next)
        ListNode kth = GetKth(groupPrev, k);
        if (kth == null) break; // còn lại < k node → dừng

        ListNode groupNext = kth.next; // node đầu nhóm tiếp theo

        // Đảo k node trong nhóm [groupPrev.next .. kth]
        ListNode prev = groupNext; // sau khi đảo, node cuối nhóm trỏ vào groupNext
        ListNode curr = groupPrev.next;
        while (curr != groupNext)
        {
            ListNode tmp = curr.next;
            curr.next = prev;
            prev = curr;
            curr = tmp;
        }
        // prev = node đầu nhóm sau khi đảo (= kth cũ)
        // groupPrev.next cũ = node cuối nhóm sau khi đảo

        ListNode tmp2 = groupPrev.next; // lưu lại node cuối nhóm (sẽ là groupPrev mới)
        groupPrev.next = prev;          // nối nhóm vừa đảo vào chuỗi
        groupPrev = tmp2;               // tiến groupPrev tới node cuối nhóm
    }
    return dummy.next;
}

// Tìm node thứ k từ node 'curr' (bỏ qua 'curr' bản thân)
private ListNode GetKth(ListNode curr, int k)
{
    while (curr != null && k > 0)
    {
        curr = curr.next;
        k--;
    }
    return curr;
}
// Time: O(n), Space: O(1)
```

**Sơ đồ (k=2, [1,2,3,4,5]):**
```
Vòng 1: groupPrev=dummy, kth=2, groupNext=3
  Đảo [1,2] với prev=3: 2→1→3→4→5
  groupPrev = node(1) (cuối nhóm)
Vòng 2: groupPrev=node(1), kth=4, groupNext=5
  Đảo [3,4] với prev=5: 4→3→5
  groupPrev = node(3)
Vòng 3: GetKth từ node(3) k=2 → trả về null → break
Kết quả: dummy→2→1→4→3→5 ✓
```

---

## Tổng kết Pattern

| Dấu hiệu đề | Pattern |
|-------------|---------|
| "Đảo danh sách" / "kiểm tra palindrome list" | In-place reversal — 3 biến: prev/curr/next |
| "Trộn hai danh sách sắp xếp" | Dummy head + hai con trỏ song song |
| "Có chu trình không?" | Fast & Slow (Floyd) |
| "Node giữa", "node thứ k từ cuối" | Fast & Slow / Two pointers cách khoảng |
| "Điểm bắt đầu chu trình" | Floyd gặp nhau → một về head, cùng 1 bước → gặp tại entry |
| "LRU Cache" / thiết kế với get/put O(1) | Doubly Linked List + HashMap |
| "Trộn k danh sách sắp xếp" | Min-Heap kích thước k |
| "Đảo từng nhóm k" | Thao tác con trỏ tại chỗ, tìm kth node |
| Yêu cầu O(1) bộ nhớ | Không được dùng mảng/set phụ — phải thao tác con trỏ |

**Quy tắc vàng:** Trước khi code linked list, **vẽ sơ đồ** (hình hộp + mũi tên) và điền giá trị con trỏ sau mỗi bước. Lỗi phổ biến nhất là mất con trỏ `next` trước khi đổi hướng — luôn `ListNode next = curr.next` trước khi `curr.next = prev`.
