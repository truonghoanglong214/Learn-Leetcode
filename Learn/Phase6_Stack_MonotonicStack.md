# Phase 6 — Stack & Monotonic Stack

> **Ngôn ngữ:** C#
> **Mục tiêu:** Giải các bài có cấu trúc **lồng nhau** (ngoặc, biểu thức, undo) bằng stack, và đặc biệt là họ bài **"phần tử lớn/nhỏ kế tiếp"** bằng monotonic stack — hạ O(n²) brute force xuống O(n).

---

## Bài học 1 — Stack Cơ Bản

### 1.1 Ý tưởng

```
Stack = LIFO (Last In, First Out): phần tử vào sau thì ra trước.

Khi nào dùng stack:
  - Cần "nhớ lại cái gần nhất đã thấy" (matching ngoặc, undo).
  - Xử lý theo thứ tự nhưng cần lùi lại (biểu thức, brace nesting).
  - Mô phỏng đệ quy bằng stack tường minh.

Trong C#: dùng Stack<T> hoặc List<T> làm stack (Push/Pop/Peek).
```

### 1.2 Template — Matching ngoặc

```csharp
// Kiểm tra chuỗi ngoặc hợp lệ
var stack = new Stack<char>();
var map = new Dictionary<char, char> { [')']='{', [']']='[', ['}']='{' };
// (Sửa map theo cặp ngoặc thực tế)

foreach (char c in s)
{
    if (c == '(' || c == '[' || c == '{')
        stack.Push(c);
    else
    {
        if (stack.Count == 0 || stack.Peek() != map[c])
            return false;
        stack.Pop();
    }
}
return stack.Count == 0;
```

---

## Bài học 2 — Monotonic Stack

### 2.1 Ý tưởng

```
Monotonic Stack = stack giữ các phần tử theo thứ tự tăng (hoặc giảm).

Vấn đề cần giải: với mỗi phần tử i, tìm phần tử j > i đầu tiên sao cho
nums[j] > nums[i] (Next Greater Element).

Brute force: với mỗi i, quét từ i+1 sang phải → O(n²).

Tại sao O(n²) lãng phí?
  - Khi đang duyệt tới j và nums[j] > nums[i], mọi k < i mà nums[k] < nums[j]
    cũng có đáp án là j — đang tính lại nhiều lần.

Monotonic stack — ý tưởng:
  - Duyệt từ trái sang phải, duy trì stack giảm dần.
  - Khi gặp nums[j], pop mọi phần tử stack nhỏ hơn nums[j]:
    → những phần tử đó tìm được đáp án là j.
  - Push j vào stack.
  - Mỗi phần tử vào stack đúng 1 lần, ra đúng 1 lần → O(n).
```

### 2.2 Template — Next Greater Element

```csharp
// result[i] = chỉ số j đầu tiên bên phải sao cho nums[j] > nums[i]
// (hoặc -1 nếu không có)
int[] result = new int[nums.Length];
Array.Fill(result, -1);
var stack = new Stack<int>(); // lưu index

for (int j = 0; j < nums.Length; j++)
{
    // Pop tất cả phần tử nhỏ hơn nums[j] — nums[j] là "next greater" của chúng
    while (stack.Count > 0 && nums[stack.Peek()] < nums[j])
        result[stack.Pop()] = j;

    stack.Push(j);
}
// Phần tử còn lại trong stack → không có next greater → giữ -1
```

### 2.3 Tăng vs Giảm — khi nào dùng cái nào?

```
Stack đơn điệu GIẢM (decreasing): pop khi phần tử mới LỚN HƠN đỉnh stack.
  → Dùng cho: Next Greater Element, Daily Temperatures, Histogram.
  → Phần tử bị pop tìm được "next greater" là phần tử mới.

Stack đơn điệu TĂNG (increasing): pop khi phần tử mới NHỎ HƠN đỉnh stack.
  → Dùng cho: Next Smaller Element, "nhìn về trái để tìm cận nhỏ hơn".
  → Phần tử bị pop tìm được "next smaller" là phần tử mới.

Quy tắc nhớ:
  - "Lớn kế tiếp" → stack giảm → pop khi nums[j] > top.
  - "Nhỏ kế tiếp" → stack tăng → pop khi nums[j] < top.
```

