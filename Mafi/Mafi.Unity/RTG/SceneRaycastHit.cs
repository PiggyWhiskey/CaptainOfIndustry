// Decompiled with JetBrains decompiler
// Type: RTG.SceneRaycastHit
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class SceneRaycastHit
  {
    private GameObjectRayHit _objectHit;
    private XZGridRayHit _gridHit;

    public bool WasAnythingHit => this._objectHit != null || this._gridHit != null;

    public bool WasAnObjectHit => this._objectHit != null;

    public bool WasGridHit => this._gridHit != null;

    public GameObjectRayHit ObjectHit => this._objectHit;

    public XZGridRayHit GridHit => this._gridHit;

    public SceneRaycastHit(GameObjectRayHit objectRayHit, XZGridRayHit gridRayHit)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._objectHit = objectRayHit;
      this._gridHit = gridRayHit;
    }
  }
}
