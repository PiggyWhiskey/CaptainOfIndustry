// Decompiled with JetBrains decompiler
// Type: Mafi.Numerics.Polygon2iMutable
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Numerics
{
  [GenerateSerializer(false, null, 0)]
  public class Polygon2iMutable
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private LystStruct<Vector2i> m_vertices;
    public readonly int MinVertexCount;
    public readonly int MaxVertexCount;

    public static void Serialize(Polygon2iMutable value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<Polygon2iMutable>(value))
        return;
      writer.EnqueueDataSerialization((object) value, Polygon2iMutable.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      LystStruct<Vector2i>.Serialize(this.m_vertices, writer);
      writer.WriteInt(this.MaxVertexCount);
      writer.WriteInt(this.MinVertexCount);
    }

    public static Polygon2iMutable Deserialize(BlobReader reader)
    {
      Polygon2iMutable polygon2iMutable;
      if (reader.TryStartClassDeserialization<Polygon2iMutable>(out polygon2iMutable))
        reader.EnqueueDataDeserialization((object) polygon2iMutable, Polygon2iMutable.s_deserializeDataDelayedAction);
      return polygon2iMutable;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.m_vertices = LystStruct<Vector2i>.Deserialize(reader);
      reader.SetField<Polygon2iMutable>(this, "MaxVertexCount", (object) reader.ReadInt());
      reader.SetField<Polygon2iMutable>(this, "MinVertexCount", (object) reader.ReadInt());
    }

    public ReadOnlyArraySlice<Vector2i> Vertices => this.m_vertices.BackingArrayAsSlice;

    public Polygon2iMutable(int minVertexCount = 3, int maxVertexCount = 1000)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.MinVertexCount = minVertexCount;
      this.MaxVertexCount = maxVertexCount;
      if (minVertexCount <= maxVertexCount)
        return;
      Log.Warning("Min vertex count is larger than max, swapping");
      Swap.Them<int>(ref this.MinVertexCount, ref this.MaxVertexCount);
    }

    public bool TryGetFastPolygon(out Polygon2iFast polygon, out string error)
    {
      return Polygon2iFast.TryCreateWithoutDegenerateEdges(this.Vertices, out polygon, out error);
    }

    public void Initialize(IEnumerable<Vector2i> vertices)
    {
      this.m_vertices.Clear();
      int num = 0;
      foreach (Vector2i vertex in vertices)
      {
        this.m_vertices.Add(vertex);
        ++num;
        if (num > this.MaxVertexCount)
          break;
      }
      if (this.m_vertices.Count >= this.MinVertexCount)
        return;
      Log.Warning("Polygon initialized with too few vertices, " + string.Format("min: {0}, actual: {1}.", (object) this.MinVertexCount, (object) this.m_vertices.Count));
    }

    public void InsertVertexAt(Vector2i position, int index)
    {
      if (this.m_vertices.Count >= this.MaxVertexCount)
        return;
      this.m_vertices.Insert(index, position);
    }

    public void RemoveVertexAt(int index)
    {
      if (this.m_vertices.Count <= this.MinVertexCount)
        return;
      this.m_vertices.RemoveAt(index);
    }

    static Polygon2iMutable()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      Polygon2iMutable.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Polygon2iMutable) obj).SerializeData(writer));
      Polygon2iMutable.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Polygon2iMutable) obj).DeserializeData(reader));
    }
  }
}
