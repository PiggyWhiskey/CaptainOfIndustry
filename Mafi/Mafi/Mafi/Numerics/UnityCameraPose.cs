// Decompiled with JetBrains decompiler
// Type: Mafi.Numerics.UnityCameraPose
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Numerics
{
  [GenerateSerializer(false, null, 0)]
  public readonly struct UnityCameraPose
  {
    public readonly float PositionX;
    public readonly float PositionY;
    public readonly float PositionZ;
    public readonly float RotationX;
    public readonly float RotationY;
    public readonly float RotationZ;
    public readonly float RotationW;
    public readonly float VerticalFieldOfView;

    public static void Serialize(UnityCameraPose value, BlobWriter writer)
    {
      writer.WriteFloat(value.PositionX);
      writer.WriteFloat(value.PositionY);
      writer.WriteFloat(value.PositionZ);
      writer.WriteFloat(value.RotationX);
      writer.WriteFloat(value.RotationY);
      writer.WriteFloat(value.RotationZ);
      writer.WriteFloat(value.RotationW);
      writer.WriteFloat(value.VerticalFieldOfView);
    }

    public static UnityCameraPose Deserialize(BlobReader reader)
    {
      return new UnityCameraPose(reader.ReadFloat(), reader.ReadFloat(), reader.ReadFloat(), reader.ReadFloat(), reader.ReadFloat(), reader.ReadFloat(), reader.ReadFloat(), reader.ReadFloat());
    }

    public UnityCameraPose(
      float positionX,
      float positionY,
      float positionZ,
      float rotationX,
      float rotationY,
      float rotationZ,
      float rotationW,
      float verticalFieldOfView)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.PositionX = positionX;
      this.PositionY = positionY;
      this.PositionZ = positionZ;
      this.RotationX = rotationX;
      this.RotationY = rotationY;
      this.RotationZ = rotationZ;
      this.RotationW = rotationW;
      this.VerticalFieldOfView = verticalFieldOfView;
    }
  }
}
