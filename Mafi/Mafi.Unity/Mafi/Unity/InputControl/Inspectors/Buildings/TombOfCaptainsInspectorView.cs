// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.TombOfCaptainsInspectorView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Base.Prototypes.Buildings;
using Mafi.Core.Entities.Static;
using Mafi.Core.Products;
using Mafi.Core.Syncers;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface.Components;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Buildings
{
  internal class TombOfCaptainsInspectorView : StaticEntityInspectorBase<TombOfCaptains>
  {
    private readonly TombOfCaptainsInspector m_controller;

    protected override TombOfCaptains Entity => this.m_controller.SelectedEntity;

    public TombOfCaptainsInspectorView(TombOfCaptainsInspector controller)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector((IEntityInspector) controller);
      this.m_controller = controller;
    }

    protected override void AddCustomItems(StackContainer itemContainer)
    {
      base.AddCustomItems(itemContainer);
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      this.Builder.NewTxt("ExtraText").SetText("").SetTextStyle(this.Builder.Style.Global.TextMedium).SetAlignment(TextAnchor.MiddleCenter).SetBackground(this.Builder.Style.Panel.ItemOverlay).AppendTo<Txt>(itemContainer, new float?(0.0f), Offset.Top(5f));
      Txt inputsTitle = this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.InputsTitle);
      BufferView gasInputBuffer = this.Builder.NewBufferView((IUiElement) itemContainer).AppendTo<BufferView>(itemContainer, new float?(this.Style.BufferView.Height));
      TextWithIcon gasInputDesc = new TextWithIcon(this.Builder).SetTextStyle(this.Style.Panel.Text).SetSuffixIcon("Assets/Unity/UserInterface/General/Clock.svg").SetSuffixText("/ 60").PutToLeftBottomOf<TextWithIcon>((IUiElement) gasInputBuffer, new Vector2(200f, 25f), Offset.Left(100f));
      updaterBuilder.Observe<TombOfCaptainsProto>((Func<TombOfCaptainsProto>) (() => this.Entity.Prototype)).Do((Action<TombOfCaptainsProto>) (proto =>
      {
        Fix32 fix32 = (proto.FuelConsumptionPerDay.Quantity * 30).Value;
        gasInputDesc.SetIcon(proto.FuelConsumptionPerDay.Product.Graphics.IconPath);
        gasInputDesc.SetPrefixText(string.Format("{0} {1}", (object) Tr.Needs, (object) fix32.ToStringRounded(1)));
      }));
      updaterBuilder.Observe<ProductProto>((Func<ProductProto>) (() => this.Entity.Prototype.FuelConsumptionPerDay.Product)).Observe<Quantity>((Func<Quantity>) (() =>
      {
        IProductBuffer valueOrNull = this.Entity.FuelBuffer.ValueOrNull;
        return valueOrNull == null ? Quantity.Zero : valueOrNull.Quantity;
      })).Observe<Quantity>((Func<Quantity>) (() =>
      {
        IProductBuffer valueOrNull = this.Entity.FuelBuffer.ValueOrNull;
        return valueOrNull == null ? Quantity.Zero : valueOrNull.Capacity;
      })).Do((Action<ProductProto, Quantity, Quantity>) ((product, quantity, capacity) =>
      {
        gasInputBuffer.UpdateState(product, capacity, quantity);
        itemContainer.SetItemVisibility((IUiElement) inputsTitle, capacity.IsNotZero);
        itemContainer.SetItemVisibility((IUiElement) gasInputBuffer, capacity.IsNotZero);
      }));
      BufferView flowersInputBuffer = this.Builder.NewBufferView((IUiElement) itemContainer).AppendTo<BufferView>(itemContainer, new float?(this.Style.BufferView.Height));
      TextWithIcon flowersInputDesc = new TextWithIcon(this.Builder).SetTextStyle(this.Style.Panel.Text).SetSuffixIcon("Assets/Unity/UserInterface/General/Clock.svg").SetSuffixText("/ 60").PutToLeftBottomOf<TextWithIcon>((IUiElement) flowersInputBuffer, new Vector2(200f, 25f), Offset.Left(100f));
      updaterBuilder.Observe<TombOfCaptainsProto>((Func<TombOfCaptainsProto>) (() => this.Entity.Prototype)).Do((Action<TombOfCaptainsProto>) (proto =>
      {
        Fix32 fix32 = (proto.FlowersConsumptionPerDay.Quantity * 30).Value;
        flowersInputDesc.SetIcon(proto.FlowersConsumptionPerDay.Product.Graphics.IconPath);
        flowersInputDesc.SetPrefixText(string.Format("{0} {1}", (object) Tr.Needs, (object) fix32.ToStringRounded(1)));
      }));
      updaterBuilder.Observe<ProductProto>((Func<ProductProto>) (() => this.Entity.Prototype.FlowersConsumptionPerDay.Product)).Observe<Quantity>((Func<Quantity>) (() =>
      {
        IProductBuffer valueOrNull = this.Entity.FlowersBuffer.ValueOrNull;
        return valueOrNull == null ? Quantity.Zero : valueOrNull.Quantity;
      })).Observe<Quantity>((Func<Quantity>) (() =>
      {
        IProductBuffer valueOrNull = this.Entity.FlowersBuffer.ValueOrNull;
        return valueOrNull == null ? Quantity.Zero : valueOrNull.Capacity;
      })).Do((Action<ProductProto, Quantity, Quantity>) ((product, quantity, capacity) =>
      {
        flowersInputBuffer.UpdateState(product, capacity, quantity);
        itemContainer.SetItemVisibility((IUiElement) flowersInputBuffer, capacity.IsNotZero);
      }));
      updaterBuilder.Observe<TombOfCaptainsProto>((Func<TombOfCaptainsProto>) (() => this.Entity.Prototype)).Do((Action<TombOfCaptainsProto>) (proto => { }));
      this.AddUpdater(updaterBuilder.Build());
    }
  }
}
