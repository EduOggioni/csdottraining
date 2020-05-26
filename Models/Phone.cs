using FluentValidation;

namespace csdottraining.Models 
{
  public class Phone 
  {
    public string ddd { get; set; }
    public string number { get; set; }
  }

  public class PhoneValidator : AbstractValidator<Phone>
  {
    private const string Expression = "^[1-9]{2}";
    private const string ErrorMessage = "Value not allowed. Please, insert a valid one.";

    public PhoneValidator()
    {
      RuleFor(phone => phone.ddd)
      .NotNull()
      .Matches(Expression)
        .WithMessage(ErrorMessage);
      
      RuleFor(phone => phone.number)
        .NotNull()
        .Length(8, 9)
          .WithMessage(ErrorMessage);
    }
  }
}