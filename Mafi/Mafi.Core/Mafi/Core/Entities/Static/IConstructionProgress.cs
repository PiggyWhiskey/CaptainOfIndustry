// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.IConstructionProgress
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Economy;
using Mafi.Core.Products;

#nullable disable
namespace Mafi.Core.Entities.Static
{
  public interface IConstructionProgress
  {
    ImmutableArray<IProductBufferReadOnly> Buffers { get; }

    AssetValue TotalCost { get; }

    AssetValue AlreadyRemovedCost { get; }

    /// <summary>
    /// Number of currently completed construction steps. This increases for construction and decreases for
    /// destruction.
    /// </summary>
    int CurrentSteps { get; }

    /// <summary>Number of max steps.</summary>
    int MaxSteps { get; }

    /// <summary>
    /// Number of extra steps that needs to be performed after all material is delivered (during construction).
    /// </summary>
    int ExtraSteps { get; }

    Percent Progress { get; }

    /// <summary>
    /// Whether construction is nearly finished. This is important for rendering since entity
    /// will be always fully covered by construction cubes and renderers can switch blueprint for models
    /// and vice versa.
    /// </summary>
    bool IsNearlyFinished { get; }

    bool WasBlockedOnProductsLastSim { get; }

    /// <summary>Whether this is actually a deconstruction.</summary>
    bool IsDeconstruction { get; }

    bool IsUpgrade { get; }

    bool IsPaused { get; }

    bool IsAllowedToFinish();

    Quantity GetMissingQuantityFor(ProductProto product);

    Upoints CostForQuickBuild(IAssetTransactionManager assetManager, out bool hasProducts);

    Upoints? CostForQuickRemove(IAssetTransactionManager assetManager);
  }
}
