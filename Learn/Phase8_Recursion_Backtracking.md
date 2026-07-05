# Phase 8 — Recursion & Backtracking

> **Ngôn ngữ:** C#
> **Mục tiêu:** Xây tư duy đệ quy vững ("leap of faith") và làm chủ backtracking để sinh mọi hoán vị / tổ hợp / tập con. Kỹ năng cốt lõi: **chọn → đệ quy → bỏ chọn (undo)**.

---

## Bài học 1 — Tư Duy Đệ Quy & Ba Khuôn Mẫu Backtracking

### 1.1 Cấu trúc đệ quy chuẩn

```
f(n) = base case              ← dừng khi nào?
     | kết hợp(f(n-1), ...)  ← tin rằng f(n-1) đúng, dùng nó
```

**Leap of faith:** Giả định hàm đệ quy đã làm đúng cho bài nhỏ hơn → dùng kết quả đó để giải bài lớn hơn. Đừng cố "theo dõi" cả chuỗi gọi — chỉ nghĩ tầng hiện tại.

### 1.2 Template Backtracking tổng quát

```csharp
void Backtrack(trạng_thái_hiện_tại, kết_quả, danh_sách_lựa_chọn)
{
    if (thỏa điều kiện dừng)
    {
        kết_quả.Add(bản_sao_trạng_thái);  // LUÔN lưu bản sao!
        return;
    }

    foreach (var lựa_chọn in danh_sách_lựa_chọn)
    {
        // Chọn
        trạng_thái.Add(lựa_chọn);

        // Đệ quy
        Backtrack(trạng_thái, kết_quả, lựa_chọn_tiếp_theo);

        // Bỏ chọn (undo)
        trạng_thái.RemoveAt(trạng_thái.Count - 1);
    }
}
```

### 1.3 Ba khuôn mẫu cốt lõi

| Khuôn mẫu | Đặc điểm | Bài điển hình |
|-----------|----------|---------------|
| **Subsets** | mỗi phần tử: chọn hoặc không chọn | Subsets (78) |
| **Combinations** | chọn k từ n, thứ tự không quan trọng, `start` tăng dần | Combination Sum (39) |
| **Permutations** | thứ tự quan trọng, mảng `used[]` | Permutations (46) |

### 1.4 Xử lý duplicate

```
Nguyên tắc: sort trước → bỏ qua phần tử trùng ở CÙNG CẤP (cùng vị trí trong đệ quy).

if (i > start && nums[i] == nums[i-1]) continue; // bỏ trùng cùng cấp
```

---

## Bài tập theo thứ tự

### Easy / Khởi động

---

#### Bài 1: Climbing Stairs (LeetCode 70) ⭐ Khởi động đệ quy

**Đề:** Có `n` bậc thang. Mỗi lần có thể leo 1 hoặc 2 bậc. Đếm số cách leo lên đỉnh.

**Ví dụ:**
```
Input:  n = 2
Output: 2
Vì: [1+1], [2]

Input:  n = 3
Output: 3
Vì: [1+1+1], [1+2], [2+1]

Input:  n = 1
Output: 1
Vì: chỉ có [1]
```

**Constraint:**
- `1 <= n <= 45`

**Phân tích 5 câu hỏi:**
1. **Input nói gì?** n nhỏ (≤ 45), không có âm, không có trùng.
2. **Output cần gì?** Một số nguyên — đếm số cách.
3. **Brute Force là gì?** Đệ quy thuần: `f(n) = f(n-1) + f(n-2)`. O(2ⁿ) — tính lại rất nhiều.
4. **Chỗ lãng phí nằm đâu?** `f(3)` được gọi nhiều lần trong các nhánh khác nhau → thêm memo hoặc đổi sang DP.
5. **Pattern nào khớp?** Đây chính là Fibonacci. Nhận ra điểm bắc cầu sang DP.

**Code C#:**
```csharp
// Brute Force — đệ quy thuần: O(2^n) time, O(n) space (stack)
public int ClimbStairsNaive(int n)
{
    if (n <= 1) return 1;
    return ClimbStairsNaive(n - 1) + ClimbStairsNaive(n - 2);
}

// Better — Memo (Top-Down DP): O(n) time, O(n) space
public int ClimbStairsMemo(int n)
{
    var memo = new int[n + 1];
    return Solve(n, memo);
}

int Solve(int n, int[] memo)
{
    if (n <= 1) return 1;
    if (memo[n] != 0) return memo[n];
    memo[n] = Solve(n - 1, memo) + Solve(n - 2, memo);
    return memo[n];
}

// Optimal — Bottom-Up DP: O(n) time, O(1) space
public int ClimbStairs(int n)
{
    if (n <= 1) return 1;
    int prev2 = 1, prev1 = 1;
    for (int i = 2; i <= n; i++)
    {
        int curr = prev1 + prev2;
        prev2 = prev1;
        prev1 = curr;
    }
    return prev1;
}
```