---

## Bài tập theo thứ tự

### Easy

---

#### Bài 1: Valid Parentheses (LeetCode 20)

**Đề:** Cho chuỗi `s` chỉ gồm `'('`, `')'`, `'{'`, `'}'`, `'['`, `']'`. Kiểm tra chuỗi có hợp lệ không: mở đóng theo đúng thứ tự và cặp.

**Ví dụ:**
```
Input:  s = "()"
Output: true

Input:  s = "()[]{}"
Output: true

Input:  s = "(]"
Output: false
Vì: '(' ghép với ']' sai.

Input:  s = ""
Output: true
(Edge case: chuỗi rỗng — hợp lệ vì không có ngoặc nào sai)

Input:  s = "["
Output: false
(Edge case: chỉ có ngoặc mở, không đóng)

Input:  s = "]"
Output: false
(Edge case: ngoặc đóng khi stack rỗng)
```

**Constraint:** `1 <= s.length <= 10^4`, s chỉ gồm 6 ký tự trên.

**Phân tích 5 câu hỏi:**
1. Input: chuỗi ký tự ngoặc, n ≤ 10⁴
2. Output: bool — hợp lệ hay không
3. Brute: thay thế dần "()", "[]", "{}" → O(n²) mỗi lần quét lại
4. Lãng phí: quét lại nhiều lần — cần nhớ "ngoặc mở gần nhất" → stack
5. Pattern: **Stack matching** — push ngoặc mở, pop + kiểm tra khi gặp ngoặc đóng

```csharp
// Brute — thay dần chuỗi, O(n²)
public bool IsValidBrute(string s)
{
    while (s.Contains("()") || s.Contains("[]") || s.Contains("{}"))
    {
        s = s.Replace("()", "").Replace("[]", "").Replace("{}", "");
    }
    return s.Length == 0;
}

// Optimal — Stack, O(n) time, O(n) space
public bool IsValid(string s)
{
    var stack = new Stack<char>();
    var close = new Dictionary<char, char>
    {
        [')'] = '(',
        [']'] = '[',
        ['}'] = '{'
    };

    foreach (char c in s)
    {
        if (c == '(' || c == '[' || c == '{')
            stack.Push(c);
        else
        {
            // Ngoặc đóng: stack phải có và đỉnh phải là cặp tương ứng
            if (stack.Count == 0 || stack.Peek() != close[c])
                return false;
            stack.Pop();
        }
    }
    return stack.Count == 0; // stack không rỗng → còn ngoặc mở chưa đóng
}
// Time: O(n), Space: O(n)
```

---

#### Bài 2: Min Stack (LeetCode 155)

**Đề:** Thiết kế stack hỗ trợ `Push(val)`, `Pop()`, `Top()`, và `GetMin()` — tất cả O(1). `GetMin()` trả về phần tử nhỏ nhất **hiện đang** trong stack.

**Ví dụ:**
```
MinStack minStack = new MinStack();
minStack.Push(-2);
minStack.Push(0);
minStack.Push(-3);
minStack.GetMin(); // → -3
minStack.Pop();
minStack.Top();    // → 0
minStack.GetMin(); // → -2

(Edge case: GetMin() sau khi pop phần tử nhỏ nhất phải cập nhật đúng min)
```

**Constraint:** `-2^31 <= val <= 2^31 - 1`, Pop/Top/GetMin chỉ gọi khi stack không rỗng, tối đa 3×10⁴ phép toán.

**Phân tích 5 câu hỏi:**
1. Input: chuỗi thao tác trên stack
2. Output: GetMin() O(1) — thách thức là cập nhật min khi pop
3. Brute: lưu một biến min, khi pop phải quét lại toàn bộ stack → O(n)
4. Lãng phí: quét lại khi pop — key insight: lưu thêm "min tại mỗi tầng"
5. Pattern: **Stack phụ theo dõi min** — mỗi lần push, ghi thêm min tương ứng

