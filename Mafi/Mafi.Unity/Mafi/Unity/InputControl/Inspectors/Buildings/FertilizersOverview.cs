// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.FertilizersOverview
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Buildings.Farms;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Buildings
{
  internal class FertilizersOverview : WindowView
  {
    private StackContainer m_container;
    private readonly ImmutableArray<KeyValuePair<ProductProto, FertilizerProductParam>> m_allFertilizers;

    public FertilizersOverview(ProtosDb protosDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(nameof (FertilizersOverview));
      this.m_allFertilizers = protosDb.All<ProductProto>().Where<ProductProto>((Func<ProductProto, bool>) (x => x.TryGetParam<FertilizerProductParam>(out FertilizerProductParam _))).Select<ProductProto, KeyValuePair<ProductProto, FertilizerProductParam>>((Func<ProductProto, KeyValuePair<ProductProto, FertilizerProductParam>>) (x => Make.Kvp<ProductProto, FertilizerProductParam>(x, x.GetParam<FertilizerProductParam>().Value))).ToImmutableArray<KeyValuePair<ProductProto, FertilizerProductParam>>();
    }

    protected override void BuildWindowContent()
    {
      this.SetTitle((LocStrFormatted) Tr.FarmFertilizersOverview__Title);
      this.m_container = this.Builder.NewStackContainer("Container").SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.Dynamic).SetItemSpacing(5f).SetInnerPadding(Offset.TopBottom(10f)).PutToTopOf<StackContainer>((IUiElement) this.GetContentPanel(), 0.0f);
      foreach (KeyValuePair<ProductProto, FertilizerProductParam> allFertilizer in this.m_allFertilizers)
        new FertilizersOverview.FertilizerInfoView(this.Builder, allFertilizer.Key, allFertilizer.Value).AppendTo<FertilizersOverview.FertilizerInfoView>(this.m_container, new float?(80f));
      this.SetContentSize(350f, this.m_container.GetDynamicHeight());
    }

    private class FertilizerInfoView : IUiElement
    {
      private readonly Panel m_container;

      public GameObject GameObject => this.m_container.GameObject;

      public RectTransform RectTransform => this.m_container.RectTransform;

      public FertilizerInfoView(
        UiBuilder builder,
        ProductProto fertilizer,
        FertilizerProductParam fertilizerParams)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_container = builder.NewPanel("Fertilizer").SetBackground(builder.Style.Panel.ItemOverlay);
        ProtoWithIcon<ProductProto> objectToPlace = new ProtoWithIcon<ProductProto>((IUiElement) this.m_container, builder);
        objectToPlace.SetProto((Option<ProductProto>) fertilizer);
        objectToPlace.PutToLeftOf<ProtoWithIcon<ProductProto>>((IUiElement) this.m_container, 60f, Offset.Left(10f) + Offset.TopBottom(10f));
        TextWithIcon leftTopOf1 = new TextWithIcon(builder).SetTextStyle(builder.Style.Global.TextInc).SetIcon("Assets/Unity/UserInterface/General/Fertility128.png").PutToLeftTopOf<TextWithIcon>((IUiElement) this.m_container, new Vector2(200f, 20f), Offset.Top(10f) + Offset.Left(100f));
        TextWithIcon leftTopOf2 = new TextWithIcon(builder).SetTextStyle(builder.Style.Global.TextInc).SetIcon("Assets/Unity/UserInterface/General/Fertility128.png").PutToLeftTopOf<TextWithIcon>((IUiElement) this.m_container, new Vector2(200f, 20f), Offset.Top(35f) + Offset.Left(100f));
        leftTopOf1.SetPrefixText(Tr.FarmFertilizer__MaxFertility.Format(fertilizerParams.MaxFertility.ToStringRounded()).Value);
        leftTopOf2.SetPrefixText(Tr.FarmFertilizer__FertilizerConversion.Format(fertilizerParams.FertilityPerQuantity.ToStringRounded(1)).Value);
      }
    }
  }
}
