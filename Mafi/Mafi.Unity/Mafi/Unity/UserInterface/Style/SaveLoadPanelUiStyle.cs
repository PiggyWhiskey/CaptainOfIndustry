// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Style.SaveLoadPanelUiStyle
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UserInterface.Style
{
  /// <summary>Styles for save and load panel.</summary>
  public class SaveLoadPanelUiStyle : BaseUiStyle
  {
    public SaveLoadPanelUiStyle(IconsPaths icons, GlobalUiStyle global)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(icons, global);
    }

    public virtual int WindowWidth_Legacy => 650;

    public virtual int WindowWidth => 1000;

    public virtual int WindowHeight => 650;

    /// <summary>
    /// Width of the column containing name of the file in the overview table of existing save files.
    /// </summary>
    public virtual int SaveNameColumnWidth => 280;

    /// <summary>
    /// Width of the column containing last modification date of the file in the overview table of existing save
    /// files.
    /// </summary>
    public virtual int ModifiedColumnWidth => 200;

    public virtual int SizeColumnWidth => 100;

    public virtual int TableRowHeight => 30;

    public virtual ColorRgba BottomBtnsHolderBg => this.Global.ControlsBgColor;

    public virtual int BottomBtnsHolderHeight => 50;

    public virtual Vector2 InputFieldSize => new Vector2(250f, 30f);
  }
}
