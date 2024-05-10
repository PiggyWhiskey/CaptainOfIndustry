// Decompiled with JetBrains decompiler
// Type: Mafi.IOptionNonGeneric
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

#nullable disable
namespace Mafi
{
  /// <summary>
  /// Non-generic interface for access. Keep in mind that access through this interface will box the Option. Use only
  /// if the option is already boxed.
  /// </summary>
  internal interface IOptionNonGeneric
  {
    bool HasValue { get; }

    object Value { get; }

    object ValueOrNull { get; }
  }
}
