using System;
using System.Collections;

namespace Repository.Core
{
    public interface IUnitOfWork : IDisposable
    {

        Hashtable RepositoryIdHashtable { get; set; }

        R GetRepository<R>()
            where R : class;
        bool Commit();
    }
}
