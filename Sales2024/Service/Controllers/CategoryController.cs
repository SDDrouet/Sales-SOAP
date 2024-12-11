using BLL;
using Entities;
using SLC;
using System.Collections.Generic;
using System.Web.Http;

namespace Service.Controllers
{
    public class CategoryController : ApiController, ICategoriesService
    {
        [HttpPost]
        public Categories CreateCategory(Categories category)
        {
            var categoryLogic = new CategoriesLogic();
            var newCategory = categoryLogic.Create(category);
            return newCategory;
        }

        [HttpDelete]
        public bool DeleteCategory(int id)
        {
            var categoryLogic = new CategoriesLogic();
            var isDeleted = categoryLogic.Delete(id);
            return isDeleted;
        }

        [HttpGet]
        public List<Categories> GetAllCategories()
        {
            var categoryLogic = new CategoriesLogic();
            var cateyories = categoryLogic.GetAllCategories();
            return cateyories;
        }

        [HttpGet]
        public Categories RetrieveCategory(int id)
        {
            var categoryLogic = new CategoriesLogic();
            var category = categoryLogic.RetrieveById(id);
            return category;
        }

        [HttpPut]
        public bool UpdateCategory(Categories category)
        {
            var categoryLogic = new CategoriesLogic();
            var isUpdated = categoryLogic.Update(category);
            return isUpdated;
        }
    }
}
