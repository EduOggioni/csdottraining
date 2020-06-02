using FluentValidation;
using System.Collections.Generic;
using csdottraining.Models;

namespace csdottraining.Contracts
{
  public class SignupResquest 
  {
    public string name { get; set; }
    public string email { get; set; }
    public string password { get; set; }
    public List<Phone> phones { get; set; }

  }
  public class SignupResquestValidator : AbstractValidator<SignupResquest> 
  {
    public SignupResquestValidator()
    {
      RuleFor(signup => signup.name).Length(2, 10);
      RuleFor(signup => signup.email).EmailAddress().NotNull();
      RuleFor(signup => signup.password).NotEmpty().NotNull();
      RuleForEach(x => x.phones).SetValidator(new PhoneValidator());
    } 
  }
}