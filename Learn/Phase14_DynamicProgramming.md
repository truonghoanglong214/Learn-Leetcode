# Phase 14 — Dynamic Programming

> **Ngôn ngữ:** C#
> **Mục tiêu:** Nhận ra **bài toán con chồng lặp**, định nghĩa **trạng thái** và **công thức truy hồi**, đi từ đệ quy → memo (top-down) → bảng (bottom-up) → tối ưu bộ nhớ.

---

## Bài học 1 — Tư duy DP

### 1.1 DP là gì?

Dynamic Programming = **ghi nhớ kết quả bài toán con** để không tính lại.

DP phù hợp khi bài có **2 tính chất**:
1. **Optimal substructure** — đáp án tối ưu của bài lớn được tổng hợp từ đáp án tối ưu bài con.
2. **Overlapping subproblems** — các bài con được tính lại nhiều lần (backtracking thuần gặp tình trạng này).

### 1.2 Quy trình 5 bước

```
1. Định nghĩa STATE — dp[i] (hoặc dp[i][j]...) nghĩa là gì?
2. Viết RECURRENCE — dp[i] tính từ trạng thái nào?
3. Xác định BASE CASE — dp[0], dp[1]... = ?
4. Chọn THỨ TỰ DUYỆT — bottom-up từ nhỏ lên lớn.
5. (Tuỳ chọn) TỐI ƯU BỘ NHỚ — giảm O(n²) → O(n) khi chỉ cần vài hàng/cột trước.
```

> **Thứ tự làm:** Đúng (bảng đầy đủ) → sau đó mới tối ưu bộ nhớ. Đừng tối ưu ngay từ đầu.

### 1.3 Phân biệt DP vs Greedy vs Backtracking

| | Backtracking | DP | Greedy |
|---|---|---|---|
| Chiến lược | Thử mọi khả năng, quay lui | Ghi nhớ kết quả bài con | Chọn tối ưu cục bộ |
| Độ phức tạp | Exponential | Polynomial | Thường O(n log n) |
| Khi dùng | Sinh tất cả nghiệm, n nhỏ | Đếm cách / tối ưu, bài con chồng lặp | Chứng minh được exchange argument |
| Mối quan hệ | DP = Backtracking + Memo | — | Greedy ⊂ DP đặc biệt |

### 1.4 Template Top-down (Memo)

```csharp
// Top-down: đệ quy + ghi nhớ
Dictionary<int, int> memo = new();

int Solve(int i)
{
    if (i <= 1) return i;                    // base case
    if (memo.ContainsKey(i)) return memo[i]; // đã tính
    memo[i] = Solve(i - 1) + Solve(i - 2);  // recurrence
    return memo[i];
}
```

### 1.5 Template Bottom-up (Bảng)

```csharp
// Bottom-up: tính từ nhỏ lên, không đệ quy
int[] dp = new int[n + 1];
dp[0] = 0; dp[1] = 1;                     // base case
for (int i = 2; i <= n; i++)
    dp[i] = dp[i - 1] + dp[i - 2];         // recurrence
return dp[n];
```

---

## Bài học 2 — Các họ DP

### 2.1 DP 1D — phụ thuộc vài trạng thái trước

```
dp[i] = f(dp[i-1], dp[i-2], ...)
```
Ví dụ: Climbing Stairs, House Robber, Coin Change.

### 2.2 DP 2D trên hai chuỗi

```
dp[i][j] = kết quả tốt nhất cho s1[0..i-1] và s2[0..j-1]
```
Ví dụ: LCS, Edit Distance.

### 2.3 Knapsack

**0/1 Knapsack** — mỗi món chọn hoặc không:
```csharp
// Duyệt capacity giảm dần để không dùng lại món
for (int i = 0; i < n; i++)
    for (int w = capacity; w >= weight[i]; w--)
        dp[w] = Math.Max(dp[w], dp[w - weight[i]] + value[i]);
```

**Unbounded Knapsack** — mỗi món dùng bao nhiêu lần cũng được:
```csharp
// Duyệt capacity tăng dần để dùng lại món
for (int i = 0; i < n; i++)
    for (int w = weight[i]; w <= capacity; w++)
        dp[w] = Math.Max(dp[w], dp[w - weight[i]] + value[i]);
```

---

## Vòng lặp tư duy 5 câu hỏi cho DP

