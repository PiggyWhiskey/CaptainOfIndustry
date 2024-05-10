// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.ResVis.LayersLegendView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core.Products;
using Mafi.Core.Terrain.Resources;
using Mafi.Localization;
using Mafi.Unity.InputControl.Toolbar;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.ResVis
{
  internal class LayersLegendView : WindowView
  {
    private readonly ToolbarController m_toolbarController;
    private readonly TerrainResourcesProvider m_provider;
    private readonly Action<LooseProductProto, bool> m_onTerrainMaterialToggle;
    private readonly Action<VirtualResourceProductProto, bool> m_onVirtualResourceToggle;
    private readonly Action<bool> m_onTerrainGridToggle;
    private readonly Action<bool> m_onTerrainDesignatorsToggle;
    private readonly Action<bool> m_onTreeDesignatorsToggle;
    private readonly Dict<LooseProductProto, bool> m_terrainMaterialItemsVisibility;
    private readonly Dict<VirtualResourceProductProto, bool> m_virtualResourceItemVisibility;
    private StackContainer m_itemsContainer;
    private LayersLegendItemView m_terrainGridToggle;
    private LayersLegendItemView m_terrainDesignatorsToggle;
    private LayersLegendItemView m_treeDesignatorsToggle;

    internal LayersLegendView(
      ToolbarController toolbarController,
      TerrainResourcesProvider provider,
      Action<LooseProductProto, bool> onTerrainMaterialToggle,
      Action<VirtualResourceProductProto, bool> onVirtualResourceToggle,
      Action<bool> onTerrainGridToggle,
      Action<bool> onTerrainDesignatorsToggle,
      Action<bool> onTreeDesignatorsToggle)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_terrainMaterialItemsVisibility = new Dict<LooseProductProto, bool>();
      this.m_virtualResourceItemVisibility = new Dict<VirtualResourceProductProto, bool>();
      // ISSUE: explicit constructor call
      base.\u002Ector("LayersLegend", WindowView.FooterStyle.None);
      this.m_toolbarController = toolbarController;
      this.m_provider = provider.CheckNotNull<TerrainResourcesProvider>();
      this.m_onTerrainMaterialToggle = onTerrainMaterialToggle.CheckNotNull<Action<LooseProductProto, bool>>();
      this.m_onVirtualResourceToggle = onVirtualResourceToggle.CheckNotNull<Action<VirtualResourceProductProto, bool>>();
      this.m_onTerrainGridToggle = onTerrainGridToggle;
      this.m_onTerrainDesignatorsToggle = onTerrainDesignatorsToggle.CheckNotNull<Action<bool>>();
      this.m_onTreeDesignatorsToggle = onTreeDesignatorsToggle.CheckNotNull<Action<bool>>();
    }

    protected override void BuildWindowContent()
    {
      this.SetTitle((LocStrFormatted) Tr.Overlays__Title);
      this.m_itemsContainer = this.Builder.NewStackContainer("Items container").SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.StaticDirectionAligned).SetInnerPadding(Offset.Top(this.Style.Panel.Padding)).PutTo<StackContainer>((IUiElement) this.GetContentPanel());
      this.m_terrainGridToggle = new LayersLegendItemView((IUiElement) this.m_itemsContainer, this.Builder, (LocStrFormatted) Tr.Overlays__Grid, "Assets/Unity/UserInterface/Toolbar/TerrainGrid.svg", new ColorRgba?(), (Action<LayersLegendItemView>) (view =>
      {
        view.Toggle();
        this.m_onTerrainGridToggle(view.IsOn);
      }), true).AppendTo<LayersLegendItemView>(this.m_itemsContainer, new float?((float) this.Builder.Style.Layers.ItemHeight));
      this.m_terrainDesignatorsToggle = new LayersLegendItemView((IUiElement) this.m_itemsContainer, this.Builder, (LocStrFormatted) Tr.Overlays__Designations, "Assets/Unity/UserInterface/Toolbar/Mining.svg", new ColorRgba?(), (Action<LayersLegendItemView>) (view =>
      {
        view.Toggle();
        this.m_onTerrainDesignatorsToggle(view.IsOn);
      }), true).AppendTo<LayersLegendItemView>(this.m_itemsContainer, new float?((float) this.Builder.Style.Layers.ItemHeight));
      this.m_treeDesignatorsToggle = new LayersLegendItemView((IUiElement) this.m_itemsContainer, this.Builder, (LocStrFormatted) Tr.Overlays__Trees, "Assets/Unity/UserInterface/Toolbar/TreeHarvesting.svg", new ColorRgba?(), (Action<LayersLegendItemView>) (view =>
      {
        view.Toggle();
        this.m_onTreeDesignatorsToggle(view.IsOn);
      }), true).AppendTo<LayersLegendItemView>(this.m_itemsContainer, new float?((float) this.Builder.Style.Layers.ItemHeight));
      this.AddSectionTitle(this.m_itemsContainer, (LocStrFormatted) Tr.Overlays__Resources);
      foreach (LooseProductProto looseProductProto in this.m_provider.LooseTerrainProducts.All())
      {
        LooseProductProto terrainProduct = looseProductProto;
        new LayersLegendItemView((IUiElement) this.m_itemsContainer, this.Builder, (LocStrFormatted) terrainProduct.Strings.Name, terrainProduct.Graphics.IconPath, new ColorRgba?(terrainProduct.Graphics.ResourcesVizColor), (Action<LayersLegendItemView>) (view =>
        {
          view.Toggle();
          this.m_onTerrainMaterialToggle(terrainProduct, view.IsOn);
          this.m_terrainMaterialItemsVisibility[terrainProduct] = view.IsOn;
        }), true).AppendTo<LayersLegendItemView>(this.m_itemsContainer, new float?((float) this.Builder.Style.Layers.ItemHeight));
        this.m_terrainMaterialItemsVisibility.Add(terrainProduct, true);
      }
      foreach (VirtualResourceProductProto resourceProductProto in this.m_provider.VirtualResourceProducts.All())
      {
        VirtualResourceProductProto virtualResource = resourceProductProto;
        new LayersLegendItemView((IUiElement) this.m_itemsContainer, this.Builder, (LocStrFormatted) virtualResource.Strings.Name, virtualResource.Product.Graphics.IconPath, new ColorRgba?(virtualResource.Graphics.ResourcesVizColor), (Action<LayersLegendItemView>) (view =>
        {
          view.Toggle();
          this.m_onVirtualResourceToggle(virtualResource, view.IsOn);
          this.m_virtualResourceItemVisibility[virtualResource] = view.IsOn;
        }), true).AppendTo<LayersLegendItemView>(this.m_itemsContainer, new float?((float) this.Builder.Style.Layers.ItemHeight));
        this.m_virtualResourceItemVisibility.Add(virtualResource, true);
      }
      this.SetContentSize((float) this.Style.Layers.Width, this.m_itemsContainer.GetDynamicHeight());
      this.DisableClippingPrevention();
      this.PutToLeftBottomOf<LayersLegendView>((IUiElement) this.m_toolbarController.OverlayAnchor, this.GetSize());
    }

    public IEnumerable<LooseProductProto> GetVisibleTerrainMaterials()
    {
      return this.m_terrainMaterialItemsVisibility.Where<KeyValuePair<LooseProductProto, bool>>((Func<KeyValuePair<LooseProductProto, bool>, bool>) (x => x.Value)).Select<KeyValuePair<LooseProductProto, bool>, LooseProductProto>((Func<KeyValuePair<LooseProductProto, bool>, LooseProductProto>) (x => x.Key));
    }

    public IEnumerable<ProductProto> GetVisibleVirtualResourcesStates()
    {
      return this.m_virtualResourceItemVisibility.Where<KeyValuePair<VirtualResourceProductProto, bool>>((Func<KeyValuePair<VirtualResourceProductProto, bool>, bool>) (x => x.Value)).Select<KeyValuePair<VirtualResourceProductProto, bool>, ProductProto>((Func<KeyValuePair<VirtualResourceProductProto, bool>, ProductProto>) (x => x.Key.Product));
    }

    public bool GetTerrainGridState() => this.m_terrainGridToggle.IsOn;

    public bool GetTerrainDesignatorsState() => this.m_terrainDesignatorsToggle.IsOn;

    public bool GetTreeDesignatorsState() => this.m_treeDesignatorsToggle.IsOn;
  }
}
