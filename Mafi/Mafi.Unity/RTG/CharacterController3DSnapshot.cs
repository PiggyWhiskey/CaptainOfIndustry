// Decompiled with JetBrains decompiler
// Type: RTG.CharacterController3DSnapshot
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class CharacterController3DSnapshot
  {
    private CharacterController _characterController;
    private Vector3 _localCenter;
    private float _localRadius;
    private float _localHeight;

    public CharacterController3DSnapshot()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public CharacterController3DSnapshot(CharacterController3DSnapshot src)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._characterController = src._characterController;
      this._localCenter = src._localCenter;
      this._localRadius = src._localRadius;
      this._localHeight = src._localHeight;
    }

    public void Snapshot(CharacterController characterController)
    {
      if ((Object) characterController == (Object) null)
        return;
      this._characterController = characterController;
      this._localCenter = characterController.center;
      this._localRadius = characterController.radius;
      this._localHeight = characterController.height;
    }

    public void Apply()
    {
      if ((Object) this._characterController == (Object) null)
        return;
      this._characterController.center = this._localCenter;
      this._characterController.radius = this._localRadius;
      this._characterController.height = this._localHeight;
    }
  }
}
