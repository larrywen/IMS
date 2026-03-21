using IMS.CoreBusiness;
using IMS.UseCases.Products;
using IMS.UseCases.PluginInterfaces;

namespace IMS.Plugins.InMemory
{
    public class ProductRepository : IProductRepository
    {
        private List<Product> _products;
        public ProductRepository()
        {
            _products = new List<Product>()
            {
                new Product { ProductId=1, ProductName="Bike", Quantity=10, Price=150 },
                new Product { ProductId=2, ProductName="Car", Quantity=10, Price=25000 },
            };   
        }

        public Task AddProductAsync(Product Product)
        {
            if (_products.Any(x => x.ProductName.Equals(Product.ProductName, StringComparison.OrdinalIgnoreCase)))
                {  return Task.CompletedTask; }

            var maxId = _products.Max(x => x.ProductId);
            Product.ProductId = maxId + 1;

            _products.Add(Product);
            return Task.CompletedTask;
        }

        public Task DeleteProductByIdAsync(int ProductId)
        {
            var Product = _products.FirstOrDefault(x => x.ProductId == ProductId);
            if(Product != null)
            {
                _products.Remove(Product);
            }
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
        {
            if(string.IsNullOrWhiteSpace(name))
                return await Task.FromResult(_products);

            return _products.Where(x => x.ProductName.Contains(name, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<Product> GetProductByIdAsync(int ProductId)
        {
            return await Task.FromResult(_products.First(x => x.ProductId == ProductId));
        }

        public Task UpdateProductAsync(Product Product)
        {
            if(_products.Any(x=>x.ProductId != Product.ProductId && 
                x.ProductName.Equals(Product.ProductName, StringComparison.OrdinalIgnoreCase)))
                return Task.CompletedTask;

            var invToUpdate = _products.FirstOrDefault(inv => inv.ProductId == Product.ProductId);
            if(invToUpdate is not null)
            {
                invToUpdate.ProductName = Product.ProductName;
                invToUpdate.Quantity = Product.Quantity;
                invToUpdate.Price = Product.Price;
            }

            return Task.CompletedTask;
        }
    }
}
