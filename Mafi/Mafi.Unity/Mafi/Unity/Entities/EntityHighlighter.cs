// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.EntityHighlighter
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core.Entities;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities
{
  /// <summary>
  /// Instantiate this class via <see cref="T:Mafi.NewInstanceOf`1" /> for entity highlighting needs.
  /// </summary>
  public class EntityHighlighter
  {
    private readonly EntitiesRenderingManager m_entitiesRenderer;
    private readonly Dict<IRenderedEntity, EntityHighlighter.HlRecord> m_highlightedEntities;

    public int HighlightedCount => this.m_highlightedEntities.Count;

    public EntityHighlighter(EntitiesRenderingManager entitiesRenderer)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_highlightedEntities = new Dict<IRenderedEntity, EntityHighlighter.HlRecord>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_entitiesRenderer = entitiesRenderer;
    }

    public void Highlight(IRenderedEntity entity, ColorRgba color)
    {
      Assert.That<bool>(color.IsEmpty).IsFalse("Highlighting with empty color.");
      if (entity.IsDestroyed)
      {
        Log.Warning(string.Format("Trying to highlight destroyed entity '{0}'.", (object) entity));
      }
      else
      {
        EntityHighlighter.HlRecord hlRecord;
        if (this.m_highlightedEntities.TryGetValue(entity, out hlRecord))
        {
          if (hlRecord.Color == color)
            return;
          this.m_highlightedEntities.Remove(entity);
          this.m_entitiesRenderer.RemoveHighlight(hlRecord.Id);
        }
        ulong id = this.m_entitiesRenderer.AddHighlight(entity, color);
        this.m_highlightedEntities[entity] = new EntityHighlighter.HlRecord(id, color);
      }
    }

    public void RemoveHighlight(IRenderedEntity entity)
    {
      EntityHighlighter.HlRecord hlRecord;
      if (!this.m_highlightedEntities.TryRemove(entity, out hlRecord))
        return;
      this.m_entitiesRenderer.RemoveHighlight(hlRecord.Id);
    }

    /// <summary>
    /// Highlights only this entity, removing other highlights.
    /// </summary>
    public void HighlightOnly(IRenderedEntity entity, ColorRgba color)
    {
      if (this.m_highlightedEntities.Count > 1 || !this.m_highlightedEntities.ContainsKey(entity))
        this.ClearAllHighlights();
      this.Highlight(entity, color);
    }

    public void ClearAllHighlights()
    {
      foreach (EntityHighlighter.HlRecord hlRecord in this.m_highlightedEntities.Values)
        this.m_entitiesRenderer.RemoveHighlight(hlRecord.Id);
      this.m_highlightedEntities.Clear();
    }

    private readonly struct HlRecord
    {
      public readonly ulong Id;
      public readonly ColorRgba Color;

      public HlRecord(ulong id, ColorRgba color)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.Id = id;
        this.Color = color;
      }
    }
  }
}
