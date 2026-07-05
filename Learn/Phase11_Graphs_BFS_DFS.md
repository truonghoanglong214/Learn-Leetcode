# Phase 11 — Graphs (BFS / DFS cơ bản)

> **Ngôn ngữ:** C#
> **Mục tiêu:** Mô hình hóa quan hệ thành đồ thị và **duyệt** thành thạo: thành phần liên thông, đường đi ngắn nhất trên đồ thị không trọng số, lan tỏa (flood fill / multi-source BFS), phát hiện chu trình.

---

## Bài học 1 — Nền Tảng

### 1.1 Biểu diễn đồ thị

```csharp
// Danh sách kề (Adjacency List) — phổ biến nhất
int n = 5;
var adj = new List<int>[n];
for (int i = 0; i < n; i++)
    adj[i] = new List<int>();

// Thêm cạnh vô hướng
void AddEdge(int u, int v) { adj[u].Add(v); adj[v].Add(u); }

// Ma trận kề — dùng khi n nhỏ (≤ 1000)
bool[,] matrix = new bool[n, n];
matrix[u, v] = matrix[v, u] = true;

// Grid (lưới 2D) — coi mỗi ô là đỉnh, ô liền kề là cạnh
int[][] dirs = { new[]{0,1}, new[]{0,-1}, new[]{1,0}, new[]{-1,0} };
// Duyệt 4 hướng từ (r, c):
foreach (var d in dirs)
{
    int nr = r + d[0], nc = c + d[1];
    if (nr >= 0 && nr < rows && nc >= 0 && nc < cols)
        // xử lý (nr, nc)
}
```

### 1.2 DFS (Depth-First Search)

```csharp
// DFS đệ quy — T: O(V+E), S: O(V) stack
void DFS(int u, bool[] visited, List<int>[] adj)
{
    visited[u] = true;
    foreach (int v in adj[u])
        if (!visited[v])
            DFS(v, visited, adj);
}

// DFS lặp (dùng Stack)
void DFSIterative(int start, List<int>[] adj)
{
    bool[] visited = new bool[adj.Length];
    var stack = new Stack<int>();
    stack.Push(start);
    while (stack.Count > 0)
    {
        int u = stack.Pop();
        if (visited[u]) continue;
        visited[u] = true;
        foreach (int v in adj[u])
            if (!visited[v])
                stack.Push(v);
    }
}
```

### 1.3 BFS (Breadth-First Search)

```csharp
// BFS — T: O(V+E), S: O(V)
// QUAN TRỌNG: đánh dấu visited khi ENQUEUE, không phải khi duyệt
int[] BFS(int start, List<int>[] adj)
{
    int n = adj.Length;
    int[] dist = new int[n];
    Array.Fill(dist, -1);
    dist[start] = 0;

    var queue = new Queue<int>();
    queue.Enqueue(start);

    while (queue.Count > 0)
    {
        int u = queue.Dequeue();
        foreach (int v in adj[u])
        {
            if (dist[v] == -1)
            {
                dist[v] = dist[u] + 1;
                queue.Enqueue(v);
            }
        }
    }
    return dist;
    // dist[v] = khoảng cách ngắn nhất từ start đến v (đơn vị cạnh)
}
```

> **Quy tắc vàng:** BFS = shortest path trên đồ thị **không trọng số**. DFS = duyệt, tìm thành phần, phát hiện chu trình.

### 1.4 Grid template

```csharp
// Template DFS/BFS trên lưới
int rows, cols;
bool[,] visited;
int[][] dirs = { new[]{0,1}, new[]{0,-1}, new[]{1,0}, new[]{-1,0} };

void DFSGrid(int r, int c, char[][] grid)
{
    if (r < 0 || r >= rows || c < 0 || c >= cols) return;  // biên
    if (visited[r, c] || grid[r][c] == '0') return;         // đã thăm / không hợp lệ
    visited[r, c] = true;
    foreach (var d in dirs)
        DFSGrid(r + d[0], c + d[1], grid);
}
```

---

## Bài tập

---

### Bài 1 — Flood Fill (LC 733) ⭐

