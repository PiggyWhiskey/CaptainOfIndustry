// Decompiled with JetBrains decompiler
// Type: Mafi.Serialization.Generators.SerializerGenerator
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

#nullable disable
namespace Mafi.Serialization.Generators
{
  public static class SerializerGenerator
  {
    public const string GENERATED_FILE_EXT = ".generated.cs";
    private static readonly char[] LINE_SEPARATORS;
    public const string SERIALIZE_METHOD_NAME = "Serialize";
    public const string DESERIALIZE_METHOD_NAME = "Deserialize";

    public static GeneratorContext ScanAllTypesInAssemblies(
      StringBuilder log,
      Assembly[] assemblies,
      bool ignoreAllChecks = false,
      Assembly[] ignoreInterfaceWithoutImpl = null,
      Assembly[] ignoreNonSerializedClassWithSerializedInterface = null)
    {
      if (ignoreInterfaceWithoutImpl == null)
        ignoreInterfaceWithoutImpl = Array.Empty<Assembly>();
      if (ignoreNonSerializedClassWithSerializedInterface == null)
        ignoreNonSerializedClassWithSerializedInterface = Array.Empty<Assembly>();
      Set<Type> allTypes = new Set<Type>(1024);
      Set<Type> set = new Set<Type>(256);
      Dict<Type, int> dict = new Dict<Type, int>();
      Stopwatch stopwatch = Stopwatch.StartNew();
      foreach (Assembly assembly in assemblies)
      {
        Type[] types;
        try
        {
          types = assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException ex)
        {
          throw new Exception("Type load fail from assembly '" + assembly.FullName + "', scroll down for details:\nexceptions:\n" + string.Join("\n", ((IEnumerable<Exception>) ex.LoaderExceptions).Select<Exception, string>((Func<Exception, string>) (x => x.Message))), (Exception) ex);
        }
        allTypes.AddRange((IEnumerable<Type>) types);
      }
      log.AppendLine(string.Format("Scanning all assemblies: {0} ms", (object) stopwatch.Elapsed.TotalMilliseconds.RoundToInt()));
      stopwatch.Restart();
      Set<Type> nonSerializedGlobalDeps = new Set<Type>();
      Set<Type> specialSerializedTypes = new Set<Type>();
      foreach (Type type in allTypes)
      {
        if (type.IsClass && !type.IsAbstract)
          SerializerGenerator.tryRegisterAsNonSerializedGlobalDep(type, nonSerializedGlobalDeps);
        if (type.GetCustomAttribute<DisableDirectCallSerializationAttribute>() != null)
          specialSerializedTypes.Add(type);
      }
      log.AppendLine(string.Format("Find global deps and special types: {0} ms", (object) stopwatch.Elapsed.TotalMilliseconds.RoundToInt()));
      stopwatch.Restart();
      Dict<Type, TypeSerializationSpec> typeSpecs = new Dict<Type, TypeSerializationSpec>();
      foreach (Type type in allTypes)
      {
        if (type.GetCustomAttribute<ManuallyWrittenSerializationAttribute>(true) != null)
        {
          if (type.GetCustomAttribute<IgnoreMissingSerializer>(true) != null)
            throw new Exception("Type '" + type.Name + "' marked with [ManuallyWrittenSerialization] is (or one of its base classes are) also marked with [IgnoreMissingSerializer].");
        }
        else
        {
          GenerateSerializer customAttribute1 = type.GetCustomAttribute<GenerateSerializer>(true);
          if (customAttribute1 == null)
          {
            set.Add(type);
          }
          else
          {
            if (customAttribute1.NewInVersion.HasValue)
            {
              GlobalDependencyAttribute customAttribute2 = type.GetCustomAttribute<GlobalDependencyAttribute>();
              if (customAttribute2 == null)
                throw new Exception(string.Format("Class '{0}' that is not global dependency was marked new in save version.", (object) type));
              if ((customAttribute2.RegistrationMode & RegistrationMode.AsSelf) != (RegistrationMode) 0)
                dict.Add(type, customAttribute1.NewInVersion.Value);
              if ((customAttribute2.RegistrationMode & RegistrationMode.AsAllInterfaces) != (RegistrationMode) 0)
              {
                foreach (Type key in type.GetInterfaces())
                  dict.Add(key, customAttribute1.NewInVersion.Value);
              }
            }
            if (type.GetCustomAttribute<IgnoreMissingSerializer>(true) != null)
              throw new Exception("Type '" + type.Name + "' marked with [GenerateSerializer] is (or one of its base classes are) also marked with [IgnoreMissingSerializer].");
            SerializerGenerator.GenSpecContext context = new SerializerGenerator.GenSpecContext((IReadOnlySet<Type>) nonSerializedGlobalDeps, (IReadOnlySet<Type>) specialSerializedTypes, (Option<GenerateSerializer>) customAttribute1);
            TypeSerializationSpec serializationSpec = SerializerGenerator.generateSerializationSpec(type, context);
            typeSpecs.Add(serializationSpec.Type, serializationSpec);
          }
        }
      }
      StringBuilder stringBuilder1 = log;
      TimeSpan elapsed = stopwatch.Elapsed;
      string str1 = string.Format("Create serialization specs: {0} ms", (object) elapsed.TotalMilliseconds.RoundToInt());
      stringBuilder1.AppendLine(str1);
      stopwatch.Restart();
      Lyst<TypeSerializationSpec> lyst1 = new Lyst<TypeSerializationSpec>();
      foreach (TypeSerializationSpec serializationSpec1 in typeSpecs.Values.ToArray<TypeSerializationSpec>())
      {
        if (!serializationSpec1.Type.IsValueType && !serializationSpec1.SerializeAsSingleton.HasValue)
        {
          lyst1.Clear();
          lyst1.Add(serializationSpec1);
          for (Type baseType = serializationSpec1.Type.BaseType; baseType != (Type) null && baseType != typeof (object); baseType = baseType.BaseType)
          {
            Type type = baseType.IsGenericType ? baseType.GetGenericTypeDefinition() : baseType;
            if (!typeSpecs.ContainsKey(type))
            {
              SerializerGenerator.GenSpecContext context = new SerializerGenerator.GenSpecContext((IReadOnlySet<Type>) nonSerializedGlobalDeps, (IReadOnlySet<Type>) specialSerializedTypes, Option<GenerateSerializer>.None);
              TypeSerializationSpec serializationSpec2 = SerializerGenerator.generateSerializationSpec(type, context);
              typeSpecs.Add(serializationSpec2.Type, serializationSpec2);
              set.Remove(serializationSpec2.Type);
              serializationSpec2.MarkAsSerializedDueToDerivedClass(serializationSpec1.Type);
              if (serializationSpec2.HasSomethingToSerialize)
              {
                foreach (TypeSerializationSpec serializationSpec3 in lyst1)
                  serializationSpec3.MarkHasBaseTypeWithSomethingToSerialize();
              }
              lyst1.Add(serializationSpec2);
            }
          }
        }
      }
      StringBuilder stringBuilder2 = log;
      elapsed = stopwatch.Elapsed;
      string str2 = string.Format("Process base classes: {0} ms", (object) elapsed.TotalMilliseconds.RoundToInt());
      stringBuilder2.AppendLine(str2);
      stopwatch.Restart();
      foreach (TypeSerializationSpec serializationSpec in typeSpecs.Values)
      {
        foreach (MemberWrapper member in serializationSpec.Members)
        {
          int version;
          if (dict.TryGetValue(member.Type, out version))
            member.MarkGlobalDependencyNewlySerializableInVersion(version);
        }
      }
      log.AppendLine(string.Format("Process newly serialized glob deps: {0} ms", (object) stopwatch.Elapsed.TotalMilliseconds.RoundToInt()));
      stopwatch.Restart();
      foreach (Type type in set)
      {
        MethodInfo method;
        if (SerializerGenerator.TryGetSerializeMethod(type, out method))
          throw new Exception("Type '" + type.FullName + "' has 'Serialize' method but it is NOT marked with [GenerateSerializer] or [ManuallyWrittenSerialization].");
        Lyst<Pair<MethodInfo, InitAfterLoadAttribute>> lyst2 = !SerializerGenerator.TryGetDeserializeMethod(type, out method) ? SerializerGenerator.getInitMethods(type) : throw new Exception("Type '" + type.FullName + "' has 'Deserialize' method but it is NOT marked with [GenerateSerializer] or [ManuallyWrittenSerialization].");
        if (lyst2.IsNotEmpty)
        {
          string str3 = ((IEnumerable<string>) lyst2.Select<string>((Func<Pair<MethodInfo, InitAfterLoadAttribute>, string>) (x => x.First.Name))).JoinStrings(",");
          throw new Exception("Type '" + type.FullName + "' has method(s) " + str3 + " marked with 'InitAfterLoadAttribute' but it is NOT marked with [GenerateSerializer] or [ManuallyWrittenSerialization].");
        }
      }
      if (!ignoreAllChecks)
      {
        Set<Type> checkedTypes = new Set<Type>(256);
        foreach (TypeSerializationSpec serializationSpec in typeSpecs.Values)
        {
          foreach (MemberWrapper member in serializationSpec.Members)
          {
            if (member.IsSerialized)
              checkType(member.Type, member, serializationSpec.Type);
          }
        }

        void checkType(Type typeToCheck, MemberWrapper member, Type ownerType)
        {
          if (typeToCheck.IsGenericParameter || !checkedTypes.Add(typeToCheck) || typeToCheck.IsPrimitive || typeToCheck.IsEnum || typeToCheck == typeof (Type) || typeToCheck == typeof (string) || typeToCheck == typeof (DateTime) || typeToCheck == typeof (object))
            return;
          if (typeToCheck.IsArray)
            checkType(typeToCheck.GetElementType(), member, ownerType);
          else if (typeToCheck.IsGenericType && !typeToCheck.IsGenericTypeDefinition)
          {
            Type genericTypeDefinition = typeToCheck.GetGenericTypeDefinition();
            if (genericTypeDefinition == typeof (Nullable<>))
            {
              checkType(typeToCheck.GetGenericArguments().First<Type>(), member, ownerType);
            }
            else
            {
              if (genericTypeDefinition == typeof (Option<>))
                checkType(typeToCheck.GetGenericArguments().First<Type>(), member, ownerType);
              checkType(genericTypeDefinition, member, ownerType);
            }
          }
          else
          {
            if (typeToCheck.GetCustomAttribute<IgnoreMissingSerializer>(true) != null || typeToCheck.GetCustomAttribute<DisableDirectCallSerializationAttribute>(true) != null)
              return;
            if (typeToCheck.IsInterface || typeToCheck.IsAbstract)
            {
              Type[] classesImplementing = SerializerGenerator.findAllNonAbstractClassesImplementing(typeToCheck, (IEnumerable<Type>) allTypes);
              if (classesImplementing.Length == 0 && !((IEnumerable<Assembly>) ignoreInterfaceWithoutImpl).Contains<Assembly>(typeToCheck.Assembly))
                throw new Exception((typeToCheck.IsInterface ? "Interface" : "Abstract class") + " " + string.Format("'{0}' is being saved as member '{1}' of '{2}' ", (object) typeToCheck, (object) member.Name, (object) ownerType.Name) + "but has no implementation.");
              foreach (Type typeToCheck1 in classesImplementing)
              {
                if (!((IEnumerable<Assembly>) ignoreNonSerializedClassWithSerializedInterface).Contains<Assembly>(typeToCheck1.Assembly))
                {
                  try
                  {
                    checkType(typeToCheck1, member, typeToCheck);
                  }
                  catch (Exception ex)
                  {
                    throw new Exception("There was serializer check error for '" + typeToCheck1.Name + "' on '" + typeToCheck.Name + "' that is being serialized in '" + member.Name + "' of '" + member.Type.Name + "': " + ex.Message);
                  }
                }
              }
            }
            else if (typeToCheck.GetCustomAttribute<ManuallyWrittenSerializationAttribute>() != null)
            {
              MethodInfo method1;
              if (!SerializerGenerator.TryGetSerializeMethod(typeToCheck, out method1))
                throw new Exception("Type '" + typeToCheck.Name + "' is marked with [ManuallyWrittenSerialization] but no public static 'Serialize' method was found.");
              ParameterInfo[] parameterInfoArray = !(method1.ReturnType != typeof (void)) ? method1.GetParameters() : throw new Exception("The return value of 'Serialize' method on type '" + typeToCheck.Name + "' is not 'void'.");
              if (parameterInfoArray.Length != 2)
                throw new Exception("The 'Serialize' method on type '" + typeToCheck.Name + "' does not have 2 parameters.");
              if (parameterInfoArray[0].ParameterType != typeToCheck)
                throw new Exception("The 1st parameter of 'Serialize' method on type '" + typeToCheck.Name + "' is not of type '" + typeToCheck.Name + "'.");
              if (parameterInfoArray[1].ParameterType != typeof (BlobWriter))
                throw new Exception("The 2nd parameter of 'Serialize' method on type '" + typeToCheck.Name + "' is not of type 'BlobWriter'.");
              MethodInfo method2;
              if (!SerializerGenerator.TryGetDeserializeMethod(typeToCheck, out method2))
                throw new Exception("Type '" + typeToCheck.Name + "' is marked with [ManuallyWrittenSerialization] but no public static 'Deserialize' method was found.");
              if (method2.ReturnType != typeToCheck)
                throw new Exception("The return value of 'Deserialize' method on type '" + typeToCheck.Name + "' is not '" + typeToCheck.Name + "'.");
              ParameterInfo[] parameters = method2.GetParameters();
              if (parameters.Length != 1)
                throw new Exception("The 'Deserialize' method on type '" + typeToCheck.Name + "' does not have 1 parameter.");
              if (parameters[0].ParameterType != typeof (BlobReader))
                throw new Exception("The 'Deserialize' method on type '" + typeToCheck.Name + "' does not have a single parameter of type 'BlobReader'.");
            }
            else if (!typeSpecs.ContainsKey(typeToCheck))
              throw new Exception("Member '" + member.Name + "' ('" + member.Type.Name + "' serialized as '" + typeToCheck.Name + "') of type '" + ownerType.Name + "' is serialized but type '" + typeToCheck.Name + "' is not marked for serialization.");
          }
        }
      }
      log.AppendLine(string.Format("Verify all serialized types: {0} ms", (object) stopwatch.Elapsed.TotalMilliseconds.RoundToInt()));
      stopwatch.Restart();
      return new GeneratorContext(log, (IReadOnlySet<Type>) allTypes, (IReadOnlySet<Type>) nonSerializedGlobalDeps, (IReadOnlyDictionary<Type, TypeSerializationSpec>) typeSpecs);
    }

