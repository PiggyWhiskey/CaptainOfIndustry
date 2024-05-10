// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Style.ResearchWindowUiStyle
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UiFramework.Components;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UserInterface.Style
{
  /// <summary>Styles used for the research window.</summary>
  public class ResearchWindowUiStyle : UnlockingTreeWindowUiStyle
  {
    public ResearchWindowUiStyle(IconsPaths icons, GlobalUiStyle global)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(icons, global);
    }

    public virtual ColorRgba[] GetTitleTextClrs()
    {
      ColorRgba[] titleTextClrs = new ColorRgba[5];
      titleTextClrs[1] = (ColorRgba) 16777215;
      titleTextClrs[2] = (ColorRgba) 15527148;
      titleTextClrs[0] = (ColorRgba) 15527148;
      return titleTextClrs;
    }

    public virtual ColorRgba[] GetTitleBgClrs()
    {
      ColorRgba[] titleBgClrs = new ColorRgba[5];
      titleBgClrs[1] = (ColorRgba) 6710372;
      titleBgClrs[2] = (ColorRgba) 8881423;
      titleBgClrs[0] = (ColorRgba) 5472841;
      return titleBgClrs;
    }

    public virtual ColorRgba NodeBlockedTitleClr => (ColorRgba) 15527148;

    public virtual ColorRgba NodeBlockedTitleBgClr => (ColorRgba) 12152320;

    public virtual ColorRgba NodeInQueueTitleClr => (ColorRgba) 15527148;

    public virtual ColorRgba NodeInQueueTitleBgClr => (ColorRgba) 8881423;

    public virtual ColorRgba NodeCanBeQueuedTitleClr => (ColorRgba) 15527148;

    public virtual ColorRgba NodeCanBeQueuedTitleBgClr => (ColorRgba) 5472841;

    public virtual ColorRgba NodeCanNotBeQueuedTitleClr => (ColorRgba) 15527148;

    public virtual ColorRgba NodeCanNotBeQueuedTitleBgClr => (ColorRgba) 7750724;

    public virtual ColorRgba NodeLockedByConditionTitleClr => this.NodeTitleText.Color;

    public virtual ColorRgba NodeLockedByConditionTitleBgClr => (ColorRgba) 7750724;

    public virtual ColorRgba NodeLockedByParentsTitleClr => this.NodeTitleText.Color;

    public virtual ColorRgba NodeLockedByParentsTitleBgClr => (ColorRgba) 4807530;

    public override int DefaultSidePanelWidth => 300;

    public virtual TextStyle NodeDetailTitleText
    {
      get
      {
        TextStyle title = this.Global.Title;
        ref TextStyle local = ref title;
        int? nullable = new int?(14);
        ColorRgba? color = new ColorRgba?();
        FontStyle? fontStyle = new FontStyle?();
        int? fontSize = nullable;
        bool? isCapitalized = new bool?();
        return local.Extend(color, fontStyle, fontSize, isCapitalized);
      }
    }

    public virtual TextStyle NodeDetailDescText
    {
      get
      {
        TextStyle text = this.Global.Text;
        ref TextStyle local = ref text;
        int? nullable = new int?(14);
        ColorRgba? color = new ColorRgba?();
        FontStyle? fontStyle = new FontStyle?();
        int? fontSize = nullable;
        bool? isCapitalized = new bool?();
        return local.Extend(color, fontStyle, fontSize, isCapitalized);
      }
    }

    public virtual TextStyle NodeDetailProgressText
    {
      get => this.Global.Title.Extend(new ColorRgba?(this.Global.GreenForDark));
    }

    public virtual TextStyle NodeDetailBlockedText
    {
      get => this.Global.Title.Extend(new ColorRgba?((ColorRgba) 12152320));
    }

    public virtual IconStyle NodeDetailBlockedIcon
    {
      get
      {
        return new IconStyle(this.Icons.Warning, new ColorRgba?((ColorRgba) 12152320), new Vector2?(new Vector2(16f, 16f)));
      }
    }

    public virtual IconStyle NodeDetailLockedIcon
    {
      get
      {
        return new IconStyle(this.Icons.Lock, new ColorRgba?((ColorRgba) 6710886), new Vector2?(new Vector2(14f, 14f)));
      }
    }

    public virtual TextStyle NodeDetailLockedText
    {
      get => this.Global.Text.Extend(new ColorRgba?((ColorRgba) 6710886));
    }

    public virtual IconStyle NodeDetailFinishedIcon
    {
      get
      {
        return new IconStyle(this.Icons.Tick, new ColorRgba?((ColorRgba) 6710886), new Vector2?(new Vector2(14f, 14f)));
      }
    }

    public virtual TextStyle NodeDetailFinishedText
    {
      get => this.Global.Title.Extend(new ColorRgba?((ColorRgba) 6710886));
    }

    public virtual int SectionTitleHeight => 20;

    public virtual int NodeDetailUnlockStripeHeight => 50;

    public virtual Vector2 NodeDetailUnlockIconSize => new Vector2(46f, 46f);

    public virtual ColorRgba NodeDetailUnlockBgStripe => (ColorRgba) 2699047;

    public virtual ColorRgba NodeDetailUnlockBgStripeRed => (ColorRgba) 4332311;

    public virtual int ProductSize => 60;

    public virtual TextStyle NoResearchText
    {
      get
      {
        return new TextStyle()
        {
          Color = (ColorRgba) 9605778,
          FontSize = 14,
          FontStyle = FontStyle.Bold
        };
      }
    }

    public virtual IconStyle NoResearchIcon
    {
      get
      {
        return new IconStyle("Assets/Unity/UserInterface/Toolbar/Research.svg", new ColorRgba?((ColorRgba) 9605778), new Vector2?(new Vector2(100f, 100f)));
      }
    }
  }
}
