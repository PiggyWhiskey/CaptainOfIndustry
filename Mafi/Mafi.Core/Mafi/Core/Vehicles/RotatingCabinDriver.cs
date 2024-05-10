// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.RotatingCabinDriver
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Dynamic;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Vehicles
{
  [GenerateSerializer(false, null, 0)]
  public class RotatingCabinDriver
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    /// <summary>The entity owning the cabin.</summary>
    private readonly DynamicGroundEntity m_entity;
    private readonly SmoothDriver m_cabinRotationDriver;

    public static void Serialize(RotatingCabinDriver value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<RotatingCabinDriver>(value))
        return;
      writer.EnqueueDataSerialization((object) value, RotatingCabinDriver.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      AngleDegrees1f.Serialize(this.CabinDirectionRelative, writer);
      writer.WriteNullableStruct<Tile2f>(this.CabinTarget);
      SmoothDriver.Serialize(this.m_cabinRotationDriver, writer);
      writer.WriteGeneric<DynamicGroundEntity>(this.m_entity);
    }

    public static RotatingCabinDriver Deserialize(BlobReader reader)
    {
      RotatingCabinDriver rotatingCabinDriver;
      if (reader.TryStartClassDeserialization<RotatingCabinDriver>(out rotatingCabinDriver))
        reader.EnqueueDataDeserialization((object) rotatingCabinDriver, RotatingCabinDriver.s_deserializeDataDelayedAction);
      return rotatingCabinDriver;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.CabinDirectionRelative = AngleDegrees1f.Deserialize(reader);
      this.CabinTarget = reader.ReadNullableStruct<Tile2f>();
      reader.SetField<RotatingCabinDriver>(this, "m_cabinRotationDriver", (object) SmoothDriver.Deserialize(reader));
      reader.SetField<RotatingCabinDriver>(this, "m_entity", (object) reader.ReadGenericAs<DynamicGroundEntity>());
    }

    /// <summary>Absolute direction of the cabin.</summary>
    public AngleDegrees1f CabinDirection => this.CabinDirectionRelative + this.m_entity.Direction;

    /// <summary>
    /// Direction of the cabin relative to the body of its entity.
    /// </summary>
    public AngleDegrees1f CabinDirectionRelative { get; private set; }

    /// <summary>
    /// Whether the cabin has reached requested target and is at rest.
    /// </summary>
    public bool IsCabinAtTarget
    {
      get
      {
        AngleDegrees1f angleDegrees1f = this.GetCabinDelta();
        angleDegrees1f = angleDegrees1f.Normalized;
        return angleDegrees1f.Abs < AngleDegrees1f.OneDegree;
      }
    }

    /// <summary>Target location that cabin should point to.</summary>
    public Tile2f? CabinTarget { get; private set; }

    public RotatingCabinDriver(RotatingCabinDriverProto proto, DynamicGroundEntity entity)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_cabinRotationDriver = new SmoothDriver(proto.MaxSpeedPerTick.Degrees, proto.MaxSpeedPerTick.Degrees, proto.MaxAccelerationPerTick.Degrees, proto.MaxBrakingPerTick.Degrees, proto.BrakingConservativeness);
      this.m_entity = entity;
    }

    public void SetSpeedFactor(Percent speedFactor)
    {
      this.m_cabinRotationDriver.SetSpeedFactor(speedFactor);
    }

    /// <summary>
    /// Updates rotation of the cabin. To be called every simulation update.
    /// </summary>
    public void Update()
    {
      AngleDegrees1f normalized = this.GetCabinDelta().Normalized;
      if (normalized < AngleDegrees1f.OneDegree || normalized > 359.Degrees())
      {
        this.CabinDirectionRelative += normalized;
        this.m_cabinRotationDriver.Reset();
      }
      else
      {
        if (normalized > AngleDegrees1f.Deg180)
          normalized -= AngleDegrees1f.Deg360;
        this.m_cabinRotationDriver.KeepSpeed(normalized.Degrees, normalized.Degrees);
        this.CabinDirectionRelative = (this.CabinDirectionRelative + this.m_cabinRotationDriver.Speed.Degrees()).Normalized;
      }
    }

    public void SetCabinTarget(Tile2f cabinTarget) => this.CabinTarget = new Tile2f?(cabinTarget);

    public void ResetCabinTarget() => this.CabinTarget = new Tile2f?();

    internal void SetAbsoluteCabinDirection(AngleDegrees1f angle)
    {
      this.CabinDirectionRelative = angle - this.m_entity.Direction;
    }

    public void Reset()
    {
      this.CabinDirectionRelative = AngleDegrees1f.Zero;
      this.CabinTarget = new Tile2f?();
      this.m_cabinRotationDriver.Reset();
    }

    public AngleDegrees1f GetCabinDelta()
    {
      AngleDegrees1f angleDegrees1f;
      if (!this.CabinTarget.HasValue)
      {
        angleDegrees1f = AngleDegrees1f.Zero;
      }
      else
      {
        if (this.CabinTarget.Value.IsNear(this.m_entity.Position2f, Fix32.Epsilon))
          return AngleDegrees1f.Zero;
        angleDegrees1f = (this.CabinTarget.Value - this.m_entity.Position2f).Angle - this.m_entity.Direction;
      }
      return angleDegrees1f - this.CabinDirectionRelative;
    }

    static RotatingCabinDriver()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      RotatingCabinDriver.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((RotatingCabinDriver) obj).SerializeData(writer));
      RotatingCabinDriver.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((RotatingCabinDriver) obj).DeserializeData(reader));
    }
  }
}
