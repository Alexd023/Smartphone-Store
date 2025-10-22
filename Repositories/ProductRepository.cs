using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Smartphone_Store.Models;
using Smartphone_Store.Repositories.Interfaces;
namespace Smartphone_Store.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductRepository(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products.Include(p => p.Images)
                                          .FirstOrDefaultAsync(p => p.ProductID == id);
        }

        public async Task AddProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task<SearchResultsViewModel> SearchProductsAsync(string query, string[] brand, decimal? minPrice, decimal? maxPrice, string sortOrder, bool isUserSpecifiedMinPrice, bool isUserSpecifiedMaxPrice)
        {
            var productsQuery = _context.Products.Include(p => p.Images).AsQueryable();

            if (!string.IsNullOrEmpty(query))
            {
                productsQuery = productsQuery.Where(p => p.Name.Contains(query) || p.Brand.Contains(query));
            }

            if (brand != null && brand.Length > 0)
            {
                productsQuery = productsQuery.Where(p => brand.Contains(p.Brand));
            }

            var dynamicMinPrice = decimal.Zero;
            var dynamicMaxPrice = decimal.Zero;

            if (!productsQuery.IsNullOrEmpty())
            {
                dynamicMinPrice = await productsQuery.MinAsync(p => p.Price);
                dynamicMaxPrice = await productsQuery.MaxAsync(p => p.Price);
            }

            if (minPrice.HasValue && isUserSpecifiedMinPrice)
            {
                productsQuery = productsQuery.Where(p => p.Price >= minPrice.Value);
            }
            else
            {
                minPrice = dynamicMinPrice;
            }

            if (maxPrice.HasValue && isUserSpecifiedMaxPrice)
            {
                productsQuery = productsQuery.Where(p => p.Price <= maxPrice.Value);
            }
            else
            {
                maxPrice = dynamicMaxPrice;
            }

            switch (sortOrder)
            {
                case "priceAsc":
                    productsQuery = productsQuery.OrderBy(p => p.Price);
                    break;
                case "priceDesc":
                    productsQuery = productsQuery.OrderByDescending(p => p.Price);
                    break;
                default:
                    productsQuery = productsQuery.OrderBy(p => p.Name);
                    break;
            }

            if (minPrice < dynamicMinPrice)
            {
                minPrice = dynamicMinPrice;
                isUserSpecifiedMinPrice = false;
            }

            if (maxPrice > dynamicMaxPrice)
            {
                maxPrice = dynamicMaxPrice;
                isUserSpecifiedMaxPrice = false;
            }

            var products = await productsQuery.ToListAsync();

            var viewModel = new SearchResultsViewModel
            {
                Products = products,
                Brands = await _context.Products.Select(p => p.Brand).Distinct().ToListAsync(),
                SelectedBrands = brand,
                MinPrice = minPrice.Value,
                MaxPrice = maxPrice.Value,
                SearchQuery = query,
                SortOrder = sortOrder,
                IsUserSpecifiedMinPrice = isUserSpecifiedMinPrice,
                IsUserSpecifiedMaxPrice = isUserSpecifiedMaxPrice
            };

            return viewModel;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products.Include(p => p.Images).ToListAsync();
        }

        public async Task UpdateStockAsync(int productId, int quantity)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductID == productId);
            if (product != null)
            {
                product.Stock -= quantity;
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateProductAsync(Product product)
        {
            var existingProduct = await _context.Products
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.ProductID == product.ProductID);

            if (existingProduct != null)
            {
                _context.Entry(existingProduct).State = EntityState.Detached;
            }

            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products.Include(p => p.Images).FirstOrDefaultAsync(p => p.ProductID == id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.Images.RemoveRange(product.Images);
                await _context.SaveChangesAsync();
            }
        }
    }
}