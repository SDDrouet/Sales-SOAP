using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SLC
{
    public interface ICategoriesService
    {
        Categories CreateCategory(Categories category);
        bool UpdateCategory(Categories category);
        bool DeleteCategory(int id);
        Categories RetrieveCategory(int id);
        List<Categories> GetAllCategories();
    }
}
