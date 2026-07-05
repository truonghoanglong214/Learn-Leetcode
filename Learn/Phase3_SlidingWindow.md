# Phase 3 — Sliding Window

> **Ngôn ngữ:** C#
> **Mục tiêu:** Giải mọi bài về **đoạn con / chuỗi con liên tiếp** — dài nhất, ngắn nhất, có/không trùng, thỏa điều kiện tần suất. Biến O(n²) → O(n) bằng cửa sổ trượt.

---

## Bài học 1 — Cửa Sổ Cố Định (Fixed Window)

### 1.1 Ý tưởng

```
Kích thước cửa sổ = k (cho trước).
Trượt cửa sổ từ trái sang phải, mỗi bước:
  - Thêm phần tử mới vào phải cửa sổ
  - Loại phần tử cũ ra khỏi trái cửa sổ

Cập nhật incremental thay vì tính lại từ đầu → O(n) thay vì O(n*k).
```

### 1.2 Template Fixed Window

```csharp
// Khởi tạo cửa sổ đầu tiên [0, k-1]
int windowSum = 0;
for (int i = 0; i < k; i++)
    windowSum += nums[i];

int result = windowSum;

// Trượt cửa sổ từ i = k đến n-1
for (int i = k; i < nums.Length; i++)
{
    windowSum += nums[i];         // thêm phần tử mới vào phải
    windowSum -= nums[i - k];     // bỏ phần tử cũ ở trái
    result = Math.Max(result, windowSum);
}
```

---

## Bài học 2 — Cửa Sổ Co Giãn (Variable Window)

### 2.1 Ý tưởng

```
Cửa sổ [left, right] — không cố định kích thước.
Chiến lược:
  1. Mở rộng right (thêm phần tử) → cửa sổ "tham lam" hơn
  2. Khi vi phạm điều kiện → thu hẹp left cho đến khi hợp lệ
  3. Cập nhật kết quả (dài nhất: sau mở rộng; ngắn nhất: sau khi hợp lệ)

Điều kiện bắt buộc: điều kiện hợp lệ có tính đơn điệu.
  → Mở rộng window làm "tệ hơn" (hoặc giữ nguyên)
  → Thu hẹp window làm "tốt hơn" (hoặc giữ nguyên)
```

### 2.2 Template Variable Window

```csharp
int left = 0;
// Trạng thái cửa sổ (count, map, set...)

for (int right = 0; right < s.Length; right++)
{
    // Bước 1: thêm s[right] vào cửa sổ (cập nhật trạng thái)
    Add(s[right]);

    // Bước 2: thu hẹp nếu vi phạm điều kiện
    while (/* cửa sổ không hợp lệ */)
    {
        Remove(s[left]);
        left++;
    }

    // Bước 3: cập nhật kết quả (cửa sổ hiện tại hợp lệ)
    result = Math.Max(result, right - left + 1);
}
```

### 2.3 Cửa sổ ngắn nhất (thu hẹp khi hợp lệ)

```csharp
int left = 0;
int result = int.MaxValue;

for (int right = 0; right < s.Length; right++)
{
    Add(s[right]);

    // Khi đã hợp lệ → ghi kết quả rồi thử thu nhỏ thêm
    while (/* cửa sổ hợp lệ */)
    {
        result = Math.Min(result, right - left + 1);
        Remove(s[left]);
        left++;
    }
}
```

---

## Bài học 3 — Window + Frequency Map

### 3.1 Theo dõi nội dung cửa sổ

```csharp
// Đếm số ký tự unique / số loại trong cửa sổ
var window = new Dictionary<char, int>();
int distinct = 0; // số loại khác nhau trong cửa sổ

// Thêm c vào cửa sổ
void Add(char c)
{
    window[c] = window.GetValueOrDefault(c, 0) + 1;
    if (window[c] == 1) distinct++; // lần đầu xuất hiện
}

// Xóa c khỏi cửa sổ
void Remove(char c)
{
    window[c]--;
    if (window[c] == 0) distinct--; // biến mất khỏi cửa sổ
}
```

### 3.2 So khớp tần suất (dùng cho Permutation in String)

