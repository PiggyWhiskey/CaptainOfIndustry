// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InstancedRendering.IRenderedChunk
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Numerics;
using UnityEngine;

#nullable disable
namespace Mafi.Unity.InstancedRendering
{
  public interface IRenderedChunk : IRenderedChunksBase
  {
    Chunk256AndIndex CoordAndIndex { get; }

    /// <summary>Origin coordinate of this chunk in the world space.</summary>
    Vector2 Origin { get; }

    /// <summary>
    /// Whether to track and call <see cref="M:Mafi.Unity.InstancedRendering.IRenderedChunk.NotifyWasNotRendered" />.
    /// </summary>
    bool TrackStoppedRendering { get; }

    /// <summary>
    /// Returns maximum possible deviation from the chunk default bounds.
    /// In other words, by how much can a model stick out of the chunk while its primary coord still being inside it.
    /// 
    /// IMPORTANT: This must be a constant value, any changes will not be reflected.
    /// </summary>
    float MaxModelDeviationFromChunkBounds { get; }

    /// <summary>
    /// Current min/max height of contents. Used for bounding box.
    /// </summary>
    Vector2 MinMaxHeight { get; }

    /// <summary>
    /// Renders everything on this chunk. This could be just scheduling instances for rendering.
    /// </summary>
    RenderStats Render(GameTime time, float cameraDistance, int lod, float pxPerMeter);

    /// <summary>
    /// This chunk is responsible to call <see cref="M:Mafi.Unity.InstancedRendering.IRenderedChunksParent.NotifyHeightRangeChanged(System.Single,System.Single)" /> whenever occupied
    /// height changes.
    /// </summary>
    void Register(IRenderedChunksParent parent);

    /// <summary>
    /// This method will be called if and only if: the <see cref="P:Mafi.Unity.InstancedRendering.IRenderedChunk.TrackStoppedRendering" /> is true
    /// AND this chunk was rendered during the previous frame AND it was not rendered during this frame.
    /// </summary>
    void NotifyWasNotRendered();
  }
}
