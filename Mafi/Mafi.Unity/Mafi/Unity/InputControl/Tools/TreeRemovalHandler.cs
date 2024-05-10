// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Tools.TreeRemovalHandler
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Input;
using Mafi.Core.Terrain.Trees;
using Mafi.Localization;
using Mafi.Unity.Trees;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Tools
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  internal sealed class TreeRemovalHandler
  {
    private readonly ITreePlantingManager m_treePlantingManager;
    private readonly TreeRemovePlanRenderer m_highlightRenderer;
    private TreeId? m_treeToValidate;
    private bool m_lastValidationResult;

    public int Priority => 100;

    public TreeRemovalHandler(
      ITreePlantingManager treePlantingManager,
      NewInstanceOf<TreeRemovePlanRenderer> highlightRenderer)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_treePlantingManager = treePlantingManager;
      this.m_highlightRenderer = highlightRenderer.Instance;
    }

    public bool TryHandleHover(TreeId? treeUnderCursor, out LocStrFormatted errorMsg)
    {
      if (!treeUnderCursor.HasValue)
      {
        errorMsg = LocStrFormatted.Empty;
        this.m_highlightRenderer.ClearAllHighlights();
        return false;
      }
      TreeId tree = treeUnderCursor.Value;
      TreeId treeId = tree;
      TreeId? treeToValidate = this.m_treeToValidate;
      if ((treeToValidate.HasValue ? (treeId != treeToValidate.GetValueOrDefault() ? 1 : 0) : 1) != 0)
      {
        this.m_treeToValidate = new TreeId?(tree);
        this.m_lastValidationResult = false;
        this.m_highlightRenderer.ClearAllHighlights();
        this.m_highlightRenderer.AddHighlight(tree, DeleteEntityInputController.COLOR_HIGHLIGHT);
        errorMsg = LocStrFormatted.Empty;
      }
      else
        errorMsg = LocStrFormatted.Empty;
      return true;
    }

    public bool TryHandleMouseDown(TreeId? treeUnderCursor)
    {
      TreeId? nullable = treeUnderCursor;
      TreeId? treeToValidate = this.m_treeToValidate;
      if (nullable.HasValue != treeToValidate.HasValue)
        return false;
      return !nullable.HasValue || nullable.GetValueOrDefault() == treeToValidate.GetValueOrDefault();
    }

    public bool TryHandleMouseUp(
      TreeId? treeUnderCursor,
      Action<IInputCommand> scheduleCommand,
      bool useQuickRemove)
    {
      if (this.m_treeToValidate.HasValue)
      {
        TreeId? nullable = treeUnderCursor;
        TreeId? treeToValidate = this.m_treeToValidate;
        if ((nullable.HasValue == treeToValidate.HasValue ? (nullable.HasValue ? (nullable.GetValueOrDefault() != treeToValidate.GetValueOrDefault() ? 1 : 0) : 0) : 1) == 0)
        {
          scheduleCommand((IInputCommand) new RemoveManualPlantTreeCmd(this.m_treeToValidate.Value));
          return false;
        }
      }
      return false;
    }

    public bool TryHandleCancel() => false;

    public void Sync()
    {
      if (this.m_lastValidationResult || !this.m_treeToValidate.HasValue)
        return;
      this.m_lastValidationResult = this.m_treePlantingManager.HasReservedManualTree((Tile2i) this.m_treeToValidate.Value.Position);
    }

    public void Deactivate()
    {
      this.m_treeToValidate = new TreeId?();
      this.m_lastValidationResult = false;
      this.m_highlightRenderer.ClearAllHighlights();
    }
  }
}
