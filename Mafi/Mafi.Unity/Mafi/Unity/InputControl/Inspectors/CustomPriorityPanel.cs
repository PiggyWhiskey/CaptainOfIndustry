// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.CustomPriorityPanel
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Entities.Commands;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Input;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UserInterface;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors
{
  public class CustomPriorityPanel : BasePriorityPanel
  {
    private readonly IInputScheduler m_inputScheduler;
    private readonly Func<IEntityWithCustomPriority> m_provider;
    private readonly string m_priorityId;

    public static CustomPriorityPanel NewForStorageImport(
      IUiElement parent,
      IInputScheduler inputScheduler,
      UiBuilder builder,
      Func<IEntityWithCustomPriority> provider)
    {
      CustomPriorityPanel customPriorityPanel = new CustomPriorityPanel(parent, inputScheduler, builder, provider, "ImportPrio", (LocStrFormatted) Tr.ImportPriority, (LocStrFormatted) Tr.ImportPriority__StorageTooltip);
      customPriorityPanel.SetCustomIcon("Assets/Unity/UserInterface/General/Import128.png");
      customPriorityPanel.SetColor(builder.Style.Global.GreenForDark);
      return customPriorityPanel;
    }

    public static CustomPriorityPanel NewForStorageExport(
      IUiElement parent,
      IInputScheduler inputScheduler,
      UiBuilder builder,
      Func<IEntityWithCustomPriority> provider)
    {
      CustomPriorityPanel customPriorityPanel = new CustomPriorityPanel(parent, inputScheduler, builder, provider, "ExportPrio", (LocStrFormatted) Tr.ExportPriority, (LocStrFormatted) Tr.ExportPriority__StorageTooltip);
      customPriorityPanel.SetCustomIcon("Assets/Unity/UserInterface/General/Export128.png");
      customPriorityPanel.SetColor(builder.Style.Global.DangerClr);
      return customPriorityPanel;
    }

    public static CustomPriorityPanel NewForShipFuelImport(
      IUiElement parent,
      IInputScheduler inputScheduler,
      UiBuilder builder,
      Func<IEntityWithCustomPriority> provider)
    {
      CustomPriorityPanel customPriorityPanel = new CustomPriorityPanel(parent, inputScheduler, builder, provider, "FuelImportPrio", (LocStrFormatted) Tr.ImportPriority, (LocStrFormatted) Tr.ImportPriority__ShipFuelTooltip);
      customPriorityPanel.SetCustomIcon("Assets/Unity/UserInterface/General/Import128.png");
      customPriorityPanel.SetColor(builder.Style.Global.GreenForDark);
      return customPriorityPanel;
    }

    public static CustomPriorityPanel NewForShipFuelExport(
      IUiElement parent,
      IInputScheduler inputScheduler,
      UiBuilder builder,
      Func<IEntityWithCustomPriority> provider)
    {
      CustomPriorityPanel customPriorityPanel = new CustomPriorityPanel(parent, inputScheduler, builder, provider, "FuelExportPrio", (LocStrFormatted) Tr.ExportPriority, (LocStrFormatted) Tr.ExportPriority__ShipFuelTooltip);
      customPriorityPanel.SetCustomIcon("Assets/Unity/UserInterface/General/Export128.png");
      customPriorityPanel.SetColor(builder.Style.Global.DangerClr);
      return customPriorityPanel;
    }

    public static CustomPriorityPanel NewForShipyardStoredCargo(
      IUiElement parent,
      IInputScheduler inputScheduler,
      UiBuilder builder,
      Func<IEntityWithCustomPriority> provider)
    {
      CustomPriorityPanel customPriorityPanel = new CustomPriorityPanel(parent, inputScheduler, builder, provider, "CargoExportPrio", (LocStrFormatted) Tr.ExportPriority, (LocStrFormatted) Tr.ExportPriority__ShipyardCargo);
      customPriorityPanel.SetCustomIcon("Assets/Unity/UserInterface/General/Export128.png");
      customPriorityPanel.SetColor(builder.Style.Global.DangerClr);
      return customPriorityPanel;
    }

    public static CustomPriorityPanel NewForTradeDockCargo(
      IUiElement parent,
      IInputScheduler inputScheduler,
      UiBuilder builder,
      Func<IEntityWithCustomPriority> provider)
    {
      CustomPriorityPanel customPriorityPanel = new CustomPriorityPanel(parent, inputScheduler, builder, provider, "CargoExportPrio", (LocStrFormatted) Tr.ExportPriority, (LocStrFormatted) Tr.ExportPriority__ShipyardCargo);
      customPriorityPanel.SetCustomIcon("Assets/Unity/UserInterface/General/Export128.png");
      customPriorityPanel.SetColor(builder.Style.Global.DangerClr);
      return customPriorityPanel;
    }

    public static CustomPriorityPanel NewForShipRepairImport(
      IUiElement parent,
      IInputScheduler inputScheduler,
      UiBuilder builder,
      Func<IEntityWithCustomPriority> provider)
    {
      CustomPriorityPanel customPriorityPanel = new CustomPriorityPanel(parent, inputScheduler, builder, provider, "ShipRepairImportPrio", (LocStrFormatted) Tr.ImportPriority, (LocStrFormatted) Tr.ImportPriority__ShipRepairTooltip);
      customPriorityPanel.SetCustomIcon("Assets/Unity/UserInterface/General/Import128.png");
      customPriorityPanel.SetColor(builder.Style.Global.GreenForDark);
      return customPriorityPanel;
    }

    public static CustomPriorityPanel NewForShipyardWorldCargoImport(
      IUiElement parent,
      IInputScheduler inputScheduler,
      UiBuilder builder,
      Func<IEntityWithCustomPriority> provider)
    {
      CustomPriorityPanel customPriorityPanel = new CustomPriorityPanel(parent, inputScheduler, builder, provider, "WorldCargoImportPrio", (LocStrFormatted) Tr.ImportPriority, (LocStrFormatted) Tr.ImportPriority__ShipCargoTooltip);
      customPriorityPanel.SetCustomIcon("Assets/Unity/UserInterface/General/Import128.png");
      customPriorityPanel.SetColor(builder.Style.Global.GreenForDark);
      return customPriorityPanel;
    }

    public CustomPriorityPanel(
      IUiElement parent,
      IInputScheduler inputScheduler,
      UiBuilder builder,
      Func<IEntityWithCustomPriority> provider,
      string priorityId,
      LocStrFormatted title,
      LocStrFormatted tooltip)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(parent, builder, 14, title.Value);
      this.m_inputScheduler = inputScheduler;
      this.m_provider = provider;
      this.m_priorityId = priorityId;
      this.SetTooltip(tooltip);
    }

    protected override void OnValueChanged(int index)
    {
      this.m_inputScheduler.ScheduleInputCmd<SetCustomPriorityCmd>(new SetCustomPriorityCmd(this.m_provider().Id, this.m_priorityId, index));
    }

    protected override int GetCurrentPriority()
    {
      return this.m_provider().GetCustomPriority(this.m_priorityId);
    }

    protected override bool IsPrioritySupported()
    {
      return this.m_provider().IsCustomPriorityVisible(this.m_priorityId);
    }
  }
}
