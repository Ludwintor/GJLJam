using System.Collections.Generic;
using UnityEngine;

public static class ListExtensions
{
    public static void Shuffle<T>(this List<T> list)
    {
        List<T> listToShuffle = list;
        List<T> shuffledList = new List<T>();

        for (int i = listToShuffle.Count - 1; i >= 0; i--)
        {
            int index = Random.Range(0, i);
            T element = listToShuffle[index];
            listToShuffle.RemoveAt(index);
            shuffledList.Insert(0, element);

            // If isn't last index
            if (listToShuffle.Count != 0 && index != listToShuffle.Count - 1)
            {
            int lastIndex = listToShuffle.Count - 1;
            T lastElement = listToShuffle[lastIndex];
            listToShuffle.RemoveAt(lastIndex);
            listToShuffle.Insert(index, lastElement);
            }

        }

        list.AddRange(shuffledList);
    }
}
