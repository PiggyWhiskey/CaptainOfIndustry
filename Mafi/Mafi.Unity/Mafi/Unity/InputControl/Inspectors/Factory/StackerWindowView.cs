// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Factory.StackerWindowView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Factory.Transports;
using Mafi.Core.Products;
using Mafi.Core.Syncers;
using Mafi.Localization;
using Mafi.Unity.Entities;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface.Components;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Factory
{
  internal class StackerWindowView : StaticEntityInspectorBase<Stacker>
  {
    private readonly StackerInspector m_controller;
    private readonly LinesFactory m_linesFactory;
    private readonly MbBasedEntitiesRenderer m_entitiesRenderer;
    private readonly InspectorContext m_context;
    private DropDepthPanel m_dropDepthPanel;
    private readonly ImmutableArray<ProductProto> m_allDumpableProducts;
    private GridContainer m_products;

    protected override Stacker Entity => this.m_controller.SelectedEntity;

    public StackerWindowView(
      StackerInspector controller,
      LinesFactory linesFactory,
      MbBasedEntitiesRenderer entitiesRenderer,
      ImmutableArray<ProductProto> allDumpableProducts)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector((IEntityInspector) controller);
      this.m_allDumpableProducts = allDumpableProducts;
      this.m_controller = controller;
      this.m_linesFactory = linesFactory;
      this.m_entitiesRenderer = entitiesRenderer;
      this.m_context = controller.Context;
    }

    protected override void AddCustomItems(StackContainer itemContainer)
    {
      base.AddCustomItems(itemContainer);
      Panel parent = this.AddOverlayPanel(itemContainer, offset: Offset.Top(10f));
      TextWithIcon throughput = new TextWithIcon(this.Builder).SetTextStyle(this.Builder.Style.Global.TextControlsBold).SetIcon("Assets/Unity/UserInterface/General/Clock.svg").PutToLeftOf<TextWithIcon>((IUiElement) parent, 0.0f, Offset.Left(15f));
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.StackerProducts__Title);
      this.m_products = this.Builder.NewGridContainer("IconsContainer").SetCellSize(this.Builder.Style.ProductWithIcon.Size).SetCellSpacing(3f).SetBackground(this.Style.Panel.ItemOverlay).SetDynamicHeightMode(6);
      this.m_products.StartBatchOperation();
      ProtoWithIcon<ProductProto>.Cache cache = new ProtoWithIcon<ProductProto>.Cache((IUiElement) itemContainer, this.Builder);
      foreach (ProductProto allDumpableProduct in this.m_allDumpableProducts)
        this.m_products.Append((IUiElement) cache.GetView(allDumpableProduct));
      this.m_products.FinishBatchOperation();
      this.m_products.AppendTo<GridContainer>(itemContainer);
      this.SetWidth(this.m_products.GetRequiredWidth());
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      updaterBuilder.Observe<StackerProto>((Func<StackerProto>) (() => this.Entity.Prototype)).Observe<bool>((Func<bool>) (() => this.Builder.DurationNormalizer.IsNormalizationOn)).Do((Action<StackerProto, bool>) ((proto, normalize) =>
      {
        Duration duration = normalize ? 60.Seconds() : 1.Seconds();
        string str = this.Builder.DurationNormalizer.NormalizeThroughput(new PartialQuantity(Fix32.One / proto.DumpPeriod.Ticks));
        throughput.SetPrefixText(Tr.ThroughputWithParam.Format(string.Format("{0} / {1}", (object) str, (object) duration.Seconds.IntegerPart)).Value.ToUpper(LocalizationManager.CurrentCultureInfo));
      }));
      this.AddUpdater(updaterBuilder.Build());
      this.m_dropDepthPanel = new DropDepthPanel((IUiElement) this, this.m_context.InputScheduler, (Func<Stacker>) (() => this.Entity), this.Builder, this.m_linesFactory, this.m_entitiesRenderer, 30, Tr.DumpOffset.TranslatedString);
      this.m_dropDepthPanel.PutToRightTopOf<DropDepthPanel>((IUiElement) this, this.m_dropDepthPanel.GetSize(), Offset.Top(165f) + Offset.Right((float) (-(double) this.m_dropDepthPanel.GetWidth() + 1.0)));
      this.AddUpdater(this.m_dropDepthPanel.Updater);
    }

    public void OnDeactivated() => this.m_dropDepthPanel.OnDeactivated();
  }
}
