﻿using System;
using System.Collections.Generic;

namespace WordPuzzles
{
    public static class ListShuffler 
    {
        private static readonly Random RandomNumberGenerator = new Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = RandomNumberGenerator.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

    }
}