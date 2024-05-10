// Decompiled with JetBrains decompiler
// Type: Mafi.Numerics.Polygon2f
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
  public readonly struct Polygon2f
  {
    public readonly ImmutableArray<Vector2f> Vertices;

    public static void Serialize(Polygon2f value, BlobWriter writer)
    {
      ImmutableArray<Vector2f>.Serialize(value.Vertices, writer);
    }

    public static Polygon2f Deserialize(BlobReader reader)
    {
      return new Polygon2f(ImmutableArray<Vector2f>.Deserialize(reader));
    }

    public Polygon2f(ImmutableArray<Vector2f> vertices)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.Vertices = vertices;
    }

    public Polygon2fFast MakeFast2f()
    {
      Polygon2fFast polygon;
      string error;
      if (!Polygon2fFast.TryCreateWithoutDegenerateEdges(this.Vertices.AsSlice, out polygon, out error))
      {
        Log.Error("Failed to make polygon fast.");
        Polygon2fFast.TryCreateWithoutDegenerateEdges(ImmutableArray.Create<Vector2f>(Vector2f.Zero, Vector2f.One).AsSlice, out polygon, out error);
      }
      return polygon;
    }
  }
}