```csharp
// Optimal — Stack phụ lưu min từng tầng, O(1) mọi thao tác
public class MinStack
{
    private Stack<int> stack;
    private Stack<int> minStack; // đỉnh = min của stack chính hiện tại

    public MinStack()
    {
        stack = new Stack<int>();
        minStack = new Stack<int>();
    }

    public void Push(int val)
    {
        stack.Push(val);
        // Min mới = min(val, min hiện tại)
        int currentMin = minStack.Count == 0 ? val : Math.Min(val, minStack.Peek());
        minStack.Push(currentMin);
    }

    public void Pop()
    {
        stack.Pop();
        minStack.Pop(); // pop cùng lúc để đồng bộ
    }

    public int Top() => stack.Peek();

    public int GetMin() => minStack.Peek();
}
// Time: O(1) mọi thao tác, Space: O(n)
```

**Tại sao push min vào stack phụ thay vì chỉ cập nhật một biến?**
Vì khi `Pop()` xóa phần tử nhỏ nhất, cần biết min mới mà không quét lại. Stack phụ lưu "min tại tầng đó trở xuống" → pop cùng lúc là đủ.

---

### Medium

---

#### Bài 3: Daily Temperatures (LeetCode 739) ⭐ Bài bản lề

**Đề:** Cho `int[] temperatures`. Với mỗi ngày `i`, tìm số ngày phải chờ (minimum) để có ngày ấm hơn. Nếu không có, ghi 0.

**Ví dụ:**
```
Input:  temperatures = [73, 74, 75, 71, 69, 72, 76, 73]
Output: [1, 1, 4, 2, 1, 1, 0, 0]
Vì: ngày 0 (73°) → ngày 1 (74°) cách 1 ngày.
    ngày 2 (75°) → ngày 6 (76°) cách 4 ngày.
    ngày 6 (76°) → không có ngày nào ấm hơn → 0.

Input:  temperatures = [30, 40, 50, 60]
Output: [1, 1, 1, 0]
(Tăng dần đều)

Input:  temperatures = [30, 60, 90]
Output: [1, 1, 0]

Input:  temperatures = [90, 80, 70, 60]
Output: [0, 0, 0, 0]
(Edge case: giảm dần — không có ngày nào ấm hơn)

Input:  temperatures = [55]
Output: [0]
(Edge case: một phần tử)
```

**Constraint:** `1 <= temperatures.length <= 10^5`, `30 <= temperatures[i] <= 100`.

**Phân tích 5 câu hỏi:**
1. Input: mảng nhiệt độ, n ≤ 10⁵
2. Output: mảng số ngày chờ cho mỗi ngày
3. Brute: với mỗi i, quét từ i+1 sang phải → O(n²)
4. Lãng phí: khi duyệt tới j, mọi ngày trước đó có nhiệt độ thấp hơn đều có đáp án là j — tính lại nhiều lần → monotonic stack
5. Pattern: **Monotonic Stack giảm** — "next greater element" theo giá trị nhiệt độ

```csharp
// Brute — O(n²)
public int[] DailyTemperaturesBrute(int[] temperatures)
{
    int n = temperatures.Length;
    int[] result = new int[n];

    for (int i = 0; i < n; i++)
        for (int j = i + 1; j < n; j++)
            if (temperatures[j] > temperatures[i])
            {
                result[i] = j - i;
                break;
            }
    return result;
}

// Optimal — Monotonic Stack, O(n) time, O(n) space
public int[] DailyTemperatures(int[] temperatures)
{
    int n = temperatures.Length;
    int[] result = new int[n]; // mặc định 0
    var stack = new Stack<int>(); // lưu index, stack giảm theo nhiệt độ

    for (int j = 0; j < n; j++)
    {
        // Pop những ngày đang chờ và có nhiệt độ thấp hơn hôm nay
        while (stack.Count > 0 && temperatures[stack.Peek()] < temperatures[j])
        {
            int i = stack.Pop();
            result[i] = j - i; // số ngày chờ
        }
        stack.Push(j);
    }
    // Phần tử còn lại trong stack → không có ngày ấm hơn → result giữ 0
    return result;
}
// Time: O(n) — mỗi index vào/ra stack đúng 1 lần
// Space: O(n)
```

