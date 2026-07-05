# Phase 1 — Arrays & Hashing

> **Ngôn ngữ:** C#
> **Mục tiêu:** Giải được các bài cần tra cứu / đếm / nhóm nhanh. Đây là phase quan trọng nhất vì hash là "phản xạ tối ưu" số một — biến O(n²) thành O(n) bằng cách đánh đổi không gian lấy tốc độ.

---

## Bài học 1 — Arrays trong C#

### 1.1 Các kiểu mảng

```csharp
// Mảng cố định kích thước — O(1) truy cập theo index
int[] arr = new int[5];          // {0, 0, 0, 0, 0}
int[] arr2 = { 3, 1, 4, 1, 5 };

// Truy cập O(1)
int x = arr2[2];        // 4
arr2[0] = 10;           // sửa tại chỗ

// Mảng động
var list = new List<int> { 1, 2, 3 };
list.Add(4);            // O(1) amortized
list.RemoveAt(0);       // O(n) — phải dịch chuyển phần tử

// Mảng 2 chiều
int[,] grid = new int[3, 4];          // 3 hàng, 4 cột
int rows = grid.GetLength(0);          // 3
int cols = grid.GetLength(1);          // 4
grid[1, 2] = 7;

// Mảng 2 chiều dạng jagged (linh hoạt hơn)
int[][] jag = new int[3][];
jag[0] = new int[] { 1, 2 };
jag[1] = new int[] { 3, 4, 5 };
```

### 1.2 Thao tác in-place

In-place = sửa trực tiếp mảng đầu vào, không tạo mảng mới → O(1) space thêm.

```csharp
// Swap hai phần tử
void Swap(int[] arr, int i, int j)
    => (arr[i], arr[j]) = (arr[j], arr[i]);

// Đảo mảng in-place
void Reverse(int[] arr)
{
    int left = 0, right = arr.Length - 1;
    while (left < right)
        Swap(arr, left++, right--);
}
```

### 1.3 Các hàm tiện ích hay dùng

```csharp
// Sort — O(n log n)
Array.Sort(arr);
Array.Sort(arr, (a, b) => b - a);      // sort giảm dần

// Sort với key
Array.Sort(arr, Comparer<int>.Create((a, b) => Math.Abs(a) - Math.Abs(b)));

// Copy
int[] copy = (int[])arr.Clone();
int[] partial = arr[2..5];             // index 2, 3, 4 (C# 8+)

// Fill
Array.Fill(arr, 0);                    // đặt tất cả = 0

// Tìm kiếm
int idx = Array.IndexOf(arr, target);  // O(n) linear search
int idx2 = Array.BinarySearch(arr, target); // O(log n), arr phải sorted
```

---

## Bài học 2 — HashSet<T>

### 2.1 Khi nào dùng HashSet?

```
Câu hỏi: "Phần tử này đã xuất hiện chưa?" / "Có tồn tại không?"
→ Dùng HashSet<T>
→ Tra cứu O(1) thay vì O(n) nếu quét mảng
```

### 2.2 API cơ bản

```csharp
var set = new HashSet<int>();

set.Add(5);              // O(1) — thêm, trả về false nếu đã có
set.Contains(5);         // O(1) — kiểm tra
set.Remove(5);           // O(1) — xóa
set.Count;               // số phần tử

// Khởi tạo từ mảng — O(n)
var set2 = new HashSet<int>(arr);

// Kiểm tra và thêm trong một bước
if (!set.Add(x))         // Add trả về false nếu đã tồn tại
    return "duplicate";
```

### 2.3 Ví dụ: Contains Duplicate (LeetCode 217)

```
Cho int[] nums. Trả về true nếu có phần tử nào xuất hiện ≥ 2 lần.
```

**5 câu hỏi:**
1. Input: n ≤ 10⁵, giá trị ±10⁹ → cần HashSet (không dùng counting array)
2. Output: bool
3. Brute Force: hai vòng lặp lồng → O(n²)
4. Lãng phí: với mỗi phần tử, quét lại mảng → dùng HashSet nhớ lại
5. Pattern: **Seen-set** → O(n) time, O(n) space

