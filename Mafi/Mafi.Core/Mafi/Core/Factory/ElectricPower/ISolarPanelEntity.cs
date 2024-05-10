// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.ElectricPower.ISolarPanelEntity
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Factory.ElectricPower
{
  /// <summary>
  /// Entity implementing this interface will automatically become a solar panel
  /// and does not need to handle anything or register anywhere in order to work.
  /// </summary>
  public interface ISolarPanelEntity : 
    IStaticEntity,
    IEntityWithPosition,
    IRenderedEntity,
    IEntity,
    IIsSafeAsHashKey,
    IAreaSelectableEntity
  {
    /// <summary>Power generated on full sun.</summary>
    Electricity MaxOutputElectricity { get; }
  }
}
