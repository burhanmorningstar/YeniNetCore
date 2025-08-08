using App.Repositories.Products;
using FluentValidation;

namespace App.Services.Products.Create
{
    public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
    {
        private readonly IProductRepository _productRepository;
        public CreateProductRequestValidator(IProductRepository productRepository)
        {
            _productRepository = productRepository;
            RuleFor(x => x.Name)
                .NotNull().WithMessage("Name is required.")
                .Length(3, 10).WithMessage("Product name must be between 3 and 10");
                //.MustAsync(MustUniqueProductNameAsync).WithMessage("Product name must be unique.");
            //.Must(MustUniqueProductName).WithMessage("Product name must be unique.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.");

            RuleFor(x => x.Stock)
                .InclusiveBetween(1,100).WithMessage("Stock quantity must be between 1 and 100");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Product category id must be greater than 0.");
        }

        //private async Task<bool> MustUniqueProductNameAsync(string name, CancellationToken cancellationToken)
        //{
        //    return !await _productRepository.Where(x => x.Name == name).AnyAsync(cancellationToken);
        //}

        //1. yol sync validation
        //private bool MustUniqueProductName(string name)
        //{
        //    return !_productRepository.Where(x => x.Name == name).Any();
        //}
    }
}
