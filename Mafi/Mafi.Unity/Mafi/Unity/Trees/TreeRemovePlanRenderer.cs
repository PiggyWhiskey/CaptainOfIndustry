// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Trees.TreeRemovePlanRenderer
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core.Terrain.Trees;
using Mafi.Unity.Terrain;
using System.Collections.Generic;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Trees
{
  /// <summary>
  /// Renders the highlights for tree removal interface.
  /// Trees that are planned to be removed by the tool are highlighted.
  /// 
  /// </summary>
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class TreeRemovePlanRenderer
  {
    private readonly ITreeRenderer m_treeRenderer;
    private readonly Dict<TreeId, TreeRemovePlanRenderer.TreeHighlightState> m_treeHighlights;

    internal TreeRemovePlanRenderer(ITreeRenderer treeRenderer)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_treeHighlights = new Dict<TreeId, TreeRemovePlanRenderer.TreeHighlightState>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_treeRenderer = treeRenderer;
    }

    public void AddHighlight(TreeId tree, ColorRgba color)
    {
      this.getHighlightState(tree).AddHighlight(this.m_treeRenderer, color);
    }

    public void RemoveHighlight(TreeId tree)
    {
      this.getHighlightState(tree).RemoveHighlight(this.m_treeRenderer);
    }

    public void ClearAllHighlights()
    {
      foreach (KeyValuePair<TreeId, TreeRemovePlanRenderer.TreeHighlightState> treeHighlight in this.m_treeHighlights)
        treeHighlight.Value.RemoveHighlight(this.m_treeRenderer);
      this.m_treeHighlights.Clear();
    }

    private TreeRemovePlanRenderer.TreeHighlightState getHighlightState(TreeId tree)
    {
      TreeRemovePlanRenderer.TreeHighlightState highlightState1;
      if (this.m_treeHighlights.TryGetValue(tree, out highlightState1))
        return highlightState1;
      TreeRemovePlanRenderer.TreeHighlightState highlightState2 = new TreeRemovePlanRenderer.TreeHighlightState(tree);
      this.m_treeHighlights.Add(tree, highlightState2);
      return highlightState2;
    }

    private class TreeHighlightState
    {
      private readonly TreeId m_tree;
      private Option<TreeMb> m_treeMb;
      private ColorRgba m_color;
      private bool m_isHighlighted;

      public TreeHighlightState(TreeId tree)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_tree = tree;
        this.m_color = ColorRgba.Empty;
      }

      public void RemoveHighlight(ITreeRenderer treeRenderer)
      {
        if (!this.m_isHighlighted || this.m_treeMb.Value.IsNullOrDestroyed())
          return;
        treeRenderer.RemoveTreeHighlight(this.m_treeMb.Value, this.m_color);
        this.m_isHighlighted = false;
      }

      public void AddHighlight(ITreeRenderer treeRenderer, ColorRgba color)
      {
        if (this.m_isHighlighted)
          return;
        if (this.m_treeMb.IsNone)
          this.m_treeMb = treeRenderer.GetPlannedMbOrNoneFor(this.m_tree);
        if (!this.m_treeMb.HasValue || this.m_treeMb.Value.IsNullOrDestroyed())
          return;
        this.m_color = color;
        treeRenderer.HighlightTree(this.m_treeMb.Value, color);
        this.m_isHighlighted = true;
      }
    }
  }
}
