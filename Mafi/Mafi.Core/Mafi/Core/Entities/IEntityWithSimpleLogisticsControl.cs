// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.IEntityWithSimpleLogisticsControl
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Entities
{
  public interface IEntityWithSimpleLogisticsControl : IEntity, IIsSafeAsHashKey
  {
    LogisticsControl LogisticsInputControl { get; }

    LogisticsControl LogisticsOutputControl { get; }

    bool IsLogisticsInputDisabled { get; }

    bool IsLogisticsOutputDisabled { get; }

    void SetLogisticsInputDisabled(bool isDisabled);

    void SetLogisticsOutputDisabled(bool isDisabled);
  }
}
