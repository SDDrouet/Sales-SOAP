using BLL;
using Entities;
using SLC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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

        public bool Delete(int id)
        {
            var productLogic = new ProductLogic();
            var isDeleted = productLogic.Delete(id);
            return isDeleted;
        }
    }
}
