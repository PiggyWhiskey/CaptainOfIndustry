// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Ships.CargoShipModuleView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Buildings.Cargo.Ships.Modules;
using Mafi.Core.Products;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Ships
{
  internal class CargoShipModuleView : IUiElement
  {
    private readonly BufferView m_bufferView;

    public GameObject GameObject => this.m_bufferView.GameObject;

    public RectTransform RectTransform => this.m_bufferView.RectTransform;

    public float RequiredHeight { get; }

    public CargoShipModuleView(IUiElement parent, UiBuilder builder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.RequiredHeight = builder.Style.BufferView.SuperCompactHeight;
      this.m_bufferView = builder.NewBufferView(parent, isCompact: true).SetAsSuperCompact();
    }

    public void Update(Option<CargoShipModule> moduleMaybe)
    {
      if (moduleMaybe.IsNone)
      {
        this.m_bufferView.UpdateState(Option<ProductProto>.None, Quantity.Zero, Quantity.Zero);
      }
      else
      {
        CargoShipModule cargoShipModule = moduleMaybe.Value;
        this.m_bufferView.UpdateState(cargoShipModule.StoredProduct, cargoShipModule.Capacity, cargoShipModule.Quantity);
        this.m_bufferView.UsePositiveColor();
      }
    }

    public class Cache : ViewsCacheHomogeneous<CargoShipModuleView>
    {
      public Cache(IUiElement parent, UiBuilder builder)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector((Func<CargoShipModuleView>) (() => new CargoShipModuleView(parent, builder)));
      }
    }
  }
}
