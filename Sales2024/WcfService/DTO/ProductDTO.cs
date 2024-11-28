using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WcfService.DTO
{
    [DataContract]
    [Serializable] // Incluido para compatibilidad general
    public class ProductDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string ProductName { get; set; }

        [DataMember]
        public int CategoryId { get; set; }

        [DataMember]
        public decimal UnitPrice { get; set; }

        [DataMember]
        public int UnitInStock { get; set; }
    }

    public class ProductMapper
    {
        public static ProductDTO ToDTO(Products product)
        {
            return new ProductDTO
            {
                Id = product.id,
                ProductName = product.productName,
                CategoryId = product.categoryId,
                UnitPrice = product.unitPrice,
                UnitInStock = product.unitInStock
            };
        }

        public static Products ToEntity(ProductDTO productDTO)
        {
            return new Products
            {
                id = productDTO.Id,
                productName = productDTO.ProductName,
                categoryId = productDTO.CategoryId,
                unitPrice = productDTO.UnitPrice,
                unitInStock = productDTO.UnitInStock
            };
        }
    }

}