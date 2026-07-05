# Phase 2 — Two Pointers

> **Ngôn ngữ:** C#
> **Mục tiêu:** Loại bỏ vòng lặp lồng nhau trên dữ liệu có thứ tự. Biến O(n²) thành O(n) bằng cách dùng hai con trỏ di chuyển thông minh thay vì so từng cặp.

---

## Bài học 1 — Two Pointers Đối Đầu (Opposite Ends)

### 1.1 Ý tưởng

```
Đặt left = 0, right = n-1.
Hai con trỏ tiến vào giữa cho đến khi left >= right.

Điều kiện dùng được: dữ liệu có tính đơn điệu (thường là đã sắp xếp).
Khi tổng quá lớn → thu right lại.
Khi tổng quá nhỏ → tiến left lên.
```

**Tại sao đúng?** Khi mảng sorted và tổng `nums[left] + nums[right] > target`, mọi cặp `(left, right-1), (left, right-2), ...` đều không cần xét nữa — vì `nums[right]` là lớn nhất còn lại. Vì thế ta an toàn thu `right`.

### 1.2 Template cơ bản

```csharp
int left = 0, right = nums.Length - 1;
while (left < right)
{
    int sum = nums[left] + nums[right];
    if (sum == target)
    {
        // Xử lý kết quả
        left++; right--;
    }
    else if (sum < target)
        left++;       // cần tổng lớn hơn → tiến left
    else
        right--;      // cần tổng nhỏ hơn → thu right
}
```

---

## Bài học 2 — Two Pointers Cùng Chiều (Fast & Slow / Read-Write)

### 2.1 Fast & Slow

```
slow = con trỏ ghi (vị trí cần điền kết quả)
fast = con trỏ đọc (duyệt qua toàn bộ mảng)

Dùng để: dồn phần tử hợp lệ, xóa tại chỗ, loại trùng.
```

```csharp
// Template: giữ lại phần tử thỏa điều kiện
int slow = 0;
for (int fast = 0; fast < nums.Length; fast++)
{
    if (/* nums[fast] hợp lệ */)
    {
        nums[slow] = nums[fast];
        slow++;
    }
}
// Mảng kết quả nằm trong nums[0..slow-1]
```

### 2.2 Phân biệt hai kiểu

```
Opposite Ends: left + right tiến vào giữa
  → dùng khi: tìm cặp/bộ có tổng, palindrome, nén hai đầu
  → yêu cầu: mảng sorted (hoặc có thể sort mà không mất đúng)

Fast & Slow cùng chiều: slow lùi, fast tiến
  → dùng khi: dồn phần tử, xóa in-place, loại trùng
  → không yêu cầu sorted
```

---

## Bài học 3 — Fix Một + Two Pointers

### 3.1 Kỹ thuật

```
Dùng cho: 3Sum, 4Sum, k-Sum

Bước 1: Sort mảng
Bước 2: Cố định phần tử thứ nhất (vòng for)
Bước 3: Dùng two pointers cho phần còn lại

Xử lý duplicate: bỏ qua khi phần tử hiện tại == phần tử trước.
```

```csharp
Array.Sort(nums);
for (int i = 0; i < nums.Length - 2; i++)
{
    if (i > 0 && nums[i] == nums[i - 1]) continue; // skip duplicate i

    int left = i + 1, right = nums.Length - 1;
    while (left < right)
    {
        int sum = nums[i] + nums[left] + nums[right];
        if (sum == 0)
        {
            // Lưu kết quả
            while (left < right && nums[left] == nums[left + 1]) left++;   // skip dup left
            while (left < right && nums[right] == nums[right - 1]) right--; // skip dup right
            left++; right--;
        }
        else if (sum < 0) left++;
        else right--;
    }
}
```

---

## Bài tập theo thứ tự

### Easy

---

#### Bài 1: Valid Palindrome (LeetCode 125) — Opposite Ends

**Đề:** Cho `string s`. Chuẩn hóa: chỉ giữ ký tự chữ cái và chữ số, đổi về chữ thường. Kiểm tra xem chuỗi sau khi chuẩn hóa có phải palindrome không.

**Ví dụ:**
```
Input:  s = "A man, a plan, a canal: Panama"
Output: true
Vì: sau chuẩn hóa → "amanaplanacanalpanama" — đọc xuôi = đọc ngược.

Input:  s = "race a car"
Output: false
Vì: sau chuẩn hóa → "raceacar" — đọc ngược là "racaecar", khác.

Input:  s = " "
Output: true
(Edge case: chỉ có khoảng trắng → sau chuẩn hóa = chuỗi rỗng → palindrome.)
```

