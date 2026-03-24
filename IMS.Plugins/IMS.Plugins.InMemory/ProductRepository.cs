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
            var prod = _products.FirstOrDefault(x => x.ProductId == ProductId);
            var newProd = new Product();
            if(prod != null)
            {
                newProd.ProductId = prod.ProductId;
                newProd.ProductName = prod.ProductName;
                newProd.Quantity = prod.Quantity;
                newProd.Price = prod.Price;
                newProd.ProductInventories = new List<ProductInventory>();
                if(prod.ProductInventories != null && prod.ProductInventories.Count > 0)
                {
                    foreach (var prodInv in prod.ProductInventories)
                    {
                        var newProdInv = new ProductInventory
                        {
                            InventoryId = prodInv.InventoryId,
                            ProductId = prodInv.ProductId,
                            Product = prod,
                            Inventory = new Inventory(),
                            InventoryQuantity = prodInv.InventoryQuantity,
                        };
                        if (prodInv.Inventory != null)
                        {
                            newProdInv.Inventory.InventoryId = prodInv.Inventory.InventoryId;
                            newProdInv.Inventory.InventoryName = prodInv.Inventory.InventoryName;
                            newProdInv.Inventory.Price = prodInv.Inventory.Price;
                            newProdInv.Inventory.Quantity = prodInv.Inventory.Quantity;
                        }

                        newProd.ProductInventories.Add(newProdInv);
                    }
                }
            }
            return await Task.FromResult(newProd);
        }

        public Task UpdateProductAsync(Product product)
        {
            if(_products.Any(x=>x.ProductId != product.ProductId && 
                x.ProductName.Equals(product.ProductName, StringComparison.OrdinalIgnoreCase)))
                return Task.CompletedTask;

            var prod = _products.FirstOrDefault(x => x.ProductId == product.ProductId);
            if(prod != null)
            {
                prod.ProductName = product.ProductName;
                prod.Quantity = product.Quantity;
                prod.Price = product.Price;
                prod.ProductInventories = product.ProductInventories;
            }

            return Task.CompletedTask;
        }

        public Task<bool> ExistsAsync(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
