// Decompiled with JetBrains decompiler
// Type: Mafi.Serialization.Generators.TypeSerializationSpec
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Serialization.Generators
{
  public class TypeSerializationSpec
  {
    public const string CTOR_ARG_PREFIX = "arg_";
    public const string OBJ_NAME = "obj";
    public readonly Type Type;
    public readonly string ClassName;
    public readonly string FileName;
    public readonly string Namespace;
    public readonly string[] Usings;
    public readonly MemberWrapper[] Members;
    public readonly string[] InitCalls;
    public readonly MemberWrapper[] CtorArgs;
    public readonly MemberWrapper[] NonCtorArgs;
    public readonly Option<string> SerializeAsSingleton;
    public readonly bool HasCustomDataSerialization;
    public readonly int? TypeNewInSaveVersion;

    public Option<Type> SerializedDueToDerivedClass { get; private set; }

    public bool HasBaseTypeWithSomethingToSerialize { get; private set; }

    public bool HasSomethingToSerialize
    {
      get
      {
        return ((IEnumerable<MemberWrapper>) this.Members).Any<MemberWrapper>((Func<MemberWrapper, bool>) (x => x.IsSerialized));
      }
    }

    public TypeSerializationSpec(
      Type type,
      string className,
      string fileName,
      string nameSpace,
      string[] usings,
      MemberWrapper[] members,
      string[] initCalls,
      MemberWrapper[] ctorArgs = null,
      MemberWrapper[] nonCtorArgs = null,
      Option<string> serializeAsSingleton = default (Option<string>),
      bool hasCustomDataSerialization = false,
      int? typeNewInSaveVersion = null)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Type = type;
      this.ClassName = className;
      this.FileName = fileName;
      this.Namespace = nameSpace;
      this.Usings = usings;
      this.Members = members;
      this.InitCalls = initCalls;
      this.CtorArgs = ctorArgs ?? Array.Empty<MemberWrapper>();
      this.NonCtorArgs = nonCtorArgs ?? this.Members;
      this.SerializeAsSingleton = serializeAsSingleton;
      this.HasCustomDataSerialization = hasCustomDataSerialization;
      this.TypeNewInSaveVersion = typeNewInSaveVersion;
    }

    internal void MarkAsSerializedDueToDerivedClass(Type type)
    {
      this.SerializedDueToDerivedClass = (Option<Type>) type;
    }

    internal void MarkHasBaseTypeWithSomethingToSerialize()
    {
      if (this.SerializeAsSingleton.HasValue)
        throw new Exception("Type '" + this.Type.Name + "' is serialized as singleton and should not have base classes.");
      this.HasBaseTypeWithSomethingToSerialize = true;
    }

    public string GetCtorWithArgsCall()
    {
      return "new " + this.ClassName + "(" + ((IEnumerable<MemberWrapper>) this.CtorArgs).Select<MemberWrapper, string>((Func<MemberWrapper, string>) (x => "arg_" + x.Name)).JoinStrings(", ") + ")";
    }

    public Option<string> GetDeclaringTypeHeader(out Option<string> closingStr)
    {
      Type declaringType = this.Type.DeclaringType;
      if (declaringType == (Type) null)
      {
        closingStr = Option<string>.None;
        return Option<string>.None;
      }
      string declaringTypeHeader = "";
      string str = "";
      for (; declaringType != (Type) null; declaringType = declaringType.DeclaringType)
      {
        declaringTypeHeader = (declaringType.IsPublic ? "public" : (declaringType.DeclaringType == (Type) null ? "internal" : "private")) + " partial " + (declaringType.IsValueType ? "struct" : "class") + " " + CSharpGen.GetCleanTypeName(declaringType, true) + " {\r\n" + declaringTypeHeader;
        str += "\r\n}";
      }
      closingStr = (Option<string>) str;
      return (Option<string>) declaringTypeHeader;
    }

    public string GetOwnerClassHeader()
    {
      return (this.Type.DeclaringType != (Type) null ? (this.Type.IsNestedPublic ? "public" : (this.Type.IsNestedPrivate ? "private" : "internal")) : (this.Type.IsPublic ? "public" : "internal")) + " partial " + (this.Type.IsValueType ? "struct" : "class") + " " + this.ClassName + " {";
    }
  }
}
