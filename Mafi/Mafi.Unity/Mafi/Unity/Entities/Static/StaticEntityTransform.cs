// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.Static.StaticEntityTransform
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities.Static
{
  public readonly struct StaticEntityTransform
  {
    public readonly Vector3 Position;
    public readonly Quaternion Rotation;
    public readonly Vector3 LocalScale;

    public StaticEntityTransform(Vector3 position, Quaternion rotation, Vector3 localScale)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.Position = position;
      this.Rotation = rotation;
      this.LocalScale = localScale;
    }

    public void Apply(Transform t)
    {
      t.localPosition = this.Position;
      t.localRotation = this.Rotation;
      t.localScale = this.LocalScale;
    }
  }
}