```csharp
// Brute Force — O(n²) time, O(1) space
public bool ContainsDuplicateBrute(int[] nums)
{
    for (int i = 0; i < nums.Length; i++)
        for (int j = i + 1; j < nums.Length; j++)
            if (nums[i] == nums[j]) return true;
    return false;
}

// Optimal — O(n) time, O(n) space
public bool ContainsDuplicate(int[] nums)
{
    var seen = new HashSet<int>();
    foreach (int x in nums)
        if (!seen.Add(x))   // Add trả false nếu đã tồn tại
            return true;
    return false;
}

// Cực ngắn (nhưng tạo toàn bộ set trước)
public bool ContainsDuplicate2(int[] nums)
    => nums.Length != new HashSet<int>(nums).Count;
```

---

## Bài học 3 — Dictionary<K, V>

### 3.1 Khi nào dùng Dictionary?

```
Câu hỏi: "Phần tử này xuất hiện bao nhiêu lần?"
         "Tôi đã thấy giá trị X chưa, và nó ở đâu (index bao nhiêu)?"
         "Nhóm các phần tử theo tiêu chí nào đó"
→ Dùng Dictionary<K, V>
```

### 3.2 API cơ bản

```csharp
var dict = new Dictionary<int, int>();

// Thêm / cập nhật
dict[key] = value;                             // O(1)
dict.Add(key, value);                          // O(1), ném exception nếu key đã có

// Đọc an toàn
if (dict.TryGetValue(key, out int val)) { }    // không ném exception
int v = dict.GetValueOrDefault(key, 0);        // 0 nếu key không tồn tại

// Kiểm tra
dict.ContainsKey(key);     // O(1)
dict.ContainsValue(val);   // O(n) — tránh dùng

// Xóa
dict.Remove(key);          // O(1)

// Đếm tần suất — pattern phổ biến
dict[x] = dict.GetValueOrDefault(x, 0) + 1;
// hoặc dùng helper:
// if (!dict.ContainsKey(x)) dict[x] = 0;
// dict[x]++;
```

### 3.3 Ví dụ: Two Sum (LeetCode 1)

```
Cho int[] nums và int target.
Trả về [i, j] sao cho nums[i] + nums[j] == target.
```

**5 câu hỏi:**
1. Input: n ≤ 10⁴, không sorted, một đáp án duy nhất
2. Output: int[] hai phần tử (indices, không phải values)
3. Brute Force: hai vòng lặp → O(n²)
4. Lãng phí: với mỗi `nums[i]`, quét lại toàn bộ để tìm `target - nums[i]`
5. Pattern: **Complement Lookup** — lưu `value → index` vào Dictionary

```csharp
// Brute Force — O(n²)
public int[] TwoSumBrute(int[] nums, int target)
{
    for (int i = 0; i < nums.Length; i++)
        for (int j = i + 1; j < nums.Length; j++)
            if (nums[i] + nums[j] == target)
                return new[] { i, j };
    return Array.Empty<int>();
}

// Optimal — O(n) time, O(n) space
public int[] TwoSum(int[] nums, int target)
{
    // Lưu: giá trị → index
    var seen = new Dictionary<int, int>();

    for (int i = 0; i < nums.Length; i++)
    {
        int complement = target - nums[i];

        if (seen.ContainsKey(complement))
            return new[] { seen[complement], i };

        seen[nums[i]] = i;   // lưu SAU khi kiểm tra, tránh dùng cùng index
    }
    return Array.Empty<int>();
}
```

**Lưu ý quan trọng:** Phải kiểm tra `complement` TRƯỚC khi `seen[nums[i]] = i`. Nếu làm ngược lại, khi `nums[i] + nums[i] == target` sẽ trả về `[i, i]` — dùng cùng index.

---

## Bài học 4 — Frequency Counting

### 4.1 Counting Array (khi miền giá trị nhỏ)

```csharp
// Ví dụ: đếm ký tự a-z trong string
string s = "hello";
int[] count = new int[26];
foreach (char c in s)
    count[c - 'a']++;

// count['e'-'a'] = count[4] = 1
// count['l'-'a'] = count[11] = 2
// count['o'-'a'] = count[14] = 1
```

**Khi nào dùng counting array thay Dictionary?**
- Miền giá trị nhỏ và biết trước: ký tự a-z (26), digit 0-9 (10), giá trị 0..k
- Nhanh hơn và ít overhead hơn Dictionary

### 4.2 Dictionary (khi miền giá trị lớn)

```csharp
// Đếm tần suất phần tử trong mảng
int[] nums = { 1, 3, 2, 3, 1, 3 };
var freq = new Dictionary<int, int>();
foreach (int x in nums)
    freq[x] = freq.GetValueOrDefault(x, 0) + 1;
// freq = { 1:2, 3:3, 2:1 }
```

### 4.3 Ví dụ: Valid Anagram (LeetCode 242)

