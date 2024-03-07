/*using FluentValidation;
using ClubManagement.ApplicationDbContext;

namespace ClubManagement.Models.Validators
{
    public class AccountRegisterDtoValidator: AbstractValidator<AccountRegisterDto>
    {
        private readonly ClubDbContext _dbContext;

        public AccountRegisterDtoValidator(ClubDbContext dbContext) 
        {
            // walidacja do AcountName
            *//*RuleFor(x => x.AccountName)
                .NotEmpty();*//*

            // Walidacja do Password
            RuleFor(x => x.PasswordHash)
                .MinimumLength(6);

            // Walidacja do ConfirmPassword
            RuleFor(x => x.ConfirmedPasswordHash)
                .Equal(e => e.PasswordHash);

            // Walidacja do Email
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Email)
                .Custom((value, context) =>
                {
                    var emailInUse = dbContext.Accounts.Any(a => a.Email == value);
                    if (emailInUse)
                    {
                        context.AddFailure("Email", "That email is taken");
                    }
                });
        }
    }
}
*/