    /// <summary>Saves all files in parallel. Called from T4 template.</summary>
    public static string WriteAllResults(IIndexable<SerializerGeneratorResult> results)
    {
      Set<string> largeDirsSet = results.AsEnumerable().GroupBy<SerializerGeneratorResult, string>((Func<SerializerGeneratorResult, string>) (x => Path.GetDirectoryName(x.FilePath))).Where<IGrouping<string, SerializerGeneratorResult>>((Func<IGrouping<string, SerializerGeneratorResult>, bool>) (x => x.Count<SerializerGeneratorResult>() > 100)).Select<IGrouping<string, SerializerGeneratorResult>, string>((Func<IGrouping<string, SerializerGeneratorResult>, string>) (x => x.Key)).ToSet<string>();
      ConcurrentBag<string> writtenFilePaths = new ConcurrentBag<string>();
      string str1 = results.AsEnumerable().AsParallel<SerializerGeneratorResult>().Select<SerializerGeneratorResult, string>((Func<SerializerGeneratorResult, string>) (result =>
      {
        string directoryName = Path.GetDirectoryName(result.FilePath);
        string path;
        if (largeDirsSet.Contains(directoryName))
        {
          string fileName = Path.GetFileName(result.FilePath);
          string str2 = Path.Combine(directoryName, char.ToUpperInvariant(fileName[0]).ToString());
          path = Path.Combine(str2, fileName);
          if (!Directory.Exists(str2))
            Directory.CreateDirectory(str2);
        }
        else
          path = result.FilePath;
        string contents = result.NewContents;
        string str3 = File.Exists(path) ? File.ReadAllText(path) : "";
        if (str3.StartsWith("#if"))
        {
          int length = str3.IndexOfAny(SerializerGenerator.LINE_SEPARATORS);
          contents = (length == -1 ? str3 : str3.Substring(0, length)) + Environment.NewLine + contents + Environment.NewLine + "#endif";
        }
        writtenFilePaths.Add(path);
        if (!(contents != str3))
          return (string) null;
        File.WriteAllText(path, contents);
        return "[INFO] New contents saved to " + path + ".";
      })).Where<string>((Func<string, bool>) (x => x != null)).JoinStrings("\n");
      Set<string> set = new Set<string>((IEnumerable<string>) writtenFilePaths);
      Lyst<string> lyst = new Lyst<string>(results.Select<SerializerGeneratorResult, string>((Func<SerializerGeneratorResult, string>) (x => Path.GetDirectoryName(x.FilePath))).Distinct<string>());
      Lyst<string> strings = new Lyst<string>();
      while (lyst.IsNotEmpty)
      {
        string path = lyst.PopLast();
        lyst.AddRange(Directory.EnumerateDirectories(path));
        foreach (string enumerateFile in Directory.EnumerateFiles(path))
        {
          if (enumerateFile.EndsWith(".generated.cs") && !set.Contains(enumerateFile))
          {
            strings.Add(enumerateFile);
            File.Delete(enumerateFile);
          }
        }
      }
      return string.Format("{0}\n\n {1} deleted files:\n {2}", (object) str1, (object) strings.Count, (object) strings.JoinStrings("\n"));
    }

