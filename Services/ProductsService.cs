using Microsoft.AspNetCore.Mvc.ModelBinding;
using Smartphone_Store.Models;
using Smartphone_Store.Repositories.Interfaces;

namespace Smartphone_Store.Services
{
    public class ProductsService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductsService(IProductRepository productRepository, IWebHostEnvironment hostEnvironment)
        {
            _productRepository = productRepository;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _productRepository.GetProductByIdAsync(id);
        }

        public async Task<bool> AddProductAsync(Product product, IFormFile[] images, ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                LogModelState(modelState);
                return false;
            }

            var uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "Images");
            if (images != null && images.Length > 0)
            {
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                foreach (var image in images)
                {
                    if (image.Length > 0)
                    {
                        var filePath = Path.Combine(uploadsFolder, image.FileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await image.CopyToAsync(fileStream);
                        }

                        var img = new Image
                        {
                            FilePath = $"Images/{image.FileName}",
                            Product = product
                        };
                        product.Images.Add(img);
                    }
                }
            }

            await _productRepository.AddProductAsync(product);
            return true;
        }

        public async Task<SearchResultsViewModel> SearchProductsAsync(string query, string[] brand, decimal? minPrice, decimal? maxPrice, string sortOrder, bool isUserSpecifiedMinPrice, bool isUserSpecifiedMaxPrice)
        {
            return await _productRepository.SearchProductsAsync(query, brand, minPrice, maxPrice, sortOrder, isUserSpecifiedMinPrice, isUserSpecifiedMaxPrice);
        }

        public async Task<bool> UpdateProductAsync(Product updatedProduct, IFormFileCollection images, ModelStateDictionary modelState, int[] existingImageIds)
        {
            if (!modelState.IsValid)
            {
                foreach (var modelStateKey in modelState.Keys)
                {
                    var value = modelState[modelStateKey];
                    foreach (var error in value.Errors)
                    {
                        Console.WriteLine($"Key: {modelStateKey}, Error: {error.ErrorMessage}");
                    }

                    return false;
                }
            }

            var uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "Images");

            var existingProduct = await _productRepository.GetProductByIdAsync(updatedProduct.ProductID);
            if (existingProduct == null)
            {
                return false;
            }

            var currentImages = existingProduct.Images.ToList();
            foreach (var image in currentImages)
            {
                if (!existingImageIds.Contains(image.Id))
                {
                    var filePath = Path.Combine(_hostEnvironment.WebRootPath, image.FilePath);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }

                    existingProduct.Images.Remove(image);
                }
            }

            if (images != null && images.Count > 0)
            {
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                foreach (var image in images)
                {
                    if (image.Length > 0)
                    {
                        var filePath = Path.Combine(uploadsFolder, image.FileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await image.CopyToAsync(fileStream);
                        }

                        var img = new Image
                        {
                            FilePath = $"Images/{image.FileName}",
                            ProductId = existingProduct.ProductID
                        };
                        existingProduct.Images.Add(img);
                    }
                }
            }

            existingProduct.Name = updatedProduct.Name;
            existingProduct.Brand = updatedProduct.Brand;
            existingProduct.ModelName = updatedProduct.ModelName;
            existingProduct.Price = updatedProduct.Price;
            existingProduct.Stock = updatedProduct.Stock;
            existingProduct.Description = updatedProduct.Description;
            existingProduct.SimType = updatedProduct.SimType;
            existingProduct.RAMSize = updatedProduct.RAMSize;
            existingProduct.ROMSize = updatedProduct.ROMSize;
            existingProduct.MicroSDsupport = updatedProduct.MicroSDsupport;
            existingProduct.ProcessorType = updatedProduct.ProcessorType;
            existingProduct.OperatingSystem = updatedProduct.OperatingSystem;
            existingProduct.DisplayType = updatedProduct.DisplayType;
            existingProduct.DisplayResolution = updatedProduct.DisplayResolution;
            existingProduct.BluetoothType = updatedProduct.BluetoothType;
            existingProduct.WiFiType = updatedProduct.WiFiType;
            existingProduct.NFCsupport = updatedProduct.NFCsupport;
            existingProduct.BatterySpecs = updatedProduct.BatterySpecs;
            existingProduct.BackCameraSpecs = updatedProduct.BackCameraSpecs;
            existingProduct.FrontCameraSpecs = updatedProduct.FrontCameraSpecs;
            existingProduct.Dimensions = updatedProduct.Dimensions;
            existingProduct.ExtraDetails = updatedProduct.ExtraDetails;

            await _productRepository.UpdateProductAsync(existingProduct);
            return true;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllProductsAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            await _productRepository.DeleteProductAsync(id);
        }
    
        private void LogModelState(ModelStateDictionary modelState)
        {
            foreach (var key in modelState.Keys)
            {
                var entry = modelState[key];
                foreach (var error in entry.Errors)
                {
                    Console.WriteLine($"Key: {key}, Error: {error.ErrorMessage}");
                }
            }
        }

        private async Task<(bool Success, List<Image> Images)> SaveImagesAsync(IFormFile[] images, Product product)
        {
            var result = new List<Image>();
            if (images == null || images.Length == 0)
                return (true, result);

            var uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "Images");
            try
            {
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                foreach (var image in images)
                {
                    if (image == null || image.Length <= 0) continue;
                    var fileName = image.FileName;
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    await using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        await image.CopyToAsync(fileStream);
                    }

                    result.Add(new Image
                    {
                        FilePath = $"Images/{fileName}",
                        Product = product
                    });
                }
                return (true, result);
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex);
                return (false, result);
            }
        }

        private void SafeDeleteFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex);
            }
        }
}
}