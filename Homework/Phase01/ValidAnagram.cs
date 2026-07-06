using System;

/// <summary>
Bài 3: Valid Anagram (LeetCode 242) — Frequency Counting

**Đề:**Cho `string s, t`. Trả về `true` nếu t là anagram của s (cùng ký tự, khác thứ tự).

**Ví dụ:**
```
Input: s = "anagram", t = "nagaram"
Output: true
Vì: cả hai có cùng ký tự: a×3, n×1, g×1, r×1, m×1.

Input:  s = "rat", t = "car"
Output: false
Vì: "rat" có t, "car" không có t (và "car" có c, "rat" không có c).

Input:  s = "a", t = "ab"
Output: false
Vì: độ dài khác nhau → không thể là anagram.
/// </summary>
public class Class1
{
	public bool isAnagram(string s, string t)
	{
		if (s.Length != t.Length) 
			return false;

		var same = new Dictionary<char, int>();

		foreach (char c in s)
			same[c] = same.GetValueOrDefaults(c, 0) + 1;

		foreach (char c in t)
		{
			if(!same.ContainsKey(c) ||  same[c] == 0)
				return false;

			same[c]--;
		}

		return true;
	}


	
}
