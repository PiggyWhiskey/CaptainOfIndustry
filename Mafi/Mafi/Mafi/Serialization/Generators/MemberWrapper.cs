// Decompiled with JetBrains decompiler
// Type: Mafi.Serialization.Generators.MemberWrapper
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System;

#nullable disable
namespace Mafi.Serialization.Generators
{
  public class MemberWrapper
  {
    public readonly Type Owner;
    public readonly bool IsField;
    public readonly string Name;
    public readonly Type Type;
    public readonly bool LoadViaReflection;
    public readonly Option<string> NewInstanceOnLoad;
    public readonly int? DeprecatedInSaveVersion;
    public readonly int? RemovedInSaveVersion;
    public readonly string NameForSorting;
    public readonly Option<string> CustomValueWhenNotLoaded;
    public readonly Option<Type> GlobalDepTypeOverride;
    public readonly bool SerializeUsingNonVariableEncoding;
    public readonly bool SerializeNullAsEmptyArray;

    public int? NewInSaveVersion { get; private set; }

    public Option<Type> DefaultValueFromResolver { get; private set; }

    public bool IsCtorArg { get; private set; }

    public bool IsLoadedAsGlobalDep { get; private set; }

    public bool IsDirectCallSerializationDisabled { get; private set; }

    public bool ShouldAssignToObj { get; private set; }

    /// <summary>
    /// Whether this member is actually serialized to the save file.
    /// </summary>
    public bool IsSerialized
    {
      get
      {
        return this.NewInstanceOnLoad.IsNone && !this.IsLoadedAsGlobalDep && !this.DeprecatedInSaveVersion.HasValue && !this.RemovedInSaveVersion.HasValue;
      }
    }

    public MemberWrapper(
      Type owner,
      bool isField,
      string name,
      Type type,
      bool loadViaReflection,
      Option<string> newInstanceOnLoad,
      int? newInSaveVersion,
      int? deprecatedInSaveVersion,
      int? removedInSaveVersion,
      string nameForSorting,
      Option<string> customValueWhenNotLoaded,
      Option<Type> defaultValueFromResolver,
      Option<Type> globalDepTypeOverride,
      bool serializeUsingNonVariableEncoding,
      bool isLoadedAsGlobalDep,
      bool serializeNullAsEmptyArray)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Owner = owner;
      this.IsField = isField;
      this.Name = name;
      this.Type = type;
      this.LoadViaReflection = loadViaReflection;
      this.NewInstanceOnLoad = newInstanceOnLoad;
      this.NewInSaveVersion = newInSaveVersion;
      this.DeprecatedInSaveVersion = deprecatedInSaveVersion;
      this.RemovedInSaveVersion = removedInSaveVersion;
      this.NameForSorting = nameForSorting;
      this.CustomValueWhenNotLoaded = customValueWhenNotLoaded;
      this.DefaultValueFromResolver = defaultValueFromResolver;
      this.GlobalDepTypeOverride = globalDepTypeOverride;
      this.SerializeUsingNonVariableEncoding = serializeUsingNonVariableEncoding;
      this.IsLoadedAsGlobalDep = isLoadedAsGlobalDep;
      this.SerializeNullAsEmptyArray = serializeNullAsEmptyArray;
    }

    internal void MarkLoadedAsGlobalDep() => this.IsLoadedAsGlobalDep = true;

    internal void MarkIsCtorArg() => this.IsCtorArg = true;

    internal void DisableDirectCallSerialization() => this.IsDirectCallSerializationDisabled = true;

    internal void MarkAssignToObj() => this.ShouldAssignToObj = true;

    internal void MarkGlobalDependencyNewlySerializableInVersion(int version)
    {
      Assert.That<bool>(this.IsLoadedAsGlobalDep).IsFalse();
      Assert.That<int?>(this.NewInSaveVersion).IsNull<int>("Not supported yet.");
      Assert.That<Option<Type>>(this.DefaultValueFromResolver).IsNone<Type>("Not supported yet.");
      this.NewInSaveVersion = new int?(version);
      this.DefaultValueFromResolver = (Option<Type>) this.Type;
    }
  }
}
