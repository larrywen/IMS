using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.CoreBusiness.Validations;
public class Product_EnsurePriceIsGreateThanInventoriesCost : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var product = (Product)validationContext.ObjectInstance;
        if(product != null)
        {
            if (!ValidatePricing(product))
            {
                return new ValidationResult($"The product's price is less than the inventories cost: {TotalInventoriesCost(product).ToString("c")}",
                    new List<string>() { validationContext.MemberName});
            }
        }

        return ValidationResult.Success;
    }

    private double TotalInventoriesCost(Product product)
    {
        return product.ProductInventories.Sum(x => x.Inventory?.Price * x.InventoryQuantity ?? 0);
    }

    private bool ValidatePricing(Product product)
    {
        if(product.ProductInventories == null || product.ProductInventories.Count <= 0)
        {
            return true; // No inventories, so no cost to compare against
        }
        if(TotalInventoriesCost(product) > product.Price)
        {
            return false; // Price is less than total cost of inventories
        }
        return true;
    }
}
