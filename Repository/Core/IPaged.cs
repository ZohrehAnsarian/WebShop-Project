using System.Collections.Generic;

namespace Repository.Core
{
    public interface IPaged<T> : IEnumerable<T>
    {
        int Count { get; }
        IEnumerable<T> GetRange(int index, int count);

    }

}
