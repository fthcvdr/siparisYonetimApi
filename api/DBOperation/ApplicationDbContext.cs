using Microsoft.EntityFrameworkCore;
using api.Models;
namespace api.DbOperations
{
    public class CreateOrderDto
    {
    public int UserId { get; set; }
    public List<OrderItemDto> OrderItems { get; set; }
    }

    public class OrderItemDto
    {
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    }

}
