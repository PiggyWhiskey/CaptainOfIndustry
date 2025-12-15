using Mafi;
using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Products;

internal static class ObjectExtensions
{
    public static void Add(this Lyst<ProductQuantity> list, ProductProto product, Quantity quantity) => list.Add(new ProductQuantity(product, quantity));
}