1. **Input nói gì?** Chuỗi, mảng, lưới? Có thứ tự không? Các phần tử độc lập hay liên quan?
2. **Output cần gì?** Đếm cách? Tối thiểu/tối đa? Có thể đạt không (boolean)?
3. **Brute Force là gì?** Backtracking thử tất cả → tính lại bài con nhiều lần → thêm memo.
4. **State là gì?** `dp[i]` đại diện cho cái gì? Cần bao nhiêu chiều để mô tả trạng thái đủ?
5. **Recurrence là gì?** dp[i] tính từ dp[i-1], dp[i-2]...? Hay dp[i][j] từ dp[i-1][j], dp[i][j-1]?

---

## Bài tập — Nhóm 1: DP 1D Khởi động

### Bài 1 — Climbing Stairs (70) ⭐ — Bản lề

**Đề:** Có `n` bậc thang. Mỗi bước leo được 1 hoặc 2 bậc. Đếm số cách khác nhau để lên đến bậc `n`.

**Ví dụ:**
```
Input:  n = 2
Output: 2
Vì: [1+1] hoặc [2] — 2 cách.

Input:  n = 3
Output: 3
Vì: [1+1+1], [1+2], [2+1] — 3 cách.

Input:  n = 1
Output: 1
Vì: edge case — chỉ một bậc, chỉ một cách.
```

**Constraint:** `1 ≤ n ≤ 45`.

**Phân tích 5 câu hỏi:**
1. Input: số nguyên n — số bậc.
2. Output: đếm số cách — số nguyên.
3. Brute force: đệ quy thử leo 1 hoặc 2 bậc → `2ⁿ` lời gọi, tính lại cùng trạng thái.
4. Chỗ lãng phí: `ways(n) = ways(n-1) + ways(n-2)` — bài con chồng lặp → memo.
5. Pattern: **DP 1D** giống Fibonacci.

```csharp
// Đệ quy thuần — O(2ⁿ), chỉ để thấy bài con chồng lặp
public int ClimbStairs_BruteForce(int n)
{
    if (n <= 1) return 1;
    return ClimbStairs_BruteForce(n - 1) + ClimbStairs_BruteForce(n - 2);
}

// Top-down (Memo) — O(n) time, O(n) space
public int ClimbStairs_Memo(int n)
{
    var memo = new int[n + 1];
    return Solve(n, memo);

    int Solve(int k, int[] m)
    {
        if (k <= 1) return 1;
        if (m[k] != 0) return m[k];
        return m[k] = Solve(k - 1, m) + Solve(k - 2, m);
    }
}

// Bottom-up (Bảng) — O(n) time, O(n) space
public int ClimbStairs_Table(int n)
{
    if (n <= 1) return 1;
    int[] dp = new int[n + 1];
    dp[0] = 1; dp[1] = 1;
    for (int i = 2; i <= n; i++)
        dp[i] = dp[i - 1] + dp[i - 2];
    return dp[n];
}

// Tối ưu bộ nhớ — O(n) time, O(1) space
public int ClimbStairs(int n)
{
    if (n <= 1) return 1;
    int prev2 = 1, prev1 = 1;
    for (int i = 2; i <= n; i++)
        (prev2, prev1) = (prev1, prev1 + prev2);
    return prev1;
}
```

> **Bài học cốt lõi:** Đây là lần đầu bạn thấy đường đi đệ quy → memo → bảng → tối ưu bộ nhớ. Hiểu rõ từng bước trước khi tiếp tục.

---

### Bài 2 — House Robber (198) ⭐⭐

**Đề:** Mảng `nums[i]` = tiền trong nhà thứ `i`. Không được cướp 2 nhà liền kề. Tìm số tiền **tối đa** có thể cướp.

**Ví dụ:**
```
Input:  nums = [1,2,3,1]
Output: 4
Vì: cướp nhà 0 (1) + nhà 2 (3) = 4.

Input:  nums = [2,7,9,3,1]
Output: 12
Vì: cướp nhà 0 (2) + nhà 2 (9) + nhà 4 (1) = 12.

Input:  nums = [1]
Output: 1
Vì: edge case — một nhà.
```

**Constraint:** `1 ≤ n ≤ 100`, `0 ≤ nums[i] ≤ 400`.

**Phân tích 5 câu hỏi:**
1. Mảng số không âm, không sắp xếp.
2. Tối đa — số nguyên.
3. Brute force: thử mọi tập con không liền kề → O(2ⁿ).
4. Tại nhà `i`: hoặc cướp `nums[i] + dp[i-2]`, hoặc bỏ qua `dp[i-1]`.
5. Pattern: **DP 1D — chọn/không chọn**.

```csharp
public int Rob(int[] nums)
{
    if (nums.Length == 1) return nums[0];

    // dp[i] = tiền tối đa nếu xét đến nhà i
    int prev2 = nums[0], prev1 = Math.Max(nums[0], nums[1]);
    for (int i = 2; i < nums.Length; i++)
    {
        int curr = Math.Max(prev1, prev2 + nums[i]); // bỏ qua hoặc cướp nhà i
        prev2 = prev1;
        prev1 = curr;
    }
    return prev1;
    // T: O(n), S: O(1)
}
```

