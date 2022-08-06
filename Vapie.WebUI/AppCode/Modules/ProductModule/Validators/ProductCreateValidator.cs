using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vapie.WebUI.AppCode.Modules.ProductModule.Validators
{
    public class ProductCreateValidator : AbstractValidator<ProductCreateCommand>
    {
        public ProductCreateValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty()
                .NotNull()
                .WithMessage("Bosh buraxila bilmez");


            RuleFor(p => p.CategoryId).GreaterThan(0).WithMessage("Duzgun melumat secilmeyib");


            //RuleForEach(p => p.Specification)
            //    .ChildRules(cp =>
            //    {
            //        cp.RuleFor(cpi => cpi.Value)
            //        .NotNull()
            //        .NotEmpty()
            //        .WithMessage("Bosh buraxila bilmez");

            //    });
        }
    }
}
