// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Storages.StorageBaseProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Prototypes;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Mafi.Core.Buildings.Storages
{
  [DebuggerDisplay("StorageBaseProto: {Id}")]
  public abstract class StorageBaseProto : LayoutEntityProto
  {
    public readonly Quantity Capacity;
    /// <summary>
    /// Limits how much this storage can receive via ports per TransferLimitDuration.
    /// </summary>
    public readonly Quantity TransferLimit;
    /// <summary>See TransferLimit.</summary>
    public readonly Duration TransferLimitDuration;

    protected StorageBaseProto(
      StaticEntityProto.ID id,
      Proto.Str strings,
      EntityLayout layout,
      Quantity capacity,
      EntityCosts costs,
      LayoutEntityProto.Gfx graphics,
      Quantity? transferLimit = null,
      Duration? transferLimitDuration = null,
      IEnumerable<Tag> tags = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      StaticEntityProto.ID id1 = id;
      Proto.Str strings1 = strings;
      EntityLayout layout1 = layout;
      EntityCosts costs1 = costs;
      LayoutEntityProto.Gfx graphics1 = graphics;
      IEnumerable<Tag> tags1 = tags;
      Duration? constructionDurationPerProduct = new Duration?();
      Upoints? boostCost = new Upoints?();
      IEnumerable<Tag> tags2 = tags1;
      // ISSUE: explicit constructor call
      base.\u002Ector(id1, strings1, layout1, costs1, graphics1, constructionDurationPerProduct, boostCost, tags: tags2);
      this.Capacity = capacity.CheckPositive();
      this.TransferLimit = (transferLimit ?? 6.Quantity()).CheckPositive();
      this.TransferLimitDuration = (transferLimitDuration ?? 1.Seconds() / 5).CheckPositive();
    }
  }
}
