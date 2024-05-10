// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Products.LooseProductsSlimIdManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Prototypes;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Products
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  [GenerateSerializer(false, null, 0)]
  public sealed class LooseProductsSlimIdManager : 
    SlimIdManagerBase<LooseProductProto, LooseProductSlimId>
  {
    /// <summary>
    /// Converts <see cref="T:Mafi.Core.Products.ProductSlimId" /> to <see cref="T:Mafi.Core.Products.LooseProductSlimId" />. If product is not loose,
    /// value will be default (<see cref="P:Mafi.Core.Products.LooseProductSlimId.PhantomId" />).
    /// </summary>
    [DoNotSave(0, null)]
    public readonly ImmutableArray<LooseProductSlimId> SlimIdToLoose;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public override LooseProductProto PhantomProto => LooseProductProto.Phantom;

    public override int MaxIdValue => (int) byte.MaxValue;

    public LooseProductsSlimIdManager(ProtosDb db, ProductsSlimIdManager productsSlimIdManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(db);
      this.SlimIdToLoose = LooseProductsSlimIdManager.createIdToLooseIdLookup(productsSlimIdManager);
    }

    [InitAfterLoad(InitPriority.High)]
    private void initAfterLoad(DependencyResolver resolver)
    {
      ReflectionUtils.SetField<LooseProductsSlimIdManager>(this, "SlimIdToLoose", (object) LooseProductsSlimIdManager.createIdToLooseIdLookup(resolver.Resolve<ProductsSlimIdManager>()));
    }

    protected override LooseProductSlimId CreateSlimId(int index)
    {
      return new LooseProductSlimId((byte) index);
    }

    protected override int GetIndex(LooseProductSlimId slimId) => (int) slimId.Value;

    private static ImmutableArray<LooseProductSlimId> createIdToLooseIdLookup(
      ProductsSlimIdManager productsSlimIdManager)
    {
      return productsSlimIdManager.ManagedProtos.Map<LooseProductSlimId>((Func<ProductProto, LooseProductSlimId>) (x => !(x is LooseProductProto looseProductProto) ? LooseProductSlimId.PhantomId : looseProductProto.LooseSlimId));
    }

    public static void Serialize(LooseProductsSlimIdManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<LooseProductsSlimIdManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, LooseProductsSlimIdManager.s_serializeDataDelayedAction);
    }

    public static LooseProductsSlimIdManager Deserialize(BlobReader reader)
    {
      LooseProductsSlimIdManager productsSlimIdManager;
      if (reader.TryStartClassDeserialization<LooseProductsSlimIdManager>(out productsSlimIdManager))
        reader.EnqueueDataDeserialization((object) productsSlimIdManager, LooseProductsSlimIdManager.s_deserializeDataDelayedAction);
      return productsSlimIdManager;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.RegisterInitAfterLoad<LooseProductsSlimIdManager>(this, "initAfterLoad", InitPriority.High);
    }

    static LooseProductsSlimIdManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      LooseProductsSlimIdManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((SlimIdManagerBase<LooseProductProto, LooseProductSlimId>) obj).SerializeData(writer));
      LooseProductsSlimIdManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((SlimIdManagerBase<LooseProductProto, LooseProductSlimId>) obj).DeserializeData(reader));
    }
  }
}
