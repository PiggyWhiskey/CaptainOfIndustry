// Decompiled with JetBrains decompiler
// Type: Mafi.Serialization.CSharpGen
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

#nullable disable
namespace Mafi.Serialization
{
  public class CSharpGen
  {
    private static readonly Dict<Type, string> PRIM_TYPE_TO_NAME;
    private readonly Func<object, string>[] m_customObjectProviders;
    private readonly Func<ParameterData, string>[] m_customParamsProviders;
    private readonly StringBuilder m_sb;

    public CSharpGen(
      Func<object, string>[] customObjectProviders = null,
      Func<ParameterData, string>[] customParamsProviders = null)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.m_sb = new StringBuilder();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_customObjectProviders = customObjectProviders ?? Array.Empty<Func<object, string>>();
      this.m_customParamsProviders = customParamsProviders ?? Array.Empty<Func<ParameterData, string>>();
    }

    public string GenerateCsharpFor(
      object obj,
      Set<string> namespaces = null,
      int indentLevel = 0,
      bool setAllPublicProperties = false,
      int? namedArgsIfMoreThan = null,
      int arraysOnePerLineIfLongerThan = 5)
    {
      this.m_sb.Clear();
      this.GenerateCsharpFor(obj, this.m_sb, namespaces, indentLevel, setAllPublicProperties, namedArgsIfMoreThan, arraysOnePerLineIfLongerThan);
      return this.m_sb.ToString();
    }

