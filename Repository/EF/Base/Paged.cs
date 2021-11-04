using Repository.Core;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Repository.EF.Base
{
    public class Paged<E> : IPaged<E>
    {
        private readonly IQueryable<E> source;

        public Paged(IQueryable<E> source)
        {
            this.source = source;
        }

        public IEnumerator<E> GetEnumerator()
        {
            return source.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count
        {
            get { return source.Count(); }
        }

        public IEnumerable<E> GetRange(int index, int count)
        {
            return source.Skip(index).Take(count);
        }
    }
}
