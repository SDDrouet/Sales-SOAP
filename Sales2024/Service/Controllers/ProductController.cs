using BLL;
using Entities;
using SLC;
using System.Collections.Generic;
using System.Web.Http;

namespace Service.Controllers
{
    public class ProductController : ApiController, IProductService
    {
        [HttpPost]
        public Products CreateProducts(Products products)
        {
            var productLogic = new ProductLogic();
            var product = productLogic.Create(products);
            return product;
        }

        [HttpDelete]
        public bool Delete(int id)
        {
            var productLogic = new ProductLogic();
            var isDeleted = productLogic.Delete(id);
            return isDeleted;
        }

        [HttpGet]
        public List<Products> GetByName(string filterName)
        {
            var productLogic = new ProductLogic();
            var products = productLogic.Filter(filterName);
            return products;
        }

        [HttpGet]
        public Products RetrieveById(int id)
        {
            var productLogic = new ProductLogic();
            var product = productLogic.RetrieveById(id);
            return product;
        }

        [HttpPut]
        public bool UpdateProduct(Products productsToUpdate)
        {
            var productLogic = new ProductLogic();
            var isUpdated = productLogic.Update(productsToUpdate);
            return isUpdated;
        }
    }
}
