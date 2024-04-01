using FluentValidation;
using HomeApi.Contracts.Models.Rooms;

namespace HomeApi.Contracts.Validation
{
    /// <summary>
    /// Класс-валидатор запросов обновления комнаты
    /// </summary>
    public class EditRoomRequestValidator : AbstractValidator<EditRoomRequest>
    {
        /// <summary>
        /// Метод, конструктор, устанавливающий правила
        /// </summary>
        public EditRoomRequestValidator()
        {
            RuleFor(x => x.NewArea).NotEmpty();
            RuleFor(x => x.NewName).NotEmpty();
            RuleFor(x => x.NewVoltage).NotEmpty();
            RuleFor(x => x.NewGasConnected).NotEmpty();
        }
    }
}
