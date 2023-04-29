using AD.Demo.BookPublication.Domain.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Demo.BookPublication.Domain.Validators
{
    public class ImportBookValidator : AbstractValidator<BookDTO>
    {
        public ImportBookValidator()
        {
            RuleFor(r => r.Title).NotEmpty().WithMessage("Title is mandatory");
            RuleFor(r => r.TotalPages).NotEmpty().When(w=> w.TotalPages == 0).WithMessage("Total Pages is mandatory");
            RuleFor(r => r.ISBN).NotEmpty().WithMessage("ISBN is mandatory");
        }
    }
}
