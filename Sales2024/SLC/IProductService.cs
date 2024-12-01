﻿using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace SLC
{
    [ServiceContract]
    public interface IProductService
    {
        [OperationContract]
        Products CreateProducts(Products products);
        [OperationContract]
        bool Delete(int  id);
        [OperationContract]
        List<Products> GetByName(String filterName);
        [OperationContract]
        Products RetrieveById(int id);
        [OperationContract]
        bool UpdateProduct(Products productsToUpdate);
    }
}
