// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Transform90RotFlip
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
  [GenerateSerializer(false, null, 0)]
  public readonly struct Transform90RotFlip : IEquatable<Transform90RotFlip>
  {
    public const int TOTAL_VALUES_COUNT = 8;
    /// <summary>
    /// Raw value in range 0-7 (three bits) where the two lower bits are rotation and third bit is reflection.
    /// </summary>
    public readonly byte RawValue;

    public static void Serialize(Transform90RotFlip value, BlobWriter writer)
    {
      writer.WriteByte(value.RawValue);
    }

    public static Transform90RotFlip Deserialize(BlobReader reader)
    {
      return new Transform90RotFlip(reader.ReadByte());
    }

    public Rotation90 Rotation90 => new Rotation90((int) this.RawValue & 3);

    public bool IsFlipped => ((uint) this.RawValue & 4U) > 0U;

    [LoadCtor]
    public Transform90RotFlip(byte rawValue)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.RawValue = rawValue;
    }

    public Transform90RotFlip(Rotation90 rotation, bool isFlipped)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.RawValue = (byte) (rotation.AngleIndex & 3 | (isFlipped ? 4 : 0));
    }

    public bool Equals(Transform90RotFlip other) => (int) this.RawValue == (int) other.RawValue;

    public override bool Equals(object obj)
    {
      return obj is Transform90RotFlip other && this.Equals(other);
    }

    public override int GetHashCode() => (int) this.RawValue;
  }
}