```csharp
// Kiểm tra cửa sổ có khớp tần suất cần thiết không
// Dùng biến 'matched' = số ký tự đã đạt đúng số lượng yêu cầu
int matched = 0; // số ký tự đang khớp đúng số lượng
int required = /* số ký tự cần match */;

// Khi thêm: nếu số lượng vừa đạt đúng → matched++
// Khi xóa:  nếu số lượng vừa thiếu → matched--
// Khi matched == required → cửa sổ hợp lệ
```

### 3.3 Mẹo: "Đúng K" = "Nhiều nhất K" − "Nhiều nhất K−1"

```
Bài hỏi "đúng k loại" thường khó xây window trực tiếp.
Mẹo: AtMost(k) - AtMost(k-1) cho ra kết quả "đúng k".

AtMost(k) = số subarray có nhiều nhất k loại khác nhau.
→ Dễ làm bằng variable window vì điều kiện đơn điệu.
```

---

## Bài tập theo thứ tự

### Easy

---

#### Bài 1: Best Time to Buy and Sell Stock (LeetCode 121) — Window ngầm

**Đề:** Cho `int[] prices` (giá cổ phiếu theo ngày). Chọn một ngày mua và một ngày bán (sau ngày mua). Trả về lợi nhuận tối đa. Nếu không có lợi → trả về 0.

**Ví dụ:**
```
Input:  prices = [7, 1, 5, 3, 6, 4]
Output: 5
Vì: mua ngày 2 (giá 1), bán ngày 5 (giá 6), lợi nhuận = 6 - 1 = 5.

Input:  prices = [7, 6, 4, 3, 1]
Output: 0
Vì: giá chỉ giảm, không có giao dịch nào có lãi.

Input:  prices = [2, 4, 1]
Output: 2
(Edge case: mua đầu → bán giữa là tốt nhất.)

Input:  prices = [1]
Output: 0
(Edge case: chỉ có một ngày, không thể mua và bán.)
```

**Constraint:** `1 <= prices.length <= 10^5`, `0 <= prices[i] <= 10^4`.

**Phân tích 5 câu hỏi:**
1. Input: n ≤ 10⁵, phải mua trước bán sau
2. Output: lợi nhuận tối đa (int)
3. Brute: mọi cặp (i, j) với i < j → O(n²)
4. Lãng phí: với mỗi ngày bán `j`, chỉ cần giá mua nhỏ nhất trong [0..j-1] → track `minPrice` khi duyệt
5. Pattern: **Cửa sổ ngầm** — left = ngày mua nhỏ nhất đã thấy, right = ngày bán hiện tại

```csharp
// Brute — O(n²)
public int MaxProfitBrute(int[] prices)
{
    int max = 0;
    for (int i = 0; i < prices.Length; i++)
        for (int j = i + 1; j < prices.Length; j++)
            max = Math.Max(max, prices[j] - prices[i]);
    return max;
}

// Optimal — O(n) time, O(1) space
public int MaxProfit(int[] prices)
{
    int minPrice = int.MaxValue;
    int maxProfit = 0;

    foreach (int price in prices)
    {
        if (price < minPrice)
            minPrice = price;         // cập nhật ngày mua rẻ nhất
        else
            maxProfit = Math.Max(maxProfit, price - minPrice); // thử bán hôm nay
    }
    return maxProfit;
}
// Time: O(n), Space: O(1)
```

**Liên hệ sliding window:** `minPrice` chính là "biên trái" của cửa sổ (ngày mua). Khi `price < minPrice`, cửa sổ di chuyển trái sang vị trí mới. Đây là fixed-left sliding window đặc biệt.

---

#### Bài 2: Maximum Average Subarray I (LeetCode 643) — Fixed Window

**Đề:** Cho `int[] nums` và `int k`. Tìm subarray liên tiếp độ dài đúng k có trung bình cộng **lớn nhất**. Trả về trung bình cộng đó (double).

