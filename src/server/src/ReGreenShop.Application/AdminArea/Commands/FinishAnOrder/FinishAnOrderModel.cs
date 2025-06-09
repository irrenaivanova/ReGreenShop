namespace ReGreenShop.Application.AdminArea.Commands.FinishAnOrder;
public class FinishAnOrderModel
{
    public string OrderId { get; set; } = string.Empty;

    public IEnumerable<GreenModel> GreenModels { get; set; } = new List<GreenModel>();
}
