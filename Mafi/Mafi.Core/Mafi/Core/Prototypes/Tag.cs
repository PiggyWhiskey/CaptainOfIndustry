// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Prototypes.Tag
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using System;
using System.Diagnostics;

#nullable disable
namespace Mafi.Core.Prototypes
{
  /// <summary>
  /// Tag that can be assigned to the <see cref="T:Mafi.Core.Prototypes.Proto" /> to allow easy filtering.
  /// </summary>
  [DebuggerDisplay("Tag {Id}")]
  public class Tag
  {
    public readonly Type TargetType;
    public readonly string Id;

    public static Tag Create<T>(string id) => new Tag(typeof (T), id);

    protected Tag(Type targetType, string id)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.TargetType = targetType.CheckNotNull<Type>();
      this.Id = id.CheckNotNull<string>();
    }

    public override string ToString() => this.Id;
  }
}
