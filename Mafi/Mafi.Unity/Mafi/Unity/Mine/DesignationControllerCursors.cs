// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Mine.DesignationControllerCursors
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.InputControl.Cursors;
using Mafi.Unity.UserInterface.Style;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Mine
{
  /// <summary>
  /// Class that provides cursors for designation controllers.
  /// </summary>
  /// <remarks>
  /// Main purpose of this class is to reduce number of cursor instances. All designation controllers share one
  /// instance of this class.
  /// </remarks>
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class DesignationControllerCursors
  {
    private readonly CursorManager m_cursorManager;
    private readonly Cursoor[] m_cursors;
    private readonly Cursoor m_clearCursor;
    private readonly Cursoor m_denyCursor;

    public DesignationControllerCursors(CursorManager cursorManager, UiStyle style)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_cursorManager = cursorManager;
      this.m_cursors = new Cursoor[2];
      this.m_cursors[1] = cursorManager.RegisterCursor(style.Cursors.Flat);
      this.m_cursors[0] = cursorManager.RegisterCursor(style.Cursors.Down);
      this.m_clearCursor = cursorManager.RegisterCursor(style.Cursors.Clear);
      this.m_denyCursor = cursorManager.RegisterCursor(style.Cursors.Deny);
    }

    public void ClearCursors() => this.m_cursorManager.ClearCursors();

    public void ShowClearDesignationCursor() => this.m_clearCursor.Show();

    public void ShowDenyDesignationCursor() => this.m_denyCursor.Show();

    public void ShowCursorFor(AreaMode mode) => this.m_cursors[(int) mode].Show();
  }
}