    public static bool TryGetSerializeMethod(Type t, out MethodInfo method)
    {
      method = t.GetMethod("Serialize", BindingFlags.Static | BindingFlags.Public);
      return method != (MethodInfo) null;
    }

    public static bool TryGetDeserializeMethod(Type t, out MethodInfo method)
    {
      method = t.GetMethod("Deserialize", BindingFlags.Static | BindingFlags.Public);
      return method != (MethodInfo) null;
    }

    public static bool IsNonSerializedGlobalDep(Type t, out GlobalDependencyAttribute globalDepAttr)
    {
      if (!t.IsClass)
      {
        globalDepAttr = (GlobalDependencyAttribute) null;
        return false;
      }
      globalDepAttr = t.GetCustomAttribute<GlobalDependencyAttribute>(false);
      return globalDepAttr != null && !TypeExtensions.HasAttribute<GenerateSerializer>(t, false);
    }

    private static void tryRegisterAsNonSerializedGlobalDep(
      Type t,
      Set<Type> nonSerializedGlobalDeps)
    {
      GlobalDependencyAttribute globalDepAttr;
      if (!SerializerGenerator.IsNonSerializedGlobalDep(t, out globalDepAttr))
        return;
      RegistrationMode registrationMode = globalDepAttr.RegistrationMode;
      if (globalDepAttr.OnlyInDebug || globalDepAttr.OnlyInDevOnly)
        return;
      if ((registrationMode & RegistrationMode.AsSelf) != (RegistrationMode) 0)
      {
        if (t.IsGenericType)
          nonSerializedGlobalDeps.Add(t.GetGenericTypeDefinition());
        nonSerializedGlobalDeps.Add(t);
      }
      if ((registrationMode & RegistrationMode.AsAllInterfaces) == (RegistrationMode) 0)
        return;
      foreach (Type element in t.GetInterfaces())
      {
        if (element.GetCustomAttribute<NotGlobalDependencyAttribute>(false) == null)
          nonSerializedGlobalDeps.Add(element);
      }
    }

