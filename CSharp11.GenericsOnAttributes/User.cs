// More robust attributes, for example:
// [Validator<object>] wouldn't compile, because the ValidatorAttribute expects 
// a type that implements IValidator

namespace CSharp11.GenericsOnAttributes;

[Validator<UserValidator>]
public class User
{

}