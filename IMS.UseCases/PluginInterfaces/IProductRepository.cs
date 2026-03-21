using IMS.CoreBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.UseCases.PluginInterfaces
{
    public interface IProductRepository
    {
        Task AddProductAsync(Product Product);
        Task DeleteProductByIdAsync(int ProductId);
        Task<IEnumerable<Product>> GetProductsByNameAsync(string name);
        Task<Product> GetProductByIdAsync(int ProductId);
        Task UpdateProductAsync(Product Product);
    }
}
