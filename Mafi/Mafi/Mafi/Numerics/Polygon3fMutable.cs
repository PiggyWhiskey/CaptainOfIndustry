// Decompiled with JetBrains decompiler
// Type: Mafi.Numerics.Polygon3fMutable
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Numerics
{
  [GenerateSerializer(false, null, 0)]
  public class Polygon3fMutable
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private LystStruct<Vector3f> m_vertices;
    public readonly int MinVertexCount;
    public readonly int MaxVertexCount;
    public readonly int ClampZMinMax;
    private static readonly Fix64 MAX_POLYGON_SQUARED_SIZE;

    public static void Serialize(Polygon3fMutable value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<Polygon3fMutable>(value))
        return;
      writer.EnqueueDataSerialization((object) value, Polygon3fMutable.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteInt(this.ClampZMinMax);
      LystStruct<Vector3f>.Serialize(this.m_vertices, writer);
      writer.WriteInt(this.MaxVertexCount);
      writer.WriteInt(this.MinVertexCount);
      writer.WriteBool(this.RoundControlPointHeightsToWholeTiles);
    }

    public static Polygon3fMutable Deserialize(BlobReader reader)
    {
      Polygon3fMutable polygon3fMutable;
      if (reader.TryStartClassDeserialization<Polygon3fMutable>(out polygon3fMutable))
        reader.EnqueueDataDeserialization((object) polygon3fMutable, Polygon3fMutable.s_deserializeDataDelayedAction);
      return polygon3fMutable;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<Polygon3fMutable>(this, "ClampZMinMax", (object) reader.ReadInt());
      this.m_vertices = LystStruct<Vector3f>.Deserialize(reader);
      reader.SetField<Polygon3fMutable>(this, "MaxVertexCount", (object) reader.ReadInt());
      reader.SetField<Polygon3fMutable>(this, "MinVertexCount", (object) reader.ReadInt());
      this.RoundControlPointHeightsToWholeTiles = reader.ReadBool();
    }

    public ReadOnlyArraySlice<Vector3f> Vertices => this.m_vertices.BackingArrayAsSlice;

    public bool RoundControlPointHeightsToWholeTiles { get; set; }

    public Polygon3fMutable(
      int minVertexCount = 3,
      int maxVertexCount = 1000,
      int clampZMinMax = -1,
      bool roundControlPointHeightsToWholeTiles = false)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.MinVertexCount = minVertexCount;
      this.MaxVertexCount = maxVertexCount;
      this.ClampZMinMax = clampZMinMax;
      if (minVertexCount > maxVertexCount)
      {
        Log.Warning("Min vertex count is larger than max, swapping");
        Swap.Them<int>(ref this.MinVertexCount, ref this.MaxVertexCount);
      }
      this.RoundControlPointHeightsToWholeTiles = roundControlPointHeightsToWholeTiles;
    }

    public Vector3f this[int i]
    {
      get => this.m_vertices[i];
      set => this.m_vertices[i] = value;
    }

    public Vector3f GetVerticesAveragePosition()
    {
      Vector3f zero = Vector3f.Zero;
      if (this.m_vertices.IsNotEmpty)
      {
        foreach (Vector3f vertex in this.m_vertices)
          zero += vertex;
        zero /= (Fix32) this.m_vertices.Count;
      }
      return zero;
    }

    public Polygon3f GetPolygon()
    {
      ImmutableArray<Vector3f> vertices = this.m_vertices.ToImmutableArray();
      if (this.RoundControlPointHeightsToWholeTiles)
        vertices = vertices.Map<Vector3f>((Func<Vector3f, Vector3f>) (x => new Vector3f(x.X, x.Y, x.Z.Rounded())));
      if (this.ClampZMinMax >= 0)
        vertices = vertices.Map<Vector3f>((Func<Vector3f, Vector3f>) (x => new Vector3f(x.X, x.Y, x.Z.Clamp((Fix32) -this.ClampZMinMax, (Fix32) this.ClampZMinMax))));
      return new Polygon3f(vertices);
    }

    public bool TryGetFastPolygon(out Polygon3fFast polygon, out string error)
    {
      return Polygon3fFast.TryCreateWithoutDegenerateEdges(this.Vertices, this.RoundControlPointHeightsToWholeTiles, this.ClampZMinMax, out polygon, out error);
    }

    public void Initialize(IEnumerable<Vector3f> vertices)
    {
      this.m_vertices.Clear();
      int num = 0;
      foreach (Vector3f vertex in vertices)
      {
        if (this.ClampZMinMax >= 0)
          this.m_vertices.Add(new Vector3f(vertex.X, vertex.Y, vertex.Z.Clamp((Fix32) -this.ClampZMinMax, (Fix32) this.ClampZMinMax)));
        else
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
      foreach (Vector3f vertex1 in this.m_vertices)
      {
        foreach (Vector3f vertex2 in this.m_vertices)
        {
          if (!(vertex1 == vertex2))
            rhs = rhs.Max(vertex2.DistanceSqrTo(vertex1));
        }
      }
      if (rhs < Polygon3fMutable.MAX_POLYGON_SQUARED_SIZE)
        return;
      Vector3f zero = Vector3f.Zero;
      foreach (Vector3f vertex in this.m_vertices)
        zero += vertex;
      Vector3f vector3f = zero / (Fix32) this.m_vertices.Count;
      Fix32 fix32 = Polygon3fMutable.MAX_POLYGON_SQUARED_SIZE.DivByPositiveUncheckedUnrounded(rhs).ToFix32();
      for (int index = 0; index < this.m_vertices.Count; ++index)
        this.m_vertices[index] = vector3f + (this.m_vertices[index] - vector3f) * fix32;
    }

    public void InsertVertexAt(Vector3f position, int index)
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

    public void ComputeBounds(out Vector3f min, out Vector3f max)
    {
      if (this.m_vertices.IsEmpty)
      {
        min = max = Vector3f.Zero;
      }
      else
      {
        min = max = this.m_vertices.First;
        for (int index = 1; index < this.m_vertices.Count; ++index)
        {
          Vector3f vertex = this.m_vertices[index];
          min = min.Min(vertex);
          max = max.Max(vertex);
        }
      }
    }

    public void TranslateBy(Vector3f delta)
    {
      if (this.ClampZMinMax >= 0)
      {
        for (int index = 0; index < this.m_vertices.Count; ++index)
        {
          Vector3f vector3f = this.m_vertices[index] + delta;
          this.m_vertices[index] = new Vector3f(vector3f.X, vector3f.Y, vector3f.Z.Clamp((Fix32) -this.ClampZMinMax, (Fix32) this.ClampZMinMax));
        }
      }
      else
      {
        for (int index = 0; index < this.m_vertices.Count; ++index)
          this.m_vertices[index] += delta;
      }
    }

    public void RotateBy(Fix32 degrees)
    {
      Vector2f zero = Vector2f.Zero;
      foreach (Vector3f vertex in this.m_vertices)
        zero += vertex.Xy;
      Vector2f vector2f1 = zero / (Fix32) this.m_vertices.Count;
      float radians = degrees.ToFloat() * ((float) Math.PI / 180f);
      Fix32 fix32_1 = MafiMath.Sin(radians).ToFix32();
      Fix32 fix32_2 = MafiMath.Cos(radians).ToFix32();
      for (int index = 0; index < this.m_vertices.Count; ++index)
      {
        Vector2f vector2f2 = this.m_vertices[index].Xy - vector2f1;
        Vector2f vector2f3 = new Vector2f(fix32_2 * vector2f2.X - fix32_1 * vector2f2.Y, fix32_1 * vector2f2.X + fix32_2 * vector2f2.Y);
        this.m_vertices[index] = new Vector3f(vector2f1 + vector2f3, this.m_vertices[index].Z);
      }
    }

    /// <summary>
    /// Enumerates edges. The first edge is a line between first and second vertex.
    /// </summary>
    public IEnumerable<Line3f> EnumerateEdges()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerable<Line3f>) new Polygon3fMutable.\u003CEnumerateEdges\u003Ed__31(-2)
      {
        \u003C\u003E4__this = this
      };
    }

    static Polygon3fMutable()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      Polygon3fMutable.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Polygon3fMutable) obj).SerializeData(writer));
      Polygon3fMutable.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Polygon3fMutable) obj).DeserializeData(reader));
      Polygon3fMutable.MAX_POLYGON_SQUARED_SIZE = (Fix32.MaxValue / 2).ToFix64();
    }
  }
}
