// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Props.PropsRemovalProcessor
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Input;
using Mafi.Core.Population;
using Mafi.Core.Terrain.Trees;
using Mafi.Localization;
using System.Linq;

#nullable disable
namespace Mafi.Core.Terrain.Props
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  public class PropsRemovalProcessor : 
    ICommandProcessor<QuickRemovePropsCmd>,
    IAction<QuickRemovePropsCmd>
  {
    public static readonly Upoints COST_PER_PROP;
    private readonly UpointsManager m_upointsManager;
    private readonly TerrainPropsManager m_propsManager;
    private readonly TreesManager m_treesManager;

    public PropsRemovalProcessor(
      UpointsManager upointsManager,
      TerrainPropsManager propsManager,
      TreesManager treesManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_upointsManager = upointsManager;
      this.m_propsManager = propsManager;
      this.m_treesManager = treesManager;
    }

    public void Invoke(QuickRemovePropsCmd cmd)
    {
      int num = 0;
      Quantity quantity = this.m_upointsManager.Quantity;
      Quantity quantityRounded = PropsRemovalProcessor.COST_PER_PROP.GetQuantityRounded();
      foreach (TerrainPropData terrainPropData in this.m_propsManager.EnumeratePropsInArea(cmd.Area).ToArray<TerrainPropData>())
      {
        if (!(quantity < quantityRounded))
        {
          if (this.m_propsManager.TryRemoveProp(terrainPropData.Id))
          {
            quantity -= quantityRounded;
            ++num;
          }
        }
        else
          break;
      }
      foreach (TreeId id in this.m_treesManager.EnumerateStumpsInArea(cmd.Area).ToArray<TreeId>())
      {
        if (!(quantity < quantityRounded))
        {
          if (this.m_treesManager.TryRemoveStump(id))
          {
            quantity -= quantityRounded;
            ++num;
          }
        }
        else
          break;
      }
      if (num > 0)
      {
        this.m_upointsManager.ConsumeAsMuchAs(IdsCore.UpointsCategories.QuickRemove, num * PropsRemovalProcessor.COST_PER_PROP, new Option<IEntity>(), new LocStr?());
        cmd.SetResultSuccess();
      }
      else
        cmd.SetResultError("Didn't remove anything");
    }

    static PropsRemovalProcessor()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      PropsRemovalProcessor.COST_PER_PROP = 0.2.Upoints();
    }
  }
}
