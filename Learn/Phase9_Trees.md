# Phase 9 — Trees (Binary Tree & BST)

> **Ngôn ngữ:** C#
> **Mục tiêu:** Áp dụng đệ quy vào cấu trúc cây: duyệt (pre/in/post/level-order), tính chiều cao/đường kính, kiểm tra tính chất, khai thác **in-order BST = dãy tăng dần**.

---

## Bài học 1 — Nền Tảng

### 1.1 Định nghĩa node (dùng xuyên suốt)

```csharp
public class TreeNode
{
    public int val;
    public TreeNode left;
    public TreeNode right;
    public TreeNode(int val = 0, TreeNode left = null, TreeNode right = null)
    {
        this.val = val;
        this.left = left;
        this.right = right;
    }
}
```

### 1.2 Bốn kiểu duyệt cây

```csharp
// Pre-order: gốc → trái → phải
void PreOrder(TreeNode node)
{
    if (node == null) return;
    Console.Write(node.val);
    PreOrder(node.left);
    PreOrder(node.right);
}

// In-order: trái → gốc → phải (BST → dãy tăng)
void InOrder(TreeNode node)
{
    if (node == null) return;
    InOrder(node.left);
    Console.Write(node.val);
    InOrder(node.right);
}

// Post-order: trái → phải → gốc
void PostOrder(TreeNode node)
{
    if (node == null) return;
    PostOrder(node.left);
    PostOrder(node.right);
    Console.Write(node.val);
}

// Level-order: BFS theo tầng
IList<IList<int>> LevelOrder(TreeNode root)
{
    var result = new List<IList<int>>();
    if (root == null) return result;

    var queue = new Queue<TreeNode>();
    queue.Enqueue(root);

    while (queue.Count > 0)
    {
        int size = queue.Count; // số node ở tầng hiện tại
        var level = new List<int>();
        for (int i = 0; i < size; i++)
        {
            var node = queue.Dequeue();
            level.Add(node.val);
            if (node.left != null) queue.Enqueue(node.left);
            if (node.right != null) queue.Enqueue(node.right);
        }
        result.Add(level);
    }
    return result;
}
```

### 1.3 Hai chiều DFS quan trọng

```
Bottom-up (trả kết quả từ con lên cha):
  → dùng khi node cha cần tổng hợp thông tin từ cả hai con
  → ví dụ: tính chiều cao, đường kính, đường đi max sum
  → return giá trị từ hàm đệ quy

Top-down (truyền ràng buộc từ cha xuống con):
  → dùng khi con cần biết ngữ cảnh từ tổ tiên
  → ví dụ: validate BST với khoảng [min, max]
  → truyền tham số xuống
```

### 1.4 Phân biệt "trả về cho cha" vs "cập nhật biến toàn cục"

```
Chiều cao (height): trả về cho cha → dùng return value
Đường kính (diameter): cần biết chiều cao CẢ HAI con của một node
                       → cập nhật biến toàn cục khi ở mỗi node
                       → return chiều cao để cha dùng

Nguyên tắc: nếu câu trả lời cuối cùng KHÔNG phải là return của hàm đệ quy
            → cần biến ngoài (int[] maxRef hoặc field class)
```

---

## Bài tập theo thứ tự

### Easy

---

#### Bài 1: Maximum Depth of Binary Tree (LeetCode 104)

**Đề:** Cho `root` của cây nhị phân. Trả về **chiều sâu lớn nhất** (số node trên đường dài nhất từ gốc đến lá).

**Ví dụ:**
```
Input:  root = [3,9,20,null,null,15,7]
          3
         / \
        9  20
           / \
          15   7
Output: 3

Input:  root = [1,null,2]
Output: 2

Input:  root = null
Output: 0
Vì: cây rỗng có chiều sâu 0
```

**Constraint:**
- Số node: `[0, 10^4]`
- `-100 <= Node.val <= 100`

