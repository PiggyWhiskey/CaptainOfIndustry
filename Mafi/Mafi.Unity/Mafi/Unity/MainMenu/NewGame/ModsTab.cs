// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MainMenu.NewGame.ModsTab
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Mods;
using Mafi.Core.SaveGame;
using Mafi.Localization;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MainMenu.NewGame
{
  public class ModsTab : Column, ITab
  {
    private readonly NewGameConfigForUi m_settings;
    private readonly IMain m_main;
    private readonly Column m_modsContainer;

    public ModsTab(NewGameConfigForUi settings, IMain main)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_settings = settings;
      this.m_main = main;
      this.Gap<ModsTab>(new Px?(2.pt())).MarginLeftRight<ModsTab>(Px.Auto).AlignItemsStretch<ModsTab>();
      this.Add((UiComponent) new Title(Tr.SelectMods_Title).UpperCase(false), (UiComponent) (this.m_modsContainer = new Column(2.pt()).AlignItemsStretch<Column>()));
    }

    void ITab.Activate()
    {
      this.m_settings.EnabledModTypes.AddRange(this.m_main.Mods.FilterAllLoadedMods());
      this.m_modsContainer.SetChildren(this.m_main.Mods.FilterThirdPartyMods().Select<UiComponent>((Func<ModData, UiComponent>) (m =>
      {
        string error = m.Exception.HasValue ? "Failed to load the mod\n" + m.Exception.Value.Message : "";
        Toggle component = new Toggle(true).Label<Toggle>(string.Format("{0} v{1}", (object) m.Name, (object) m.Version.GetValueOrDefault()).AsLoc()).Tooltip<Toggle>(new LocStrFormatted?(error.AsLoc())).Value(m.IsFullyLoaded && this.m_settings.EnabledModTypes.Contains(m)).OnValueChanged((Action<bool>) (include =>
        {
          if (!m.IsFullyLoaded)
            return;
          if (include)
            this.m_settings.EnabledModTypes.Add(m);
          else
            this.m_settings.EnabledModTypes.Remove(m);
        })).Enabled<Toggle>(m.IsFullyLoaded);
        if (m.IsFullyLoaded)
          return (UiComponent) component;
        return (UiComponent) new Row(8.pt())
        {
          (UiComponent) component.MarginRight<Toggle>(Px.Auto),
          (UiComponent) new ButtonText((LocStrFormatted) Tr.Error__Copy, (Action) (() => GUIUtility.systemCopyBuffer = error))
        };
      })));
      this.m_modsContainer.Add(this.m_main.FailedMods.Select<UiComponent>((Func<ModInfoRaw, UiComponent>) (m =>
      {
        string error = "Failed to load the type";
        Toggle component = new Toggle(true).Label<Toggle>(m.Name.AsLoc()).Tooltip<Toggle>(new LocStrFormatted?(error.AsLoc())).Value(false).Enabled<Toggle>(false);
        return (UiComponent) new Row(8.pt())
        {
          (UiComponent) component.MarginRight<Toggle>(Px.Auto),
          (UiComponent) new ButtonText((LocStrFormatted) Tr.Error__Copy, (Action) (() => GUIUtility.systemCopyBuffer = error))
        };
      })));
    }

    void ITab.Deactivate()
    {
    }
  }
}
