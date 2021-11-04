using DAL;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System;
using System.Linq.Expressions;

namespace Repository.EF.Base
{
    public class EFBaseRepository<E> where E : class
    {
        #region Main members
        public WebShopEntities Context { get; set; }
        public int CurrentLanguageId { get; set; }
        public EFBaseRepository()
        {

        }
        public EFBaseRepository(WebShopEntities context)
        {
            Context = context;
        }
        protected E Get(object id)
        {
            return Context.Set<E>().Find(id);
        }
        protected IEnumerable<E> Get()
        {
            return Context.Set<E>().ToArray();
        }
        protected IEnumerable<E> Get(Expression<Func<E, bool>> predicate)
        {
            //var test = Context.Set<E>().Where(predicate.Compile());
            //var test1 = Context.Set<E>().Where(predicate.Compile()).AsEnumerable();
            return Context.Set<E>().Where(predicate).AsEnumerable();
        }
        protected IEnumerable<E> Get(Func<E, bool> predicate)
        {
            return Context.Set<E>().Where(predicate);
        }
        protected E Add(E entity)
        {
            Context.Set<E>().Add(entity);
            return entity;
        }
        protected void Update(E entity)
        {
            Context.Set<E>().Attach(entity);
            Context.Entry<E>(entity).State = EntityState.Modified;
        }
        protected void Delete(E entity)
        {
            Context.Set<E>().Attach(entity);
            Context.Set<E>().Remove(entity);
        }

        public virtual bool InsertRule() { return true; }
        public virtual bool UpdateRule() { return true; }
        public virtual bool DeleteRule() { return true; }
        #endregion

        #region IDisposable Support
        //private bool disposedValue = false; // To detect redundant calls
        //protected virtual void Dispose(bool disposing)
        //{
        //    if (!disposedValue)
        //    {
        //        if (disposing)
        //        {
        //            Context.Dispose();
        //        }
        //        disposedValue = true;
        //    }
        //}
        //~EFBaseRepository()
        //{
        //    Dispose(false);
        //}
        //public void Dispose()
        //{
        //    Dispose(true);
        //    GC.SuppressFinalize(this);
        //}
        #endregion
    }
}
