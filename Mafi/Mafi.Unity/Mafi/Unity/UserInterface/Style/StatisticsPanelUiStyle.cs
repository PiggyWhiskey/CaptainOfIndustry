// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Style.StatisticsPanelUiStyle
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
  /// <summary>Styles for statistics.</summary>
  public class StatisticsPanelUiStyle : BaseUiStyle
  {
    public StatisticsPanelUiStyle(IconsPaths icons, GlobalUiStyle global)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(icons, global);
    }

    public virtual int WindowWidth => 820;

    public virtual int WindowHeight => 740;

    public virtual TextStyle HighligtedText
    {
      get
      {
        return new TextStyle()
        {
          FontSize = 12,
          FontStyle = FontStyle.Bold,
          IsCapitalized = true
        };
      }
    }

    public virtual ColorRgba PositiveMoneyColor => (ColorRgba) 4371254;

    public virtual ColorRgba NegativeMoneyColor => (ColorRgba) 16528955;

    public virtual TextStyle PositiveMoneyText
    {
      get
      {
        return new TextStyle()
        {
          Color = (ColorRgba) 4371254,
          FontSize = 12
        };
      }
    }

    public virtual TextStyle NegativeMoneyText
    {
      get
      {
        return new TextStyle()
        {
          Color = (ColorRgba) 16528955,
          FontSize = 12
        };
      }
    }

    public virtual ColorRgba ZeroMoneyColor => this.Global.DefaultPanelTextColor;

    public virtual TextStyle TableHeaderTextStyle
    {
      get
      {
        return new TextStyle()
        {
          Color = this.Global.DefaultPanelTextColor,
          FontStyle = FontStyle.Bold,
          FontSize = 12,
          IsCapitalized = true
        };
      }
    }

    public virtual ColorRgba TableDividersColor => new ColorRgba(6710886, 120);

    public virtual int PoductNameColumnWidth => 140;

    public virtual int QuantityColumnWidth => 100;

    public virtual int MoneyColumnWidth => 100;

    /// <summary>Icons for tab headers.</summary>
    public virtual IconStyle ProductsIcon
    {
      get
      {
        return new IconStyle("Assets/Unity/UserInterface/Toolbar/Stats.svg", new ColorRgba?(this.Global.DefaultPanelTextColor));
      }
    }

    public virtual IconStyle FoodIcon
    {
      get
      {
        return new IconStyle("Assets/Unity/UserInterface/General/Food.svg", new ColorRgba?(this.Global.DefaultPanelTextColor));
      }
    }

    public virtual IconStyle MaintenanceIcon
    {
      get
      {
        return new IconStyle("Assets/Unity/UserInterface/General/Working128.png", new ColorRgba?(this.Global.DefaultPanelTextColor));
      }
    }

    public virtual IconStyle HealthIcon
    {
      get
      {
        return new IconStyle("Assets/Unity/UserInterface/General/Health.svg", new ColorRgba?(this.Global.DefaultPanelTextColor));
      }
    }

    public virtual IconStyle PollutionIcon
    {
      get
      {
        return new IconStyle("Assets/Unity/UserInterface/General/Trash128.png", new ColorRgba?(this.Global.DefaultPanelTextColor));
      }
    }

    public virtual IconStyle PopulationIcon
    {
      get
      {
        return new IconStyle("Assets/Unity/UserInterface/General/PopulationSmall.svg", new ColorRgba?(this.Global.DefaultPanelTextColor));
      }
    }

    public virtual IconStyle UnityIcon
    {
      get
      {
        return new IconStyle("Assets/Unity/UserInterface/General/UnitySmall.svg", new ColorRgba?(this.Global.DefaultPanelTextColor));
      }
    }

    public virtual IconStyle PopDiffIcon
    {
      get
      {
        return new IconStyle("Assets/Unity/UserInterface/General/PopulationSmall.svg", new ColorRgba?(this.Global.DefaultPanelTextColor));
      }
    }

    public virtual IconStyle FuelIcon
    {
      get
      {
        return new IconStyle("Assets/Unity/UserInterface/Toolbar/Oil.svg", new ColorRgba?(this.Global.DefaultPanelTextColor));
      }
    }

    public virtual IconStyle BreakdownIcon
    {
      get
      {
        return new IconStyle("Assets/Unity/UserInterface/General/EditDescription.svg", new ColorRgba?(this.Global.DefaultPanelTextColor));
      }
    }

    public virtual IconStyle ChartIcon
    {
      get
      {
        return new IconStyle("Assets/Unity/UserInterface/Toolbar/Stats.svg", new ColorRgba?(this.Global.DefaultPanelTextColor));
      }
    }

    public virtual IconStyle GeneralSettingsIcon
    {
      get
      {
        return new IconStyle("Assets/Unity/UserInterface/General/Working128.png", new ColorRgba?(this.Global.DefaultPanelTextColor));
      }
    }

    public virtual IconStyle VideoSettingsIcon
    {
      get
      {
        return new IconStyle("Assets/Unity/UserInterface/General/Video.svg", new ColorRgba?(this.Global.DefaultPanelTextColor));
      }
    }

    public virtual IconStyle AudioSettingsIcon
    {
      get
      {
        return new IconStyle("Assets/Unity/UserInterface/General/Audio.svg", new ColorRgba?(this.Global.DefaultPanelTextColor));
      }
    }

    public virtual IconStyle ControlsSettingsIcon
    {
      get
      {
        return new IconStyle("Assets/Unity/UserInterface/General/RightClick128.png", new ColorRgba?(this.Global.DefaultPanelTextColor));
      }
    }
  }
}
