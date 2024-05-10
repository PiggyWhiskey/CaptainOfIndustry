// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Cursors.CursorManager
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core.Console;
using Mafi.Unity.UiFramework.Styles;
using Mafi.Unity.UserInterface.Style;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Cursors
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class CursorManager
  {
    private Cursoor m_activeCursor;
    private readonly AssetsDb m_assetsDb;
    private readonly Dict<string, Cursoor> m_cursors;
    private readonly Cursoor m_defaultCursoor;

    public Cursoor ActiveCursor => this.m_activeCursor;

    public CursorMode CursorMode { get; private set; }

    public CursorManager(AssetsDb assetsDb, UiStyle style)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_cursors = new Dict<string, Cursoor>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_assetsDb = assetsDb;
      this.m_defaultCursoor = new Cursoor(this, this.getCursorTexture(style.Cursors.Main.AssetPath), style.Cursors.Main.Hotspot);
      this.m_defaultCursoor.Show();
    }

    [ConsoleCommand(true, false, null, null)]
    private string toggleCursorRenderingMode()
    {
      this.CursorMode = this.CursorMode == CursorMode.Auto ? CursorMode.ForceSoftware : CursorMode.Auto;
      this.m_activeCursor.Show(true);
      return string.Format("Cursor rendering was set to: '{0}'.", (object) this.CursorMode);
    }

    public Cursoor RegisterCursor(CursorStyle style)
    {
      string id = style.Id;
      if (this.m_cursors.ContainsKey(id))
        return this.m_cursors[id];
      Cursoor cursoor = new Cursoor(this, this.getCursorTexture(style.AssetPath), style.Hotspot);
      this.m_cursors.Add(id, cursoor);
      return cursoor;
    }

    public void SetActiveCursor(Cursoor cursor) => this.m_activeCursor = cursor;

    public void RestoreCursor() => this.m_activeCursor.Show(true);

    public void ClearCursors()
    {
      this.m_defaultCursoor.Show();
      this.m_activeCursor = this.m_defaultCursoor;
    }

    private Texture2D getCursorTexture(string path) => this.m_assetsDb.GetSharedTexture(path);
  }
}
