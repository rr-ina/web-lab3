using FluentValidation;
using lab3.DTOs;

namespace lab3.Validators
{
    public class SongValidator : AbstractValidator<CreateSongDto>
    {
        public SongValidator()
        {
            RuleFor(song => song.Title)
                .NotEmpty().WithMessage("Назва пісні є обов'язковою.")
                .NotEqual("string").WithMessage("Введіть реальну назву пісні.")
                .MaximumLength(50).WithMessage("Назва пісні занадто довга (максимум 50 символів).");

            RuleFor(song => song.Artist)
                .NotEmpty().WithMessage("Виконавець є обов'язковим.")
                .NotEqual("string").WithMessage("Введіть реальне ім'я виконавця.");

            RuleFor(song => song.DurationSeconds)
                .GreaterThan(0).WithMessage("Тривалість пісні має бути додатною.");

            RuleFor(song => song.PlaylistId)
                .GreaterThan(0).WithMessage("Необхідно вказати ідентифікатор плейлиста.");
        }
    }
}