    public void GenerateCsharpFor(
      object obj,
      StringBuilder codeSb,
      Set<string> namespaces = null,
      int indentLevel = 0,
      bool setAllPublicProperties = false,
      int? namedArgsIfMoreThan = null,
      int arraysOnePerLineIfLongerThan = 5)
    {
      if (obj == null)
      {
        codeSb.Append("null");
      }
      else
      {
        Type type = obj.GetType();
        if (namespaces != null)
          CSharpGen.CollectNamespaces(type, namespaces);
        if (type.IsValueType)
        {
          if (type.IsEnum)
          {
            string name = Enum.GetName(type, obj);
            string cleanTypeName = CSharpGen.GetCleanTypeName(type);
            codeSb.Append(name == null ? string.Format("({0}){1}", (object) cleanTypeName, (object) (int) obj) : cleanTypeName + "." + name);
            return;
          }
          if (type.IsPrimitive)
          {
            if (obj is bool)
            {
              bool flag = (bool) obj;
              codeSb.Append(CSharpGen.GetBoolStr(flag));
              return;
            }
            if (obj is float)
            {
              float f = (float) obj;
              if (float.IsNaN(f))
              {
                codeSb.Append("float.NaN");
                return;
              }
              if (float.IsPositiveInfinity(f))
              {
                codeSb.Append("float.PositiveInfinity");
                return;
              }
              if (float.IsNegativeInfinity(f))
              {
                codeSb.Append("float.NegativeInfinity");
                return;
              }
              codeSb.Append(f.ToString() + "f");
              return;
            }
            codeSb.Append(obj);
            return;
          }
          if (type.IsGenericType)
          {
            if (obj is IOptionNonGeneric optionNonGeneric)
            {
              Type genericArgument = type.GetGenericArguments()[0];
              codeSb.Append("Option<" + CSharpGen.GetCleanTypeName(genericArgument) + ">");
              if (optionNonGeneric.HasValue)
              {
                codeSb.Append(".Some(");
                this.GenerateCsharpFor(optionNonGeneric.Value, codeSb, namespaces, indentLevel, namedArgsIfMoreThan: namedArgsIfMoreThan, arraysOnePerLineIfLongerThan: arraysOnePerLineIfLongerThan);
                codeSb.Append(")");
                return;
              }
              codeSb.Append(".None");
              return;
            }
            if (obj is IImmutableArray immutableArray)
            {
              Type genericArgument = type.GetGenericArguments()[0];
              Array array = immutableArray.Array;
              if (array.Length == 0)
              {
                codeSb.Append("ImmutableArray<" + CSharpGen.GetCleanTypeName(genericArgument) + ">.Empty");
                return;
              }
              bool flag = array.Length > arraysOnePerLineIfLongerThan;
              int num = flag ? indentLevel + 1 : indentLevel;
              string str = flag ? "," : ", ";
              codeSb.Append("ImmutableArray.Create<");
              codeSb.Append(CSharpGen.GetCleanTypeName(genericArgument));
              codeSb.Append(">(");
              for (int index = 0; index < array.Length; ++index)
              {
                if (index > 0)
                  codeSb.Append(str);
                if (flag)
                  appendLineWithIndent(num);
                this.GenerateCsharpFor(array.GetValue(index), codeSb, namespaces, num, namedArgsIfMoreThan: namedArgsIfMoreThan, arraysOnePerLineIfLongerThan: arraysOnePerLineIfLongerThan);
              }
              codeSb.Append(")");
              return;
            }
          }
        }
        else
        {
          if (obj is string str1)
          {
            codeSb.Append('"');
            codeSb.Append(str1.Replace("\\", "\\\\").Replace("\"", "\\\""));
            codeSb.Append('"');
            return;
          }
          if (type.IsGenericType && obj is IDictNonGeneric dictNonGeneric)
          {
            Type[] genericArguments = type.GetGenericArguments();
            Assert.That<Type[]>(genericArguments).HasLength<Type>(2);
            Type type1 = genericArguments[0];
            Type type2 = genericArguments[1];
            int prime = HashHelpers.GetPrime(dictNonGeneric.Count);
            Assert.That<bool>(dictNonGeneric.HasDefaultComparer).IsTrue();
            string str2 = CSharpGen.GetCleanTypeName(type1) + ", " + CSharpGen.GetCleanTypeName(type2);
            string str3 = dictNonGeneric.HasDefaultComparer ? "" : ", comparer: /* dict has non-default comparer, this is not supported! */";
            codeSb.Append(string.Format("new Dict<{0}>(capacity: {1}{2})", (object) str2, (object) prime, (object) str3));
            if (dictNonGeneric.Count <= 0)
              return;
            codeSb.Append(" {");
            int num = indentLevel + 1;
            bool flag = true;
            foreach (KeyValuePair<object, object> keyValuePair in dictNonGeneric)
            {
              if (flag)
                flag = false;
              else
                codeSb.Append(",");
              appendLineWithIndent(num);
              codeSb.Append(" {");
              this.GenerateCsharpFor(keyValuePair.Key, codeSb, namespaces, num, namedArgsIfMoreThan: namedArgsIfMoreThan, arraysOnePerLineIfLongerThan: arraysOnePerLineIfLongerThan);
              codeSb.Append(", ");
              this.GenerateCsharpFor(keyValuePair.Value, codeSb, namespaces, num, namedArgsIfMoreThan: namedArgsIfMoreThan, arraysOnePerLineIfLongerThan: arraysOnePerLineIfLongerThan);
              codeSb.Append("}");
            }
            appendLineWithIndent(indentLevel);
            codeSb.Append("}");
            return;
          }
        }
        foreach (Func<object, string> customObjectProvider in this.m_customObjectProviders)
        {
          string str = customObjectProvider(obj);
          if (str != null)
          {
            codeSb.Append(str);
            return;
          }
        }
        FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
        object[] objArray1 = (object[]) null;
        ParameterInfo[] parameterInfoArray = (ParameterInfo[]) null;
        ConstructorInfo[] source = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
        ConstructorInfo[] array1 = ((IEnumerable<ConstructorInfo>) source).Where<ConstructorInfo>((Func<ConstructorInfo, bool>) (x => x.GetCustomAttribute<CSharpGenCtorAttribute>() != null)).ToArray<ConstructorInfo>();
        if (array1.Length != 0)
          source = array1;
        ParameterInfo missingCtorParam = (ParameterInfo) null;
        foreach (MethodBase methodBase in source)
        {
          ParameterInfo[] parameters = methodBase.GetParameters();
          if (parameters.Length != 0)
          {
            missingCtorParam = (ParameterInfo) null;
            object[] objArray2 = parameters.MapArray<ParameterInfo, object>((Func<ParameterInfo, object>) (p => getValueOrNull(p, ref missingCtorParam)));
            if (missingCtorParam == null && (objArray1 == null || objArray1.Length < objArray2.Length))
            {
              objArray1 = objArray2;
              parameterInfoArray = parameters;
            }
          }
        }
        if (objArray1 == null && type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, (Binder) null, CallingConventions.Any, Array.Empty<Type>(), (ParameterModifier[]) null) != (ConstructorInfo) null)
        {
          objArray1 = Array.Empty<object>();
          parameterInfoArray = Array.Empty<ParameterInfo>();
        }
        ParameterInfo missingFactoryParam = (ParameterInfo) null;
        if (objArray1 != null)
        {
          codeSb.Append("new " + CSharpGen.GetCleanTypeName(type) + "(");
        }
        else
        {
          MethodInfo method = type.GetMethod("FromRaw", BindingFlags.Static | BindingFlags.Public);
          if (method != (MethodInfo) null)
          {
            ParameterInfo[] parameters = method.GetParameters();
            missingFactoryParam = (ParameterInfo) null;
            object[] objArray3 = method.GetParameters().MapArray<ParameterInfo, object>((Func<ParameterInfo, object>) (p => getValueOrNull(p, ref missingFactoryParam)));
            if (missingFactoryParam == null)
            {
              objArray1 = objArray3;
              parameterInfoArray = parameters;
            }
            codeSb.Append(CSharpGen.GetCleanTypeName(type) + ".FromRaw(");
          }
        }
        if (objArray1 == null)
        {
          Log.Error(string.Format("Failed to convert class {0} to C#. No suitable constructor or factory method found.\n", (object) type) + "Missing ctor param: " + missingCtorParam?.Name + "\nAvailable fields: " + ((IEnumerable<string>) fields.MapArray<FieldInfo, string>((Func<FieldInfo, string>) (x => x.Name))).JoinStrings(", ") + "\nAvailable properties: " + ((IEnumerable<string>) properties.MapArray<PropertyInfo, string>((Func<PropertyInfo, string>) (x => x.Name))).JoinStrings(", "));
          codeSb.Append("/* NO CTOR OR FACTORY FOUND FOR " + type.Name + ", missing param '" + (missingFactoryParam?.Name ?? missingCtorParam?.Name) + "' */");
        }
        else
        {
          Assert.That<int>(objArray1.Length).IsEqualTo(parameterInfoArray.Length);
          bool flag = namedArgsIfMoreThan.HasValue && objArray1.Length > namedArgsIfMoreThan.Value;
          int num = flag ? indentLevel + 1 : indentLevel;
          for (int index = 0; index < objArray1.Length; ++index)
          {
            if (flag)
            {
              if (index > 0)
                codeSb.Append(",");
              appendLineWithIndent(num);
              codeSb.Append(parameterInfoArray[index].Name);
              codeSb.Append(": ");
            }
            else if (index > 0)
              codeSb.Append(", ");
            if (objArray1[index] is CSharpGen.CustomParam customParam)
              codeSb.Append(customParam.CSharp);
            else
              this.GenerateCsharpFor(objArray1[index], codeSb, namespaces, num, namedArgsIfMoreThan: namedArgsIfMoreThan, arraysOnePerLineIfLongerThan: arraysOnePerLineIfLongerThan);
          }
          codeSb.Append(")");
          if (!setAllPublicProperties)
            return;
          codeSb.Append(" {");
          foreach (PropertyInfo property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
          {
            if (property.CanRead && property.CanWrite)
            {
              appendLineWithIndent(indentLevel + 1);
              codeSb.Append(property.Name + " = ");
              this.GenerateCsharpFor(property.GetValue(obj), codeSb, namespaces, indentLevel + 1, namedArgsIfMoreThan: namedArgsIfMoreThan, arraysOnePerLineIfLongerThan: arraysOnePerLineIfLongerThan);
              codeSb.Append(",");
            }
          }
          codeSb.Append("}");
        }

        object getValueOrNull(ParameterInfo parameter, ref ParameterInfo missingParam)
        {
          if (missingParam != null)
            return (object) null;
          ParameterData parameterData = new ParameterData(type, parameter);
          foreach (Func<ParameterData, string> customParamsProvider in this.m_customParamsProviders)
          {
            string cSharp = customParamsProvider(parameterData);
            if (cSharp != null)
              return (object) new CSharpGen.CustomParam(cSharp);
          }
          FieldInfo fieldInfo = ((IEnumerable<FieldInfo>) fields).FirstOrDefault<FieldInfo>((Func<FieldInfo, bool>) (x => x.Name.Equals(parameter.Name, StringComparison.OrdinalIgnoreCase) || x.Name.StartsWith("m_") && x.Name.Substring(2).Equals(parameter.Name, StringComparison.OrdinalIgnoreCase)));
          if (fieldInfo != (FieldInfo) null)
            return fieldInfo.GetValue(obj);
          PropertyInfo propertyInfo = ((IEnumerable<PropertyInfo>) properties).FirstOrDefault<PropertyInfo>((Func<PropertyInfo, bool>) (x => x.Name.Equals(parameter.Name, StringComparison.OrdinalIgnoreCase)));
          if (propertyInfo != (PropertyInfo) null)
            return propertyInfo.GetValue(obj);
          missingParam = parameter;
          return (object) null;
        }
      }

      void appendLineWithIndent(int i)
      {
        codeSb.AppendLine();
        if (i <= 0)
          return;
        codeSb.Append(new string('\t', i));
      }
    }