```
Cho string s và t. Trả về true nếu t là anagram của s.
```

**5 câu hỏi:**
1. Input: lowercase a-z → miền 26 → dùng int[26]
2. Output: bool
3. Brute Force: sort cả hai → O(n log n)
4. Lãng phí: sort không cần thiết nếu chỉ so sánh tần suất
5. Pattern: **Frequency Counting** → O(n) time, O(1) space

```csharp
// Cách 1: Sort — O(n log n)
public bool IsAnagramSort(string s, string t)
{
    if (s.Length != t.Length) return false;
    char[] sc = s.ToCharArray(), tc = t.ToCharArray();
    Array.Sort(sc);
    Array.Sort(tc);
    return sc.SequenceEqual(tc);
}

// Cách 2: Counting Array — O(n) time, O(1) space (26 ký tự cố định)
public bool IsAnagram(string s, string t)
{
    if (s.Length != t.Length) return false;

    int[] count = new int[26];
    foreach (char c in s) count[c - 'a']++;   // tăng với s
    foreach (char c in t) count[c - 'a']--;   // giảm với t

    foreach (int x in count)
        if (x != 0) return false;   // nếu chênh lệch → không anagram

    return true;
}

// Cách 3: Nếu có Unicode (không chỉ a-z) → dùng Dictionary
public bool IsAnagramUnicode(string s, string t)
{
    if (s.Length != t.Length) return false;

    var map = new Dictionary<char, int>();
    foreach (char c in s) map[c] = map.GetValueOrDefault(c, 0) + 1;
    foreach (char c in t)
    {
        if (!map.ContainsKey(c) || map[c] == 0) return false;
        map[c]--;
    }
    return true;
}
```

### 4.4 Ví dụ: Majority Element (LeetCode 169)

```
Cho int[] nums (n phần tử). Tìm phần tử xuất hiện > n/2 lần.
Đảm bảo luôn tồn tại.
```

**5 câu hỏi:**
1. Input: n ≤ 5*10⁴, một đáp án tồn tại
2. Output: một số nguyên
3. Brute Force: đếm tần suất từng phần tử → O(n) time, O(n) space
4. Tối ưu hơn: Boyer-Moore Voting → O(n) time, **O(1) space**
5. Pattern: **Frequency Counting** → sau đó học Boyer-Moore

```csharp
// Cách 1: Dictionary — O(n) time, O(n) space
public int MajorityElement(int[] nums)
{
    var freq = new Dictionary<int, int>();
    foreach (int x in nums)
    {
        freq[x] = freq.GetValueOrDefault(x, 0) + 1;
        if (freq[x] > nums.Length / 2)
            return x;
    }
    return -1; // không bao giờ xảy ra theo đề
}

// Cách 2: Boyer-Moore Voting — O(n) time, O(1) space
// Ý tưởng: candidate và count — nếu gặp giống candidate thì +1, khác thì -1.
// Khi count = 0, chọn candidate mới. Majority element luôn thắng.
public int MajorityElementOptimal(int[] nums)
{
    int candidate = nums[0], count = 1;
    for (int i = 1; i < nums.Length; i++)
    {
        if (count == 0)
        {
            candidate = nums[i];
            count = 1;
        }
        else if (nums[i] == candidate)
            count++;
        else
            count--;
    }
    return candidate;
}
```

---

## Bài học 5 — Ba Pattern Quan Trọng

### Pattern 1: Seen-set

**Dấu hiệu:** "Có tồn tại cặp không?" / "Có trùng lặp không?"

```
Nhìn thấy x → lưu vào HashSet → lần sau gặp lại → tìm thấy
```

```csharp
// Template
var seen = new HashSet<T>();
foreach (var x in collection)
{
    if (seen.Contains(/* điều kiện */))
        return /* tìm thấy */;
    seen.Add(x);
}
```

---

### Pattern 2: Complement Lookup

**Dấu hiệu:** "Tìm cặp/bộ có tổng = target"

```
Với mỗi x, tính complement = target - x
→ Kiểm tra complement đã thấy chưa (O(1))
→ Lưu x vào Dictionary
```

```csharp
// Template
var seen = new Dictionary<T, int>(); // value → index
for (int i = 0; i < n; i++)
{
    T complement = /* target - nums[i] hoặc điều kiện bù */;
    if (seen.ContainsKey(complement))
        return /* kết quả */;
    seen[nums[i]] = i;
}
```

---

### Pattern 3: Grouping by Normalized Key

