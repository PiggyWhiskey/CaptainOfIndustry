// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Messages.MessageProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Prototypes;
using Mafi.Localization;
using System;
using System.Text;

#nullable disable
namespace Mafi.Core.Messages
{
  public class MessageProto : Proto
  {
    /// <summary>
    /// Whether this message should force open on the screen once delivered. This is used
    /// for the welcome message.
    /// </summary>
    public readonly bool ForceOpen;
    public readonly InGameMessageType MessageType;
    public readonly ImmutableArray<string> Content;
    public readonly int CurrentVersion;
    public readonly Option<MessageGroupProto> Group;
    /// <summary>
    /// This message should always be notified to the player no matter if they saw it in a previous game sessions.
    /// </summary>
    public readonly bool AlwaysNotify;
    public readonly bool UnlockSilentlyFromStart;

    public MessageProto(
      Proto.ID id,
      LocStr title,
      ImmutableArray<string> content,
      InGameMessageType messageType,
      bool forceOpen = false,
      bool alwaysNotify = false,
      bool unlockSilentlyFromStart = false,
      MessageGroupProto group = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.CurrentVersion = 1;
      // ISSUE: explicit constructor call
      base.\u002Ector(id, new Proto.Str(title));
      this.Content = content;
      this.MessageType = messageType;
      this.ForceOpen = forceOpen;
      this.AlwaysNotify = alwaysNotify;
      this.UnlockSilentlyFromStart = unlockSilentlyFromStart;
      this.Group = (Option<MessageGroupProto>) group;
    }

    public MessageProto(
      Proto.ID id,
      LocStr title,
      string content,
      InGameMessageType messageType,
      bool forceOpen = false,
      bool alwaysNotify = false,
      bool unlockSilentlyFromStart = false,
      MessageGroupProto group = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(id, title, MessageProto.preProcessContent(content), messageType, forceOpen, alwaysNotify, unlockSilentlyFromStart, group);
    }

    public MessageProto(
      Proto.ID id,
      string title,
      string content,
      InGameMessageType messageType,
      bool forceOpen = false,
      bool alwaysNotify = false,
      bool unlockSilentlyFromStart = false,
      MessageGroupProto group = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(id, Loc.Str(id.Value + "__name", title, "title of message or tutorial"), MessageProto.preProcessContent(content), messageType, forceOpen, alwaysNotify, unlockSilentlyFromStart, group);
    }

    private static ImmutableArray<string> preProcessContent(string str)
    {
      string[] strArray = str.Split('\n');
      Lyst<string> lyst = new Lyst<string>();
      StringBuilder stringBuilder = new StringBuilder();
      foreach (string str1 in strArray)
      {
        string str2 = str1.Trim();
        if (str2 == "")
        {
          if (stringBuilder.Length > 0)
          {
            lyst.Add(stringBuilder.ToString().Trim());
            stringBuilder.Clear();
          }
        }
        else if (str2.StartsWith("<", StringComparison.Ordinal) && str2.EndsWith(">", StringComparison.Ordinal))
        {
          if (stringBuilder.Length > 0)
          {
            lyst.Add(stringBuilder.ToString().Trim());
            stringBuilder.Clear();
          }
          lyst.Add(str2);
        }
        else
        {
          stringBuilder.Append(" ");
          stringBuilder.Append(str2);
        }
      }
      if (stringBuilder.Length > 0)
      {
        lyst.Add(stringBuilder.ToString().Trim());
        stringBuilder.Clear();
      }
      return lyst.ToImmutableArray();
    }
  }
}