**Lập luận tại sao O(n):** Mỗi phần tử được `Push` đúng 1 lần và `Pop` tối đa 1 lần → tổng số thao tác stack ≤ 2n = O(n).

---

#### Bài 4: Next Greater Element II (LeetCode 503)

**Đề:** Cho `int[] nums` là mảng **vòng tròn** (sau phần tử cuối lại quay về đầu). Với mỗi phần tử, tìm next greater element theo chiều đi vòng. Trả về -1 nếu không có.

**Ví dụ:**
```
Input:  nums = [1, 2, 1]
Output: [2, -1, 2]
Vì: 1 → 2 (kế tiếp); 2 → không có số nào lớn hơn 2 trong vòng; 1 → 2 (quay đầu).

Input:  nums = [1, 2, 3, 4, 3]
Output: [2, 3, 4, -1, 4]
Vì: 4 → không có gì lớn hơn → -1.

Input:  nums = [5, 4, 3, 2, 1]
Output: [-1, 5, 5, 5, 5]
(Edge case: mảng giảm dần — mỗi phần tử trừ 5 đều tìm được 5 khi quay vòng)

Input:  nums = [1]
Output: [-1]
(Edge case: một phần tử, vòng về chính nó không lớn hơn)
```

**Constraint:** `1 <= nums.length <= 10^4`, `-10^9 <= nums[i] <= 10^9`.

```csharp
// Optimal — Duyệt 2 vòng (mô phỏng circular), O(n)
public int[] NextGreaterElements(int[] nums)
{
    int n = nums.Length;
    int[] result = new int[n];
    Array.Fill(result, -1);
    var stack = new Stack<int>(); // lưu index

    // Duyệt 2n bước để mô phỏng vòng tròn (dùng % để lấy index thực)
    for (int i = 0; i < 2 * n; i++)
    {
        int idx = i % n;
        while (stack.Count > 0 && nums[stack.Peek()] < nums[idx])
            result[stack.Pop()] = nums[idx];

        // Chỉ push ở vòng đầu (i < n) để tránh xử lý index lặp lại
        if (i < n) stack.Push(idx);
    }
    return result;
}
// Time: O(n), Space: O(n)
```

**Mẹo vòng tròn:** Duyệt 2 lần (2n bước, lấy index = `i % n`). Vòng đầu xây stack; vòng sau chỉ pop (không push thêm) để cấp đáp án cho phần tử chưa giải quyết.

---

#### Bài 5: Evaluate Reverse Polish Notation (LeetCode 150)

**Đề:** Cho `string[] tokens` biểu diễn biểu thức hậu tố (RPN). Tính giá trị biểu thức. Phép tính: `+`, `-`, `*`, `/` (chia nguyên về phía 0).

**Ví dụ:**
```
Input:  tokens = ["2", "1", "+", "3", "*"]
Output: 9
Vì: ((2 + 1) * 3) = 9

Input:  tokens = ["4", "13", "5", "/", "+"]
Output: 6
Vì: (4 + (13 / 5)) = (4 + 2) = 6

Input:  tokens = ["10", "6", "9", "3", "+", "-11", "*", "/", "*", "17", "+", "5", "+"]
Output: 22

Input:  tokens = ["3"]
Output: 3
(Edge case: chỉ một số)

Input:  tokens = ["2", "3", "-"]
Output: -1
(Edge case: kết quả âm)
```

**Constraint:** `1 <= tokens.length <= 10^4`, token là số nguyên hoặc toán tử, phép tính luôn hợp lệ, kết quả trong phạm vi int 32-bit.

```csharp
// Optimal — Stack mô phỏng, O(n) time, O(n) space
public int EvalRPN(string[] tokens)
{
    var stack = new Stack<long>(); // dùng long tránh tràn số trung gian
    var ops = new HashSet<string> { "+", "-", "*", "/" };

    foreach (string token in tokens)
    {
        if (ops.Contains(token))
        {
            long b = stack.Pop(); // toán hạng phải (đầu tiên ra)
            long a = stack.Pop(); // toán hạng trái
            long val = token switch
            {
                "+" => a + b,
                "-" => a - b,
                "*" => a * b,
                "/" => a / b, // C# chia nguyên tự nhiên về phía 0
                _   => throw new InvalidOperationException()
            };
            stack.Push(val);
        }
        else
            stack.Push(long.Parse(token));
    }
    return (int)stack.Pop();
}
// Time: O(n), Space: O(n)
```