**Đề:** Cho ảnh `image` (ma trận 2D số nguyên) và điểm bắt đầu `(sr, sc)`, thực hiện flood fill: tô màu `newColor` cho ô bắt đầu và tất cả ô kề liên thông có cùng màu gốc.

**Ví dụ:**
```
Input:  image = [[1,1,1],[1,1,0],[1,0,1]], sr=1, sc=1, newColor=2
Output: [[2,2,2],[2,2,0],[2,0,1]]
Vì: (1,1) màu 1, lan ra tất cả ô màu 1 liền kề 4 hướng

Input:  image = [[0,0,0],[0,0,0]], sr=0, sc=0, newColor=0
Output: [[0,0,0],[0,0,0]]
Vì: newColor == màu gốc, không cần làm gì (tránh vòng lặp vô hạn)

Input:  image = [[1]], sr=0, sc=0, newColor=5
Output: [[5]]
```

**Constraint:**
- `1 <= image.length, image[0].length <= 50`
- `0 <= image[i][j], newColor < 2^16`
- `0 <= sr < image.length`, `0 <= sc < image[0].length`

**Code C#:**
```csharp
// DFS — T: O(m*n), S: O(m*n) stack
public int[][] FloodFill(int[][] image, int sr, int sc, int newColor)
{
    int oldColor = image[sr][sc];
    if (oldColor == newColor) return image;  // tránh lặp vô hạn
    DFS(image, sr, sc, oldColor, newColor);
    return image;
}

private void DFS(int[][] image, int r, int c, int oldColor, int newColor)
{
    if (r < 0 || r >= image.Length || c < 0 || c >= image[0].Length) return;
    if (image[r][c] != oldColor) return;
    image[r][c] = newColor;
    DFS(image, r + 1, c, oldColor, newColor);
    DFS(image, r - 1, c, oldColor, newColor);
    DFS(image, r, c + 1, oldColor, newColor);
    DFS(image, r, c - 1, oldColor, newColor);
    // T: O(m*n)   S: O(m*n) do stack đệ quy
}
```

---

### Bài 2 — Number of Islands (LC 200) ⭐⭐ (bản lề)

**Đề:** Cho lưới `grid` gồm `'1'` (đất) và `'0'` (nước). Đảo là nhóm `'1'` liền kề (4 hướng). Đếm số đảo.

**Ví dụ:**
```
Input:
grid = [["1","1","1","1","0"],
        ["1","1","0","1","0"],
        ["1","1","0","0","0"],
        ["0","0","0","0","0"]]
Output: 1

Input:
grid = [["1","1","0","0","0"],
        ["1","1","0","0","0"],
        ["0","0","1","0","0"],
        ["0","0","0","1","1"]]
Output: 3

Input:  grid = [["1"]], Output: 1
Input:  grid = [["0"]], Output: 0  (edge case: không có đảo)
```

**Constraint:**
- `1 <= grid.length, grid[0].length <= 300`
- `grid[i][j]` là `'0'` hoặc `'1'`

**Phân tích 5 câu hỏi** (bài bản lề — connected components trên grid):
1. **Input nói gì?** Lưới 2D, mỗi ô là đỉnh, các ô `'1'` liền kề là cạnh.
2. **Output cần gì?** Số thành phần liên thông gồm `'1'`.
3. **Brute Force là gì?** Với mỗi ô `'1'`, DFS/BFS đánh dấu toàn bộ đảo. Đếm số lần khởi động DFS.
4. **Chỗ lãng phí?** Không có — DFS mỗi ô đúng 1 lần, O(m*n) là tối ưu.
5. **Pattern?** **Connected components / Flood fill** — DFS/BFS từ mỗi ô chưa thăm.

