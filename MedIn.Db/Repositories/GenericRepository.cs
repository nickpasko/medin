using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using MedIn.Db.Entities;
using MedIn.Db.Infrastructure;
using MedIn.Libs.Services;

namespace MedIn.Db.Repositories
{
    public abstract class GenericRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, IEntity, new()
    {
    	private readonly IDatabaseFactory _db;
		private ObjectContext _context;

		public ObjectContext Context
        {
            get
            {
            	return _context ?? (_context = _db.Get());
            }
        }

		[DebuggerStepThrough]
    	protected GenericRepository(IDatabaseFactory db)
        {
        	_db = db;
        }

		private ObjectSet<TEntity> _sourceEntitySet;
		private IQueryable<TEntity> _entitySet;

	    protected string Lang()
	    {
			var lang = DependencyResolver.Current.GetService<ILocalizationProvider>();
		    return lang.GetLanguageName();
	    }

	    protected bool IsLocalizable()
	    {
		    return typeof (ILocalizableEntity).IsAssignableFrom(typeof (TEntity));
	    }

	    protected ObjectSet<TEntity> SourceEntitySet
	    {
		    get { return _sourceEntitySet ?? (_sourceEntitySet = Context.CreateObjectSet<TEntity>()); }
	    }

	    protected IQueryable<TEntity> EntitySet
        {
            get
            {
	            return _entitySet ?? (_entitySet = IsLocalizable() ?
														SourceEntitySet.Where("it.Lang=@lang", new ObjectParameter("lang", Lang())) :
														SourceEntitySet);
            }
        }

        public IEnumerable<TEntity> All
        {
            get
            { 
                return EntitySet; 
            }
        }

	    protected TEntity SetLang(TEntity entity)
	    {
			if (IsLocalizable())
			{
				((ILocalizableEntity)entity).Lang = Lang();
			}
		    return entity;
	    }

	    public TEntity Create()
        {
			return SetLang(Activator.CreateInstance<TEntity>());
        }

        public TEntity GetById(int id)
        {
            return EntitySet.SingleOrDefault(x => x.Id == id);
        }

        public void Add(TEntity entity)
        {
			SourceEntitySet.AddObject(SetLang(entity));
        }

        public void Delete(TEntity entity)
        {
            SourceEntitySet.DeleteObject(entity);
        }

    	public void DeleteById(int id)
    	{
    		Delete(GetById(id));
    	}
    }
}
