// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Terrain.TerrainPropMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Terrain.Props;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Terrain
{
  /// <summary>
  /// Terrain Prop in MB form. Not to be used for anything substantial - please use TerrainPropsRenderer instead.
  /// This implementation exists only to serve a preview of a prop, currently only in the map editor.
  /// </summary>
  public class TerrainPropMb : MonoBehaviour
  {
    public void Initialize(AssetsDb db, TerrainPropData data)
    {
      Transform transform = this.transform;
      Fix32 degrees = data.RotationRoll.Degrees;
      double x = (double) degrees.ToFloat();
      degrees = data.RotationYaw.Degrees;
      double y = (double) degrees.ToFloat();
      degrees = data.RotationPitch.Degrees;
      double z = (double) degrees.ToFloat();
      Quaternion quaternion = Quaternion.Euler((float) x, (float) y, (float) z);
      transform.rotation = quaternion;
      this.transform.localScale = Vector3.one * data.Scale.ToFloat();
      this.transform.position = data.Position.ExtendHeight(data.PlacedAtHeight + data.PlacementHeightOffset).ToVector3();
      this.gameObject.GetComponent<MeshRenderer>().sharedMaterial = Object.Instantiate<Material>(db.GetSharedMaterial(data.Proto.Graphics.PreviewMatPath));
    }

    public TerrainPropMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
