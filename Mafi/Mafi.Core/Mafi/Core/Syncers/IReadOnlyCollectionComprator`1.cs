// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Syncers.IReadOnlyCollectionComprator`1
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Syncers
{
  public interface IReadOnlyCollectionComprator<T> : ICollectionComparator<T, IReadOnlyCollection<T>>
  {
  }
}
