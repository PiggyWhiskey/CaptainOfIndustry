// Decompiled with JetBrains decompiler
// Type: Mafi.Numerics.UnityMatrix4
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Numerics
{
  /// <summary>
  /// Serializable representation of Unity matrix (column-major).
  /// </summary>
  /// <remarks>Interactions with Unity types are written as extensions in the Unity project.</remarks>
  [GenerateSerializer(false, null, 0)]
  public readonly struct UnityMatrix4
  {
    public readonly float M00;
    public readonly float M10;
    public readonly float M20;
    public readonly float M30;
    public readonly float M01;
    public readonly float M11;
    public readonly float M21;
    public readonly float M31;
    public readonly float M02;
    public readonly float M12;
    public readonly float M22;
    public readonly float M32;
    public readonly float M03;
    public readonly float M13;
    public readonly float M23;
    public readonly float M33;

    public static void Serialize(UnityMatrix4 value, BlobWriter writer)
    {
      writer.WriteFloat(value.M00);
      writer.WriteFloat(value.M10);
      writer.WriteFloat(value.M20);
      writer.WriteFloat(value.M30);
      writer.WriteFloat(value.M01);
      writer.WriteFloat(value.M11);
      writer.WriteFloat(value.M21);
      writer.WriteFloat(value.M31);
      writer.WriteFloat(value.M02);
      writer.WriteFloat(value.M12);
      writer.WriteFloat(value.M22);
      writer.WriteFloat(value.M32);
      writer.WriteFloat(value.M03);
      writer.WriteFloat(value.M13);
      writer.WriteFloat(value.M23);
      writer.WriteFloat(value.M33);
    }

    public static UnityMatrix4 Deserialize(BlobReader reader)
    {
      return new UnityMatrix4(reader.ReadFloat(), reader.ReadFloat(), reader.ReadFloat(), reader.ReadFloat(), reader.ReadFloat(), reader.ReadFloat(), reader.ReadFloat(), reader.ReadFloat(), reader.ReadFloat(), reader.ReadFloat(), reader.ReadFloat(), reader.ReadFloat(), reader.ReadFloat(), reader.ReadFloat(), reader.ReadFloat(), reader.ReadFloat());
    }

    public UnityMatrix4(
      float m00,
      float m10,
      float m20,
      float m30,
      float m01,
      float m11,
      float m21,
      float m31,
      float m02,
      float m12,
      float m22,
      float m32,
      float m03,
      float m13,
      float m23,
      float m33)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.M00 = m00;
      this.M10 = m10;
      this.M20 = m20;
      this.M30 = m30;
      this.M01 = m01;
      this.M11 = m11;
      this.M21 = m21;
      this.M31 = m31;
      this.M02 = m02;
      this.M12 = m12;
      this.M22 = m22;
      this.M32 = m32;
      this.M03 = m03;
      this.M13 = m13;
      this.M23 = m23;
      this.M33 = m33;
    }
  }
}
