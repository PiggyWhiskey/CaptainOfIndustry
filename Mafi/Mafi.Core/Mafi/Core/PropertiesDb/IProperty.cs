// Decompiled with JetBrains decompiler
// Type: Mafi.Core.PropertiesDb.IProperty
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using System.Text;

#nullable disable
namespace Mafi.Core.PropertiesDb
{
  public interface IProperty
  {
    string Id { get; }

    /// <summary>Is meant for debug only, not regular use.</summary>
    void SetValueFromString(string value);

    void DumpInfo(StringBuilder sb);
  }
}
