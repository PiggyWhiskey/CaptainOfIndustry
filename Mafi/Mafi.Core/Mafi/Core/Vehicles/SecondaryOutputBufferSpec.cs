// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.SecondaryOutputBufferSpec
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
  public readonly struct SecondaryOutputBufferSpec
  {
    public readonly RegisteredOutputBuffer Buffer;
    public readonly Quantity Quantity;

    public static void Serialize(SecondaryOutputBufferSpec value, BlobWriter writer)
    {
      RegisteredOutputBuffer.Serialize(value.Buffer, writer);
      Quantity.Serialize(value.Quantity, writer);
    }

    public static SecondaryOutputBufferSpec Deserialize(BlobReader reader)
    {
      return new SecondaryOutputBufferSpec(RegisteredOutputBuffer.Deserialize(reader), Quantity.Deserialize(reader));
    }

    public SecondaryOutputBufferSpec(RegisteredOutputBuffer buffer, Quantity quantity)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Buffer = buffer;
      this.Quantity = quantity;
    }
  }
}