**Dấu hiệu:** "Nhóm các phần tử giống nhau theo tiêu chí nào đó"

```
Mỗi phần tử → biến thành key chuẩn hóa → nhóm theo key
Anagram → key là chuỗi sorted / int[26] đếm
```

```csharp
// Template
var groups = new Dictionary<K, List<V>>();
foreach (var item in collection)
{
    K key = Normalize(item);  // hàm chuẩn hóa
    if (!groups.ContainsKey(key))
        groups[key] = new List<V>();
    groups[key].Add(item);
}
```

---

## Bài tập theo thứ tự

### Easy

---

#### Bài 1: Contains Duplicate (LeetCode 217) — Seen-set

**Đề:** Cho `int[] nums`. Trả về `true` nếu có phần tử xuất hiện ≥ 2 lần.

**Ví dụ:**
```
Input:  nums = [1, 2, 3, 1]
Output: true
Vì: 1 xuất hiện ở index 0 và index 3.

Input:  nums = [1, 2, 3, 4]
Output: false
Vì: không có phần tử nào lặp lại.

Input:  nums = [1, 1, 1, 3, 3, 4, 3, 2, 4, 2]
Output: true
```

**Constraint:** `1 <= nums.length <= 10^5`, `-10^9 <= nums[i] <= 10^9`

**Phân tích:**
- n ≤ 10⁵, ±10⁹ → HashSet<int>
- Brute: O(n²) → Optimal: O(n) với seen-set

```csharp
public bool ContainsDuplicate(int[] nums)
{
    var seen = new HashSet<int>();
    foreach (int x in nums)
        if (!seen.Add(x))  // Add trả false nếu đã có
            return true;
    return false;
}
// Time: O(n), Space: O(n)
```

**Biến thể:** Contains Duplicate III (220) — dùng SortedSet để kiểm tra khoảng.

---

#### Bài 2: Two Sum (LeetCode 1) — Complement Lookup

**Đề:** Cho `int[] nums` và `int target`. Trả về `[i, j]` với `nums[i] + nums[j] == target`. Không dùng cùng một phần tử hai lần. Đảm bảo có đúng một đáp án.

**Ví dụ:**
```
Input:  nums = [2, 7, 11, 15], target = 9
Output: [0, 1]
Vì: nums[0] + nums[1] = 2 + 7 = 9.

Input:  nums = [3, 2, 4], target = 6
Output: [1, 2]
Vì: nums[1] + nums[2] = 2 + 4 = 6.

Input:  nums = [3, 3], target = 6
Output: [0, 1]
(Edge case: hai phần tử bằng nhau, phải dùng hai index khác nhau.)
```

**Constraint:** `2 <= nums.length <= 10^4`, `-10^9 <= nums[i] <= 10^9`, `-10^9 <= target <= 10^9`

**Phân tích:**
- Với mỗi `nums[i]`, cần tìm `target - nums[i]` trong các phần tử đã duyệt
- Lưu `value → index` vào Dictionary

```csharp
public int[] TwoSum(int[] nums, int target)
{
    var seen = new Dictionary<int, int>(); // value → index
    for (int i = 0; i < nums.Length; i++)
    {
        int complement = target - nums[i];
        if (seen.TryGetValue(complement, out int j))
            return new[] { j, i };
        seen[nums[i]] = i;
    }
    return Array.Empty<int>();
}
// Time: O(n), Space: O(n)
```

**Sai lầm phổ biến:** Lưu `seen[nums[i]] = i` TRƯỚC khi kiểm tra → có thể match với chính nó khi `2 * nums[i] == target`.

---

#### Bài 3: Valid Anagram (LeetCode 242) — Frequency Counting

**Đề:** Cho `string s, t`. Trả về `true` nếu t là anagram của s (cùng ký tự, khác thứ tự).

**Ví dụ:**
```
Input:  s = "anagram", t = "nagaram"
Output: true
Vì: cả hai có cùng ký tự: a×3, n×1, g×1, r×1, m×1.

Input:  s = "rat", t = "car"
Output: false
Vì: "rat" có t, "car" không có t (và "car" có c, "rat" không có c).

Input:  s = "a", t = "ab"
Output: false
Vì: độ dài khác nhau → không thể là anagram.
```

**Constraint:** `1 <= s.length, t.length <= 5 * 10^4`, chỉ gồm chữ thường a-z.