**Ví dụ:**
```
Input:  nums = [1, 12, -5, -6, 50, 3], k = 4
Output: 12.75
Vì: subarray [12, -5, -6, 50] có tổng 51, trung bình 51/4 = 12.75.

Input:  nums = [5], k = 1
Output: 5.00
(Edge case: một phần tử, k = 1.)

Input:  nums = [0, 4, 0, 3, 2], k = 1
Output: 4.00
(Edge case: k = 1, tìm phần tử lớn nhất.)
```

**Constraint:** `n == nums.length`, `1 <= k <= n <= 10^5`, `-10^4 <= nums[i] <= 10^4`.

**Phân tích 5 câu hỏi:**
1. Input: n ≤ 10⁵, k cho trước
2. Output: double (trung bình)
3. Brute: mỗi vị trí tính tổng k phần tử → O(n*k)
4. Lãng phí: mỗi bước cộng/trừ 1 phần tử thay vì tính lại toàn bộ
5. Pattern: **Fixed Window** — trượt và cập nhật incremental

```csharp
// Brute — O(n * k)
public double FindMaxAverageBrute(int[] nums, int k)
{
    double max = double.MinValue;
    for (int i = 0; i <= nums.Length - k; i++)
    {
        double sum = 0;
        for (int j = i; j < i + k; j++) sum += nums[j];
        max = Math.Max(max, sum / k);
    }
    return max;
}

// Optimal — O(n) time, O(1) space
public double FindMaxAverage(int[] nums, int k)
{
    // Khởi tạo cửa sổ đầu tiên
    double windowSum = 0;
    for (int i = 0; i < k; i++)
        windowSum += nums[i];

    double maxSum = windowSum;

    // Trượt cửa sổ
    for (int i = k; i < nums.Length; i++)
    {
        windowSum += nums[i];       // thêm phần tử mới (phải)
        windowSum -= nums[i - k];   // bỏ phần tử cũ (trái)
        maxSum = Math.Max(maxSum, windowSum);
    }

    return maxSum / k;
}
// Time: O(n), Space: O(1)
```

---

### Medium

---

#### Bài 3: Longest Substring Without Repeating Characters (LeetCode 3) ⭐ Bài bản lề

**Đề:** Cho `string s`. Tìm độ dài **chuỗi con liên tiếp dài nhất** không có ký tự trùng lặp.

**Ví dụ:**
```
Input:  s = "abcabcbb"
Output: 3
Vì: "abc" (dài 3) là chuỗi con không trùng dài nhất.

Input:  s = "bbbbb"
Output: 1
Vì: chỉ có "b" (dài 1).

Input:  s = "pwwkew"
Output: 3
Vì: "wke" hoặc "kew" (dài 3). Không phải "pwke" vì "w" trùng.

Input:  s = ""
Output: 0
(Edge case: chuỗi rỗng.)

Input:  s = "a"
Output: 1
(Edge case: một ký tự.)
```

**Constraint:** `0 <= s.length <= 5 * 10^4`, s gồm các ký tự ASCII in được.

**Phân tích 5 câu hỏi:**
1. Input: chuỗi bất kỳ, ký tự ASCII
2. Output: int (độ dài)
3. Brute: xét mọi cặp (i, j), kiểm tra unique → O(n³) hoặc O(n²)
4. Lãng phí: khi gặp ký tự trùng, không cần xét lại từ đầu — chỉ cần di left qua vị trí cũ của ký tự trùng đó
5. Pattern: **Variable Window + HashSet/Dictionary**

