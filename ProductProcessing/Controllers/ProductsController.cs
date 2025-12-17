using Microsoft.AspNetCore.Mvc;
using ProductProcessing.Models;
using ProductProcessing.Services;

namespace ProductProcessing.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IProductService productService, ILogger<ProductsController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    /// <summary>
    /// Gets all products.
    /// </summary>
    /// <returns>List of products.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<Product>>> GetAll()
    {
        try
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all products");
            return StatusCode(500, "An error occurred while retrieving products");
        }
    }

    /// <summary>
    /// Gets a product by its identifier.
    /// </summary>
    /// <param name="id">Product identifier.</param>
    /// <returns>The requested product.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Product>> GetById(int id)
    {
        try
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound($"Product with id {id} not found");
            }
            return Ok(product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting product {Id}", id);
            return StatusCode(500, "An error occurred while retrieving the product");
        }
    }

    /// <summary>
    /// Creates a new product.
    /// </summary>
    /// <param name="product">Product payload.</param>
    /// <returns>The created product.</returns>
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Product>> Create([FromBody] Product product)
    {
        try
        {
            var createdProduct = await _productService.CreateProductAsync(product);
            return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating product");
            return StatusCode(500, "An error occurred while creating the product");
        }
    }

    /// <summary>
    /// Updates an existing product.
    /// </summary>
    /// <param name="id">Product identifier.</param>
    /// <param name="product">Updated product payload.</param>
    /// <returns>The updated product.</returns>
    [HttpPut("{id}")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Product>> Update(int id, [FromBody] Product product)
    {
        try
        {
            var updatedProduct = await _productService.UpdateProductAsync(id, product);
            return Ok(updatedProduct);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating product {Id}", id);
            return StatusCode(500, "An error occurred while updating the product");
        }
    }

    /// <summary>
    /// Deletes a product.
    /// </summary>
    /// <param name="id">Product identifier.</param>
    /// <returns>No content if deletion succeeds.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            var result = await _productService.DeleteProductAsync(id);
            if (!result)
            {
                return NotFound($"Product with id {id} not found");
            }
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting product {Id}", id);
            return StatusCode(500, "An error occurred while deleting the product");
        }
    }

    /// <summary>
    /// Calculates the discounted price for a product.
    /// </summary>
    /// <param name="id">Product identifier.</param>
    /// <param name="percentage">Discount percentage (0-100).</param>
    /// <returns>Original and discounted price info.</returns>
    [HttpGet("{id}/discount")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<decimal>> CalculateDiscount(int id, [FromQuery] decimal percentage)
    {
        try
        {
            var (originalPrice, discountedPrice) = await _productService.CalculateDiscountedPriceAsync(id, percentage);
            return Ok(new { originalPrice, discountedPrice, discountPercentage = percentage });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating discount for product {Id}", id);
            return StatusCode(500, "An error occurred while calculating the discount");
        }
    }
}
