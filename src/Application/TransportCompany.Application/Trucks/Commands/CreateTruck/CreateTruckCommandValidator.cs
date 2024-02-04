using FluentValidation;

namespace TransportCompany.Application.Trucks.Commands.CreateTruck
{
    public class CreateTruckCommandValidator : AbstractValidator<CreateTruckCommand>
    {
        public CreateTruckCommandValidator() {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Code).NotEmpty();
            RuleFor(x => x.Code).Matches("^[a-zA-Z0-9]+$");
        }
    }
}