---

### Bài 3 — House Robber II (213) ⭐⭐

**Đề:** Như House Robber nhưng các nhà xếp thành **vòng tròn** (nhà đầu và cuối liền kề). Tìm tiền tối đa.

**Ví dụ:**
```
Input:  nums = [2,3,2]
Output: 3
Vì: không thể cướp [0] và [2] cùng lúc (liền kề vòng). Tốt nhất: nhà 1 = 3.

Input:  nums = [1,2,3,1]
Output: 4
Vì: nhà 0 (1) + nhà 2 (3) = 4.

Input:  nums = [1]
Output: 1
Vì: edge case — một nhà.
```

**Constraint:** `1 ≤ n ≤ 100`, `0 ≤ nums[i] ≤ 1000`.

```csharp
public int Rob2(int[] nums)
{
    if (nums.Length == 1) return nums[0];
    if (nums.Length == 2) return Math.Max(nums[0], nums[1]);

    // Vòng tròn → không thể cướp cả đầu lẫn cuối
    // Chia thành 2 bài House Robber thường:
    // - nums[0..n-2] (bỏ nhà cuối)
    // - nums[1..n-1] (bỏ nhà đầu)
    return Math.Max(RobLinear(nums, 0, nums.Length - 2),
                    RobLinear(nums, 1, nums.Length - 1));
}

int RobLinear(int[] nums, int lo, int hi)
{
    int prev2 = 0, prev1 = 0;
    for (int i = lo; i <= hi; i++)
    {
        int curr = Math.Max(prev1, prev2 + nums[i]);
        prev2 = prev1;
        prev1 = curr;
    }
    return prev1;
    // T: O(n), S: O(1)
}
```

> **Kỹ thuật:** "Bài vòng tròn = max(bài tuyến tính bỏ đầu, bài tuyến tính bỏ cuối)" — áp dụng được nhiều bài dạng vòng.

---

### Bài 4 — Coin Change (322) ⭐⭐ — Bản lề

**Đề:** Cho mảng `coins` và số `amount`. Tìm số đồng xu **ít nhất** để tổng thành `amount`. Nếu không thể, trả về `-1`. Mỗi đồng có thể dùng bất kỳ số lần nào.

**Ví dụ:**
```
Input:  coins = [1,5,11], amount = 15
Output: 3
Vì: 5+5+5 = 3 đồng.

Input:  coins = [2], amount = 3
Output: -1
Vì: không tạo được 3 từ các đồng 2.

Input:  coins = [1], amount = 0
Output: 0
Vì: edge case — amount = 0, không cần đồng nào.
```

**Constraint:** `1 ≤ coins.length ≤ 12`, `1 ≤ coins[i] ≤ 2³¹-1`, `0 ≤ amount ≤ 10⁴`.

**Phân tích 5 câu hỏi:**
1. Mảng đồng xu (không sắp xếp), một số amount.
2. Tối thiểu — số nguyên hoặc -1.
3. Brute force: đệ quy thử từng đồng → O(Sⁿ) với S = lượng đồng xu.
4. Chỗ lãng phí: `dp[amount]` chỉ cần tính một lần từ các `dp[amount - coin]` đã biết.
5. Pattern: **Unbounded Knapsack — tối thiểu**.

```csharp
// Brute force đệ quy — chỉ để minh hoạ, TLE
public int CoinChange_BruteForce(int[] coins, int amount)
{
    if (amount == 0) return 0;
    int res = int.MaxValue;
    foreach (int c in coins)
        if (c <= amount)
        {
            int sub = CoinChange_BruteForce(coins, amount - c);
            if (sub != -1) res = Math.Min(res, sub + 1);
        }
    return res == int.MaxValue ? -1 : res;
}

// Bottom-up DP — O(amount × n) time, O(amount) space
public int CoinChange(int[] coins, int amount)
{
    int[] dp = new int[amount + 1];
    Array.Fill(dp, amount + 1); // sentinel "vô cực"
    dp[0] = 0;

    for (int a = 1; a <= amount; a++)
        foreach (int c in coins)
            if (c <= a)
                dp[a] = Math.Min(dp[a], dp[a - c] + 1);

    return dp[amount] > amount ? -1 : dp[amount];
}
```

> **Tại sao `amount + 1` làm sentinel?** Số đồng tối đa không bao giờ vượt `amount` (dùng toàn đồng 1), nên `amount + 1` đảm bảo là "vô cực" hợp lệ mà không tràn số.

