// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.OutputSortedProductBufferView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Buildings.OreSorting;
using Mafi.Core.Entities.Static;
using Mafi.Core.Input;
using Mafi.Core.Products;
using Mafi.Core.Syncers;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Buildings
{
  internal class OutputSortedProductBufferView : IUiElementWithUpdater, IUiElement
  {
    public static float RequiredHeight;
    private readonly Panel m_container;
    private ProductProto m_product;
    private readonly IInputScheduler m_inputScheduler;

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    public IProductBuffer Buffer { get; set; }

    private OreSortingPlant Entity { get; set; }

    public IUiUpdater Updater { get; }

    public OutputSortedProductBufferView(UiBuilder builder, IInputScheduler inputScheduler)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      OutputSortedProductBufferView productBufferView = this;
      this.m_inputScheduler = inputScheduler;
      this.m_container = builder.NewPanel(nameof (OutputSortedProductBufferView)).SetBackground(builder.Style.Panel.ItemOverlay);
      BufferView bufferView = builder.NewBufferView((IUiElement) this.m_container, isCompact: true).PutTo<BufferView>((IUiElement) this.m_container, Offset.Right(110f));
      TextWithIcon bufferDesc = new TextWithIcon(builder, 16).SetTextStyle(builder.Style.Panel.Text).SetIcon("Assets/Unity/UserInterface/General/Clock.svg").PutToLeftBottomOf<TextWithIcon>((IUiElement) bufferView, new Vector2(200f, 25f), Offset.Left(100f));
      IconContainer stuckIcon = builder.NewIconContainer("StuckIcon").SetIcon("Assets/Unity/UserInterface/General/Warning-v2.svg", builder.Style.Global.OrangeText).PutToLeftBottomOf<IconContainer>((IUiElement) bufferView, 20.Vector2(), Offset.Left(65f) + Offset.Bottom(5f)).Hide<IconContainer>();
      Tooltip tooltip = builder.AddTooltipFor<IconContainer>((IUiElementWithHover<IconContainer>) stuckIcon);
      tooltip.SetErrorTextStyle();
      tooltip.SetText((LocStrFormatted) TrCore.OreSorter_ProductBlocked__Tooltip);
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      updaterBuilder.Observe<Quantity>((Func<Quantity>) (() => productBufferView.Entity.GetSortedLastMonth(productBufferView.Buffer.Product))).Do((Action<Quantity>) (sorted => bufferDesc.SetPrefixText(string.Format("{0}: {1} / 60", (object) Tr.ProducedLastMonth, (object) sorted))));
      updaterBuilder.Observe<ProductProto>((Func<ProductProto>) (() => productBufferView.Buffer.Product)).Observe<Quantity>((Func<Quantity>) (() => productBufferView.Buffer.Capacity)).Observe<Quantity>((Func<Quantity>) (() => productBufferView.Buffer.Quantity)).Do((Action<ProductProto, Quantity, Quantity>) ((product, capacity, quantity) => bufferView.UpdateState(product, capacity, quantity)));
      List<string> options = new List<string>();
      for (int index = 0; index < 4; ++index)
      {
        char ch = (char) (65 + index);
        options.Add(ch.ToString());
      }
      Dropdwn dropdown = builder.NewDropdown("").AddOptions(options).AddTooltip(TrCore.OutputPort__Tooltip.TranslatedString).OnValueChange(new Action<int>(this.onValueChanged));
      dropdown.PutToRightTopOf<Dropdwn>((IUiElement) this.m_container, new Vector2(70f, 28f), Offset.Right(25f) + Offset.Top(2f));
      updaterBuilder.Observe<int>((Func<int>) (() => productBufferView.Entity.GetPortIndexFor(productBufferView.Buffer.Product))).Do((Action<int>) (index => dropdown.SetValueWithoutNotify(index)));
      updaterBuilder.Observe<bool>((Func<bool>) (() => !productBufferView.Entity.CanAcceptMoreTrucksForUi(productBufferView.Buffer.Product))).Do((Action<bool>) (isStuck => stuckIcon.SetVisibility<IconContainer>(isStuck)));
      Btn toggleAlertBtn = builder.NewBtn("TogglePanelBtn").SetButtonStyle(builder.Style.Global.GeneralBtnToToggle).SetIcon("Assets/Unity/UserInterface/General/Bell128.png", new Offset?(Offset.All(5f))).AddToolTip(TrCore.OreSorter_BlockedAlert__Tooltip).OnClick((Action) (() => productBufferView.m_inputScheduler.ScheduleInputCmd<SortingPlantSetBlockedProductAlertCmd>(new SortingPlantSetBlockedProductAlertCmd(productBufferView.Entity.Id, productBufferView.Buffer.Product.Id, !productBufferView.Entity.IsProductBlockedAlertEnabled(productBufferView.Buffer.Product)))));
      toggleAlertBtn.PutToRightTopOf<Btn>((IUiElement) this.m_container, 26.Vector2(), Offset.Right(30f + dropdown.GetWidth()) + Offset.Top(3f));
      updaterBuilder.Observe<bool>((Func<bool>) (() => productBufferView.Entity.IsProductBlockedAlertEnabled(productBufferView.Buffer.Product))).Do((Action<bool>) (alertsEnabled =>
      {
        if (alertsEnabled)
          toggleAlertBtn.SetButtonStyle(builder.Style.Global.GeneralBtnActive);
        else
          toggleAlertBtn.SetButtonStyle(builder.Style.Global.GeneralBtnToToggle);
      }));
      this.Updater = updaterBuilder.Build();
    }

    private void onValueChanged(int index)
    {
      this.m_inputScheduler.ScheduleInputCmd<SetProductPortCmd>(new SetProductPortCmd(this.Entity, this.m_product, index));
    }

    public void SetOutputBuffer(IProductBuffer buffer, OreSortingPlant entity)
    {
      this.Buffer = buffer;
      this.Entity = entity;
      this.m_product = this.Buffer.Product;
    }

    static OutputSortedProductBufferView()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      OutputSortedProductBufferView.RequiredHeight = 50f;
    }
  }
}
