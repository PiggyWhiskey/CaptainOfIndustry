// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Components.UnlockingTreeView.UnlockingTreeEditor`1
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.UnlockingTree;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UserInterface.Components.UnlockingTreeView
{
  /// <summary>
  /// Research tree editor that is supposed to be used in the debug mode.
  /// </summary>
  internal class UnlockingTreeEditor<TTreeNode> : IUnlockingTreeEditor<TTreeNode> where TTreeNode : IUnlockingNode<TTreeNode>
  {
    private readonly Action m_onRebuildNeeded;
    private readonly string m_genCodeUpdatePath;
    private readonly Dict<TTreeNode, Vector2i> m_positionsMap;
    private readonly Dict<TTreeNode, Lyst<TTreeNode>> m_childrenMap;

    internal UnlockingTreeEditor(
      ImmutableArray<TTreeNode> nodes,
      Action onRebuildNeeded,
      string genCodeUpdatePath = "")
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_positionsMap = new Dict<TTreeNode, Vector2i>();
      this.m_childrenMap = new Dict<TTreeNode, Lyst<TTreeNode>>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_onRebuildNeeded = onRebuildNeeded;
      this.m_genCodeUpdatePath = genCodeUpdatePath;
      foreach (TTreeNode node in nodes)
        this.m_positionsMap[node] = node.GridPosition;
      foreach (TTreeNode node in nodes)
        this.m_childrenMap[node] = new Lyst<TTreeNode>(node.Children.AsEnumerable());
    }

    public void BuildUi(UiBuilder builder, IUiElement parent)
    {
      builder.NewBtn("GeneratePos").SetText("GenPos").OnClick(new Action(this.genCode)).SetButtonStyle(builder.Style.Global.PrimaryBtn).PutToRightBottomOf<Btn>(parent, new Vector2(100f, 20f));
    }

    public void InputUpdate(
      Option<IUnlockingTreeNodeView<TTreeNode>> selectedNodeView)
    {
      if (selectedNodeView.IsNone)
        return;
      Vector2i change;
      if (Input.GetKey(KeyCode.LeftArrow))
        change = new Vector2i(-1, 0);
      else if (Input.GetKey(KeyCode.RightArrow))
        change = new Vector2i(1, 0);
      else if (Input.GetKey(KeyCode.UpArrow))
      {
        change = new Vector2i(0, -1);
      }
      else
      {
        if (!Input.GetKey(KeyCode.DownArrow))
          return;
        change = new Vector2i(0, 1);
      }
      if (Input.GetKey(KeyCode.LeftShift))
        change *= 10;
      if (Input.GetKey(KeyCode.LeftControl))
        change *= 2;
      this.moveNode(selectedNodeView.Value.Node, change, Input.GetKey(KeyCode.LeftControl), new Set<TTreeNode>());
      this.m_onRebuildNeeded();
    }

    public Vector2i GetNodeGridPos(TTreeNode node) => this.m_positionsMap[node];

    public IEnumerable<TTreeNode> GetChildrenOf(TTreeNode node)
    {
      return (IEnumerable<TTreeNode>) this.m_childrenMap[node];
    }

    private void moveNode(
      TTreeNode node,
      Vector2i change,
      bool recursively,
      Set<TTreeNode> alreadyMoved)
    {
      if (alreadyMoved.Contains(node))
        return;
      Vector2i positions = this.m_positionsMap[node];
      this.m_positionsMap[node] = positions + change;
      alreadyMoved.Add(node);
      if (!recursively)
        return;
      this.m_childrenMap[node].ForEach((Action<TTreeNode>) (x => this.moveNode(x, change, true, alreadyMoved)));
    }

    public bool NodeClicked(
      IUnlockingTreeNodeView<TTreeNode> nodeView,
      Option<IUnlockingTreeNodeView<TTreeNode>> selectedNodeView)
    {
      if (!Input.GetKey(KeyCode.LeftControl) || selectedNodeView.IsNone)
        return false;
      if (this.m_childrenMap[selectedNodeView.Value.Node].Contains(nodeView.Node))
      {
        this.m_childrenMap[selectedNodeView.Value.Node].Remove(nodeView.Node);
        this.m_onRebuildNeeded();
        return true;
      }
      this.m_childrenMap[selectedNodeView.Value.Node].Add(nodeView.Node);
      this.m_onRebuildNeeded();
      return true;
    }

    private void genCode()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("// GENERATED CODE BEGIN");
      foreach (KeyValuePair<TTreeNode, Vector2i> positions in this.m_positionsMap)
      {
        Vector2i vector2i = positions.Value;
        stringBuilder.AppendLine(string.Format("setPosition({0}, new Vector2i({1}, {2}));", (object) positions.Key.GetNodeId(), (object) vector2i.X, (object) vector2i.Y));
      }
      stringBuilder.AppendLine();
      foreach (KeyValuePair<TTreeNode, Lyst<TTreeNode>> children in this.m_childrenMap)
      {
        TTreeNode key = children.Key;
        foreach (TTreeNode treeNode in children.Value)
          stringBuilder.AppendLine("setParent(parent: " + key.GetNodeId() + ", of: " + treeNode.GetNodeId() + ");");
      }
      stringBuilder.Append("// GENERATED CODE END");
      if (File.Exists(this.m_genCodeUpdatePath))
      {
        string str = File.ReadAllText(this.m_genCodeUpdatePath);
        int num1 = str.IndexOf("// GENERATED CODE BEGIN", StringComparison.Ordinal);
        int num2 = str.IndexOf("// GENERATED CODE END", num1, StringComparison.Ordinal);
        if (num1 >= 0 && num2 > num1)
        {
          File.WriteAllText(this.m_genCodeUpdatePath, str.Substring(0, num1) + stringBuilder?.ToString() + str.Substring(num2 + "// GENERATED CODE END".Length));
          Log.Error("New data written to: " + this.m_genCodeUpdatePath);
          return;
        }
        Log.Error("Failed to locate flowing gen code markings in file '" + this.m_genCodeUpdatePath + "' :\n// GENERATED CODE BEGIN\n// GENERATED CODE END");
        Log.Error("Failed to locate flowing gen code markings in file '" + this.m_genCodeUpdatePath + "' (full path: " + Path.GetFullPath(this.m_genCodeUpdatePath) + "):\n// GENERATED CODE BEGIN\n// GENERATED CODE END");
      }
      Log.Error((string.IsNullOrEmpty(this.m_genCodeUpdatePath) ? "No gen code file specified." : "Non-existent gen file" + this.m_genCodeUpdatePath) + ", current location: " + Path.GetFullPath("./"));
      Log.Error(stringBuilder.ToString());
    }
  }
}
