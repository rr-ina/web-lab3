using FluentValidation;
using lab3.DTOs;

namespace lab3.Validators
{
    public class PlaylistValidator : AbstractValidator<CreatePlaylistDto>
    {
        public PlaylistValidator()
        {
            RuleFor(playlist => playlist.Name)
                .NotEmpty().WithMessage("Назва плейлиста є обов'язковою.")
                .NotEqual("string").WithMessage("Введіть реальну назву плейлиста.")
                .MaximumLength(50).WithMessage("Назва плейлиста занадто довга.");

            RuleFor(playlist => playlist.Description)
                .NotEqual("string").WithMessage("Введіть коректний опис або залиште поле пустим.");

            RuleFor(playlist => playlist.Description)
                .NotEmpty()
                .When(playlist => playlist.IsPublic)
                .WithMessage("Публічні плейлисти повинні мати опис.");
        }
    }
}