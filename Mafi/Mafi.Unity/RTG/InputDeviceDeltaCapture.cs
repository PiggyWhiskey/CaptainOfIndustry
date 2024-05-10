// Decompiled with JetBrains decompiler
// Type: RTG.InputDeviceDeltaCapture
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class InputDeviceDeltaCapture
  {
    private int _id;
    private Vector3 _origin;
    private Vector3 _delta;

    public int Id => this._id;

    public Vector3 Origin => this._origin;

    public Vector3 Delta => this._delta;

    public InputDeviceDeltaCapture(int id, Vector3 origin)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._id = id;
      this._origin = origin;
    }

    public void Update(Vector3 devicePosition) => this._delta = devicePosition - this._origin;
  }
}
