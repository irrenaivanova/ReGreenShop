namespace ReGreenShop.Application.Common.Exceptions;
public class BusinessRulesException : Exception
{
    public BusinessRulesException(string message)
         : base(message)
    {
    }
}
