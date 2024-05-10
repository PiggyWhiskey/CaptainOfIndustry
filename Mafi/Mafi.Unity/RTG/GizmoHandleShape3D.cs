// Decompiled with JetBrains decompiler
// Type: RTG.GizmoHandleShape3D
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class GizmoHandleShape3D
  {
    private bool _isVisible;
    private bool _isHoverable;
    private Shape3D _shape;

    public Shape3D Shape => this._shape;

    public bool IsVisible
    {
      get => this._isVisible;
      set => this._isVisible = value;
    }

    public bool IsHoverable
    {
      get => this._isHoverable;
      set => this._isHoverable = value;
    }

    public GizmoHandleShape3D(Shape3D shape)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._isVisible = true;
      this._isHoverable = true;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._shape = shape;
    }
  }
}