**Code C#:**
```csharp
// Optimal — DFS Bottom-Up: O(n) time, O(h) space (h = chiều cao cây)
public int MaxDepth(TreeNode root)
{
    if (root == null) return 0; // base case

    int leftDepth = MaxDepth(root.left);
    int rightDepth = MaxDepth(root.right);

    return 1 + Math.Max(leftDepth, rightDepth); // tổng hợp từ hai con
}

// Cách 2 — BFS (level-order): O(n) time, O(w) space (w = chiều rộng lớn nhất)
public int MaxDepthBFS(TreeNode root)
{
    if (root == null) return 0;

    var queue = new Queue<TreeNode>();
    queue.Enqueue(root);
    int depth = 0;

    while (queue.Count > 0)
    {
        int size = queue.Count;
        depth++;
        for (int i = 0; i < size; i++)
        {
            var node = queue.Dequeue();
            if (node.left != null) queue.Enqueue(node.left);
            if (node.right != null) queue.Enqueue(node.right);
        }
    }
    return depth;
}
```

---

#### Bài 2: Invert Binary Tree (LeetCode 226)

**Đề:** Cho `root` của cây nhị phân. Đảo ngược cây (mirror), trả về root của cây đã đảo.

**Ví dụ:**
```
Input:  root = [4,2,7,1,3,6,9]
          4              4
         / \    →       / \
        2   7          7   2
       / \ / \        / \ / \
      1  3 6  9      9  6 3  1
Output: [4,7,2,9,6,3,1]

Input:  root = [2,1,3]
Output: [2,3,1]

Input:  root = null
Output: null
```

**Constraint:**
- Số node: `[0, 100]`
- `-100 <= Node.val <= 100`

**Code C#:**
```csharp
// Optimal — DFS: O(n) time, O(h) space
public TreeNode InvertTree(TreeNode root)
{
    if (root == null) return null;

    // swap hai con — đệ quy xử lý phần còn lại
    (root.left, root.right) = (InvertTree(root.right), InvertTree(root.left));

    return root;
}
```

---

#### Bài 3: Same Tree (LeetCode 100) & Subtree of Another Tree (LeetCode 572)

**Đề (100):** Cho hai cây `p` và `q`. Trả về `true` nếu chúng có cấu trúc và giá trị giống nhau.

**Ví dụ (100):**
```
Input:  p = [1,2,3], q = [1,2,3]
Output: true

Input:  p = [1,2], q = [1,null,2]
Output: false
Vì: cấu trúc khác nhau

Input:  p = [1,2,1], q = [1,1,2]
Output: false
```

**Đề (572):** Cho hai cây `root` và `subRoot`. Trả về `true` nếu `subRoot` là cây con của `root` (cấu trúc và giá trị giống nhau).

**Ví dụ (572):**
```
Input:  root = [3,4,5,1,2], subRoot = [4,1,2]
Output: true

Input:  root = [3,4,5,1,2,null,null,null,null,0], subRoot = [4,1,2]
Output: false
Vì: cây con [4,1,2,null,null,0] ≠ [4,1,2]

Input:  root = [1], subRoot = [1]
Output: true
```

**Constraint:**
- (100): số node `[0, 100]`; (572): số node root `[1, 2000]`, subRoot `[1, 1000]`

**Code C#:**
```csharp
// Same Tree — O(n) time, O(h) space
public bool IsSameTree(TreeNode p, TreeNode q)
{
    if (p == null && q == null) return true;
    if (p == null || q == null) return false; // một null, một không
    if (p.val != q.val) return false;

    return IsSameTree(p.left, q.left) && IsSameTree(p.right, q.right);
}

// Subtree — O(m*n) time, O(h) space (m = size root, n = size subRoot)
public bool IsSubtree(TreeNode root, TreeNode subRoot)
{
    if (root == null) return false;
    if (IsSameTree(root, subRoot)) return true;

    return IsSubtree(root.left, subRoot) || IsSubtree(root.right, subRoot);
}
```

---

### Medium

---

#### Bài 4: Binary Tree Level Order Traversal (LeetCode 102) ⭐ Bài bản lề BFS

