// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Style.UnlockingTreeWindowUiStyle
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UserInterface.Style
{
  /// <summary>Style used in UnlockingTreeWindowView and its nodes.</summary>
  public class UnlockingTreeWindowUiStyle : BaseUiStyle
  {
    public UnlockingTreeWindowUiStyle(IconsPaths icons, GlobalUiStyle global)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(icons, global);
    }

    public virtual Vector2i TreePadding => new Vector2i(100, 100);

    public virtual ColorRgba WindowBg => (ColorRgba) 3815994;

    public virtual Vector2 ArrowSize => new Vector2(18f, 18f);

    public virtual int LineWidth => 32;

    public virtual int CornerSize => 32;

    public virtual int LineLeftOffset => 2;

    public virtual ColorRgba LineColor => (ColorRgba) 5066061;

    public virtual ColorRgba LineColorHighlight => (ColorRgba) 10128456;

    public virtual ColorRgba NodeUnlockIconBg => (ColorRgba) 3289650;

    public virtual Vector2i NodeSize => new Vector2i(260, 96);

    public virtual int NodeSelectionThickness => 9;

    public virtual int NodeBarHeight => 26;

    public virtual int NodeUnlockBoxSize => 58;

    public virtual int NodeUnlockBoxHorizontalOffset => 4;

    public virtual int NodeUnlockBoxVerticalOffset => 6;

    public virtual ColorRgba NodeBarBg => this.Global.ControlsBgColor;

    public virtual BtnStyle NodeMainStyle
    {
      get
      {
        return new BtnStyle()
        {
          BackgroundClr = new ColorRgba?((ColorRgba) 3947580),
          Shadow = true,
          HoveredMaskClr = new ColorRgba?((ColorRgba) 15856113),
          PressedMaskClr = new ColorRgba?((ColorRgba) 16777215)
        };
      }
    }

    public virtual ColorRgba NodeProgressBarBg => (ColorRgba) 4028429;

    public virtual ColorRgba NodeBackground => new ColorRgba(3947580);

    public virtual ColorRgba NodeShadow => new ColorRgba(0, 110);

    public virtual ColorRgba NodeSelectedGlowClr => new ColorRgba(13813823, 35);

    public virtual ColorRgba NodeHoveredGlowClr => new ColorRgba(13813823, 16);

    public virtual ColorRgba NodeHighlightedGlowClr => new ColorRgba(11447982, 75);

    public virtual ColorRgba NodeSelectedBorderClr => new ColorRgba(7435805);

    public virtual ColorRgba NodeHoveredBorderClr => new ColorRgba(7435805, 120);

    public virtual TextStyle NodeTitleText => this.Global.Title;

    public virtual IconStyle NodeLockedIcon
    {
      get
      {
        return new IconStyle(this.Icons.Lock, new ColorRgba?((ColorRgba) 12237498), new Vector2?(new Vector2(14f, 14f)));
      }
    }

    public virtual IconStyle NodeDoneIcon
    {
      get
      {
        return new IconStyle(this.Icons.Tick, new ColorRgba?((ColorRgba) 15527148), new Vector2?(new Vector2(16f, 16f)));
      }
    }

    public virtual IconStyle NodeInProgressIcon
    {
      get
      {
        return new IconStyle(this.Icons.Working, new ColorRgba?((ColorRgba) 15527148), new Vector2?(new Vector2(18f, 18f)));
      }
    }

    public virtual IconStyle NodeBlockedIcon
    {
      get
      {
        return new IconStyle(this.Icons.Warning, new ColorRgba?((ColorRgba) 15527148), new Vector2?(new Vector2(16f, 16f)));
      }
    }

    public virtual IconStyle NodeQueuedIcon
    {
      get
      {
        return new IconStyle("Assets/Unity/UserInterface/General/Plus.svg", new ColorRgba?((ColorRgba) 15527148), new Vector2?(new Vector2(16f, 16f)));
      }
    }

    public virtual Offset NodeRightIconOffset => Offset.Right(6f);

    public virtual int DefaultSidePanelWidth => 0;

    public virtual ColorRgba SidePanelLeftDividerClr => (ColorRgba) 2696228;
  }
}
