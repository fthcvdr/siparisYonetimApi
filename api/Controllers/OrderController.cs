using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using api.Models;
using api.DbOperations;
[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public OrderController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Yeni Sipariş Ekleme
    [HttpPost("create")]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto orderDto)
    {
        // Stok kontrolü yapılacak
        foreach (var item in orderDto.OrderItems)
        {
            var product = await _context.Products.FindAsync(item.ProductId);
            if (product == null || product.StockQuantity < item.Quantity)
            {
                return BadRequest($"Ürün {item.ProductId} için yeterli stok yok.");
            }
        }

        // Yeni sipariş oluşturuluyor
        var order = new Order
        {
            UserId = orderDto.UserId,
            OrderDate = DateTime.Now,
            IsCompleted = false,  // Sipariş tamamlanmamış olarak işaretleniyor
            TotalAmount = orderDto.OrderItems.Sum(item => item.Quantity * item.UnitPrice),
            OrderItems = orderDto.OrderItems.Select(item => new OrderItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice
            }).ToList()
        };

        // Siparişi kaydet
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        // Stokları güncelle
        foreach (var item in orderDto.OrderItems)
        {
            var product = await _context.Products.FindAsync(item.ProductId);
            product.StockQuantity -= item.Quantity;
        }

        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
    }

    // Siparişleri Listeleme
    [HttpGet("list/{userId}")]
    public async Task<IActionResult> GetOrders(int userId)
    {
        var orders = await _context.Orders
            .Where(o => o.UserId == userId)
            .Select(o => new
            {
                o.Id,
                o.OrderDate,
                o.TotalAmount,
                o.IsCompleted
            })
            .ToListAsync();

        return Ok(orders);
    }

    // Sipariş Detayını Getirme
    [HttpGet("detail/{id}")]
    public async Task<IActionResult> GetOrder(int id)
    {
        var order = await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
        {
            return NotFound("Sipariş bulunamadı.");
        }

        var orderDetails = new
        {
            order.Id,
            order.OrderDate,
            order.TotalAmount,
            order.IsCompleted,
            Items = order.OrderItems.Select(oi => new
            {
                oi.Product.Name,
                oi.Quantity,
                oi.UnitPrice,
                TotalPrice = oi.Quantity * oi.UnitPrice
            })
        };

        return Ok(orderDetails);
    }

    // Sipariş Silme
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var order = await _context.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
        {
            return NotFound("Sipariş bulunamadı.");
        }

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();

        return NoContent();  // Başarıyla silindi
    }
}
