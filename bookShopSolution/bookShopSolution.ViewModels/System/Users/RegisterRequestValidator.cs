using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookShopSolution.ViewModels.System.Users
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.FirstName).MaximumLength(100).WithMessage("FirstName very long");
            RuleFor(x => x.LastName).MaximumLength(100).WithMessage("LastName very long");
            RuleFor(x => x.Email).Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").WithMessage("Email format not match");
            RuleFor(x => x.BirthDay).LessThan(DateTime.Now.AddYears(-1)).WithMessage("BirthDay cannot less than 1 years old");
            RuleFor(x => x.Password).MinimumLength(6).WithMessage("Password has at least 6 characters");
            RuleFor(x => x).Custom((request, context) =>
            {
                if (request.Password != request.ConfirmPassword)
                {
                    context.AddFailure("Confirm password is not match");
                }
            });
        }
    }
}