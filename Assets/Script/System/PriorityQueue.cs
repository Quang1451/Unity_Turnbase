using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue<T>
{
	private List<Tuple<T, int>> elements = new List<Tuple<T, int>>();

	public List<Tuple<T, int>> ListQueue => elements;

    public void SoftQueue()
    {
		elements.Sort((p1, p2) => p1.Item2.CompareTo(p2.Item2));
	}

	public int Count
	{
		get { return elements.Count; }
	}

	public void Enqueue(T item, int priorityValue)
	{
		elements.Add(Tuple.Create(item, priorityValue));
	}

	public Tuple<T, int> Dequeue()
	{
		int bestPriorityIndex = 0;

		for (int i = 0; i < elements.Count; i++)
		{
			if (elements[i].Item2 < elements[bestPriorityIndex].Item2)
			{
				bestPriorityIndex = i;
			}
		}

		Tuple<T, int> bestItem = elements[bestPriorityIndex];
		elements.RemoveAt(bestPriorityIndex);
		return bestItem;
	}

	public Tuple<T, int> Peek()
	{
		int bestPriorityIndex = 0;

		for (int i = 0; i < elements.Count; i++)
		{
			if (elements[i].Item2 < elements[bestPriorityIndex].Item2)
			{
				bestPriorityIndex = i;
			}
		}
		return elements[bestPriorityIndex];
	}

	public void UpdatePriorityAll(int number)
    {
		for (int i = 0; i < elements.Count; i++)
		{
			elements[i] = Tuple.Create(elements[i].Item1, elements[i].Item2 - number);
		}
	}
}