using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MedIn.Db.Entities;
using MedIn.Domain.Entities;
using MedIn.OziCms.Controllers;
using MedIn.OziCms.ViewModels;

namespace MedIn.Web.Mvc
{
    public abstract class BaseController : OziController
    {
        protected bool IsPjax
        {
            get { return Request.Headers["X-PJAX"] != null; }
        }

        private Dictionary<Type, object> cache = new Dictionary<Type, object>();

        public IWebContext WebContext { get { return DependencyResolver.Current.GetService<IWebContext>(); } }

        protected IQueryable<T> GetEntities<T>() where T : class
        {
            if (cache.ContainsKey(typeof(T)))
            {
                return (IQueryable<T>)cache[typeof(T)];
            }
            var db = DependencyResolver.Current.GetService<DataModelContext>();
            var list = db.CreateObjectSet<T>();
            IQueryable<T> result = list;
            cache.Add(typeof(T), result);
            return result;
        }

        protected IQueryable<T> GetVisible<T>(int? page = null, int pageSize = 0) where T : class, IVisibleEntity
        {
            var result = GetEntities<T>().Where(arg => arg.Visibility);
            if (page.HasValue)
            {
                result = result.ToList().Skip((page.Value - 1) * pageSize).Take(pageSize).AsQueryable();
            }
            return result;
        }

        protected IQueryable<T> GetSortedVisible<T, TKey>(Func<T, TKey> sortAction, int? page = null, int pageSize = 0) where T : class, IVisibleEntity
        {
            var result = GetEntities<T>().Where(arg => arg.Visibility);
            if (sortAction != null)
            {
                result = result.ToList().OrderBy(sortAction).AsQueryable();
            }
            if (page.HasValue)
            {
                result = result.Skip((page.Value - 1) * pageSize).Take(pageSize);
            }
            return result;
        }

        protected IQueryable<T> GetSortedVisible<T>(int? page = null, int pageSize = 0) where T : class, IVisibleEntity
        {
            var result = GetEntities<T>().Where(arg => arg.Visibility);
            if (typeof(ISortableEntity).IsAssignableFrom(typeof(T)))
            {
                result = result.ToList().OrderBy(arg => ((ISortableEntity)arg).Sort).AsQueryable();
            }
            //else if (typeof(ISortableByDateEntity).IsAssignableFrom(typeof(T)))
            //{
            //    result = result.ToList().OrderByDescending(arg => ((ISortableByDateEntity)arg).SortDate).AsQueryable();
            //}
            if (page.HasValue)
            {
                result = result.Skip((page.Value - 1) * pageSize).Take(pageSize);
            }
            return result;
        }

        protected T GetById<T>(int id) where T : class, IEntity
        {
            return GetEntities<T>().FirstOrDefault(arg => arg.Id == id);
        }

        protected T GetByAlias<T>(string alias) where T : class
        {
            if (typeof(IHaveAliasEntity).IsAssignableFrom(typeof(T)))
            {
                return GetEntities<T>().ToList().FirstOrDefault(arg => ((IHaveAliasEntity)arg).Alias == alias);
            }
            return null;
        }

        protected IQueryable<T> GetSorted<T>() where T : class, ISortableEntity
        {
            return GetEntities<T>().OrderBy(arg => arg.Sort);
        }

        protected PagerViewModel CreatePager<T>(int page, int count, Func<int, ActionResult> action) where T : class, IVisibleEntity
        {
            var result = new PagerViewModel
                {
                    Action = action,
                    ItemsCount = GetVisible<T>().Count(),
                    Page = page,
                    PageSize = count
                };
            return result;
        }

        protected int? GetItemPage<T>(int pageSize, T item) where T : class, IVisibleEntity
        {
            var id = item.Id;
            var items = GetVisible<T>();
            if (item is ISortableEntity)
            {
                items = items.ToList().OrderBy(arg => ((ISortableEntity)arg).Sort).AsQueryable();
            }
            var index = items.ToList().TakeWhile(e => e.Id != id).Count();
            var result = index / pageSize + 1;
            return result == 1 ? null : (int?)result;

        }
    }
}