---

## Bài tập — Nhóm 2: Chuỗi / Dãy con

### Bài 5 — Longest Increasing Subsequence (300) ⭐⭐⭐ — Bản lề

**Đề:** Cho mảng `nums`. Tìm độ dài **dãy con tăng dần dài nhất** (các phần tử không cần liên tiếp).

**Ví dụ:**
```
Input:  nums = [10,9,2,5,3,7,101,18]
Output: 4
Vì: [2,3,7,101] hoặc [2,5,7,101] — dài nhất 4.

Input:  nums = [0,1,0,3,2,3]
Output: 4
Vì: [0,1,2,3].

Input:  nums = [7,7,7,7]
Output: 1
Vì: edge case — tất cả bằng nhau, dãy tăng dần dài nhất = 1.
```

**Constraint:** `1 ≤ n ≤ 2500`, `-10⁴ ≤ nums[i] ≤ 10⁴`.

**Phân tích 5 câu hỏi:**
1. Mảng số nguyên, không sắp xếp.
2. Độ dài tối đa — số nguyên.
3. Brute force: với mỗi phần tử, thử chọn hay bỏ → O(2ⁿ).
4. Chỗ lãng phí: `dp[i]` = LIS kết thúc tại `i` → chỉ cần xét các `j < i` mà `nums[j] < nums[i]`.
5. Pattern: **DP 1D — "kết thúc tại i"**.

```csharp
// DP O(n²) — đủ cho n ≤ 2500
public int LengthOfLIS(int[] nums)
{
    int n = nums.Length;
    int[] dp = new int[n];
    Array.Fill(dp, 1); // mỗi phần tử là dãy dài 1

    int ans = 1;
    for (int i = 1; i < n; i++)
    {
        for (int j = 0; j < i; j++)
            if (nums[j] < nums[i])
                dp[i] = Math.Max(dp[i], dp[j] + 1);
        ans = Math.Max(ans, dp[i]);
    }
    return ans;
    // T: O(n²), S: O(n)
}

// Tối ưu O(n log n) — Binary Search + tails array
public int LengthOfLIS_Optimal(int[] nums)
{
    var tails = new List<int>(); // tails[i] = phần tử nhỏ nhất kết thúc LIS độ dài i+1

    foreach (int x in nums)
    {
        int lo = 0, hi = tails.Count;
        while (lo < hi)
        {
            int mid = (lo + hi) / 2;
            if (tails[mid] < x) lo = mid + 1;
            else hi = mid;
        }
        if (lo == tails.Count) tails.Add(x);
        else tails[lo] = x;
    }
    return tails.Count;
    // T: O(n log n), S: O(n)
}
```

> **Cách đọc `tails`:** Đây không phải là LIS thực sự mà là mảng giúp đếm độ dài. Phần tử `tails[i]` là phần tử **nhỏ nhất có thể** làm phần tử cuối của dãy tăng dài `i+1` — bằng binary search tìm vị trí thay thế.

---

### Bài 6 — Longest Common Subsequence (1143) ⭐⭐⭐ — Bản lề

**Đề:** Cho hai chuỗi `text1` và `text2`. Tìm độ dài **dãy con chung dài nhất** (không cần liên tiếp).

**Ví dụ:**
```
Input:  text1 = "abcde", text2 = "ace"
Output: 3
Vì: "ace" là dãy con chung dài nhất.

Input:  text1 = "abc", text2 = "abc"
Output: 3
Vì: toàn bộ chuỗi.

Input:  text1 = "abc", text2 = "def"
Output: 0
Vì: edge case — không có ký tự chung.
```

**Constraint:** `1 ≤ m, n ≤ 1000`, chỉ gồm chữ thường.

**Phân tích 5 câu hỏi:**
1. Hai chuỗi, không sắp xếp.
2. Độ dài tối đa — số nguyên.
3. Brute force: sinh mọi dãy con của text1, kiểm tra xem có phải dãy con của text2 không → O(2ᵐ × n).
4. Tại mỗi cặp (i, j): nếu `text1[i] == text2[j]` → thêm 1 vào `dp[i-1][j-1]`; nếu không → max(bỏ text1[i], bỏ text2[j]).
5. Pattern: **DP 2D — hai chuỗi**.

