using DAL;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ProductLogic
    {
        public Products Create(Products products)
        {
            Products res = null;

            using (var r = RepositoryFactory.CreateRepository()) {
                Products result = r.Retrieve<Products>(p => p.productName == products.productName);

                if (result == null)
                {
                    res = r.Create(products);
                } else
                {
                    Console.WriteLine("Producto ya existente");
                }
            }

            return res;
        }

        public Products RetrieveById(int id) {
            Products res = null;

            using (var r = RepositoryFactory.CreateRepository()) {
                res = r.Retrieve<Products>(p => p.id == id);
            }

            return res;
        }

        public bool Update(Products productsToUpdate)
        {
            bool res = false;

            using (var r = RepositoryFactory.CreateRepository())
            {
                // Validar que el nombre del producto no exista
                Products temp = r.Retrieve<Products>
                    (p => p.productName == productsToUpdate.productName
                    && p.id != productsToUpdate.id);
                if (temp != null)
                {
                    res = r.Update(productsToUpdate);
                }
                else
                {
                    Console.WriteLine("Nombre ya existente");
                }

            }

            return res;
        }

        public bool Delete(int id)
        {
            bool res = false;

            var product = RetrieveById(id);

            if (product != null)
            {
                if(product.unitInStock == 0)
                {
                    using (var r = RepositoryFactory.CreateRepository())
                    {
                        res = r.Delete(product);
                    }
                } else
                {
                    Console.WriteLine("No se puede eliminar, hay productos en stock");
                }
            } else
            {
                Console.WriteLine("No se pudo eliminar: Producto no encontrado");
            }

            return res;
        }

        public List<Products> Filter(string filterName)
        {
            List<Products> res = null;

            using (var repository = RepositoryFactory.CreateRepository())
            {
                res = repository.Filter<Products>(
                    p => p.productName == filterName);
            }

            return res;
        }
    }
}
