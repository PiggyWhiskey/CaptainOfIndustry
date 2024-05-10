// Decompiled with JetBrains decompiler
// Type: RTG.SphereTree`1
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class SphereTree<T>
  {
    private SphereTreeNode<T> _root;

    public SphereTree()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._root = new SphereTreeNode<T>(default (T), new Sphere(Vector3.zero, 1f));
    }

    public void DebugDraw()
    {
      Material gizmoSolidHandle = Singleton<MaterialPool>.Get.GizmoSolidHandle;
      gizmoSolidHandle.SetInt("_IsLit", 0);
      gizmoSolidHandle.SetColor(Color.green.KeepAllButAlpha(0.3f));
      gizmoSolidHandle.SetPass(0);
      this._root.DebugDraw();
    }

    public SphereTreeNode<T> AddNode(T nodeData, Sphere sphere)
    {
      SphereTreeNode<T> node = new SphereTreeNode<T>(nodeData, sphere);
      this.InsertNode(node);
      return node;
    }

    public void RemoveNode(SphereTreeNode<T> node)
    {
      if (node == this._root)
        return;
      SphereTreeNode<T> sphereTreeNode = node.Parent;
      node.SetParent((SphereTreeNode<T>) null);
      SphereTreeNode<T> parent;
      for (; sphereTreeNode != null && sphereTreeNode.NumChildren == 0 && sphereTreeNode != this._root; sphereTreeNode = parent)
      {
        parent = sphereTreeNode.Parent;
        sphereTreeNode.SetParent((SphereTreeNode<T>) null);
      }
      sphereTreeNode.EncapsulateChildrenBottomUp();
    }

    public void OnNodeSphereUpdated(SphereTreeNode<T> node)
    {
      if (!node.IsLeaf || !node.IsOutsideParent())
        return;
      SphereTreeNode<T> parent = node.Parent;
      node.SetParent((SphereTreeNode<T>) null);
      if (parent.NumChildren == 0)
        this.RemoveNode(parent);
      else
        parent.EncapsulateChildrenBottomUp();
      this.InsertNode(node);
    }

    public bool RaycastAll(Ray ray, List<SphereTreeNodeRayHit<T>> hits)
    {
      hits.Clear();
      if (this._root == null)
        return false;
      this._root.StackPush(this._root);
      while (this._root.StackTop != null)
      {
        SphereTreeNode<T> hitNode = this._root.StackPop();
        if (!hitNode.IsLeaf)
        {
          if (SphereMath.Raycast(ray, hitNode.Center, hitNode.Radius))
          {
            if (SphereMath.Raycast(ray, hitNode.Children[0].Center, hitNode.Children[0].Radius))
              this._root.StackPush(hitNode.Children[0]);
            if (hitNode.Children[1] != null && SphereMath.Raycast(ray, hitNode.Children[1].Center, hitNode.Children[1].Radius))
              this._root.StackPush(hitNode.Children[1]);
          }
        }
        else
        {
          float t;
          if (SphereMath.Raycast(ray, out t, hitNode.Center, hitNode.Radius))
            hits.Add(new SphereTreeNodeRayHit<T>(ray, hitNode, t));
        }
      }
      return hits.Count != 0;
    }

    public bool OverlapBox(OBB box, List<SphereTreeNode<T>> nodes)
    {
      nodes.Clear();
      if (this._root == null)
        return false;
      this._root.StackPush(this._root);
      while (this._root.StackTop != null)
      {
        SphereTreeNode<T> sphereTreeNode = this._root.StackPop();
        if (!sphereTreeNode.IsLeaf)
        {
          if (SphereMath.ContainsPoint(box.GetClosestPoint(sphereTreeNode.Center), sphereTreeNode.Center, sphereTreeNode.Radius))
          {
            if (SphereMath.ContainsPoint(box.GetClosestPoint(sphereTreeNode.Children[0].Center), sphereTreeNode.Children[0].Center, sphereTreeNode.Children[0].Radius))
              this._root.StackPush(sphereTreeNode.Children[0]);
            if (sphereTreeNode.Children[1] != null && SphereMath.ContainsPoint(box.GetClosestPoint(sphereTreeNode.Children[1].Center), sphereTreeNode.Children[1].Center, sphereTreeNode.Children[1].Radius))
              this._root.StackPush(sphereTreeNode.Children[1]);
          }
        }
        else if (SphereMath.ContainsPoint(box.GetClosestPoint(sphereTreeNode.Center), sphereTreeNode.Center, sphereTreeNode.Radius))
          nodes.Add(sphereTreeNode);
      }
      return nodes.Count != 0;
    }

    private void InsertNode(SphereTreeNode<T> node)
    {
      SphereTreeNode<T> newParent1;
      for (newParent1 = this._root; !newParent1.IsLeaf; newParent1 = newParent1.ClosestChild(node))
      {
        if (newParent1.NumChildren < 2)
        {
          node.SetParent(newParent1);
          newParent1.EncapsulateChildrenBottomUp();
          return;
        }
      }
      SphereTreeNode<T> newParent2 = new SphereTreeNode<T>();
      newParent2.Data = default (T);
      newParent2.Sphere = newParent1.Sphere;
      SphereTreeNode<T> parent = newParent1.Parent;
      newParent1.SetParent((SphereTreeNode<T>) null);
      newParent2.SetParent(parent);
      newParent1.SetParent(newParent2);
      node.SetParent(newParent2);
      newParent2.EncapsulateChildrenBottomUp();
      if (newParent1 != this._root)
        return;
      this._root = newParent2;
    }
  }
}
