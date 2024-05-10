// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Research.NodeColorsSelector
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Research;
using Mafi.Unity.UserInterface.Style;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Research
{
  internal struct NodeColorsSelector
  {
    private readonly UiStyle m_style;
    private readonly ColorRgba[] m_titleTextClrs;
    private readonly ColorRgba[] m_titleBgClrs;

    public NodeColorsSelector(UiStyle style)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_style = style;
      this.m_titleTextClrs = style.Research.GetTitleTextClrs();
      this.m_titleBgClrs = style.Research.GetTitleBgClrs();
    }

    [Pure]
    public ColorRgba ResolveTitleBgColor(bool isBlocked, ResearchNode.InfoForUi nodeInfo)
    {
      if (isBlocked)
        return this.m_style.Research.NodeBlockedTitleBgClr;
      if (nodeInfo.CanBeEnqueuedDirect)
        return this.m_style.Research.NodeCanBeQueuedTitleBgClr;
      if (nodeInfo.IsInQueue)
        return this.m_style.Research.NodeInQueueTitleBgClr;
      if (!nodeInfo.CanBeEnqueued && nodeInfo.State == ResearchNodeState.NotResearched)
        return this.m_style.Research.NodeCanNotBeQueuedTitleBgClr;
      if (nodeInfo.IsLockedByCondition)
        return this.m_style.Research.NodeLockedByConditionTitleBgClr;
      return nodeInfo.IsLockedByParents ? this.m_style.Research.NodeLockedByParentsTitleBgClr : this.m_titleBgClrs[(int) nodeInfo.State];
    }

    [Pure]
    public ColorRgba ResolveTitleTextColor(bool isBlocked, ResearchNode.InfoForUi nodeInfo)
    {
      if (isBlocked)
        return this.m_style.Research.NodeBlockedTitleClr;
      if (nodeInfo.CanBeEnqueuedDirect)
        return this.m_style.Research.NodeCanBeQueuedTitleClr;
      if (nodeInfo.IsInQueue)
        return this.m_style.Research.NodeInQueueTitleClr;
      if (!nodeInfo.CanBeEnqueued && nodeInfo.State == ResearchNodeState.NotResearched)
        return this.m_style.Research.NodeCanNotBeQueuedTitleClr;
      if (nodeInfo.IsLockedByCondition)
        return this.m_style.Research.NodeLockedByConditionTitleClr;
      return nodeInfo.IsLockedByParents ? this.m_style.Research.NodeLockedByParentsTitleClr : this.m_titleTextClrs[(int) nodeInfo.State];
    }
  }
}
