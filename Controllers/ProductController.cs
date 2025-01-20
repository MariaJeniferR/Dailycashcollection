using Microsoft.AspNetCore.Mvc;
using API_Sample.Data;
using System.Linq;
using System;
using API_Sample.Models;

namespace API_Sample.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            var products = _context.Products.ToList();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
         
        // POST api/products
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] Product newProduct)
        {
            // Check if the received product is valid
            if (newProduct == null)
            {
                return BadRequest("Product data is invalid.");
            }

            // Add the new product to the database
            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();

            // Return the created product with a 201 status code (Created)
            return CreatedAtAction(nameof(GetProductById), new { id = newProduct.Id }, newProduct);
        }


        // PUT api/products/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product updatedProduct)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            // Update the product fields with the new values
            product.Name = updatedProduct.Name;
           
            // Save  updated product to the database
            await _context.SaveChangesAsync();

            return NoContent(); 
        }

        // DELETE api/products/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            // Find the product by ID
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            // Remove the product from the database
            _context.Products.Remove(product);

            // Save the changes
            await _context.SaveChangesAsync();

            return NoContent(); 
        }
    }
}