    private static Type[] findAllNonAbstractClassesImplementing(
      Type type,
      IEnumerable<Type> allTypes)
    {
      if (!type.IsGenericType)
        return allTypes.Where<Type>((Func<Type, bool>) (x => x.IsClass && !x.IsAbstract && x.IsAssignableTo(type))).ToArray<Type>();
      Type interfaceType = type.GetGenericTypeDefinition();
      return allTypes.Where<Type>((Func<Type, bool>) (x => x.IsClass && !x.IsAbstract)).Where<Type>((Func<Type, bool>) (x => ((IEnumerable<Type>) x.GetInterfaces()).Any<Type>((Func<Type, bool>) (y => y.IsGenericType && y.GetGenericTypeDefinition() == interfaceType)))).ToArray<Type>();
    }

    private static TypeSerializationSpec generateSerializationSpec(
      Type type,
      SerializerGenerator.GenSpecContext context)
    {
      return !type.IsValueType ? SerializerGenerator.generateClassSerializer(type, context) : SerializerGenerator.generateStructSerializer(type, context);
    }

    private static TypeSerializationSpec generateStructSerializer(
      Type type,
      SerializerGenerator.GenSpecContext context)
    {
      Mafi.Assert.That<bool>(type.IsValueType).IsTrue();
      if (context.GenSerializerAttr.HasValue)
      {
        if (context.GenSerializerAttr.Value.CustomClassDataSerialization)
          throw new Exception("Struct '" + type.Name + "' cannot be marked with custom data serialization. If you wish to write custom serialization use [ManuallyWrittenSerialization].");
        if (context.GenSerializerAttr.Value.SerializeAsSingleton.HasValue)
          throw new Exception("Struct '{type.Name}' cannot be marked with singleton serialization. ");
      }
      MemberWrapper[] allSaveMembersFor = SerializerGenerator.GetAllSaveMembersFor(type, context);
      MemberWrapper[] ctorArgsInOrder = SerializerGenerator.getCtorArgsInOrder(type, allSaveMembersFor);
      MemberWrapper[] array1 = ((IEnumerable<MemberWrapper>) allSaveMembersFor).Where<MemberWrapper>((Func<MemberWrapper, bool>) (x => !x.IsCtorArg)).ToArray<MemberWrapper>();
      foreach (MemberWrapper memberWrapper in array1)
        memberWrapper.MarkAssignToObj();
      MemberWrapper[] array2 = ((IEnumerable<MemberWrapper>) ctorArgsInOrder).Concat<MemberWrapper>((IEnumerable<MemberWrapper>) array1).ToArray<MemberWrapper>();
      string cleanTypeName = CSharpGen.GetCleanTypeName(type, true);
      string fileName = SerializerGenerator.getFileName(type);
      string[] listOfUsings = SerializerGenerator.generateListOfUsings(type.Namespace, array2);
      string[] initCalls = SerializerGenerator.generateInitCalls(type);
      if (initCalls.IsNotEmpty<string>())
        throw new Exception("Implement init calls on structs when we need it.");
      return new TypeSerializationSpec(type, cleanTypeName, fileName, type.Namespace, listOfUsings, array2, initCalls, ctorArgsInOrder, array1);
    }

