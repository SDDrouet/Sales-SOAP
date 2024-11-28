using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SLC
{
    [ServiceContract]
    public interface ICategoriesService
    {
        [OperationContract]
        Categories CreateCategory(Categories categoryDTO);

        [OperationContract]
        bool UpdateCategory(Categories categoryDTO);

        [OperationContract]
        bool DeleteCategory(int id);

        [OperationContract]
        Categories RetrieveCategory(int id);

        [OperationContract]
        List<Categories> GetAllCategories();
    }
}
