// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Components.UiUnLockerForTech
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Prototypes;
using Mafi.Unity.InputControl;
using Mafi.Unity.InputControl.Inspectors;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UserInterface.Components
{
  public class UiUnLockerForTech
  {
    private readonly UnlockedProtosDbForUi m_unlockedProtosDb;
    private readonly ProtosDb m_protosDb;
    private Proto m_techToCheck;
    private StackContainer m_parentContainer;
    private IUiElement[] m_items;

    public UiUnLockerForTech(InspectorContext context)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      this.\u002Ector(context.UnlockedProtosDbForUi, context.ProtosDb);
    }

    public UiUnLockerForTech(UnlockedProtosDbForUi unlockedProtosDb, ProtosDb protosDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_unlockedProtosDb = unlockedProtosDb;
      this.m_protosDb = protosDb;
    }

    public void SetupVisibilityHook(
      Proto.ID techToCheck,
      StackContainer parentContainer,
      params IUiElement[] items)
    {
      Option<Proto> option = this.m_protosDb.Get<Proto>(techToCheck);
      if (option.IsNone)
        Log.Error(string.Format("Technology {0} was not found!", (object) techToCheck));
      this.m_techToCheck = option.Value;
      this.m_parentContainer = parentContainer;
      this.m_items = items;
      if (this.m_unlockedProtosDb.IsUnlocked((IProto) this.m_techToCheck))
        return;
      parentContainer.StartBatchOperation();
      for (int index = 0; index < items.Length; ++index)
        parentContainer.HideItem(items[index]);
      parentContainer.FinishBatchOperation();
      this.m_unlockedProtosDb.OnUnlockedSetChangedForUi += new Action(this.onUnlockedChanged);
    }

    private void onUnlockedChanged()
    {
      if (!this.m_unlockedProtosDb.IsUnlocked((IProto) this.m_techToCheck))
        return;
      this.m_parentContainer.StartBatchOperation();
      for (int index = 0; index < this.m_items.Length; ++index)
        this.m_parentContainer.ShowItem(this.m_items[index]);
      this.m_parentContainer.FinishBatchOperation();
      this.m_unlockedProtosDb.OnUnlockedSetChangedForUi -= new Action(this.onUnlockedChanged);
    }
  }
}