```csharp
// Brute — O(n²) với HashSet
public int LengthOfLongestSubstringBrute(string s)
{
    int max = 0;
    for (int i = 0; i < s.Length; i++)
    {
        var seen = new HashSet<char>();
        for (int j = i; j < s.Length; j++)
        {
            if (!seen.Add(s[j])) break;
            max = Math.Max(max, j - i + 1);
        }
    }
    return max;
}

// Optimal với HashSet — O(n) time, O(min(n, alphabet)) space
public int LengthOfLongestSubstring(string s)
{
    var window = new HashSet<char>();
    int left = 0, max = 0;

    for (int right = 0; right < s.Length; right++)
    {
        // Thu hẹp khi gặp ký tự trùng
        while (window.Contains(s[right]))
        {
            window.Remove(s[left]);
            left++;
        }
        window.Add(s[right]);
        max = Math.Max(max, right - left + 1);
    }
    return max;
}

// Optimal với Dictionary (index) — O(n), bước nhảy left nhanh hơn
// Thay vì thu hẹp từng bước, nhảy thẳng qua vị trí trùng
public int LengthOfLongestSubstringV2(string s)
{
    var lastIndex = new Dictionary<char, int>(); // ký tự → index cuối cùng xuất hiện
    int left = 0, max = 0;

    for (int right = 0; right < s.Length; right++)
    {
        // Nếu ký tự đã trong cửa sổ, nhảy left qua nó
        if (lastIndex.TryGetValue(s[right], out int prevIdx) && prevIdx >= left)
            left = prevIdx + 1;

        lastIndex[s[right]] = right;
        max = Math.Max(max, right - left + 1);
    }
    return max;
}
// Time: O(n), Space: O(min(n, 128)) — 128 ký tự ASCII
```

**Lưu ý V2:** Khi dùng Dictionary với `lastIndex`, phải kiểm tra `prevIdx >= left` — ký tự có thể đã xuất hiện trước cửa sổ hiện tại, trường hợp đó không cần nhảy.

---

#### Bài 4: Longest Repeating Character Replacement (LeetCode 424)

**Đề:** Cho `string s` (chỉ gồm A-Z) và `int k`. Được phép thay thế **tối đa k ký tự** thành bất kỳ ký tự nào. Tìm độ dài chuỗi con liên tiếp dài nhất chỉ gồm **một loại ký tự** (sau khi thay thế).

**Ví dụ:**
```
Input:  s = "ABAB", k = 2
Output: 4
Vì: thay 2 ký tự B thành A (hoặc 2 A thành B) → "AAAA" hoặc "BBBB", dài 4.

Input:  s = "AABABBA", k = 1
Output: 4
Vì: thay ký tự B tại index 4 → "AABAAAA" không liên tiếp.
    Cửa sổ "ABBB" (index 3-6): thay 1 A → "BBBB", dài 4.

Input:  s = "AAAA", k = 0
Output: 4
(Edge case: không cần thay, cả chuỗi là 1 ký tự.)

Input:  s = "A", k = 0
Output: 1
(Edge case: một ký tự.)
```

**Constraint:** `1 <= s.length <= 10^5`, `s` chỉ gồm A-Z, `0 <= k <= s.length`.

**Phân tích 5 câu hỏi:**
1. Input: chuỗi chỉ A-Z, k cho trước
2. Output: int (độ dài)
3. Brute: xét mọi cặp (i, j), tính max frequency → O(n² * 26)
4. Lãng phí: cửa sổ hợp lệ khi `windowSize - maxFreq <= k`. Khi vi phạm → thu left
5. Pattern: **Variable Window + đếm tần suất**

```csharp
public int CharacterReplacement(string s, int k)
{
    int[] count = new int[26];     // đếm tần suất trong cửa sổ
    int left = 0, maxFreq = 0, result = 0;

    for (int right = 0; right < s.Length; right++)
    {
        count[s[right] - 'A']++;
        // maxFreq = số lần xuất hiện của ký tự phổ biến nhất trong cửa sổ
        maxFreq = Math.Max(maxFreq, count[s[right] - 'A']);

        // Số ký tự cần thay = windowSize - maxFreq
        // Nếu > k → vi phạm → thu left
        int windowSize = right - left + 1;
        if (windowSize - maxFreq > k)
        {
            count[s[left] - 'A']--;
            left++;
        }

        result = Math.Max(result, right - left + 1);
    }
    return result;
}
// Time: O(n), Space: O(26) = O(1)
```

**Tại sao không cần cập nhật maxFreq khi thu left?** Vì chúng ta chỉ quan tâm tới cửa sổ **dài nhất** đã tìm được. Nếu maxFreq giảm, cửa sổ mới sẽ không dài hơn cửa sổ tốt nhất cũ → không cần update. Đây là tối ưu hóa quan trọng.

