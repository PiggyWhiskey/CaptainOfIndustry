// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.UnlockedProtosDbForUi
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.GameLoop;
using Mafi.Core.Prototypes;
using System;
using System.Collections.Generic;
using System.Linq;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class UnlockedProtosDbForUi
  {
    private readonly UnlockedProtosDb m_unlockedProtosDb;
    private bool m_syncNewChanges;
    private bool m_newChangesForRender;
    private readonly Set<Proto> m_unlockedProtos;

    public event Action OnUnlockedSetChangedForUi;

    public UnlockedProtosDbForUi(UnlockedProtosDb unlockedProtosDb, IGameLoopEvents gameLoopEvents)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_unlockedProtos = new Set<Proto>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_unlockedProtosDb = unlockedProtosDb;
      this.m_unlockedProtos.AddRange(unlockedProtosDb.AllUnlocked<Proto>());
      unlockedProtosDb.OnUnlockedSetChanged.AddNonSaveable<UnlockedProtosDbForUi>(this, new Action(this.onNewChanges));
      gameLoopEvents.SyncUpdate.AddNonSaveable<UnlockedProtosDbForUi>(this, new Action<GameTime>(this.syncUpdate));
      gameLoopEvents.RenderUpdate.AddNonSaveable<UnlockedProtosDbForUi>(this, new Action<GameTime>(this.renderUpdate));
    }

    private void onNewChanges() => this.m_syncNewChanges = true;

    private void syncUpdate(GameTime gameTime)
    {
      if (!this.m_syncNewChanges)
        return;
      this.m_syncNewChanges = false;
      this.m_unlockedProtos.AddRange(this.m_unlockedProtosDb.AllUnlocked<Proto>());
      this.m_newChangesForRender = true;
    }

    private void renderUpdate(GameTime gameTime)
    {
      if (!this.m_newChangesForRender)
        return;
      this.m_newChangesForRender = false;
      Action unlockedSetChangedForUi = this.OnUnlockedSetChangedForUi;
      if (unlockedSetChangedForUi == null)
        return;
      unlockedSetChangedForUi();
    }

    public IEnumerable<T> AllUnlocked<T>() where T : IProto
    {
      return this.m_unlockedProtos.OfType<T>().Where<T>((Func<T, bool>) (x => !x.IsNotAvailable));
    }

    public IEnumerable<T> FilterUnlocked<T>(IEnumerable<T> candidates) where T : Proto
    {
      return candidates.Where<T>((Func<T, bool>) (x => x.IsAvailable && this.m_unlockedProtos.Contains((Proto) x)));
    }

    public Option<Proto> GetNearestUnlockedDowngradeFor(IProto proto)
    {
      if (!(proto is IProtoWithUpgrade protoWithUpgrade))
        return Option<Proto>.None;
      for (Option<IProtoWithUpgrade> previousTierNonGeneric = protoWithUpgrade.UpgradeNonGeneric.PreviousTierNonGeneric; previousTierNonGeneric.HasValue; previousTierNonGeneric = previousTierNonGeneric.Value.UpgradeNonGeneric.PreviousTierNonGeneric)
      {
        if (this.IsUnlocked((IProto) previousTierNonGeneric.Value))
          return previousTierNonGeneric.As<Proto>();
      }
      return Option<Proto>.None;
    }

    public bool IsUnlocked(IProto proto)
    {
      return proto.IsAvailable && ((IEnumerable<IProto>) this.m_unlockedProtos).Contains<IProto>(proto);
    }

    public bool IsLocked(Proto proto)
    {
      return proto.IsNotAvailable || !this.m_unlockedProtos.Contains(proto);
    }

    public bool AnyUnlocked<T>() where T : IProto
    {
      foreach (Proto unlockedProto in this.m_unlockedProtos)
      {
        if (unlockedProto.IsAvailable && unlockedProto is T)
          return true;
      }
      return false;
    }
  }
}
