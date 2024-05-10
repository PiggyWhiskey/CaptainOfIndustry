// Decompiled with JetBrains decompiler
// Type: Mafi.Core.UnlockingTree.ProtoWithIconUnlock
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Prototypes;
using Mafi.Localization;

#nullable disable
namespace Mafi.Core.UnlockingTree
{
  /// <summary>
  /// Unlocks <see cref="T:Mafi.Core.Prototypes.IProtoWithIcon" />.
  /// </summary>
  public class ProtoWithIconUnlock : ProtoUnlock, IUnlockUnitWithTitleAndIcon, IUnlockNodeUnit
  {
    /// <summary>Proto to be unlocked.</summary>
    public readonly IProtoWithIcon Proto;

    public LocStrFormatted Title => this.Proto.Strings.Name.AsFormatted;

    public LocStrFormatted Description => this.Proto.Strings.DescShort.AsFormatted;

    public Option<string> IconPath => (Option<string>) this.Proto.IconPath;

    public ProtoWithIconUnlock(IProtoWithIcon protoToUnlock, bool hideInUi = false)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector((IProto) protoToUnlock, hideInUi);
      this.Proto = protoToUnlock.CheckNotNull<IProtoWithIcon>();
    }
  }
}
