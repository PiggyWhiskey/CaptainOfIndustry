// Decompiled with JetBrains decompiler
// Type: Mafi.Core.TileTransform
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core
{
  /// <summary>
  /// Immutable representation of transformation in tile space. Represents translation, rotation, and reflection. The
  /// order is: reflect, rotate, translate. This class also contain some pre-computed convenience members to help with
  /// application of this transformation.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public struct TileTransform : IEquatable<TileTransform>
  {
    public static readonly TileTransform Identity;
    /// <summary>Absolute global tile coordinates.</summary>
    [OnlyForSaveCompatibility(null)]
    public readonly Tile3i Position;
    /// <summary>Rotation around the origin.</summary>
    [OnlyForSaveCompatibility(null)]
    public readonly Rotation90 Rotation;
    /// <summary>
    /// Reflection around x axis. Reflection is applied before rotation. Reflection around y axis is not needed, the
    /// same effect can be achieved by rotation and x reflection.
    /// </summary>
    public readonly bool IsReflected;
    [DoNotSave(0, null)]
    private Matrix2i? m_transformMatrix;

    public static void Serialize(TileTransform value, BlobWriter writer)
    {
      Tile3i.Serialize(value.Position, writer);
      Rotation90.Serialize(value.Rotation, writer);
      writer.WriteBool(value.IsReflected);
    }

    public static TileTransform Deserialize(BlobReader reader)
    {
      return new TileTransform(Tile3i.Deserialize(reader), Rotation90.Deserialize(reader), reader.ReadBool());
    }

    public readonly Transform90RotFlip Transform90RotFlip
    {
      get => new Transform90RotFlip(this.Rotation, this.IsReflected);
    }

    /// <summary>
    /// Transform matrix that can be used to apply rotation and reflection of this transform around (0, 0). It does
    /// not apply any translation. This matrix is pre-computed for efficient transformations.
    /// </summary>
    public Matrix2i TransformMatrix
    {
      get
      {
        if (!this.m_transformMatrix.HasValue)
          this.m_transformMatrix = new Matrix2i?(Matrix2i.FromRotationFlip(this.Rotation, this.IsReflected));
        return this.m_transformMatrix.Value;
      }
    }

    [LoadCtor]
    public TileTransform(Tile3i position, Rotation90 rotation = default (Rotation90), bool isReflected = false)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Position = position;
      this.Rotation = rotation;
      this.IsReflected = isReflected;
      this.m_transformMatrix = new Matrix2i?();
    }

    /// <summary>
    /// Transforms given rotation according to this transformation.
    /// </summary>
    public readonly Rotation90 Transform(Rotation90 rotation)
    {
      return rotation + this.Rotation + (rotation.Dx == 0 || !this.IsReflected ? Rotation90.Deg0 : Rotation90.Deg180);
    }

    /// <summary>
    /// Transforms given rotation according to this transformation.
    /// </summary>
    public readonly Direction90 Transform(Direction90 direction)
    {
      return direction + this.Rotation + (direction.DirectionVector.X == 0 || !this.IsReflected ? Rotation90.Deg0 : Rotation90.Deg180);
    }

    /// <summary>
    /// Transforms given angle according to this transformation.
    /// </summary>
    public readonly AngleDegrees1f Transform(AngleDegrees1f angle)
    {
      if (this.IsReflected)
        angle = AngleDegrees1f.Deg180 - angle;
      return (angle + this.Rotation.Angle).Normalized;
    }

    public bool Equals(TileTransform other) => this == other;

    public override bool Equals(object obj) => obj is TileTransform other && this.Equals(other);

    public override readonly int GetHashCode()
    {
      return (this.Position.GetHashCode() * 397 ^ this.Rotation.GetHashCode()) * 397 ^ this.IsReflected.GetHashCode();
    }

    public static bool operator ==(TileTransform lhs, TileTransform rhs)
    {
      return lhs.Position == rhs.Position && lhs.Rotation == rhs.Rotation && lhs.IsReflected == rhs.IsReflected;
    }

    public static bool operator !=(TileTransform lhs, TileTransform rhs) => !(lhs == rhs);

    public override readonly string ToString()
    {
      return string.Format("{0} {1} {2}", (object) this.Position, (object) this.Rotation, this.IsReflected ? (object) "reflected" : (object) "");
    }

    [MustUseReturnValue]
    public readonly TileTransform OffsetBy(RelTile3i offset)
    {
      return new TileTransform(this.Position + offset, this.Rotation, this.IsReflected);
    }

    [MustUseReturnValue]
    public readonly TileTransform RotateBy(Rotation90 rotation)
    {
      return new TileTransform(this.Position, this.Rotation + rotation, this.IsReflected);
    }

    static TileTransform()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TileTransform.Identity = new TileTransform(Tile3i.Zero, Rotation90.Deg0);
    }
  }
}