**Constraint:** `1 <= s.length <= 2 * 10^5`, s gồm ký tự ASCII in được.

**Phân tích 5 câu hỏi:**
1. Input: chuỗi hỗn hợp, cần lọc + chuẩn hóa trước
2. Output: bool
3. Brute: tạo chuỗi mới, so với Reverse → O(n) time, O(n) space
4. Tối ưu hơn: không cần tạo chuỗi mới → two pointers trực tiếp trên string gốc, bỏ qua ký tự không hợp lệ
5. Pattern: **Opposite Ends** + bỏ qua ký tự không hợp lệ

```csharp
// Brute Force — O(n) time, O(n) space
public bool IsPalindromeBrute(string s)
{
    string clean = new string(s.Where(char.IsLetterOrDigit)
                               .Select(char.ToLower)
                               .ToArray());
    return clean == new string(clean.Reverse().ToArray());
}

// Optimal — O(n) time, O(1) space
public bool IsPalindrome(string s)
{
    int left = 0, right = s.Length - 1;
    while (left < right)
    {
        // Bỏ qua ký tự không hợp lệ
        while (left < right && !char.IsLetterOrDigit(s[left]))  left++;
        while (left < right && !char.IsLetterOrDigit(s[right])) right--;

        if (char.ToLower(s[left]) != char.ToLower(s[right]))
            return false;

        left++;
        right--;
    }
    return true;
}
// Time: O(n), Space: O(1)
```

---

#### Bài 2: Two Sum II – Input Array Is Sorted (LeetCode 167)

**Đề:** Cho `int[] numbers` đã sắp xếp tăng dần (1-indexed). Tìm `[i, j]` (1-indexed) sao cho `numbers[i-1] + numbers[j-1] == target`. Đảm bảo đúng một đáp án. Chỉ dùng O(1) space.

**Ví dụ:**
```
Input:  numbers = [2, 7, 11, 15], target = 9
Output: [1, 2]
Vì: numbers[0] + numbers[1] = 2 + 7 = 9.

Input:  numbers = [2, 3, 4], target = 6
Output: [1, 3]
Vì: numbers[0] + numbers[2] = 2 + 4 = 6.

Input:  numbers = [-1, 0], target = -1
Output: [1, 2]
(Edge case: số âm, mảng hai phần tử.)
```

**Constraint:** `2 <= numbers.length <= 3 * 10^4`, `-1000 <= numbers[i] <= 1000`, `-1000 <= target <= 1000`.

**Phân tích 5 câu hỏi:**
1. Input: đã sorted, 1-indexed output, O(1) space → không dùng hash map
2. Output: int[] hai phần tử (1-indexed)
3. Brute: hai vòng lặp O(n²)
4. Lãng phí: mảng đã sorted, khi tổng quá lớn chắc chắn phải thu phải, quá nhỏ chắc chắn phải tiến trái → two pointers
5. Pattern: **Opposite Ends** trên mảng sorted

```csharp
public int[] TwoSumII(int[] numbers, int target)
{
    int left = 0, right = numbers.Length - 1;
    while (left < right)
    {
        int sum = numbers[left] + numbers[right];
        if (sum == target)
            return new[] { left + 1, right + 1 }; // 1-indexed
        else if (sum < target)
            left++;
        else
            right--;
    }
    return Array.Empty<int>(); // không bao giờ xảy ra theo đề
}
// Time: O(n), Space: O(1)
```

**So sánh với Two Sum (Phase 1):** Two Sum dùng hash vì mảng không sorted. Two Sum II dùng two pointers vì mảng sorted + yêu cầu O(1) space.

---

#### Bài 3: Move Zeroes (LeetCode 283) — Read-Write

**Đề:** Cho `int[] nums`. Di chuyển tất cả số 0 về cuối mảng, giữ nguyên thứ tự các phần tử khác 0. Thao tác **in-place**.

**Ví dụ:**
```
Input:  nums = [0, 1, 0, 3, 12]
Output: [1, 3, 12, 0, 0]
Vì: 1, 3, 12 giữ nguyên thứ tự; hai số 0 dồn về cuối.

Input:  nums = [0]
Output: [0]
(Edge case: một phần tử bằng 0.)

Input:  nums = [1, 2, 3]
Output: [1, 2, 3]
(Edge case: không có số 0.)
```

