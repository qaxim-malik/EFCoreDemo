using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EFCoreDemo.Domain.Context;
using EFCoreDemo.Domain.Entities.SimpleEntities;
using EFCoreDemo.Dto;
using EFCoreDemo.Domain.Entities.AdvanceEntities;

namespace EFCoreDemo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly EFCoreDemoContext _context;

    public ProductsController(EFCoreDemoContext context)
    {
        _context = context;
    }

    #region CRUD
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        return await _context.Products.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        return product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct([FromBody] ProductDto productDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var product = new Product
        {
            Name = productDto.Name,
            Description = productDto.Description,
            Price = productDto.Price,
            CreatedBy = "AccountService"
        };
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductDto productDto)
    {
        var productFromDB = await _context.Products.FirstOrDefaultAsync(a => a.Id == id);

        if (productFromDB == null)
        {
            return NotFound();
        }

        productFromDB.Name = productDto.Name;
        productFromDB.Description = productDto.Description;
        productFromDB.Price = productDto.Price;
        productFromDB.ModifiedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return NoContent();

        // soft delete
        //product.IsDeleted = true;
        //await _context.SaveChangesAsync();
    }

    #endregion

    #region Raw SQL
    [HttpGet("FromSql")]
    public async Task<ActionResult<IEnumerable<Product>>> GetProductsByNameFromSql(string name)
    {
        var products = await _context.Products
            .FromSql($"SELECT * FROM Products WHERE Name LIKE %{name}%")
            .FirstOrDefaultAsync();

        // execute Stored Procedures
        var productsFromSP = _context.Products
            .FromSql($"EXECUTE dbo.GetAllProducts")
            .ToList();

        return Ok(products);
    }

    [HttpGet("SqlQuery")]
    public async Task<ActionResult<IEnumerable<Product>>> GetProductsSqlQuery()
    {
        var products = await _context.Database
            .SqlQuery<Product>($"SELECT * FROM Products")
            .ToListAsync();

        return Ok(products);
    }

    [HttpPost("ExecuteSql")]
    public async Task<IActionResult> ExecuteSqlCommand()
    {
        var rowsAffected = await _context.Database.ExecuteSqlAsync($"UPDATE Products SET Price = Price * 1.1");

        return Ok(new { RowsAffected = rowsAffected });
    }
    #endregion
}
