namespace ReGreenShop.Application.Common.Exceptions;
public class InsufficientQuantityException : Exception
{
    public InsufficientQuantityException(string productName)
           : base($"Not enough quantity available in stock for Product '{productName}'.")
    {
    }

    public InsufficientQuantityException(string productName, int quantity)
          : base($"Not enough quantity available in stock for Product '{productName}'. Only {quantity} left")
    {
    }
}