**Lưu ý thứ tự pop:** Khi pop hai toán hạng, `b = Pop()` là toán hạng **phải** (vào sau thì ra trước), `a = Pop()` là toán hạng **trái**. Quan trọng với `-` và `/` vì không giao hoán.

---

#### Bài 6: Car Fleet (LeetCode 853)

**Đề:** Có `n` xe xuất phát tại vị trí `position[i]` với tốc độ `speed[i]`, cùng đến đích `target` (đơn vị: dặm). Xe sau không vượt xe trước — nếu bắt kịp thì chạy cùng tốc độ (thành một đội). Đếm số đội xe đến đích.

**Ví dụ:**
```
Input:  target = 12, position = [10,8,0,5,3], speed = [2,4,1,1,3]
Output: 3
Vì: Thời gian đến đích: xe tại 10→1h, 8→2h, 0→12h, 5→7h, 3→3h.
    Sắp theo vị trí giảm dần: (10,1h),(8,2h),(5,7h),(3,3h),(0,12h).
    10→1h: đội 1.
    8→2h: chậm hơn 10 (1h) → bắt kịp → đội 1.
    5→7h: chậm hơn 8 (2h) → bắt kịp → đội 1? Không — thực ra đội 5 bắt kịp đội 8 thì đội 8 đã thuộc đội 10 (1h < 7h là không bắt kịp) → đội 2.
    3→3h: bắt kịp 5 (7h > 3h nên 3 bắt kịp 5 không? 3 xuất phát sau về vị trí nhưng nhanh hơn) → đội 2.
    0→12h: không bắt kịp → đội 3.
    → 3 đội.

Input:  target = 10, position = [3], speed = [3]
Output: 1
(Edge case: một xe)

Input:  target = 100, position = [0, 2, 4], speed = [4, 2, 1]
Output: 1
Vì: xe 0 nhanh nhất, đuổi kịp và hợp nhất tất cả.
```

**Constraint:** `n == position.length == speed.length`, `1 <= n <= 10^5`, `0 < target <= 10^6`, `0 < speed[i] <= 10^6`, vị trí phân biệt, tất cả position < target.

```csharp
// Optimal — Sort + Stack, O(n log n)
public int CarFleet(int target, int[] position, int[] speed)
{
    int n = position.Length;

    // Ghép vị trí và thời gian đến đích, sắp theo vị trí giảm dần (gần đích nhất trước)
    var cars = new (int pos, double time)[n];
    for (int i = 0; i < n; i++)
        cars[i] = (position[i], (double)(target - position[i]) / speed[i]);

    Array.Sort(cars, (a, b) => b.pos.CompareTo(a.pos));

    // Stack lưu thời gian đến đích của các đội
    var stack = new Stack<double>();

    foreach (var (_, time) in cars)
    {
        // Xe hiện tại có bắt kịp đội phía trước không?
        // Nếu time <= top của stack → bắt kịp → cùng đội (không push)
        // Nếu time > top → không bắt kịp → đội mới
        if (stack.Count == 0 || time > stack.Peek())
            stack.Push(time);
        // else: bắt kịp đội phía trước, không tạo đội mới
    }
    return stack.Count;
}
// Time: O(n log n) do sort, Space: O(n)
```

**Lập luận cốt lõi:** Sắp theo vị trí giảm dần (xe gần đích trước). Xe sau chỉ hợp vào đội xe trước nếu đến đích **nhanh hơn hoặc bằng** → nó sẽ bị kìm lại. Dùng stack lưu thời gian đội dẫn đầu: nếu xe mới nhanh hơn đội đứng đầu → bị kìm → không push; nếu chậm hơn → đội mới → push.

---

### Hard

---