**Đề:** Cho `root` của cây nhị phân. Trả về giá trị các node theo từng tầng (từ trái sang phải, từ trên xuống dưới).

**Ví dụ:**
```
Input:  root = [3,9,20,null,null,15,7]
Output: [[3],[9,20],[15,7]]

Input:  root = [1]
Output: [[1]]

Input:  root = null
Output: []
```

**Constraint:**
- Số node: `[0, 2000]`
- `-1000 <= Node.val <= 1000`

**Phân tích 5 câu hỏi:**
1. **Input nói gì?** Cây nhị phân bất kỳ, có thể rỗng.
2. **Output cần gì?** Danh sách các danh sách — nhóm theo tầng, thứ tự trái→phải.
3. **Brute Force là gì?** DFS với tham số `depth` → lưu vào `result[depth]`. Hoạt động nhưng ít tự nhiên cho bài "theo tầng".
4. **Chỗ lãng phí nằm đâu?** DFS không "biết" mình đang ở tầng nào tự nhiên. BFS tự nhiên xử lý từng tầng.
5. **Pattern nào khớp?** BFS + Queue. Key trick: chụp `size = queue.Count` trước khi lặp — đó là số node ở tầng hiện tại.

**Code C#:**
```csharp
// Optimal — BFS: O(n) time, O(w) space (w = chiều rộng lớn nhất)
public IList<IList<int>> LevelOrder(TreeNode root)
{
    var result = new List<IList<int>>();
    if (root == null) return result;

    var queue = new Queue<TreeNode>();
    queue.Enqueue(root);

    while (queue.Count > 0)
    {
        int size = queue.Count; // số node ở tầng hiện tại — snapshot trước khi thêm con
        var level = new List<int>();

        for (int i = 0; i < size; i++)
        {
            var node = queue.Dequeue();
            level.Add(node.val);
            if (node.left != null) queue.Enqueue(node.left);
            if (node.right != null) queue.Enqueue(node.right);
        }
        result.Add(level);
    }
    return result;
}

// Cách 2 — DFS với depth: O(n) time, O(h) space
public IList<IList<int>> LevelOrderDFS(TreeNode root)
{
    var result = new List<IList<int>>();
    Dfs(root, 0, result);
    return result;
}

void Dfs(TreeNode node, int depth, List<IList<int>> result)
{
    if (node == null) return;
    if (depth == result.Count) result.Add(new List<int>());
    result[depth].Add(node.val);
    Dfs(node.left, depth + 1, result);
    Dfs(node.right, depth + 1, result);
}
```

---

#### Bài 5: Validate Binary Search Tree (LeetCode 98)

**Đề:** Cho `root` của cây nhị phân. Kiểm tra xem cây có phải BST hợp lệ hay không. BST hợp lệ: mọi node trong cây con trái < node hiện tại < mọi node trong cây con phải.

**Ví dụ:**
```
Input:  root = [2,1,3]
Output: true

Input:  root = [5,1,4,null,null,3,6]
          5
         / \
        1   4
           / \
          3   6
Output: false
Vì: 4 < 5 nhưng 4 là con phải của 5 → sai

Input:  root = [5,4,6,null,null,3,7]
Output: false
Vì: node 3 nằm trong cây con phải của 5 nhưng 3 < 5
```

**Constraint:**
- Số node: `[1, 10^4]`
- `-2^31 <= Node.val <= 2^31 - 1`

**Code C#:**
```csharp
// Brute Force — In-order rồi kiểm tra tăng dần: O(n) time, O(n) space
public bool IsValidBSTInorder(TreeNode root)
{
    var vals = new List<int>();
    InOrder(root, vals);
    for (int i = 1; i < vals.Count; i++)
        if (vals[i] <= vals[i - 1]) return false;
    return true;
}

void InOrder(TreeNode node, List<int> vals)
{
    if (node == null) return;
    InOrder(node.left, vals);
    vals.Add(node.val);
    InOrder(node.right, vals);
}

// Optimal — Top-Down truyền khoảng [min, max]: O(n) time, O(h) space
public bool IsValidBST(TreeNode root)
{
    return Validate(root, long.MinValue, long.MaxValue);
}

bool Validate(TreeNode node, long min, long max)
{
    if (node == null) return true;
    if (node.val <= min || node.val >= max) return false;

    // con trái: giá trị phải < node.val → max = node.val
    // con phải: giá trị phải > node.val → min = node.val
    return Validate(node.left, min, node.val)
        && Validate(node.right, node.val, max);
}
```

