using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Utils
{
    public static class RandomHelper
    {
        public static T GetRandomElement<T>(this T[] array)
        {
            return array[Random.Range(0, array.Length)];
        }

        public static T GetRandomElement<T>(this List<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }
    
        public static T GetRandomElementInFirstCountElements<T>(this T[] array, int whichFirsCount)
        {
            return array[Random.Range(0, Math.Min(whichFirsCount, array.Length))];
        }

        public static T GetRandomElementInFirstCountElements<T>(this List<T> list, int whichFirsCount)
        {
            return list[Random.Range(0, Math.Min(whichFirsCount, list.Count))];
        }

        public static T[] GetRandomElements<T>(this T[] array, int elementCount)
        {
            var pickedElements = new List<T>();
            var currentElements = new List<T>(array);

            for (int i = 0; i < elementCount; i++)
            {
                var randomIndex = Random.Range(0, currentElements.Count);
                pickedElements.Add(currentElements[randomIndex]);
                currentElements.RemoveAt(randomIndex);
            }

            return pickedElements.ToArray();
        }
    
        public static T[] GetRandomElements<T>(this List<T> array, int elementCount)
        {
            var pickedElements = new List<T>();
            var currentElements = new List<T>(array);

            for (int i = 0; i < elementCount; i++)
            {
                var randomIndex = Random.Range(0, currentElements.Count);
                pickedElements.Add(currentElements[randomIndex]);
                currentElements.RemoveAt(randomIndex);
            }

            return pickedElements.ToArray();
        }

        public static T[] GetShuffledElements<T>(this T[] array)
        {
            return array.GetRandomElements(array.Length);
        }

        public static T[] GetShuffledElements<T>(this List<T> list)
        {
            return list.GetRandomElements(list.Count);
        }

        public static void Shuffle<T>(this List<T> list)
        {
            var shuffledElements = list.GetShuffledElements();
            list.Clear();
            list.AddRange(shuffledElements);
        }
    }

    public class RandomWeightedElementSelector<T>
    {
        private readonly Dictionary<T, int> _weights;

        public RandomWeightedElementSelector()
        {
            _weights = new Dictionary<T, int>();
        }

        public RandomWeightedElementSelector(ElementWeightPair<T>[] typeWeightPairs)
        {
            _weights = new Dictionary<T, int>();

            foreach (var typeWeightPair in typeWeightPairs)
            {
                Add(typeWeightPair.element, typeWeightPair.weight);
            }
        }

        public void Add(T element, int weight)
        {
            if (weight < 1)
            {
                return;
            }

            _weights[element] = weight;
        }

        public void Remove(T element)
        {
            _weights.Remove(element);
        }

        public bool IsEmpty => _weights.Count == 0;

        public T SelectOne()
        {
            var sortedSpawnRate = Sort(_weights);

            var sum = 0;
            foreach (var spawn in _weights)
            {
                sum += spawn.Value;
            }

            var roll = Random.Range(0, sum);

            var selected = sortedSpawnRate[sortedSpawnRate.Count - 1].Key;
            foreach (var spawn in sortedSpawnRate)
            {
                if (roll < spawn.Value)
                {
                    selected = spawn.Key;
                    break;
                }

                roll -= spawn.Value;
            }

            return selected;
        }

        private List<KeyValuePair<T, int>> Sort(Dictionary<T, int> weights)
        {
            var list = new List<KeyValuePair<T, int>>(weights);
        
            list.Sort(
                delegate(KeyValuePair<T, int> firstPair, KeyValuePair<T, int> nextPair)
                {
                    return firstPair.Value.CompareTo(nextPair.Value);
                
                });

            return list;
        }
    
        public static void Shuffle<T>(IList<T> list)
        {
            var n = list.Count;
            for (int i = n - 1; i > 0; i--)
            {
                var j = Random.Range(0, i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
    }

    [Serializable]
    public class ElementWeightPair<T>
    {
        public T element;
        public int weight;

        public ElementWeightPair(T element, int weight)
        {
            this.element = element;
            this.weight = weight;
        }
    }
}