**Constraint:** `1 <= nums.length <= 10^4`, `-2^31 <= nums[i] <= 2^31 - 1`.

**Phân tích 5 câu hỏi:**
1. Input: mảng có thể có số 0 xen kẽ
2. Output: chính mảng đó (in-place)
3. Brute: tạo mảng mới — O(n) time, O(n) space
4. Tối ưu: dùng slow pointer ghi phần tử khác 0, fast duyệt qua → O(1) space
5. Pattern: **Fast & Slow (Read-Write)**

```csharp
// Brute — O(n) time, O(n) space
public void MoveZeroesBrute(int[] nums)
{
    int[] temp = nums.Where(x => x != 0).ToArray();
    for (int i = 0; i < temp.Length; i++) nums[i] = temp[i];
    for (int i = temp.Length; i < nums.Length; i++) nums[i] = 0;
}

// Optimal — O(n) time, O(1) space
public void MoveZeroes(int[] nums)
{
    int slow = 0; // vị trí kế tiếp cần ghi phần tử khác 0
    for (int fast = 0; fast < nums.Length; fast++)
    {
        if (nums[fast] != 0)
        {
            nums[slow] = nums[fast];
            slow++;
        }
    }
    // Điền 0 vào phần còn lại
    for (int i = slow; i < nums.Length; i++)
        nums[i] = 0;
}
// Time: O(n), Space: O(1)

// Variant — dùng swap (ít lần ghi hơn khi nhiều phần tử khác 0)
public void MoveZeroesSwap(int[] nums)
{
    int slow = 0;
    for (int fast = 0; fast < nums.Length; fast++)
    {
        if (nums[fast] != 0)
        {
            (nums[slow], nums[fast]) = (nums[fast], nums[slow]);
            slow++;
        }
    }
}
```

---

### Medium

---

#### Bài 4: 3Sum (LeetCode 15) — Fix + Two Pointers ⭐ Bài bản lề

**Đề:** Cho `int[] nums`. Tìm tất cả bộ ba `[nums[i], nums[j], nums[k]]` với `i ≠ j ≠ k` và `nums[i] + nums[j] + nums[k] == 0`. Kết quả **không trùng lặp**.

**Ví dụ:**
```
Input:  nums = [-1, 0, 1, 2, -1, -4]
Output: [[-1, -1, 2], [-1, 0, 1]]
Vì: (-1)+(-1)+2=0 và (-1)+0+1=0. Không lặp lại cùng bộ.

Input:  nums = [0, 1, 1]
Output: []
Vì: không có bộ ba nào tổng = 0.

Input:  nums = [0, 0, 0]
Output: [[0, 0, 0]]
(Edge case: ba số 0, đúng một bộ ba.)

Input:  nums = []
Output: []
(Edge case: mảng rỗng.)
```

**Constraint:** `0 <= nums.length <= 3000`, `-10^5 <= nums[i] <= 10^5`.

**Phân tích 5 câu hỏi:**
1. Input: n ≤ 3000, có thể có duplicate
2. Output: tất cả bộ ba unique, không quan tâm thứ tự
3. Brute: ba vòng lặp lồng O(n³)
4. Lãng phí: cố định i, dùng O(n²) tìm cặp j+k — còn tối ưu hơn bằng two pointers sau khi sort
5. Pattern: **Sort + Fix i + Opposite Ends** + bỏ qua duplicate

```csharp
// Brute — O(n³)
public IList<IList<int>> ThreeSumBrute(int[] nums)
{
    var result = new HashSet<string>();
    var ans = new List<IList<int>>();
    for (int i = 0; i < nums.Length; i++)
        for (int j = i + 1; j < nums.Length; j++)
            for (int k = j + 1; k < nums.Length; k++)
                if (nums[i] + nums[j] + nums[k] == 0)
                {
                    var tri = new[] { nums[i], nums[j], nums[k] };
                    Array.Sort(tri);
                    string key = $"{tri[0]},{tri[1]},{tri[2]}";
                    if (result.Add(key))
                        ans.Add(tri);
                }
    return ans;
}

// Optimal — O(n²) time, O(1) space thêm
public IList<IList<int>> ThreeSum(int[] nums)
{
    Array.Sort(nums); // O(n log n)
    var result = new List<IList<int>>();

    for (int i = 0; i < nums.Length - 2; i++)
    {
        // Tối ưu: nếu nums[i] > 0 thì tổng luôn > 0 → dừng
        if (nums[i] > 0) break;

        // Bỏ qua duplicate cho i
        if (i > 0 && nums[i] == nums[i - 1]) continue;

        int left = i + 1, right = nums.Length - 1;
        while (left < right)
        {
            int sum = nums[i] + nums[left] + nums[right];
            if (sum == 0)
            {
                result.Add(new List<int> { nums[i], nums[left], nums[right] });

                // Bỏ qua duplicate cho left và right
                while (left < right && nums[left]  == nums[left  + 1]) left++;
                while (left < right && nums[right] == nums[right - 1]) right--;
                left++;
                right--;
            }
            else if (sum < 0)
                left++;
            else
                right--;
        }
    }
    return result;
}
// Time: O(n²), Space: O(1) thêm (không kể output)
```