    private static TypeSerializationSpec generateClassSerializer(
      Type type,
      SerializerGenerator.GenSpecContext context)
    {
      Mafi.Assert.That<bool>(type.IsValueType).IsFalse();
      SerializerGenerator.assertHasNoLoadCtorAnnotation(type);
      bool hasCustomDataSerialization = context.GenSerializerAttr.HasValue && context.GenSerializerAttr.Value.CustomClassDataSerialization;
      MemberWrapper[] members = hasCustomDataSerialization || context.GenSerializerAttr.HasValue && context.GenSerializerAttr.Value.SerializeAsSingleton.HasValue ? Array.Empty<MemberWrapper>() : SerializerGenerator.GetAllSaveMembersFor(type, context);
      string cleanTypeName = CSharpGen.GetCleanTypeName(type, true);
      string fileName = SerializerGenerator.getFileName(type);
      string[] listOfUsings = SerializerGenerator.generateListOfUsings(type.Namespace, members);
      string[] initCalls = SerializerGenerator.generateInitCalls(type);
      Option<string> serializeAsSingleton = context.GenSerializerAttr.HasValue ? context.GenSerializerAttr.Value.SerializeAsSingleton : Option<string>.None;
      if (serializeAsSingleton.HasValue && initCalls.IsNotEmpty<string>())
        throw new Exception("Init self on singleton '" + type.Name + "' is not supported.");
      if (hasCustomDataSerialization && initCalls.IsNotEmpty<string>())
        throw new Exception("Init self on '" + type.Name + "' with custom data serialization is not supported, put your init code to the custom deserialize method.");
      return new TypeSerializationSpec(type, cleanTypeName, fileName, type.Namespace, listOfUsings, members, initCalls, serializeAsSingleton: serializeAsSingleton, hasCustomDataSerialization: hasCustomDataSerialization);
    }

    private static string getFileName(Type type)
    {
      string fileName = type.Name;
      for (Type declaringType = type.DeclaringType; declaringType != (Type) null; declaringType = declaringType.DeclaringType)
        fileName = declaringType.Name + "." + fileName;
      return fileName;
    }

    private static string[] generateListOfUsings(string currentNamespace, MemberWrapper[] members)
    {
      Set<string> set = new Set<string>()
      {
        "System",
        "Mafi.Serialization"
      };
      foreach (MemberWrapper member in members)
        CSharpGen.CollectNamespaces(member.Type, set);
      set.Remove("Mafi");
      set.Remove(currentNamespace);
      return set.OrderBy<string, string>((Func<string, string>) (x => x)).ToArray<string>();
    }

    private static void assertHasNoLoadCtorAnnotation(Type type)
    {
      if (((IEnumerable<ConstructorInfo>) type.GetConstructors()).Any<ConstructorInfo>((Func<ConstructorInfo, bool>) (x => x.HasAttribute<LoadCtorAttribute>(true))))
        throw new InvalidDataException("[LoadCtor] annotation is not allowed! Found it on class " + type.Name + "!");
    }