```csharp
public int LongestCommonSubsequence(string text1, string text2)
{
    int m = text1.Length, n = text2.Length;
    // dp[i][j] = LCS của text1[0..i-1] và text2[0..j-1]
    int[,] dp = new int[m + 1, n + 1];

    for (int i = 1; i <= m; i++)
        for (int j = 1; j <= n; j++)
            if (text1[i - 1] == text2[j - 1])
                dp[i, j] = dp[i - 1, j - 1] + 1;      // ký tự khớp
            else
                dp[i, j] = Math.Max(dp[i - 1, j], dp[i, j - 1]); // bỏ một bên

    return dp[m, n];
    // T: O(m×n), S: O(m×n) — có thể tối ưu xuống O(n)
}
```

> **Bảng dp trực quan** (text1="abc", text2="ac"):
> ```
>     ""  a  c
> ""   0  0  0
>  a   0  1  1
>  b   0  1  1
>  c   0  1  2
> ```

---

### Bài 7 — Edit Distance (72) ⭐⭐⭐

**Đề:** Cho hai chuỗi `word1` và `word2`. Tìm số thao tác **ít nhất** (chèn, xóa, thay thế) để biến `word1` thành `word2`.

**Ví dụ:**
```
Input:  word1 = "horse", word2 = "ros"
Output: 3
Vì: horse→rorse (thay h→r) →rose (xóa r) →ros (xóa e).

Input:  word1 = "intention", word2 = "execution"
Output: 5
Vì: 5 thao tác.

Input:  word1 = "", word2 = "abc"
Output: 3
Vì: edge case — word1 rỗng, chèn 3 ký tự.
```

**Constraint:** `0 ≤ m, n ≤ 500`.

```csharp
public int MinDistance(string word1, string word2)
{
    int m = word1.Length, n = word2.Length;
    // dp[i][j] = edit distance giữa word1[0..i-1] và word2[0..j-1]
    int[,] dp = new int[m + 1, n + 1];

    // Base case: một chuỗi rỗng
    for (int i = 0; i <= m; i++) dp[i, 0] = i; // xóa i ký tự
    for (int j = 0; j <= n; j++) dp[0, j] = j; // chèn j ký tự

    for (int i = 1; i <= m; i++)
        for (int j = 1; j <= n; j++)
            if (word1[i - 1] == word2[j - 1])
                dp[i, j] = dp[i - 1, j - 1];           // ký tự khớp — không tốn thao tác
            else
                dp[i, j] = 1 + Math.Min(dp[i - 1, j - 1], // thay thế
                                Math.Min(dp[i - 1, j],      // xóa từ word1
                                         dp[i, j - 1]));    // chèn vào word1
    return dp[m, n];
    // T: O(m×n), S: O(m×n)
}
```

> **Cách nhớ ba lựa chọn:** nhìn vào ô `dp[i][j]`:
> - Từ `dp[i-1][j-1]` = thay thế (hoặc khớp).
> - Từ `dp[i-1][j]` = xóa `word1[i]`.
> - Từ `dp[i][j-1]` = chèn `word2[j]` vào word1.

---

### Bài 8 — Word Break (139) ⭐⭐⭐

**Đề:** Cho chuỗi `s` và danh sách từ `wordDict`. Hỏi có thể tách `s` thành các từ trong `wordDict` không?

**Ví dụ:**
```
Input:  s = "leetcode", wordDict = ["leet","code"]
Output: true
Vì: "leet" + "code".

Input:  s = "applepenapple", wordDict = ["apple","pen"]
Output: true
Vì: "apple" + "pen" + "apple".

Input:  s = "catsandog", wordDict = ["cats","dog","sand","and","cat"]
Output: false
Vì: edge case — không tách được hoàn toàn.
```

**Constraint:** `1 ≤ s.length ≤ 300`, `1 ≤ wordDict.length ≤ 1000`.

```csharp
public bool WordBreak(string s, IList<string> wordDict)
{
    var wordSet = new HashSet<string>(wordDict);
    int n = s.Length;
    // dp[i] = true nếu s[0..i-1] có thể tách thành các từ hợp lệ
    bool[] dp = new bool[n + 1];
    dp[0] = true; // chuỗi rỗng luôn hợp lệ

    for (int i = 1; i <= n; i++)
        for (int j = 0; j < i; j++)
            if (dp[j] && wordSet.Contains(s.Substring(j, i - j)))
            {
                dp[i] = true;
                break; // tìm được một cách tách tại i
            }

    return dp[n];
    // T: O(n² × L) với L = độ dài từ trung bình, S: O(n + W)
}
```

---

## Bài tập — Nhóm 3: Lưới 2D

### Bài 9 — Unique Paths (62) ⭐⭐

**Đề:** Robot ở góc trên-trái của lưới `m × n`. Chỉ đi **phải** hoặc **xuống**. Đếm số đường đi khác nhau đến góc dưới-phải.

