using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WcfService.DTO;

namespace WcfService
{
    [ServiceContract]
    public interface IProductService
    {
        [OperationContract]
        ProductDTO CreateProducts(ProductDTO products);
        [OperationContract]
        bool Delete(int  id);
        [OperationContract]
        List<ProductDTO> GetByName(String filterName);
        [OperationContract]
        ProductDTO RetrieveById(int id);
        [OperationContract]
        bool UpdateProduct(ProductDTO productsToUpdate);
    }
}
