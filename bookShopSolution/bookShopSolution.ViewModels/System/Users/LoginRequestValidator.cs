using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookShopSolution.ViewModels.System.Users
{
    public class ProductCreateRequestValidator : AbstractValidator<LoginRequest>
    {
        public ProductCreateRequestValidator()
        {
            
        }
    }
}