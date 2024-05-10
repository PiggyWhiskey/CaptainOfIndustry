// Decompiled with JetBrains decompiler
// Type: Mafi.Numerics.Polygon2fMutable
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
  public class Polygon2fMutable
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private LystStruct<Vector2f> m_vertices;
    public readonly int MinVertexCount;
    public readonly int MaxVertexCount;
    private static readonly Fix64 MAX_POLYGON_SQUARED_SIZE;

    public static void Serialize(Polygon2fMutable value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<Polygon2fMutable>(value))
        return;
      writer.EnqueueDataSerialization((object) value, Polygon2fMutable.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      LystStruct<Vector2f>.Serialize(this.m_vertices, writer);
      writer.WriteInt(this.MaxVertexCount);
      writer.WriteInt(this.MinVertexCount);
    }

    public static Polygon2fMutable Deserialize(BlobReader reader)
    {
      Polygon2fMutable polygon2fMutable;
      if (reader.TryStartClassDeserialization<Polygon2fMutable>(out polygon2fMutable))
        reader.EnqueueDataDeserialization((object) polygon2fMutable, Polygon2fMutable.s_deserializeDataDelayedAction);
      return polygon2fMutable;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.m_vertices = LystStruct<Vector2f>.Deserialize(reader);
      reader.SetField<Polygon2fMutable>(this, "MaxVertexCount", (object) reader.ReadInt());
      reader.SetField<Polygon2fMutable>(this, "MinVertexCount", (object) reader.ReadInt());
    }

    public ReadOnlyArraySlice<Vector2f> Vertices => this.m_vertices.BackingArrayAsSlice;

    public Polygon2fMutable(int minVertexCount = 3, int maxVertexCount = 1000)
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

    public Vector2f this[int i]
    {
      get => this.m_vertices[i];
      set => this.m_vertices[i] = value;
    }

    public Vector2f GetVerticesAveragePosition()
    {
      Vector2f zero = Vector2f.Zero;
      if (this.m_vertices.IsNotEmpty)
      {
        foreach (Vector2f vertex in this.m_vertices)
          zero += vertex;
        zero /= (Fix32) this.m_vertices.Count;
      }
      return zero;
    }

    public Polygon2f GetPolygon() => new Polygon2f(this.m_vertices.ToImmutableArray());

    public bool TryGetFastPolygon(out Polygon2fFast polygon, out string error)
    {
      return Polygon2fFast.TryCreateWithoutDegenerateEdges(this.Vertices, out polygon, out error);
    }

    public void Initialize(IEnumerable<Vector2f> vertices)
    {
      this.m_vertices.Clear();
      int num = 0;
      foreach (Vector2f vertex in vertices)
      {
        this.m_vertices.Add(vertex);
        ++num;
        if (num > this.MaxVertexCount)
          break;
      }
      this.clampToMaxSize();
      if (this.m_vertices.Count >= this.MinVertexCount)
        return;
      Log.Warning("Polygon initialized with too few vertices, " + string.Format("min: {0}, actual: {1}.", (object) this.MinVertexCount, (object) this.m_vertices.Count));
    }

    /// <summary>
    /// Limits max distance between any two vertices. If this limit is exceeded we shrink the polygon
    /// toward its centre point such that the limit is respected.
    /// </summary>
    private void clampToMaxSize()
    {
      Fix64 rhs = Fix64.Zero;
      foreach (Vector2f vertex1 in this.m_vertices)
      {
        foreach (Vector2f vertex2 in this.m_vertices)
        {
          if (!(vertex1 == vertex2))
            rhs = rhs.Max(vertex2.DistanceSqrTo(vertex1));
        }
      }
      if (rhs < Polygon2fMutable.MAX_POLYGON_SQUARED_SIZE)
        return;
      Vector2f zero = Vector2f.Zero;
      foreach (Vector2f vertex in this.m_vertices)
        zero += vertex;
      Vector2f vector2f = zero / (Fix32) this.m_vertices.Count;
      Fix32 fix32 = Polygon2fMutable.MAX_POLYGON_SQUARED_SIZE.DivByPositiveUncheckedUnrounded(rhs).ToFix32();
      for (int index = 0; index < this.m_vertices.Count; ++index)
        this.m_vertices[index] = vector2f + (this.m_vertices[index] - vector2f) * fix32;
    }

    public void InsertVertexAt(Vector2f position, int index)
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

    public void ComputeBounds(out Vector2f min, out Vector2f max)
    {
      if (this.m_vertices.IsEmpty)
      {
        min = max = Vector2f.Zero;
      }
      else
      {
        min = max = this.m_vertices.First;
        for (int index = 1; index < this.m_vertices.Count; ++index)
        {
          Vector2f vertex = this.m_vertices[index];
          min = min.Min(vertex);
          max = max.Max(vertex);
        }
      }
    }

    public void TranslateBy(Vector2f delta)
    {
      for (int index = 0; index < this.m_vertices.Count; ++index)
        this.m_vertices[index] += delta;
    }

    public void RotateBy(Fix32 degrees)
    {
      Vector2f zero = Vector2f.Zero;
      foreach (Vector2f vertex in this.m_vertices)
        zero += vertex;
      Vector2f vector2f1 = zero / (Fix32) this.m_vertices.Count;
      float radians = degrees.ToFloat() * ((float) Math.PI / 180f);
      Fix32 fix32_1 = MafiMath.Sin(radians).ToFix32();
      Fix32 fix32_2 = MafiMath.Cos(radians).ToFix32();
      for (int index = 0; index < this.m_vertices.Count; ++index)
      {
        Vector2f vector2f2 = this.m_vertices[index] - vector2f1;
        Vector2f vector2f3 = new Vector2f(fix32_2 * vector2f2.X - fix32_1 * vector2f2.Y, fix32_1 * vector2f2.X + fix32_2 * vector2f2.Y);
        this.m_vertices[index] = vector2f1 + vector2f3;
      }
    }

    /// <summary>
    /// Enumerates edges. The first edge is a line between first and second vertex.
    /// </summary>
    public IEnumerable<Line2f> EnumerateEdges()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerable<Line2f>) new Polygon2fMutable.\u003CEnumerateEdges\u003Ed__26(-2)
      {
        \u003C\u003E4__this = this
      };
    }

    static Polygon2fMutable()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      Polygon2fMutable.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Polygon2fMutable) obj).SerializeData(writer));
      Polygon2fMutable.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Polygon2fMutable) obj).DeserializeData(reader));
      Polygon2fMutable.MAX_POLYGON_SQUARED_SIZE = (Fix32.MaxValue / 2).ToFix64();
    }
  }
}
