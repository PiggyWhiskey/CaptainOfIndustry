// Decompiled with JetBrains decompiler
// Type: Mafi.Core.PathFinding.VehiclePathFindingParams
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.PathFinding
{
  [GenerateSerializer(false, null, 0)]
  public class VehiclePathFindingParams
  {
    public static readonly VehiclePathFindingParams DEFAULT;
    public readonly RelTile1i RequiredClearance;
    public readonly SteepnessPathability SteepnessPathability;
    public readonly HeightClearancePathability HeightClearancePathability;
    /// <summary>
    /// Controls how sensitive is the vehicle for material penalties. For example, excavator with track is not very
    /// sensitive a should have this value below 100%.
    /// </summary>
    public readonly Percent MaterialTraversalSensitivity;
    public readonly ulong PathabilityQueryMask;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public VehiclePathFindingParams(
      RelTile1i requiredClearance,
      SteepnessPathability steepnessPathability,
      HeightClearancePathability heightClearancePathability,
      Percent materialTraversalSensitivity)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.RequiredClearance = requiredClearance;
      this.HeightClearancePathability = heightClearancePathability;
      this.MaterialTraversalSensitivity = materialTraversalSensitivity;
      this.SteepnessPathability = steepnessPathability;
      this.PathabilityQueryMask = ClearancePathabilityProvider.GetPathabilityQueryMask(this.RequiredClearance, this.SteepnessPathability, this.HeightClearancePathability);
    }

    public static Tile2i ConvertToCornerTileSpace(Tile2i coord, RelTile1i clearance)
    {
      return coord.AddXy(-clearance.Value / 2);
    }

    /// <summary>
    /// Converts the given center-based coord to corner-based space where clearance is a square towards +XY, not
    /// around a point.
    /// </summary>
    public Tile2i ConvertToCornerTileSpace(Tile2i coord)
    {
      return VehiclePathFindingParams.ConvertToCornerTileSpace(coord, this.RequiredClearance);
    }

    public Tile2i ConvertToCornerTileSpace(Tile2f coord)
    {
      return coord.AddXy(-this.RequiredClearance.Value.Over(2)).Tile2i;
    }

    /// <summary>
    /// Converts corner-based tile space to center-based coords.
    /// </summary>
    public Tile2f ConvertToCenterTileSpace(Tile2i coord)
    {
      return coord.CornerTile2f.AddXy(this.RequiredClearance.Value.Over(2));
    }

    public Tile2i ConvertToCenterTileSpace2i(Tile2i coord)
    {
      return coord.AddXy(this.RequiredClearance.Value / 2);
    }

    public Tile2i RoundCenterSpace(Tile2f coord)
    {
      return coord.AddXy(-this.RequiredClearance.Value.Over(2) % Fix32.One).Tile2iRounded;
    }

    public static void Serialize(VehiclePathFindingParams value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<VehiclePathFindingParams>(value))
        return;
      writer.EnqueueDataSerialization((object) value, VehiclePathFindingParams.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteUInt((uint) this.HeightClearancePathability);
      Percent.Serialize(this.MaterialTraversalSensitivity, writer);
      writer.WriteULong(this.PathabilityQueryMask);
      RelTile1i.Serialize(this.RequiredClearance, writer);
      writer.WriteUInt((uint) this.SteepnessPathability);
    }

    public static VehiclePathFindingParams Deserialize(BlobReader reader)
    {
      VehiclePathFindingParams pathFindingParams;
      if (reader.TryStartClassDeserialization<VehiclePathFindingParams>(out pathFindingParams))
        reader.EnqueueDataDeserialization((object) pathFindingParams, VehiclePathFindingParams.s_deserializeDataDelayedAction);
      return pathFindingParams;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<VehiclePathFindingParams>(this, "HeightClearancePathability", (object) (HeightClearancePathability) reader.ReadUInt());
      reader.SetField<VehiclePathFindingParams>(this, "MaterialTraversalSensitivity", (object) Percent.Deserialize(reader));
      reader.SetField<VehiclePathFindingParams>(this, "PathabilityQueryMask", (object) reader.ReadULong());
      reader.SetField<VehiclePathFindingParams>(this, "RequiredClearance", (object) RelTile1i.Deserialize(reader));
      reader.SetField<VehiclePathFindingParams>(this, "SteepnessPathability", (object) (SteepnessPathability) reader.ReadUInt());
    }

    static VehiclePathFindingParams()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      VehiclePathFindingParams.DEFAULT = new VehiclePathFindingParams(RelTile1i.One, SteepnessPathability.SlightSlopeAllowed, HeightClearancePathability.NoPassingUnder, Percent.Hundred);
      VehiclePathFindingParams.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((VehiclePathFindingParams) obj).SerializeData(writer));
      VehiclePathFindingParams.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((VehiclePathFindingParams) obj).DeserializeData(reader));
    }
  }
}