```csharp
public bool IsAnagram(string s, string t)
{
    if (s.Length != t.Length) return false;
    int[] count = new int[26];
    for (int i = 0; i < s.Length; i++)
    {
        count[s[i] - 'a']++;
        count[t[i] - 'a']--;
    }
    return count.All(x => x == 0);
}
// Time: O(n), Space: O(1) — array 26 phần tử cố định
```

---

#### Bài 4: Majority Element (LeetCode 169) — Frequency + Boyer-Moore

**Đề:** Cho `int[] nums` (n phần tử). Tìm phần tử xuất hiện **> n/2 lần**. Đảm bảo luôn tồn tại.

**Ví dụ:**
```
Input:  nums = [3, 2, 3]
Output: 3
Vì: 3 xuất hiện 2 lần > 3/2 = 1.5 lần.

Input:  nums = [2, 2, 1, 1, 1, 2, 2]
Output: 2
Vì: 2 xuất hiện 4 lần > 7/2 = 3.5 lần.

Input:  nums = [1]
Output: 1
(Edge case: mảng một phần tử.)
```

**Constraint:** `1 <= nums.length <= 5 * 10^4`, `-10^9 <= nums[i] <= 10^9`

```csharp
// Boyer-Moore Voting
public int MajorityElement(int[] nums)
{
    int candidate = nums[0], count = 1;
    for (int i = 1; i < nums.Length; i++)
    {
        if (count == 0) { candidate = nums[i]; count = 1; }
        else if (nums[i] == candidate) count++;
        else count--;
    }
    return candidate;
}
// Time: O(n), Space: O(1)
```

**Tại sao Boyer-Moore đúng?** Majority element xuất hiện > n/2 lần. Mỗi lần "triệt tiêu" giữa candidate và non-candidate, candidate mất 1 phiếu và non-candidate mất 1 phiếu. Vì majority > tất cả phần còn lại, candidate cuối cùng là majority.

---

### Medium

---

#### Bài 5: Group Anagrams (LeetCode 49) — Grouping

**Đề:** Cho `string[] strs`. Nhóm các anagram lại với nhau. Thứ tự trong nhóm và thứ tự các nhóm không quan trọng.

**Ví dụ:**
```
Input:  strs = ["eat","tea","tan","ate","nat","bat"]
Output: [["bat"], ["nat","tan"], ["ate","eat","tea"]]
Vì: "eat","tea","ate" là anagram nhau; "tan","nat" là anagram nhau; "bat" đứng riêng.

Input:  strs = [""]
Output: [[""]]
(Edge case: chuỗi rỗng tạo một nhóm riêng.)

Input:  strs = ["a"]
Output: [["a"]]
```

**Constraint:** `1 <= strs.length <= 10^4`, `0 <= strs[i].length <= 100`, chỉ gồm chữ thường a-z.

**5 câu hỏi:**
1. Input: mảng string, n ≤ 10⁴, k ≤ 100 (độ dài chuỗi)
2. Output: `IList<IList<string>>` — danh sách các nhóm
3. Brute: so từng cặp O(n² * k log k)
4. Lãng phí: so nhiều lần; có thể tạo key chuẩn hóa cho mỗi string → nhóm theo key
5. Pattern: **Grouping** — key là chuỗi đã sort

```csharp
public IList<IList<string>> GroupAnagrams(string[] strs)
{
    var groups = new Dictionary<string, List<string>>();

    foreach (string s in strs)
    {
        // Chuẩn hóa: sort các ký tự → key
        char[] chars = s.ToCharArray();
        Array.Sort(chars);
        string key = new string(chars);

        if (!groups.ContainsKey(key))
            groups[key] = new List<string>();
        groups[key].Add(s);
    }

    return new List<IList<string>>(groups.Values);
}
// Time: O(n * k log k), Space: O(n * k)
// k = độ dài chuỗi trung bình

// Cách 2: key là int[26] đếm ký tự — tránh sort, O(n * k) time
public IList<IList<string>> GroupAnagramsV2(string[] strs)
{
    var groups = new Dictionary<string, List<string>>();

    foreach (string s in strs)
    {
        int[] count = new int[26];
        foreach (char c in s) count[c - 'a']++;
        // Tạo key từ mảng đếm: "1#0#2#..." 
        string key = string.Join("#", count);

        if (!groups.ContainsKey(key))
            groups[key] = new List<string>();
        groups[key].Add(s);
    }

    return new List<IList<string>>(groups.Values);
}
// Time: O(n * k), Space: O(n * k)
```

**Khi nào dùng cách 2?** Khi k lớn hoặc cần tránh O(k log k) của sort.

