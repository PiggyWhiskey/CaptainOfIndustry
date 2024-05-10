// Decompiled with JetBrains decompiler
// Type: RTG.SphereTreeNode`1
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class SphereTreeNode<T>
  {
    private SphereTreeNode<T> _stackTop;
    private SphereTreeNode<T> _stackPrevious;
    private Sphere _sphere;
    private T _data;
    private SphereTreeNode<T> _parent;
    private SphereTreeNode<T>[] _children;
    private int _numChildren;

    public SphereTreeNode<T>[] Children => this._children;

    public int NumChildren => this._numChildren;

    public bool IsLeaf => (object) this._data != null;

    public SphereTreeNode()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._children = new SphereTreeNode<T>[2];
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._sphere = new Sphere(Vector3.zero, 1f);
      this._data = default (T);
    }

    public SphereTreeNode(T data, Sphere sphere)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._children = new SphereTreeNode<T>[2];
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._sphere = sphere;
      this._data = data;
    }

    public Sphere Sphere
    {
      get => this._sphere;
      set => this._sphere = value;
    }

    public Vector3 Center
    {
      get => this._sphere.Center;
      set => this._sphere.Center = value;
    }

    public float Radius
    {
      get => this._sphere.Radius;
      set => this._sphere.Radius = value;
    }

    public SphereTreeNode<T> Parent => this._parent;

    public T Data
    {
      get => this._data;
      set => this._data = value;
    }

    public SphereTreeNode<T> StackTop => this._stackTop;

    public void StackPush(SphereTreeNode<T> node)
    {
      node._stackPrevious = this._stackTop;
      this._stackTop = node;
    }

    public SphereTreeNode<T> StackPop()
    {
      SphereTreeNode<T> stackTop = this._stackTop;
      this._stackTop = stackTop._stackPrevious;
      return stackTop;
    }

    /// <summary>
    /// Checks if the node's sphere is outside of its parent's
    /// sphere. The method returns false if the node doesn't have
    /// a parent.
    /// </summary>
    public bool IsOutsideParent()
    {
      return this._parent != null && (double) ((this.Center - this._parent.Center).magnitude + this.Radius) > (double) this._parent.Radius;
    }

    /// <summary>Finds the child which is closest to 'node'.</summary>
    /// <returns>
    /// The child closest to 'node' or null if the no children
    /// are available.
    /// </returns>
    public SphereTreeNode<T> ClosestChild(SphereTreeNode<T> node)
    {
      if (this._numChildren == 0)
        return (SphereTreeNode<T>) null;
      SphereTreeNode<T> child = this._children[0];
      float magnitude = (node.Center - this._children[0].Center).magnitude;
      if (this._children[1] != null && (double) (node.Center - this._children[1].Center).magnitude < (double) magnitude)
        child = this._children[1];
      return child;
    }

    /// <summary>
    /// Sets the node's parent. This call is ignored if the specified parent
    /// is the node itself or if it's the same as the current parent.
    /// </summary>
    public void SetParent(SphereTreeNode<T> newParent)
    {
      if (newParent == this || newParent == this._parent)
        return;
      if (this._parent != null)
      {
        if (this._parent._children[0] == this)
        {
          this._parent._children[0] = this._parent._children[1];
          this._parent._children[1] = (SphereTreeNode<T>) null;
        }
        else
          this._parent._children[1] = (SphereTreeNode<T>) null;
        --this._parent._numChildren;
        this._parent = (SphereTreeNode<T>) null;
      }
      if (newParent != null)
      {
        this._parent = newParent;
        this._parent._children[this._parent._numChildren++] = this;
      }
      else
        this._parent = (SphereTreeNode<T>) null;
    }

    /// <summary>
    /// This method will recalculate the node's center and radius
    /// so that it encapsulates all children. This is a recursive
    /// call which propagates up the hierarchy towards the root.
    /// </summary>
    public void EncapsulateChildrenBottomUp()
    {
      if (this.NumChildren == 0)
        return;
      for (SphereTreeNode<T> sphereTreeNode = this; sphereTreeNode != null; sphereTreeNode = sphereTreeNode.Parent)
      {
        Vector3 center = sphereTreeNode._children[0].Center;
        if (sphereTreeNode._children[1] != null)
          center += sphereTreeNode._children[1].Center;
        sphereTreeNode.Center = center * (1f / (float) sphereTreeNode.NumChildren);
        float num1 = (sphereTreeNode._children[0].Center - sphereTreeNode.Center).magnitude + sphereTreeNode._children[0].Radius;
        if (sphereTreeNode._children[1] != null)
        {
          float num2 = (sphereTreeNode._children[1].Center - sphereTreeNode.Center).magnitude + sphereTreeNode._children[1].Radius;
          if ((double) num2 > (double) num1)
            num1 = num2;
        }
        sphereTreeNode.Radius = num1;
      }
    }

    /// <summary>
    /// Allows the node to render itself for debugging purposes. The client
    /// code is responsible for setting up the rendering material.
    /// </summary>
    /// <remarks>
    /// This method is recursive and will draw the node's children also. Thus,
    /// it is enough to call this method for the root of a sphere tree in order
    /// to draw the entire tree.
    /// </remarks>
    public void DebugDraw()
    {
      Graphics.DrawMeshNow(Singleton<MeshPool>.Get.UnitSphere, Matrix4x4.TRS(this._sphere.Center, Quaternion.identity, Vector3Ex.FromValue(this._sphere.Radius)));
      foreach (SphereTreeNode<T> child in this._children)
        child.DebugDraw();
    }
  }
}