---

### Medium

---

#### Bài 2: Subsets (LeetCode 78) ⭐ Bài bản lề Backtracking số một

**Đề:** Cho mảng `nums` với các phần tử **phân biệt**. Trả về tất cả các tập con (power set). Thứ tự trong kết quả không quan trọng.

**Ví dụ:**
```
Input:  nums = [1,2,3]
Output: [[],[1],[2],[3],[1,2],[1,3],[2,3],[1,2,3]]

Input:  nums = [0]
Output: [[],[0]]

Input:  nums = []
Output: [[]]
Vì: tập rỗng luôn là tập con
```

**Constraint:**
- `1 <= nums.length <= 10`
- `-10 <= nums[i] <= 10`
- Tất cả phần tử trong `nums` là **phân biệt**

**Phân tích 5 câu hỏi:**
1. **Input nói gì?** n ≤ 10 → O(2ⁿ) chấp nhận được. Phần tử phân biệt (không cần xử lý duplicate).
2. **Output cần gì?** Tất cả tập con — sinh nghiệm hoàn toàn.
3. **Brute Force là gì?** Với mỗi phần tử, quyết định chọn hay không → cây nhị phân 2ⁿ nhánh.
4. **Chỗ lãng phí nằm đâu?** Không có lãng phí — đây là bài sinh nghiệm, O(2ⁿ) là tối ưu.
5. **Pattern nào khớp?** Backtracking — khuôn mẫu Subsets. Dùng `start` để tránh lấy phần tử đứng trước.

**Code C#:**
```csharp
// Optimal — Backtracking: O(n * 2^n) time, O(n) space (stack)
public IList<IList<int>> Subsets(int[] nums)
{
    var result = new List<IList<int>>();
    Backtrack(nums, 0, new List<int>(), result);
    return result;
}

void Backtrack(int[] nums, int start, List<int> current, List<IList<int>> result)
{
    result.Add(new List<int>(current)); // lưu bản sao tại mọi nút

    for (int i = start; i < nums.Length; i++)
    {
        current.Add(nums[i]);               // chọn
        Backtrack(nums, i + 1, current, result); // đệ quy — i+1 để không lấy lại
        current.RemoveAt(current.Count - 1); // bỏ chọn
    }
}

// Cách 2 — Iterative (Bitmask): O(n * 2^n) time, O(2^n) space
public IList<IList<int>> SubsetsBitmask(int[] nums)
{
    int n = nums.Length;
    var result = new List<IList<int>>();
    for (int mask = 0; mask < (1 << n); mask++)
    {
        var subset = new List<int>();
        for (int i = 0; i < n; i++)
            if ((mask & (1 << i)) != 0)
                subset.Add(nums[i]);
        result.Add(subset);
    }
    return result;
}
```

---

#### Bài 3: Combination Sum (LeetCode 39)

**Đề:** Cho mảng `candidates` với các số **phân biệt** và số nguyên `target`. Tìm tất cả tổ hợp mà tổng bằng `target`. Mỗi số trong `candidates` có thể dùng **không giới hạn lần**. Không trả về tổ hợp trùng lặp.

**Ví dụ:**
```
Input:  candidates = [2,3,6,7], target = 7
Output: [[2,2,3],[7]]

Input:  candidates = [2,3,5], target = 8
Output: [[2,2,2,2],[2,3,3],[3,5]]

Input:  candidates = [2], target = 1
Output: []
Vì: không thể đạt tổng 1 từ [2]
```

**Constraint:**
- `1 <= candidates.length <= 30`
- `2 <= candidates[i] <= 40`
- Tất cả phần tử phân biệt
- `1 <= target <= 40`

**Code C#:**
```csharp
// Optimal — Backtracking + Pruning: O(n^(T/M)) time, O(T/M) space
// T = target, M = giá trị nhỏ nhất trong candidates
public IList<IList<int>> CombinationSum(int[] candidates, int target)
{
    Array.Sort(candidates); // sort để cắt nhánh sớm
    var result = new List<IList<int>>();
    Backtrack(candidates, target, 0, new List<int>(), result);
    return result;
}

void Backtrack(int[] candidates, int remain, int start, List<int> current, List<IList<int>> result)
{
    if (remain == 0)
    {
        result.Add(new List<int>(current));
        return;
    }

    for (int i = start; i < candidates.Length; i++)
    {
        if (candidates[i] > remain) break; // pruning — candidates đã sort

        current.Add(candidates[i]);
        Backtrack(candidates, remain - candidates[i], i, current, result); // i, không phải i+1 — dùng lại được
        current.RemoveAt(current.Count - 1);
    }
}
```

