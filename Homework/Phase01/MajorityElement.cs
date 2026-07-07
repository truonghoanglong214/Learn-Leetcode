using System;
/// <summary>
**Đề:**Cho `int[] nums` (n phần tử). Tìm phần tử xuất hiện **> n/2 lần**. Đảm bảo luôn tồn tại.

**Ví dụ:**
```
Input: nums = [3, 2, 3]
Output: 3
Vì: 3 xuất hiện 2 lần > 3/2 = 1.5 lần.

Input:  nums = [2, 2, 1, 1, 1, 2, 2]
Output: 2
Vì: 2 xuất hiện 4 lần > 7/2 = 3.5 lần.

Input:  nums = [1]
Output: 1
(Edge case: mảng một phần tử.)
/// </summary>
public class Class1
{
	public int MajorityElement(int[] nums)
	{
		var seen = new Dictionary<int , int>();
		foreach(int x in nums)
		{			
			seen[x] = seen.GetValueOrDefault(x, 0) + 1;
            if (seen(x) > nums.Length / 2)
                return x;
        }

		return -1;
	}

	public int MajorityElement2(int[] nums) 
	{
		int candidate = nums[0] , count = 1;

		for(int i = 1; i < nums.Length; i++) 
			
		{
			if (count == 0)
			{
				candidate = nums[i];
				count = 1;
			}
			else if (candidate == nums[i]) count++;
			else count--;
		}
        
        return candidate;
	}

    public int MajorityElement3(int[] nums)
	{
		int candidate = 0, count = 0;

		foreach(int x in nums)
		{
			if (count == 0) candidate = x;
            else if (candidate == x) count++;
			else count--;
		}	
		return candidate;
	}
}
