// Decompiled with JetBrains decompiler
// Type: Mafi.ValueProviderAttribute
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
  /// Use this annotation to specify a type that contains static or const fields
  /// with values for the annotated property/field/parameter.
  /// The specified type will be used to improve completion suggestions.
  /// </summary>
  /// <example><code>
  /// namespace TestNamespace
  /// {
  ///   public class Constants
  ///   {
  ///     public static int INT_CONST = 1;
  ///     public const string STRING_CONST = "1";
  ///   }
  /// 
  ///   public class Class1
  ///   {
  ///     [ValueProvider("TestNamespace.Constants")] public int myField;
  ///     public void Foo([ValueProvider("TestNamespace.Constants")] string str) { }
  /// 
  ///     public void Test()
  ///     {
  ///       Foo(/*try completion here*/);//
  ///       myField = /*try completion here*/
  ///     }
  ///   }
  /// }
  /// </code></example>
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true)]
  public sealed class ValueProviderAttribute : Attribute
  {
    public ValueProviderAttribute([NotNull] string name)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Name = name;
    }

    [NotNull]
    public string Name { get; }
  }
}
