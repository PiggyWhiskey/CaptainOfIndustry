// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Transports.TransportPillarAddRequest
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Validators;

#nullable disable
namespace Mafi.Core.Factory.Transports
{
  public sealed class TransportPillarAddRequest : IEntityAddRequest
  {
    public static readonly TransportPillarAddRequest Instance;

    public EntityAddReason ReasonToAdd { get; }

    public TransportPillarAddRequest()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static TransportPillarAddRequest()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TransportPillarAddRequest.Instance = new TransportPillarAddRequest();
    }
  }
}
