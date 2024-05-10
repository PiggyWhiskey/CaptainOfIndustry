// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MainMenu.NewGame.DifficultyTab
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UiToolkit;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MainMenu.NewGame
{
  public class DifficultyTab : Mafi.Unity.UiToolkit.Library.Column
  {
    private static readonly ColorRgba STRIPE_COLOR;
    private static readonly Percent WIDTH;
    private readonly NewGameConfigForUi m_settings;
    private readonly ButtonColumn[] m_bigButtons;
    private readonly ScrollColumn m_scrollContainer;
    private int m_rowIndex;

    public DifficultyTab(NewGameConfigForUi settings)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_settings = settings;
      this.AlignItemsCenter<DifficultyTab>().Fill<DifficultyTab>();
      UiComponent[] uiComponentArray = new UiComponent[2];
      Mafi.Unity.UiToolkit.Library.Row component1 = new Mafi.Unity.UiToolkit.Library.Row(3.pt());
      component1.Add<Mafi.Unity.UiToolkit.Library.Row>((Action<Mafi.Unity.UiToolkit.Library.Row>) (c => c.AlignItemsStretch<Mafi.Unity.UiToolkit.Library.Row>().Fill<Mafi.Unity.UiToolkit.Library.Row>().Width<Mafi.Unity.UiToolkit.Library.Row>(DifficultyTab.WIDTH).RelativePosition<Mafi.Unity.UiToolkit.Library.Row>()));
      component1.Add((IEnumerable<UiComponent>) (this.m_bigButtons = SelectDifficultyData.All.Select<ButtonColumn>((Func<SelectDifficultyData, int, ButtonColumn>) ((_, idx) =>
      {
        ButtonColumn component2 = new ButtonColumn((Action) (() => this.selectDifficulty(idx)), Outer.ShadowAll);
        component2.Add<ButtonColumn>((Action<ButtonColumn>) (c => c.AlignItemsStretch<ButtonColumn>().Width<ButtonColumn>((100 / SelectDifficultyData.All.Length).Percent()).Padding<ButtonColumn>(3.pt()).Variant<ButtonColumn>(ButtonVariant.Boxy)));
        return component2;
      })).ToArray<ButtonColumn>()));
      uiComponentArray[0] = (UiComponent) component1;
      ScrollColumn component3 = new ScrollColumn();
      component3.Add<ScrollColumn>((Action<ScrollColumn>) (c => c.AbsolutePositionFillParent<ScrollColumn>().AlignItemsCenter<ScrollColumn>().IgnoreInputPicking<ScrollColumn>(recursive: true).PreventResizeForScroller().PaddingTopBottom<ScrollBase>(1.px()).ScrollerAlwaysVisible()));
      component3.Add((UiComponent) this.row((Func<SelectDifficultyData, UiComponent>) (d => (UiComponent) new StretchedImg(d.ImageAsset)), stripe: false, customize: (Action<Mafi.Unity.UiToolkit.Library.Column>) (c => c.Padding<Mafi.Unity.UiToolkit.Library.Column>(Px.Zero).MarginLeftRight<Mafi.Unity.UiToolkit.Library.Column>(1.px()))));
      component3.Add((UiComponent) this.row((Func<SelectDifficultyData, UiComponent>) (d =>
      {
        Mafi.Unity.UiToolkit.Library.Column component4 = new Mafi.Unity.UiToolkit.Library.Column(Outer.Panel);
        component4.Add<Mafi.Unity.UiToolkit.Library.Column>((Action<Mafi.Unity.UiToolkit.Library.Column>) (c => c.Padding<Mafi.Unity.UiToolkit.Library.Column>(2.pt(), 3.pt(), 3.pt(), 3.pt()).AlignItemsCenter<Mafi.Unity.UiToolkit.Library.Column>().Gap<Mafi.Unity.UiToolkit.Library.Column>(new Px?(1.pt())).Fill<Mafi.Unity.UiToolkit.Library.Column>()));
        component4.Add((UiComponent) new Mafi.Unity.UiToolkit.Library.Label(d.Name).FontSize<Mafi.Unity.UiToolkit.Library.Label>(24).AlignTextCenter<Mafi.Unity.UiToolkit.Library.Label>().FontBold<Mafi.Unity.UiToolkit.Library.Label>().UpperCase().Color<Mafi.Unity.UiToolkit.Library.Label>(new ColorRgba?(Theme.PrimaryColor)).TextShadow<Mafi.Unity.UiToolkit.Library.Label>());
        component4.Add((UiComponent) new Mafi.Unity.UiToolkit.Library.Label(d.Description).AlignText<Mafi.Unity.UiToolkit.Library.Label>(TextAlign.CenterTop));
        return (UiComponent) component4;
      }), stripe: false, customize: (Action<Mafi.Unity.UiToolkit.Library.Column>) (c => c.Class<Mafi.Unity.UiToolkit.Library.Column>(Cls.panel).MarginTop<Mafi.Unity.UiToolkit.Library.Column>(-10.pt()).JustifyItemsStart<Mafi.Unity.UiToolkit.Library.Column>().PaddingBottom<Mafi.Unity.UiToolkit.Library.Column>(0.px()))));
      component3.Add((UiComponent) this.row((Func<SelectDifficultyData, UiComponent>) (d => (UiComponent) new Mafi.Unity.UiToolkit.Library.Label(d.Food)), "Assets/Unity/UserInterface/General/Food.svg", imageY: -4));
      component3.Add((UiComponent) this.row((Func<SelectDifficultyData, UiComponent>) (d => (UiComponent) new Mafi.Unity.UiToolkit.Library.Label(d.Construction)), "Assets/Base/Products/Icons/ConstructionParts1.svg"));
      component3.Add((UiComponent) this.row((Func<SelectDifficultyData, UiComponent>) (d => (UiComponent) new Mafi.Unity.UiToolkit.Library.Label(d.Fuel)), "Assets/Unity/UserInterface/General/FuelTank128.png"));
      component3.Add((UiComponent) this.row((Func<SelectDifficultyData, UiComponent>) (d => (UiComponent) new Mafi.Unity.UiToolkit.Library.Label(d.Maintenance)), "Assets/Base/Products/Icons/Maintenance1.svg"));
      component3.Add((UiComponent) this.row((Func<SelectDifficultyData, UiComponent>) (d => (UiComponent) new Mafi.Unity.UiToolkit.Library.Label(d.Mining)), "Assets/Unity/UserInterface/Toolbar/Mining.svg"));
      component3.Add((UiComponent) this.row((Func<SelectDifficultyData, UiComponent>) (d => (UiComponent) new Mafi.Unity.UiToolkit.Library.Label(d.Research)), "Assets/Unity/UserInterface/Toolbar/Research.svg"));
      component3.Add((UiComponent) this.row((Func<SelectDifficultyData, UiComponent>) (d => (UiComponent) new Mafi.Unity.UiToolkit.Library.Label(d.Growth)), "Assets/Unity/UserInterface/Toolbar/Farms.svg"));
      component3.Add((UiComponent) this.row((Func<SelectDifficultyData, UiComponent>) (d => (UiComponent) new Mafi.Unity.UiToolkit.Library.Label(d.Rain)), "Assets/Base/Icons/Weather/Rain.svg"));
      component3.Add((UiComponent) this.row((Func<SelectDifficultyData, UiComponent>) (d => (UiComponent) new Mafi.Unity.UiToolkit.Library.Label(d.Contracts)), "Assets/Unity/UserInterface/Toolbar/CargoShip.svg"));
      component3.Add((UiComponent) this.row((Func<SelectDifficultyData, UiComponent>) (d => (UiComponent) new Mafi.Unity.UiToolkit.Library.Label(d.Disease)), "Assets/Unity/UserInterface/General/Population.svg", imageY: -6));
      component3.Add((UiComponent) this.row((Func<SelectDifficultyData, UiComponent>) (d => (UiComponent) new Mafi.Unity.UiToolkit.Library.Label(d.Pollution)), "Assets/Unity/UserInterface/Toolbar/Waste.svg"));
      component3.Add((UiComponent) this.row((Func<SelectDifficultyData, UiComponent>) (d => (UiComponent) new Mafi.Unity.UiToolkit.Library.Label(d.Unity)), "Assets/Unity/UserInterface/General/Unity.svg").PaddingBottom<Mafi.Unity.UiToolkit.Library.Row>(10.pt()));
      ScrollColumn scrollColumn = component3;
      this.m_scrollContainer = component3;
      uiComponentArray[1] = (UiComponent) scrollColumn;
      this.Add(uiComponentArray);
      this.selectDifficulty(this.m_settings.DifficultyIndex);
      this.RegisterCallback<WheelEvent>((EventCallback<WheelEvent>) (evt => this.m_scrollContainer.HandleScroll(evt)), TrickleDown.NoTrickleDown);
    }

    private void selectDifficulty(int index)
    {
      index = index.Clamp(0, SelectDifficultyData.All.Length - 1);
      this.m_settings.DifficultyIndex = index;
      this.m_settings.DifficultyConfig = SelectDifficultyData.All[index].CreateConfig();
      for (int index1 = 0; index1 < this.m_bigButtons.Length; ++index1)
      {
        bool isSelected = index1 == index;
        this.m_bigButtons[index1].Selected<ButtonColumn>(isSelected);
      }
    }

    private Mafi.Unity.UiToolkit.Library.Row row(
      Func<SelectDifficultyData, UiComponent> buildCell,
      string iconPath = null,
      bool stripe = true,
      int imageY = 0,
      Action<Mafi.Unity.UiToolkit.Library.Column> customize = null)
    {
      ++this.m_rowIndex;
      bool striped = stripe && (this.m_rowIndex & 1) == 0;
      Mafi.Unity.UiToolkit.Library.Row component1 = new Mafi.Unity.UiToolkit.Library.Row(3.pt());
      component1.Add<Mafi.Unity.UiToolkit.Library.Row>((Action<Mafi.Unity.UiToolkit.Library.Row>) (c => c.AlignItemsStretch<Mafi.Unity.UiToolkit.Library.Row>().Width<Mafi.Unity.UiToolkit.Library.Row>(DifficultyTab.WIDTH)));
      component1.Add((IEnumerable<UiComponent>) SelectDifficultyData.All.Select<Mafi.Unity.UiToolkit.Library.Column>((Func<SelectDifficultyData, Mafi.Unity.UiToolkit.Library.Column>) (d =>
      {
        Mafi.Unity.UiToolkit.Library.Column component2 = new Mafi.Unity.UiToolkit.Library.Column();
        component2.Add<Mafi.Unity.UiToolkit.Library.Column>((Action<Mafi.Unity.UiToolkit.Library.Column>) (c => c.AlignItemsStretch<Mafi.Unity.UiToolkit.Library.Column>().JustifyItemsCenter<Mafi.Unity.UiToolkit.Library.Column>().Width<Mafi.Unity.UiToolkit.Library.Column>((100 / SelectDifficultyData.All.Length).Percent()).Padding<Mafi.Unity.UiToolkit.Library.Column>(3.pt()).MarginLeftRight<Mafi.Unity.UiToolkit.Library.Column>(2.px()).Background<Mafi.Unity.UiToolkit.Library.Column>(new ColorRgba?(striped ? DifficultyTab.STRIPE_COLOR : ColorRgba.Empty))));
        component2.Add<Mafi.Unity.UiToolkit.Library.Column>(customize);
        component2.Add(buildCell(d));
        return component2;
      })));
      component1.Add(iconPath != null ? (UiComponent) new Icon(iconPath).Large().AbsolutePositionMiddle<Icon>(new Px?(-50.px())).MarginTop<Icon>(imageY.px()) : (UiComponent) null);
      return component1;
    }

    static DifficultyTab()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      DifficultyTab.STRIPE_COLOR = new ColorRgba(0, 70);
      DifficultyTab.WIDTH = 80.Percent();
    }
  }
}
