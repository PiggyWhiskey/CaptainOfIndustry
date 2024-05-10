// Decompiled with JetBrains decompiler
// Type: Mafi.CollectionAccessAttribute
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
  /// Indicates how method, constructor invocation, or property access
  /// over collection type affects the contents of the collection.
  /// Use <see cref="P:Mafi.CollectionAccessAttribute.CollectionAccessType" /> to specify the access type.
  /// </summary>
  /// <remarks>
  /// Using this attribute only makes sense if all collection methods are marked with this attribute.
  /// </remarks>
  /// <example><code>
  /// public class MyStringCollection : List&lt;string&gt;
  /// {
  ///   [CollectionAccess(CollectionAccessType.Read)]
  ///   public string GetFirstString()
  ///   {
  ///     return this.ElementAt(0);
  ///   }
  /// }
  /// class Test
  /// {
  ///   public void Foo()
  ///   {
  ///     // Warning: Contents of the collection is never updated
  ///     var col = new MyStringCollection();
  ///     string x = col.GetFirstString();
  ///   }
  /// }
  /// </code></example>
  [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property)]
  public sealed class CollectionAccessAttribute : Attribute
  {
    public CollectionAccessAttribute(CollectionAccessType collectionAccessType)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.CollectionAccessType = collectionAccessType;
    }

    public CollectionAccessType CollectionAccessType { get; }
  }
}