    public static string GetCleanTypeName(
      Type type,
      bool ignoreDeclaringTypes = false,
      bool includeNamespace = false)
    {
      string cleanTypeName1;
      if (CSharpGen.PRIM_TYPE_TO_NAME.TryGetValue(type, out cleanTypeName1))
        return cleanTypeName1;
      if (type.IsGenericParameter)
        return type.Name;
      if (type.IsArray)
        return CSharpGen.GetCleanTypeName(type.GetElementType(), ignoreDeclaringTypes, includeNamespace) + "[]";
      string cleanTypeName2;
      if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Nullable<>))
      {
        cleanTypeName2 = ((IEnumerable<Type>) type.GetGenericArguments()).Select<Type, string>((Func<Type, string>) (x => CSharpGen.GetCleanTypeName(x, ignoreDeclaringTypes))).JoinStrings(", ") + "?";
      }
      else
      {
        cleanTypeName2 = CSharpGen.CleanBackQuote(type.Name);
        if (type.IsGenericType || type.IsGenericTypeDefinition)
        {
          int count = 0;
          if (type.DeclaringType != (Type) null && type.DeclaringType.IsGenericType)
            count = type.DeclaringType.GetGenericArguments().Length;
          string str = ((IEnumerable<Type>) type.GetGenericArguments()).Skip<Type>(count).Select<Type, string>((Func<Type, string>) (x => CSharpGen.GetCleanTypeName(x, ignoreDeclaringTypes))).JoinStrings(", ");
          if (str.Length > 0)
            cleanTypeName2 = cleanTypeName2 + "<" + str + ">";
        }
        if (!ignoreDeclaringTypes)
        {
          for (Type declaringType = type.DeclaringType; declaringType != (Type) null; declaringType = declaringType.DeclaringType)
            cleanTypeName2 = CSharpGen.GetCleanTypeName(declaringType, true) + "." + cleanTypeName2;
        }
      }
      if (includeNamespace && !string.IsNullOrEmpty(type.Namespace))
        cleanTypeName2 = type.Namespace + "." + cleanTypeName2;
      return cleanTypeName2;
    }

    public static string GetBoolStr(bool value) => !value ? "false" : "true";

    public static string CleanBackQuote(string name)
    {
      int length = name.IndexOf('`');
      return length >= 0 ? name.Substring(0, length) : name;
    }

    public static void CollectNamespaces(Type type, Set<string> namespaces)
    {
      Assert.That<Type>(type).IsNotNull<Type>();
      Assert.That<Set<string>>(namespaces).IsNotNull<Set<string>>();
      if (type.IsArray)
      {
        CSharpGen.CollectNamespaces(type.GetElementType(), namespaces);
      }
      else
      {
        namespaces.Add(type.Namespace);
        foreach (Type genericTypeArgument in type.GenericTypeArguments)
          CSharpGen.CollectNamespaces(genericTypeArgument, namespaces);
      }
    }

    public string GenerateUsings(IEnumerable<string> namespaces)
    {
      this.m_sb.Clear();
      bool flag = true;
      foreach (string str in (IEnumerable<string>) namespaces.OrderBy<string, string>((Func<string, string>) (x => x), (IComparer<string>) StringComparer.Ordinal))
      {
        if (flag)
          flag = false;
        else
          this.m_sb.Append("\r\n");
        this.m_sb.Append("using ");
        this.m_sb.Append(str);
        this.m_sb.Append(";");
      }
      return this.m_sb.ToString();
    }

    static CSharpGen()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      CSharpGen.PRIM_TYPE_TO_NAME = new Dict<Type, string>()
      {
        {
          typeof (bool),
          "bool"
        },
        {
          typeof (byte),
          "byte"
        },
        {
          typeof (sbyte),
          "sbyte"
        },
        {
          typeof (char),
          "char"
        },
        {
          typeof (short),
          "short"
        },
        {
          typeof (ushort),
          "ushort"
        },
        {
          typeof (int),
          "int"
        },
        {
          typeof (uint),
          "uint"
        },
        {
          typeof (long),
          "long"
        },
        {
          typeof (ulong),
          "ulong"
        },
        {
          typeof (float),
          "float"
        },
        {
          typeof (double),
          "double"
        },
        {
          typeof (string),
          "string"
        },
        {
          typeof (object),
          "object"
        }
      };
    }

    private readonly struct CustomParam
    {
      public readonly string CSharp;

      public CustomParam(string cSharp)
      {
        MBiHIp97M4MqqbtZOh.RFLpSOptx();
        this.CSharp = cSharp;
      }
    }
  }
}
