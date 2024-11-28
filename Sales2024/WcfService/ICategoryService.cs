using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WcfService.DTO;

namespace WcfService
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "ICategoryService" en el código y en el archivo de configuración a la vez.
    [ServiceContract]
    public interface ICategoriesService
    {
        [OperationContract]
        CategoryDTO CreateCategory(CategoryDTO categoryDTO);

        [OperationContract]
        bool UpdateCategory(CategoryDTO categoryDTO);

        [OperationContract]
        bool DeleteCategory(int id);

        [OperationContract]
        CategoryDTO RetrieveCategory(int id);

        [OperationContract]
        List<CategoryDTO> GetAllCategories();
    }
}
