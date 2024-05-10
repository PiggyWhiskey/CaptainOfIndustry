// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Utils.ReloadAfterAssetUpdateManager
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core.GameLoop;
using System;
using System.Collections.Generic;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Utils
{
  /// <summary>
  /// Workaround for a Unity issue where material looses buffer after asset update.
  /// https://forum.unity.com/threads/shader-stops-rendering-when-assets-are-updated.1240933/
  /// </summary>
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class ReloadAfterAssetUpdateManager : IDisposable
  {
    private LystStruct<IReloadAfterAssetUpdate> m_instances;

    public ReloadAfterAssetUpdateManager(IGameLoopEvents gameLoopEvents)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      gameLoopEvents.OnProjectChanged.AddNonSaveable<ReloadAfterAssetUpdateManager>(this, new Action(this.onProjectChanged));
    }

    public void Register(IReloadAfterAssetUpdate instance)
    {
      if (instance == null)
        Log.Warning("Trying to register null at ReloadAfterAssetUpdateManager.");
      this.m_instances.Add(instance);
    }

    public bool TryUnregister(IReloadAfterAssetUpdate instance)
    {
      return instance != null && this.m_instances.Remove(instance);
    }

    public bool TryUnregisterAndDispose<T>(T instance) where T : IReloadAfterAssetUpdate, IDisposable
    {
      if ((object) instance == null)
        return false;
      instance.Dispose();
      return this.m_instances.Remove((IReloadAfterAssetUpdate) instance);
    }

    private void onProjectChanged()
    {
      foreach (IReloadAfterAssetUpdate instance in this.m_instances)
        instance.ReloadAfterAssetUpdate();
    }

    public void Dispose()
    {
      if (!this.m_instances.IsNotEmpty)
        return;
      Log.Warning(string.Format("{0} instances of {1} were not disposed:\n", (object) this.m_instances.Count, (object) "IReloadAfterAssetUpdate") + ((IEnumerable<string>) this.m_instances.ToArray().MapArray<IReloadAfterAssetUpdate, string>((Func<IReloadAfterAssetUpdate, string>) (x => x.GetType().FullName))).JoinStrings("\n"));
    }
  }
}
