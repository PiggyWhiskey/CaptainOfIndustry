// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Factory.Transports.TransportFlowIndicatorMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Factory.Transports;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Factory.Transports
{
  internal class TransportFlowIndicatorMb : MonoBehaviour
  {
    private FluidIndicatorState m_state;
    private float m_targetFlowRate;
    private Color m_targetColor;

    public int IndicatorIndex { get; private set; }

    public int SegmentIndex { get; private set; }

    public void Initialize(
      int indicatorIndex,
      TransportProto.Gfx.FlowIndicatorSpec indicatorProto,
      int segmentIndex)
    {
      this.IndicatorIndex = indicatorIndex.CheckNotNegative();
      this.SegmentIndex = segmentIndex.CheckNotNegative();
      Transform resultTransform;
      MeshRenderer component;
      if (this.transform.TryFindChild(indicatorProto.FlowPrefabPath, out resultTransform) && resultTransform.TryGetComponent<MeshRenderer>(out component))
        this.m_state = new FluidIndicatorState(component, indicatorProto.Parameters);
      else
        Log.Error("No flow meshes found on flow indicator.");
    }

    /// <summary>
    /// Sets flow rate (invoked on sim thread). This won't be applied until Sync.
    /// </summary>
    public void SetFlowRate(RelTile1f tilesPerTick, ColorRgba color)
    {
      this.m_targetFlowRate = tilesPerTick.ToUnityUnits();
      this.m_targetColor = color.ToColor();
    }

    public void SyncUpdate()
    {
      this.m_state?.SetFlow(this.m_targetFlowRate);
      this.m_state?.SetColor(this.m_targetColor);
    }

    public void RenderUpdate(GameTime time) => this.m_state?.RenderUpdate(time);

    public void SkipTransition() => this.m_state?.SkipTransition();

    public TransportFlowIndicatorMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
