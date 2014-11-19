using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using MedIn.Db.Entities;

namespace MedIn.Domain.Entities
{
    public partial class Category : ISortableEntity, IVisibleEntity, IHaveAliasEntity, IMetadataEntity,INestedEntity<Category>
    {
        public int Level { get; set; }
        public bool HasChilds { get; set; }
        public EntityCollection<Category> Children
        {
            get { return Categories1; }
            set { Categories1 = value; }
        }

        public Category Parent
        {
            get { return Category1; }
            set { Category1 = value; }
        }

        public override string ToString()
        {
            return Name;
        }

        public Category Root
        {
            get
            {
                return Parent == null ? this : Parent.Root;
            }
        }

        public IEnumerable<Product> AllProducts
        {
            get { return GetAllProducts(); }
        }

        public string Url
        {
            get
            {
                if (Parent != null)
                    return string.Format("{0}/{1}", Parent.Url, Alias);
                return string.Format("{0}", Alias);
            }
        }

        private IEnumerable<Product> GetAllProducts()
        {
            var children = new List<Product>();
            if (Children.Any())
            {
                children = Children.ToList().SelectMany(category => category.AllProducts).ToList();
            }
            return Products.Union(children);
        }
    }
}
