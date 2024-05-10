// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.StatueInspectorView
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
  internal class StatueInspectorView : StaticEntityInspectorBase<Statue>
  {
    private readonly StatueInspector m_controller;

    protected override Statue Entity => this.m_controller.SelectedEntity;

    public StatueInspectorView(StatueInspector controller)
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
      Txt extraText = this.Builder.NewTxt("ExtraText").SetText("").SetTextStyle(this.Builder.Style.Global.TextMedium).SetAlignment(TextAnchor.MiddleCenter).SetBackground(this.Builder.Style.Panel.ItemOverlay).AppendTo<Txt>(itemContainer, new float?(0.0f), Offset.Top(5f));
      Txt inputsTitle = this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.InputsTitle);
      BufferView inputBuffer = this.Builder.NewBufferView((IUiElement) itemContainer).AppendTo<BufferView>(itemContainer, new float?(this.Style.BufferView.Height));
      TextWithIcon inputDesc = new TextWithIcon(this.Builder).SetTextStyle(this.Style.Panel.Text).SetSuffixIcon("Assets/Unity/UserInterface/General/Clock.svg").SetSuffixText("/ 60").PutToLeftBottomOf<TextWithIcon>((IUiElement) inputBuffer, new Vector2(200f, 25f), Offset.Left(100f));
      updaterBuilder.Observe<StatueProto>((Func<StatueProto>) (() => this.Entity.Prototype)).Do((Action<StatueProto>) (proto =>
      {
        if (!proto.InputProduct.HasValue)
          return;
        Fix32 fix32 = (Fix32) (1.Months().Ticks / proto.DurationPerOneQuantity.Ticks);
        inputDesc.SetIcon(proto.InputProduct.Value.Graphics.IconPath);
        inputDesc.SetPrefixText(string.Format("{0} {1}", (object) Tr.Needs, (object) fix32.ToStringRounded(1)));
      }));
      updaterBuilder.Observe<Option<ProductProto>>((Func<Option<ProductProto>>) (() => this.Entity.Prototype.InputProduct)).Observe<Quantity>((Func<Quantity>) (() =>
      {
        IProductBuffer valueOrNull = this.Entity.InputBuffer.ValueOrNull;
        return valueOrNull == null ? Quantity.Zero : valueOrNull.Quantity;
      })).Observe<Quantity>((Func<Quantity>) (() =>
      {
        IProductBuffer valueOrNull = this.Entity.InputBuffer.ValueOrNull;
        return valueOrNull == null ? Quantity.Zero : valueOrNull.Capacity;
      })).Do((Action<Option<ProductProto>, Quantity, Quantity>) ((product, quantity, capacity) =>
      {
        if (product.HasValue)
          inputBuffer.UpdateState(product, capacity, quantity);
        itemContainer.SetItemVisibility((IUiElement) inputsTitle, product.HasValue);
        itemContainer.SetItemVisibility((IUiElement) inputBuffer, product.HasValue);
      }));
      updaterBuilder.Observe<StatueProto>((Func<StatueProto>) (() => this.Entity.Prototype)).Do((Action<StatueProto>) (proto =>
      {
        if (proto.ExtraText.TranslatedString.IsNotEmpty())
        {
          extraText.SetText((LocStrFormatted) proto.ExtraText);
          itemContainer.UpdateItemHeight((IUiElement) extraText, extraText.GetPreferedHeight(itemContainer.GetWidth()) + 10f);
        }
        itemContainer.SetItemVisibility((IUiElement) extraText, proto.ExtraText.TranslatedString.IsNotEmpty());
      }));
      this.AddUpdater(updaterBuilder.Build());
    }
  }
}
