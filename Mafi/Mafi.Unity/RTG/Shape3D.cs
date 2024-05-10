// Decompiled with JetBrains decompiler
// Type: RTG.Shape3D
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public abstract class Shape3D
  {
    public bool Raycast(Ray ray) => this.Raycast(ray, out float _);

    public bool RaycastWire(Ray ray) => this.RaycastWire(ray, out float _);

    public virtual bool RaycastWire(Ray ray, out float t) => this.Raycast(ray, out t);

    public abstract void RenderSolid();

    public abstract void RenderWire();

    public abstract bool Raycast(Ray ray, out float t);

    public abstract AABB GetAABB();

    protected Shape3D()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
