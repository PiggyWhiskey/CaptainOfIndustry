// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Map.MapCellSurfaceGeneratorProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain.Generation;
using Mafi.Random.Noise;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Mafi.Core.Map
{
  public class MapCellSurfaceGeneratorProto : Proto
  {
    public readonly ImmutableArray<KeyValuePair<TerrainMaterialProto, ThicknessTilesF>> SurfaceMaterialsTopToBottom;
    public readonly ImmutableArray<KeyValuePair<TerrainMaterialProto, ThicknessTilesF>> SurfaceMaterialsTopToBottomAlt;
    public readonly ICellSurfaceGenerator Generator;
    public readonly SimplexNoise2dParams? AltSurfaceNoiseParams;
    public readonly NoiseTurbulenceParams? AltSurfNoiseTurbulenceParams;
    public readonly Fix64 AltNoiseStartTransition;
    public readonly Fix64 AltNoiseEndTransition;

    public MapCellSurfaceGeneratorProto(
      MapCellSurfaceGeneratorProto.ID id,
      Proto.Str strings,
      ImmutableArray<KeyValuePair<TerrainMaterialProto, ThicknessTilesF>> surfaceMaterialsTopToBottom,
      ImmutableArray<KeyValuePair<TerrainMaterialProto, ThicknessTilesF>>? surfaceMaterialsTopToBottomAlt = null,
      SimplexNoise2dParams? altSurfaceNoiseParams = null,
      NoiseTurbulenceParams? altSurfNoiseTurbulenceParams = null,
      Fix64? altNoiseStartTransition = null,
      Fix64? altNoiseEndTransition = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector((Proto.ID) id, strings);
      this.SurfaceMaterialsTopToBottom = surfaceMaterialsTopToBottom;
      this.AltSurfNoiseTurbulenceParams = altSurfNoiseTurbulenceParams;
      this.SurfaceMaterialsTopToBottomAlt = surfaceMaterialsTopToBottomAlt ?? ImmutableArray<KeyValuePair<TerrainMaterialProto, ThicknessTilesF>>.Empty;
      this.AltSurfaceNoiseParams = altSurfaceNoiseParams;
      Fix64? nullable = altNoiseStartTransition;
      this.AltNoiseStartTransition = nullable ?? Fix64.Zero;
      nullable = altNoiseEndTransition;
      this.AltNoiseEndTransition = nullable ?? Fix64.One;
      if (this.AltSurfaceNoiseParams.HasValue && (this.SurfaceMaterialsTopToBottom.Length != 1 || this.SurfaceMaterialsTopToBottomAlt.Length != 1))
        throw new InvalidProtoException("Exactly one material per layer must be set when interleaving using noise fn.");
      this.Generator = (ICellSurfaceGenerator) new MapCellSurfaceGenerator(this);
    }

    public MapCellSurfaceGeneratorProto.ID Id => new MapCellSurfaceGeneratorProto.ID(base.Id.Value);

    [DebuggerDisplay("{Value,nq}")]
    [DebuggerStepThrough]
    [ManuallyWrittenSerialization]
    public new readonly struct ID : 
      IEquatable<MapCellSurfaceGeneratorProto.ID>,
      IComparable<MapCellSurfaceGeneratorProto.ID>
    {
      /// <summary>Underlying string value of this Id.</summary>
      public readonly string Value;

      public ID(string value)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.Value = value;
      }

      /// <summary>
      /// Implicit conversion to parent <see cref="T:Mafi.Core.Prototypes.Proto.ID" />.
      /// </summary>
      public static implicit operator Proto.ID(MapCellSurfaceGeneratorProto.ID id)
      {
        return new Proto.ID(id.Value);
      }

      public static bool operator ==(Proto.ID lhs, MapCellSurfaceGeneratorProto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator ==(MapCellSurfaceGeneratorProto.ID lhs, Proto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(Proto.ID lhs, MapCellSurfaceGeneratorProto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(MapCellSurfaceGeneratorProto.ID lhs, Proto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator ==(
        MapCellSurfaceGeneratorProto.ID lhs,
        MapCellSurfaceGeneratorProto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(
        MapCellSurfaceGeneratorProto.ID lhs,
        MapCellSurfaceGeneratorProto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public override bool Equals(object other)
      {
        return other is MapCellSurfaceGeneratorProto.ID other1 && this.Equals(other1);
      }

      public bool Equals(MapCellSurfaceGeneratorProto.ID other)
      {
        return string.Equals(this.Value, other.Value, StringComparison.Ordinal);
      }

      public int CompareTo(MapCellSurfaceGeneratorProto.ID other)
      {
        return string.CompareOrdinal(this.Value, other.Value);
      }

      public override string ToString() => this.Value ?? string.Empty;

      public override int GetHashCode()
      {
        string str = this.Value;
        return str == null ? 0 : str.GetHashCode();
      }

      public static void Serialize(MapCellSurfaceGeneratorProto.ID value, BlobWriter writer)
      {
        writer.WriteString(value.Value);
      }

      public static MapCellSurfaceGeneratorProto.ID Deserialize(BlobReader reader)
      {
        return new MapCellSurfaceGeneratorProto.ID(reader.ReadString());
      }
    }
  }
}
