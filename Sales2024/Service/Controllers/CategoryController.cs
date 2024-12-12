using BLL;
using Entities;
using Security;
using SLC;
using System.Collections.Generic;
using System.Web.Http;

namespace Service.Controllers
{
    public class CategoryController : ApiController, ICategoriesService
    {
        [HttpPost]
        [AuthorizeRoles("ADMIN", "EDITOR")]
        public Categories CreateCategory(Categories category)
        {
            var categoryLogic = new CategoriesLogic();
            var newCategory = categoryLogic.Create(category);
            return newCategory;
        }

        [HttpDelete]
        [AuthorizeRoles("ADMIN", "EDITOR")]
        public bool DeleteCategory(int id)
        {
            var categoryLogic = new CategoriesLogic();
            var isDeleted = categoryLogic.Delete(id);
            return isDeleted;
        }

        [HttpGet]
        [AuthorizeRoles("ADMIN", "EDITOR", "VIEWER")]
        public List<Categories> GetAllCategories()
        {
            var categoryLogic = new CategoriesLogic();
            var cateyories = categoryLogic.GetAllCategories();
            return cateyories;
        }

        [HttpGet]
        [AuthorizeRoles("ADMIN", "EDITOR", "VIEWER")]
        public Categories RetrieveCategory(int id)
        {
            var categoryLogic = new CategoriesLogic();
            var category = categoryLogic.RetrieveById(id);
            return category;
        }

        [HttpPut]
        [AuthorizeRoles("ADMIN", "EDITOR")]
        public bool UpdateCategory(Categories category)
        {
            var categoryLogic = new CategoriesLogic();
            var isUpdated = categoryLogic.Update(category);
            return isUpdated;
        }
    }
}
