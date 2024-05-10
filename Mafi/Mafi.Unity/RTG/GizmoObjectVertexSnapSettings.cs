// Decompiled with JetBrains decompiler
// Type: RTG.GizmoObjectVertexSnapSettings
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  [Serializable]
  public class GizmoObjectVertexSnapSettings : Settings
  {
    [SerializeField]
    private int _snapDestinationLayers;
    [SerializeField]
    private bool _canSnapToGrid;
    [SerializeField]
    private bool _canSnapToObjectVerts;

    public int SnapDestinationLayers
    {
      get => this._snapDestinationLayers;
      set => this._snapDestinationLayers = value;
    }

    public bool CanSnapToGrid
    {
      get => this._canSnapToGrid;
      set => this._canSnapToGrid = value;
    }

    public bool CanSnapToObjectVerts
    {
      get => this._canSnapToObjectVerts;
      set => this._canSnapToObjectVerts = value;
    }

    public bool IsLayerSnapDestination(int objectLayer)
    {
      return LayerEx.IsLayerBitSet(this._snapDestinationLayers, objectLayer);
    }

    public void SetLayerSnapDestination(int objectLayer, bool isSnapDestination)
    {
      if (isSnapDestination)
        this._snapDestinationLayers = LayerEx.SetLayerBit(this._snapDestinationLayers, objectLayer);
      else
        this._snapDestinationLayers = LayerEx.ClearLayerBit(this._snapDestinationLayers, objectLayer);
    }

    public void Transfer(GizmoObjectVertexSnapSettings destination)
    {
      destination.SnapDestinationLayers = this.SnapDestinationLayers;
      destination.CanSnapToGrid = this.CanSnapToGrid;
      destination.CanSnapToObjectVerts = this.CanSnapToObjectVerts;
    }

    public GizmoObjectVertexSnapSettings()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._snapDestinationLayers = -1;
      this._canSnapToGrid = true;
      this._canSnapToObjectVerts = true;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