#### Bài 7: Largest Rectangle in Histogram (LeetCode 84) ⭐ Monotonic Stack đỉnh cao

**Đề:** Cho `int[] heights` là chiều cao các cột của biểu đồ cột (mỗi cột rộng 1). Tìm diện tích hình chữ nhật lớn nhất có thể vẽ trong biểu đồ.

**Ví dụ:**
```
Input:  heights = [2, 1, 5, 6, 2, 3]
Output: 10
Vì: hình chữ nhật cao 5, rộng 2 (cột 2–3) = diện tích 10.

Input:  heights = [2, 4]
Output: 4
Vì: hình chữ nhật cao 4, rộng 1 = 4; hoặc cao 2, rộng 2 = 4.

Input:  heights = [1, 1, 1, 1]
Output: 4
(Edge case: tất cả bằng nhau → rộng toàn bộ)

Input:  heights = [6]
Output: 6
(Edge case: một cột)

Input:  heights = [5, 4, 3, 2, 1]
Output: 9
(Edge case: giảm dần — hình chữ nhật tốt nhất là cột 2 rộng 3: 3×3=9)
```

**Constraint:** `1 <= heights.length <= 10^5`, `0 <= heights[i] <= 10^4`.

**Phân tích 5 câu hỏi:**
1. Input: mảng chiều cao, n ≤ 10⁵
2. Output: diện tích lớn nhất (int)
3. Brute: với mỗi cặp (i, j) tính min(heights[i..j]) × (j−i+1) → O(n²) hoặc O(n³)
4. Lãng phí: phần tử nhỏ hơn trong mảng giới hạn chiều cao của hình chữ nhật chứa nó; cần tìm nhanh "cột thấp hơn kế tiếp bên trái/phải" → monotonic stack
5. Pattern: **Monotonic Stack tăng** — tìm "previous smaller" và "next smaller" để xác định chiều rộng tối đa của hình chữ nhật cao `heights[i]`

```csharp
// Brute — O(n²)
public int LargestRectangleAreaBrute(int[] heights)
{
    int n = heights.Length, maxArea = 0;
    for (int i = 0; i < n; i++)
    {
        int minH = heights[i];
        for (int j = i; j < n; j++)
        {
            minH = Math.Min(minH, heights[j]);
            maxArea = Math.Max(maxArea, minH * (j - i + 1));
        }
    }
    return maxArea;
}

// Optimal — Monotonic Stack (tăng), O(n) time, O(n) space
public int LargestRectangleArea(int[] heights)
{
    int n = heights.Length;
    int maxArea = 0;
    // Stack lưu index, chiều cao tăng dần từ dưới lên
    var stack = new Stack<int>();

    for (int i = 0; i <= n; i++)
    {
        // Sentinel: khi i == n, dùng chiều cao 0 để flush toàn bộ stack
        int h = i == n ? 0 : heights[i];

        // Pop khi gặp cột thấp hơn đỉnh stack → đỉnh bị "chặn" bởi h
        while (stack.Count > 0 && heights[stack.Peek()] > h)
        {
            int height = heights[stack.Pop()];
            // Chiều rộng: từ "previous smaller" (đỉnh stack mới) đến i - 1
            int width = stack.Count == 0 ? i : i - stack.Peek() - 1;
            maxArea = Math.Max(maxArea, height * width);
        }
        stack.Push(i);
    }
    return maxArea;
}
// Time: O(n), Space: O(n)
```

**Lập luận cốt lõi:** Với mỗi cột `i` có chiều cao `h`, hình chữ nhật cao `h` kéo dài được từ "cột thấp hơn kế tiếp bên trái" đến "cột thấp hơn kế tiếp bên phải". Monotonic stack tăng tự động cung cấp cả hai thông tin này: khi pop `i` do gặp `j` (phải) thấp hơn, đỉnh stack mới (trái) chính là "previous smaller bên trái". Chiều rộng = `j - leftBound - 1`.

---

#### Bài 8: Trapping Rain Water (LeetCode 42) — Cách Stack

**Đề:** Cho `int[] height` là bản đồ cao thấp. Tính lượng nước mưa có thể giữ lại (mỗi đơn vị chiều ngang rộng 1).

