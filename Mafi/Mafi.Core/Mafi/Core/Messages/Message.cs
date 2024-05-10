// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Messages.Message
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Messages
{
  [GenerateSerializer(false, null, 0)]
  public class Message
  {
    public readonly MessageProto Proto;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public bool IsRead { get; private set; }

    [DoNotSave(0, null)]
    public LocStrFormatted NotificationTitle { get; private set; }

    [DoNotSave(0, null)]
    public LocStrFormatted Title { get; private set; }

    [DoNotSave(0, null)]
    public ColorRgba BgColor { get; private set; }

    [DoNotSave(0, null)]
    public string IconPath { get; private set; }

    public ImmutableArray<string> Content => this.Proto.Content;

    public Message(MessageProto proto)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Proto = proto;
      this.initAfterLoad();
    }

    public void MarkAsRead() => this.IsRead = true;

    [InitAfterLoad(InitPriority.Normal)]
    private void initAfterLoad()
    {
      switch (this.Proto.MessageType)
      {
        case InGameMessageType.Tutorial:
          this.NotificationTitle = (LocStrFormatted) this.Proto.Strings.Name;
          this.BgColor = (ColorRgba) 3417450;
          this.IconPath = "Assets/Unity/UserInterface/Toolbar/Tutorials.svg";
          break;
        case InGameMessageType.Warning:
          this.NotificationTitle = (LocStrFormatted) this.Proto.Strings.Name;
          this.BgColor = (ColorRgba) 9978897;
          this.IconPath = "Assets/Unity/UserInterface/General/Warning128.png";
          break;
        default:
          this.NotificationTitle = (LocStrFormatted) this.Proto.Strings.Name;
          this.BgColor = (ColorRgba) 3417450;
          this.IconPath = "Assets/Unity/UserInterface/General/Message.svg";
          break;
      }
      this.Title = this.NotificationTitle;
    }

    public static void Serialize(Message value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<Message>(value))
        return;
      writer.EnqueueDataSerialization((object) value, Message.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteBool(this.IsRead);
      writer.WriteGeneric<MessageProto>(this.Proto);
    }

    public static Message Deserialize(BlobReader reader)
    {
      Message message;
      if (reader.TryStartClassDeserialization<Message>(out message))
        reader.EnqueueDataDeserialization((object) message, Message.s_deserializeDataDelayedAction);
      return message;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.IsRead = reader.ReadBool();
      reader.SetField<Message>(this, "Proto", (object) reader.ReadGenericAs<MessageProto>());
      reader.RegisterInitAfterLoad<Message>(this, "initAfterLoad", InitPriority.Normal);
    }

    static Message()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      Message.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Message) obj).SerializeData(writer));
      Message.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Message) obj).DeserializeData(reader));
    }
  }
}