**Điều kiện hợp lệ:** `windowSize - maxFreq <= k` — trong cửa sổ, số ký tự cần thay = tổng - số ký tự của loại phổ biến nhất.

---

#### Bài 5: Permutation in String (LeetCode 567) — Fixed Window + So Khớp Tần Suất

**Đề:** Cho `string s1` và `string s2`. Kiểm tra xem có **hoán vị của s1** nào xuất hiện dưới dạng chuỗi con của `s2` không.

**Ví dụ:**
```
Input:  s1 = "ab", s2 = "eidbaooo"
Output: true
Vì: "ba" (index 3-4 trong s2) là hoán vị của "ab".

Input:  s1 = "ab", s2 = "eidboaoo"
Output: false
Vì: không có hoán vị nào của "ab" là chuỗi con liên tiếp của s2.

Input:  s1 = "adc", s2 = "dcda"
Output: true
Vì: "dca" (index 1-3) hay "cda" — "cda" là hoán vị của "adc".

Input:  s1 = "a", s2 = "a"
Output: true
(Edge case: s1 = s2 = một ký tự.)
```

**Constraint:** `1 <= s1.length, s2.length <= 10^4`, s1 và s2 chỉ gồm a-z.

**Phân tích 5 câu hỏi:**
1. Input: s1 nhỏ (≤ 10⁴), s2 có thể dài hơn; chỉ a-z
2. Output: bool
3. Brute: tạo tất cả hoán vị s1 rồi tìm trong s2 → O(k! * n) — quá chậm
4. Better: hoán vị = cùng tần suất → cửa sổ kích thước `len(s1)` trong s2, so tần suất
5. Pattern: **Fixed Window kích thước k + so khớp tần suất**

```csharp
// Brute — O(n * k) mỗi lần so mảng 26 phần tử (acceptable nhưng không cần)

// Optimal với mảng đếm + biến matched
public bool CheckInclusion(string s1, string s2)
{
    if (s1.Length > s2.Length) return false;

    int[] need   = new int[26]; // tần suất cần có (từ s1)
    int[] window = new int[26]; // tần suất trong cửa sổ hiện tại

    foreach (char c in s1) need[c - 'a']++;

    int k = s1.Length;
    int matched = 0;                        // số ký tự đang khớp đúng tần suất
    int required = need.Count(x => x > 0);  // số loại ký tự cần khớp

    for (int right = 0; right < s2.Length; right++)
    {
        int c = s2[right] - 'a';
        window[c]++;
        // Kiểm tra ký tự này có vừa khớp đúng không
        if (window[c] == need[c]) matched++;

        // Bỏ phần tử trái khi cửa sổ quá dài
        if (right >= k)
        {
            int l = s2[right - k] - 'a';
            if (window[l] == need[l]) matched--; // sắp mất khớp
            window[l]--;
        }

        if (matched == required) return true;
    }
    return false;
}
// Time: O(n), Space: O(26) = O(1)

// Cách đơn giản hơn: so trực tiếp hai mảng 26 phần tử
public bool CheckInclusionSimple(string s1, string s2)
{
    if (s1.Length > s2.Length) return false;

    int[] need = new int[26];
    int[] win  = new int[26];
    foreach (char c in s1) need[c - 'a']++;

    for (int i = 0; i < s2.Length; i++)
    {
        win[s2[i] - 'a']++;
        if (i >= s1.Length)
            win[s2[i - s1.Length] - 'a']--;
        if (win.SequenceEqual(need)) return true; // O(26) = O(1)
    }
    return false;
}
```

---

#### Bài 6: Fruit Into Baskets (LeetCode 904) — Variable Window "Nhiều nhất K loại"

**Đề:** Có `int[] fruits` (loại quả tại mỗi cây). Có 2 giỏ, mỗi giỏ chứa đúng **một loại** quả. Phải hái liên tiếp từ một cây bắt đầu. Tìm số quả **nhiều nhất** có thể hái.

*Quy về bài toán:* Tìm subarray dài nhất chứa **nhiều nhất 2 loại phần tử khác nhau**.

