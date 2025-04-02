using Microsoft.EntityFrameworkCore;
using api.Models;
namespace api.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }  // Mevcut stok miktarı
    }
}