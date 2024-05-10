// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Components.UnlockingTreeView.NoOpUnlockingTreeEditor`1
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.UnlockingTree;
using Mafi.Unity.UiFramework;
using System.Collections.Generic;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UserInterface.Components.UnlockingTreeView
{
  /// <summary>
  /// No-op implementation of the editor for production version.
  /// </summary>
  internal class NoOpUnlockingTreeEditor<TTreeNode> : IUnlockingTreeEditor<TTreeNode> where TTreeNode : IUnlockingNode<TTreeNode>
  {
    public void BuildUi(UiBuilder builder, IUiElement parent)
    {
    }

    public void InputUpdate(
      Option<IUnlockingTreeNodeView<TTreeNode>> selectedNodeView)
    {
    }

    public Vector2i GetNodeGridPos(TTreeNode node) => node.GridPosition;

    public IEnumerable<TTreeNode> GetChildrenOf(TTreeNode node) => node.Children.AsEnumerable();

    public bool NodeClicked(
      IUnlockingTreeNodeView<TTreeNode> nodeView,
      Option<IUnlockingTreeNodeView<TTreeNode>> selectedNodeView)
    {
      return false;
    }

    public NoOpUnlockingTreeEditor()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