---

#### Bài 4: Permutations (LeetCode 46)

**Đề:** Cho mảng `nums` với các phần tử **phân biệt**. Trả về tất cả hoán vị có thể của mảng.

**Ví dụ:**
```
Input:  nums = [1,2,3]
Output: [[1,2,3],[1,3,2],[2,1,3],[2,3,1],[3,1,2],[3,2,1]]

Input:  nums = [0,1]
Output: [[0,1],[1,0]]

Input:  nums = [1]
Output: [[1]]
```

**Constraint:**
- `1 <= nums.length <= 6`
- `-10 <= nums[i] <= 10`
- Tất cả phần tử phân biệt

**Code C#:**
```csharp
// Optimal — Backtracking với mảng used[]: O(n! * n) time, O(n) space
public IList<IList<int>> Permute(int[] nums)
{
    var result = new List<IList<int>>();
    Backtrack(nums, new bool[nums.Length], new List<int>(), result);
    return result;
}

void Backtrack(int[] nums, bool[] used, List<int> current, List<IList<int>> result)
{
    if (current.Count == nums.Length)
    {
        result.Add(new List<int>(current));
        return;
    }

    for (int i = 0; i < nums.Length; i++)
    {
        if (used[i]) continue; // phần tử đã chọn trong hoán vị này

        used[i] = true;
        current.Add(nums[i]);
        Backtrack(nums, used, current, result);
        current.RemoveAt(current.Count - 1);
        used[i] = false;
    }
}
```

---

#### Bài 5: Subsets II (LeetCode 90) — Xử lý Duplicate

**Đề:** Cho mảng `nums` có thể có **phần tử trùng**. Trả về tất cả tập con (không có tập con trùng lặp).

**Ví dụ:**
```
Input:  nums = [1,2,2]
Output: [[],[1],[1,2],[1,2,2],[2],[2,2]]

Input:  nums = [0]
Output: [[],[0]]

Input:  nums = [1,1,1]
Output: [[],[1],[1,1],[1,1,1]]
Vì: chỉ có 4 tập con khác nhau dù nums có 3 số 1
```

**Constraint:**
- `1 <= nums.length <= 10`
- `-10 <= nums[i] <= 10`

**Code C#:**
```csharp
// Optimal — Backtracking + Skip duplicate: O(n * 2^n) time, O(n) space
public IList<IList<int>> SubsetsWithDup(int[] nums)
{
    Array.Sort(nums); // bắt buộc sort để phát hiện trùng
    var result = new List<IList<int>>();
    Backtrack(nums, 0, new List<int>(), result);
    return result;
}

void Backtrack(int[] nums, int start, List<int> current, List<IList<int>> result)
{
    result.Add(new List<int>(current));

    for (int i = start; i < nums.Length; i++)
    {
        // bỏ qua phần tử trùng ở CÙNG CẤP (cùng vị trí start)
        if (i > start && nums[i] == nums[i - 1]) continue;

        current.Add(nums[i]);
        Backtrack(nums, i + 1, current, result);
        current.RemoveAt(current.Count - 1);
    }
}
```

---

#### Bài 6: Combination Sum II (LeetCode 40) — Chọn không lặp + Duplicate

**Đề:** Cho mảng `candidates` (có thể có trùng) và `target`. Tìm tất cả tổ hợp mà tổng bằng `target`. Mỗi số chỉ dùng **một lần**. Không trả về tổ hợp trùng lặp.

**Ví dụ:**
```
Input:  candidates = [10,1,2,7,6,1,5], target = 8
Output: [[1,1,6],[1,2,5],[1,7],[2,6]]

Input:  candidates = [2,5,2,1,2], target = 5
Output: [[1,2,2],[5]]

Input:  candidates = [1], target = 2
Output: []
```

**Constraint:**
- `1 <= candidates.length <= 100`
- `1 <= candidates[i] <= 50`
- `1 <= target <= 30`

