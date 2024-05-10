// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MainMenu.NewGame.MapDetailWindow
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Prototypes;
using Mafi.Core.Syncers;
using Mafi.Core.Terrain.Generation;
using Mafi.Localization;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MainMenu.NewGame
{
  public class MapDetailWindow : Mafi.Unity.UiToolkit.Library.Window
  {
    public readonly IUiUpdater Updater;
    private readonly ProtosDb m_protosDb;
    private readonly NewGameConfigForUi m_settings;
    private NewGameMapSelection m_map;

    private int ImageCount => this.m_map.AdditionalData.PreviewImagesData.Length;

    public MapDetailWindow(ProtosDb protosDb, NewGameConfigForUi settings)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(settings.Map.Value.Name);
      MapDetailWindow mapDetailWindow = this;
      this.m_protosDb = protosDb;
      this.m_settings = settings;
      this.m_map = settings.Map.Value;
      UpdaterBuilder builder = UpdaterBuilder.Start();
      this.Fullscreen();
      this.Body.AlignItemsStretch<Column>();
      Column body = this.Body;
      UiComponent[] uiComponentArray = new UiComponent[2]
      {
        new UiComponent(),
        null
      };
      Row component = new Row(2.pt());
      component.Add<Row>((Action<Row>) (c => c.PaddingTop<Row>(2.pt())));
      component.Add((UiComponent) new Label((LocStrFormatted) Tr.StartingLocation_Title).MarginLeft<Label>(Px.Auto));
      component.Add((UiComponent) new Dropdown<StartingLocationPreview>().Width<Dropdown<StartingLocationPreview>>(new Px?(50.pt())).SetOptionViewFactory(Utilities.StartingLocationViewFactory(this.m_map.ShowLocationDifficulty)).SetOptions(this.m_map.AdditionalData.StartingLocations.AsEnumerable()).ValueIndexObserve<StartingLocationPreview>(builder, (Func<int>) (() => mapDetailWindow.m_settings.StartingLocationIndex)).OnValueChanged((Action<StartingLocationPreview, int>) ((_, idx) => mapDetailWindow.m_settings.StartingLocationIndex = idx)));
      component.Add((UiComponent) new Toggle(true).MarginLeft<Toggle>(4.pt()).MarginRight<Toggle>(Px.Auto).Label<Toggle>((LocStrFormatted) Tr.MapResources_ShowPinsTooltip).OnValueChanged((Action<bool>) (b => mapDetailWindow.m_settings.ShowResourcesOnMap = b)).ValueObserve<Toggle>(builder, (Func<bool>) (() => mapDetailWindow.m_settings.ShowResourcesOnMap)));
      uiComponentArray[1] = (UiComponent) component;
      body.Add(uiComponentArray);
      this.update();
      builder.Observe<int>((Func<int>) (() => settings.StartingLocationIndex)).Do((Action<int>) (idx => mapDetailWindow.update()));
      builder.Observe<int>((Func<int>) (() => settings.PreviewIndex)).Do((Action<int>) (idx => mapDetailWindow.update()));
      builder.Observe<bool>((Func<bool>) (() => settings.ShowResourcesOnMap)).Do((Action<bool>) (_ => mapDetailWindow.update()));
      this.Updater = builder.Build();
    }

    public override bool InputUpdate()
    {
      if (base.InputUpdate())
        return true;
      if (this.ImageCount < 2)
        return false;
      if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.UpArrow))
      {
        this.m_settings.PreviewIndex = (this.m_settings.PreviewIndex - 1).Max(0);
        return true;
      }
      if (!Input.GetKeyDown(KeyCode.RightArrow) && !Input.GetKeyDown(KeyCode.DownArrow))
        return false;
      this.m_settings.PreviewIndex = (this.m_settings.PreviewIndex + 1).Min(this.ImageCount - 1);
      return true;
    }

    private Action handleArrowClick(int dir)
    {
      return (Action) (() => this.m_settings.PreviewIndex += dir);
    }

    private void update()
    {
      UiComponent child = this.m_map.GetPreviewImage((IComponentWithImage) new FillImg(), this.m_protosDb, this.m_settings).Fill<UiComponent>();
      if (this.ImageCount > 1)
      {
        UiComponent component1 = child;
        UiComponent[] uiComponentArray = new UiComponent[2];
        IconClickable component2 = new IconClickable("Assets/Unity/UserInterface/General/Left.svg", this.handleArrowClick(-1));
        Px? nullable1 = new Px?(0.px());
        Px? nullable2 = new Px?(0.px());
        Px? top1 = new Px?();
        Px? right1 = new Px?();
        Px? bottom1 = nullable2;
        Px? left1 = nullable1;
        uiComponentArray[0] = (UiComponent) component2.AbsolutePosition<IconClickable>(top1, right1, bottom1, left1).Enabled<IconClickable>(this.m_settings.PreviewIndex > 0).Shadow().Large();
        IconClickable component3 = new IconClickable("Assets/Unity/UserInterface/General/Right.svg", this.handleArrowClick(1));
        nullable2 = new Px?(0.px());
        nullable1 = new Px?(0.px());
        Px? top2 = new Px?();
        Px? right2 = nullable2;
        Px? bottom2 = nullable1;
        Px? left2 = new Px?();
        uiComponentArray[1] = (UiComponent) component3.AbsolutePosition<IconClickable>(top2, right2, bottom2, left2).Enabled<IconClickable>(this.m_settings.PreviewIndex < this.ImageCount - 1).Shadow().Large();
        component1.Add(uiComponentArray);
      }
      this.Body[0].RemoveFromHierarchy();
      this.Body.InsertAt(0, child);
    }
  }
}
