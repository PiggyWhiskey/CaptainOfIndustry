// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.EncodedImageAndMatrix
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Numerics;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Terrain.Generation
{
  [GenerateSerializer(false, null, 0)]
  public readonly struct EncodedImageAndMatrix
  {
    public readonly byte[] ImageData;
    [NewInSaveVersion(145, null, null, null, null)]
    public readonly UnityCameraPose? CameraPose;
    public readonly UnityMatrix4? ViewProjectionMatrix;
    [NewInSaveVersion(154, null, "0.00012f", null, null)]
    public readonly float FogDensity;

    public static void Serialize(EncodedImageAndMatrix value, BlobWriter writer)
    {
      writer.WriteArray<byte>(value.ImageData);
      writer.WriteNullableStruct<UnityCameraPose>(value.CameraPose);
      writer.WriteNullableStruct<UnityMatrix4>(value.ViewProjectionMatrix);
      writer.WriteFloat(value.FogDensity);
    }

    public static EncodedImageAndMatrix Deserialize(BlobReader reader)
    {
      return new EncodedImageAndMatrix(reader.ReadArray<byte>(), reader.LoadedSaveVersion >= 145 ? reader.ReadNullableStruct<UnityCameraPose>() : new UnityCameraPose?(), reader.ReadNullableStruct<UnityMatrix4>(), reader.LoadedSaveVersion >= 154 ? reader.ReadFloat() : 0.00012f);
    }

    public EncodedImageAndMatrix(
      byte[] imageData,
      UnityCameraPose? cameraPose,
      UnityMatrix4? viewProjectionMatrix,
      float fogDensity)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.ImageData = imageData;
      this.CameraPose = cameraPose;
      this.ViewProjectionMatrix = viewProjectionMatrix;
      this.FogDensity = fogDensity;
    }
  }
}
