// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.AreaTool.AreaSelectionToolV2
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Terrain;
using Mafi.Unity.Terrain;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.AreaTool
{
  public class AreaSelectionToolV2
  {
    private readonly ShortcutsManager m_shortcutsManager;
    private readonly TerrainCursor m_terrainCursor;
    private readonly Area2iRenderer m_selectionRenderer;
    private KeyBindings m_primaryBinding;
    private KeyBindings m_primaryCancelBinding;
    private Color m_primaryColor;
    private bool m_isSelecting;
    private Tile2i m_selectionStart;
    private Tile2i m_selectionEnd;

    public bool IsActive { get; private set; }

    public event AreaSelectionToolV2.AreaSelectionDelegate SelectionCompleted;

    public AreaSelectionToolV2(
      ShortcutsManager shortcutsManager,
      NewInstanceOf<TerrainCursor> terrainCursor,
      NewInstanceOf<Area2iRenderer> selectionRenderer)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_shortcutsManager = shortcutsManager;
      this.m_terrainCursor = terrainCursor.Instance;
      this.m_selectionRenderer = selectionRenderer.Instance;
      this.m_selectionRenderer.Hide();
    }

    public void SetPrimaryBinding(
      KeyBindings selectBinding,
      KeyBindings cancelBinding,
      Color color)
    {
      this.m_primaryBinding = selectBinding;
      this.m_primaryCancelBinding = cancelBinding;
      this.m_primaryColor = color;
    }

    public void SetWidth(float width) => this.m_selectionRenderer.SetWidth(width);

    /// <summary>Starts listening to area selection events.</summary>
    public void Activate()
    {
      if (this.IsActive)
        return;
      if (this.m_primaryBinding.IsEmpty)
        Log.Warning("Primary binding was not set.");
      this.IsActive = true;
      this.m_terrainCursor.Activate();
    }

    public void Deactivate()
    {
      if (!this.IsActive)
        return;
      this.IsActive = false;
      this.m_terrainCursor.Deactivate();
      this.CancelSelection();
    }

    public bool InputUpdate()
    {
      if (!this.IsActive)
        return false;
      if (this.m_shortcutsManager.IsDown(this.m_primaryBinding) && this.m_terrainCursor.HasValue)
      {
        this.m_isSelecting = true;
        this.m_selectionRenderer.Show();
        this.m_selectionStart = this.m_selectionEnd = this.m_terrainCursor.Tile2i;
        this.m_selectionRenderer.SetColor(this.m_primaryColor);
        this.updateArea();
        return true;
      }
      if (this.m_isSelecting)
      {
        if (this.m_shortcutsManager.IsOn(this.m_primaryBinding))
        {
          if (this.m_shortcutsManager.IsOn(this.m_primaryCancelBinding))
          {
            this.CancelSelection();
            return true;
          }
          if (this.m_terrainCursor.HasValue)
          {
            this.m_selectionEnd = this.m_terrainCursor.Tile2i;
            this.updateArea();
          }
        }
        else
        {
          this.CancelSelection();
          this.SelectionCompleted(RectangleTerrainArea2i.FromTwoPositions(this.m_selectionStart, this.m_selectionEnd));
          return true;
        }
      }
      return false;
    }

    public void CancelSelection()
    {
      if (!this.m_isSelecting)
        return;
      this.m_isSelecting = false;
      this.m_selectionRenderer.Hide();
    }

    private void updateArea()
    {
      this.m_selectionRenderer.SetArea(RectangleTerrainArea2i.FromTwoPositions(this.m_selectionStart, this.m_selectionEnd), this.m_terrainCursor.TerrainManager);
    }

    public delegate void AreaSelectionDelegate(RectangleTerrainArea2i area);
  }
}