**Ví dụ:**
```
Input:  fruits = [1, 2, 1]
Output: 3
Vì: hái cả mảng [1, 2, 1] — dùng giỏ 1 cho loại 1, giỏ 2 cho loại 2.

Input:  fruits = [0, 1, 2, 2]
Output: 3
Vì: tốt nhất là [1, 2, 2] — hai loại, dài 3.

Input:  fruits = [1, 2, 3, 2, 2]
Output: 4
Vì: [2, 3, 2, 2] — hai loại (2 và 3), dài 4.

Input:  fruits = [1, 1, 1, 1]
Output: 4
(Edge case: chỉ một loại, hái cả mảng.)
```

**Constraint:** `1 <= fruits.length <= 10^5`, `0 <= fruits[i] < fruits.length`.

**Phân tích 5 câu hỏi:**
1. Input: cần ≤ 2 loại trong subarray
2. Output: int (độ dài)
3. Brute: xét mọi subarray → O(n²)
4. Lãng phí: khi có 3 loại → thu left cho đến khi ≤ 2 loại
5. Pattern: **Variable Window + Frequency Map** với giới hạn `distinct ≤ k`

```csharp
// Template tổng quát: subarray dài nhất với nhiều nhất k loại phân biệt
public int TotalFruits(int[] fruits)
{
    return LongestSubarrayWithAtMostKDistinct(fruits, 2);
}

private int LongestSubarrayWithAtMostKDistinct(int[] nums, int k)
{
    var window = new Dictionary<int, int>(); // loại → số lần
    int left = 0, result = 0;

    for (int right = 0; right < nums.Length; right++)
    {
        // Thêm phần tử vào cửa sổ
        window[nums[right]] = window.GetValueOrDefault(nums[right], 0) + 1;

        // Thu hẹp khi có quá k loại
        while (window.Count > k)
        {
            int leftVal = nums[left];
            window[leftVal]--;
            if (window[leftVal] == 0)
                window.Remove(leftVal);
            left++;
        }

        result = Math.Max(result, right - left + 1);
    }
    return result;
}
// Time: O(n), Space: O(k)
```

**Tổng quát hóa:** Bài này là "nhiều nhất 2 loại". Pattern tương tự cho "nhiều nhất k loại" — chỉ đổi `2` thành `k`. Đây là template chuẩn cho nhóm bài "at most k distinct".

---

### Hard

---

#### Bài 7: Minimum Window Substring (LeetCode 76) — Master Template

**Đề:** Cho `string s` và `string t`. Tìm **chuỗi con ngắn nhất** của `s` chứa đủ tất cả ký tự của `t` (kể cả trùng). Nếu không tồn tại → trả về `""`.

**Ví dụ:**
```
Input:  s = "ADOBECODEBANC", t = "ABC"
Output: "BANC"
Vì: "BANC" chứa A, B, C và là chuỗi con ngắn nhất thỏa điều kiện.

Input:  s = "a", t = "a"
Output: "a"
(Edge case: s = t = một ký tự.)

Input:  s = "a", t = "aa"
Output: ""
Vì: t cần hai 'a' nhưng s chỉ có một.

Input:  s = "ADOBECODEBANC", t = "AABC"
Output: "ADOBEC"
(Edge case: t có ký tự trùng — cần đủ số lượng.)
```

**Constraint:** `m == s.length`, `n == t.length`, `1 <= m, n <= 10^5`, s và t gồm A-Z, a-z.

**Phân tích 5 câu hỏi:**
1. Input: cần đủ tần suất của từng ký tự trong t
2. Output: string (chuỗi con ngắn nhất)
3. Brute: xét mọi cặp (i, j) → O(n² * 128)
4. Lãng phí: cửa sổ mở rộng phải cho đến khi đủ, rồi thu trái để ngắn nhất
5. Pattern: **Variable Window + Frequency Map + biến `matched`** — master template

