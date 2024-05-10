// Decompiled with JetBrains decompiler
// Type: Mafi.Numerics.Polygon3i
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Numerics
{
  [GenerateSerializer(false, null, 0)]
  public readonly struct Polygon3i
  {
    public readonly ImmutableArray<Vector3i> Vertices;

    public static void Serialize(Polygon3i value, BlobWriter writer)
    {
      ImmutableArray<Vector3i>.Serialize(value.Vertices, writer);
    }

    public static Polygon3i Deserialize(BlobReader reader)
    {
      return new Polygon3i(ImmutableArray<Vector3i>.Deserialize(reader));
    }

    public Polygon3i(ImmutableArray<Vector3i> vertices)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.Vertices = vertices;
    }
  }
}
