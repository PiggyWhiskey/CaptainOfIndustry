// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Settlements.SettlementDecorationModule
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Entities.Static.Layout.Upgrade;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Buildings.Settlements
{
  [GenerateSerializer(false, null, 0)]
  public class SettlementDecorationModule : 
    LayoutEntityBase,
    IUpgradableEntity,
    IStaticEntity,
    IEntityWithPosition,
    IRenderedEntity,
    IEntity,
    IIsSafeAsHashKey,
    IAreaSelectableEntity,
    ISettlementSquareModule,
    ILayoutEntity,
    IEntityWithParticles
  {
    private SettlementDecorationModuleProto m_proto;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    [DoNotSave(0, null)]
    public SettlementDecorationModuleProto Prototype
    {
      get => this.m_proto;
      private set
      {
        this.m_proto = value;
        this.Prototype = (LayoutEntityProto) value;
      }
    }

    public override bool CanBePaused => false;

    public Option<Mafi.Core.Buildings.Settlements.Settlement> Settlement { get; private set; }

    public IUpgrader Upgrader { get; private set; }

    public bool AreParticlesEnabled => this.IsEnabled;

    public RelTile2i CoreSize => this.Prototype.Layout.CoreSize;

    public Tile3i Position => this.Transform.Position;

    public SettlementDecorationModule(
      EntityId id,
      SettlementDecorationModuleProto settlementModuleProto,
      TileTransform transform,
      EntityContext context,
      ILayoutEntityUpgraderFactory upgraderFactory)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (LayoutEntityProto) settlementModuleProto, transform, context);
      this.Prototype = settlementModuleProto;
      this.Upgrader = upgraderFactory.CreateInstance<SettlementDecorationModuleProto, SettlementDecorationModule>(this, this.Prototype);
    }

    public void SetSettlement(Mafi.Core.Buildings.Settlements.Settlement settlement)
    {
      this.Settlement = (Option<Mafi.Core.Buildings.Settlements.Settlement>) settlement;
    }

    public void ClearSettlement() => this.Settlement = Option<Mafi.Core.Buildings.Settlements.Settlement>.None;

    bool IUpgradableEntity.IsUpgradeAvailable(out LocStrFormatted errorMessage)
    {
      errorMessage = LocStrFormatted.Empty;
      return true;
    }

    public void UpgradeSelf()
    {
      if (this.Prototype.Upgrade.NextTier.IsNone)
        Log.Error("Upgrade not available!");
      else
        this.Prototype = this.Prototype.Upgrade.NextTier.Value;
    }

    public static void Serialize(SettlementDecorationModule value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<SettlementDecorationModule>(value))
        return;
      writer.EnqueueDataSerialization((object) value, SettlementDecorationModule.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteGeneric<SettlementDecorationModuleProto>(this.m_proto);
      Option<Mafi.Core.Buildings.Settlements.Settlement>.Serialize(this.Settlement, writer);
      writer.WriteGeneric<IUpgrader>(this.Upgrader);
    }

    public static SettlementDecorationModule Deserialize(BlobReader reader)
    {
      SettlementDecorationModule decorationModule;
      if (reader.TryStartClassDeserialization<SettlementDecorationModule>(out decorationModule))
        reader.EnqueueDataDeserialization((object) decorationModule, SettlementDecorationModule.s_deserializeDataDelayedAction);
      return decorationModule;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.m_proto = reader.ReadGenericAs<SettlementDecorationModuleProto>();
      this.Settlement = Option<Mafi.Core.Buildings.Settlements.Settlement>.Deserialize(reader);
      this.Upgrader = reader.ReadGenericAs<IUpgrader>();
    }

    static SettlementDecorationModule()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      SettlementDecorationModule.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      SettlementDecorationModule.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
    }
  }
}
