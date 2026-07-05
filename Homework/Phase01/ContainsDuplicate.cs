using System;

/// <summary>
**Đề:**Cho `int[] nums`. Trả về `true` nếu có phần tử xuất hiện ≥ 2 lần.

**Ví dụ: **
```
Input: nums = [1, 2, 3, 1]
Output: true
Vì: 1 xuất hiện ở index 0 và index 3.

Input:  nums = [1, 2, 3, 4]
Output: false
Vì: không có phần tử nào lặp lại.

Input:  nums = [1, 1, 1, 3, 3, 4, 3, 2, 4, 2]
Output: true
```

**Constraint:** `1 <= nums.length <= 10 ^ 5`, `-10 ^ 9 <= nums[i] <= 10 ^ 9`
/// </summary>

    public bool DuplicateNumber(int[] nums)
    {
        HashSet<int> list = new HashSet<int>();
        for (int i = 0; i < nums.Length; i++)
        {
            if (!list.Add(nums[i]))
                return true;
        }

        return false;
    }

