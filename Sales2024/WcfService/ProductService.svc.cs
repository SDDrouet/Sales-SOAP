using BLL;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WcfService.DTO;

namespace WcfService
{
    public class ProductService : IProductService
    {
        public ProductDTO CreateProducts(ProductDTO productDTO)
        {
            var productLogic = new ProductLogic();
            var productEntity = ProductMapper.ToEntity(productDTO); // Mapea el DTO a la entidad
            var createdProduct = productLogic.Create(productEntity);
            return createdProduct != null ? ProductMapper.ToDTO(createdProduct) : null; // Verifica si la entidad es null antes de mapear
        }

        public bool Delete(int id)
        {
            var productLogic = new ProductLogic();
            var result = productLogic.Delete(id);
            return result;
        }

        public List<ProductDTO> GetByName(string filterName)
        {
            var productLogic = new ProductLogic();
            var products = productLogic.Filter(filterName);
            return products?.Select(ProductMapper.ToDTO).ToList() ?? new List<ProductDTO>(); // Verifica si products es null y devuelve una lista vacía si es necesario
        }

        public ProductDTO RetrieveById(int id)
        {
            var productLogic = new ProductLogic();
            var product = productLogic.RetrieveById(id);
            return product != null ? ProductMapper.ToDTO(product) : null; // Verifica si la entidad es null antes de mapear
        }

        public bool UpdateProduct(ProductDTO productDTO)
        {
            var productLogic = new ProductLogic();
            var productEntity = ProductMapper.ToEntity(productDTO); // Mapea el DTO a la entidad
            var result = productLogic.Update(productEntity);
            return result;
        }
    }
}
