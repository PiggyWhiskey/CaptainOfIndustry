// Decompiled with JetBrains decompiler
// Type: RTG.Light3DSnapshot
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class Light3DSnapshot
  {
    private Light _light;
    private Vector3 _position;
    private Quaternion _rotation;
    private float _range;
    private float _spotAngle;

    public Light3DSnapshot()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public Light3DSnapshot(Light3DSnapshot src)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._light = src._light;
      this._position = src._position;
      this._rotation = src._rotation;
      this._range = src._range;
      this._spotAngle = src._spotAngle;
    }

    public void Snapshot(Light light)
    {
      if ((Object) light == (Object) null)
        return;
      this._light = light;
      this._position = light.transform.position;
      this._rotation = light.transform.rotation;
      this._range = light.range;
      this._spotAngle = light.spotAngle;
    }

    public void Apply()
    {
      if ((Object) this._light == (Object) null)
        return;
      this._light.transform.position = this._position;
      this._light.transform.rotation = this._rotation;
      this._light.range = this._range;
      this._light.spotAngle = this._spotAngle;
    }
  }
}