---

#### Bài 6: Lowest Common Ancestor (LeetCode 236 & 235)

**Đề (236 — Binary Tree):** Cho cây nhị phân và hai node `p`, `q`. Tìm tổ tiên chung gần nhất (LCA). LCA là node thấp nhất mà cả `p` và `q` đều là con cháu (một node có thể là con cháu của chính nó).

**Ví dụ:**
```
Input:  root = [3,5,1,6,2,0,8,null,null,7,4], p = 5, q = 1
Output: 3
Vì: 5 ở nhánh trái, 1 ở nhánh phải của 3

Input:  root = [3,5,1,6,2,0,8,null,null,7,4], p = 5, q = 4
Output: 5
Vì: 4 là con cháu của 5, và 5 là tổ tiên của chính nó

Input:  root = [1,2], p = 1, q = 2
Output: 1
```

**Constraint:**
- Số node: `[2, 10^5]`
- `p ≠ q`, cả `p` và `q` đều tồn tại trong cây

**Code C#:**
```csharp
// LCA cho Binary Tree (236): O(n) time, O(h) space
public TreeNode LowestCommonAncestor(TreeNode root, TreeNode p, TreeNode q)
{
    if (root == null) return null;
    if (root == p || root == q) return root; // tìm thấy một trong hai

    var left = LowestCommonAncestor(root.left, p, q);
    var right = LowestCommonAncestor(root.right, p, q);

    if (left != null && right != null) return root; // p và q mỗi bên một nhánh
    return left ?? right; // cả hai cùng nhánh → trả về nhánh có kết quả
}

// LCA cho BST (235) — khai thác tính chất BST: O(h) time, O(1) space
public TreeNode LowestCommonAncestorBST(TreeNode root, TreeNode p, TreeNode q)
{
    while (root != null)
    {
        if (p.val < root.val && q.val < root.val)
            root = root.left;  // cả hai ở nhánh trái
        else if (p.val > root.val && q.val > root.val)
            root = root.right; // cả hai ở nhánh phải
        else
            return root; // split point — đây là LCA
    }
    return null;
}
```

---

#### Bài 7: Construct Binary Tree from Preorder and Inorder (LeetCode 105)

**Đề:** Cho hai mảng `preorder` và `inorder` của cùng một cây nhị phân. Dựng lại cây.

**Ví dụ:**
```
Input:  preorder = [3,9,20,15,7], inorder = [9,3,15,20,7]
Output: [3,9,20,null,null,15,7]
Vì: preorder[0]=3 là gốc; trong inorder, 3 tại index 1
    → bên trái có 1 node (9), bên phải có 3 node (15,20,7)

Input:  preorder = [-1], inorder = [-1]
Output: [-1]

Input:  preorder = [1,2], inorder = [1,2]
Output: [1,null,2]
```

**Constraint:**
- `1 <= preorder.length <= 3000`
- Tất cả giá trị phân biệt

