// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.OutputProductBufferViewWithUpdater
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Buildings.Waste;
using Mafi.Core.Entities.Static;
using Mafi.Core.Products;
using Mafi.Core.Syncers;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Buildings
{
  internal class OutputProductBufferViewWithUpdater : IUiElementWithUpdater, IUiElement
  {
    private readonly Panel m_container;

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    public float RequiredHeight { get; }

    public IProductBuffer Buffer { get; set; }

    private WasteSortingPlant Entity { get; set; }

    public IUiUpdater Updater { get; }

    public OutputProductBufferViewWithUpdater(UiBuilder builder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.RequiredHeight = builder.Style.BufferView.CompactHeight;
      this.m_container = builder.NewPanel("ProductBufferViewWithUpdater");
      BufferView bufferView = builder.NewBufferView((IUiElement) this.m_container, isCompact: true).PutTo<BufferView>((IUiElement) this.m_container);
      TextWithIcon bufferDesc = new TextWithIcon(builder).SetTextStyle(builder.Style.Panel.Text).SetIcon("Assets/Unity/UserInterface/General/Clock.svg").PutToLeftBottomOf<TextWithIcon>((IUiElement) bufferView, new Vector2(200f, 25f), Offset.Left(100f));
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      updaterBuilder.Observe<PartialQuantityLarge>((Func<PartialQuantityLarge>) (() => this.Entity.GetRecycledLastMonth(this.Buffer.Product))).Do((Action<PartialQuantityLarge>) (recycled => bufferDesc.SetPrefixText(string.Format("{0}: {1} / 60", (object) Tr.ProducedLastMonth, (object) recycled.Value.ToStringRounded(1)))));
      updaterBuilder.Observe<ProductProto>((Func<ProductProto>) (() => this.Buffer.Product)).Observe<Quantity>((Func<Quantity>) (() => this.Buffer.Capacity)).Observe<Quantity>((Func<Quantity>) (() => this.Buffer.Quantity)).Do((Action<ProductProto, Quantity, Quantity>) ((product, capacity, quantity) => bufferView.UpdateState(product, capacity, quantity)));
      this.Updater = updaterBuilder.Build();
    }

    public void SetOutputBuffer(IProductBuffer buffer, WasteSortingPlant entity)
    {
      this.Buffer = buffer;
      this.Entity = entity;
    }

    public class Cache : ViewsCacheHomogeneous<OutputProductBufferViewWithUpdater>
    {
      public Cache(UiBuilder builder)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector((Func<OutputProductBufferViewWithUpdater>) (() => new OutputProductBufferViewWithUpdater(builder)));
      }
    }
  }
}
