// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MainMenu.MainMenuController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Utils;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UserInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MainMenu
{
  internal class MainMenuController : IRootEscapeManager
  {
    internal const string LAST_SEEN_VERSION_PERFS_KEY = "LastVersion";
    public readonly IMain Main;
    private readonly UiBuilder m_builder;
    private readonly IFileSystemHelper m_fileSystemHelper;
    private readonly DependencyResolver m_resolver;
    private PreInitModsAndProtos m_modsAndProtos;
    private Option<MainMenuScreen> m_mainMenu;
    private Option<CreditsScreen> m_credits;
    private Option<IRootEscapeHandler> m_rootEscHandler;

    internal MainMenuController(
      IMain main,
      UiBuilder builder,
      IFileSystemHelper fileSystemHelper,
      DependencyResolver resolver)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Main = main;
      this.m_builder = builder;
      this.m_fileSystemHelper = fileSystemHelper;
      this.m_resolver = resolver;
    }

    internal void BuildUi(PreInitModsAndProtos modsAndProtos)
    {
      this.m_modsAndProtos = modsAndProtos;
      string str;
      if (((IEnumerable<string>) Environment.GetCommandLineArgs()).Contains<string>("--reset-last-seen-change"))
      {
        str = "";
        PlayerPrefs.DeleteKey("LastVersion");
        PlayerPrefs.Save();
      }
      else
      {
        str = PlayerPrefs.GetString("LastVersion", "0.6.3a");
        PlayerPrefs.SetString("LastVersion", "0.6.3a");
        PlayerPrefs.Save();
      }
      Option<string> lastSeenVersion = (Option<string>) Option.None;
      if (!string.IsNullOrWhiteSpace(str) && str != "0.6.3a")
      {
        string cleanVersionStr = ChangelogUtils.GetCleanVersionStr(str, true);
        try
        {
          foreach (string enumerateAllVersion in ChangelogUtils.EnumerateAllVersions())
          {
            if (ChangelogUtils.GetCleanVersionStr(enumerateAllVersion, true) == cleanVersionStr)
            {
              lastSeenVersion = (Option<string>) str;
              break;
            }
          }
        }
        catch (Exception ex)
        {
          Log.Exception(ex, "Failed to show patch notes.");
        }
      }
      this.m_mainMenu = (Option<MainMenuScreen>) new MainMenuScreen(this.Main, this.m_fileSystemHelper, this.m_builder, this.m_modsAndProtos, this.m_resolver, lastSeenVersion, new Action(this.ShowCredits));
      this.m_builder.AddComponent((UiComponent) this.m_mainMenu.Value);
      this.m_builder.SetRootEscManager((IRootEscapeManager) this);
    }

    public void ShowCredits()
    {
      if (!this.m_credits.IsNone)
        return;
      this.m_credits = (Option<CreditsScreen>) new CreditsScreen(this.m_builder);
    }

    internal void Update(GameTime gameTime)
    {
      if (this.m_rootEscHandler.HasValue && Input.GetKey(KeyCode.Escape))
      {
        this.m_rootEscHandler.Value.OnEscape();
        this.m_rootEscHandler = (Option<IRootEscapeHandler>) Option.None;
      }
      else
      {
        MainMenuScreen valueOrNull = this.m_mainMenu.ValueOrNull;
        if ((valueOrNull != null ? (valueOrNull.InputUpdate() ? 1 : 0) : 0) != 0 || !this.m_credits.HasValue)
          return;
        if (this.m_credits.Value.IsDestroyed)
        {
          this.m_credits = Option<CreditsScreen>.None;
        }
        else
        {
          if (!Input.GetKeyDown(KeyCode.Escape) && !Input.GetKeyDown(KeyCode.Return) && !Input.GetKeyDown(KeyCode.Space))
            return;
          this.m_credits.Value.Destroy();
          this.m_credits = Option<CreditsScreen>.None;
        }
      }
    }

    public void SetRootEscapeHandler(IRootEscapeHandler handler)
    {
      this.m_rootEscHandler = handler.SomeOption<IRootEscapeHandler>();
    }

    public void ClearRootEscapeHandler(IRootEscapeHandler handler)
    {
      this.m_rootEscHandler = Option<IRootEscapeHandler>.None;
    }
  }
}
