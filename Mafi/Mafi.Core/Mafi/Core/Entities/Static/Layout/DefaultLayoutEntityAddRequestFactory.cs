// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.Layout.DefaultLayoutEntityAddRequestFactory
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Factory.Transports;
using System;

#nullable disable
namespace Mafi.Core.Entities.Static.Layout
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public class DefaultLayoutEntityAddRequestFactory : 
    IFactory<ILayoutEntityProto, EntityAddRequestData, LayoutEntityAddRequest>
  {
    private readonly TransportsManager m_transportsManager;

    public DefaultLayoutEntityAddRequestFactory(TransportsManager transportsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_transportsManager = transportsManager;
    }

    public LayoutEntityAddRequest Create(ILayoutEntityProto proto, EntityAddRequestData data)
    {
      return proto is ILayoutEntityProtoWithElevation protoWithElevation && protoWithElevation.CanBeElevated ? LayoutEntityAddRequest.GetPooledInstanceToCreateEntity(proto, new EntityAddRequestData(data.Transform, data.DisableMiniZipperPlacement, data.IgnoreForCollisions.HasValue ? (Predicate<EntityId>) (x => data.IgnoreForCollisions.ValueOrNull(x) || this.m_transportsManager.IgnorePillarsPredicate(x)) : (Predicate<EntityId>) (x => this.m_transportsManager.IgnorePillarsPredicate(x)), data.RecordTileErrors)) : LayoutEntityAddRequest.GetPooledInstanceToCreateEntity(proto, data);
    }
  }
}
