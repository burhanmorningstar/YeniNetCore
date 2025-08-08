﻿using FluentValidation;

namespace App.Services.Products.Update
{
    public class UpdateProductRequestValidator: AbstractValidator<UpdateProductRequest>
    {
        public UpdateProductRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotNull().WithMessage("Name is required.")
                .Length(3, 10).WithMessage("Product name must be between 3 and 10");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.");

            RuleFor(x => x.Stock)
                .InclusiveBetween(1, 100).WithMessage("Stock quantity must be between 1 and 100");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Product category id must be greater than 0.");
        }
    }
}