**Code C#:**
```csharp
// Optimal — Backtracking + Sort + Skip duplicate: O(2^n) time, O(n) space
public IList<IList<int>> CombinationSum2(int[] candidates, int target)
{
    Array.Sort(candidates);
    var result = new List<IList<int>>();
    Backtrack(candidates, target, 0, new List<int>(), result);
    return result;
}

void Backtrack(int[] candidates, int remain, int start, List<int> current, List<IList<int>> result)
{
    if (remain == 0)
    {
        result.Add(new List<int>(current));
        return;
    }

    for (int i = start; i < candidates.Length; i++)
    {
        if (candidates[i] > remain) break;             // pruning
        if (i > start && candidates[i] == candidates[i - 1]) continue; // skip duplicate cùng cấp

        current.Add(candidates[i]);
        Backtrack(candidates, remain - candidates[i], i + 1, current, result); // i+1: không dùng lại
        current.RemoveAt(current.Count - 1);
    }
}
```

---

#### Bài 7: Word Search (LeetCode 79) — Backtracking trên Grid

**Đề:** Cho lưới ký tự `board` (m×n) và chuỗi `word`. Trả về `true` nếu `word` tồn tại trong lưới theo đường đi liên tiếp (trên, dưới, trái, phải). Mỗi ô chỉ dùng một lần.

**Ví dụ:**
```
Input:  board = [["A","B","C","E"],
                 ["S","F","C","S"],
                 ["A","D","E","E"]], word = "ABCCED"
Output: true

Input:  board = [["A","B","C","E"],
                 ["S","F","C","S"],
                 ["A","D","E","E"]], word = "SEE"
Output: true

Input:  board = [["A","B","C","E"],
                 ["S","F","C","S"],
                 ["A","D","E","E"]], word = "ABCB"
Output: false
Vì: B không thể dùng hai lần
```

**Constraint:**
- `m == board.length`, `n == board[i].length`
- `1 <= m, n <= 6`
- `1 <= word.length <= 15`
- `board` và `word` chỉ chứa ký tự in hoa

**Code C#:**
```csharp
// Optimal — DFS Backtracking trên grid: O(m * n * 4^L) time, O(L) space
// L = word.length
public bool Exist(char[][] board, string word)
{
    int m = board.Length, n = board[0].Length;

    for (int r = 0; r < m; r++)
        for (int c = 0; c < n; c++)
            if (Dfs(board, word, r, c, 0))
                return true;

    return false;
}

bool Dfs(char[][] board, string word, int r, int c, int idx)
{
    if (idx == word.Length) return true; // tìm thấy toàn bộ word

    if (r < 0 || r >= board.Length || c < 0 || c >= board[0].Length)
        return false;
    if (board[r][c] != word[idx]) return false;

    char temp = board[r][c];
    board[r][c] = '#'; // đánh dấu đã thăm (undo sau)

    bool found = Dfs(board, word, r + 1, c, idx + 1)
              || Dfs(board, word, r - 1, c, idx + 1)
              || Dfs(board, word, r, c + 1, idx + 1)
              || Dfs(board, word, r, c - 1, idx + 1);

    board[r][c] = temp; // undo — bỏ đánh dấu
    return found;
}
```

---

### Hard

---

#### Bài 8: N-Queens (LeetCode 51)

**Đề:** Đặt `n` quân hậu trên bàn cờ `n×n` sao cho không có hai quân nào tấn công nhau (không cùng hàng, cột, đường chéo). Trả về tất cả cách đặt hợp lệ.

**Ví dụ:**
```
Input:  n = 4
Output: [[".Q..","...Q","Q...","..Q."],
         ["..Q.","Q...","...Q",".Q.."]]
Vì: có 2 cách đặt 4 quân hậu

Input:  n = 1
Output: [["Q"]]

Input:  n = 3
Output: []
Vì: không tồn tại cách đặt hợp lệ với n = 3
```

**Constraint:**
- `1 <= n <= 9`

**Code C#:**
```csharp
// Optimal — Backtracking theo từng hàng + Set kiểm tra O(1):
// O(n!) time, O(n) space
public IList<string[]> SolveNQueens(int n)
{
    var result = new List<string[]>();
    var cols = new HashSet<int>();
    var diag1 = new HashSet<int>(); // (row - col) không đổi trên đường chéo "\"
    var diag2 = new HashSet<int>(); // (row + col) không đổi trên đường chéo "/"

    int[] queens = new int[n]; // queens[row] = col
    Backtrack(0, n, queens, cols, diag1, diag2, result);
    return result;
}

void Backtrack(int row, int n, int[] queens,
    HashSet<int> cols, HashSet<int> diag1, HashSet<int> diag2,
    List<string[]> result)
{
    if (row == n)
    {
        result.Add(BuildBoard(queens, n));
        return;
    }

    for (int col = 0; col < n; col++)
    {
        if (cols.Contains(col)) continue;
        if (diag1.Contains(row - col)) continue;
        if (diag2.Contains(row + col)) continue;

        queens[row] = col;
        cols.Add(col); diag1.Add(row - col); diag2.Add(row + col);

        Backtrack(row + 1, n, queens, cols, diag1, diag2, result);

        cols.Remove(col); diag1.Remove(row - col); diag2.Remove(row + col);
    }
}

string[] BuildBoard(int[] queens, int n)
{
    var board = new string[n];
    for (int r = 0; r < n; r++)
    {
        var row = new char[n];
        Array.Fill(row, '.');
        row[queens[r]] = 'Q';
        board[r] = new string(row);
    }
    return board;
}
```

