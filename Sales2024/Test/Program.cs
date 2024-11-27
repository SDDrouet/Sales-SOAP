using Entities;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            addCategoryAndProduct();
        }

        static void addCategoryAndProduct()
        {
            var categories = new Categories();
            categories.categoryName = "Test";
            categories.description = "description Test";

            var product = new Products();
            product.productName = "Papitas";
            product.unitPrice = 10;
            product.unitInStock = 101;

            categories.Products.Add(product);

            using (var repository = RepositoryFactory.CreateRepository())
            {
                repository.Create(categories);
            }

            Console.WriteLine($"Categoria: {categories.id}, Producto: {product.id}");
            Console.ReadLine();
        }
    }
}
