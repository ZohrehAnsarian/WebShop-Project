using DAL;
using Repository.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;

namespace Repository.EF.UnitOfWork
{
    public class EFUnitOfWork : IUnitOfWork
    {
        public int CurrentLanguageId { get; set; }

        private readonly DbContext _context;
        public EFUnitOfWork(int languageId)
        {
            CurrentLanguageId = languageId;
            _context = new WebShopEntities();
        }

        private Hashtable _repositoryList { get; set; }
        public Hashtable RepositoryIdHashtable { get; set; }
        private List<string> _repositoryTypeList { get; set; }
        public bool HasUnsavedChanges()
        {
            return _context.ChangeTracker.Entries().Any(e => e.State == EntityState.Added
                                                      || e.State == EntityState.Modified
                                                      || e.State == EntityState.Deleted);
        }
        public bool Commit()
        {
            var result = true;

            if (HasUnsavedChanges())
            {
                try
                {
                    _context.SaveChanges();

                   var entries = _context.ChangeTracker.Entries();
                    foreach (var entry in entries)
                    {
                        if (_repositoryTypeList.Contains(entry.Entity.GetType().Name))
                        {
                            PropertyInfo propertyInfo = entry.Entity.GetType().GetProperty("Id");
                            RepositoryIdHashtable.Add(entry.Entity.GetType(), propertyInfo.GetValue(entry.Entity));
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return result;

        }
        public R GetRepository<R>()
          where R : class
        {

            if (_repositoryList == null)
                _repositoryList = new Hashtable();

            if (RepositoryIdHashtable == null)
                RepositoryIdHashtable = new Hashtable();

            if (_repositoryTypeList == null)
                _repositoryTypeList = new List<string>();

            var type = typeof(R).Name;

            if (_repositoryList.ContainsKey(type))
                return (R)_repositoryList[type];

            var repositoryType = typeof(R);
            //var repositoryInstance = Activator.CreateInstance(
            //    repositoryType.MakeGenericType(typeof(E)));

            var repositoryInstance = Activator.CreateInstance(repositoryType);

            //var repositoryInstance = Activator.CreateInstance(repositoryType, _context);

            PropertyInfo propertyInfo = repositoryInstance.GetType().GetProperty("Context");

            if (propertyInfo != null && propertyInfo.CanWrite)
            {
                propertyInfo.SetValue(repositoryInstance, _context, null);
            }

            propertyInfo = repositoryInstance.GetType().GetProperty("CurrentLanguageId");

            if (propertyInfo != null && propertyInfo.CanWrite)
            {
                propertyInfo.SetValue(repositoryInstance, CurrentLanguageId, null);
            }

            _repositoryList.Add(type, repositoryInstance);
            _repositoryTypeList.Add(type);
            return (R)_repositoryList[type];

        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                disposedValue = true;
            }
        }
        ~EFUnitOfWork()
        {
            Dispose(false);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

    }
}
