// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Messages.MessageGroupsData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Messages;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Base.Prototypes.Messages
{
  internal class MessageGroupsData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      ProtosDb prototypesDb = registrator.PrototypesDb;
      prototypesDb.Add<MessageGroupProto>(new MessageGroupProto(Ids.MessageGroups.GettingStarted, "Getting started", "name of tutorial group for initial tutorials", 0));
      prototypesDb.Add<MessageGroupProto>(new MessageGroupProto(Ids.MessageGroups.Terraforming, "Terraforming", "name of tutorial group about terraforming (mining, dumping)", 1));
      prototypesDb.Add<MessageGroupProto>(new MessageGroupProto(Ids.MessageGroups.General, "General", "name of tutorial group about general topics", 2));
      prototypesDb.Add<MessageGroupProto>(new MessageGroupProto(Ids.MessageGroups.FoodProduction, "Food production", "name of tutorial group about food production", 3));
      prototypesDb.Add<MessageGroupProto>(new MessageGroupProto(Ids.MessageGroups.Settlement, "Settlement", "name of tutorial group about settlements", 4));
      prototypesDb.Add<MessageGroupProto>(new MessageGroupProto(Ids.MessageGroups.Logistics, "Logistics", "name of tutorial group about logistics (truck, belts, ships)", 5));
      prototypesDb.Add<MessageGroupProto>(new MessageGroupProto(Ids.MessageGroups.World, "World", "name of tutorial group about world map (world settlements, exploration, ships)", 6));
      prototypesDb.Add<MessageGroupProto>(new MessageGroupProto(Ids.MessageGroups.Tools, "Tools", "name of tutorial group about game tools (copy, paste, pause)", 7));
      prototypesDb.Add<MessageGroupProto>(new MessageGroupProto(Ids.MessageGroups.Warnings, "Warnings", "name of messages group for warnings", 8));
    }

    public MessageGroupsData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
