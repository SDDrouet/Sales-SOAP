using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using WcfService.DTO;

namespace WcfService
{
    public class CategoryService : ICategoriesService
    {
        private readonly CategoriesLogic _categoriesLogic;

        public CategoryService()
        {
            // Instancia del repositorio. Aquí se asume que EFRepository es instanciado de alguna forma.
            _categoriesLogic = new CategoriesLogic();
        }

        public CategoryDTO CreateCategory(CategoryDTO categoryDTO)
        {
            var categoryEntity = CategoryDTO.ToEntity(categoryDTO); // Mapea el DTO a la entidad
            var createdCategory = _categoriesLogic.Create(categoryEntity);
            return createdCategory != null ? CategoryDTO.ToDTO(createdCategory) : null; // Verifica si la entidad es null antes de mapear
        }

        public bool UpdateCategory(CategoryDTO categoryDTO)
        {
            var categoryEntity = CategoryDTO.ToEntity(categoryDTO);
            return _categoriesLogic.Update(categoryEntity);
        }

        public bool DeleteCategory(int id)
        {
            var categoryEntity = _categoriesLogic.RetrieveById(id);
            if (categoryEntity != null)
            {
                return _categoriesLogic.Delete(id);
            }
            return false;
        }

        public CategoryDTO RetrieveCategory(int id)
        {
            var categoryEntity = _categoriesLogic.RetrieveById(id);
            return categoryEntity != null ? CategoryDTO.ToDTO(categoryEntity) : null;
        }

        public List<CategoryDTO> GetAllCategories()
        {
            var categories = _categoriesLogic.GetAllCategories();
            return categories?.Select(CategoryDTO.ToDTO).ToList() ?? new List<CategoryDTO>();
        }

        public List<CategoryDTO> FilterCategories(string filterName)
        {
            var categories = _categoriesLogic.Filter(filterName);
            return categories?.Select(CategoryDTO.ToDTO).ToList() ?? new List<CategoryDTO>();
        }
    }
}
