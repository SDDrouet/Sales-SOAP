using DAL;
using Entities;
using Security;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BLL
{
    public class CategoriesLogic
    {
        private LogLogic logLogic = new LogLogic();

        public Categories Create(Categories category)
        {
            Categories res = null;

            using (var r = RepositoryFactory.CreateRepository())
            {
                Categories result = r.Retrieve<Categories>(c => c.categoryName == category.categoryName);

                if (result == null)
                {
                    res = r.Create(category);
                }
                else
                {
                    Console.WriteLine("Categoría ya existente");
                }
            }

            if (res != null)
            {
                logLogic.Create(SessionContext.Username, "Se creó la nueva categoría: " + res.categoryName + ", con id: " + res.id);
            }

            return res;
        }

        public Categories RetrieveById(int id)
        {
            Categories res = null;

            using (var r = RepositoryFactory.CreateRepository())
            {
                res = r.Retrieve<Categories>(c => c.id == id);
            }

            return res;
        }

        public bool Update(Categories categoryToUpdate)
        {
            bool res = false;

            using (var r = RepositoryFactory.CreateRepository())
            {
                // Validar que el nombre de la categoría no exista ya
                Categories temp = r.Retrieve<Categories>(c => c.categoryName == categoryToUpdate.categoryName && c.id != categoryToUpdate.id);

                if (temp == null)
                {
                    res = r.Update(categoryToUpdate);
                }
                else
                {
                    Console.WriteLine("Nombre de categoría ya existente");
                }
            }

            if (res)
            {
                logLogic.Create(SessionContext.Username, "Se actualizó la categoría: " + categoryToUpdate.categoryName + ", con id: " + categoryToUpdate.id);
            }

            return res;
        }

        public bool Delete(int id)
        {
            bool res = false;

            var category = RetrieveById(id);

            if (category != null)
            {
                using (var r = RepositoryFactory.CreateRepository())
                {
                    res = r.Delete(category);
                }
            }
            else
            {
                Console.WriteLine("No se pudo eliminar: Categoría no encontrada");
            }

            if (res)
            {
                logLogic.Create(SessionContext.Username, "Se eliminó la categoría: " + category.categoryName + ", con id: " + category.id);
            }

            return res;
        }

        public List<Categories> Filter(string filterName)
        {
            List<Categories> res = null;

            using (var repository = RepositoryFactory.CreateRepository())
            {
                if (string.IsNullOrEmpty(filterName))
                {
                    res = repository.Filter<Categories>(c => true);
                }
                else
                {
                    res = repository.Filter<Categories>(c => c.categoryName.Contains(filterName));
                }
            }

            return res;
        }

        public List<Categories> GetAllCategories()
        {
            List<Categories> res = null;

            using (var r = RepositoryFactory.CreateRepository())
            {
                res = r.Filter<Categories>(c => true);
            }

            return res;
        }
    }
}