**Ví dụ:**
```
Input:  m = 3, n = 7
Output: 28

Input:  m = 3, n = 2
Output: 3
Vì: [phải→xuống→xuống], [xuống→phải→xuống], [xuống→xuống→phải].

Input:  m = 1, n = 1
Output: 1
Vì: edge case — đã ở đích.
```

**Constraint:** `1 ≤ m, n ≤ 100`.

```csharp
public int UniquePaths(int m, int n)
{
    // dp[i][j] = số đường đến ô (i,j)
    int[,] dp = new int[m, n];

    // Base case: hàng đầu và cột đầu chỉ có 1 cách
    for (int i = 0; i < m; i++) dp[i, 0] = 1;
    for (int j = 0; j < n; j++) dp[0, j] = 1;

    for (int i = 1; i < m; i++)
        for (int j = 1; j < n; j++)
            dp[i, j] = dp[i - 1, j] + dp[i, j - 1]; // từ trên + từ trái

    return dp[m - 1, n - 1];
    // T: O(m×n), S: O(m×n) — tối ưu xuống O(n) bằng 1 hàng
}
```

---

### Bài 10 — Minimum Path Sum (64) ⭐⭐

**Đề:** Lưới `m × n` chứa số không âm. Tìm đường từ góc trên-trái đến góc dưới-phải (chỉ đi phải/xuống) có **tổng nhỏ nhất**.

**Ví dụ:**
```
Input:  grid = [[1,3,1],[1,5,1],[4,2,1]]
Output: 7
Vì: 1→3→1→1→1 = 7.

Input:  grid = [[1,2,3],[4,5,6]]
Output: 12
Vì: 1→2→3→6 = 12.

Input:  grid = [[5]]
Output: 5
Vì: edge case — lưới 1×1.
```

**Constraint:** `1 ≤ m, n ≤ 200`, `0 ≤ grid[i][j] ≤ 200`.

```csharp
public int MinPathSum(int[][] grid)
{
    int m = grid.Length, n = grid[0].Length;
    // Tối ưu in-place: dùng luôn grid làm bảng dp
    for (int i = 0; i < m; i++)
        for (int j = 0; j < n; j++)
        {
            if (i == 0 && j == 0) continue;
            int fromTop  = i > 0 ? grid[i - 1][j] : int.MaxValue;
            int fromLeft = j > 0 ? grid[i][j - 1] : int.MaxValue;
            grid[i][j] += Math.Min(fromTop, fromLeft);
        }
    return grid[m - 1][n - 1];
    // T: O(m×n), S: O(1) nếu chỉnh in-place
}
```

---

## Bài tập — Nhóm 4: Knapsack & Phân hoạch

### Bài 11 — Partition Equal Subset Sum (416) ⭐⭐⭐ — Bản lề

**Đề:** Cho mảng `nums`. Hỏi có thể chia thành **hai phần có tổng bằng nhau** không?

**Ví dụ:**
```
Input:  nums = [1,5,11,5]
Output: true
Vì: [1,5,5] và [11].

Input:  nums = [1,2,3,5]
Output: false
Vì: không chia được.

Input:  nums = [1,1]
Output: true
Vì: edge case — hai phần tử bằng nhau.
```

**Constraint:** `1 ≤ n ≤ 200`, `1 ≤ nums[i] ≤ 100`.

**Phân tích 5 câu hỏi:**
1. Mảng số dương.
2. Boolean — có thể chia không.
3. Brute force: thử mọi tập con, kiểm tra tổng = total/2 → O(2ⁿ).
4. Quy về: "có tập con nào có tổng = total/2?" → 0/1 Knapsack boolean.
5. Pattern: **0/1 Knapsack — tìm tổng mục tiêu**.

```csharp
public bool CanPartition(int[] nums)
{
    int total = nums.Sum();
    if (total % 2 != 0) return false; // tổng lẻ → không chia được

    int target = total / 2;
    // dp[w] = true nếu có thể tạo ra tổng w từ tập con của nums
    bool[] dp = new bool[target + 1];
    dp[0] = true;

    foreach (int num in nums)
        // Duyệt ngược để không dùng lại num (0/1 knapsack)
        for (int w = target; w >= num; w--)
            dp[w] = dp[w] || dp[w - num];

    return dp[target];
    // T: O(n × target), S: O(target)
}
```

> **Tại sao duyệt ngược?** Nếu duyệt xuôi, `dp[w - num]` đã được cập nhật trong vòng lặp này → `num` có thể được dùng nhiều lần (unbounded). Duyệt ngược đảm bảo mỗi `num` chỉ dùng một lần.

---

### Bài 12 — Coin Change II (518) ⭐⭐⭐

