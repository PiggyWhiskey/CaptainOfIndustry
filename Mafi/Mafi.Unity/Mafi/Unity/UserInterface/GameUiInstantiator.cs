// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.GameUiInstantiator
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.GameLoop;
using System.Collections.Generic;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UserInterface
{
  /// <summary>
  /// Helper class that is responsible for instantiation of all in-game UI. This class does not handle Menus or other
  /// screens.
  /// </summary>
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  internal class GameUiInstantiator
  {
    public GameUiInstantiator(IGameLoopEvents gameLoopEvents, DependencyResolver resolver)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      gameLoopEvents.RegisterRendererInitState((object) this, this.buildUi(resolver));
    }

    private IEnumerator<string> buildUi(DependencyResolver resolver)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator<string>) new GameUiInstantiator.\u003CbuildUi\u003Ed__1(0)
      {
        resolver = resolver
      };
    }
  }
}
