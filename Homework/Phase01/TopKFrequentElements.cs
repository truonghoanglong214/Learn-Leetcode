using System;
/// <summary>
**Đề:**Cho `int[] nums` và `int k`. Trả về k phần tử xuất hiện nhiều nhất (thứ tự kết quả không quan trọng).

**Ví dụ:**
```
Input: nums = [1, 1, 1, 2, 2, 3], k = 2
Output: [1, 2]
Vì: 1 xuất hiện 3 lần (nhiều nhất), 2 xuất hiện 2 lần (nhiều thứ hai).

Input:  nums = [1], k = 1
Output: [1]
(Edge case:
    một phần tử, k = 1.)

Input: nums = [1, 2], k = 2
Output: [1, 2]
(Edge case:
    hai phần tử cùng tần suất, lấy cả hai.)
```

**Constraint:** `1 <= nums.length <= 10 ^ 5`, `-10 ^ 4 <= nums[i] <= 10 ^ 4`, `k` luôn hợp lệ(1 ≤ k ≤ số phần tử unique).
/// </summary>
public class Class1
{
    public int[] TopKFrequent(int[] nums, int k)
    {
        var seen = new Dictionary<int, int>();
        
        foreach(int num in nums)
        {
            seen[num] = seen.GetValueOrDefault(num, 0) + 1;
        }

        return seen.Values.OrderByDescending(x => seen[x]).Take(k).ToArray();
    }
}
