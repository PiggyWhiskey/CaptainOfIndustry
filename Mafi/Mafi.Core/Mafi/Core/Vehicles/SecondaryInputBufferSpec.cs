// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.SecondaryInputBufferSpec
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Vehicles
{
  [GenerateSerializer(false, null, 0)]
  public readonly struct SecondaryInputBufferSpec
  {
    public readonly RegisteredInputBuffer Buffer;
    public readonly Quantity Quantity;

    public static void Serialize(SecondaryInputBufferSpec value, BlobWriter writer)
    {
      RegisteredInputBuffer.Serialize(value.Buffer, writer);
      Quantity.Serialize(value.Quantity, writer);
    }

    public static SecondaryInputBufferSpec Deserialize(BlobReader reader)
    {
      return new SecondaryInputBufferSpec(RegisteredInputBuffer.Deserialize(reader), Quantity.Deserialize(reader));
    }

    public SecondaryInputBufferSpec(RegisteredInputBuffer buffer, Quantity quantity)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Buffer = buffer;
      this.Quantity = quantity;
    }
  }
}
