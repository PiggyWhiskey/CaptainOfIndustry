// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Factory.DataCenterWindowView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Economy;
using Mafi.Core.Factory.Datacenters;
using Mafi.Core.Products;
using Mafi.Core.Syncers;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using Mafi.Unity.UserInterface.Style;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Factory
{
  internal class DataCenterWindowView : StaticEntityInspectorBase<DataCenter>
  {
    private static int WINDOW_WIDTH;
    private ImmutableArray<DataCenterWindowView.ServerRackView> m_rackViews;
    private readonly DataCenterInspector m_controller;
    private readonly IAssetTransactionManager m_assetTransactions;
    private readonly ImmutableArray<ServerRackProto> m_rackProtos;

    protected override DataCenter Entity => this.m_controller.SelectedEntity;

    public DataCenterWindowView(
      DataCenterInspector controller,
      IAssetTransactionManager tradingManager,
      ImmutableArray<ServerRackProto> rackProtos)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector((IEntityInspector) controller);
      this.m_assetTransactions = tradingManager.CheckNotNull<IAssetTransactionManager>();
      this.m_controller = controller.CheckNotNull<DataCenterInspector>();
      this.m_rackProtos = rackProtos.CheckNotDefaultStruct<ImmutableArray<ServerRackProto>>();
    }

    protected override void AddCustomItems(StackContainer itemContainer)
    {
      base.AddCustomItems(itemContainer);
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      this.AddClearButton(new Action(((EntityInspector<DataCenter, DataCenterWindowView>) this.m_controller).Clear));
      this.AddComputingGenerationPanel(updaterBuilder, (Func<Computing>) (() => this.Entity.MaxComputingGenerationCapacity));
      StatusPanel statusInfo = this.AddStatusInfoPanel();
      updaterBuilder.Observe<DataCenter.State>((Func<DataCenter.State>) (() => this.Entity.CurrentState)).Do((Action<DataCenter.State>) (state =>
      {
        switch (state)
        {
          case DataCenter.State.Working:
            statusInfo.SetStatusWorking();
            break;
          case DataCenter.State.NoRacks:
            statusInfo.SetStatus(Tr.EntityStatus__Datacenter_NoServers, StatusPanel.State.Critical);
            break;
          case DataCenter.State.Paused:
            statusInfo.SetStatusPaused();
            break;
          case DataCenter.State.Broken:
            statusInfo.SetStatus(TrCore.EntityStatus__Broken, StatusPanel.State.Critical);
            break;
          case DataCenter.State.NotEnoughWorkers:
            statusInfo.SetStatusNoWorkers();
            break;
          case DataCenter.State.NotEnoughElectricity:
            statusInfo.SetStatus(TrCore.EntityStatus__LowPower, StatusPanel.State.Critical);
            break;
          case DataCenter.State.NotEnoughCoolant:
            statusInfo.SetStatus(Tr.EntityStatus__MissingCoolant, StatusPanel.State.Critical);
            break;
          case DataCenter.State.FullOutput:
            statusInfo.SetStatus(Tr.EntityStatus__FullOutput, StatusPanel.State.Critical);
            break;
        }
      }));
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.InputsTitle);
      BufferView parent1 = this.Builder.NewBufferView((IUiElement) this.ItemsContainer, isCompact: true).AppendTo<BufferView>(this.ItemsContainer, new float?(this.Builder.Style.BufferView.CompactHeight));
      TextWithIcon bufferDesc1 = new TextWithIcon(this.Builder).SetTextStyle(this.Builder.Style.Panel.Text).SetSuffixText("/ 60").SetSuffixIcon("Assets/Unity/UserInterface/General/Clock.svg").PutToLeftBottomOf<TextWithIcon>((IUiElement) parent1, new Vector2(200f, 25f), Offset.Left(100f));
      updaterBuilder.Observe<ProductProto>((Func<ProductProto>) (() => this.Entity.Prototype.CoolantIn)).Observe<PartialQuantity>((Func<PartialQuantity>) (() => this.Entity.CoolantInPerTick)).Do((Action<ProductProto, PartialQuantity>) ((product, quantity) =>
      {
        bufferDesc1.SetIcon(product.Graphics.IconPath);
        bufferDesc1.SetPrefixText((quantity * 1.Months().Ticks).ToStringRounded(1));
      }));
      updaterBuilder.Observe<ProductProto>((Func<ProductProto>) (() => this.Entity.CoolantInBuffer.Product)).Observe<Quantity>((Func<Quantity>) (() => this.Entity.CoolantInBuffer.Capacity)).Observe<Quantity>((Func<Quantity>) (() => this.Entity.CoolantInBuffer.Quantity)).Do(new Action<ProductProto, Quantity, Quantity>(parent1.UpdateState));
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.OutputsTitle);
      BufferView parent2 = this.Builder.NewBufferView((IUiElement) this.ItemsContainer, isCompact: true).AppendTo<BufferView>(this.ItemsContainer, new float?(this.Builder.Style.BufferView.CompactHeight));
      TextWithIcon bufferDesc2 = new TextWithIcon(this.Builder).SetTextStyle(this.Builder.Style.Panel.Text).SetSuffixText("/ 60").SetSuffixIcon("Assets/Unity/UserInterface/General/Clock.svg").PutToLeftBottomOf<TextWithIcon>((IUiElement) parent2, new Vector2(200f, 25f), Offset.Left(100f));
      updaterBuilder.Observe<ProductProto>((Func<ProductProto>) (() => this.Entity.Prototype.CoolantOut)).Observe<PartialQuantity>((Func<PartialQuantity>) (() => this.Entity.CoolantOutPerTick)).Do((Action<ProductProto, PartialQuantity>) ((product, quantity) =>
      {
        bufferDesc2.SetIcon(product.Graphics.IconPath);
        bufferDesc2.SetPrefixText((quantity * 1.Months().Ticks).ToStringRounded(1));
      }));
      updaterBuilder.Observe<ProductProto>((Func<ProductProto>) (() => this.Entity.CoolantOutBuffer.Product)).Observe<Quantity>((Func<Quantity>) (() => this.Entity.CoolantOutBuffer.Capacity)).Observe<Quantity>((Func<Quantity>) (() => this.Entity.CoolantOutBuffer.Quantity)).Do(new Action<ProductProto, Quantity, Quantity>(parent2.UpdateState));
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.ServerRacks__Title, new LocStrFormatted?((LocStrFormatted) Tr.ServerRacks__Tooltip));
      ImmutableArrayBuilder<DataCenterWindowView.ServerRackView> immutableArrayBuilder = new ImmutableArrayBuilder<DataCenterWindowView.ServerRackView>(this.m_rackProtos.Length);
      int i = 0;
      foreach (ServerRackProto rackProto in this.m_rackProtos)
      {
        DataCenterWindowView.ServerRackView element = new DataCenterWindowView.ServerRackView(this.Builder, rackProto, this.m_assetTransactions, (Func<DataCenter>) (() => this.Entity), new Action<ServerRackProto>(this.m_controller.AddRack), new Action<ServerRackProto>(this.m_controller.SellRack));
        immutableArrayBuilder[i] = element;
        itemContainer.Append((IUiElement) element, new float?(80f));
        this.AddUpdater(element.Updater);
        ++i;
      }
      this.m_rackViews = immutableArrayBuilder.GetImmutableArrayAndClear();
      this.SetContentSize((float) DataCenterWindowView.WINDOW_WIDTH, itemContainer.GetDynamicHeight());
      this.AddUpdater(updaterBuilder.Build());
    }

    static DataCenterWindowView()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      DataCenterWindowView.WINDOW_WIDTH = 500;
    }

    private class ServerRackView : IUiElement
    {
      private readonly StackContainer m_container;

      public GameObject GameObject => this.m_container.GameObject;

      public RectTransform RectTransform => this.m_container.RectTransform;

      public IUiUpdater Updater { get; private set; }

      public ServerRackView(
        UiBuilder builder,
        ServerRackProto proto,
        IAssetTransactionManager assetManager,
        Func<DataCenter> entityProvider,
        Action<ServerRackProto> onAddClick,
        Action<ServerRackProto> onRemoveClick)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        UiStyle style = builder.Style;
        this.m_container = builder.NewStackContainer(nameof (ServerRackView)).SetSizeMode(StackContainer.SizeMode.StaticDirectionAligned).SetStackingDirection(StackContainer.Direction.LeftToRight).SetItemSpacing(8f).SetInnerPadding(Offset.Left(style.Panel.Indent) + Offset.TopBottom(5f)).SetBackground(style.Panel.ItemOverlay);
        Txt countTxt = builder.NewTxt("Count").SetTextStyle(new TextStyle(style.Global.BlueForDark, 16, new FontStyle?(FontStyle.Bold))).SetAlignment(TextAnchor.MiddleLeft).EnableRichText().AppendTo<Txt>(this.m_container, new float?(50f));
        Panel parent1 = builder.NewPanel("IconWithName").AppendTo<Panel>(this.m_container, new float?(80f), noSpacingAfterThis: true);
        builder.NewTxt("RackName").AllowVerticalOverflow().SetAlignment(TextAnchor.UpperCenter).SetTextStyle(style.Panel.Text).SetText((LocStrFormatted) proto.Strings.Name).PutToBottomOf<Txt>((IUiElement) parent1, 18f);
        builder.NewIconContainer("RackIcon").SetIcon(proto.Graphics.IconPath).PutTo<IconContainer>((IUiElement) parent1, Offset.Bottom(18f));
        Panel parent2 = builder.NewPanel("AddRemoveBtns").AppendTo<Panel>(this.m_container, new float?(30f));
        Btn btn1 = builder.NewBtn("Add");
        BtnStyle primaryBtn = style.Global.PrimaryBtn;
        ref BtnStyle local1 = ref primaryBtn;
        int? nullable = new int?(16);
        ColorRgba? color1 = new ColorRgba?();
        FontStyle? fontStyle1 = new FontStyle?();
        int? fontSize1 = nullable;
        bool? isCapitalized1 = new bool?();
        BtnStyle buttonStyle1 = local1.ExtendText(color1, fontStyle1, fontSize1, isCapitalized1);
        Btn addBtn = btn1.SetButtonStyle(buttonStyle1).SetText("+").OnClick((Action) (() => onAddClick(proto))).PlayErrorSoundWhenDisabled().PutToTopOf<Btn>((IUiElement) parent2, 30f, Offset.Top(2.5f));
        CostTooltip costTooltip1 = new CostTooltip(builder, assetManager);
        costTooltip1.SetCost(new AssetValue(proto.ProductToAddThis));
        costTooltip1.AttachTo<Btn>((IUiElementWithHover<Btn>) addBtn);
        Btn btn2 = builder.NewBtn("Remove");
        BtnStyle minusPrimaryBtn = style.Global.MinusPrimaryBtn;
        ref BtnStyle local2 = ref minusPrimaryBtn;
        nullable = new int?(16);
        ColorRgba? color2 = new ColorRgba?();
        FontStyle? fontStyle2 = new FontStyle?();
        int? fontSize2 = nullable;
        bool? isCapitalized2 = new bool?();
        BtnStyle buttonStyle2 = local2.ExtendText(color2, fontStyle2, fontSize2, isCapitalized2);
        Btn removeBtn = btn2.SetButtonStyle(buttonStyle2).SetText("-").OnClick((Action) (() => onRemoveClick(proto))).PlayErrorSoundWhenDisabled().PutToBottomOf<Btn>((IUiElement) parent2, 30f, Offset.Bottom(2.5f));
        CostTooltip costTooltip2 = new CostTooltip(builder, assetManager, builder.Style.PricePanel.VehiclesSellProfitPanelStyle);
        costTooltip2.SetCost(new AssetValue(proto.ProductToRemoveThis));
        costTooltip2.AttachTo<Btn>((IUiElementWithHover<Btn>) removeBtn);
        builder.NewPanel("Divider").SetBackground((ColorRgba) 2039583).AppendTo<Panel>(this.m_container, new float?(1f), Offset.TopBottom(-5f));
        Panel panel = builder.NewPanel("InfoContainer");
        Panel leftMiddleOf1 = builder.NewPanel("PowerContainer").PutToLeftMiddleOf<Panel>((IUiElement) panel, new Vector2(60f, 50f));
        builder.NewIconContainer("PowerIcon").SetIcon(new IconStyle("Assets/Unity/UserInterface/General/Electricity.svg", new ColorRgba?(style.Global.OrangeText))).PutToCenterTopOf<IconContainer>((IUiElement) leftMiddleOf1, new Vector2(32f, 32f));
        builder.NewTxt("PowerText").SetAlignment(TextAnchor.MiddleCenter).SetTextStyle(style.Panel.Text).SetText(proto.ConsumedPowerPerTick.Format()).PutToBottomOf<Txt>((IUiElement) leftMiddleOf1, 22f);
        IconContainer leftMiddleOf2 = builder.NewIconContainer("->").SetIcon(new IconStyle(style.Icons.Transform, new ColorRgba?(style.Panel.PlainIconColor))).PutToLeftMiddleOf<IconContainer>((IUiElement) panel, style.RecipeDetail.TransformIconSize, Offset.Left(60f));
        Panel leftMiddleOf3 = builder.NewPanel("PowerContainer").PutToLeftMiddleOf<Panel>((IUiElement) panel, new Vector2(80f, 50f), Offset.Left(60f + style.RecipeDetail.TransformIconSize.x));
        builder.NewIconContainer("ComputingIcon").SetIcon(new IconStyle("Assets/Unity/UserInterface/General/Computing128.png", new ColorRgba?(style.Global.BlueForDark))).PutToCenterTopOf<IconContainer>((IUiElement) leftMiddleOf3, new Vector2(32f, 32f));
        builder.NewTxt("ComputingText").SetAlignment(TextAnchor.MiddleCenter).SetTextStyle(style.Panel.Text).SetText(proto.CreatedComputingPerTick.Format()).PutToBottomOf<Txt>((IUiElement) leftMiddleOf3, 22f);
        panel.AppendTo<Panel>(this.m_container, new float?(leftMiddleOf1.GetWidth() + leftMiddleOf3.GetWidth() + leftMiddleOf2.GetWidth()));
        UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
        updaterBuilder.Observe<int>((Func<int>) (() => entityProvider().GetNumberOfRacksFor(proto))).Observe<DataCenterProto>((Func<DataCenterProto>) (() => entityProvider().Prototype)).Do((Action<int, DataCenterProto>) ((count, dataCenterProto) =>
        {
          removeBtn.SetEnabled(count > 0);
          countTxt.SetText(string.Format("{0} <size=12>/ {1}</size>", (object) count, (object) dataCenterProto.RacksCapacity));
        }));
        updaterBuilder.Observe<bool>((Func<bool>) (() => entityProvider().RacksCount < entityProvider().Prototype.RacksCapacity)).Observe<bool>((Func<bool>) (() => entityProvider().IsConstructed)).Do((Action<bool, bool>) ((canAdd, isConstructed) => addBtn.SetEnabled(canAdd & isConstructed)));
        this.Updater = updaterBuilder.Build();
        this.Updater.AddChildUpdater(costTooltip1.CreateUpdater());
        this.Updater.AddChildUpdater(costTooltip2.CreateUpdater());
      }
    }
  }
}
