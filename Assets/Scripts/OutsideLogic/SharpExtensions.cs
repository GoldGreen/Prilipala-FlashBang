using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class SharpExtensions
{
    public static Tin AddTo<Tin, TCol>(this Tin element, ICollection<TCol> collection)
    where Tin : TCol
    {
        collection.Add(element);
        return element;
    }

    public static T FindFirst<T>(this IEnumerable<T> objects, Predicate<T> predicate, out int index)
    {
        int i = 0;
        foreach (var obj in objects)
        {
            if (predicate(obj))
            {
                index = i;
                return obj;
            }
            i++;
        }

        index = -1;
        return default;
    }

    public static bool AllIs<TIn>(Predicate<TIn> predicate, params TIn[] objects)
    {
        foreach (var obj in objects)
        {
            if (!predicate(obj))
                return false;
        }

        return true;
    }

    public static bool AnyIs<TIn>(Predicate<TIn> predicate, params TIn[] objects)
    {
        foreach (var obj in objects)
        {
            if (predicate(obj))
                return true;
        }

        return false;
    }
    public static float FixeTo(this float value, int digits) => (float)Math.Round(value, digits);

    public static bool IsOneOf<T>(this T value, params T[] keys) => keys.Contains(value);
    public static bool IsOneOf<T>(this T value, IEnumerable<T> keys) => keys.Contains(value);
    public static WithEnumerable<T> With<T>(this IEnumerable<T> source1, IEnumerable<T> source2)
    {
        var withEnumerable = new WithEnumerable<T>
        {
            source1,
            source2
        };
        return withEnumerable;
    }

    public static WithEnumerable<T> With<T>(this WithEnumerable<T> withEnumerable, IEnumerable<T> source)
    {
        withEnumerable.Add(source);
        return withEnumerable;
    }

    public static ICollection<T> With<T>(this T source1, T source2)
    {
        ICollection<T> collection = new List<T>
        {
            source1,
            source2
        };

        return collection;
    }

    public static ICollection<T> With<T>(this ICollection<T> collection, T source)
    {
        source.AddTo(collection);
        return collection;
    }

    public static T GetRandomElement<T>(this IEnumerable<T> items)
    => items.ElementAt(UnityEngine.Random.Range(0, items.Count()));

    public static T GetRandomElement<T>(this ICollection<T> items)
    => items.ElementAt(UnityEngine.Random.Range(0, items.Count));

    public static T GetRandomElement<T>(this IReadOnlyCollection<T> items)
    => items.ElementAt(UnityEngine.Random.Range(0, items.Count));

    public static T GetRandomElement<T>(this T[] items)
    => items[UnityEngine.Random.Range(0, items.Length)];

    public static float Randomize(this float value, float multiply)
    {
        var min = value / multiply;
        var max = value * multiply;

        return UnityEngine.Random.Range(Mathf.Min(min, max), Mathf.Max(min, max));
    }

    public static IEnumerable<T> ForEach<T>(this IEnumerable<T> items, Action<T> action)
    {
        foreach (var item in items)
            action(item);

        return items;
    }

    public static bool Equal<T1, T2>() => typeof(T1) == typeof(T2);

    public static T NotNull<T>(this T obj, Action<T> action)
    where T : class
    {
        if (obj != null)
            action(obj);

        return obj;
    }
}