**Code C#:**
```csharp
// Optimal — Đệ quy + HashMap: O(n) time, O(n) space
public TreeNode BuildTree(int[] preorder, int[] inorder)
{
    var indexMap = new Dictionary<int, int>();
    for (int i = 0; i < inorder.Length; i++)
        indexMap[inorder[i]] = i;

    return Build(preorder, 0, preorder.Length - 1, 0, inorder.Length - 1, indexMap);
}

TreeNode Build(int[] pre, int preL, int preR, int inL, int inR, Dictionary<int, int> indexMap)
{
    if (preL > preR) return null;

    int rootVal = pre[preL];           // phần tử đầu tiên trong preorder = gốc
    int mid = indexMap[rootVal];       // vị trí gốc trong inorder
    int leftSize = mid - inL;          // kích thước cây con trái

    var root = new TreeNode(rootVal);
    root.left = Build(pre, preL + 1, preL + leftSize, inL, mid - 1, indexMap);
    root.right = Build(pre, preL + leftSize + 1, preR, mid + 1, inR, indexMap);
    return root;
}
```

---

#### Bài 8: Kth Smallest Element in a BST (LeetCode 230)

**Đề:** Cho `root` của BST và số nguyên `k`. Trả về phần tử nhỏ thứ `k` trong BST.

**Ví dụ:**
```
Input:  root = [3,1,4,null,2], k = 1
Output: 1

Input:  root = [5,3,6,2,4,null,null,1], k = 3
Output: 3

Input:  root = [1], k = 1
Output: 1
```

**Constraint:**
- Số node: `[1, 10^4]`
- `1 <= k <= n`
- `0 <= Node.val <= 10^4`

**Code C#:**
```csharp
// Optimal — In-order dừng sớm: O(h + k) time, O(h) space
public int KthSmallest(TreeNode root, int k)
{
    int count = 0, result = 0;
    InOrder(root, k, ref count, ref result);
    return result;
}

void InOrder(TreeNode node, int k, ref int count, ref int result)
{
    if (node == null) return;

    InOrder(node.left, k, ref count, ref result);

    count++;
    if (count == k)
    {
        result = node.val;
        return; // dừng sớm — đã tìm thấy
    }

    InOrder(node.right, k, ref count, ref result);
}
```

---

### Hard

---

#### Bài 9: Binary Tree Maximum Path Sum (LeetCode 124) ⭐ Bài bản lề Hard

**Đề:** Cho `root` của cây nhị phân. **Đường đi** là chuỗi node liên tiếp theo cạnh (không lặp lại). Tổng đường đi là tổng giá trị các node. Trả về tổng đường đi lớn nhất (đường đi có thể bắt đầu và kết thúc tại bất kỳ node nào).

**Ví dụ:**
```
Input:  root = [1,2,3]
Output: 6
Vì: đường đi 2 → 1 → 3 có tổng 6

Input:  root = [-10,9,20,null,null,15,7]
         -10
         / \
        9  20
           / \
          15   7
Output: 42
Vì: đường đi 15 → 20 → 7 có tổng 42

Input:  root = [-3]
Output: -3
Vì: phải chọn ít nhất 1 node
```

**Constraint:**
- Số node: `[1, 3*10^4]`
- `-1000 <= Node.val <= 1000`

**Phân tích 5 câu hỏi:**
1. **Input nói gì?** Cây nhị phân bất kỳ, có thể có giá trị âm. Đường đi không phải gốc-tới-lá — bất kỳ node tới node nào.
2. **Output cần gì?** Một số nguyên — tổng đường đi lớn nhất.
3. **Brute Force là gì?** Với mỗi node, tính tổng mọi đường đi đi qua nó. O(n²).
4. **Chỗ lãng phí nằm đâu?** Tính lại subtree nhiều lần. Mỗi node chỉ cần biết "nhánh trái dài nhất" và "nhánh phải dài nhất" của nó.
5. **Pattern nào khớp?** DFS Bottom-up. **Điểm mấu chốt:** phân biệt hai thứ — (a) giá trị trả về cho cha (chỉ được đi một hướng: trái hoặc phải, không cả hai) và (b) giá trị cập nhật toàn cục (đường đi qua node này — đi cả trái lẫn phải).

