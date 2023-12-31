﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskMediaExpert.Application.Interface;
using TaskMediaExpert.Infrastructure;
using TaskMediaExpert.Infrastructure.Models;

namespace TaskMediaExpert.Application.CQRS.Product.Query
{
    public class GetPageableProductQueryHandler : IRequestHandler<GetPageableProductQuery, PaginationResponse<IEnumerable<ProductModel>>>
    {
        private readonly IProductRepository _productRepository;

        public GetPageableProductQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<PaginationResponse<IEnumerable<ProductModel>>> Handle(GetPageableProductQuery request, CancellationToken cancellationToken)
        {
            var entities = await _productRepository.GetPageable(request.Page, request.ItemsPerPage);
            var total = await _productRepository.GetAllAsync();
            IEnumerable<ProductModel> result = entities.Select(x => new ProductModel() { Id = x.Id, Code = x.Code, Name = x.Name, Price = x.Price });
            return new PaginationResponse<IEnumerable<ProductModel>>(result,
                total.Where(x => x.IsActive).Count(), request.Page, request.ItemsPerPage);

        }
    }
}