**Đề:** Cho mảng `coins` và số `amount`. Đếm số cách khác nhau để tạo ra `amount`. Mỗi đồng dùng bao nhiêu lần cũng được.

**Ví dụ:**
```
Input:  coins = [1,2,5], amount = 5
Output: 4
Vì: 5; 2+2+1; 2+1+1+1; 1+1+1+1+1.

Input:  coins = [2], amount = 3
Output: 0
Vì: không tạo được.

Input:  coins = [10], amount = 10
Output: 1
Vì: edge case — chỉ một cách.
```

**Constraint:** `1 ≤ coins.length ≤ 300`, `0 ≤ amount ≤ 5000`.

```csharp
public int Change(int amount, int[] coins)
{
    // dp[w] = số cách tạo tổng w
    int[] dp = new int[amount + 1];
    dp[0] = 1; // 1 cách tạo tổng 0: không chọn gì

    // Duyệt coins trước, amount sau → mỗi tổ hợp (không phải hoán vị)
    foreach (int coin in coins)
        for (int w = coin; w <= amount; w++)
            dp[w] += dp[w - coin];

    return dp[amount];
    // T: O(n × amount), S: O(amount)
}
```

> **Tại sao duyệt coins ngoài, amount trong?** Nếu đổi thứ tự (amount ngoài, coins trong) sẽ đếm **hoán vị** (1+2 và 2+1 tính là 2 cách). Duyệt coins ngoài đảm bảo mỗi coin chỉ được "giới thiệu" một lần → đếm **tổ hợp**.

---

## Bài tập — Nhóm 5: Hard

### Bài 13 — Best Time to Buy and Sell Stock with Cooldown (309) ⭐⭐⭐⭐

**Đề:** Mảng `prices[i]` = giá cổ phiếu ngày `i`. Có thể mua-bán bao nhiêu lần, nhưng sau khi bán phải **nghỉ 1 ngày** (cooldown) mới được mua lại. Tìm lợi nhuận tối đa.

**Ví dụ:**
```
Input:  prices = [1,2,3,0,2]
Output: 3
Vì: mua(0)→bán(1)→cooldown→mua(0)→bán(2) = 1+2 = 3.

Input:  prices = [1]
Output: 0
Vì: edge case — một ngày, không làm gì.

Input:  prices = [1,2]
Output: 1
Vì: mua(1)→bán(2) = 1.
```

**Constraint:** `1 ≤ n ≤ 5000`, `0 ≤ prices[i] ≤ 1000`.

**Phân tích 5 câu hỏi:**
1. Mảng giá, không âm.
2. Lợi nhuận tối đa — số nguyên.
3. Brute force: thử mọi tập mua-bán → O(2ⁿ).
4. Ba trạng thái mỗi ngày: **Hold** (đang nắm cổ phiếu), **Sold** (vừa bán xong), **Rest** (đang nghỉ / không có gì).
5. Pattern: **DP máy trạng thái**.

```csharp
public int MaxProfit(int[] prices)
{
    // Ba trạng thái:
    // hold = lợi nhuận tối đa khi đang nắm cổ phiếu
    // sold = lợi nhuận tối đa ngày vừa bán (→ hôm sau phải rest)
    // rest = lợi nhuận tối đa khi không nắm, không vừa bán (có thể mua hôm nay)
    int hold = int.MinValue, sold = 0, rest = 0;

    foreach (int price in prices)
    {
        int prevHold = hold, prevSold = sold, prevRest = rest;

        hold = Math.Max(prevHold, prevRest - price); // giữ tiếp hoặc mua (từ rest)
        sold = prevHold + price;                      // bán hôm nay (đang hold)
        rest = Math.Max(prevRest, prevSold);           // nghỉ: giữ rest hoặc qua cooldown

        // T: O(n), S: O(1)
    }
    return Math.Max(sold, rest); // không nắm cổ phiếu ở cuối
}
```

> **Sơ đồ máy trạng thái:**
> ```
> rest ──(mua)──► hold ──(bán)──► sold
>  ▲                               │
>  └──────────(cooldown)──────────┘
>  ↕ (tự chuyển)
> ```

---

### Bài 14 — Regular Expression Matching (10) ⭐⭐⭐⭐⭐

**Đề:** Implement khớp regex đơn giản với `.` (khớp mọi ký tự) và `*` (0 hoặc nhiều ký tự trước). Hỏi `s` có khớp với pattern `p` không?

**Ví dụ:**
```
Input:  s = "aa", p = "a*"
Output: true
Vì: a* = 0 hoặc nhiều 'a'.

Input:  s = "ab", p = ".*"
Output: true
Vì: .* khớp mọi chuỗi.

Input:  s = "aab", p = "c*a*b"
Output: true
Vì: c* = 0 'c', a* = 2 'a', b = 'b'.
```

