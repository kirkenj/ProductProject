﻿using Application.DTOs.Product;
using Application.Features.Product.Requests.Commands;
using Application.Features.Product.Requests.Queries;
using Application.Models.Product;
using Constants;
using CustomResponse;
using Extensions.ClaimsPrincipalExtensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Models;


namespace ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator = null!;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductListDto>>> Get([FromQuery] ProductFilter productFilter, int? page, int? pageSize)
        {
            Response<IEnumerable<ProductListDto>> result = await _mediator.Send(new GetProducListtDtoRequest() { ProductFilter = productFilter, Page = page, PageSize = pageSize });
            return result.GetActionResult();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> Get(Guid id)
        {
            Response<ProductDto> result = await _mediator.Send(new GetProductDtoRequest() { Id = id });
            return result.GetActionResult();
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Guid>> Post([FromBody] CreateProductDto createProductDto)
        {
            if (!User.IsInRole(ApiConstants.ADMIN_ROLE_NAME))
            {
                createProductDto.ProducerId = User.GetUserId() ?? throw new ApplicationException("Couldn't get user's id");
            }

            Response<Guid> result = await _mediator.Send(new CreateProductCommand() { CreateProductDto = createProductDto });
            return result.GetActionResult();
        }

        [HttpPut("{id}")]
        [Authorize]
        [Produces("text/plain")]
        public async Task<ActionResult<string>> Put(Guid id, [FromBody] UpdateProductModel updateProductModel)
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

            if (User.IsInRole(ApiConstants.ADMIN_ROLE_NAME) == false)
            {
                if (productRequestResult.Result.ProducerId != User.GetUserId())
                {
                    return BadRequest("You are not the owner of this product");
                }

                updateProductModel.ProducerId = productRequestResult.Result.ProducerId;
            }

            UpdateProductCommand updateProductCommand = new()
            {
                UpdateProductDto = new()
                {
                    Id = id,
                    CreationDate = updateProductModel.CreationDate,
                    Description = updateProductModel.Description,
                    IsAvailable = updateProductModel.IsAvailable,
                    Name = updateProductModel.Name,
                    Price = updateProductModel.Price,
                    ProducerId = updateProductModel.ProducerId
                }
            };

            var result = await _mediator.Send(updateProductCommand);
            return result.GetActionResult();
        }

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
