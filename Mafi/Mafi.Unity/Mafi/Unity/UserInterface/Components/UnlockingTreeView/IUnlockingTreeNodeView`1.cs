// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Components.UnlockingTreeView.IUnlockingTreeNodeView`1
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.UnlockingTree;
using Mafi.Unity.UiFramework;
using System;

#nullable disable
namespace Mafi.Unity.UserInterface.Components.UnlockingTreeView
{
  public interface IUnlockingTreeNodeView<TTreeNode> : IUiElement where TTreeNode : IUnlockingNode<TTreeNode>
  {
    event Action<IUnlockingTreeNodeView<TTreeNode>> Click;

    event Action<IUnlockingTreeNodeView<TTreeNode>> RightClick;

    event Action<IUnlockingTreeNodeView<TTreeNode>> DoubleClick;

    TTreeNode Node { get; }

    void SetSelected(bool selected);

    void SetHighlightedAsParent(bool highlighted);

    void SetMarkAsSearchResult(bool isMarked);

    void SyncUpdate();

    void RenderUpdate();

    bool Matches(string[] query);
  }
}
