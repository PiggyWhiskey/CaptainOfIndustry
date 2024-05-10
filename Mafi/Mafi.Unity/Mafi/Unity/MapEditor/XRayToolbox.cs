// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MapEditor.XRayToolbox
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.Audio;
using Mafi.Unity.InputControl;
using Mafi.Unity.InputControl.Toolbar;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MapEditor
{
  public class XRayToolbox : Mafi.Unity.InputControl.Toolbar.Toolbox
  {
    private readonly AudioSource m_invalidSound;
    private readonly AudioSource m_upSound;
    private readonly AudioSource m_downSound;
    private Option<Func<bool?>> m_onUp;
    private Option<Func<bool?>> m_onDown;

    public XRayToolbox(
      ToolbarController toolbar,
      UiBuilder builder,
      AudioDb audioDb,
      UiAudio audio)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(toolbar, builder);
      this.m_invalidSound = audioDb.GetSharedAudio(audio.InvalidOp);
      this.m_upSound = audioDb.GetSharedAudio(audio.Up);
      this.m_downSound = audioDb.GetSharedAudio(audio.Down);
    }

    public void SetOnUp(Func<bool?> action)
    {
      Assert.That<Option<Func<bool?>>>(this.m_onUp).IsNone<Func<bool?>>();
      this.m_onUp = (Option<Func<bool?>>) action;
    }

    public void SetOnDown(Func<bool?> action)
    {
      Assert.That<Option<Func<bool?>>>(this.m_onDown).IsNone<Func<bool?>>();
      this.m_onDown = (Option<Func<bool?>>) action;
    }

    private void onUp()
    {
      if (this.m_onUp.IsNone)
        return;
      bool? nullable1 = this.m_onUp.Value();
      bool? nullable2 = nullable1;
      bool flag1 = true;
      if (nullable2.GetValueOrDefault() == flag1 & nullable2.HasValue)
      {
        this.m_upSound.Play();
      }
      else
      {
        nullable2 = nullable1;
        bool flag2 = false;
        if (!(nullable2.GetValueOrDefault() == flag2 & nullable2.HasValue))
          return;
        this.m_invalidSound.Play();
      }
    }

    private void onDown()
    {
      if (this.m_onDown.IsNone)
        return;
      bool? nullable1 = this.m_onDown.Value();
      bool? nullable2 = nullable1;
      bool flag1 = true;
      if (nullable2.GetValueOrDefault() == flag1 & nullable2.HasValue)
      {
        this.m_downSound.Play();
      }
      else
      {
        nullable2 = nullable1;
        bool flag2 = false;
        if (!(nullable2.GetValueOrDefault() == flag2 & nullable2.HasValue))
          return;
        this.m_invalidSound.Play();
      }
    }

    protected override void BuildCustomItems(UiBuilder builder)
    {
      this.AddButton("Up", "Assets/Unity/UserInterface/General/PlatformUp128.png", new Action(this.onUp), (Func<ShortcutsManager, KeyBindings>) (m => m.RaiseUp));
      this.AddButton("Down", "Assets/Unity/UserInterface/General/PlatformDown128.png", new Action(this.onDown), (Func<ShortcutsManager, KeyBindings>) (m => m.LowerDown));
      this.AddToToolbar();
    }
  }
}
