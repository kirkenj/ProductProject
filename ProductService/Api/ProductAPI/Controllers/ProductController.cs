using Application.DTOs.Product;
using Application.Features.Product.Requests.Commands;
using Application.Features.Product.Requests.Queries;
using Application.Models.Product;
using AutoMapper;
using Clients.AuthApi;
using Constants;
using CustomResponse;
using Extensions.ClaimsPrincipalExtensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator = null!;

        public ProductController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
        }

        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductListDto>>> Get([FromQuery] ProductFilter productFilter, int? page, int? pageSize)
        {
            Response<IEnumerable<ProductListDto>> result = await _mediator.Send(new GetProducListtDtoRequest() { ProductFilter = productFilter, Page = page, PageSize = pageSize });
            return result.GetActionResult();
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> Get(Guid id)
        {
            Response<ProductDto> result = await _mediator.Send(new GetProductDtoRequest() { Id = id });
            return result.GetActionResult();
        }

        // POST api/<ValuesController>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Guid>> Post([FromBody] CreateProductDto createProductDto)
        {
            if (User.IsInRole(ApiConstants.ADMIN_ROLE_NAME))
            {
                createProductDto.ProducerId = User.GetUserId() ?? throw new ApplicationException("Couldn't get user's id");
            }

            Response<Guid> result = await _mediator.Send(new CreateProductCommand() { CreateProductDto = createProductDto });
            return result.GetActionResult();
        }

        // PUT api/<ValuesController>/5
        [HttpPut]
        [Authorize]
        [Produces("text/plain")]
        public async Task<ActionResult<string>> Put([FromBody] UpdateProductDto updateProductDto)
        {
            Response<ProductDto> productRequestResult = await _mediator.Send(new GetProductDtoRequest() { Id = updateProductDto.Id });

            if (productRequestResult.Success == false)
            {
                return productRequestResult.GetActionResult();
            }

            if (productRequestResult.Result == null)
            {
                throw new ApplicationException();
            }

            UpdateProductCommand updateProductCommand = new() { UpdateProductDto = updateProductDto };

            if (User.IsInRole(ApiConstants.ADMIN_ROLE_NAME) == false)
            {
                if (productRequestResult.Result.ProducerId != User.GetUserId())
                {
                    return BadRequest("You are not the owner of this product");
                }

                updateProductDto.ProducerId = productRequestResult.Result.ProducerId;
            }

            var result = await _mediator.Send(updateProductCommand);
            return result.GetActionResult();
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        [Authorize]
        [Produces("text/plain")]
        public async Task<ActionResult<string>> Delete(Guid id)
        {
            Response<ProductDto> productRequestResult = await _mediator.Send(new GetProductDtoRequest() { Id = id });

            if (productRequestResult.Success == false)
            {
                return productRequestResult.GetActionResult();
            }

            if (productRequestResult.Result == null)
            {
                throw new ApplicationException();
            }

            if (User.IsInRole(ApiConstants.ADMIN_ROLE_NAME) || productRequestResult.Result.ProducerId == User.GetUserId())
            {
                return (await _mediator.Send(new RemovePrductComand() { ProductId = id })).GetActionResult();
            }

            return BadRequest("You are not the owner of the product");
        }
    }
}
