using BLL;
using Entities;
using Security;
using SLC;
using System.Collections.Generic;
using System.Web.Http;


namespace Service.Controllers
{
    public class ProductController : ApiController, IProductService
    {
        [HttpPost]
        [AuthorizeRoles("ADMIN", "EDITOR")]
        public Products CreateProducts(Products products)
        {
            string currentUsername = SessionContext.Username;
            string currentRol = SessionContext.Rol;

            var productLogic = new ProductLogic();
            var product = productLogic.Create(products);
            return product;
        }

        [HttpDelete]
        [AuthorizeRoles("ADMIN", "EDITOR")]
        public bool Delete(int id)
        {
            var productLogic = new ProductLogic();
            var isDeleted = productLogic.Delete(id);
            return isDeleted;
        }

        [HttpGet]
        [AuthorizeRoles("ADMIN", "EDITOR", "VIEWER")]
        public List<Products> GetByName(string filterName)
        {
            var productLogic = new ProductLogic();
            var products = productLogic.Filter(filterName);
            return products;
        }

        [HttpGet]
        [AuthorizeRoles("ADMIN", "EDITOR", "VIEWER")]
        public Products RetrieveById(int id)
        {
            var productLogic = new ProductLogic();
            var product = productLogic.RetrieveById(id);
            return product;
        }

        [HttpPut]
        [AuthorizeRoles("ADMIN", "EDITOR")]
        public bool UpdateProduct(Products productsToUpdate)
        {
            var productLogic = new ProductLogic();
            var isUpdated = productLogic.Update(productsToUpdate);
            return isUpdated;
        }
    }
}
