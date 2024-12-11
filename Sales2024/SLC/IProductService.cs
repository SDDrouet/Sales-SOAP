using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace SLC
{
    public interface IProductService
    {
        Products CreateProducts(Products products);
        bool Delete(int  id);
        List<Products> GetByName(String filterName);
        Products RetrieveById(int id);
        bool UpdateProduct(Products productsToUpdate);
    }
}