**Lý do phải sort trước:** Sau khi sort, tính đơn điệu được đảm bảo — tổng tăng khi tiến left, giảm khi thu right. Không sort thì không thể dùng two pointers đúng.

**Lý do bỏ qua duplicate đúng cách:**
- Với `i`: chỉ skip khi `i > 0` (không skip `i = 0`)
- Với `left/right`: skip TRONG khi `left < right`, TRƯỚC khi `left++ / right--`

---

#### Bài 5: Container With Most Water (LeetCode 11) — Opposite Ends Tham Lam

**Đề:** Cho `int[] height` (n phần tử). Chọn hai chỉ số `i < j` để tạo container chứa nước nhiều nhất. Lượng nước = `min(height[i], height[j]) * (j - i)`.

**Ví dụ:**
```
Input:  height = [1, 8, 6, 2, 5, 4, 8, 3, 7]
Output: 49
Vì: chọn i=1 (height=8), j=8 (height=7): min(8,7) * (8-1) = 7*7 = 49.

Input:  height = [1, 1]
Output: 1
(Edge case: hai phần tử, diện tích = min(1,1) * 1 = 1.)

Input:  height = [4, 3, 2, 1, 4]
Output: 16
Vì: chọn i=0 (height=4), j=4 (height=4): min(4,4) * 4 = 16.
```

**Constraint:** `n == height.length`, `2 <= n <= 10^5`, `0 <= height[i] <= 10^4`.

**Phân tích 5 câu hỏi:**
1. Input: mảng height, không sorted
2. Output: int (diện tích lớn nhất)
3. Brute: xét mọi cặp (i, j) → O(n²)
4. Lãng phí: khi đang xét `(left, right)`, nếu `height[left] < height[right]` thì mọi cặp `(left, right-1), (left, right-2), ...` đều cho diện tích nhỏ hơn hoặc bằng (vì chiều cao bị giới hạn bởi `height[left]`, và độ rộng giảm). Vì vậy an toàn bỏ `left`.
5. Pattern: **Opposite Ends** + lập luận tham lam

```csharp
// Brute — O(n²)
public int MaxAreaBrute(int[] height)
{
    int max = 0;
    for (int i = 0; i < height.Length; i++)
        for (int j = i + 1; j < height.Length; j++)
            max = Math.Max(max, Math.Min(height[i], height[j]) * (j - i));
    return max;
}

// Optimal — O(n)
public int MaxArea(int[] height)
{
    int left = 0, right = height.Length - 1;
    int maxWater = 0;

    while (left < right)
    {
        int water = Math.Min(height[left], height[right]) * (right - left);
        maxWater = Math.Max(maxWater, water);

        // Bỏ cạnh thấp hơn — vì giữ cạnh cao hơn mới có cơ hội tăng diện tích
        if (height[left] < height[right])
            left++;
        else
            right--;
    }
    return maxWater;
}
// Time: O(n), Space: O(1)
```

**Tại sao bỏ cạnh thấp hơn?** Giả sử `height[left] < height[right]`. Mọi cặp `(left, right')` với `right' < right` đều có: chiều cao ≤ `height[left]` (đã là nút thắt) và độ rộng < `(right - left)`. Vậy diện tích chỉ nhỏ hơn. Không cần xét nữa → bỏ `left`.

---

#### Bài 6: Sort Colors (LeetCode 75) — Dutch National Flag (Ba con trỏ)

**Đề:** Cho `int[] nums` gồm 0, 1, 2. Sắp xếp **in-place** để tất cả 0 ở đầu, 1 ở giữa, 2 ở cuối. Không dùng sort chuẩn.

