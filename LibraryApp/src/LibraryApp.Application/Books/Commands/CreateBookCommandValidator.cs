using FluentValidation;
using LibraryApp.Application.Books.Commands;

namespace LibraryApp.src.LibraryApp.Application.Books.Commands
{
    public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
    {
        public CreateBookCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title must be at most 100 characters.");
        }
    }
}
