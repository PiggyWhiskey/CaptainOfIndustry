// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MainMenu.MainMenuScene
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Simulation;
using Mafi.Localization;
using Mafi.Unity.Audio;
using Mafi.Unity.InputControl;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Style;
using Mafi.Unity.Utils;
using System;
using System.Collections.Generic;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MainMenu
{
  internal class MainMenuScene : IGameScene
  {
    private readonly IMain m_main;
    private readonly MainMenuArgs m_args;
    private MainMenuController m_mainMenuController;
    private UiBuilder m_uiBuilder;
    private readonly GameTime m_dummyGameTime;

    public event Action<DependencyResolver> ResolverCreated;

    public MainMenuScene(IMain main, MainMenuArgs args)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_dummyGameTime = new GameTime();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_main = main.CheckNotNull<IMain>();
      this.m_args = args.CheckNotNull<MainMenuArgs>();
    }

    public IEnumerator<string> Initialize()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator<string>) new MainMenuScene.\u003CInitialize\u003Ed__9(0)
      {
        \u003C\u003E4__this = this
      };
    }

    private DependencyResolver createResolver()
    {
      DependencyResolverBuilder dependencyResolverBuilder = new DependencyResolverBuilder();
      dependencyResolverBuilder.RegisterInstance<IMain>(this.m_main).As<IMain>();
      dependencyResolverBuilder.RegisterInstance<AssetsDb>(this.m_main.AssetsDb).AsSelf();
      dependencyResolverBuilder.RegisterInstance<AudioDb>(this.m_main.AudioDb).AsSelf();
      dependencyResolverBuilder.RegisterInstance<IFileSystemHelper>(this.m_main.FileSystemHelper).AsAllInterfaces();
      dependencyResolverBuilder.RegisterInstance<BackgroundMusicManager>(this.m_main.BackgroundMusicManager).AsSelf();
      dependencyResolverBuilder.RegisterInstance<RandomProvider>(new RandomProvider("MaFi seed")).AsSelf();
      dependencyResolverBuilder.RegisterDependency<UiStyle>().AsSelf();
      dependencyResolverBuilder.RegisterDependency<UiAudio>().AsSelf();
      dependencyResolverBuilder.RegisterDependency<ShortcutsManager>().AsSelf();
      dependencyResolverBuilder.RegisterDependency<SimLoopEvents>().AsAllInterfaces();
      dependencyResolverBuilder.RegisterDependency<UiBuilder>().AsSelf();
      dependencyResolverBuilder.RegisterDependency<CoroutineHelper>().AsSelf();
      return dependencyResolverBuilder.BuildAndClear();
    }

    public void Update(Fix32 deltaMs)
    {
      if (this.m_mainMenuController == null)
      {
        Log.Error("Updating non-initialized main menu.");
        this.Initialize().EnumerateToTheEnd<string>();
      }
      this.m_mainMenuController.Update(this.m_dummyGameTime);
    }

    public void Terminate()
    {
      GlobalGfxSettings.NotifyIsInMenus(false);
      this.m_uiBuilder?.Destroy();
      this.m_mainMenuController = (MainMenuController) null;
      LocalizationManager.IgnoreDuplicates();
    }

    public void OnProjectChanged()
    {
    }
  }
}