**Code C#:**
```csharp
// Optimal — DFS Bottom-Up + biến toàn cục: O(n) time, O(h) space
public int MaxPathSum(TreeNode root)
{
    int maxSum = int.MinValue;
    MaxGain(root, ref maxSum);
    return maxSum;
}

int MaxGain(TreeNode node, ref int maxSum)
{
    if (node == null) return 0;

    // lấy max với 0 để bỏ nhánh âm
    int leftGain = Math.Max(MaxGain(node.left, ref maxSum), 0);
    int rightGain = Math.Max(MaxGain(node.right, ref maxSum), 0);

    // giá trị đường đi qua node này (có thể rẽ cả hai hướng)
    int pathThroughNode = node.val + leftGain + rightGain;
    maxSum = Math.Max(maxSum, pathThroughNode); // cập nhật toàn cục

    // trả về cho cha: chỉ được đi một hướng (trái hoặc phải, không cả hai)
    return node.val + Math.Max(leftGain, rightGain);
}
```

---

#### Bài 10: Serialize and Deserialize Binary Tree (LeetCode 297)

**Đề:** Thiết kế thuật toán để serialize (mã hóa) và deserialize (giải mã) một cây nhị phân. Serialize là biến cây thành chuỗi; Deserialize là dựng lại cây từ chuỗi đó.

**Ví dụ:**
```
Input:  root = [1,2,3,null,null,4,5]
Serialize → "1,2,3,null,null,4,5"  (hoặc format khác tùy chọn)
Deserialize → cây gốc [1,2,3,null,null,4,5]

Input:  root = null
Serialize → "null"
Deserialize → null

Input:  root = [1]
Serialize → "1,null,null"
Deserialize → cây chỉ có node 1
```

**Constraint:**
- Số node: `[0, 10^4]`
- `-1000 <= Node.val <= 1000`

**Code C#:**
```csharp
// Optimal — Pre-order DFS với marker "null": O(n) time & space
public class Codec
{
    public string Serialize(TreeNode root)
    {
        var sb = new System.Text.StringBuilder();
        SerializeHelper(root, sb);
        return sb.ToString().TrimEnd(',');
    }

    void SerializeHelper(TreeNode node, System.Text.StringBuilder sb)
    {
        if (node == null) { sb.Append("null,"); return; }
        sb.Append(node.val).Append(',');
        SerializeHelper(node.left, sb);
        SerializeHelper(node.right, sb);
    }

    public TreeNode Deserialize(string data)
    {
        var tokens = new Queue<string>(data.Split(','));
        return DeserializeHelper(tokens);
    }

    TreeNode DeserializeHelper(Queue<string> tokens)
    {
        string val = tokens.Dequeue();
        if (val == "null") return null;

        var node = new TreeNode(int.Parse(val));
        node.left = DeserializeHelper(tokens);   // pre-order: trái trước
        node.right = DeserializeHelper(tokens);
        return node;
    }
}
```

---

## Tổng kết Pattern

| Dấu hiệu đề | Kiểu DFS | Ghi chú |
|-------------|----------|---------|
| Chiều cao, min/max depth | Bottom-up, return value | `1 + max(left, right)` |
| Đường đi max sum, đường kính | Bottom-up + biến toàn cục | Return ≠ answer |
| Validate BST | Top-down truyền khoảng | Cần `long` để tránh tràn |
| Theo tầng, BFS, zigzag | BFS + Queue | Chụp `size` trước vòng lặp |
| BST → in-order = sorted | In-order | Kth smallest, successor |
| LCA binary tree | Bottom-up, trả node | Nếu cả hai con non-null → gốc là LCA |
| LCA BST | Iterative theo tính chất | O(h) thay vì O(n) |
| Dựng cây từ duyệt | Pre/In order + hashmap | Preorder[0] = root |

## Tiêu chí hoàn thành
- [ ] Viết cả 4 kiểu duyệt từ trí nhớ.
- [ ] Phân biệt rõ "trả về cho cha" và "cập nhật biến toàn cục".
- [ ] Validate BST đúng cách (truyền khoảng, không chỉ so cha–con).
- [ ] Giải 124 (Max Path Sum) không nhìn lời giải.
- [ ] Hiểu tại sao in-order BST = dãy tăng và ứng dụng được.
