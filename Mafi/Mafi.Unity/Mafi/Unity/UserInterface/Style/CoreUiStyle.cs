// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Style.CoreUiStyle
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UiFramework.Styles;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UserInterface.Style
{
  public class CoreUiStyle : BaseUiStyle
  {
    public CoreUiStyle(IconsPaths icons, GlobalUiStyle global)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(icons, global);
    }

    public virtual SlicedSpriteStyle TextFieldBgSprite => this.Icons.WhiteBgGrayBorder;

    public virtual ColorRgba TextFieldBgColor => (ColorRgba) 3223857;

    public virtual ColorRgba TextFieldBgColorDark => (ColorRgba) 2565927;
  }
}
