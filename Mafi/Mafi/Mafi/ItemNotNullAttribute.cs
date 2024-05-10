// Decompiled with JetBrains decompiler
// Type: Mafi.ItemNotNullAttribute
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System;

#nullable disable
namespace Mafi
{
  /// <summary>
  /// Can be applied to symbols of types derived from IEnumerable as well as to symbols of Task
  /// and Lazy classes to indicate that the value of a collection item, of the Task.Result property
  /// or of the Lazy.Value property can never be null.
  /// </summary>
  /// <example><code>
  /// public void Foo([ItemNotNull]List&lt;string&gt; books)
  /// {
  ///   foreach (var book in books) {
  ///     if (book != null) // Warning: Expression is always true
  ///      Console.WriteLine(book.ToUpper());
  ///   }
  /// }
  /// </code></example>
  [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Delegate)]
  public sealed class ItemNotNullAttribute : Attribute
  {
    public ItemNotNullAttribute()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
