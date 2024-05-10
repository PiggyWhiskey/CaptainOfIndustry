// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Trees.TreesCommandsProcessor
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Terrain.Trees
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class TreesCommandsProcessor : 
    ICommandProcessor<PrepareManualPlantTreeCmd>,
    IAction<PrepareManualPlantTreeCmd>,
    ICommandProcessor<RemoveManualPlantTreeCmd>,
    IAction<RemoveManualPlantTreeCmd>
  {
    private readonly ProtosDb m_protosDb;
    private readonly UnlockedProtosDb m_unlockedProtosDb;
    private readonly ITreePlantingManager m_treeManager;

    public TreesCommandsProcessor(
      ProtosDb protosDb,
      UnlockedProtosDb unlockedProtosDb,
      ITreePlantingManager treeManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_protosDb = protosDb;
      this.m_unlockedProtosDb = unlockedProtosDb;
      this.m_treeManager = treeManager;
    }

    public void Invoke(PrepareManualPlantTreeCmd cmd)
    {
      TreeProto proto;
      if (!this.m_protosDb.TryGetProto<TreeProto>(cmd.ProtoId, out proto))
        cmd.SetResultError(false, string.Format("Unknown proto '{0}'", (object) cmd.ProtoId));
      else if (!this.m_unlockedProtosDb.IsUnlocked((Proto) proto))
        cmd.SetResultError(false, string.Format("Proto '{0}' is not unlocked.", (object) cmd.ProtoId));
      else if (!this.m_treeManager.TryAddManualTree(proto, cmd.Transform.Position.Xy.CenterTile2f, cmd.Transform.Rotation.Angle.ToSlim()))
        cmd.SetResultError(false, string.Format("Failed to add tree '{0}'.", (object) cmd.ProtoId));
      else
        cmd.SetResultSuccess(true);
    }

    public void Invoke(RemoveManualPlantTreeCmd cmd)
    {
      if (!this.m_treeManager.TryRemoveManualTree((Tile2i) cmd.Id.Position, false))
        cmd.SetResultError(false, string.Format("Failed to remove tree '{0}'.", (object) cmd.Id.Position));
      else
        cmd.SetResultSuccess(true);
    }
  }
}
