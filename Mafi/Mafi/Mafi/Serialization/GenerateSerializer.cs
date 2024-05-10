// Decompiled with JetBrains decompiler
// Type: Mafi.Serialization.GenerateSerializer
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System;

#nullable disable
namespace Mafi.Serialization
{
  /// <summary>
  /// Mark any partial class / struct with this attribute to get its serializers automatically generated for all fields
  /// and settable properties. If you wish to not save some of the member, mark them with <see cref="T:Mafi.Serialization.DoNotSaveAttribute" />.
  /// 
  /// When saving class:
  /// - Empty class will be created (ctors will be skipped)
  /// - All fields, props will be set via reflection
  /// - LoadCtor annotation is forbidden
  /// - Classes are always serialized via reference.
  /// 
  /// When saving struct:
  /// - Ctor marked with <see cref="T:Mafi.Serialization.LoadCtorAttribute" /> will be used. Or if there is just one, then that one.ManuallyWrittenSerializationAttribute
  /// - Ctor args must have the same name as the fields so the generator can match them and populate the ctor.
  /// - You are not allowed to do any work in your load ctor!!ManuallyWrittenSerializationAttribute
  /// 
  /// NOTE: Deserialization of structs would require boxing / unboxing and would be 10 times slower then via ctor.
  /// Which on millions of instance can account for several seconds.
  /// </summary>
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
  public sealed class GenerateSerializer : Attribute
  {
    public readonly bool CustomClassDataSerialization;
    public readonly Option<string> SerializeAsSingleton;
    public readonly int? NewInVersion;

    public GenerateSerializer(
      bool customClassDataSerialization = false,
      string serializeAsSingleton = null,
      int newInVersion = 0)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.CustomClassDataSerialization = customClassDataSerialization;
      this.SerializeAsSingleton = (Option<string>) serializeAsSingleton;
      this.NewInVersion = newInVersion == 0 ? new int?() : new int?(newInVersion);
    }
  }
}
