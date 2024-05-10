// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.CropPicker
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Buildings.Farms;
using Mafi.Core.PropertiesDb;
using Mafi.Core.Prototypes;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Buildings
{
  internal class CropPicker : WindowView
  {
    private readonly UnlockedProtosDbForUi m_unlockedProtosDb;
    private readonly ProtosDb m_protosDb;
    private readonly IPropertiesDb m_propsDb;
    private readonly IUiElement m_parent;
    private readonly Action<Option<CropProto>> m_onCropPicked;
    private readonly ImmutableArray<CropProto> m_allCrops;
    private StackContainer m_container;
    private ViewsCacheHomogeneous<CropView> m_cropsCache;
    private Btn m_none;
    private static int LEGEND_HEIGHT;
    private static float MAX_HEIGHT;
    private float m_maxHeight;
    private ScrollableContainer m_scrollCont;

    public CropPicker(
      UnlockedProtosDbForUi unlockedProtosDb,
      ProtosDb protosDb,
      IPropertiesDb propsDb,
      IUiElement parent,
      Action<Option<CropProto>> onCropPicked)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(nameof (CropPicker));
      this.m_unlockedProtosDb = unlockedProtosDb;
      this.m_protosDb = protosDb;
      this.m_propsDb = propsDb;
      this.m_parent = parent;
      this.m_onCropPicked = onCropPicked;
      this.m_allCrops = this.m_protosDb.All<CropProto>().ToImmutableArray<CropProto>();
    }

    protected override void BuildWindowContent()
    {
      this.m_maxHeight = CropPicker.MAX_HEIGHT.Min(this.ResolveWindowSize().y - 300f);
      this.SetTitle((LocStrFormatted) Tr.FarmCropSelector);
      // ISSUE: method pointer
      this.m_cropsCache = new ViewsCacheHomogeneous<CropView>((Func<CropView>) (() => new CropView(this.Builder, this.m_protosDb, this.m_propsDb, new Action<CropView>((object) this, __methodptr(\u003CBuildWindowContent\u003Eg__onCropClick\u007C14_0)))));
      TextWithIcon textWithIcon = new TextWithIcon(this.Builder).SetTextStyle(this.Builder.Style.Global.Text).SetPrefixText(Tr.MonthDurationLegend.Format(60.ToString()).Value).SetIcon("Assets/Unity/UserInterface/General/Clock.svg");
      textWithIcon.PutToLeftTopOf<TextWithIcon>((IUiElement) this.GetContentPanel(), new Vector2(textWithIcon.GetWidth(), (float) CropPicker.LEGEND_HEIGHT), Offset.Left(10f));
      this.m_scrollCont = this.Builder.NewScrollableContainer("ScrollableCont").AddVerticalScrollbar().SetScrollSensitivity(this.Builder.Style.RecipeDetail.Height);
      this.m_container = this.Builder.NewStackContainer("Container").SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.Dynamic).SetItemSpacing(5f);
      this.m_scrollCont.AddItemTop((IUiElement) this.m_container);
      this.m_scrollCont.PutTo<ScrollableContainer>((IUiElement) this.GetContentPanel(), Offset.Top((float) CropPicker.LEGEND_HEIGHT));
      this.m_none = this.Builder.NewBtn("none").SetButtonStyle(new BtnStyle(backgroundClr: new ColorRgba?(this.Builder.Style.Panel.ItemOverlay), normalMaskClr: new ColorRgba?((ColorRgba) 14540253), hoveredMaskClr: new ColorRgba?((ColorRgba) 15658734), pressedMaskClr: new ColorRgba?((ColorRgba) 16777215))).OnClick((Action) (() => this.m_onCropPicked(Option<CropProto>.None))).PutTo<Btn>((IUiElement) this);
      this.Builder.NewIconContainer("NoneIcon").SetIcon("Assets/Unity/UserInterface/General/Empty128.png").PutToRightMiddleOf<IconContainer>((IUiElement) this.m_none, new Vector2(40f, 40f), Offset.Right(20f));
      this.Builder.NewTxt("Txt").SetText((LocStrFormatted) Tr.None).SetTextStyle(this.Builder.Style.Global.Title).SetAlignment(TextAnchor.MiddleRight).PutToRightOf<Txt>((IUiElement) this.m_none, 100f, Offset.Right(70f));
    }

    public void RefreshData(FarmProto farm)
    {
      this.m_container.ClearAll();
      this.m_container.StartBatchOperation();
      this.m_cropsCache.ReturnAll();
      this.m_none.AppendTo<Btn>(this.m_container, new float?(this.Builder.Style.RecipeDetail.Height));
      foreach (CropProto allCrop in this.m_allCrops)
      {
        if (this.m_unlockedProtosDb.IsUnlocked((IProto) allCrop))
          this.m_cropsCache.GetView().SetCrop(allCrop, farm).AppendTo<CropView>(this.m_container);
      }
      this.m_container.FinishBatchOperation();
      float self = this.m_container.GetDynamicHeight() + (float) CropPicker.LEGEND_HEIGHT;
      float height = self.Min(this.m_maxHeight).Min(this.m_parent.GetHeight() - 80f);
      if ((double) self > (double) this.m_maxHeight)
      {
        this.SetContentSize(416f, height);
        this.m_container.SetInnerPadding(Offset.Right(16f));
      }
      else
      {
        this.SetContentSize(400f, height);
        this.m_container.SetInnerPadding(Offset.Zero);
      }
    }

    static CropPicker()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      CropPicker.LEGEND_HEIGHT = 35;
      CropPicker.MAX_HEIGHT = 500f;
    }
  }
}
