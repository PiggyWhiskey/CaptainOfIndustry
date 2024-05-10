// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.SingleProductFilterEditor`1
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core.Prototypes;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors
{
  /// <summary>
  /// A filtered products editor that is especially designed to work with inspector windows.
  /// </summary>
  public class SingleProductFilterEditor<T> : IUiElement where T : class, IProtoWithIconAndName
  {
    private readonly ProtoPicker<T> m_protoPicker;
    private readonly Func<IEnumerable<T>> m_availableProductsProvider;
    private readonly Func<Option<T>> m_assignedProductProvider;
    private readonly Lyst<T> m_visibleProductsCache;
    private readonly Panel m_container;
    private readonly ProtoWithIcon<T> m_protoWithIcon;
    private readonly ProtoWithIcon<T> m_noneProto;

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    public SingleProductFilterEditor(
      UiBuilder builder,
      ItemDetailWindowView parentWindow,
      Action<T> onProductRemoved,
      Action<T> onProductAdded,
      Func<IEnumerable<T>> availableProductsProvider,
      Func<Option<T>> assignedProductProvider)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_visibleProductsCache = new Lyst<T>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      SingleProductFilterEditor<T> productFilterEditor = this;
      this.m_availableProductsProvider = availableProductsProvider;
      this.m_assignedProductProvider = assignedProductProvider;
      this.m_container = builder.NewPanel("ProductFilter");
      this.m_protoPicker = new ProtoPicker<T>((Action<T>) (x =>
      {
        onProductAdded(x);
        productFilterEditor.m_protoPicker.Hide();
      }));
      Panel btnHolder = builder.NewPanel("BtnHolder").PutToLeftOf<Panel>((IUiElement) this.m_container, 40f);
      Btn btn = builder.NewBtnGeneral("PlusBtn").SetIcon(builder.Style.Icons.Plus).OnClick(new Action(this.plusBtnClicked));
      btn.PutToLeftMiddleOf<Btn>((IUiElement) btnHolder, 34.Vector2());
      this.m_noneProto = new ProtoWithIcon<T>((IUiElement) this.m_container, builder);
      this.m_noneProto.PutToLeftMiddleOf<ProtoWithIcon<T>>((IUiElement) this.m_container, builder.Style.ProductWithIcon.Size, Offset.Left(btn.GetWidth() + 5f));
      this.m_noneProto.SetProto(Option<T>.None);
      this.m_protoWithIcon = new ProtoWithIcon<T>((IUiElement) this.m_container, builder, (Action<T>) (x =>
      {
        onProductRemoved(x);
        productFilterEditor.m_protoPicker.Hide();
      })).EnableRemoveHoverEffect();
      this.m_protoWithIcon.PutToLeftMiddleOf<ProtoWithIcon<T>>((IUiElement) this.m_container, builder.Style.ProductWithIcon.Size, Offset.Left(btn.GetWidth() + 5f));
      parentWindow.SetupInnerWindowWithButton((WindowView) this.m_protoPicker, (IUiElement) btnHolder, (IUiElement) btn, new Action(returnBtnHolder), new Action(this.plusBtnClicked));
      parentWindow.OnHide += (Action) (() => productFilterEditor.m_protoPicker.Hide());

      void returnBtnHolder()
      {
        btnHolder.PutToLeftOf<Panel>((IUiElement) closure_0.m_container, 40f);
      }
    }

    public void UpdateFilteredProduct(Option<T> filteredProduct)
    {
      if (filteredProduct.HasValue)
        this.m_protoWithIcon.SetProto(filteredProduct);
      this.m_protoWithIcon.SetVisibility<ProtoWithIcon<T>>(filteredProduct.HasValue);
      this.m_noneProto.SetVisibility<ProtoWithIcon<T>>(filteredProduct.IsNone);
    }

    private void plusBtnClicked()
    {
      if (this.m_protoPicker.IsVisible)
      {
        this.m_protoPicker.Hide();
      }
      else
      {
        this.m_visibleProductsCache.Clear();
        this.m_visibleProductsCache.AddRange(this.m_availableProductsProvider());
        Option<T> option = this.m_assignedProductProvider();
        if (option.HasValue)
          this.m_visibleProductsCache.Remove(option.Value);
        this.m_protoPicker.SetVisibleProtos((IEnumerable<T>) this.m_visibleProductsCache);
        this.m_protoPicker.Show();
      }
    }
  }
}