**Code C#:**
```csharp
// DFS — T: O(m*n), S: O(m*n)
public int NumIslands(char[][] grid)
{
    int rows = grid.Length, cols = grid[0].Length;
    int count = 0;

    for (int r = 0; r < rows; r++)
        for (int c = 0; c < cols; c++)
            if (grid[r][c] == '1')
            {
                count++;
                Sink(grid, r, c, rows, cols);  // đánh dấu toàn đảo
            }

    return count;
}

private void Sink(char[][] grid, int r, int c, int rows, int cols)
{
    if (r < 0 || r >= rows || c < 0 || c >= cols || grid[r][c] != '1') return;
    grid[r][c] = '0';  // đánh dấu đã thăm bằng cách "nhấn chìm"
    Sink(grid, r + 1, c, rows, cols);
    Sink(grid, r - 1, c, rows, cols);
    Sink(grid, r, c + 1, rows, cols);
    Sink(grid, r, c - 1, rows, cols);
    // T: O(m*n)   S: O(m*n) stack đệ quy
}
```

---

### Bài 3 — Rotting Oranges (LC 994) ⭐⭐

**Đề:** Lưới `grid` có 0 (rỗng), 1 (cam tươi), 2 (cam thối). Mỗi phút, cam thối làm thối cam tươi kề nó. Trả về số phút tối thiểu để tất cả cam thối hết (−1 nếu không thể).

**Ví dụ:**
```
Input:  grid = [[2,1,1],[1,1,0],[0,1,1]]
Output: 4

Input:  grid = [[2,1,1],[0,1,1],[1,0,1]]
Output: -1
Vì: góc dưới trái không thể bị thối

Input:  grid = [[0,2]]
Output: 0
Vì: không có cam tươi nào
```

**Constraint:**
- `1 <= grid.length, grid[0].length <= 10`
- `grid[i][j]` ∈ {0, 1, 2}

**Phân tích 5 câu hỏi** (bài bản lề — multi-source BFS):
1. **Input nói gì?** Lưới, lan tỏa đồng thời từ nhiều nguồn.
2. **Output cần gì?** Số bước (phút) tối thiểu — đây là độ sâu BFS.
3. **Brute Force là gì?** Mô phỏng từng phút, scan toàn lưới mỗi lần. T: O((m*n)²).
4. **Chỗ lãng phí?** Scan lại toàn bộ mỗi phút. BFS từ tất cả nguồn cùng lúc, mỗi ô chỉ thăm 1 lần.
5. **Pattern?** **Multi-source BFS** — enqueue tất cả cam thối ban đầu, BFS theo tầng.

**Code C#:**
```csharp
// Multi-source BFS — T: O(m*n), S: O(m*n)
public int OrangesRotting(int[][] grid)
{
    int rows = grid.Length, cols = grid[0].Length;
    var queue = new Queue<(int r, int c)>();
    int fresh = 0;

    // Tìm tất cả cam thối ban đầu và đếm cam tươi
    for (int r = 0; r < rows; r++)
        for (int c = 0; c < cols; c++)
        {
            if (grid[r][c] == 2) queue.Enqueue((r, c));
            else if (grid[r][c] == 1) fresh++;
        }

    if (fresh == 0) return 0;

    int[][] dirs = { new[]{0,1}, new[]{0,-1}, new[]{1,0}, new[]{-1,0} };
    int minutes = 0;

    while (queue.Count > 0 && fresh > 0)
    {
        minutes++;
        int size = queue.Count;  // xử lý đúng 1 tầng (1 phút)
        for (int i = 0; i < size; i++)
        {
            var (r, c) = queue.Dequeue();
            foreach (var d in dirs)
            {
                int nr = r + d[0], nc = c + d[1];
                if (nr >= 0 && nr < rows && nc >= 0 && nc < cols && grid[nr][nc] == 1)
                {
                    grid[nr][nc] = 2;
                    fresh--;
                    queue.Enqueue((nr, nc));
                }
            }
        }
    }

    return fresh == 0 ? minutes : -1;
    // T: O(m*n)   S: O(m*n)
}
```

---

### Bài 4 — Clone Graph (LC 133) ⭐⭐

**Đề:** Cho một đỉnh trong đồ thị vô hướng liên thông (mỗi đỉnh có `val` và `neighbors`). Tạo bản sao sâu (deep copy) của toàn bộ đồ thị.

