// Decompiled with JetBrains decompiler
// Type: Mafi.Core.UnlockingTree.ProtoUnlock
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Prototypes;
using Mafi.Core.Utils;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.UnlockingTree
{
  public class ProtoUnlock : IProtoUnlock, IUnlockNodeUnit
  {
    private readonly bool m_hideInUi;

    public ImmutableArray<IProto> UnlockedProtos { get; }

    public bool HideInUI
    {
      get
      {
        if (this.m_hideInUi)
          return true;
        IProto proto = this.UnlockedProtos.FirstOrDefault();
        return proto != null && proto.IsNotAvailable;
      }
    }

    protected ProtoUnlock(ImmutableArray<IProto> protos, bool hideInUi = false)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.UnlockedProtos = protos.CheckNotDefaultStruct<ImmutableArray<IProto>>();
      this.m_hideInUi = hideInUi;
    }

    public ProtoUnlock(IProto proto, bool hideInUi = false)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.UnlockedProtos = ImmutableArray.Create<IProto>(proto);
      this.m_hideInUi = hideInUi;
    }

    public static IEnumerable<IProto> GetUnlockedProtos(IEnumerable<IUnlockNodeUnit> units)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerable<IProto>) new ProtoUnlock.\u003CGetUnlockedProtos\u003Ed__8(-2)
      {
        \u003C\u003E3__units = units
      };
    }

    public bool MatchesSearchQuery(string[] query)
    {
      foreach (IProto unlockedProto in this.UnlockedProtos)
      {
        if (UiSearchUtils.Matches(unlockedProto.Strings.Name.TranslatedString, query))
          return true;
      }
      return false;
    }
  }
}
