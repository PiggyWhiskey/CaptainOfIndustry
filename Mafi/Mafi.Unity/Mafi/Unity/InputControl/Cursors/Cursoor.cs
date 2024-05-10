// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Cursors.Cursoor
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Cursors
{
  /// <summary>Stateful cursor wrapper to simplify with cursors.</summary>
  public class Cursoor
  {
    private readonly CursorManager m_cursorManager;
    private readonly Texture2D m_texture;
    private readonly Vector2 m_hotspot;
    private bool m_isTemporaryShown;

    public Cursoor(CursorManager cursorManager, Texture2D texture, Vector2 hotspot)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_cursorManager = cursorManager;
      this.m_texture = texture;
      this.m_hotspot = hotspot;
    }

    public void Show(bool force = false)
    {
      if (!force && this.m_cursorManager.ActiveCursor == this)
        return;
      Cursor.SetCursor(this.m_texture, this.m_hotspot, this.m_cursorManager.CursorMode);
      this.m_cursorManager.SetActiveCursor(this);
    }

    public void Hide()
    {
      if (this.m_cursorManager.ActiveCursor != this)
        return;
      this.m_cursorManager.ClearCursors();
    }

    /// <summary>
    /// Shows a cursor that will be displayed only temporarily and then a previous cursor that was active will be
    /// restored. Don't forget to call <see cref="M:Mafi.Unity.InputControl.Cursors.Cursoor.HideTemporary" />. Currently used in free look mode.
    /// </summary>
    public void ShowTemporary()
    {
      Assert.That<bool>(this.m_isTemporaryShown).IsFalse();
      Cursor.SetCursor(this.m_texture, this.m_hotspot, this.m_cursorManager.CursorMode);
      this.m_isTemporaryShown = true;
    }

    /// <summary>
    /// Hides cursor that was displayed temporarily by <see cref="M:Mafi.Unity.InputControl.Cursors.Cursoor.ShowTemporary" />.
    /// </summary>
    public void HideTemporary()
    {
      Assert.That<bool>(this.m_isTemporaryShown).IsTrue();
      this.m_cursorManager.RestoreCursor();
      this.m_isTemporaryShown = false;
    }

    public void SetTemporaryVisibility(bool isVisible)
    {
      if (this.m_isTemporaryShown == isVisible)
        return;
      this.m_isTemporaryShown = isVisible;
      if (isVisible)
        Cursor.SetCursor(this.m_texture, this.m_hotspot, this.m_cursorManager.CursorMode);
      else
        this.m_cursorManager.RestoreCursor();
    }
  }
}
