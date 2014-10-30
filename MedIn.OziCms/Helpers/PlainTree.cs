using System.Collections.Generic;
using System.Linq;
using MedIn.Db.Entities;

namespace MedIn.OziCms.Helpers
{
    public class PlainTree
    {
        /// <summary>
        /// функция создает "плоское" дерево - список объектов упорядоченных по вложенности,
        /// каждому элементу присвоено значение level начиная с 1
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="list">Список объектов из которых надо сделать "плоское" дерево</param>
        /// <param name="parentId">Идентификатор родительского объекта</param>
        /// <param name="currentLevel"></param>
        /// <returns>Возвращает одноуровневый список, упорядоченный по вложенности</returns>
        public static List<TEntity> GetTree<TEntity>(IEnumerable<TEntity> list, int? parentId = null, int currentLevel = 1)
        {
            var plainTree = new List<TEntity>();

	        var items = list as IList<TEntity> ?? list.ToList();
	        var children = items.Cast<IPlainTreeItem>().Where(item => (item.ParentId == null && parentId == null) || item.ParentId == parentId);

            foreach (var child in children)
            {
                child.Level = currentLevel;
                plainTree.Add((TEntity)child);

                var childChilds = GetTree(items, child.Id, currentLevel + 1);

            	if (childChilds.Count == 0) continue;
            	child.HasChilds = true;
            	plainTree.AddRange(childChilds);
            }

            return plainTree;
        }

        public static List<object> GetTree(List<object> list, int? parentId = null, int currentLevel = 1)
        {
            var plainTree = new List<object>();

	        var items = list.ToList();
	        var children = items.Cast<IPlainTreeItem>().Where(item => (item.ParentId == null && parentId == null) || item.ParentId == parentId);

            foreach (var child in children)
            {
                child.Level = currentLevel;
                plainTree.Add(child);

                var childChilds = GetTree(items, child.Id, currentLevel + 1);

            	if (childChilds.Count == 0) continue;
            	child.HasChilds = true;
            	plainTree.AddRange(childChilds);
            }

            return plainTree;
        }
    }
}
