using FluentValidation;

namespace csdottraining.Contracts
{
  public class SigninRequest
  {
    public string email { get; set; }
    public string password { get; set; }
  }
  public class SigninRequestValidator : AbstractValidator<SigninRequest> 
  {
    public SigninRequestValidator()
    {
      RuleFor(signin => signin.email).EmailAddress().NotNull();
      RuleFor(signin => signin.password).NotEmpty().NotNull();
    } 
  }
}