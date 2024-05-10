// Decompiled with JetBrains decompiler
// Type: Mafi.Serialization.Generators.MembersGenerator
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using System;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace Mafi.Serialization.Generators
{
  public class MembersGenerator
  {
    public static readonly IReadOnlyDictionary<Type, string> TYPE_TO_WRITE_METHOD_NAME;
    public static readonly IReadOnlyDictionary<Type, string> TYPE_TO_READ_METHOD_NAME;

    public static string GenerateWriterFor(MemberWrapper member)
    {
      if (member.NewInstanceOnLoad.HasValue)
        return "// Skip '" + member.Name + "' (new instance on load)";
      if (member.IsLoadedAsGlobalDep)
        return "// Skip '" + member.Name + "' (resolved dependency)";
      return member.RemovedInSaveVersion.HasValue || member.DeprecatedInSaveVersion.HasValue ? "// Skip '" + member.Name + "' (deprecated)" : MembersGenerator.getWriteCodeStr(member);
    }

    /// <summary>
    /// Returns code that writes a value of given name and type to the blob writer.
    /// </summary>
    private static string getWriteCodeStr(MemberWrapper member)
    {
      Type type = member.Type;
      string str1 = member.Name;
      if (member.Owner.IsValueType)
        str1 = "value." + str1;
      if (type.IsEnum)
      {
        Type underlyingType = Enum.GetUnderlyingType(type);
        return "writer." + MembersGenerator.TYPE_TO_WRITE_METHOD_NAME[underlyingType] + "((" + CSharpGen.GetCleanTypeName(underlyingType) + ")" + str1 + ");";
      }
      if (type.IsArray)
      {
        string cleanTypeName = CSharpGen.GetCleanTypeName(type.GetElementType());
        return "writer.WriteArray(" + str1 + (member.SerializeNullAsEmptyArray ? " ?? Array.Empty<" + cleanTypeName + ">()" : "") + ");";
      }
      if (type.IsGenericParameter || type.IsInterface || type.IsAbstract || member.IsDirectCallSerializationDisabled)
        return "writer.WriteGeneric(" + str1 + ");";
      if (type.IsValueType && type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Nullable<>))
        return "writer.WriteNullableStruct(" + str1 + ");";
      string str2;
      if (!MembersGenerator.TYPE_TO_WRITE_METHOD_NAME.TryGetValue(type, out str2))
        return MembersGenerator.getCleanTypeNameNotCollidingWithMembers(type, member.Owner) + ".Serialize(" + str1 + ", writer);";
      string str3 = member.SerializeUsingNonVariableEncoding ? "NonVariable" : "";
      return "writer." + str2 + str3 + "(" + str1 + ");";
    }

    public static string GenerateReaderFor(
      MemberWrapper member,
      string linePrefix,
      bool skipDeprecated = false)
    {
      if (member.IsLoadedAsGlobalDep)
      {
        Type type1 = member.GlobalDepTypeOverride.ValueOrNull;
        if ((object) type1 == null)
          type1 = member.Type;
        Type type2 = type1;
        return linePrefix + "reader.RegisterResolvedMember(this, nameof(" + member.Name + "), typeof(" + CSharpGen.GetCleanTypeName(type2) + "), " + (member.IsField ? "true" : "false") + ");";
      }
      Assert.That<Option<Type>>(member.GlobalDepTypeOverride).IsNone<Type, string, string>("Member '{0}' on type '{1}' has global dep type override but it is not being loaded as global dep.", member.Name, member.Owner.Name);
      string str1 = MembersGenerator.getReaderCodeStr(member);
      if (member.RemovedInSaveVersion.HasValue && !skipDeprecated)
      {
        string str2 = "";
        if (member.NewInstanceOnLoad.HasValue)
        {
          str2 = MembersGenerator.GenerateReaderFor(member, linePrefix, true) + "\r\n";
          str1 = MembersGenerator.getReaderCodeStr(member, true);
        }
        string readerFor;
        if (member.NewInSaveVersion.HasValue)
          readerFor = str2 + string.Format("{0}if (reader.{1} >= {2} ", (object) linePrefix, (object) "LoadedSaveVersion", (object) member.NewInSaveVersion.Value) + string.Format("&& reader.{0} < {1}) ", (object) "LoadedSaveVersion", (object) member.RemovedInSaveVersion.Value) + "{ " + str1 + "; } // Removed member '" + member.Name + "'";
        else
          readerFor = str2 + string.Format("{0}if (reader.{1} < {2}) ", (object) linePrefix, (object) "LoadedSaveVersion", (object) member.RemovedInSaveVersion.Value) + "{ " + str1 + "; } // Removed member '" + member.Name + "'";
        return readerFor;
      }
      if (member.DeprecatedInSaveVersion.HasValue && !skipDeprecated)
        return string.Format("{0}if (reader.{1} < {2}) ", (object) linePrefix, (object) "LoadedSaveVersion", (object) member.DeprecatedInSaveVersion.Value) + "{ " + MembersGenerator.GenerateReaderFor(member, "", true) + " } // Deprecated member '" + member.Name + "'";
      Option<Type> loadDefaultFromResolver = Option<Type>.None;
      if (member.NewInSaveVersion.HasValue)
      {
        loadDefaultFromResolver = member.DefaultValueFromResolver;
        str1 = string.Format("reader.{0} >= {1} ", (object) "LoadedSaveVersion", (object) member.NewInSaveVersion.Value) + "? " + str1 + " : " + (member.CustomValueWhenNotLoaded.ValueOrNull ?? "default");
      }
      if (member.IsCtorArg)
      {
        if (loadDefaultFromResolver.HasValue)
          throw new Exception("Cannot load default from resolver for member '" + member.Name + "'.");
        return linePrefix + "var arg_" + member.Name + " = " + str1 + ";";
      }
      if (member.LoadViaReflection)
      {
        string str3 = member.IsField ? "SetField" : "SetProperty";
        return linePrefix + "reader." + str3 + "(this, nameof(" + member.Name + "), " + str1 + ");" + getDefaultFromResolver("this");
      }
      return member.ShouldAssignToObj ? linePrefix + "obj." + member.Name + " = " + str1 + ";" + getDefaultFromResolver("obj") : linePrefix + member.Name + " = " + str1 + ";" + getDefaultFromResolver("this");

      string getDefaultFromResolver(string objName)
      {
        if (loadDefaultFromResolver.IsNone)
          return "";
        return string.Format("\r\n{0}if (reader.{1} < {2}) ", (object) linePrefix, (object) "LoadedSaveVersion", (object) member.NewInSaveVersion.Value) + "{ reader.RegisterResolvedMember(" + objName + ", nameof(" + member.Name + "), typeof(" + CSharpGen.GetCleanTypeName(loadDefaultFromResolver.Value) + "), " + (member.IsField ? "true" : "false") + "); }";
      }
    }

    /// <summary>
    /// Returns code that reads and returns a value for given member.
    /// </summary>
    private static string getReaderCodeStr(MemberWrapper member, bool originalMember = false)
    {
      Type type = member.Type;
      if (member.NewInstanceOnLoad.HasValue && !originalMember)
        return member.NewInstanceOnLoad.Value != "" ? (!(member.NewInstanceOnLoad.Value == "new()") ? member.NewInstanceOnLoad.Value : "new " + CSharpGen.GetCleanTypeName(type) + "()") : (type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Option<>) ? "Option.Some(new " + CSharpGen.GetCleanTypeName(type.GenericTypeArguments[0]) + "())" : "new " + CSharpGen.GetCleanTypeName(type) + "()");
      if (type.IsEnum)
      {
        Type underlyingType = Enum.GetUnderlyingType(type);
        return "(" + CSharpGen.GetCleanTypeName(type) + ")reader." + MembersGenerator.TYPE_TO_READ_METHOD_NAME[underlyingType] + "()";
      }
      if (type.IsArray)
        return "reader.ReadArray<" + CSharpGen.GetCleanTypeName(type.GetElementType()) + ">()";
      if (type.IsGenericParameter || type.IsInterface || type.IsAbstract || member.IsDirectCallSerializationDisabled)
        return "reader.ReadGenericAs<" + CSharpGen.GetCleanTypeName(type) + ">()";
      if (type.IsValueType && type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Nullable<>))
        return "reader.ReadNullableStruct<" + CSharpGen.GetCleanTypeName(type.GetGenericArguments()[0]) + ">()";
      string str1;
      if (!MembersGenerator.TYPE_TO_READ_METHOD_NAME.TryGetValue(type, out str1))
        return MembersGenerator.getCleanTypeNameNotCollidingWithMembers(type, member.Owner) + ".Deserialize(reader)";
      string str2 = member.SerializeUsingNonVariableEncoding ? "NonVariable" : "";
      return "reader." + str1 + str2 + "()";
    }

    private static string getCleanTypeNameNotCollidingWithMembers(
      Type typeToConvert,
      Type ownerClassType)
    {
      string cleanTypeName = CSharpGen.GetCleanTypeName(typeToConvert);
      if (ownerClassType.GetField(cleanTypeName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy) != (FieldInfo) null || ownerClassType.GetProperty(cleanTypeName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy) != (PropertyInfo) null)
        cleanTypeName = CSharpGen.GetCleanTypeName(typeToConvert, includeNamespace: true);
      return cleanTypeName;
    }

    public MembersGenerator()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static MembersGenerator()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      MembersGenerator.TYPE_TO_WRITE_METHOD_NAME = (IReadOnlyDictionary<Type, string>) new Dict<Type, string>()
      {
        {
          typeof (bool),
          "WriteBool"
        },
        {
          typeof (sbyte),
          "WriteSByte"
        },
        {
          typeof (byte),
          "WriteByte"
        },
        {
          typeof (char),
          "WriteChar"
        },
        {
          typeof (short),
          "WriteShort"
        },
        {
          typeof (ushort),
          "WriteUShort"
        },
        {
          typeof (int),
          "WriteInt"
        },
        {
          typeof (uint),
          "WriteUInt"
        },
        {
          typeof (long),
          "WriteLong"
        },
        {
          typeof (ulong),
          "WriteULong"
        },
        {
          typeof (float),
          "WriteFloat"
        },
        {
          typeof (double),
          "WriteDouble"
        },
        {
          typeof (string),
          "WriteString"
        },
        {
          typeof (Type),
          "WriteType"
        },
        {
          typeof (object),
          "WriteGeneric"
        },
        {
          typeof (DateTime),
          "WriteDateTime"
        }
      };
      MembersGenerator.TYPE_TO_READ_METHOD_NAME = (IReadOnlyDictionary<Type, string>) new Dict<Type, string>()
      {
        {
          typeof (bool),
          "ReadBool"
        },
        {
          typeof (byte),
          "ReadByte"
        },
        {
          typeof (sbyte),
          "ReadSByte"
        },
        {
          typeof (char),
          "ReadChar"
        },
        {
          typeof (short),
          "ReadShort"
        },
        {
          typeof (ushort),
          "ReadUShort"
        },
        {
          typeof (int),
          "ReadInt"
        },
        {
          typeof (uint),
          "ReadUInt"
        },
        {
          typeof (long),
          "ReadLong"
        },
        {
          typeof (ulong),
          "ReadULong"
        },
        {
          typeof (float),
          "ReadFloat"
        },
        {
          typeof (double),
          "ReadDouble"
        },
        {
          typeof (string),
          "ReadString"
        },
        {
          typeof (Type),
          "ReadType"
        },
        {
          typeof (object),
          "ReadGenericAs<object>"
        },
        {
          typeof (DateTime),
          "ReadDateTime"
        }
      };
    }
  }
}
