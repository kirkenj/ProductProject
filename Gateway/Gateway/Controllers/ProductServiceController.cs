using Clients.ProductApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomGateway.Controllers;

public class ProductServiceController : Microsoft.AspNetCore.Mvc.ControllerBase
{
    private readonly IProductApiClient _implementation = null!;

    public ProductServiceController(IProductApiClient implementation)
    {
        _implementation = implementation;
    }


    [Microsoft.AspNetCore.Mvc.HttpGet, Microsoft.AspNetCore.Mvc.Route("api/Product", Name = "PoductList")]
    public async Task<ICollection<ProductListDto>> List([FromQuery] IEnumerable<Guid>? ids, [FromQuery] string? namePart, [FromQuery] string? descriptionPart, [FromQuery] double? priceStart, [FromQuery] double? priceEnd, [FromQuery] bool? isAvailable, [FromQuery] IEnumerable<Guid>? producerIds, [FromQuery] DateTimeOffset? creationDateStart, [FromQuery] DateTimeOffset? creationDateEnd, [FromQuery] int? page, [FromQuery] int? pageSize)
    {
        return await _implementation.ProductAllAsync(ids, namePart, descriptionPart, priceStart, priceEnd, isAvailable, producerIds, creationDateStart, creationDateEnd, page, pageSize);
    }

    [Authorize]
    [Microsoft.AspNetCore.Mvc.HttpPost, Microsoft.AspNetCore.Mvc.Route("api/Product", Name = "ProductPOST")]
    public async System.Threading.Tasks.Task<System.Guid> ProductPOST([Microsoft.AspNetCore.Mvc.FromBody] CreateProductDto? body)
    {
        return await _implementation.ProductPOSTAsync(body);
    }

    [Authorize]
    [Microsoft.AspNetCore.Mvc.HttpPut, Microsoft.AspNetCore.Mvc.Route("api/Product/{id}", Name = "ProductPUT")]
    [Produces("text/plain")]
    public async System.Threading.Tasks.Task<string> ProductPUT(Guid id, [Microsoft.AspNetCore.Mvc.FromBody] UpdateProductModel? body)
    {
        return await _implementation.ProductPUTAsync(id, body);
    }

    [Microsoft.AspNetCore.Mvc.HttpGet, Microsoft.AspNetCore.Mvc.Route("api/Product/{id}", Name = "ProductGET")]
    public async System.Threading.Tasks.Task<ProductDto> ProductGET(System.Guid id)
    {
        return await _implementation.ProductGETAsync(id);
    }

    [Authorize]
    [Microsoft.AspNetCore.Mvc.HttpDelete, Microsoft.AspNetCore.Mvc.Route("api/Product/{id}", Name = "ProductDELETE")]
    [Produces("text/plain")]
    public async System.Threading.Tasks.Task<string> ProductDELETE(System.Guid id)
    {
        return await _implementation.ProductDELETEAsync(id);
    }
}