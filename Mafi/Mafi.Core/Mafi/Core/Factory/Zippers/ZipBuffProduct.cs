// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Zippers.ZipBuffProduct
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Factory.Zippers
{
  [GenerateSerializer(false, null, 0)]
  public struct ZipBuffProduct
  {
    public readonly ProductQuantity ProductQuantity;
    public readonly SimStep EnqueuedAtStep;

    public ZipBuffProduct(ProductQuantity productQuantity, SimStep enqueuedAtStep)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.ProductQuantity = productQuantity;
      this.EnqueuedAtStep = enqueuedAtStep;
    }

    public static void Serialize(ZipBuffProduct value, BlobWriter writer)
    {
      ProductQuantity.Serialize(value.ProductQuantity, writer);
      SimStep.Serialize(value.EnqueuedAtStep, writer);
    }

    public static ZipBuffProduct Deserialize(BlobReader reader)
    {
      return new ZipBuffProduct(ProductQuantity.Deserialize(reader), SimStep.Deserialize(reader));
    }
  }
}
