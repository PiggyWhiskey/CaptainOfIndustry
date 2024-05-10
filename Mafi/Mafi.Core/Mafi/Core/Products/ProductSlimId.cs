// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Products.ProductSlimId
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Products
{
  /// <summary>
  /// Special ID for terrain materials that is one byte. There is maximum of 255 unique terrain materials + phantom.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public readonly struct ProductSlimId : IEquatable<ProductSlimId>
  {
    public readonly byte Value;

    public static ProductSlimId PhantomId => new ProductSlimId();

    public bool IsPhantom => this.Value == (byte) 0;

    public bool IsNotPhantom => this.Value > (byte) 0;

    public ProductSlimId(byte value)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Value = value;
    }

    [Pure]
    public ProductProto ToFullOrPhantom(ProductsSlimIdManager manager)
    {
      return manager.ResolveOrPhantom(this);
    }

    public override string ToString() => this.Value.ToString();

    public static bool operator ==(ProductSlimId left, ProductSlimId right)
    {
      return (int) left.Value == (int) right.Value;
    }

    public static bool operator !=(ProductSlimId left, ProductSlimId right)
    {
      return (int) left.Value != (int) right.Value;
    }

    public bool Equals(ProductSlimId other) => (int) this.Value == (int) other.Value;

    public override bool Equals(object obj) => obj is ProductSlimId other && this.Equals(other);

    public override int GetHashCode() => (int) this.Value;

    public static void Serialize(ProductSlimId value, BlobWriter writer)
    {
      writer.WriteByte(value.Value);
    }

    public static ProductSlimId Deserialize(BlobReader reader)
    {
      return new ProductSlimId(reader.ReadByte());
    }
  }
}
