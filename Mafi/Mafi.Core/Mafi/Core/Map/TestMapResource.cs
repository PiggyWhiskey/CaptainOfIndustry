// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Map.TestMapResource
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Prototypes;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Map
{
  [GenerateSerializer(false, null, 0)]
  public readonly struct TestMapResource
  {
    public readonly Proto.ID ResourceId;
    public readonly Percent Size;
    public readonly Percent Depth;
    public readonly Percent Height;

    public TestMapResource(Proto.ID resourceId, Percent? size = null, Percent? depth = null, Percent? height = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.ResourceId = resourceId;
      Percent? nullable = size;
      this.Size = nullable ?? Percent.Hundred;
      nullable = depth;
      this.Depth = nullable ?? Percent.Hundred;
      nullable = height;
      this.Height = nullable ?? Percent.Hundred;
    }

    public static void Serialize(TestMapResource value, BlobWriter writer)
    {
      Proto.ID.Serialize(value.ResourceId, writer);
      Percent.Serialize(value.Size, writer);
      Percent.Serialize(value.Depth, writer);
      Percent.Serialize(value.Height, writer);
    }

    public static TestMapResource Deserialize(BlobReader reader)
    {
      return new TestMapResource(Proto.ID.Deserialize(reader), new Percent?(Percent.Deserialize(reader)), new Percent?(Percent.Deserialize(reader)), new Percent?(Percent.Deserialize(reader)));
    }
  }
}
