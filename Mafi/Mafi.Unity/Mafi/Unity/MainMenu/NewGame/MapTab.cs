// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MainMenu.NewGame.MapTab
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Base.Terrain.Maps;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.SaveGame;
using Mafi.Core.Syncers;
using Mafi.Core.Terrain.Generation;
using Mafi.Localization;
using Mafi.Unity.UiToolkit;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library;
using Mafi.Unity.UserInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MainMenu.NewGame
{
  public class MapTab : Mafi.Unity.UiToolkit.Library.Row, ITab
  {
    private static readonly Px SIDE_COLUMN_WIDTH;
    private readonly IMain m_main;
    private readonly NewGameConfigForUi m_settings;
    private readonly ProtosDb m_protosDb;
    private readonly ImmutableArray<IMod> m_mods;
    private ImmutableArray<NewGameMapSelection> m_maps;
    private readonly ScrollColumn m_mapScroller;
    private readonly Mafi.Unity.UiToolkit.Library.Row m_mapContainer;
    private readonly ScrollColumn m_detailsContainer;
    private Mafi.Unity.UiToolkit.Library.Row m_nameBanner;
    private Mafi.Unity.UiToolkit.Library.Label m_difficultyLabel;
    private Mafi.Unity.UiToolkit.Library.Label m_startingLocationTitle;
    private Mafi.Unity.UiToolkit.Library.Label m_startingLocationDescription;
    private Mafi.Unity.UiToolkit.Library.Column m_extraResourcesContainer;
    private Option<MapDetailWindow> m_mapDetailWindow;
    private Option<IVisualElementScheduledItem> m_updateTask;
    private Option<MapTab.MapButton> m_selectedMapButton;
    private bool m_showEasyResources;
    private Option<IUiUpdater> m_updater;

    private Option<NewGameMapSelection> SelectedMap
    {
      get => (Option<NewGameMapSelection>) this.m_selectedMapButton.ValueOrNull?.Map;
    }

    private StartingLocationDifficulty CurrentDifficulty
    {
      get
      {
        NewGameMapSelection valueOrNull = this.SelectedMap.ValueOrNull;
        if (valueOrNull == null)
          return StartingLocationDifficulty.Easy;
        return !valueOrNull.AdditionalData.StartingLocations.IsNotEmpty ? valueOrNull.PreviewData.Difficulty : valueOrNull.AdditionalData.StartingLocations[this.m_settings.StartingLocationIndex].Difficulty;
      }
    }

    public MapTab(
      NewGameConfigForUi settings,
      IMain main,
      ProtosDb protosDb,
      ImmutableArray<IMod> mods)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_showEasyResources = true;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_settings = settings;
      this.m_main = main;
      this.m_protosDb = protosDb;
      this.m_mods = mods;
      this.AlignItemsStretch<MapTab>().Fill<MapTab>();
      UiComponent[] uiComponentArray = new UiComponent[2];
      ScrollColumn scrollColumn = new ScrollColumn();
      scrollColumn.Add((UiComponent) (this.m_mapContainer = new Mafi.Unity.UiToolkit.Library.Row().AlignItemsStretch<Mafi.Unity.UiToolkit.Library.Row>().Wrap<Mafi.Unity.UiToolkit.Library.Row>()));
      ScrollColumn component = scrollColumn;
      this.m_mapScroller = scrollColumn;
      uiComponentArray[0] = (UiComponent) component.AlignItemsStretch<ScrollColumn>().Width<ScrollColumn>(new Px?(452.px())).ScrollerAlwaysVisible();
      uiComponentArray[1] = (UiComponent) (this.m_detailsContainer = new ScrollColumn().PaddingLeft<ScrollColumn>(2.pt()).AlignItemsStretch<ScrollColumn>().Fill<ScrollColumn>());
      this.Add(uiComponentArray);
    }

    private void selectMap(Option<MapTab.MapButton> button)
    {
      MapTab.MapButton valueOrNull1 = this.m_selectedMapButton.ValueOrNull;
      if (valueOrNull1 != null)
        valueOrNull1.Selected<MapTab.MapButton>(false);
      MapTab.MapButton valueOrNull2 = button.ValueOrNull;
      if (valueOrNull2 != null)
        valueOrNull2.Selected<MapTab.MapButton>();
      this.m_selectedMapButton = button;
      if (this.m_settings.Map.IsNone || button.IsNone || this.m_settings.Map.Value.PreviewData.FilePath != button.Value.Map.PreviewData.FilePath)
      {
        this.m_settings.PreviewIndex = 0;
        this.m_settings.StartingLocationIndex = 0;
      }
      this.m_settings.Map = (Option<NewGameMapSelection>) button.ValueOrNull?.Map;
      this.loadMapDetails();
    }

    private bool selectMap(int indexOffset)
    {
      NewGameMapSelection map = this.m_maps[Math.Clamp(this.m_maps.IndexOf(this.m_settings.Map.ValueOrNull) + indexOffset, 0, this.m_maps.Length - 1)];
      if (map == this.SelectedMap.ValueOrNull)
        return false;
      foreach (UiComponent child in (UiComponent) this.m_mapContainer)
      {
        if (child is MapTab.MapButton button && button.Map == map)
        {
          this.selectMap((Option<MapTab.MapButton>) button);
          this.m_mapScroller.ScrollTo(child);
          return true;
        }
      }
      return false;
    }

    private void updateStartingLocation()
    {
      StartingLocationDifficulty currentDifficulty = this.CurrentDifficulty;
      this.m_nameBanner.Background<Mafi.Unity.UiToolkit.Library.Row>(new ColorRgba?(currentDifficulty.ToColor()));
      this.m_difficultyLabel.Text<Mafi.Unity.UiToolkit.Library.Label>(Tr.Difficulty.Format(currentDifficulty.ToLabel())).Tooltip<Mafi.Unity.UiToolkit.Library.Label>(new LocStrFormatted?((LocStrFormatted) currentDifficulty.ToDescription()));
      this.m_startingLocationTitle.Visible<Mafi.Unity.UiToolkit.Library.Label>(false);
      this.m_startingLocationDescription.Visible<Mafi.Unity.UiToolkit.Library.Label>(false);
      this.m_extraResourcesContainer.Visible<Mafi.Unity.UiToolkit.Library.Column>(false);
      ImmutableArray<StartingLocationPreview> startingLocations = this.SelectedMap.Value.AdditionalData.StartingLocations;
      if (!startingLocations.IsNotEmpty || startingLocations.Length <= 1)
        return;
      StartingLocationPreview startingLocationPreview = startingLocations[this.m_settings.StartingLocationIndex];
      bool isVisible = !string.IsNullOrWhiteSpace(startingLocationPreview.Description.ValueOrNull);
      this.m_startingLocationTitle.Visible<Mafi.Unity.UiToolkit.Library.Label>(isVisible);
      this.m_startingLocationDescription.Visible<Mafi.Unity.UiToolkit.Library.Label>(isVisible);
      if (isVisible)
        this.m_startingLocationDescription.Text<Mafi.Unity.UiToolkit.Library.Label>(startingLocationPreview.Description.Value.AsLoc());
      if (!startingLocationPreview.ExtraStartingResources.IsNotEmpty)
        return;
      this.m_extraResourcesContainer.SetChildren(((IEnumerable<UiComponent>) startingLocationPreview.ExtraStartingResources.Select<MapTab.ResourceInfo>((Func<ProductQuantity, MapTab.ResourceInfo>) (r => new MapTab.ResourceInfo(r.Product, (long) r.Quantity.Value)))).Prepend<UiComponent>(this.m_extraResourcesContainer[0]));
      this.m_extraResourcesContainer.Visible<Mafi.Unity.UiToolkit.Library.Column>(true);
    }

    void ITab.Activate()
    {
      Lyst<NewGameMapSelection> indexable = new Lyst<NewGameMapSelection>();
      foreach (IMod mod in this.m_mods)
      {
        IModWithMaps mwm = mod as IModWithMaps;
        if (mwm != null)
        {
          try
          {
            indexable.AddRange(mwm.GetMapPreviews(this.m_main.FileSystemHelper, this.m_protosDb, false).Select<IWorldRegionMapPreviewData, NewGameMapSelection>((Func<IWorldRegionMapPreviewData, NewGameMapSelection>) (m => new NewGameMapSelection(mwm, m))));
          }
          catch (Exception ex)
          {
            Log.Exception(ex, string.Format("Exception retrieving maps from mod {0} v{1}, ", (object) mod.Name, (object) mod.Version) + mod.GetType().FullName);
          }
        }
      }
      indexable.RemoveWhere((Predicate<NewGameMapSelection>) (x =>
      {
        foreach (ModInfoRaw requiredMod in x.PreviewData.RequiredMods)
        {
          ModInfoRaw modInfo = requiredMod;
          if (!this.m_mods.Any((Func<IMod, bool>) (m => m.GetType().FullName == modInfo.TypeStr)))
          {
            Log.Info(string.Format("Unable to load map '{0}' since it does not have required mod ", (object) x.Name) + "'" + modInfo.Name + "' (" + modInfo.TypeStr + "). Available mods: " + this.m_mods.Select<string>((Func<IMod, string>) (m => m.GetType().FullName)).JoinStrings(", "));
            return true;
          }
        }
        return false;
      }));
      indexable.Sort((Comparison<NewGameMapSelection>) ((l, r) =>
      {
        if (l.PreviewData.NameTranslationId.ValueOrNull == AlphaStaticIslandMap.LocName.Id)
          return -1;
        if (r.PreviewData.NameTranslationId.ValueOrNull == AlphaStaticIslandMap.LocName.Id)
          return 1;
        int num = l.PreviewData.Difficulty - r.PreviewData.Difficulty;
        return num != 0 ? num : l.Name.Value.CompareTo(r.Name.Value);
      }));
      NewGameMapSelection[] array1 = indexable.Where<NewGameMapSelection>((Func<NewGameMapSelection, bool>) (m => m.IsBuiltIn)).ToArray<NewGameMapSelection>();
      NewGameMapSelection[] array2 = indexable.Where<NewGameMapSelection>((Func<NewGameMapSelection, bool>) (m => m.IsFromDlc)).ToArray<NewGameMapSelection>();
      NewGameMapSelection[] array3 = indexable.Where<NewGameMapSelection>((Func<NewGameMapSelection, bool>) (m => !m.IsBuiltIn && !m.IsFromDlc)).ToArray<NewGameMapSelection>();
      Lyst<NewGameMapSelection> lyst = new Lyst<NewGameMapSelection>(indexable.Count);
      this.m_mapContainer.SetChildren((IEnumerable<UiComponent>) ((IEnumerable<NewGameMapSelection>) array1).Select<NewGameMapSelection, MapTab.MapButton>((Func<NewGameMapSelection, int, MapTab.MapButton>) ((m, i) => new MapTab.MapButton(new Action<Option<MapTab.MapButton>>(this.selectMap), m))));
      lyst.AddRange(array1);
      if (array2.Length != 0)
      {
        this.m_mapContainer.Add((UiComponent) new Title(Tr.SupporterMaps__Title).Width<Title>(Percent.Hundred).MarginTopBottom<Title>(2.pt()));
        this.m_mapContainer.Add((IEnumerable<UiComponent>) ((IEnumerable<NewGameMapSelection>) array2).Select<NewGameMapSelection, MapTab.MapButton>((Func<NewGameMapSelection, int, MapTab.MapButton>) ((m, i) => new MapTab.MapButton(new Action<Option<MapTab.MapButton>>(this.selectMap), m))));
        lyst.AddRange(array2);
      }
      if (array3.Length != 0)
      {
        this.m_mapContainer.Add((UiComponent) new Title(Tr.CustomMaps__Title).Width<Title>(Percent.Hundred).MarginTopBottom<Title>(2.pt()));
        this.m_mapContainer.Add((IEnumerable<UiComponent>) ((IEnumerable<NewGameMapSelection>) array3).Select<NewGameMapSelection, MapTab.MapButton>((Func<NewGameMapSelection, int, MapTab.MapButton>) ((m, i) => new MapTab.MapButton(new Action<Option<MapTab.MapButton>>(this.selectMap), m))));
        lyst.AddRange(array3);
      }
      this.m_maps = lyst.ToImmutableArrayAndClear();
      if (this.m_settings.Map.IsNone)
        this.m_settings.Map = (Option<NewGameMapSelection>) ((IEnumerable<NewGameMapSelection>) array1).FirstOrDefault<NewGameMapSelection>();
      MapTab.MapButton mapButton1 = (MapTab.MapButton) null;
      foreach (UiComponent uiComponent in (UiComponent) this.m_mapContainer)
      {
        if (uiComponent is MapTab.MapButton mapButton2)
        {
          Option<string> filePath1 = mapButton2.Map.PreviewData.FilePath;
          Option<string>? filePath2 = this.m_settings.Map.ValueOrNull?.PreviewData.FilePath;
          if ((filePath2.HasValue ? (filePath1 == filePath2.GetValueOrDefault() ? 1 : 0) : 0) != 0)
          {
            mapButton1 = mapButton2;
            break;
          }
        }
      }
      this.selectMap(mapButton1.CreateOption<MapTab.MapButton>());
    }

    private void loadMapDetails()
    {
      NewGameMapSelection map = this.SelectedMap.ValueOrNull;
      this.m_detailsContainer.Clear();
      if (map == null)
        return;
      this.m_settings.IsSelectedMapCorrupted = false;
      if (!map.TryGetMapData(this.m_main.FileSystemHelper, this.m_protosDb))
      {
        this.m_settings.IsSelectedMapCorrupted = true;
        this.m_detailsContainer.Add((UiComponent) new Mafi.Unity.UiToolkit.Library.Label((LocStrFormatted) Tr.LoadDisabled__Corrupted));
      }
      else
      {
        ImmutableArray<StartingLocationPreview> startingLocations = map.AdditionalData.StartingLocations;
        if (map.ShowStartingLocations)
          this.m_settings.StartingLocationIndex = this.m_settings.StartingLocationIndex.Clamp(0, startingLocations.Length);
        UpdaterBuilder builder = UpdaterBuilder.Start();
        this.m_mapDetailWindow = (Option<MapDetailWindow>) Option.None;
        ScrollColumn detailsContainer = this.m_detailsContainer;
        UiComponent[] uiComponentArray = new UiComponent[1];
        Mafi.Unity.UiToolkit.Library.Row component1 = new Mafi.Unity.UiToolkit.Library.Row(3.pt());
        component1.Add<Mafi.Unity.UiToolkit.Library.Row>((Action<Mafi.Unity.UiToolkit.Library.Row>) (c => c.AlignItemsStart<Mafi.Unity.UiToolkit.Library.Row>()));
        Mafi.Unity.UiToolkit.Library.Column column1 = new Mafi.Unity.UiToolkit.Library.Column();
        column1.Add<Mafi.Unity.UiToolkit.Library.Column>((Action<Mafi.Unity.UiToolkit.Library.Column>) (c => c.AlignItemsStretch<Mafi.Unity.UiToolkit.Library.Column>().Width<Mafi.Unity.UiToolkit.Library.Column>(100.Percent()).PaddingLeft<Mafi.Unity.UiToolkit.Library.Column>(1.pt()).PaddingBottom<Mafi.Unity.UiToolkit.Library.Column>(10.pt())));
        column1.Add((UiComponent) new MapTab.PreviewImage(builder, this.m_protosDb, this.m_settings, new Action(this.launchDetailWindow)).SetMap(map).MarginLeft<MapTab.PreviewImage>(-1.pt()));
        Mafi.Unity.UiToolkit.Library.Row component2 = new Mafi.Unity.UiToolkit.Library.Row(Outer.ShadowAll);
        component2.Add<Mafi.Unity.UiToolkit.Library.Row>((Action<Mafi.Unity.UiToolkit.Library.Row>) (c =>
        {
          Mafi.Unity.UiToolkit.Library.Row component3 = c.Padding<Mafi.Unity.UiToolkit.Library.Row>(2.pt(), 10.px()).MarginLeft<Mafi.Unity.UiToolkit.Library.Row>(-1.pt()).AlignItemsCenter<Mafi.Unity.UiToolkit.Library.Row>().JustifyItemsSpaceBetween<Mafi.Unity.UiToolkit.Library.Row>();
          Px? nullable = new Px?(6.px());
          Px? top = new Px?();
          Px? bottom = nullable;
          Px? left = new Px?();
          Px? right = new Px?();
          component3.BorderRadius<Mafi.Unity.UiToolkit.Library.Row>(top, bottom, left, right);
        }));
        component2.Add((UiComponent) new Mafi.Unity.UiToolkit.Library.Label(map.Name).FontBold<Mafi.Unity.UiToolkit.Library.Label>().MinWidth<Mafi.Unity.UiToolkit.Library.Label>(30.Percent()));
        Mafi.Unity.UiToolkit.Library.Row child1 = new Mafi.Unity.UiToolkit.Library.Row(2.pt());
        child1.Add((UiComponent) new Icon("Assets/Unity/UserInterface/Toolbar/MapPin.svg").Medium().Tooltip<Icon>(new LocStrFormatted?((LocStrFormatted) Tr.StartingLocation_Title)));
        child1.Add((UiComponent) new Dropdown<StartingLocationPreview>().Width<Dropdown<StartingLocationPreview>>(new Px?(26.pt())).ConstrainMenuWidth(false).SetOptionViewFactory(Utilities.StartingLocationViewFactory(map.ShowLocationDifficulty)).SetOptions(startingLocations.AsEnumerable()).ValueIndexObserve<StartingLocationPreview>(builder, (Func<int>) (() => this.m_settings.StartingLocationIndex)).OnValueChanged((Action<StartingLocationPreview, int>) ((_, idx) => this.m_settings.StartingLocationIndex = idx)));
        component2.Add((UiComponent) child1);
        component2.Add((UiComponent) (this.m_difficultyLabel = new Mafi.Unity.UiToolkit.Library.Label(Tr.Difficulty.Format(this.CurrentDifficulty.ToLabel())).Tooltip<Mafi.Unity.UiToolkit.Library.Label>(new LocStrFormatted?((LocStrFormatted) this.CurrentDifficulty.ToDescription())).AlignText<Mafi.Unity.UiToolkit.Library.Label>(TextAlign.RightMiddle).MinWidth<Mafi.Unity.UiToolkit.Library.Label>(30.Percent())));
        Mafi.Unity.UiToolkit.Library.Row child2 = component2;
        this.m_nameBanner = component2;
        column1.Add((UiComponent) child2);
        column1.Add((UiComponent) new Mafi.Unity.UiToolkit.Library.Label(map.Description).PaddingTopBottom<Mafi.Unity.UiToolkit.Library.Label>(4.pt()).FontSize<Mafi.Unity.UiToolkit.Library.Label>(16));
        column1.Add((UiComponent) (this.m_startingLocationTitle = new Title((LocStrFormatted) Tr.StartingLocation_Title).UpperCase(false).MarginTop<Mafi.Unity.UiToolkit.Library.Label>(2.pt()).MarginLeft<Mafi.Unity.UiToolkit.Library.Label>(-1.pt())));
        column1.Add((UiComponent) (this.m_startingLocationDescription = new Mafi.Unity.UiToolkit.Library.Label(LocStrFormatted.Empty).MarginTop<Mafi.Unity.UiToolkit.Library.Label>(2.pt())));
        component1.Add((UiComponent) column1);
        Mafi.Unity.UiToolkit.Library.Column column2 = new Mafi.Unity.UiToolkit.Library.Column(2.pt());
        column2.Add<Mafi.Unity.UiToolkit.Library.Column>((Action<Mafi.Unity.UiToolkit.Library.Column>) (c => c.FlexNoShrink<Mafi.Unity.UiToolkit.Library.Column>().AlignItemsStretch<Mafi.Unity.UiToolkit.Library.Column>().PaddingRight<Mafi.Unity.UiToolkit.Library.Column>(1.pt())));
        LocStrFormatted mapAreaTotal = (LocStrFormatted) Tr.MapArea__Total;
        int? nullable1 = new int?(map.PreviewData.MapSize.X);
        int? nullable2 = new int?(map.PreviewData.MapSize.Y);
        int? squareTileCount = new int?();
        int? tilesX = nullable1;
        int? tilesY = nullable2;
        column2.Add((UiComponent) new MapTab.MapInfo("Assets/Unity/UserInterface/General/MapBounds.svg", mapAreaTotal, squareTileCount, tilesX, tilesY));
        column2.Add((UiComponent) new MapTab.MapInfo("Assets/Unity/UserInterface/Toolbar/TerrainGrid.svg", (LocStrFormatted) Tr.MapArea__Land, new int?(map.AdditionalData.NonOceanTilesCount)));
        column2.Add((UiComponent) new MapTab.MapInfo("Assets/Unity/UserInterface/Toolbar/Buildings.svg", (LocStrFormatted) Tr.MapArea__Flat, new int?(map.AdditionalData.FlatNonOceanTilesCount)));
        Mafi.Unity.UiToolkit.Library.Row row = new Mafi.Unity.UiToolkit.Library.Row(2.pt());
        row.Add<Mafi.Unity.UiToolkit.Library.Row>((Action<Mafi.Unity.UiToolkit.Library.Row>) (c => c.MarginTop<Mafi.Unity.UiToolkit.Library.Row>(2.pt())));
        row.Add((UiComponent) new Title((LocStrFormatted) Tr.MapResources_Title).UpperCase(false).Fill<Mafi.Unity.UiToolkit.Library.Label>());
        IconClickable easyToReachResBtn;
        row.Add((UiComponent) (easyToReachResBtn = new IconClickable("Assets/Unity/UserInterface/General/Speed.svg").Tooltip<IconClickable>(new LocStrFormatted?(Tr.MapResources_EasyToReach_Tooltip.Format(WorldRegionMapAdditionalData.MAX_DEPTH_FOR_EASY_RESOURCES.ToString()))).Class<IconClickable>(Cls.toggle)));
        column2.Add((UiComponent) row);
        Mafi.Unity.UiToolkit.Library.Column resourcesCol;
        column2.Add((UiComponent) (resourcesCol = new Mafi.Unity.UiToolkit.Library.Column(2.pt()).FlexNoShrink<Mafi.Unity.UiToolkit.Library.Column>().AlignItemsStretch<Mafi.Unity.UiToolkit.Library.Column>()));
        column2.Add((UiComponent) new Mafi.Unity.UiToolkit.Library.Toggle(true).Label<Mafi.Unity.UiToolkit.Library.Toggle>((LocStrFormatted) Tr.MapResources_ShowPins).MarginTop<Mafi.Unity.UiToolkit.Library.Toggle>(5.pt()).OnValueChanged((Action<bool>) (b => this.m_settings.ShowResourcesOnMap = b)).ValueObserve<Mafi.Unity.UiToolkit.Library.Toggle>(builder, (Func<bool>) (() => this.m_settings.ShowResourcesOnMap)));
        Mafi.Unity.UiToolkit.Library.Column component4 = new Mafi.Unity.UiToolkit.Library.Column(2.pt());
        component4.Add((UiComponent) new Title("Extra starting resources".AsLoc()).UpperCase(false).MarginTop<Mafi.Unity.UiToolkit.Library.Label>(2.pt()));
        column2.Add((UiComponent) (this.m_extraResourcesContainer = component4.AlignItemsStretch<Mafi.Unity.UiToolkit.Library.Column>()));
        component1.Add((UiComponent) column2);
        uiComponentArray[0] = (UiComponent) component1;
        detailsContainer.SetChildren(uiComponentArray);
        this.updateStartingLocation();
        updateEasyToReach();
        easyToReachResBtn.OnClick<IconClickable>((Action) (() =>
        {
          this.m_showEasyResources = !this.m_showEasyResources;
          updateEasyToReach();
        }));
        builder.Observe<int>((Func<int>) (() => this.m_settings.StartingLocationIndex)).Do((Action<int>) (idx => this.updateStartingLocation()));
        IUiUpdater updater = builder.Build();
        this.m_updater = updater.SomeOption<IUiUpdater>();
        this.m_updateTask.ValueOrNull?.Pause();
        this.m_updateTask = this.Schedule.Execute((Action) (() =>
        {
          updater.SyncUpdate();
          updater.RenderUpdate();
        })).Every(50L).SomeOption<IVisualElementScheduledItem>();

        void updateEasyToReach()
        {
          resourcesCol.SetChildren((IEnumerable<UiComponent>) map.GetMapResources(this.m_protosDb, this.m_showEasyResources).Select<KeyValuePair<Proto.ID, long>, MapTab.ResourceInfo>((Func<KeyValuePair<Proto.ID, long>, MapTab.ResourceInfo>) (kvp => new MapTab.ResourceInfo(this.m_protosDb.Get<ProductProto>(kvp.Key).Value, kvp.Value))));
          easyToReachResBtn.Selected<IconClickable>(this.m_showEasyResources);
        }
      }
    }

    private void launchDetailWindow()
    {
      if (this.m_mapDetailWindow.IsNone)
      {
        MapDetailWindow mdw = new MapDetailWindow(this.m_protosDb, this.m_settings);
        this.m_mapDetailWindow = (Option<MapDetailWindow>) mdw;
        this.m_updater.ValueOrNull?.AddChildUpdater(mdw.Updater);
        mdw.OnHide((Action) (() => mdw.RemoveFromHierarchy()));
      }
      this.RunWithBuilder((Action<UiBuilder>) (b => b.AddComponent((UiComponent) this.m_mapDetailWindow.Value)));
      this.m_mapDetailWindow.Value.Show<MapDetailWindow>();
    }

    void ITab.Deactivate()
    {
      foreach (IMod mod in this.m_mods)
      {
        if (mod is IModWithMaps modWithMaps)
          modWithMaps.ClearMapData();
      }
    }

    public bool InputUpdate()
    {
      MapDetailWindow valueOrNull = this.m_mapDetailWindow.ValueOrNull;
      if ((valueOrNull != null ? (valueOrNull.IsVisible() ? 1 : 0) : 0) != 0)
        return this.m_mapDetailWindow.Value.InputUpdate();
      if (UnityEngine.Input.GetKeyDown(KeyCode.W))
        return this.selectMap(-2);
      if (UnityEngine.Input.GetKeyDown(KeyCode.A))
        return this.selectMap(-1);
      if (UnityEngine.Input.GetKeyDown(KeyCode.S))
        return this.selectMap(2);
      if (UnityEngine.Input.GetKeyDown(KeyCode.D))
        return this.selectMap(1);
      if (this.SelectedMap.IsNone || this.m_settings.IsSelectedMapCorrupted)
        return false;
      if (UnityEngine.Input.GetKeyDown(KeyCode.Space) || UnityEngine.Input.GetKeyDown(KeyCode.Return))
      {
        this.launchDetailWindow();
        return true;
      }
      int length = this.SelectedMap.Value.AdditionalData.PreviewImagesData.Length;
      if (length < 2)
        return false;
      if (UnityEngine.Input.GetKeyDown(KeyCode.LeftArrow) || UnityEngine.Input.GetKeyDown(KeyCode.UpArrow))
      {
        this.m_settings.PreviewIndex = (this.m_settings.PreviewIndex - 1).Max(0);
        return true;
      }
      if (!UnityEngine.Input.GetKeyDown(KeyCode.RightArrow) && !UnityEngine.Input.GetKeyDown(KeyCode.DownArrow))
        return false;
      this.m_settings.PreviewIndex = (this.m_settings.PreviewIndex + 1).Min(length - 1);
      return true;
    }

    private static LocStrFormatted formatNumber(long value)
    {
      string str = value >= 1000L ? "K" : "";
      return ((value >= 1000L ? value / 1000L : value).RoundToSigDigits(2, false, false, true) + str).AsLoc();
    }

    static MapTab()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      MapTab.SIDE_COLUMN_WIDTH = 20.pt();
    }

    private class MapButton : ButtonColumn
    {
      private static readonly Px MAX_WIDTH;
      public readonly NewGameMapSelection Map;

      public MapButton(Action<Option<MapTab.MapButton>> onClick, NewGameMapSelection map)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        MapTab.MapButton mapButton = this;
        this.Map = map;
        this.Variant<MapTab.MapButton>(ButtonVariant.Area);
        this.Gap<MapTab.MapButton>(new Px?(1.pt())).FlexGrow<MapTab.MapButton>(0.0f).AlignItemsCenter<MapTab.MapButton>();
        this.Padding<MapTab.MapButton>(2.pt());
        this.Add((UiComponent) new Mafi.Unity.UiToolkit.Library.Label(map.Name).MaxWidth<Mafi.Unity.UiToolkit.Library.Label>(MapTab.MapButton.MAX_WIDTH).FontBold<Mafi.Unity.UiToolkit.Library.Label>().AlignTextCenter<Mafi.Unity.UiToolkit.Library.Label>().FlexGrow<Mafi.Unity.UiToolkit.Library.Label>(1f), map.GetThumbnailImage().Size<UiComponent>(MapTab.MapButton.MAX_WIDTH, 100.px()));
        this.OnClick<MapTab.MapButton>((Action) (() => onClick((Option<MapTab.MapButton>) mapButton)));
      }

      static MapButton()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        MapTab.MapButton.MAX_WIDTH = 200.px();
      }
    }

    private class ResourceInfo : Mafi.Unity.UiToolkit.Library.Row
    {
      public ResourceInfo(ProductProto proto, long amount)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector(2.pt());
        this.AlignItemsEnd<MapTab.ResourceInfo>();
        this.Gap<MapTab.ResourceInfo>(new Px?(2.pt()));
        this.Add((UiComponent) new Img(proto.IconPath).FlexNoShrink<Img>().Size<Img>(20.px()), (UiComponent) new Mafi.Unity.UiToolkit.Library.Label((LocStrFormatted) proto.Strings.Name).FlexGrow<Mafi.Unity.UiToolkit.Library.Label>(1f).FlexNoShrink<Mafi.Unity.UiToolkit.Library.Label>().MinWidth<Mafi.Unity.UiToolkit.Library.Label>(new Px?(MapTab.SIDE_COLUMN_WIDTH)), (UiComponent) new Mafi.Unity.UiToolkit.Library.Label(MapTab.formatNumber(amount)).MinWidth<Mafi.Unity.UiToolkit.Library.Label>(new Px?(MapTab.SIDE_COLUMN_WIDTH)).AlignText<Mafi.Unity.UiToolkit.Library.Label>(TextAlign.RightMiddle));
      }
    }

    private class MapInfo : Mafi.Unity.UiToolkit.Library.Row
    {
      public MapInfo(
        string iconPath,
        LocStrFormatted label,
        int? squareTileCount = null,
        int? tilesX = null,
        int? tilesY = null)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.AlignItemsEnd<MapTab.MapInfo>();
        this.Gap<MapTab.MapInfo>(new Px?(2.pt()));
        LocStrFormatted locStr = squareTileCount.HasValue ? Tr.Area_Value.Format(((squareTileCount.Value / 1000 * 4).ToFix32() / 1000).ToStringRoundedAdaptive()) : Tr.MapSize_XY.Format(((tilesX.Value * 2).ToFix32() / 1000).ToStringRoundedAdaptive(), ((tilesY.Value * 2).ToFix32() / 1000).ToStringRoundedAdaptive());
        this.Add((UiComponent) new Img(iconPath).FlexNoShrink<Img>().Size<Img>(20.px()), (UiComponent) new Mafi.Unity.UiToolkit.Library.Label(label).FlexGrow<Mafi.Unity.UiToolkit.Library.Label>(1f).FlexNoShrink<Mafi.Unity.UiToolkit.Library.Label>().MinWidth<Mafi.Unity.UiToolkit.Library.Label>(new Px?(MapTab.SIDE_COLUMN_WIDTH)), (UiComponent) new Mafi.Unity.UiToolkit.Library.Label(locStr).MinWidth<Mafi.Unity.UiToolkit.Library.Label>(new Px?(MapTab.SIDE_COLUMN_WIDTH)).AlignText<Mafi.Unity.UiToolkit.Library.Label>(TextAlign.RightMiddle));
      }
    }

    private class PreviewImage : Mafi.Unity.UiToolkit.Library.Row
    {
      private readonly ProtosDb m_protosDb;
      private readonly NewGameConfigForUi m_settings;
      private NewGameMapSelection m_map;
      private readonly Action m_launchDetailView;

      public PreviewImage(
        UpdaterBuilder builder,
        ProtosDb protosDb,
        NewGameConfigForUi settings,
        Action launchDetailView)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        Px? gap = new Px?();
        // ISSUE: explicit constructor call
        base.\u002Ector(gap: gap);
        MapTab.PreviewImage previewImage = this;
        this.m_protosDb = protosDb;
        this.m_settings = settings;
        this.m_launchDetailView = launchDetailView;
        this.AlignItemsStretch<MapTab.PreviewImage>();
        UiComponent[] uiComponentArray = new UiComponent[3]
        {
          new UiComponent(),
          null,
          null
        };
        IconClickable component1 = new IconClickable("Assets/Unity/UserInterface/General/Left.svg", this.handleArrowClick(-1));
        gap = new Px?(0.px());
        Px? nullable = new Px?(0.px());
        Px? top1 = new Px?();
        Px? right1 = new Px?();
        Px? bottom1 = nullable;
        Px? left1 = gap;
        uiComponentArray[1] = (UiComponent) component1.AbsolutePosition<IconClickable>(top1, right1, bottom1, left1).Shadow().Large();
        IconClickable component2 = new IconClickable("Assets/Unity/UserInterface/General/Right.svg", this.handleArrowClick(1));
        nullable = new Px?(0.px());
        gap = new Px?(0.px());
        Px? top2 = new Px?();
        Px? right2 = nullable;
        Px? bottom2 = gap;
        Px? left2 = new Px?();
        uiComponentArray[2] = (UiComponent) component2.AbsolutePosition<IconClickable>(top2, right2, bottom2, left2).Shadow().Large();
        this.Add(uiComponentArray);
        builder.Observe<int>((Func<int>) (() => settings.PreviewIndex)).Do((Action<int>) (_ => previewImage.update()));
        builder.Observe<int>((Func<int>) (() => settings.StartingLocationIndex)).Do((Action<int>) (_ => previewImage.update()));
        builder.Observe<bool>((Func<bool>) (() => settings.ShowResourcesOnMap)).Do((Action<bool>) (_ => previewImage.update()));
      }

      public MapTab.PreviewImage SetMap(NewGameMapSelection map)
      {
        this.m_map = map;
        this.update();
        return this;
      }

      private UiComponent getPreview()
      {
        ButtonColumn component = new ButtonColumn(this.m_launchDetailView, Outer.ShadowAll);
        component.Add<ButtonColumn>((Action<ButtonColumn>) (c => c.AlignItemsStretch<ButtonColumn>().Fill<ButtonColumn>().Variant<ButtonColumn>(ButtonVariant.Boxy).Padding<ButtonColumn>(1.px(), 1.px(), 3.px(), 1.px()).MarginBottom<ButtonColumn>(-3.px())));
        component.Add(this.m_map.GetPreviewImage((IComponentWithImage) new StretchedImg(), this.m_protosDb, this.m_settings).Fill<UiComponent>().BorderRadius<UiComponent>(2.px()));
        component.Add((UiComponent) new Icon("Assets/Unity/UserInterface/General/ExpandScreen.svg").AbsolutePosition<Icon>(new Px?(2.pt()), new Px?(2.pt())));
        return (UiComponent) component;
      }

      private void update()
      {
        this.ChildAtOrDefault(0).RemoveFromHierarchy();
        this.InsertAt(0, this.getPreview());
        int length = this.m_map.AdditionalData.PreviewImagesData.Length;
        this.ChildAtOrDefault(1).Visible<UiComponent>(length > 1).Enabled<UiComponent>(this.m_settings.PreviewIndex > 0);
        this.ChildAtOrDefault(2).Visible<UiComponent>(length > 1).Enabled<UiComponent>(this.m_settings.PreviewIndex < length - 1);
      }

      private Action handleArrowClick(int dir)
      {
        return (Action) (() => this.m_settings.PreviewIndex += dir);
      }
    }
  }
}