    private static Lyst<Pair<MethodInfo, InitAfterLoadAttribute>> getInitMethods(Type type)
    {
      Lyst<Pair<MethodInfo, InitAfterLoadAttribute>> initMethods = new Lyst<Pair<MethodInfo, InitAfterLoadAttribute>>();
      foreach (MethodInfo method in type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
      {
        InitAfterLoadAttribute customAttribute = method.GetCustomAttribute<InitAfterLoadAttribute>(false);
        if (customAttribute != null)
          initMethods.Add(Pair.Create<MethodInfo, InitAfterLoadAttribute>(method, customAttribute));
      }
      return initMethods;
    }

    private static string[] generateInitCalls(Type type)
    {
      return SerializerGenerator.getInitMethods(type).ToArray<string>((Func<Pair<MethodInfo, InitAfterLoadAttribute>, string>) (pair =>
      {
        MethodInfo first = pair.First;
        string str1 = "";
        ParameterInfo[] parameters = first.GetParameters();
        string initCalls;
        if (pair.Second.Priority == InitPriority.ImmediatelyAfterSelfDeserialized)
        {
          string str2 = "";
          foreach (ParameterInfo parameterInfo in parameters)
          {
            Type parameterType = parameterInfo.ParameterType;
            if (str2.Length > 0)
              str2 += ",";
            if (parameterType == typeof (int))
            {
              str2 += "reader.LoadedSaveVersion";
            }
            else
            {
              if (parameterType == typeof (DependencyResolver))
                throw new Exception("Init method '" + first.Name + "' on '" + type.Name + "' has invalid parameter type '" + parameterType.Name + "', resolver cannot be requested in 'ImmediatelyAfterSelfDeserialized' call.");
              throw new Exception("Init method '" + first.Name + "' on '" + type.Name + "' has invalid parameter type '" + parameterType.Name + "'.");
            }
          }
          initCalls = str1 + first.Name + "(" + str2 + ");";
        }
        else
        {
          foreach (ParameterInfo parameterInfo in parameters)
          {
            if (parameterInfo.ParameterType != typeof (int) && parameterInfo.ParameterType != typeof (DependencyResolver))
              throw new Exception("Init method '" + first.Name + "' on '" + type.Name + "' has invalid parameter type '" + parameterInfo.ParameterType.Name + "'.");
          }
          initCalls = str1 + "reader.RegisterInitAfterLoad(this, nameof(" + first.Name + "), " + string.Format("InitPriority.{0});", (object) pair.Second.Priority);
        }
        return initCalls;
      }));
    }

    public static MemberWrapper[] GetAllSaveMembersFor(
      Type type,
      SerializerGenerator.GenSpecContext context)
    {
      IEnumerable<PropertyInfo> propertyInfos = ((IEnumerable<PropertyInfo>) type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)).Where<PropertyInfo>((Func<PropertyInfo, bool>) (x => !x.HasAttribute<DoNotSaveAttribute>() && x.GetSetMethod(true) == (MethodInfo) null));
      List<FieldInfo> list = ((IEnumerable<FieldInfo>) type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)).Where<FieldInfo>((Func<FieldInfo, bool>) (x => x.Name.EndsWith("BackingField"))).ToList<FieldInfo>();
      foreach (PropertyInfo propertyInfo in propertyInfos)
      {
        PropertyInfo property = propertyInfo;
        if (list.FirstOrDefault<FieldInfo>((Func<FieldInfo, bool>) (x => x.Name == "<" + property.Name + ">k__BackingField")) != (FieldInfo) null)
          throw new ArgumentException("Property '" + property.Name + "' on type '" + type.Name + "' has no setter but has a backing field. Can't be saved!");
      }
      IEnumerable<MemberWrapper> second1 = ((IEnumerable<PropertyInfo>) type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)).Where<PropertyInfo>((Func<PropertyInfo, bool>) (x =>
      {
        if (x.IsSpecialName || x.GetIndexParameters().Length != 0)
          return false;
        DoNotSaveAttribute customAttribute = x.GetCustomAttribute<DoNotSaveAttribute>();
        if (customAttribute == null)
          return x.GetSetMethod(true) != (MethodInfo) null;
        return customAttribute.RemovedInSaveVersion.HasValue || customAttribute.ResolveAfterLoad.HasValue;
      })).Select<PropertyInfo, MemberWrapper>((Func<PropertyInfo, MemberWrapper>) (x =>
      {
        NewInSaveVersionAttribute customAttribute1 = x.GetCustomAttribute<NewInSaveVersionAttribute>();
        DoNotSaveAttribute customAttribute2 = x.GetCustomAttribute<DoNotSaveAttribute>();
        DoNotSaveCreateNewOnLoadAttribute customAttribute3 = x.GetCustomAttribute<DoNotSaveCreateNewOnLoadAttribute>();
        RenamedInVersionAttribute customAttribute4 = x.GetCustomAttribute<RenamedInVersionAttribute>();
        if (customAttribute1 != null && customAttribute1.CustomSortingName.ValueOrNull != null && customAttribute4 != null)
          throw new Exception(string.Format("Two conflicting names for member {0}: {1} vs {2}", (object) x, (object) customAttribute1?.CustomSortingName.Value, (object) customAttribute4.OldName));
        if (customAttribute2 != null && customAttribute3 != null)
          throw new Exception("Member '" + x.Name + "' marked with conflicting attributes 'DoNotSaveAttribute' and 'DoNotSaveCreateNewOnLoadAttribute'");
        int? nullable1 = (int?) customAttribute3?.RemovedInSaveVersion;
        int? removedInSaveVersion1 = (int?) customAttribute2?.RemovedInSaveVersion;
        if (customAttribute2 != null && customAttribute2.RemovedInSaveVersion.HasValue && x.GetSetMethod(true) == (MethodInfo) null)
          nullable1 = new int?(customAttribute2.RemovedInSaveVersion.Value);
        bool flag1 = x.GetCustomAttribute<SerializeUsingNonVariableEncodingAttribute>() != null;
        bool flag2 = x.GetCustomAttribute<SerializeNullAsEmptyArrayAttribute>() != null;
        Type owner = type;
        string name = x.Name;
        Type propertyType = x.PropertyType;
        int num1 = !x.CanWrite ? 1 : 0;
        Option<string> createNewInstanceCode = (Option<string>) customAttribute3?.CreateNewInstanceCode;
        int? version = customAttribute1?.Version;
        int? nullable2 = nullable1;
        int? deprecatedInSaveVersion = removedInSaveVersion1;
        int? removedInSaveVersion2 = nullable2;
        string nameForSorting = customAttribute4?.OldName ?? customAttribute1?.CustomSortingName.ValueOrNull ?? x.Name;
        Option<string> customValueWhenNotLoaded = customAttribute1 != null ? customAttribute1.CustomValueWhenNotLoaded : Option<string>.None;
        Option<Type> defaultValueFromResolver = customAttribute1 != null ? customAttribute1.DefaultValueFromResolver : Option<Type>.None;
        Option<Type> globalDepTypeOverride = customAttribute1 != null ? customAttribute1.GlobalDepTypeOverride : (customAttribute2 != null ? customAttribute2.ResolveAfterLoad : Option<Type>.None);
        int num2 = flag1 ? 1 : 0;
        int num3 = customAttribute2 != null ? (customAttribute2.ResolveAfterLoad.HasValue ? 1 : 0) : 0;
        int num4 = flag2 ? 1 : 0;
        return new MemberWrapper(owner, false, name, propertyType, num1 != 0, createNewInstanceCode, version, deprecatedInSaveVersion, removedInSaveVersion2, nameForSorting, customValueWhenNotLoaded, defaultValueFromResolver, globalDepTypeOverride, num2 != 0, num3 != 0, num4 != 0);
      }));
      Lyst<string> eventNames = ((IEnumerable<EventInfo>) type.GetEvents(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)).Select<EventInfo, string>((Func<EventInfo, string>) (eventInfo => eventInfo.Name)).ToLyst<string>();
      IEnumerable<MemberWrapper> first = ((IEnumerable<FieldInfo>) type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)).Where<FieldInfo>((Func<FieldInfo, bool>) (x =>
      {
        if (x.IsSpecialName || x.Name.EndsWith("_BackingField") || eventNames.Contains(x.Name))
          return false;
        DoNotSaveAttribute customAttribute = x.GetCustomAttribute<DoNotSaveAttribute>();
        return customAttribute == null || customAttribute.RemovedInSaveVersion.HasValue || customAttribute.ResolveAfterLoad.HasValue;
      })).Select<FieldInfo, MemberWrapper>((Func<FieldInfo, MemberWrapper>) (x =>
      {
        NewInSaveVersionAttribute customAttribute5 = x.GetCustomAttribute<NewInSaveVersionAttribute>();
        DoNotSaveAttribute customAttribute6 = x.GetCustomAttribute<DoNotSaveAttribute>();
        DoNotSaveCreateNewOnLoadAttribute customAttribute7 = x.GetCustomAttribute<DoNotSaveCreateNewOnLoadAttribute>();
        RenamedInVersionAttribute customAttribute8 = x.GetCustomAttribute<RenamedInVersionAttribute>();
        if (customAttribute5 != null && customAttribute5.CustomSortingName.ValueOrNull != null && customAttribute8 != null)
          throw new Exception(string.Format("Two conflicting names for member {0}: {1} vs {2}", (object) x, (object) customAttribute5?.CustomSortingName.Value, (object) customAttribute8.OldName));
        if (customAttribute6 != null && customAttribute7 != null)
          throw new Exception("Member '" + x.Name + "' marked with conflicting attributes 'DoNotSaveAttribute' and 'DoNotSaveCreateNewOnLoadAttribute'");
        bool flag3 = x.GetCustomAttribute<SerializeUsingNonVariableEncodingAttribute>() != null;
        bool flag4 = x.GetCustomAttribute<SerializeNullAsEmptyArrayAttribute>() != null;
        Type owner = type;
        string name = x.Name;
        Type fieldType = x.FieldType;
        int num5 = x.IsInitOnly ? 1 : 0;
        Option<string> createNewInstanceCode = (Option<string>) customAttribute7?.CreateNewInstanceCode;
        int? version = customAttribute5?.Version;
        int? removedInSaveVersion3 = (int?) customAttribute7?.RemovedInSaveVersion;
        int? removedInSaveVersion4 = customAttribute6?.RemovedInSaveVersion;
        int? removedInSaveVersion5 = removedInSaveVersion3;
        string nameForSorting = customAttribute8?.OldName ?? customAttribute5?.CustomSortingName.ValueOrNull ?? x.Name;
        Option<string> customValueWhenNotLoaded = customAttribute5 != null ? customAttribute5.CustomValueWhenNotLoaded : Option<string>.None;
        Option<Type> defaultValueFromResolver = customAttribute5 != null ? customAttribute5.DefaultValueFromResolver : Option<Type>.None;
        Option<Type> globalDepTypeOverride = customAttribute5 != null ? customAttribute5.GlobalDepTypeOverride : (customAttribute6 != null ? customAttribute6.ResolveAfterLoad : Option<Type>.None);
        int num6 = flag3 ? 1 : 0;
        int num7 = customAttribute6 != null ? (customAttribute6.ResolveAfterLoad.HasValue ? 1 : 0) : 0;
        int num8 = flag4 ? 1 : 0;
        return new MemberWrapper(owner, true, name, fieldType, num5 != 0, createNewInstanceCode, version, removedInSaveVersion4, removedInSaveVersion5, nameForSorting, customValueWhenNotLoaded, defaultValueFromResolver, globalDepTypeOverride, num6 != 0, num7 != 0, num8 != 0);
      }));
      IEnumerable<MemberWrapper> second2 = type.GetCustomAttributes<MemberRemovedInSaveVersionAttribute>(false).Select<MemberRemovedInSaveVersionAttribute, MemberWrapper>((Func<MemberRemovedInSaveVersionAttribute, MemberWrapper>) (x =>
      {
        Type owner = type;
        string name1 = x.Name;
        Type type1 = x.Type;
        Option<string> none1 = Option<string>.None;
        int? wasNewInVersion = x.WasNewInVersion;
        int? nullable = new int?(x.RemovedInVersion);
        int? deprecatedInSaveVersion = new int?();
        int? removedInSaveVersion = nullable;
        string name2 = x.Name;
        Option<Type> none2 = Option<Type>.None;
        Option<string> none3 = Option<string>.None;
        Option<Type> defaultValueFromResolver = none2;
        Option<Type> none4 = Option<Type>.None;
        int num = x.WasSerializedUsingNonVariableEncoding ? 1 : 0;
        return new MemberWrapper(owner, true, name1, type1, false, none1, wasNewInVersion, deprecatedInSaveVersion, removedInSaveVersion, name2, none3, defaultValueFromResolver, none4, num != 0, false, false);
      }));
      MemberWrapper[] array = first.Concat<MemberWrapper>(second1).Concat<MemberWrapper>(second2).OrderBy<MemberWrapper, string>((Func<MemberWrapper, string>) (x => x.NameForSorting)).ToArray<MemberWrapper>();
      foreach (MemberWrapper memberWrapper in array)
      {
        if (context.SpecialSerializedTypes.Contains(memberWrapper.Type))
          memberWrapper.DisableDirectCallSerialization();
        if (!SerializerGenerator.isIProductBuffer(memberWrapper.Type))
        {
          if (context.NonSerializedGlobalDeps.Contains(memberWrapper.Type))
            memberWrapper.MarkLoadedAsGlobalDep();
          else if (memberWrapper.Type.IsGenericType && context.NonSerializedGlobalDeps.Contains(memberWrapper.Type.GetGenericTypeDefinition()))
            memberWrapper.MarkLoadedAsGlobalDep();
          else if (memberWrapper.Type.GetCustomAttribute<SerializeAsGlobalDepAttribute>() != null)
            memberWrapper.MarkLoadedAsGlobalDep();
        }
      }
      return array;
    }

    private static bool isIProductBuffer(Type givenType)
    {
      for (; givenType != (Type) null; givenType = givenType.BaseType)
      {
        if (givenType.FullName == "Mafi.Core.Entities.Static.IProductBuffer")
          return true;
      }
      return false;
    }

    private static MemberWrapper[] getCtorArgsInOrder(Type type, MemberWrapper[] members)
    {
      Mafi.Assert.That<bool>(type.IsValueType).IsTrue("Load ctor can only be used for structs!");
      ConstructorInfo[] constructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
      ConstructorInfo[] array1 = ((IEnumerable<ConstructorInfo>) constructors).Where<ConstructorInfo>((Func<ConstructorInfo, bool>) (x => x.GetCustomAttributes(typeof (LoadCtorAttribute), true).Length != 0)).ToArray<ConstructorInfo>();
      if (array1.Length == 2)
        throw new InvalidDataException("Struct " + type.Name + " has multiple constructors marked with [LoadCtor]!");
      ConstructorInfo constructorInfo = (ConstructorInfo) null;
      if (array1.Length == 0)
      {
        if (constructors.Length > 1)
          throw new InvalidDataException("Struct " + type.Name + " has no constructor marked with [LoadCtor]!");
        if (constructors.Length == 0)
          return Array.Empty<MemberWrapper>();
        if (constructors.Length == 1)
          constructorInfo = constructors[0];
      }
      else
        constructorInfo = array1[0];
      ParameterInfo[] parameters = constructorInfo.GetParameters();
      if (parameters.Length == 0)
        return Array.Empty<MemberWrapper>();
      MemberWrapper[] ctorArgsInOrder = new MemberWrapper[parameters.Length];
      for (int index = 0; index < parameters.Length; ++index)
      {
        string paramName = parameters[index].Name;
        MemberWrapper[] array2 = ((IEnumerable<MemberWrapper>) members).Where<MemberWrapper>((Func<MemberWrapper, bool>) (x =>
        {
          if (x.Name.Equals(paramName, StringComparison.OrdinalIgnoreCase))
            return true;
          return x.Name.StartsWith("m_") && x.Name.Substring(2).Equals(paramName, StringComparison.OrdinalIgnoreCase);
        })).ToArray<MemberWrapper>();
        if (array2.Length != 1)
        {
          string str = array2.IsEmpty<MemberWrapper>() ? "none" : ((IEnumerable<MemberWrapper>) array2).Select<MemberWrapper, string>((Func<MemberWrapper, string>) (x => x.Type?.ToString() + " " + x.Name)).JoinStrings(", ");
          throw new Exception("Cannot find field to put into ctor of `" + type.Name + "` for parameter `" + paramName + "`. Options are: " + str);
        }
        MemberWrapper memberWrapper = array2.First<MemberWrapper>();
        ctorArgsInOrder[index] = !memberWrapper.IsCtorArg ? memberWrapper : throw new Exception("Failed to gather ctor args for '" + type.Name + "', member '" + memberWrapper.Name + "' matched more than once.");
        memberWrapper.MarkIsCtorArg();
      }
      return ctorArgsInOrder;
    }

    static SerializerGenerator()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      SerializerGenerator.LINE_SEPARATORS = new char[2]
      {
        '\r',
        '\n'
      };
    }

    public readonly struct GenSpecContext
    {
      public readonly IReadOnlySet<Type> NonSerializedGlobalDeps;
      public readonly IReadOnlySet<Type> SpecialSerializedTypes;
      public readonly Option<GenerateSerializer> GenSerializerAttr;

      public GenSpecContext(
        IReadOnlySet<Type> nonSerializedGlobalDeps,
        IReadOnlySet<Type> specialSerializedTypes,
        Option<GenerateSerializer> genSerializerAttr)
      {
        MBiHIp97M4MqqbtZOh.RFLpSOptx();
        this.NonSerializedGlobalDeps = nonSerializedGlobalDeps;
        this.SpecialSerializedTypes = specialSerializedTypes;
        this.GenSerializerAttr = genSerializerAttr;
      }
    }
  }
}