**Ví dụ:**
```
Input:  adjList = [[2,4],[1,3],[2,4],[1,3]]
        (1 kề 2,4; 2 kề 1,3; 3 kề 2,4; 4 kề 1,3)
Output: bản sao với cấu trúc y hệt nhưng node mới hoàn toàn

Input:  node = null → Output: null

Input:  node duy nhất, không neighbors → Output: node mới, không neighbors
```

**Constraint:**
- Số đỉnh: 0–100, giá trị: 1–100, duy nhất.
- Không có cạnh lặp, không tự kề.

**Code C#:**
```csharp
public class Node
{
    public int val;
    public IList<Node> neighbors;
    public Node(int val) { this.val = val; neighbors = new List<Node>(); }
}

// DFS + HashMap ánh xạ node gốc → node bản sao — T: O(V+E), S: O(V)
public Node CloneGraph(Node node)
{
    if (node == null) return null;
    var visited = new Dictionary<Node, Node>();
    return DFS(node, visited);
}

private Node DFS(Node node, Dictionary<Node, Node> visited)
{
    if (visited.ContainsKey(node)) return visited[node];

    var copy = new Node(node.val);
    visited[node] = copy;  // đánh dấu TRƯỚC khi đệ quy (tránh vòng lặp)

    foreach (var neighbor in node.neighbors)
        copy.neighbors.Add(DFS(neighbor, visited));

    return copy;
    // T: O(V+E)   S: O(V)
}
```

---

### Bài 5 — Pacific Atlantic Water Flow (LC 417) ⭐⭐

**Đề:** Lưới `heights` biểu diễn độ cao. Nước có thể chảy 4 hướng sang ô có độ cao ≤ hiện tại. Bờ trái/trên → Thái Bình Dương; bờ phải/dưới → Đại Tây Dương. Tìm tất cả ô có thể chảy ra **cả hai** đại dương.

**Ví dụ:**
```
Input:
heights = [[1,2,2,3,5],
           [3,2,3,4,4],
           [2,4,5,3,1],
           [6,7,1,4,5],
           [5,1,1,2,4]]
Output: [[0,4],[1,3],[1,4],[2,2],[3,0],[3,1],[4,0]]

Input:  heights = [[1]], Output: [[0,0]]
Input:  heights = [[1,1],[1,1],[1,1]], Output: tất cả ô
```

**Constraint:**
- `1 <= heights.length, heights[0].length <= 200`
- `0 <= heights[i][j] <= 10^5`

**Phân tích 5 câu hỏi** (kỹ thuật đảo chiều):
1. **Input nói gì?** Lưới cao độ, nước chảy xuống thấp.
2. **Output cần gì?** Ô chảy được tới cả 2 đại dương.
3. **Brute Force là gì?** Với mỗi ô, DFS xuống thấp xem có tới 2 bờ không. T: O((m*n)²).
4. **Chỗ lãng phí?** DFS từ mỗi ô. Đảo chiều: BFS/DFS từ bờ **lên cao** — ô nào đến được từ cả 2 bờ là đáp án.
5. **Pattern?** **Multi-source BFS từ biên, đảo chiều tư duy** — chạy ngược từ đại dương lên.

