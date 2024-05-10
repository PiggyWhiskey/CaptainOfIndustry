// Decompiled with JetBrains decompiler
// Type: Mafi.Serialization.IIsSafeAsHashKey
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

#nullable disable
namespace Mafi.Serialization
{
  /// <summary>
  /// Marks a class to be safe as a key of hash-based container such as dict or set. This means that the class must
  /// produce hash with is not based in the object ID (which is not stable between game instances).
  /// </summary>
  public interface IIsSafeAsHashKey
  {
  }
}
