// Decompiled with JetBrains decompiler
// Type: Mafi.Numerics.Polygon2i
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
  public readonly struct Polygon2i
  {
    public readonly ImmutableArray<Vector2i> Vertices;

    public static void Serialize(Polygon2i value, BlobWriter writer)
    {
      ImmutableArray<Vector2i>.Serialize(value.Vertices, writer);
    }

    public static Polygon2i Deserialize(BlobReader reader)
    {
      return new Polygon2i(ImmutableArray<Vector2i>.Deserialize(reader));
    }

    public Polygon2i(ImmutableArray<Vector2i> vertices)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.Vertices = vertices;
    }
  }
}
