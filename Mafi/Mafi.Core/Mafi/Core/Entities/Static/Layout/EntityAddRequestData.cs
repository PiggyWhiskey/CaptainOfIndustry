// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.Layout.EntityAddRequestData
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using System;

#nullable disable
namespace Mafi.Core.Entities.Static.Layout
{
  public readonly struct EntityAddRequestData
  {
    public readonly TileTransform Transform;
    public readonly bool DisableMiniZipperPlacement;
    public readonly Option<Predicate<EntityId>> IgnoreForCollisions;
    public readonly bool RecordTileErrors;

    public EntityAddRequestData(
      TileTransform transform,
      bool enableMiniZipperPlacement = false,
      Predicate<EntityId> ignoreForCollisions = null,
      bool recordTileErrors = false)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Transform = transform;
      this.DisableMiniZipperPlacement = !enableMiniZipperPlacement;
      this.IgnoreForCollisions = (Option<Predicate<EntityId>>) ignoreForCollisions;
      this.RecordTileErrors = recordTileErrors;
    }
  }
}
