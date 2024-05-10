// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.IProductBufferReadOnly
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Products;

#nullable disable
namespace Mafi.Core.Entities.Static
{
  public interface IProductBufferReadOnly
  {
    /// <summary>Product contained by this buffer.</summary>
    ProductProto Product { get; }

    /// <summary>Available capacity for storing.</summary>
    Quantity UsableCapacity { get; }

    /// <summary>Storage capacity of this buffer.</summary>
    Quantity Capacity { get; }

    /// <summary>Available quantity for removing.</summary>
    Quantity Quantity { get; }
  }
}
