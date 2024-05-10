// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Messages.ChatMessagesData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Messages;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Localization;
using System;

#nullable disable
namespace Mafi.Base.Prototypes.Messages
{
  internal class ChatMessagesData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      ProtosDb prototypesDb = registrator.PrototypesDb;
      MessageGroupProto orThrow = prototypesDb.GetOrThrow<MessageGroupProto>(Ids.MessageGroups.GettingStarted);
      LocStr locStr1 = Loc.Str("MessageWelcome__part1", "Captain, we have found the island we’ve been searching for, it was not on the map! Our ship took a lot of damage and will need major repairs in order to sail again but we should be safe here.", "");
      LocStr1 locStr1_1 = Loc.Str1("MessageWelcome__part2V2", "Besides our initial supplies we are starting from scratch. We should start manufacturing {0} to be able to build our infrastructure, <b>grow food</b> to feed our people, and find a way to <b>make fuel</b> for our vehicles.", "{0} = Construction parts");
      LocStr locStr2 = Loc.Str("MessageWelcome__part3V2", "The island looks abandoned but it has plenty of natural resources that we could use. There are even some <b>abandoned buildings</b> around that we could <b>disassemble</b> for scrap.", "");
      LocStr1 locStr1_2 = Loc.Str1("MessageWelcome__part4V2", "Also we should set up a {0} so we can reinvent all the technologies we took for granted for so long.", "{0} = Research Lab");
      LocStr1 locStr1_3 = Loc.Str1("MessageWelcome__part5", "We found out about a settlement nearby we can <b>trade with</b> in case we run out of something. They can deliver the goods to us once we have a {0}.", "{0} = Trading Dock");
      LocStr locStr3 = Loc.Str("MessageWelcome__part6", "The entire crew is counting on you after everything we have been through together. Good luck!", "");
      string content1 = "{0}\n\n{1}\n\n{2}\n\n{3}\n\n{4}\n\n{5}".FormatInvariant((object) locStr1.TranslatedString, (object) locStr1_1.Format(Ids.Products.ConstructionParts.TranslatedName(prototypesDb).MakeBold()), (object) locStr2.TranslatedString, (object) locStr1_2.Format(Ids.Buildings.ResearchLab1.TranslatedName(prototypesDb).MakeBold()), (object) locStr1_3.Format(Ids.Buildings.TradeDock.TranslatedName(prototypesDb).MakeBold()), (object) locStr3.TranslatedString);
      MessageProto messageProto1 = prototypesDb.Add<MessageProto>(new MessageProto(Ids.Messages.MessageWelcome, "Welcome Captain!", content1, InGameMessageType.Message, true, true, group: orThrow));
      prototypesDb.Add<MessageTriggerDelayedProto>(new MessageTriggerDelayedProto(messageProto1, 1));
      LocStr title = Loc.Str("MessageOnVictory__title", "Congratulations!", "victory message caption");
      string content2 = "{0}\n\n{1}\n\n{2}\n\nMarek & Filip".FormatInvariant((object) Loc.Str("MessageOnVictory__part1", "Congratulations, Captain! Against all the odds, you have built a thriving industrial empire and launched a rocket to space!", "victory message (part 1)").TranslatedString, (object) Loc.Str("MessageOnVictory__part2", "This is the end of content of this the Early Access version of Captain of Industry, but there are still a lot of things to explore! You can keep playing on this map for as long as you want, or try different maps and higher difficulty settings.", "victory message (part 2)").TranslatedString, (object) Loc.Str("MessageOnVictory__part3", "If you have any feedback, please reach out to us on our Discord server or via Steam forums, we’d love to hear from you!", "victory message (part 3)").TranslatedString);
      MessageProto messageProto2 = prototypesDb.Add<MessageProto>(new MessageProto(Ids.Messages.MessageGameVictory, title, content2, InGameMessageType.Message, true, true));
      prototypesDb.Add<MessageTriggerOnEventProto>(new MessageTriggerOnEventProto(messageProto2, 10, (Func<IResolver, IEvent>) (resolver => resolver.Resolve<GameVictoryManager>().OnGameVictory)));
    }

    public ChatMessagesData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