---

#### Bài 6: Top K Frequent Elements (LeetCode 347)

**Đề:** Cho `int[] nums` và `int k`. Trả về k phần tử xuất hiện nhiều nhất (thứ tự kết quả không quan trọng).

**Ví dụ:**
```
Input:  nums = [1, 1, 1, 2, 2, 3], k = 2
Output: [1, 2]
Vì: 1 xuất hiện 3 lần (nhiều nhất), 2 xuất hiện 2 lần (nhiều thứ hai).

Input:  nums = [1], k = 1
Output: [1]
(Edge case: một phần tử, k = 1.)

Input:  nums = [1, 2], k = 2
Output: [1, 2]
(Edge case: hai phần tử cùng tần suất, lấy cả hai.)
```

**Constraint:** `1 <= nums.length <= 10^5`, `-10^4 <= nums[i] <= 10^4`, `k` luôn hợp lệ (1 ≤ k ≤ số phần tử unique).

**5 câu hỏi:**
1. Input: n ≤ 10⁵, giá trị ±10⁴
2. Output: int[] kích thước k
3. Brute: đếm tần suất + sort → O(n log n)
4. Lãng phí: sort toàn bộ trong khi chỉ cần top k
5. Pattern: **Frequency + Bucket Sort** → O(n)

```csharp
// Cách 1: Dictionary + Sort — O(n log n)
public int[] TopKFrequent(int[] nums, int k)
{
    var freq = new Dictionary<int, int>();
    foreach (int x in nums)
        freq[x] = freq.GetValueOrDefault(x, 0) + 1;

    return freq.Keys
               .OrderByDescending(x => freq[x])
               .Take(k)
               .ToArray();
}

// Cách 2: Bucket Sort — O(n) time
// Ý tưởng: tần suất tối đa là n → tạo bucket[0..n],
// bucket[i] = danh sách số xuất hiện đúng i lần
public int[] TopKFrequentOptimal(int[] nums, int k)
{
    // Bước 1: đếm tần suất
    var freq = new Dictionary<int, int>();
    foreach (int x in nums)
        freq[x] = freq.GetValueOrDefault(x, 0) + 1;

    // Bước 2: bucket theo tần suất
    var buckets = new List<int>[nums.Length + 1];
    foreach (var (val, f) in freq)
    {
        buckets[f] ??= new List<int>();
        buckets[f].Add(val);
    }

    // Bước 3: lấy top k từ bucket cao nhất
    var result = new List<int>();
    for (int i = buckets.Length - 1; i >= 0 && result.Count < k; i--)
    {
        if (buckets[i] != null)
            result.AddRange(buckets[i]);
    }

    return result.Take(k).ToArray();
}
// Time: O(n), Space: O(n)
```

---

#### Bài 7: Product of Array Except Self (LeetCode 238)

**Đề:** Cho `int[] nums`. Trả về mảng `output` sao cho `output[i]` = tích tất cả phần tử **ngoại trừ** `nums[i]`. Không được dùng phép chia. Yêu cầu O(n) time, O(1) space thêm (output không tính).

**Ví dụ:**
```
Input:  nums = [1, 2, 3, 4]
Output: [24, 12, 8, 6]
Vì:
  output[0] = 2 * 3 * 4 = 24
  output[1] = 1 * 3 * 4 = 12
  output[2] = 1 * 2 * 4 = 8
  output[3] = 1 * 2 * 3 = 6

Input:  nums = [-1, 1, 0, -3, 3]
Output: [0, 0, 9, 0, 0]
(Edge case: có số 0 — tất cả output = 0 trừ vị trí có số 0.)

Input:  nums = [0, 0]
Output: [0, 0]
(Edge case: hai số 0 — mọi output đều = 0.)
```

**Constraint:** `2 <= nums.length <= 10^5`, `-30 <= nums[i] <= 30`, kết quả đảm bảo vừa 32-bit integer.

**5 câu hỏi:**
1. Input: n ≤ 10⁵, có thể có số 0
2. Output: int[] kích thước n
3. Brute: với mỗi i, tính tích toàn bộ trừ nums[i] → O(n²)
4. Lãng phí: tính lại tích nhiều lần; tách thành tích bên trái * tích bên phải
5. Pattern: **Prefix Product + Suffix Product** → cầu nối sang Phase 4

