using FluentValidation;

namespace LibraryApp.src.LibraryApp.Application.Books.Commands
{
    public class BorrowBookCommandValidator : AbstractValidator<BorrowBookCommand>
    {
        public BorrowBookCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Book ID is required.");
        }
    }
}