**Constraint:** `1 ≤ s.length ≤ 20`, `1 ≤ p.length ≤ 30`.

```csharp
public bool IsMatch(string s, string p)
{
    int m = s.Length, n = p.Length;
    // dp[i][j] = s[0..i-1] có khớp p[0..j-1] không
    bool[,] dp = new bool[m + 1, n + 1];
    dp[0, 0] = true;

    // Base case: pattern như "a*b*c*" khớp được chuỗi rỗng
    for (int j = 2; j <= n; j += 2)
        if (p[j - 1] == '*') dp[0, j] = dp[0, j - 2];

    for (int i = 1; i <= m; i++)
        for (int j = 1; j <= n; j++)
        {
            char sc = s[i - 1], pc = p[j - 1];
            if (pc == '*')
            {
                char prev = p[j - 2];
                bool charMatch = prev == '.' || prev == sc;
                dp[i, j] = dp[i, j - 2]                        // dùng 0 lần
                         || (charMatch && dp[i - 1, j]);        // dùng >=1 lần
            }
            else if (pc == '.' || pc == sc)
                dp[i, j] = dp[i - 1, j - 1];                   // ký tự khớp
            // else dp[i,j] = false (mặc định)
        }

    return dp[m, n];
    // T: O(m×n), S: O(m×n)
}
```

---

### Bài 15 — Burst Balloons (312) ⭐⭐⭐⭐⭐

**Đề:** Có `n` bóng bay, bóng thứ `i` có giá trị `nums[i]`. Nổ bóng `i` được `nums[i-1] × nums[i] × nums[i+1]` điểm. Hỏi điểm tối đa khi nổ hết?

**Ví dụ:**
```
Input:  nums = [3,1,5,8]
Output: 167
Vì: nổ [3,1,5,8] theo thứ tự 1,5,3,8: 3×1×5=15 + 3×5×8=120 + 1×3×8=24 + 1×8×1=8 = 167.

Input:  nums = [1,5]
Output: 10
Vì: nổ 1 trước: 1×1×5=5, nổ 5: 1×5×1=5 → tổng 10. Hoặc nổ 5 trước: 1×5×1=5, nổ 1: 1×1×1=1 → 6. Optimal = 10.

Input:  nums = [1]
Output: 1
Vì: edge case — một bóng, 1×1×1=1.
```

**Constraint:** `1 ≤ n ≤ 300`, `0 ≤ nums[i] ≤ 100`.

```csharp
public int MaxCoins(int[] nums)
{
    // Thêm biên 1 ở hai phía
    int n = nums.Length;
    int[] arr = new int[n + 2];
    arr[0] = arr[n + 1] = 1;
    for (int i = 0; i < n; i++) arr[i + 1] = nums[i];

    // dp[i][j] = điểm tối đa khi nổ hết bóng trong khoảng (i, j) mở
    int[,] dp = new int[n + 2, n + 2];

    // len = số bóng trong khoảng
    for (int len = 1; len <= n; len++)
        for (int left = 1; left <= n - len + 1; left++)
        {
            int right = left + len - 1;
            // k là bóng cuối cùng bị nổ trong (left-1, right+1)
            for (int k = left; k <= right; k++)
                dp[left, right] = Math.Max(dp[left, right],
                    dp[left, k - 1] + arr[left - 1] * arr[k] * arr[right + 1] + dp[k + 1, right]);
        }

    return dp[1, n];
    // T: O(n³), S: O(n²)
}
```

> **Tư duy then chốt:** Thay vì nghĩ "nổ đầu tiên" (khó — không biết láng giềng sau này), hãy nghĩ **"nổ cuối cùng trong khoảng (i,j)"**. Khi k nổ cuối cùng trong khoảng, láng giềng của k lúc đó chính là biên khoảng — ta biết chính xác điểm.

---

## Tiêu chí hoàn thành Phase 14

- [ ] Đi được mạch đệ quy → memo → bảng cho ít nhất 5 bài (bắt đầu từ Climbing Stairs).
- [ ] Tự định nghĩa state + recurrence cho bài Medium mới mà không nhìn lời giải.
- [ ] Phân biệt ngay 1D vs 2D vs Knapsack vs LCS qua dấu hiệu đề.
- [ ] Giải thích được tại sao Coin Change II duyệt coins ngoài (đếm tổ hợp) vs amount ngoài (đếm hoán vị).
- [ ] Nhận ra bài nào là DP, bài nào là Greedy, bài nào là Backtracking qua dấu hiệu đề.
