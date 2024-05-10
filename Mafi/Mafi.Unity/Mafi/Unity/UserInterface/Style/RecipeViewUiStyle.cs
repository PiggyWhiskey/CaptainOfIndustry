// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Style.RecipeViewUiStyle
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UserInterface.Style
{
  /// <summary>Recipe view styles.</summary>
  public class RecipeViewUiStyle
  {
    public virtual Vector2 PlusIconSize => new Vector2(16f, 16f);

    public virtual Vector2 TransformIconSize => new Vector2(24f, 24f);

    public virtual ColorRgba ProgressBarBackground => new ColorRgba(32768, 40);

    public virtual float ItemWidth => 80f;

    public virtual float Height => 75f;

    public virtual float DurationTextLineHeight => 14f;

    public virtual float DurationTextWidth => 40f;

    public RecipeViewUiStyle()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
