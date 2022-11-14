using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Autocomplete
{
    internal class AutocompleteTask
    {
        /// <returns>
        /// Возвращает первую фразу словаря, начинающуюся с prefix.
        /// </returns>
        /// <remarks>
        /// Эта функция уже реализована, она заработает,  как только вы выполните задачу в файле LeftBorderTask
        /// </remarks>
        public static string FindFirstByPrefix(IReadOnlyList<string> phrases, string prefix)
        {
            var index = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count) + 1;
            if (index < phrases.Count && phrases[index].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                return phrases[index];

            return null;
        }

        /// <returns>
        /// Возвращает первые в лексикографическом порядке count (или меньше, если их меньше count) 
        /// элементов словаря, начинающихся с prefix.
        /// </returns>
        /// <remarks>Эта функция должна работать за O(log(n) + count)</remarks>
        public static string[] GetTopByPrefix(IReadOnlyList<string> phrases, string prefix, int count)
        {
            // тут стоит использовать написанный ранее класс LeftBorderTask
            var left = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count) + 1;
            if (phrases.Count - left < count)
                count = phrases.Count - left;
            var tempList = new List<string>();
            for (var i = 0; i < count; i++)
            {
                if (phrases[left + i].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    tempList.Add(phrases[left + i]);
            }
            
            return tempList.ToArray();
        }

        /// <returns>
        /// Возвращает количество фраз, начинающихся с заданного префикса
        /// </returns>
        public static int GetCountByPrefix(IReadOnlyList<string> phrases, string prefix)
        {
            // тут стоит использовать написанные ранее классы LeftBorderTask и RightBorderTask
            var left = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count);
            var right = RightBorderTask.GetRightBorderIndex(phrases, prefix, -1, phrases.Count) - 1;
            return right - left;
        }
    }

    [TestFixture]
    public class AutocompleteTests
    {
        [Test]
        [TestCase(new string[] {"aa", "ab", "ac", "bc"}, "a", 2,new string[] {"aa", "ab"})]
        [TestCase(new string[] {"a", "b", "c", "c", "d", "e"}, "c", 10,new string[] {"c", "c"})]
        [TestCase(new string[] {"aa", "ab", "ac"}, "z", 2,new string[] {})]
        [TestCase(new string[] {}, "z", 2,new string[] {})]
        public void TopByPrefix_IsEmpty_WhenNoPhrases(IReadOnlyList<string> phrases, string prefix, int count, string[] topByprefix)
        {
            var array = AutocompleteTask.GetTopByPrefix(phrases, prefix, count);
            Assert.AreEqual(topByprefix, array);
        }

        [Test]
        [TestCase(new string[] {"aa", "ab", "bc", "bd", "be", "ca", "cb"}, "a", 2)]
        [TestCase(new string[] {"aa", "ab", "bc", "bd", "be", "ca", "cb"}, "b", 3)]
        [TestCase(new string[] {"aa", "ab", "bc", "bd", "be", "ca", "cb"}, "d", 0)]
        [TestCase(new string[] {"aa", "ab", "bc", "bd", "be", "ca", "cb"}, "cb", 1)]
        [TestCase(new string[] {"aa", "ab", "bc", "bd", "be", "ca", "cb"}, "az", 0)]
        public void CountByPrefix_IsTotalCount_WhenEmptyPrefix(IReadOnlyList<string> phrases, string prefix, int totalCount)
        {
            var total = AutocompleteTask.GetCountByPrefix(phrases, prefix);
            Assert.AreEqual(totalCount, total);
        }
    }
}