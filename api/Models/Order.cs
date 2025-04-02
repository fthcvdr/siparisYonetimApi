using Microsoft.EntityFrameworkCore;
using api.Models;
namespace api.Models
{

    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }  // Siparişi veren kullanıcının ID'si
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsCompleted { get; set; }

       public ICollection<OrderItem> OrderItems { get; set; }
    }
}