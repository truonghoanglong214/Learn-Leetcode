using System;


/// <summary>
Đề:** Cho `string[] strs`. Nhóm các anagram lại với nhau. Thứ tự trong nhóm và thứ tự các nhóm không quan trọng.

**Ví dụ: **
```
Input: strs = ["eat", "tea", "tan", "ate", "nat", "bat"]
Output: [["bat"], ["nat", "tan"], ["ate", "eat", "tea"]]
Vì: "eat","tea","ate" là anagram nhau; "tan","nat" là anagram nhau; "bat" đứng riêng.

Input: strs = [""]
Output: [[""]]
(Edge case:
    chuỗi rỗng tạo một nhóm riêng.)

Input: strs = ["a"]
Output: [["a"]]
```

**Constraint:** `1 <= strs.length <= 10 ^ 4`, `0 <= strs[i].length <= 100`, chỉ gồm chữ thường a - z.
/// </summary>
public class Class1
{
	public IList<IList<string>> GroupAnagrams(string[] strs)
    {
        var seen = new Dictionary<string, List<string>>();

    }
}
