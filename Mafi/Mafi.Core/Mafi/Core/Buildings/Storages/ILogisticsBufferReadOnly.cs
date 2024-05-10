// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Storages.ILogisticsBufferReadOnly
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Entities.Static;

#nullable disable
namespace Mafi.Core.Buildings.Storages
{
  public interface ILogisticsBufferReadOnly : IProductBufferReadOnly
  {
    /// <summary>
    /// 0-100% of capacity of buffer until which should be imported.
    /// </summary>
    Percent ImportUntilPercent { get; }

    /// <summary>
    /// 0-100% of capacity of buffer from which should be exported.
    /// </summary>
    Percent ExportFromPercent { get; }
  }
}
