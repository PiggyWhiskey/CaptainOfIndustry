// Decompiled with JetBrains decompiler
// Type: Mafi.Numerics.Polygon3f
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Numerics
{
  [GenerateSerializer(false, null, 0)]
  public readonly struct Polygon3f
  {
    public readonly ImmutableArray<Vector3f> Vertices;

    public static void Serialize(Polygon3f value, BlobWriter writer)
    {
      ImmutableArray<Vector3f>.Serialize(value.Vertices, writer);
    }

    public static Polygon3f Deserialize(BlobReader reader)
    {
      return new Polygon3f(ImmutableArray<Vector3f>.Deserialize(reader));
    }

    public Polygon3f(ImmutableArray<Vector3f> vertices)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.Vertices = vertices;
    }

    public Polygon2f To2f()
    {
      return new Polygon2f(this.Vertices.Select<Vector2f>((Func<Vector3f, Vector2f>) (v => v.Xy)).ToImmutableArray<Vector2f>());
    }

    public Polygon3fFast MakeFast3f()
    {
      Polygon3fFast polygon;
      string error;
      if (!Polygon3fFast.TryCreateWithoutDegenerateEdges(this.Vertices.AsSlice, false, -1, out polygon, out error))
      {
        Log.Error("Failed to make polygon fast.");
        Polygon3fFast.TryCreateWithoutDegenerateEdges(ImmutableArray.Create<Vector3f>(Vector3f.Zero, Vector3f.One).AsSlice, false, -1, out polygon, out error);
      }
      return polygon;
    }
  }
}
