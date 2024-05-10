// Decompiled with JetBrains decompiler
// Type: Mafi.Collections.QueueueExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

#nullable disable
namespace Mafi.Collections
{
  public static class QueueueExtensions
  {
    /// <summary>
    /// Returns first element or <see cref="F:Mafi.Option`1.None" /> if the queue is empty.
    /// </summary>
    /// <remarks>
    /// This needs to be an extension method because we need to make sure that the generic argument is a class.
    /// </remarks>
    public static Option<T> FirstOrNone<T>(this Queueue<T> queue) where T : class
    {
      return !queue.IsEmpty ? (Option<T>) queue.First : Option<T>.None;
    }

    /// <summary>
    /// Returns last element or <see cref="F:Mafi.Option`1.None" /> if the queue is empty.
    /// </summary>
    /// <remarks>
    /// This needs to be an extension method because we need to make sure that the generic argument is a class.
    /// </remarks>
    public static Option<T> LastOrNone<T>(this Queueue<T> queue) where T : class
    {
      return !queue.IsEmpty ? (Option<T>) queue.Last : Option<T>.None;
    }

    public static T? FirstOrNull<T>(this Queueue<T> queue) where T : struct
    {
      return !queue.IsEmpty ? new T?(queue.First) : new T?();
    }

    public static T? LastOrNull<T>(this Queueue<T> queue) where T : struct
    {
      return !queue.IsEmpty ? new T?(queue.Last) : new T?();
    }
  }
}