```csharp
// Brute — O(n²)
public string MinWindowBrute(string s, string t)
{
    string result = "";
    for (int i = 0; i < s.Length; i++)
        for (int j = i + 1; j <= s.Length; j++)
        {
            string sub = s[i..j];
            if (ContainsAll(sub, t) && (result == "" || sub.Length < result.Length))
                result = sub;
        }
    return result;
}

bool ContainsAll(string window, string t)
{
    var need = new Dictionary<char, int>();
    foreach (char c in t) need[c] = need.GetValueOrDefault(c, 0) + 1;
    foreach (char c in window)
        if (need.ContainsKey(c))
        {
            need[c]--;
            if (need[c] == 0) need.Remove(c);
        }
    return need.Count == 0;
}

// Optimal — O(n) time, O(128) space
public string MinWindow(string s, string t)
{
    if (s.Length < t.Length) return "";

    // Đếm tần suất cần có từ t
    var need = new Dictionary<char, int>();
    foreach (char c in t)
        need[c] = need.GetValueOrDefault(c, 0) + 1;

    int required = need.Count;  // số loại ký tự cần khớp đủ số lượng
    int matched  = 0;           // số loại đang khớp đúng

    var window = new Dictionary<char, int>();
    int left = 0;
    int minLen = int.MaxValue, minLeft = 0;

    for (int right = 0; right < s.Length; right++)
    {
        // Thêm s[right] vào cửa sổ
        char c = s[right];
        window[c] = window.GetValueOrDefault(c, 0) + 1;

        // Kiểm tra loại ký tự này có vừa đạt đủ số lượng không
        if (need.TryGetValue(c, out int needCount) && window[c] == needCount)
            matched++;

        // Khi đã hợp lệ (đủ tất cả ký tự) → thử thu hẹp từ trái
        while (matched == required)
        {
            // Ghi nhận kết quả
            if (right - left + 1 < minLen)
            {
                minLen  = right - left + 1;
                minLeft = left;
            }

            // Bỏ s[left] khỏi cửa sổ
            char l = s[left];
            window[l]--;
            if (need.TryGetValue(l, out int nlCount) && window[l] < nlCount)
                matched--; // mất khớp
            left++;
        }
    }

    return minLen == int.MaxValue ? "" : s.Substring(minLeft, minLen);
}
// Time: O(m + n), Space: O(128) = O(1)
```

**Template tổng quát cho "window ngắn nhất thỏa điều kiện":**
1. Mở rộng right cho đến khi hợp lệ
2. Ghi kết quả
3. Thu left cho đến khi không còn hợp lệ
4. Lặp lại

---

#### Bài 8 (Nâng cao): Sliding Window Maximum (LeetCode 239)

> **Lưu ý:** Bài này yêu cầu **Monotonic Deque** — sẽ học kỹ ở Phase 15. Hiểu ý tưởng là đủ tại Phase 3.

**Đề:** Cho `int[] nums` và `int k`. Với mỗi cửa sổ kích thước k trượt qua mảng, tìm **phần tử lớn nhất**. Trả về mảng các max đó.

**Ví dụ:**
```
Input:  nums = [1, 3, -1, -3, 5, 3, 6, 7], k = 3
Output: [3, 3, 5, 5, 6, 7]
Vì:
  [1, 3, -1] → max 3
  [3, -1, -3] → max 3
  [-1, -3, 5] → max 5
  [-3, 5, 3] → max 5
  [5, 3, 6] → max 6
  [3, 6, 7] → max 7
```

**Constraint:** `1 <= nums.length <= 10^5`, `-10^4 <= nums[i] <= 10^4`, `1 <= k <= nums.length`.

**Ý tưởng (preview Phase 15):** Dùng **Deque (hàng đợi hai đầu)** lưu index, giữ tính đơn điệu giảm dần về giá trị. Phần tử đầu deque luôn là max của cửa sổ hiện tại.

