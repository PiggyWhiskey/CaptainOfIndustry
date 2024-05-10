// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Mine.MineTowerProtoBuilder
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Static;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Buildings.Mine
{
  public sealed class MineTowerProtoBuilder : IProtoBuilder
  {
    public ProtosDb ProtosDb => this.Registrator.PrototypesDb;

    public ProtoRegistrator Registrator { get; }

    public MineTowerProtoBuilder(ProtoRegistrator registrator)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Registrator = registrator;
    }

    public MineTowerProtoBuilder.State Start(string name, StaticEntityProto.ID labId)
    {
      return new MineTowerProtoBuilder.State(this, labId, name);
    }

    public class State : LayoutEntityBuilderState<MineTowerProtoBuilder.State>
    {
      private readonly StaticEntityProto.ID m_id;
      private MineTowerProto.MineArea? m_mineArea;

      public State(MineTowerProtoBuilder builder, StaticEntityProto.ID id, string name)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector((IProtoBuilder) builder, id, name);
        this.m_id = id;
      }

      public MineTowerProtoBuilder.State SetMineArea(MineTowerProto.MineArea mineArea)
      {
        this.m_mineArea = new MineTowerProto.MineArea?(mineArea);
        return this;
      }

      public MineTowerProto BuildAndAdd()
      {
        return this.AddToDb<MineTowerProto>(new MineTowerProto(this.m_id, this.Strings, this.LayoutOrThrow, this.Costs, this.ValueOrThrow<MineTowerProto.MineArea>(this.m_mineArea, "Mine area"), this.Graphics));
      }
    }
  }
}
