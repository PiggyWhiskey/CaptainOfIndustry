// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Messages.MessageGroupProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Prototypes;
using Mafi.Localization;

#nullable disable
namespace Mafi.Core.Messages
{
  public class MessageGroupProto : Proto
  {
    public readonly int Order;

    public MessageGroupProto(Proto.ID id, string title, string translationComment, int order)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, new Proto.Str(Loc.Str(string.Format("{0}{1}", (object) id, (object) "__name"), title, translationComment)));
      this.Order = order;
    }
  }
}
