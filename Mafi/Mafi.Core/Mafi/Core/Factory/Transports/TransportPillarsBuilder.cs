// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Transports.TransportPillarsBuilder
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;

#nullable disable
namespace Mafi.Core.Factory.Transports
{
  /// <summary>Helper class that instantiates transports.</summary>
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class TransportPillarsBuilder
  {
    public readonly TransportPillarProto PillarProto;
    private readonly EntityId.Factory m_idFactory;
    private readonly EntityContext m_context;

    public TransportPillarsBuilder(EntityId.Factory idFactory, EntityContext context)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_idFactory = idFactory;
      this.m_context = context;
      this.PillarProto = context.ProtosDb.First<TransportPillarProto>().ValueOrThrow("No transport pillar proto found.");
    }

    public TransportPillar Create(Tile3i position, ThicknessTilesI height)
    {
      return new TransportPillar(this.m_idFactory.GetNextId(), this.PillarProto, this.m_context, position, height);
    }
  }
}
