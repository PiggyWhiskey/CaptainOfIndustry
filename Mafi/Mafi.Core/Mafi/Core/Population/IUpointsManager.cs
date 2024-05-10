// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Population.IUpointsManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Entities;
using Mafi.Core.Prototypes;
using Mafi.Localization;

#nullable disable
namespace Mafi.Core.Population
{
  public interface IUpointsManager
  {
    Quantity Quantity { get; }

    Percent QuickActionCostMultiplier { get; }

    bool CanConsume(Upoints unity);

    bool TryConsume(
      Proto.ID categoryId,
      Upoints unity,
      Option<IEntity> consumer = default (Option<IEntity>),
      LocStr? extraTitle = null);

    Upoints ConsumeAsMuchAs(
      Proto.ID categoryId,
      Upoints unity,
      Option<IEntity> consumer = default (Option<IEntity>),
      LocStr? extraTitle = null);

    void ConsumeExactly(
      Proto.ID categoryId,
      Upoints unity,
      Option<IEntity> consumer = default (Option<IEntity>),
      LocStr? extraTitle = null);

    void AddConsumer(UnityConsumer consumer);

    void RemoveConsumer(UnityConsumer consumer);

    void GenerateUnity(Proto.ID categoryId, Upoints generated, Upoints? max = null, LocStr? extraTitle = null);
  }
}
