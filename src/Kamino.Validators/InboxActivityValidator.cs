using FluentValidation;
using Kamino.Models;

namespace Kamino.Validators;

public class ObjectInboxModelValidator : AbstractValidator<ObjectInboxModel>
{
    public ObjectInboxModelValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(a => a.Type).NotNull().NotEmpty();
        RuleFor(a => a.Actor).NotNull().NotEmpty();
        RuleFor(a => a.Object).NotNull().NotEmpty();
    }
}
