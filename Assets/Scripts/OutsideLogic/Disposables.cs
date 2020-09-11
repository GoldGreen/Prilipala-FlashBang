using System;
using System.Collections.Generic;

public interface IDisposableIEnumerable : IDisposable, IEnumerable<IDisposable> { }
public interface IDisposableCollection : IDisposableIEnumerable, ICollection<IDisposable> { }

public class Disposables : List<IDisposable>, IDisposableCollection
{
    public void Dispose()
    {
        ForEach(x => x.Dispose());
    }
}