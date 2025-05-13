namespace ReGreenShop.Application.Common.Exceptions;
public class InsufficientQuantityException : Exception
{
    public InsufficientQuantityException(string productName)
      : base($"Not enough quantity available in stock for Product '{productName}'.")
    {
    }
}