```csharp
// Brute — O(n * k)
public int[] MaxSlidingWindowBrute(int[] nums, int k)
{
    int n = nums.Length;
    int[] result = new int[n - k + 1];
    for (int i = 0; i <= n - k; i++)
    {
        int max = nums[i];
        for (int j = i + 1; j < i + k; j++)
            max = Math.Max(max, nums[j]);
        result[i] = max;
    }
    return result;
}

// Optimal — O(n) với Monotonic Deque (học kỹ ở Phase 15)
public int[] MaxSlidingWindow(int[] nums, int k)
{
    var deque = new LinkedList<int>(); // lưu index, giảm dần về giá trị
    int n = nums.Length;
    int[] result = new int[n - k + 1];

    for (int i = 0; i < n; i++)
    {
        // Bỏ phần tử ra khỏi cửa sổ (index quá cũ)
        if (deque.Count > 0 && deque.First.Value < i - k + 1)
            deque.RemoveFirst();

        // Duy trì tính đơn điệu giảm: bỏ từ cuối nếu nhỏ hơn nums[i]
        while (deque.Count > 0 && nums[deque.Last.Value] < nums[i])
            deque.RemoveLast();

        deque.AddLast(i);

        // Bắt đầu ghi kết quả khi cửa sổ đủ k phần tử
        if (i >= k - 1)
            result[i - k + 1] = nums[deque.First.Value];
    }
    return result;
}
// Time: O(n), Space: O(k)
```

---

## Tổng kết Template C# cho Phase 3

```csharp
// Template 1: Fixed Window kích thước k
int windowSum = nums[..k].Sum();
int result = windowSum;
for (int i = k; i < nums.Length; i++)
{
    windowSum += nums[i] - nums[i - k];
    result = Math.Max(result, windowSum);
}

// Template 2: Variable Window — tìm DÀI nhất
int left = 0, maxLen = 0;
for (int right = 0; right < n; right++)
{
    Add(s[right]);                          // cập nhật trạng thái
    while (/* không hợp lệ */)
    {
        Remove(s[left]);
        left++;
    }
    maxLen = Math.Max(maxLen, right - left + 1);
}

// Template 3: Variable Window — tìm NGẮN nhất
int left = 0, minLen = int.MaxValue;
for (int right = 0; right < n; right++)
{
    Add(s[right]);
    while (/* hợp lệ */)
    {
        minLen = Math.Min(minLen, right - left + 1);
        Remove(s[left]);
        left++;
    }
}
return minLen == int.MaxValue ? 0 : minLen;

// Template 4: "Đúng K" = AtMost(K) - AtMost(K-1)
int ExactlyK(int[] nums, int k)
    => AtMost(nums, k) - AtMost(nums, k - 1);

int AtMost(int[] nums, int k)
{
    var window = new Dictionary<int, int>();
    int left = 0, result = 0;
    for (int right = 0; right < nums.Length; right++)
    {
        window[nums[right]] = window.GetValueOrDefault(nums[right], 0) + 1;
        while (window.Count > k)
        {
            window[nums[left]]--;
            if (window[nums[left]] == 0) window.Remove(nums[left]);
            left++;
        }
        result += right - left + 1; // đếm số subarray kết thúc tại right
    }
    return result;
}
```

---

## Tiêu chí hoàn thành Phase 3

- [ ] Viết được template variable window co giãn từ trí nhớ (cả "dài nhất" và "ngắn nhất").
- [ ] Tự giải bài 3 (Longest Substring) và bài 76 (Min Window Substring) không nhìn lời giải.
- [ ] Phân biệt được khi nào dùng fixed window vs variable window ngay khi đọc đề.
- [ ] Giải thích được tại sao không cần update `maxFreq` khi thu left trong bài 424.
- [ ] Nhận ra "đúng k loại" → dùng mẹo `AtMost(k) - AtMost(k-1)`.

---

## Ghi chú học tập

> Điền sau mỗi bài:
>
> | Bài | Kiểu Window | Điều kiện hợp lệ | Lỗi đã mắc |
> |-----|-------------|-----------------|-----------|
> | LeetCode 3 | Variable, dài nhất | không có ký tự trùng | |
> | LeetCode 424 | Variable, dài nhất | windowSize - maxFreq ≤ k | |
> | LeetCode 76 | Variable, ngắn nhất | matched == required | |
> | ... | ... | ... | ... |

**Thời gian dự kiến:** 1 tuần
**Mức độ:** ⭐⭐–⭐⭐⭐