**Ví dụ:**
```
Input:  nums = [2, 0, 2, 1, 1, 0]
Output: [0, 0, 1, 1, 2, 2]

Input:  nums = [2, 0, 1]
Output: [0, 1, 2]

Input:  nums = [0]
Output: [0]
(Edge case: một phần tử.)

Input:  nums = [1, 1, 1]
Output: [1, 1, 1]
(Edge case: tất cả là 1.)
```

**Constraint:** `1 <= nums.length <= 300`, `nums[i]` thuộc {0, 1, 2}.

**Phân tích 5 câu hỏi:**
1. Input: chỉ có 3 giá trị 0/1/2
2. Output: in-place, O(1) space
3. Brute: đếm số lượng 0/1/2 rồi điền lại → O(n), nhưng hai lần duyệt
4. Tối ưu: một lần duyệt — Dutch National Flag
5. Pattern: **Ba con trỏ** (low, mid, high)

```csharp
// Counting — O(n), hai lần duyệt
public void SortColorsCounting(int[] nums)
{
    int c0 = nums.Count(x => x == 0);
    int c1 = nums.Count(x => x == 1);
    for (int i = 0; i < nums.Length; i++)
        nums[i] = i < c0 ? 0 : i < c0 + c1 ? 1 : 2;
}

// Dutch National Flag — O(n), một lần duyệt, O(1) space
// Bất biến:
//   nums[0..low-1]  = 0  (đã đặt đúng)
//   nums[low..mid-1]= 1  (đã đặt đúng)
//   nums[mid..high] = chưa xử lý
//   nums[high+1..n-1] = 2 (đã đặt đúng)
public void SortColors(int[] nums)
{
    int low = 0, mid = 0, high = nums.Length - 1;

    while (mid <= high)
    {
        if (nums[mid] == 0)
        {
            (nums[low], nums[mid]) = (nums[mid], nums[low]);
            low++;
            mid++;
        }
        else if (nums[mid] == 1)
        {
            mid++;
        }
        else // nums[mid] == 2
        {
            (nums[mid], nums[high]) = (nums[high], nums[mid]);
            high--;
            // Không tăng mid vì nums[mid] mới có thể là 0 hoặc 1
        }
    }
}
// Time: O(n), Space: O(1)
```

**Lưu ý:** Khi swap với `high`, không tăng `mid` ngay vì phần tử vừa swap về có thể là 0 (cần xử lý tiếp). Khi swap với `low`, tăng cả `low` và `mid` vì phần tử từ `low` về luôn là 1 (vùng [low..mid-1] là 1).

---

### Hard

---

#### Bài 7: Trapping Rain Water (LeetCode 42) — Two Pointers với Max Hai Phía

**Đề:** Cho `int[] height` đại diện bức tường. Tính lượng nước mưa bị chặn lại sau cơn mưa.

**Ví dụ:**
```
Input:  height = [0, 1, 0, 2, 1, 0, 1, 3, 2, 1, 2, 1]
Output: 6
Vì: 6 đơn vị nước bị giữ lại (vẽ hình để thấy rõ).

Input:  height = [4, 2, 0, 3, 2, 5]
Output: 9
Vì: tại index 1: min(4,5)-2=2; index 2: min(4,5)-0=4; index 3: min(4,5)-3=1... tổng 9.

Input:  height = [1, 0, 1]
Output: 1
(Edge case: đơn giản nhất — 1 đơn vị nước.)

Input:  height = [3, 0, 0, 2, 0, 4]
Output: 10
```

**Constraint:** `n == height.length`, `1 <= n <= 2 * 10^4`, `0 <= height[i] <= 10^5`.

**Phân tích 5 câu hỏi:**
1. Input: mảng height không sorted
2. Output: tổng lượng nước (int)
3. Brute: với mỗi vị trí i, tìm maxLeft và maxRight → O(n²)
4. Better: prefix max trái và phải → O(n) time, O(n) space
5. Optimal: two pointers — không cần lưu hai mảng, dùng `maxLeft` và `maxRight` biến
   Pattern: **Opposite Ends** + "nút thắt thấp hơn quyết định"

