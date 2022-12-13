using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personal_StoreFront.DATA.EF.Models//.Metadata
{
    [ModelMetadataType(typeof(CardConditionMetadata))]
    public partial class CardCondition
    {

    }

    [ModelMetadataType(typeof(CategoryMetadata))]
    public partial class Category
    {

    }

    [ModelMetadataType(typeof(OrderDetailMetadata))]
    public partial class OrderDetail
    {

    }

    //[ModelMetadataType(typeof(OrderProductMetadata))]
    //public partial class OrderProduct
    //{

    //}

    [ModelMetadataType(typeof(ProductMetadata))]
    public partial class Product
    {
        [NotMapped]
        public IFormFile? Image { get; set; }
    }

    [ModelMetadataType(typeof(ProductTypeMetadata))]
    public partial class ProductType
    {

    }

    [ModelMetadataType(typeof(UsersDetailMetadata))]
    public partial class UsersDetail
    {

    }
}