**Code C#:**
```csharp
// BFS từ 2 bờ ngược lên — T: O(m*n), S: O(m*n)
public IList<IList<int>> PacificAtlantic(int[][] heights)
{
    int rows = heights.Length, cols = heights[0].Length;
    bool[,] pacific = new bool[rows, cols];
    bool[,] atlantic = new bool[rows, cols];

    var pacQueue = new Queue<(int, int)>();
    var atlQueue = new Queue<(int, int)>();

    for (int r = 0; r < rows; r++)
    {
        pacQueue.Enqueue((r, 0));        pacific[r, 0] = true;
        atlQueue.Enqueue((r, cols - 1)); atlantic[r, cols - 1] = true;
    }
    for (int c = 0; c < cols; c++)
    {
        pacQueue.Enqueue((0, c));        pacific[0, c] = true;
        atlQueue.Enqueue((rows - 1, c)); atlantic[rows - 1, c] = true;
    }

    BFSUp(heights, pacQueue, pacific, rows, cols);
    BFSUp(heights, atlQueue, atlantic, rows, cols);

    var result = new List<IList<int>>();
    for (int r = 0; r < rows; r++)
        for (int c = 0; c < cols; c++)
            if (pacific[r, c] && atlantic[r, c])
                result.Add(new List<int> { r, c });

    return result;
    // T: O(m*n)   S: O(m*n)
}

private void BFSUp(int[][] heights, Queue<(int, int)> queue, bool[,] visited, int rows, int cols)
{
    int[][] dirs = { new[]{0,1}, new[]{0,-1}, new[]{1,0}, new[]{-1,0} };
    while (queue.Count > 0)
    {
        var (r, c) = queue.Dequeue();
        foreach (var d in dirs)
        {
            int nr = r + d[0], nc = c + d[1];
            if (nr >= 0 && nr < rows && nc >= 0 && nc < cols
                && !visited[nr, nc]
                && heights[nr][nc] >= heights[r][c])  // ngược chiều: lên cao
            {
                visited[nr, nc] = true;
                queue.Enqueue((nr, nc));
            }
        }
    }
}
```

---

### Bài 6 — Course Schedule (LC 207) ⭐⭐ (mở đường sang Topo Sort)

**Đề:** Có `numCourses` khóa học (0..n-1) và danh sách `prerequisites[i] = [a, b]` nghĩa là phải học b trước a. Kiểm tra xem có thể hoàn thành tất cả khóa học không (tức là không có chu trình phụ thuộc).

**Ví dụ:**
```
Input:  numCourses = 2, prerequisites = [[1,0]]
Output: true
Vì: 0 → 1, không có chu trình

Input:  numCourses = 2, prerequisites = [[1,0],[0,1]]
Output: false
Vì: 0→1→0 là chu trình

Input:  numCourses = 1, prerequisites = []
Output: true
```

**Constraint:**
- `1 <= numCourses <= 2000`
- `0 <= prerequisites.length <= 5000`
- `prerequisites[i].length == 2`, không có self-loop, không trùng cặp.

**Phân tích 5 câu hỏi** (bài mở đường Topo Sort):
1. **Input nói gì?** Đồ thị có hướng biểu diễn phụ thuộc.
2. **Output cần gì?** Có chu trình không? (nếu có thì không thể hoàn thành)
3. **Brute Force là gì?** Thử mọi thứ tự — O(n!), không khả thi.
4. **Chỗ lãng phí?** Dùng DFS với 3 trạng thái (chưa thăm / đang thăm / đã thăm) phát hiện chu trình O(V+E).
5. **Pattern?** **DFS phát hiện chu trình** — đỉnh "đang thăm" mà gặp lại → chu trình.

**Code C#:**
```csharp
// DFS 3 màu phát hiện chu trình — T: O(V+E), S: O(V+E)
public bool CanFinish(int numCourses, int[][] prerequisites)
{
    var adj = new List<int>[numCourses];
    for (int i = 0; i < numCourses; i++)
        adj[i] = new List<int>();

    foreach (var pre in prerequisites)
        adj[pre[1]].Add(pre[0]);  // b → a (học b trước a)

    // 0 = chưa thăm, 1 = đang thăm (trong stack), 2 = đã xong
    int[] state = new int[numCourses];

    for (int i = 0; i < numCourses; i++)
        if (state[i] == 0 && HasCycle(adj, state, i))
            return false;

    return true;
}

private bool HasCycle(List<int>[] adj, int[] state, int u)
{
    state[u] = 1;  // đang thăm
    foreach (int v in adj[u])
    {
        if (state[v] == 1) return true;   // gặp lại đỉnh đang thăm → chu trình
        if (state[v] == 0 && HasCycle(adj, state, v)) return true;
    }
    state[u] = 2;  // đã xong
    return false;
    // T: O(V+E)   S: O(V+E)
}
```

---

### Bài 7 — Word Ladder (LC 127) ⭐⭐⭐ (bài Hard — BFS trên đồ thị ẩn)

