// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Components.UnlockingTreeView.IUnlockingTreeEditor`1
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.UnlockingTree;
using Mafi.Unity.UiFramework;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Unity.UserInterface.Components.UnlockingTreeView
{
  /// <summary>
  /// Interface for an editor to be used to edit the research tree during the runtime.
  /// </summary>
  internal interface IUnlockingTreeEditor<TTreeNode> where TTreeNode : IUnlockingNode<TTreeNode>
  {
    void BuildUi(UiBuilder builder, IUiElement parent);

    void InputUpdate(
      Option<IUnlockingTreeNodeView<TTreeNode>> selectedNodeView);

    Vector2i GetNodeGridPos(TTreeNode node);

    IEnumerable<TTreeNode> GetChildrenOf(TTreeNode node);

    bool NodeClicked(
      IUnlockingTreeNodeView<TTreeNode> nodeView,
      Option<IUnlockingTreeNodeView<TTreeNode>> selectedNodeView);
  }
}
