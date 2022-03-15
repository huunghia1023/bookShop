using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookShopSolution.ViewModels.Catalog.Products
{
    public class ProductCreateRequestValidator : AbstractValidator<ProductCreateRequest>
    {
        public ProductCreateRequestValidator()
        {
            RuleFor(x => x.Price).NotEmpty().WithMessage("Price can not be null");
            RuleFor(x => x.OriginalPrice).NotEmpty().WithMessage("Original Price can not be null");
            RuleFor(x => x.Stock).NotEmpty().WithMessage("Stock can not be null");
        }
    }
}