using FluentValidation;

namespace TransportCompany.Application.Trucks.Commands.EditTruck
{
    internal class EditTruckCommandValidator : AbstractValidator<EditTruckCommand>
    {
        public EditTruckCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Code).NotEmpty();
            RuleFor(x => x.Code).Matches("^[a-zA-Z0-9]+$");
        }
    }
}
