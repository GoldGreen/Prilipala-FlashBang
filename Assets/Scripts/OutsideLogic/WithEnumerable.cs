using System.Collections;
using System.Collections.Generic;

public class WithEnumerable<T> : IEnumerable<T>
{
    private ICollection<IEnumerable<T>> enumerablesList = new List<IEnumerable<T>>();

    public void Add(IEnumerable<T> enumberable)
    {
        enumberable.AddTo(enumerablesList);
    }

    public void Add(T item)
    {
        enumerablesList.Add(new[] { item });
    }

    public IEnumerator<T> GetEnumerator()
    {
        foreach (var enumberable in enumerablesList)
        {
            foreach (var item in enumberable)
            {
                yield return item;
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}