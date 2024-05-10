// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.UniversalProductsSourceView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Base.Prototypes.Buildings;
using Mafi.Collections.ImmutableCollections;
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
  internal class UniversalProductsSourceView : StaticEntityInspectorBase<UniversalProductsSource>
  {
    private readonly UniversalProductsSourceInspector m_controller;
    private readonly ImmutableArray<ProductProto> m_availableProducts;
    private ProtoPicker<ProductProto> m_protoPicker;

    protected override UniversalProductsSource Entity => this.m_controller.SelectedEntity;

    public UniversalProductsSourceView(
      UniversalProductsSourceInspector controller,
      ImmutableArray<ProductProto> availableProducts)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector((IEntityInspector) controller);
      this.m_controller = controller;
      this.m_availableProducts = availableProducts;
    }

    protected override void AddCustomItems(StackContainer itemContainer)
    {
      base.AddCustomItems(itemContainer);
      this.AddSectionTitle(this.ItemsContainer, "Provided product");
      BufferView bufferView = this.AddBufferView(this.Style.BufferView.Height, new Action(this.trashBtnClicked), new Action(this.plusBtnClicked));
      this.m_protoPicker = new ProtoPicker<ProductProto>((Action<ProductProto>) (p =>
      {
        if (p != null)
        {
          this.Entity.SetProvidedProduct((Option<ProductProto>) p);
          this.m_protoPicker.Hide();
        }
        else
          Log.Error(string.Format("Trying to set non-product '{0}' as provided product", (object) p));
      }));
      this.m_protoPicker.BuildUi(this.Builder);
      bufferView.BindProductPicker((WindowView) this, this.AddOverlay(new Action(((View) this.m_protoPicker).Hide)), this.m_protoPicker);
      this.m_protoPicker.AppendTo<ProtoPicker<ProductProto>>(itemContainer, new Vector2?(this.m_protoPicker.GetSize()), ContainerPosition.MiddleOrCenter);
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      updaterBuilder.Observe<Option<ProductProto>>((Func<Option<ProductProto>>) (() => this.Entity.ProvidedProduct)).Observe<Quantity>((Func<Quantity>) (() => this.Entity.MaxProvidedPerTick)).Observe<Quantity>((Func<Quantity>) (() => this.Entity.ProvidedLastTick)).Do(new Action<Option<ProductProto>, Quantity, Quantity>(bufferView.UpdateState));
      this.AddUpdater(updaterBuilder.Build());
      this.OnHide += (Action) (() => this.m_protoPicker.Hide());
    }

    private void trashBtnClicked() => this.Entity.SetProvidedProduct(Option<ProductProto>.None);

    private void plusBtnClicked()
    {
      if (this.m_protoPicker.IsVisible)
      {
        this.m_protoPicker.Hide();
      }
      else
      {
        this.m_protoPicker.SetVisibleProtos(this.m_availableProducts.AsEnumerable());
        this.m_protoPicker.Show();
      }
    }
  }
}