```csharp
// Prefix arrays — O(n) time, O(n) space
public int TrapPrefix(int[] height)
{
    int n = height.Length;
    int[] leftMax  = new int[n];
    int[] rightMax = new int[n];

    leftMax[0] = height[0];
    for (int i = 1; i < n; i++)
        leftMax[i] = Math.Max(leftMax[i - 1], height[i]);

    rightMax[n - 1] = height[n - 1];
    for (int i = n - 2; i >= 0; i--)
        rightMax[i] = Math.Max(rightMax[i + 1], height[i]);

    int water = 0;
    for (int i = 0; i < n; i++)
        water += Math.Min(leftMax[i], rightMax[i]) - height[i];

    return water;
}

// Two Pointers — O(n) time, O(1) space
// Nguyên lý: nước tại i = min(maxLeft, maxRight) - height[i].
// Nếu maxLeft < maxRight → kết quả phụ thuộc maxLeft → xử lý phía trái an toàn.
// Nếu maxRight <= maxLeft → xử lý phía phải an toàn.
public int Trap(int[] height)
{
    int left = 0, right = height.Length - 1;
    int maxLeft = 0, maxRight = 0;
    int water = 0;

    while (left < right)
    {
        if (height[left] < height[right])
        {
            if (height[left] >= maxLeft)
                maxLeft = height[left];
            else
                water += maxLeft - height[left]; // lượng nước tại left
            left++;
        }
        else
        {
            if (height[right] >= maxRight)
                maxRight = height[right];
            else
                water += maxRight - height[right]; // lượng nước tại right
            right--;
        }
    }
    return water;
}
// Time: O(n), Space: O(1)
```

**Tại sao two pointers đúng?** Tại mỗi bước, ta xử lý phía có bức tường thấp hơn. Vì nước tại vị trí đó được quyết định bởi `min(maxLeft, maxRight)`, mà phía thấp hơn đang biết chắc `max` của nó (đã track theo), còn phía bên kia chỉ có thể cao hơn hoặc bằng — vì vậy `min` chính là `max` của phía thấp hơn.

---

## Tổng kết Template C# cho Phase 2

```csharp
// Template 1: Opposite Ends (mảng sorted)
int left = 0, right = nums.Length - 1;
while (left < right)
{
    int sum = nums[left] + nums[right];
    if (sum == target) { /* xử lý */ left++; right--; }
    else if (sum < target) left++;
    else right--;
}

// Template 2: Fast & Slow / Read-Write
int slow = 0;
for (int fast = 0; fast < nums.Length; fast++)
{
    if (/* nums[fast] hợp lệ */)
        nums[slow++] = nums[fast];
}

// Template 3: Fix i + Two Pointers (3Sum/kSum)
Array.Sort(nums);
for (int i = 0; i < nums.Length - 2; i++)
{
    if (i > 0 && nums[i] == nums[i - 1]) continue; // bỏ dup i
    int left = i + 1, right = nums.Length - 1;
    while (left < right)
    {
        int sum = nums[i] + nums[left] + nums[right];
        if (sum == 0)
        {
            /* lưu kết quả */
            while (left < right && nums[left]  == nums[left  + 1]) left++;
            while (left < right && nums[right] == nums[right - 1]) right--;
            left++; right--;
        }
        else if (sum < 0) left++;
        else right--;
    }
}

// Template 4: Dutch National Flag (3 vùng)
int lo = 0, mid = 0, hi = nums.Length - 1;
while (mid <= hi)
{
    if (nums[mid] == 0)      { Swap(lo, mid); lo++; mid++; }
    else if (nums[mid] == 1) { mid++; }
    else                     { Swap(mid, hi); hi--; } // không tăng mid
}
```

---

## Tiêu chí hoàn thành Phase 2

- [ ] Tự giải 3Sum không nhìn lời giải, xử lý đúng duplicate.
- [ ] Phân biệt rõ khi nào dùng opposite ends vs fast & slow.
- [ ] Giải thích được tại sao bỏ cạnh thấp hơn trong Container With Most Water.
- [ ] Viết đúng Dutch National Flag (lý do không tăng mid khi swap với high).
- [ ] Hiểu tại sao two pointers yêu cầu tính đơn điệu (sorted hoặc điều kiện tăng/giảm rõ).

---

## Ghi chú học tập

> Điền sau mỗi bài:
>
> | Bài | Pattern | Điều cần nhớ | Lỗi đã mắc |
> |-----|---------|-------------|-----------|
> | Valid Palindrome | Opposite Ends | Skip ký tự không hợp lệ trước khi so sánh | |
> | 3Sum | Fix + Two Pointers | Sort trước; bỏ dup i khi i>0; bỏ dup left/right sau khi tìm | |
> | ... | ... | ... | ... |

**Thời gian dự kiến:** 4–7 ngày
**Mức độ:** ⭐⭐
