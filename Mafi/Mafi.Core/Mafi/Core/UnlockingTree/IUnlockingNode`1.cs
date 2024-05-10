// Decompiled with JetBrains decompiler
// Type: Mafi.Core.UnlockingTree.IUnlockingNode`1
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections.ImmutableCollections;

#nullable disable
namespace Mafi.Core.UnlockingTree
{
  public interface IUnlockingNode<TTreeNode> : IUnlockingNode where TTreeNode : IUnlockingNode<TTreeNode>
  {
    Vector2i GridPosition { get; }

    ImmutableArray<TTreeNode> Children { get; }

    ImmutableArray<TTreeNode> Parents { get; }

    bool AnyParentCanUnlock { get; }

    bool IsNotAvailable();

    string GetNodeId();
  }
}
