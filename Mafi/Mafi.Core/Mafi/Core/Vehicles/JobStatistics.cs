// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.JobStatistics
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Products;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Vehicles
{
  [GenerateSerializer(false, null, 0)]
  public struct JobStatistics
  {
    public ProductProto Product;
    public Quantity Quantity;
    public int JobsCount;

    public static void Serialize(JobStatistics value, BlobWriter writer)
    {
      writer.WriteInt(value.JobsCount);
      writer.WriteGeneric<ProductProto>(value.Product);
      Quantity.Serialize(value.Quantity, writer);
    }

    public static JobStatistics Deserialize(BlobReader reader)
    {
      return new JobStatistics()
      {
        JobsCount = reader.ReadInt(),
        Product = reader.ReadGenericAs<ProductProto>(),
        Quantity = Quantity.Deserialize(reader)
      };
    }
  }
}