```csharp
// Cách 1: Hai mảng prefix/suffix — O(n) time, O(n) space thêm
public int[] ProductExceptSelf(int[] nums)
{
    int n = nums.Length;
    int[] left  = new int[n]; // left[i]  = tích nums[0..i-1]
    int[] right = new int[n]; // right[i] = tích nums[i+1..n-1]

    left[0] = 1;
    for (int i = 1; i < n; i++)
        left[i] = left[i - 1] * nums[i - 1];

    right[n - 1] = 1;
    for (int i = n - 2; i >= 0; i--)
        right[i] = right[i + 1] * nums[i + 1];

    int[] output = new int[n];
    for (int i = 0; i < n; i++)
        output[i] = left[i] * right[i];

    return output;
}

// Cách 2: O(1) space thêm — dùng chính output làm prefix
public int[] ProductExceptSelfOptimal(int[] nums)
{
    int n = nums.Length;
    int[] output = new int[n];

    // Pass 1: output[i] = tích bên trái
    output[0] = 1;
    for (int i = 1; i < n; i++)
        output[i] = output[i - 1] * nums[i - 1];

    // Pass 2: nhân thêm tích bên phải (tính on-the-fly)
    int rightProduct = 1;
    for (int i = n - 1; i >= 0; i--)
    {
        output[i] *= rightProduct;
        rightProduct *= nums[i];
    }

    return output;
}
// Time: O(n), Space: O(1) thêm (output không tính)
```

---

#### Bài 8: Longest Consecutive Sequence (LeetCode 128)

**Đề:** Cho `int[] nums` (chưa sắp xếp). Tìm độ dài của **chuỗi số nguyên liên tiếp** dài nhất. Yêu cầu O(n).

**Ví dụ:**
```
Input:  nums = [100, 4, 200, 1, 3, 2]
Output: 4
Vì: chuỗi liên tiếp dài nhất là [1, 2, 3, 4], độ dài 4.

Input:  nums = [0, 3, 7, 2, 5, 8, 4, 6, 0, 1]
Output: 9
Vì: chuỗi [0, 1, 2, 3, 4, 5, 6, 7, 8], độ dài 9. (0 trùng lặp không tính thêm.)

Input:  nums = []
Output: 0
(Edge case: mảng rỗng.)

Input:  nums = [1]
Output: 1
(Edge case: một phần tử.)
```

**Constraint:** `0 <= nums.length <= 10^5`, `-10^9 <= nums[i] <= 10^9`

**5 câu hỏi:**
1. Input: n ≤ 10⁵, giá trị ±10⁹ → không dùng mảng đếm
2. Output: int (độ dài)
3. Brute: sort rồi quét → O(n log n) — không đạt yêu cầu O(n)
4. Lãng phí: sort; thực ra chỉ cần kiểm tra "x-1 có trong set không?" để biết điểm bắt đầu
5. Pattern: **HashSet + chỉ đếm từ đầu chuỗi**

```csharp
public int LongestConsecutive(int[] nums)
{
    var numSet = new HashSet<int>(nums);   // O(n)
    int longest = 0;

    foreach (int x in numSet)
    {
        // Chỉ bắt đầu đếm nếu x là đầu chuỗi (x-1 không tồn tại)
        if (!numSet.Contains(x - 1))
        {
            int current = x;
            int length = 1;
            while (numSet.Contains(current + 1))
            {
                current++;
                length++;
            }
            longest = Math.Max(longest, length);
        }
    }
    return longest;
}
// Time: O(n) — mỗi phần tử được visit tối đa 2 lần (1 lần kiểm tra, 1 lần đếm)
// Space: O(n) — HashSet
```

**Tại sao O(n)?** Mỗi số chỉ được "đếm" (trong while) đúng 1 lần — khi nó là một phần của chuỗi bắt đầu từ đầu. Tổng tất cả các lần chạy while = n lần.

---

### Hard

---

#### Bài 9: First Missing Positive (LeetCode 41)

**Đề:** Cho `int[] nums` (chưa sort, có thể có số âm và số 0). Tìm số nguyên dương nhỏ nhất **không** có trong mảng. Yêu cầu O(n) time, **O(1) space**.

**Ví dụ:**
```
Input:  nums = [1, 2, 0]
Output: 3
Vì: 1 và 2 có mặt, số dương nhỏ nhất thiếu là 3.

Input:  nums = [3, 4, -1, 1]
Output: 2
Vì: 1 có mặt, nhưng 2 bị thiếu.

Input:  nums = [7, 8, 9, 11, 12]
Output: 1
(Edge case: thiếu ngay số 1 — số dương nhỏ nhất.)

Input:  nums = [1, 2, 3]
Output: 4
(Edge case: đủ 1..n → thiếu n+1.)
```

