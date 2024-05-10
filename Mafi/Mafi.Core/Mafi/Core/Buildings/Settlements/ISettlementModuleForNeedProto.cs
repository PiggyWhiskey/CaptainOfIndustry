// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Settlements.ISettlementModuleForNeedProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Population;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Buildings.Settlements
{
  public interface ISettlementModuleForNeedProto : 
    ILayoutEntityProto,
    IStaticEntityProto,
    IEntityProto,
    IProto
  {
    PopNeedProto PopsNeed { get; }
  }
}
