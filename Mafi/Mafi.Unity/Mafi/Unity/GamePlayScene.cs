// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.GamePlayScene
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Game;
using Mafi.Core.GameLoop;
using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity
{
  /// <summary>Represent the main game play.</summary>
  internal class GamePlayScene : IGameScene
  {
    internal const int INIT_UPDATES_COUNT = 10;
    private static readonly int ID_GAME_TIME;
    private static readonly int ID_COS_GAME_TIME;
    private static readonly int ID_SIN_GAME_TIME;
    private static readonly int ID_GAME_TIME_ABS_T;
    private static readonly int ID_COOKIE_TIME_ABS_T;
    private readonly IMain m_main;
    private readonly Option<StartNewGameArgs> m_newGameArgs;
    private readonly Option<LoadGameArgsFromFile> m_loadGameArgs;
    private DependencyResolver m_resolver;
    private GameRunner m_gameRunner;

    public bool IsInitialized => this.m_resolver != null;

    public event Action<DependencyResolver> ResolverCreated;

    public GamePlayScene(IMain main, StartNewGameArgs newGameArgs)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_main = main.CheckNotNull<IMain>();
      this.m_newGameArgs = (Option<StartNewGameArgs>) newGameArgs.CheckNotNull<StartNewGameArgs>();
    }

    public GamePlayScene(IMain main, LoadGameArgsFromFile loadGameArgs)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_main = main.CheckNotNull<IMain>();
      this.m_loadGameArgs = (Option<LoadGameArgsFromFile>) loadGameArgs.CheckNotNull<LoadGameArgsFromFile>();
    }

    public IEnumerator<string> Initialize()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator<string>) new GamePlayScene.\u003CInitialize\u003Ed__18(0)
      {
        \u003C\u003E4__this = this
      };
    }

    private void resolverCreated(DependencyResolver resolver)
    {
      this.m_resolver = resolver;
      Action<DependencyResolver> resolverCreated = this.ResolverCreated;
      if (resolverCreated == null)
        return;
      resolverCreated(resolver);
    }

    private IEnumerator<string> buildNewGame(StartNewGameArgs args)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator<string>) new GamePlayScene.\u003CbuildNewGame\u003Ed__20(0)
      {
        \u003C\u003E4__this = this,
        args = args
      };
    }

    private IEnumerator<string> loadGame(LoadGameArgsFromFile args)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator<string>) new GamePlayScene.\u003CloadGame\u003Ed__21(0)
      {
        \u003C\u003E4__this = this,
        args = args
      };
    }

    private void renderUpdate(GameTime time)
    {
      double num1 = time.TimeSinceLoadMs.ToDouble();
      float num2 = (float) (num1 / 1000.0);
      Shader.SetGlobalVector(GamePlayScene.ID_GAME_TIME, new Vector4((float) (num1 / 20000.0), num2, (float) (num1 / 500.0), (float) (num1 / 333.3333333)));
      Shader.SetGlobalVector(GamePlayScene.ID_COS_GAME_TIME, new Vector4(MafiMath.Cos(num2 / 64f), MafiMath.Cos(num2 / 16f), MafiMath.Cos(num2 / 4f), MafiMath.Cos(num2)));
      Shader.SetGlobalVector(GamePlayScene.ID_SIN_GAME_TIME, new Vector4(MafiMath.Sin(num2 / 64f), MafiMath.Sin(num2 / 16f), MafiMath.Sin(num2 / 4f), MafiMath.Sin(num2)));
      Shader.SetGlobalVector(GamePlayScene.ID_GAME_TIME_ABS_T, new Vector4((float) time.SimStepsSinceLoad, time.AbsoluteT, (float) time.GameSpeedMult, 0.0f));
      Shader.SetGlobalVector(GamePlayScene.ID_COOKIE_TIME_ABS_T, new Vector4((float) time.SimStepsSinceLoad, time.AbsoluteT, (float) time.GameSpeedMult, 0.0f));
    }

    public void Update(Fix32 deltaMs) => this.m_gameRunner.Update(deltaMs);

    public void Terminate()
    {
      if (this.m_gameRunner != null)
      {
        this.m_gameRunner.Terminate();
        this.m_gameRunner = (GameRunner) null;
      }
      else
        Log.Warning("No game runner, already terminated?");
      if (this.m_resolver != null)
      {
        this.m_resolver.TerminateAndClear();
        this.m_resolver = (DependencyResolver) null;
      }
      else
        Log.Warning("No resolver, already terminated?");
      StateAssert.Reset();
      ThreadAssert.Reset();
    }

    public void OnProjectChanged()
    {
      this.m_resolver.Resolve<GameLoopEvents>().InvokeOnProjectChanged();
    }

    public DependencyResolver GetResolver()
    {
      Assert.That<DependencyResolver>(this.m_resolver).IsNotNull<DependencyResolver>("No resolver!");
      return this.m_resolver;
    }

    static GamePlayScene()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      GamePlayScene.ID_GAME_TIME = Shader.PropertyToID("_GameTime");
      GamePlayScene.ID_COS_GAME_TIME = Shader.PropertyToID("_CosGameTime");
      GamePlayScene.ID_SIN_GAME_TIME = Shader.PropertyToID("_SinGameTime");
      GamePlayScene.ID_GAME_TIME_ABS_T = Shader.PropertyToID("_Mafi_GameTime");
      GamePlayScene.ID_COOKIE_TIME_ABS_T = Shader.PropertyToID("_Mafi_CookieTime");
    }
  }
}