**Nhận xét quan trọng:** Đáp án luôn nằm trong khoảng **[1, n+1]** (n = độ dài mảng). Vì nếu mảng chứa đúng 1, 2, ..., n thì thiếu n+1; ngược lại thiếu ở đâu đó trong [1, n].

**Constraint:** `1 <= nums.length <= 10^5`, `-2^31 <= nums[i] <= 2^31 - 1`

**5 câu hỏi:**
1. Input: n phần tử, có thể có số âm / 0 / lớn hơn n
2. Output: số nguyên dương nhỏ nhất thiếu (luôn trong khoảng [1, n+1])
3. Brute: sort → O(n log n), hoặc HashSet → O(n) space — không đạt O(1) space
4. Ý tưởng: dùng chính mảng làm bảng hash!
5. Pattern: **Index-as-hash** — đặt mỗi số x vào vị trí x-1 (nếu 1 ≤ x ≤ n)

```csharp
public int FirstMissingPositive(int[] nums)
{
    int n = nums.Length;

    // Bước 1: Đặt mỗi số x vào vị trí x-1
    // nums[i] nên là i+1
    for (int i = 0; i < n; i++)
    {
        // Di chuyển nums[i] đến vị trí đúng chừng nào còn hợp lệ
        while (nums[i] >= 1 && nums[i] <= n && nums[nums[i] - 1] != nums[i])
        {
            // Swap nums[i] với nums[nums[i]-1]
            int pos = nums[i] - 1;
            (nums[i], nums[pos]) = (nums[pos], nums[i]);
        }
    }

    // Bước 2: Quét tìm vị trí đầu tiên không đúng
    for (int i = 0; i < n; i++)
        if (nums[i] != i + 1)
            return i + 1;

    // Nếu tất cả đúng: 1,2,3,...,n → thiếu n+1
    return n + 1;
}
// Time: O(n) — mỗi phần tử swap tối đa 1 lần đến vị trí đúng
// Space: O(1)
```

**Tại sao vòng while O(n) tổng?** Mỗi swap đặt ít nhất một số vào đúng vị trí vĩnh viễn. Tổng số swap ≤ n lần.

---

## Tổng kết Template C# cho Phase 1

```csharp
// Template 1: Seen-set (kiểm tra tồn tại)
var seen = new HashSet<T>();
foreach (var x in nums)
{
    if (seen.Contains(/* complement hoặc x */))
        return /* found */;
    seen.Add(x);
}

// Template 2: Complement Lookup (Two Sum kiểu)
var map = new Dictionary<int, int>(); // value → index
for (int i = 0; i < nums.Length; i++)
{
    int comp = target - nums[i];
    if (map.TryGetValue(comp, out int j))
        return new[] { j, i };
    map[nums[i]] = i;
}

// Template 3: Frequency Count
var freq = new Dictionary<T, int>();
foreach (var x in nums)
    freq[x] = freq.GetValueOrDefault(x, 0) + 1;

// Template 4: Grouping
var groups = new Dictionary<K, List<V>>();
foreach (var item in collection)
{
    var key = Normalize(item);
    if (!groups.TryGetValue(key, out var list))
        groups[key] = list = new List<V>();
    list.Add(item);
}

// Template 5: Counting Array (lowercase a-z)
int[] count = new int[26];
foreach (char c in s) count[c - 'a']++;
```

---

## Tiêu chí hoàn thành Phase 1

- [ ] Gặp bài "tìm cặp/trùng/đếm" → phản xạ nghĩ tới hash ngay.
- [ ] Tự giải ≥ 6/9 bài trên không nhìn lời giải.
- [ ] Giải thích được khi nào dùng `int[]` counting array vs `Dictionary` vs `HashSet`.
- [ ] Viết đúng Two Sum: kiểm tra complement TRƯỚC khi lưu vào map.
- [ ] Hiểu tại sao Bucket Sort trong TopK Frequent là O(n), và Longest Consecutive cũng O(n).

---

## Ghi chú học tập

> Điền sau mỗi bài:
>
> | Bài | Pattern | Điều cần nhớ | Lỗi đã mắc |
> |-----|---------|-------------|-----------|
> | Two Sum | Complement Lookup | Lưu SAU khi check | Dùng cùng index |
> | ... | ... | ... | ... |

**Thời gian dự kiến:** 1–2 tuần
**Mức độ:** ⭐⭐