**Đề:** Cho `beginWord`, `endWord` và `wordList`. Mỗi bước đổi đúng 1 ký tự, từ mới phải có trong wordList. Tìm số bước ngắn nhất từ `beginWord` đến `endWord` (0 nếu không có đường).

**Ví dụ:**
```
Input:  beginWord="hit", endWord="cog", wordList=["hot","dot","dog","lot","log","cog"]
Output: 5
Vì: hit → hot → dot → dog → cog (5 từ = 4 bước + 1)

Input:  beginWord="hit", endWord="cog", wordList=["hot","dot","dog","lot","log"]
Output: 0
Vì: "cog" không trong wordList

Input:  beginWord="a", endWord="c", wordList=["a","b","c"]
Output: 2
```

**Constraint:**
- `1 <= beginWord.length <= 10`
- `endWord.length == beginWord.length`
- `1 <= wordList.length <= 5000`
- Tất cả chỉ gồm chữ thường.

**Phân tích 5 câu hỏi**:
1. **Input nói gì?** Không có đồ thị tường minh — cạnh được xác định khi 2 từ khác 1 ký tự.
2. **Output cần gì?** Đường đi ngắn nhất (số bước) → BFS.
3. **Brute Force là gì?** Với mỗi cặp từ, kiểm tra kề nhau. BFS là O(V+E) đã ổn.
4. **Chỗ lãng phí?** Khi tìm hàng xóm, thay vì duyệt toàn bộ wordList O(n), dùng kỹ thuật "thay từng ký tự" O(26 * len).
5. **Pattern?** **BFS trên đồ thị ẩn** — xây cạnh ngầm trong lúc BFS.

**Code C#:**
```csharp
// BFS xây cạnh ngầm — T: O(n * len * 26), S: O(n)
public int LadderLength(string beginWord, string endWord, IList<string> wordList)
{
    var wordSet = new HashSet<string>(wordList);
    if (!wordSet.Contains(endWord)) return 0;

    var queue = new Queue<string>();
    queue.Enqueue(beginWord);
    wordSet.Remove(beginWord);
    int steps = 1;

    while (queue.Count > 0)
    {
        steps++;
        int size = queue.Count;
        for (int i = 0; i < size; i++)
        {
            string word = queue.Dequeue();
            char[] arr = word.ToCharArray();

            for (int j = 0; j < arr.Length; j++)
            {
                char orig = arr[j];
                for (char c = 'a'; c <= 'z'; c++)
                {
                    if (c == orig) continue;
                    arr[j] = c;
                    string next = new string(arr);
                    if (next == endWord) return steps;
                    if (wordSet.Contains(next))
                    {
                        wordSet.Remove(next);  // xóa để không thăm lại
                        queue.Enqueue(next);
                    }
                }
                arr[j] = orig;
            }
        }
    }

    return 0;
    // T: O(n * len * 26)   S: O(n)
}
```

---

## Tổng kết Pattern

| Pattern | Dấu hiệu đề | Thuật toán | Độ phức tạp |
|---------|------------|-----------|------------|
| Connected components | "đảo", "vùng liên thông", "nhóm" | DFS/BFS + visited | O(V+E) |
| Shortest path (unweighted) | "số bước **ít nhất**" | BFS | O(V+E) |
| Multi-source BFS | "lan từ nhiều nguồn đồng thời" | Enqueue tất cả nguồn | O(V+E) |
| Cycle detection | "có thể hoàn thành không", "phụ thuộc vòng" | DFS 3 màu | O(V+E) |
| BFS trên đồ thị ẩn | "từ biến đổi", cạnh ngầm từ điều kiện | BFS + xây cạnh inline | O(V+E) |
| Đảo chiều BFS | "chảy tới 2 biên", "reach từ 2 phía" | BFS từ biên ngược lại | O(V+E) |

## Tiêu chí hoàn thành Phase 11

- [ ] Viết DFS và BFS trên cả grid lẫn danh sách kề từ trí nhớ.
- [ ] Nhận ra "shortest unweighted → BFS" ngay.
- [ ] Giải Number of Islands và Rotting Oranges không nhìn lời giải.
