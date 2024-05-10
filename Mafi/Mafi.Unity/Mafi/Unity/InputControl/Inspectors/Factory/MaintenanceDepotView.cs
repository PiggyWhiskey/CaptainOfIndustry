// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Factory.MaintenanceDepotView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Maintenance;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Syncers;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Factory
{
  internal class MaintenanceDepotView : MachineWindowViewBase<MaintenanceDepot, MaintenanceDepotView>
  {
    private readonly MaintenanceDepotInspector m_controller;
    private readonly MaintenanceManager m_maintenanceManager;
    private readonly UnlockedProtosDb m_unlockedProtosDb;

    public MaintenanceDepotView(
      MaintenanceDepotInspector controller,
      MaintenanceManager maintenanceManager,
      UnlockedProtosDb unlockedProtosDb,
      UnlockedProtosDbForUi unlockedProtosDbForUi)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector((EntityInspector<MaintenanceDepot, MaintenanceDepotView>) controller, unlockedProtosDbForUi, 450f);
      this.m_controller = controller;
      this.m_maintenanceManager = maintenanceManager;
      this.m_unlockedProtosDb = unlockedProtosDb;
    }

    protected override void AddCustomItems(StackContainer itemContainer)
    {
      base.AddCustomItems(itemContainer);
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.GlobalMaintenanceStatus__Title, new LocStrFormatted?((LocStrFormatted) Tr.GlobalMaintenanceStatus__Tooltip));
      foreach (IMaintenanceBufferReadonly maintenanceBufferReadonly in (IEnumerable<IMaintenanceBufferReadonly>) this.m_maintenanceManager.MaintenanceBuffers.OrderBy<IMaintenanceBufferReadonly, ProductProto>((Func<IMaintenanceBufferReadonly, ProductProto>) (x => x.Product)))
      {
        IMaintenanceBufferReadonly buffer = maintenanceBufferReadonly;
        BufferView bufferView = this.Builder.NewBufferView((IUiElement) itemContainer, isCompact: true).AppendTo<BufferView>(itemContainer, new float?(this.Style.BufferView.CompactHeight));
        TextWithIcon desc = new TextWithIcon(this.Builder).SetTextStyle(this.Builder.Style.Panel.Text).SetIcon("Assets/Unity/UserInterface/General/Clock.svg").EnableRichText().PutToLeftBottomOf<TextWithIcon>((IUiElement) bufferView, new Vector2(300f, 25f), Offset.Left(100f));
        updaterBuilder.Observe<Quantity>((Func<Quantity>) (() => buffer.Capacity)).Observe<Quantity>((Func<Quantity>) (() => buffer.Quantity)).Do((Action<Quantity, Quantity>) ((capacity, quantity) => bufferView.UpdateState(buffer.Product, capacity, quantity)));
        updaterBuilder.Observe<bool>((Func<bool>) (() => buffer.ShouldBeLastDeltaReported)).Observe<PartialQuantity>((Func<PartialQuantity>) (() => buffer.DeltaLastMonth)).Observe<PartialQuantity>((Func<PartialQuantity>) (() => buffer.MonthlyNeededMaintenance)).Observe<PartialQuantity>((Func<PartialQuantity>) (() => buffer.MonthlyNeededMaintenanceMax)).Do((Action<bool, PartialQuantity, PartialQuantity, PartialQuantity>) ((shouldDeltaBeReported, delta, monthlyNeed, monthlyNeededMax) =>
        {
          string translatedString = Tr.GlobalNeedPrefix.TranslatedString;
          desc.SetPrefixText(translatedString + " " + monthlyNeed.ToStringRounded(0) + " (" + monthlyNeededMax.ToStringRounded(0) + " MAX) / 60");
          if (!shouldDeltaBeReported)
          {
            desc.SetSuffixText("");
          }
          else
          {
            ref readonly LocStr1 local = ref Tr.LastDelta;
            Quantity integerPart = delta.IntegerPart;
            string str1 = string.Format(" {0}{1}", (object) (integerPart.IsNegative ? "" : "+"), (object) delta.IntegerPart);
            string str2 = local.Format(str1).Value;
            integerPart = delta.IntegerPart;
            if (integerPart.IsNotPositive)
              desc.SetSuffixText("  |  <color=#C2292D>" + str2 + "</color>");
            else
              desc.SetSuffixText("  |  <color=#42B336>" + str2 + "</color>");
          }
        }));
        updaterBuilder.Observe<bool>((Func<bool>) (() => this.m_unlockedProtosDb.IsUnlocked((Proto) buffer.Product))).Do((Action<bool>) (isUnlocked => itemContainer.SetItemVisibility((IUiElement) bufferView, isUnlocked)));
      }
      this.AddUpdater(updaterBuilder.Build());
    }
  }
}
