//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v14.1.0.0 (NJsonSchema v11.0.2.0 (Newtonsoft.Json v13.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------

#nullable enable

#pragma warning disable 108 // Disable "CS0108 '{derivedDto}.ToJson()' hides inherited member '{dtoBase}.ToJson()'. Use the new keyword if hiding was intended."
#pragma warning disable 114 // Disable "CS0114 '{derivedDto}.RaisePropertyChanged(String)' hides inherited member 'dtoBase.RaisePropertyChanged(String)'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword."
#pragma warning disable 472 // Disable "CS0472 The result of the expression is always 'false' since a value of type 'Int32' is never equal to 'null' of type 'Int32?'
#pragma warning disable 612 // Disable "CS0612 '...' is obsolete"
#pragma warning disable 649 // Disable "CS0649 Field is never assigned to, and will always have its default value null"
#pragma warning disable 1573 // Disable "CS1573 Parameter '...' has no matching param tag in the XML comment for ...
#pragma warning disable 1591 // Disable "CS1591 Missing XML comment for publicly visible type or member ..."
#pragma warning disable 8073 // Disable "CS8073 The result of the expression is always 'false' since a value of type 'T' is never equal to 'null' of type 'T?'"
#pragma warning disable 3016 // Disable "CS3016 Arrays as attribute arguments is not CLS-compliant"
#pragma warning disable 8603 // Disable "CS8603 Possible null reference return"
#pragma warning disable 8604 // Disable "CS8604 Possible null reference argument for parameter"
#pragma warning disable 8625 // Disable "CS8625 Cannot convert null literal to non-nullable reference type"
#pragma warning disable 8765 // Disable "CS8765 Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes)."

namespace CustomGateway.Controllers.Product
{
    using Clients.ProductApi;
    using Microsoft.AspNetCore.Mvc;
    using System = global::System;

    [System.CodeDom.Compiler.GeneratedCode("NSwag", "14.1.0.0 (NJsonSchema v11.0.2.0 (Newtonsoft.Json v13.0.0.0))")]
    [ApiController]

    public partial class ProductServiceController : Microsoft.AspNetCore.Mvc.ControllerBase
    {
        private IProductApiClient _implementation;

        public ProductServiceController(IProductApiClient implementation)
        {
            _implementation = implementation;
        }

        /// <returns>Success</returns>
        [Microsoft.AspNetCore.Mvc.HttpGet, Microsoft.AspNetCore.Mvc.Route("api/Product", Name = "ProductAll")]
        public async System.Threading.Tasks.Task<System.Collections.Generic.ICollection<ProductListDto>> ProductAll()
        {
            var res = await _implementation.ProductAllAsync(null, null, null, null, null, null, null, null, null, null, null);
            return res;
        }

        /// <returns>Success</returns>
        [Microsoft.AspNetCore.Mvc.HttpPost, Microsoft.AspNetCore.Mvc.Route("api/Product", Name = "ProductPOST")]
        public async System.Threading.Tasks.Task<System.Guid> ProductPOST([Microsoft.AspNetCore.Mvc.FromBody] CreateProductDto? body)
        {

            return await _implementation.ProductPOSTAsync(body);
        }

        /// <returns>Success</returns>
        [Microsoft.AspNetCore.Mvc.HttpPut, Microsoft.AspNetCore.Mvc.Route("api/Product", Name = "ProductPUT")]
        public async System.Threading.Tasks.Task<string> ProductPUT([Microsoft.AspNetCore.Mvc.FromBody] UpdateProductDto? body)
        {

            return await _implementation.ProductPUTAsync(body);
        }

        /// <returns>Success</returns>
        [Microsoft.AspNetCore.Mvc.HttpGet, Microsoft.AspNetCore.Mvc.Route("api/Product/{id}", Name = "ProductGET")]
        public async System.Threading.Tasks.Task<ProductDto> ProductGET(System.Guid id)
        {

            return await _implementation.ProductGETAsync(id);
        }

        /// <returns>Success</returns>
        [Microsoft.AspNetCore.Mvc.HttpDelete, Microsoft.AspNetCore.Mvc.Route("api/Product/{id}", Name = "ProductDELETE")]
        public async System.Threading.Tasks.Task<string> ProductDELETE(System.Guid id)
        {

            return await _implementation.ProductDELETEAsync(id);
        }

    }

}

#pragma warning restore  108
#pragma warning restore  114
#pragma warning restore  472
#pragma warning restore  612
#pragma warning restore 1573
#pragma warning restore 1591
#pragma warning restore 8073
#pragma warning restore 3016
#pragma warning restore 8603
#pragma warning restore 8604
#pragma warning restore 8625