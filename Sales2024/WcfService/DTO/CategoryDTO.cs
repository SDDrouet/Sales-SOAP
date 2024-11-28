using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WcfService.DTO
{
    [DataContract]
    [Serializable]
    public class CategoryDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string CategoryName { get; set; }

        [DataMember]
        public string Description { get; set; }

        public static Categories ToEntity(CategoryDTO categoryDTO)
        {
            if (categoryDTO == null)
                return null;

            return new Categories
            {
                id = categoryDTO.Id,
                categoryName = categoryDTO.CategoryName,
                description = categoryDTO.Description
            };
        }

        /// <summary>
        /// Convierte una entidad de categoría a un DTO de categoría.
        /// </summary>
        public static CategoryDTO ToDTO(Categories category)
        {
            if (category == null)
                return null;

            return new CategoryDTO
            {
                Id = category.id,
                CategoryName = category.categoryName,
                Description = category.description
            };
        }
    }


}