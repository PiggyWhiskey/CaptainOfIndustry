// Decompiled with JetBrains decompiler
// Type: Mafi.Core.UnlockingTree.ProtoUnitUnlocker
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.UnlockingTree
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public class ProtoUnitUnlocker : UnitUnlockerBase<ProtoUnlock>
  {
    private readonly UnlockedProtosDb m_unlockedProtosDb;
    private readonly Lyst<IProto> m_protosBuffer;

    public ProtoUnitUnlocker(UnlockedProtosDb unlockedProtosDb)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_protosBuffer = new Lyst<IProto>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_unlockedProtosDb = unlockedProtosDb;
    }

    public override void Unlock(IIndexable<ProtoUnlock> units)
    {
      Assert.That<Lyst<IProto>>(this.m_protosBuffer).IsEmpty<IProto>();
      foreach (ProtoUnlock unit in units)
        this.m_protosBuffer.AddRange(unit.UnlockedProtos);
      this.m_unlockedProtosDb.Unlock(this.m_protosBuffer.ToImmutableArrayAndClear());
    }
  }
}