**Ví dụ:**
```
Input:  height = [0, 1, 0, 2, 1, 0, 1, 3, 2, 1, 2, 1]
Output: 6

Input:  height = [4, 2, 0, 3, 2, 5]
Output: 9

Input:  height = [3, 0, 0, 2, 0, 4]
Output: 10

Input:  height = [1, 1]
Output: 0
(Edge case: không có chỗ trũng)

Input:  height = [3]
Output: 0
(Edge case: một cột)
```

**Constraint:** `n == height.length`, `1 <= n <= 2×10^4`, `0 <= height[i] <= 10^5`.

**Phân tích 5 câu hỏi:**
1. Input: mảng chiều cao, n ≤ 2×10⁴
2. Output: tổng lượng nước (int)
3. Brute: với mỗi cột, tính min(maxLeft, maxRight) − height[i] → O(n) với prefix
4. Lãng phí: phải tính trước prefix max (Phase 4) hoặc dùng two pointers (Phase 2)
5. Pattern (phiên bản stack): **Monotonic Stack** — tính nước theo từng "tầng" nằm ngang

```csharp
// Cách 1: Prefix Max — O(n) time, O(n) space (cách từ Phase 4)
public int TrapPrefix(int[] height)
{
    int n = height.Length;
    int[] maxLeft = new int[n], maxRight = new int[n];

    for (int i = 1; i < n; i++)
        maxLeft[i] = Math.Max(maxLeft[i-1], height[i-1]);
    for (int i = n-2; i >= 0; i--)
        maxRight[i] = Math.Max(maxRight[i+1], height[i+1]);

    int water = 0;
    for (int i = 0; i < n; i++)
        water += Math.Max(0, Math.Min(maxLeft[i], maxRight[i]) - height[i]);
    return water;
}

// Cách 2: Monotonic Stack — O(n) time, O(n) space
// Tư duy: tính nước theo "lớp nằm ngang" giữa bờ trái và bờ phải
public int Trap(int[] height)
{
    int water = 0;
    var stack = new Stack<int>(); // stack giảm — lưu index

    for (int i = 0; i < height.Length; i++)
    {
        while (stack.Count > 0 && height[stack.Peek()] < height[i])
        {
            int bottom = stack.Pop();   // điểm đáy chỗ trũng
            if (stack.Count == 0) break; // không có bờ trái → không giữ nước

            int left = stack.Peek();    // bờ trái
            int right = i;              // bờ phải (hiện tại)
            int boundedHeight = Math.Min(height[left], height[right]) - height[bottom];
            int width = right - left - 1;
            water += boundedHeight * width;
        }
        stack.Push(i);
    }
    return water;
}
// Time: O(n), Space: O(n)
```

**So sánh ba cách:** Prefix Max (đơn giản nhất, dễ hiểu). Two Pointers (O(1) space, Phase 2). Stack (tính theo tầng ngang, khác góc nhìn — gặp lại từ Phase 2).

---

## Tổng kết Pattern

| Dấu hiệu đề | Pattern |
|-------------|---------|
| "Ngoặc hợp lệ", "lồng nhau", "khớp cặp" | Stack matching — push mở, pop+kiểm tra khi đóng |
| "Min/Max của stack", "undo gần nhất" | Stack phụ theo dõi cực trị |
| "Phần tử lớn hơn kế tiếp", "nhiệt độ ấm hơn" | Monotonic stack **giảm** — pop khi gặp lớn hơn |
| "Phần tử nhỏ hơn kế tiếp", "next smaller" | Monotonic stack **tăng** — pop khi gặp nhỏ hơn |
| "Diện tích hình chữ nhật trong histogram" | Monotonic stack tăng + sentinel cuối |
| "Nước mưa giữ lại" | Prefix max / Two pointers / Stack theo tầng |
| Mảng vòng tròn + next greater | Duyệt 2n lần, index = `i % n` |

**Quy tắc vàng:** Brute force là "với mỗi i, quét sang phải O(n)" → nghĩ ngay tới monotonic stack. Mỗi phần tử vào/ra đúng 1 lần → O(n).
