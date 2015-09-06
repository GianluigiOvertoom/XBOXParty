using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public static class ExtentionMethods
{
    public static void Shuffle<T>(this IList<T> list)
    {
        //https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle
        System.Random rand = new System.Random();

        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rand.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
