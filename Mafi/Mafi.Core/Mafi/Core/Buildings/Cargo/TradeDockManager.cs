// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Cargo.TradeDockManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Static;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Buildings.Cargo
{
  [GenerateSerializer(false, null, 0)]
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class TradeDockManager
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public Option<Mafi.Core.Buildings.Cargo.TradeDock> TradeDock { get; private set; }

    public TradeDockManager(IConstructionManager constructionManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      constructionManager.EntityConstructed.Add<TradeDockManager>(this, new Action<IStaticEntity>(this.entityConstructed));
      constructionManager.EntityStartedDeconstruction.Add<TradeDockManager>(this, new Action<IStaticEntity>(this.entityDeconstructionStarted));
    }

    private void entityConstructed(IStaticEntity entity)
    {
      if (!(entity is Mafi.Core.Buildings.Cargo.TradeDock tradeDock))
        return;
      if (this.TradeDock.HasValue)
      {
        Assert.Fail("Cannot add another trade dock!");
      }
      else
      {
        Assert.That<Option<Mafi.Core.Buildings.Cargo.TradeDock>>(this.TradeDock).IsNone<Mafi.Core.Buildings.Cargo.TradeDock>();
        this.TradeDock = (Option<Mafi.Core.Buildings.Cargo.TradeDock>) tradeDock;
      }
    }

    private void entityDeconstructionStarted(IStaticEntity entity)
    {
      if (!(entity is Mafi.Core.Buildings.Cargo.TradeDock tradeDock))
        return;
      if (this.TradeDock.HasValue && this.TradeDock != tradeDock)
      {
        Assert.Fail("Trade dock already exists!");
      }
      else
      {
        Assert.That<Mafi.Core.Buildings.Cargo.TradeDock>(tradeDock).IsEqualTo<Mafi.Core.Buildings.Cargo.TradeDock>(this.TradeDock.ValueOrNull);
        this.TradeDock = (Option<Mafi.Core.Buildings.Cargo.TradeDock>) Option.None;
      }
    }

    public static void Serialize(TradeDockManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<TradeDockManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, TradeDockManager.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Option<Mafi.Core.Buildings.Cargo.TradeDock>.Serialize(this.TradeDock, writer);
    }

    public static TradeDockManager Deserialize(BlobReader reader)
    {
      TradeDockManager tradeDockManager;
      if (reader.TryStartClassDeserialization<TradeDockManager>(out tradeDockManager))
        reader.EnqueueDataDeserialization((object) tradeDockManager, TradeDockManager.s_deserializeDataDelayedAction);
      return tradeDockManager;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.TradeDock = Option<Mafi.Core.Buildings.Cargo.TradeDock>.Deserialize(reader);
    }

    static TradeDockManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TradeDockManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((TradeDockManager) obj).SerializeData(writer));
      TradeDockManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((TradeDockManager) obj).DeserializeData(reader));
    }
  }
}
