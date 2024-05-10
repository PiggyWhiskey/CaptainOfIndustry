// Decompiled with JetBrains decompiler
// Type: Mafi.Numerics.Polygon3iMutable
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
  public class Polygon3iMutable
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private LystStruct<Vector3i> m_vertices;
    public readonly int MinVertexCount;
    public readonly int MaxVertexCount;

    public static void Serialize(Polygon3iMutable value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<Polygon3iMutable>(value))
        return;
      writer.EnqueueDataSerialization((object) value, Polygon3iMutable.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      LystStruct<Vector3i>.Serialize(this.m_vertices, writer);
      writer.WriteInt(this.MaxVertexCount);
      writer.WriteInt(this.MinVertexCount);
    }

    public static Polygon3iMutable Deserialize(BlobReader reader)
    {
      Polygon3iMutable polygon3iMutable;
      if (reader.TryStartClassDeserialization<Polygon3iMutable>(out polygon3iMutable))
        reader.EnqueueDataDeserialization((object) polygon3iMutable, Polygon3iMutable.s_deserializeDataDelayedAction);
      return polygon3iMutable;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.m_vertices = LystStruct<Vector3i>.Deserialize(reader);
      reader.SetField<Polygon3iMutable>(this, "MaxVertexCount", (object) reader.ReadInt());
      reader.SetField<Polygon3iMutable>(this, "MinVertexCount", (object) reader.ReadInt());
    }

    public ReadOnlyArraySlice<Vector3i> Vertices => this.m_vertices.BackingArrayAsSlice;

    public Polygon3iMutable(int minVertexCount = 3, int maxVertexCount = 1000)
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

    public bool TryGetFastPolygon(out Polygon3iFast polygon, out string error)
    {
      return Polygon3iFast.TryCreateWithoutDegenerateEdges(this.Vertices, out polygon, out error);
    }

    public void Initialize(IEnumerable<Vector3i> vertices)
    {
      this.m_vertices.Clear();
      int num = 0;
      foreach (Vector3i vertex in vertices)
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

    public void InsertVertexAt(Vector3i position, int index)
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

    static Polygon3iMutable()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      Polygon3iMutable.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Polygon3iMutable) obj).SerializeData(writer));
      Polygon3iMutable.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Polygon3iMutable) obj).DeserializeData(reader));
    }
  }
}
