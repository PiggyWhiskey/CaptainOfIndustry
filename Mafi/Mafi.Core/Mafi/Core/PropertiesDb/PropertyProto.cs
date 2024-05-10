// Decompiled with JetBrains decompiler
// Type: Mafi.Core.PropertiesDb.PropertyProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Prototypes;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.PropertiesDb
{
  public abstract class PropertyProto : Proto
  {
    [OnlyForSaveCompatibility(null)]
    public abstract string PropertyId { get; }

    protected PropertyProto(Proto.ID id)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, Proto.Str.Empty);
    }

    public abstract IProperty CreateProperty(IPropertiesDbInternal propsDb);
  }
}
