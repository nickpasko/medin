using System.Data.Linq.Mapping;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using MedIn.Db.Entities;
using MedIn.Libs.Services;

namespace MedIn.OziCms.Repositories
{
	public class GenericRepository<TEntity, TDbContext> : IRepository<TEntity, TDbContext>
        where TEntity : class, IEntity
		where TDbContext : ObjectContext, new()
    {

		private TDbContext _dataContext;
		public TDbContext DataContext
        {
            get
            {
	            return _dataContext ?? (_dataContext = new TDbContext());
            }
        }

        public TEntity GetByPrimaryKey(int id)
        {
	        return DataContext.CreateObjectSet<TEntity>().FirstOrDefault(entity => entity.Id == id);
        }

        public PropertyInfo GetPrimaryKeyInfo()
        {
            var properties = typeof(TEntity).GetProperties();
            foreach (var pI in properties)
            {
                var attributes = pI.GetCustomAttributes(true);
                foreach (var attribute in attributes)
                {
                    if (attribute is EdmScalarPropertyAttribute)
                    {
                        if ((attribute as EdmScalarPropertyAttribute).EntityKeyProperty)
                            return pI;
                    }
                    else if (attribute is ColumnAttribute)
                    {

                        if ((attribute as ColumnAttribute).IsPrimaryKey)
                            return pI;
                    }
                }
            }
            return null;
        }

		public IQueryable<TEntity> All(string lang)
		{
			var result = DataContext.CreateObjectSet<TEntity>();
			if (IsLocalizable())
			{
				return result.Where("it.Lang=@lang", new ObjectParameter("lang", lang)).AsQueryable();
			}
			return result;
		}

		public IQueryable<TEntity> All()
        {
	        if (IsLocalizable())
	        {
				var lang = DependencyResolver.Current.GetService<ILocalizationProvider>();
		        return All(lang.GetLanguageName());
	        }
			return DataContext.CreateObjectSet<TEntity>();
        }

		protected bool IsLocalizable()
		{
			return typeof (ILocalizableEntity).IsAssignableFrom(typeof (TEntity));
		}

		protected TEntity SetLang(TEntity entity)
		{
			if (IsLocalizable())
			{
				var lang = DependencyResolver.Current.GetService<ILocalizationProvider>();
				((ILocalizableEntity)entity).Lang = lang.GetLanguageName();
			}
			return entity;
		}

		public void AddObject(TEntity entity)
        {
			DataContext.CreateObjectSet<TEntity>().AddObject(SetLang(entity));
        }
        
        public void Save()
        {
			DataContext.SaveChanges();
        }


        public void Delete(TEntity entity)
        {
            DataContext.CreateObjectSet<TEntity>().DeleteObject(entity);
            Save();
        }
    }
}