---

#### Bài 9: Sudoku Solver (LeetCode 37)

**Đề:** Giải bảng Sudoku 9×9. Ô trống được đánh dấu `'.'`. Điền số 1–9 vào các ô trống sao cho mỗi hàng, cột, và ô 3×3 chứa đủ 1–9.

**Ví dụ:**
```
Input:
  [["5","3",".",".","7",".",".",".","."],
   ["6",".",".","1","9","5",".",".","."],
   [".","9","8",".",".",".",".","6","."],
   ["8",".",".",".","6",".",".",".","3"],
   ["4",".",".","8",".","3",".",".","1"],
   ["7",".",".",".","2",".",".",".","6"],
   [".","6",".",".",".",".","2","8","."],
   [".",".",".","4","1","9",".",".","5"],
   [".",".",".",".","8",".",".","7","9"]]
Output: (bảng đã điền đầy đủ hợp lệ)

Input: board chỉ có một ô trống
Output: board đã điền số đó

Input: board hoàn toàn trống (tất cả '.')
Output: một giải pháp hợp lệ (đề đảm bảo có nghiệm duy nhất)
```

**Constraint:**
- `board.length == 9`, `board[i].length == 9`
- `board[i][j]` là chữ số hoặc `'.'`
- Đề đảm bảo có nghiệm duy nhất

**Code C#:**
```csharp
// Optimal — Backtracking + Pruning với HashSet: O(9^m) time
// m = số ô trống, O(1) space (bảng 9x9 cố định)
public void SolveSudoku(char[][] board)
{
    Solve(board);
}

bool Solve(char[][] board)
{
    for (int r = 0; r < 9; r++)
    {
        for (int c = 0; c < 9; c++)
        {
            if (board[r][c] != '.') continue;

            for (char num = '1'; num <= '9'; num++)
            {
                if (!IsValid(board, r, c, num)) continue;

                board[r][c] = num;
                if (Solve(board)) return true; // nghiệm tìm thấy — dừng
                board[r][c] = '.';            // undo
            }

            return false; // không số nào hợp lệ → backtrack
        }
    }
    return true; // đã điền hết
}

bool IsValid(char[][] board, int row, int col, char num)
{
    int boxRow = (row / 3) * 3;
    int boxCol = (col / 3) * 3;

    for (int i = 0; i < 9; i++)
    {
        if (board[row][i] == num) return false;      // cùng hàng
        if (board[i][col] == num) return false;      // cùng cột
        if (board[boxRow + i / 3][boxCol + i % 3] == num) return false; // cùng ô 3×3
    }
    return true;
}
```

---

## Tổng kết Pattern

| Dấu hiệu đề | Pattern | Kỹ thuật chính |
|-------------|---------|----------------|
| "Liệt kê tất cả tập con" | Subsets | `start` tăng dần |
| "Liệt kê tất cả tổ hợp có tổng = k" | Combinations | `start` + pruning |
| "Liệt kê tất cả hoán vị" | Permutations | `used[]` |
| Có phần tử trùng | + Skip duplicate | Sort + `i > start && nums[i] == nums[i-1]` |
| Backtracking trên grid | DFS + undo ô | Đánh dấu ô `'#'`, undo sau DFS |
| "Đặt từng quân / ô thỏa ràng buộc" | Row-by-row + Set | Set kiểm tra O(1) cho hàng/cột/chéo |

## Tiêu chí hoàn thành
- [ ] Viết được ba khuôn mẫu subsets / permutations / combinations từ trí nhớ.
- [ ] Xử lý đúng duplicate trong cả ba khuôn mẫu.
- [ ] Tin và áp dụng được "leap of faith" khi thiết kế đệ quy.
- [ ] Giải Word Search và N-Queens không nhìn lời giải.
