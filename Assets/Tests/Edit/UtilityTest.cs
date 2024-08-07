using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class UtilityTest
{
    [Test]
    public void Shuffle_Arrays_ReturnsDifferentEveryTime()
    {
        List<int> list = new List<int>();
        list.AddRange(new[] { 1, 2, 3, 4, 5, 6 });

        List<int> list2 = Utility.Shuffle(list);
        List<int> list3 = Utility.Shuffle(list);

        Assert.IsTrue(List.Equals(list, list));
        Assert.IsFalse(List.Equals(list2, list));
        Assert.IsFalse(List.Equals(list3, list));
        Assert.IsFalse(List.Equals(list2, list3));
    }

    [Test]
    public void Shuffle_Arrays_SizeIsTheSame()
    {
        List<int> list = new List<int>();

        list.AddRange(new[] { 1, 2, 3 });
        List<int> list2 = Utility.Shuffle(list);
        Assert.IsTrue(list.Count == list2.Count);

        list.AddRange(new[] { 1, 2, 3, 4, 5, 6 });
        list2 = Utility.Shuffle(list);
        Assert.IsTrue(list.Count == list2.Count);

    }
    [Test]
    public void AddCards_Arrays_SizeIsRightAndNoForbiddenCards()
    {

        List<int> allNumbers = new List<int>();
        allNumbers = Utility.AddUnoCardNumbers(allNumbers);

        Assert.IsTrue(allNumbers.Count == (121 - 4));

        Assert.IsTrue(allNumbers.Contains(0));
        Assert.IsTrue(allNumbers.Contains(1));
        Assert.IsTrue(allNumbers.Contains(111));

        Assert.IsFalse(allNumbers.Contains(56));
        Assert.IsFalse(allNumbers.Contains(70));
        Assert.IsFalse(allNumbers.Contains(84));
        Assert.IsFalse(allNumbers.Contains(98));
    }
}
