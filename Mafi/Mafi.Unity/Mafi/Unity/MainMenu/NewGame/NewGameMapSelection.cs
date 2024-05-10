// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MainMenu.NewGame.NewGameMapSelection
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Base;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain.Generation;
using Mafi.Localization;
using Mafi.Numerics;
using Mafi.Unity.UiToolkit;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MainMenu.NewGame
{
  public class NewGameMapSelection
  {
    private static readonly Set<Proto.ID> IGNORED_RESOURCES;
    private static readonly string BUILT_IN_MAPS_PATH;
    private static readonly string DLC_MAPS_PATH;
    public readonly IModWithMaps Mod;
    public readonly IWorldRegionMapPreviewData PreviewData;
    public readonly bool IsBuiltIn;
    public readonly bool IsFromDlc;
    private Option<Texture2D> m_thumbnail;
    private Option<Texture2D>[] m_previews;
    private readonly Dict<Proto.ID, long> m_terrainResources;
    private readonly Dict<Proto.ID, long> m_terrainResourcesEasyToReach;

    public IWorldRegionMapAdditionalData AdditionalData { get; private set; }

    public WorldRegionMapFactoryConfig FactoryConfig { get; private set; }

    public LocStrFormatted Name
    {
      get
      {
        return this.PreviewData.NameTranslationId.HasValue ? (LocStrFormatted) LocalizationManager.LoadLocalizedString0(this.PreviewData.NameTranslationId.Value) : this.PreviewData.Name.AsLoc();
      }
    }

    public LocStrFormatted Description
    {
      get
      {
        return this.PreviewData.DescriptionTranslationId.HasValue ? (LocStrFormatted) LocalizationManager.LoadLocalizedString0(this.PreviewData.DescriptionTranslationId.Value) : this.PreviewData.Description.AsLoc();
      }
    }

    public bool ShowStartingLocations
    {
      get
      {
        return this.AdditionalData.StartingLocations.IsNotEmpty && this.AdditionalData.StartingLocations.Length > 1;
      }
    }

    public bool ShowLocationDifficulty
    {
      get
      {
        return this.ShowStartingLocations && this.AdditionalData.StartingLocations.Any((Func<StartingLocationPreview, bool>) (sl => sl.Difficulty != this.PreviewData.Difficulty));
      }
    }

    public NewGameMapSelection(IModWithMaps mod, IWorldRegionMapPreviewData previewData)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_terrainResources = new Dict<Proto.ID, long>();
      this.m_terrainResourcesEasyToReach = new Dict<Proto.ID, long>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Mod = mod;
      this.PreviewData = previewData;
      if (!this.PreviewData.FilePath.HasValue)
        return;
      this.IsBuiltIn = this.PreviewData.FilePath.Value.StartsWith(NewGameMapSelection.BUILT_IN_MAPS_PATH, StringComparison.Ordinal);
      this.IsFromDlc = this.PreviewData.FilePath.Value.StartsWith(NewGameMapSelection.DLC_MAPS_PATH, StringComparison.Ordinal) && Path.GetDirectoryName(this.PreviewData.FilePath.Value).EndsWith("Maps", StringComparison.Ordinal);
    }

    public bool TryGetMapData(IFileSystemHelper fsHelper, ProtosDb protosDb)
    {
      IWorldRegionMapAdditionalData fullData;
      WorldRegionMapFactoryConfig factoryConfig;
      bool mapData = this.Mod.TryGetMapData(this.PreviewData, fsHelper, protosDb, out fullData, out factoryConfig);
      this.AdditionalData = fullData;
      this.FactoryConfig = factoryConfig;
      return mapData;
    }

    public UiComponent GetThumbnailImage()
    {
      Option<Texture2D> option = this.m_thumbnail;
      byte[] imageData = this.PreviewData.ThumbnailImageData.ImageData;
      if (option.IsNone && imageData != null && imageData.Length != 0)
      {
        Texture2D tex = new Texture2D(0, 0);
        bool x = tex.LoadImage(imageData, true);
        x.AssertTrue("Failed to load map thumbnail.");
        if (x)
        {
          option = (Option<Texture2D>) tex;
          this.m_thumbnail = option;
        }
      }
      return !option.HasValue ? (UiComponent) new Img("Assets/Unity/UserInterface/General/ComingSoon.svg") : (UiComponent) new Img(option.Value, Outer.ShadowAll);
    }

    public UiComponent GetPreviewImage(
      IComponentWithImage img,
      ProtosDb protosDb,
      NewGameConfigForUi settings)
    {
      if (this.m_previews == null)
        this.m_previews = new Option<Texture2D>[this.AdditionalData.PreviewImagesData.Length];
      int index1 = settings.PreviewIndex.Clamp(0, this.AdditionalData.PreviewImagesData.Length - 1);
      Option<Texture2D> texture = this.m_previews[index1];
      ImmutableArray<EncodedImageAndMatrix> previewImagesData = this.AdditionalData.PreviewImagesData;
      if (texture.IsNone && previewImagesData.IsNotEmpty)
      {
        byte[] imageData = previewImagesData[index1].ImageData;
        if ((imageData != null ? (imageData.Length != 0 ? 1 : 0) : 0) != 0)
        {
          Texture2D tex = new Texture2D(0, 0);
          bool x = tex.LoadImage(previewImagesData[index1].ImageData, true);
          x.AssertTrue("Failed to load map preview image.");
          if (x)
          {
            texture = (Option<Texture2D>) tex;
            this.m_previews[index1] = texture;
          }
        }
      }
      if (texture.IsNone)
        return (UiComponent) new Img("Assets/Unity/UserInterface/General/ComingSoon.svg").Size<Img>(120.px()).MarginTopBottom<Img>(95.px()).AlignSelf<Img>(Align.Center);
      img.Image<IComponentWithImage>((Option<Texture2D>) texture.Value);
      UiComponent previewImage = img as UiComponent;
      Matrix4x4 viewMatrix = previewImagesData[index1].ViewProjectionMatrix.Value.ToUnityMatrix();
      Percent? nullable1;
      Percent? nullable2;
      if (settings.ShowResourcesOnMap)
      {
        Lyst<(float, UiComponent)> source = new Lyst<(float, UiComponent)>();
        foreach (MapResourceLocation resourceLocation in this.AdditionalData.ResourceLocations)
        {
          if (!NewGameMapSelection.IGNORED_RESOURCES.Contains((Proto.ID) resourceLocation.ProductProtoId))
          {
            (float, float, bool) tuple = translatePosition(resourceLocation.Position.ToVector3());
            float num1 = tuple.Item1;
            float num2 = tuple.Item2;
            if (tuple.Item3)
            {
              Option<ProductProto> option = protosDb.Get<ProductProto>((Proto.ID) resourceLocation.ProductProtoId);
              if (option.IsNone)
              {
                Log.Warning(string.Format("Product not of the expected type: {0}", (object) resourceLocation.ProductProtoId));
              }
              else
              {
                Icon component1 = new Icon("Assets/Unity/UserInterface/Toolbar/MapPinResource.png");
                Icon component2 = component1.Size<Icon>(60.px()).Color<Icon>(new ColorRgba?(new ColorRgba(26, 39, 62, 190))).Tooltip<Icon>(new LocStrFormatted?((LocStrFormatted) option.Value.Strings.Name)).Padding<Icon>(0.px()).Translate<Icon>(-50.Percent(), 0.Percent());
                nullable1 = new Percent?((num1 * 100f).Percent());
                nullable2 = new Percent?(((float) ((1.0 - (double) num2) * 100.0)).Percent());
                Percent? top = new Percent?();
                Percent? right = new Percent?();
                Percent? bottom = nullable2;
                Percent? left = nullable1;
                component2.AbsolutePosition<Icon>(top, right, bottom, left);
                component1.Add((UiComponent) new Icon(option.Value.IconPath).Large().Color<Icon>(new ColorRgba?(ColorRgba.White)).AbsolutePositionCenterMiddle<Icon>().Translate<Icon>(-50.Percent(), -70.Percent()));
                source.Add((num2, (UiComponent) component1));
              }
            }
          }
        }
        previewImage.Add(source.OrderBy<(float, UiComponent), float>((Func<(float, UiComponent), float>) (p => p.top)).Select<(float, UiComponent), UiComponent>((Func<(float, UiComponent), UiComponent>) (p => p.pin)));
      }
      for (int index2 = 0; index2 < this.AdditionalData.StartingLocations.Length; ++index2)
      {
        StartingLocationPreview startingLocation = this.AdditionalData.StartingLocations[index2];
        (float, float, bool) tuple = translatePosition(startingLocation.Position.ToCenterVector3());
        float num3 = tuple.Item1;
        float num4 = tuple.Item2;
        if (tuple.Item3)
        {
          int indexCapturable = index2;
          IconClickable child = new IconClickable("Assets/Unity/UserInterface/Toolbar/MapPinNumbered.svg", (Action) (() => settings.StartingLocationIndex = indexCapturable));
          IconClickable component = child.Large().Tooltip<IconClickable>(new LocStrFormatted?(string.Format("{0} {1}\n{2}", (object) Tr.StartingLocation_Title, (object) (index2 + 1), (object) Tr.Difficulty.Format(startingLocation.Difficulty.ToLabel())).AsLoc())).Padding<IconClickable>(0.px()).Translate<IconClickable>(-50.Percent(), 0.Percent());
          nullable2 = new Percent?((num3 * 100f).Percent());
          nullable1 = new Percent?(((float) ((1.0 - (double) num4) * 100.0)).Percent());
          Percent? top = new Percent?();
          Percent? right = new Percent?();
          Percent? bottom = nullable1;
          Percent? left = nullable2;
          component.AbsolutePosition<IconClickable>(top, right, bottom, left).Color<IconClickable>(new ColorRgba?(settings.StartingLocationIndex == index2 ? Theme.PrimaryColor : ColorRgba.White));
          child.Add((UiComponent) new Label((index2 + 1).ToString().AsLoc()).Color<Label>(new ColorRgba?(ColorRgba.Black)).FontSize<Label>(16).FontBold<Label>().AbsolutePositionCenterMiddle<Label>().Translate<Label>(-45.Percent(), -65.Percent()));
          previewImage.Add((UiComponent) child);
        }
      }
      return previewImage;

      (float, float, bool) translatePosition(Vector3 worldPos)
      {
        Vector2 vector2 = (Vector2) viewMatrix.MultiplyPoint(worldPos);
        float num1 = vector2.x / (float) texture.Value.width;
        float num2 = vector2.y / (float) texture.Value.height;
        bool flag = (double) num1 >= 0.0 && (double) num1 <= 1.0 && (double) num2 >= 0.0 && (double) num2 <= 1.0;
        return (num1, num2, flag);
      }
    }

    public IEnumerable<KeyValuePair<Proto.ID, long>> GetMapResources(
      ProtosDb protosDb,
      bool easyToReach = false)
    {
      if (this.m_terrainResources.IsNotEmpty)
        return !easyToReach ? (IEnumerable<KeyValuePair<Proto.ID, long>>) this.m_terrainResources : (IEnumerable<KeyValuePair<Proto.ID, long>>) this.m_terrainResourcesEasyToReach;
      foreach (MapTerrainResourceStats terrainResourcesStat in this.AdditionalData.TotalTerrainResourcesStats)
        processRes(terrainResourcesStat, this.m_terrainResources);
      foreach (MapTerrainResourceStats terrainResourcesStat in this.AdditionalData.EasyToReachTerrainResourcesStats)
        processRes(terrainResourcesStat, this.m_terrainResourcesEasyToReach);
      foreach (MapOtherResourceStats otherResourcesStat in this.AdditionalData.TotalOtherResourcesStats)
        processOther(otherResourcesStat, this.m_terrainResources);
      foreach (MapOtherResourceStats otherResourcesStat in this.AdditionalData.EasyToReachOtherResourcesStats)
        processOther(otherResourcesStat, this.m_terrainResourcesEasyToReach);
      return !easyToReach ? (IEnumerable<KeyValuePair<Proto.ID, long>>) this.m_terrainResources : (IEnumerable<KeyValuePair<Proto.ID, long>>) this.m_terrainResourcesEasyToReach;

      void processRes(MapTerrainResourceStats mtrs, Dict<Proto.ID, long> dict)
      {
        TerrainMaterialProto proto;
        if (mtrs.VolumeTilesCubed <= 0L || !protosDb.TryGetProto<TerrainMaterialProto>(mtrs.MaterialProtoId, out proto))
          return;
        ProductProto minedProduct = (ProductProto) proto.MinedProduct;
        if (NewGameMapSelection.IGNORED_RESOURCES.Contains((Proto.ID) minedProduct.Id))
          return;
        long longFloored = ((Fix64) mtrs.VolumeTilesCubed * proto.MinedQuantityPerTileCubed.Value.ToFix64()).ToLongFloored();
        if (longFloored <= 0L)
          return;
        dict[(Proto.ID) minedProduct.Id] = longFloored;
      }

      static void processOther(MapOtherResourceStats mors, Dict<Proto.ID, long> dict)
      {
        if (NewGameMapSelection.IGNORED_RESOURCES.Contains((Proto.ID) mors.ProductProtoId))
          return;
        long num;
        dict.TryGetValue((Proto.ID) mors.ProductProtoId, out num);
        dict[(Proto.ID) mors.ProductProtoId] = num + mors.Quantity.Value;
      }
    }

    static NewGameMapSelection()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      NewGameMapSelection.IGNORED_RESOURCES = new Set<Proto.ID>()
      {
        (Proto.ID) Ids.Products.Dirt,
        (Proto.ID) Ids.Products.Rock
      };
      NewGameMapSelection.BUILT_IN_MAPS_PATH = Path.GetFullPath("Maps");
      NewGameMapSelection.DLC_MAPS_PATH = Path.GetFullPath("DLCs");
    }
  }
}
