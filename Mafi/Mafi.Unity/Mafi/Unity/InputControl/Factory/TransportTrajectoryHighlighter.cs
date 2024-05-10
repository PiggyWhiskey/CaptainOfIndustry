// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Factory.TransportTrajectoryHighlighter
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core.Factory.Transports;
using Mafi.Unity.Factory.Transports;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Factory
{
  /// <summary>
  /// Helper class that enables highlighting transport trajectories.
  /// </summary>
  public class TransportTrajectoryHighlighter
  {
    private readonly TransportModelFactory m_transportModelFactory;
    private readonly ObjectHighlighter m_highlighter;
    private readonly Dict<TransportTrajectory, ObjectHighlightSpec> m_highlightedTrajectories;

    public TransportTrajectoryHighlighter(
      TransportModelFactory modelFactory,
      ObjectHighlighter highlighter)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_highlightedTrajectories = new Dict<TransportTrajectory, ObjectHighlightSpec>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_transportModelFactory = modelFactory.CheckNotNull<TransportModelFactory>();
      this.m_highlighter = highlighter.CheckNotNull<ObjectHighlighter>();
    }

    public void HighlightTrajectory(TransportTrajectory trajectory, ColorRgba color)
    {
      ObjectHighlightSpec highlightSpec;
      GameObject go;
      if (this.m_highlightedTrajectories.TryGetValue(trajectory, out highlightSpec))
      {
        if (highlightSpec.Color == color)
          return;
        this.m_highlighter.RemoveHighlight(highlightSpec);
        go = highlightSpec.Go;
      }
      else
        go = this.m_transportModelFactory.CreateModel(trajectory, true, true, true);
      this.m_highlightedTrajectories[trajectory] = new ObjectHighlightSpec(go, color);
      this.m_highlighter.HighlightNewAndAssert(go, color);
      go.GetComponent<MeshRenderer>().enabled = false;
    }

    public void ClearHighlightedTrajectory(TransportTrajectory trajectory)
    {
      ObjectHighlightSpec objectHighlightSpec;
      if (!this.m_highlightedTrajectories.TryRemove(trajectory, out objectHighlightSpec))
        return;
      this.m_highlighter.RemoveHighlightAndAssert(objectHighlightSpec.Go, objectHighlightSpec.Color);
      objectHighlightSpec.Go.Destroy();
    }

    public void ClearAllHighlights()
    {
      foreach (ObjectHighlightSpec objectHighlightSpec in this.m_highlightedTrajectories.Values)
      {
        this.m_highlighter.RemoveHighlightAndAssert(objectHighlightSpec.Go, objectHighlightSpec.Color);
        objectHighlightSpec.Go.Destroy();
      }
      this.m_highlightedTrajectories.Clear();
    }
  }
}
