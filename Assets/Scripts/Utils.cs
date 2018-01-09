using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class Utils {
	
	private static System.Random rng = new System.Random();

	/// <summary>
	/// Shuffle the specified list.
	/// Used to shuffle the order of trophys.
	/// </summary>
	/// <param name="list">List to be shuffled.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public static void Shuffle<T>(this IList<T> list)
	{
		int n = list.Count;
		while (n > 1) {
			n--;
			int k = rng.Next (n + 1);
			T value = list [k];
			list [k] = list [n];
			list [n] = value;
		}
	}
}
