// Decompiled with JetBrains decompiler
// Type: Mafi.Core.SaveGame.CustomSerializer.CustomTextReader
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Game;
using Mafi.Core.Prototypes;
using Mafi.Localization;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

#nullable disable
namespace Mafi.Core.SaveGame.CustomSerializer
{
  public class CustomTextReader : IDisposable
  {
    public const char NEW_LINE = '\n';
    public const char CR = '\r';
    public const char DELIMITER = ' ';
    private readonly StreamReader m_reader;
    private readonly StringBuilder m_sb;
    private Option<ProtosDb> m_protosDb;
    private readonly Dict<Proto.ID, Proto.ID> m_protoIdsMap;
    private readonly Set<Proto.ID> m_missingProtoIds;
    private readonly Dict<Type, Func<object>> m_readGenericFns;

    public int Level { get; private set; }

    public int SaveVersion { get; private set; }

    public ISet<Proto.ID> MissingProtoIds => (ISet<Proto.ID>) this.m_missingProtoIds;

    public CustomTextReader(Stream stream)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_sb = new StringBuilder();
      this.m_protoIdsMap = new Dict<Proto.ID, Proto.ID>();
      this.m_missingProtoIds = new Set<Proto.ID>();
      this.m_readGenericFns = new Dict<Type, Func<object>>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_reader = new StreamReader(stream, Encoding.UTF8, false, 1024, true);
      this.m_readGenericFns.Add(typeof (bool), (Func<object>) (() => (object) this.ReadBool()));
      this.m_readGenericFns.Add(typeof (sbyte), (Func<object>) (() => (object) (sbyte) this.ReadInt()));
      this.m_readGenericFns.Add(typeof (byte), (Func<object>) (() => (object) (byte) this.ReadInt()));
      this.m_readGenericFns.Add(typeof (short), (Func<object>) (() => (object) (short) this.ReadInt()));
      this.m_readGenericFns.Add(typeof (ushort), (Func<object>) (() => (object) (ushort) this.ReadInt()));
      this.m_readGenericFns.Add(typeof (char), (Func<object>) (() => (object) (char) this.ReadInt()));
      this.m_readGenericFns.Add(typeof (int), (Func<object>) (() => (object) this.ReadInt()));
      this.m_readGenericFns.Add(typeof (uint), (Func<object>) (() => (object) this.ReadUint()));
      this.m_readGenericFns.Add(typeof (long), (Func<object>) (() => (object) this.ReadLong()));
      this.m_readGenericFns.Add(typeof (ulong), (Func<object>) (() => (object) this.ReadULong()));
      this.m_readGenericFns.Add(typeof (string), (Func<object>) (() => (object) this.ReadString()));
      this.m_readGenericFns.Add(typeof (LocStr), (Func<object>) (() => (object) LocalizationManager.LoadOrCreateLocalizedString0(this.ReadString(), this.ReadString())));
    }

    /// <summary>This enables protos reading.</summary>
    public void SetProtosDb(ProtosDb protosDb)
    {
      Assert.That<Option<ProtosDb>>(this.m_protosDb).IsNone<ProtosDb>("Protos DB was already set.");
      this.m_protosDb = (Option<ProtosDb>) protosDb;
    }

    public void Dispose() => this.m_reader.Dispose();

    public string ReadSectionStart(string nameForVerification = null)
    {
      this.m_sb.Clear();
      int num1;
      while ((num1 = this.m_reader.Read()) >= 0)
      {
        switch (num1)
        {
          case 10:
            goto label_4;
          case 13:
            continue;
          default:
            this.m_sb.Append((char) num1);
            continue;
        }
      }
label_4:
      ++this.Level;
      string str = this.m_sb.ToString();
      if (nameForVerification != null && str != nameForVerification)
        throw new Exception("Failed to read section '" + nameForVerification + "', got '" + str + "'");
      for (int index = 0; index < this.Level; ++index)
      {
        int num2 = this.m_reader.Read();
        if (num2 != 32)
          throw new Exception(string.Format("Failed to read delimiter, got '{0}'.", (object) (char) num2));
      }
      return str;
    }

    public void ReadSectionEnd()
    {
      if (this.Level <= 0)
        throw new Exception("Failed to read section end, indent level is not positive.");
      --this.Level;
      this.ReadNewLine();
    }

    public void ReadNewLine(bool increaseIndent = false, bool decreaseIndent = false)
    {
      int num1 = this.m_reader.Read();
      if (num1 == 13)
        num1 = this.m_reader.Read();
      if (num1 != 10)
        throw new Exception(string.Format("Failed to read new line, got '{0}'.", (object) (char) num1));
      if (increaseIndent)
        ++this.Level;
      if (decreaseIndent)
      {
        --this.Level;
        Assert.That<int>(this.Level).IsNotNegative();
      }
      for (int index = 0; index < this.Level; ++index)
      {
        int num2 = this.m_reader.Read();
        if (num2 != 32)
          throw new Exception(string.Format("Failed to read delimiter, got '{0}'.", (object) (char) num2));
      }
    }

    public int ReadInt() => (int) this.ReadLong();

    public uint ReadUint() => (uint) this.ReadLong();

    public long ReadLong()
    {
      bool flag = false;
      long num1 = 0;
      int num2 = this.m_reader.Read();
      if (num2 == 45)
      {
        flag = true;
        num2 = this.m_reader.Read();
      }
      if (num2 < 48 || num2 > 57)
        throw new Exception(string.Format("Invalid first char '{0}' while reading int.", (object) num2));
      for (; num2 >= 0 && num2 != 32; num2 = this.m_reader.Read())
      {
        if (num2 < 48 || num2 > 57)
          throw new Exception(string.Format("Invalid char '{0}' while reading int.", (object) num2));
        num1 = num1 * 10L + (long) (num2 - 48);
      }
      return !flag ? num1 : -num1;
    }

    public ulong ReadULong()
    {
      ulong num = 0;
      for (int index = this.m_reader.Read(); index >= 0 && index != 32; index = this.m_reader.Read())
      {
        if (index < 48 || index > 57)
          throw new Exception(string.Format("Invalid char '{0}' while reading int.", (object) index));
        num = num * 10UL + (ulong) (index - 48);
      }
      return num;
    }

    public Fix32 ReadFix32() => Fix32.FromRaw(this.ReadInt());

    public string ReadString()
    {
      int num = this.ReadInt();
      char[] chArray = ArrayPool<char>.Get(num);
      this.m_reader.Read(chArray, 0, num);
      Assert.That<int>(this.m_reader.Read()).IsEqualTo(32);
      string str = new string(chArray);
      chArray.ReturnToPool<char>();
      return str;
    }

    public Vector2i ReadVector2i() => new Vector2i(this.ReadInt(), this.ReadInt());

    public Vector2f ReadVector2f() => new Vector2f(this.ReadFix32(), this.ReadFix32());

    public Vector3i ReadVector3i() => new Vector3i(this.ReadInt(), this.ReadInt(), this.ReadInt());

    public bool ReadBool() => this.ReadInt() != 0;

    public ImmutableArray<T> ReadArray<T>(Func<CustomTextReader, T> readFunc)
    {
      int length = this.ReadInt();
      ImmutableArrayBuilder<T> immutableArrayBuilder = new ImmutableArrayBuilder<T>(length);
      for (int i = 0; i < length; ++i)
        immutableArrayBuilder[i] = readFunc(this);
      return immutableArrayBuilder.GetImmutableArrayAndClear();
    }

    public Type ReadTypeOrThrow()
    {
      string typeName = this.ReadString();
      Type type;
      try
      {
        type = Type.GetType(typeName);
      }
      catch (Exception ex)
      {
        throw new CorruptedSaveException("Failed to read type '" + typeName + "'.", ex);
      }
      return !(type == (Type) null) ? type : throw new CorruptedSaveException("Failed to read type '" + typeName + "'.");
    }

    public bool TryReadType(out Type type)
    {
      string typeName = this.ReadString();
      try
      {
        type = Type.GetType(typeName);
        return true;
      }
      catch
      {
        type = (Type) null;
        Log.Warning("Failed to read type '" + typeName + "'.");
        return false;
      }
    }

    public bool TryReadGeneric(out object value)
    {
      if (!this.ReadBool())
      {
        value = (object) null;
        return false;
      }
      Type type = this.ReadTypeOrThrow();
      Func<object> readFn;
      if (!this.tryGetReadFunctionFor(type, out readFn))
        throw new CorruptedSaveException("Unhandled type '" + type.Name + "' to read.");
      value = readFn();
      return true;
    }

    private bool tryGetReadFunctionFor(Type type, out Func<object> readFn)
    {
      if (this.m_readGenericFns.TryGetValue(type, out readFn))
        return true;
      if (type.IsAssignableTo<Type>())
      {
        readFn = (Func<object>) (() => (object) this.ReadTypeOrThrow());
        this.m_readGenericFns.Add(type, readFn);
        return true;
      }
      if (type.IsValueType)
      {
        if (type.IsEnum)
        {
          readFn = (Func<object>) (() => (object) this.ReadInt());
          this.m_readGenericFns.Add(type, readFn);
          return true;
        }
        if (type.FullName.EndsWith("Proto+ID"))
        {
          readFn = (Func<object>) (() =>
          {
            string str = this.ReadString();
            Proto.ID id;
            if (this.m_protoIdsMap.TryGetValue(new Proto.ID(str), out id))
              str = id.Value;
            return Activator.CreateInstance(type, (object) str);
          });
          this.m_readGenericFns.Add(type, readFn);
          return true;
        }
      }
      if (type.IsArray)
      {
        if (type.GetElementType().IsAbstract)
        {
          readFn = (Func<object>) (() =>
          {
            int length = this.ReadInt();
            Array instance = Array.CreateInstance(type.GetElementType(), length);
            for (int index = 0; index < length; ++index)
            {
              object obj;
              if (this.TryReadGeneric(out obj))
                instance.SetValue(obj, index);
            }
            return (object) instance;
          });
          this.m_readGenericFns.Add(type, readFn);
          return true;
        }
        Func<object> elementReadFn;
        if (!this.tryGetReadFunctionFor(type.GetElementType(), out elementReadFn))
          return false;
        readFn = (Func<object>) (() =>
        {
          int length = this.ReadInt();
          Array instance = Array.CreateInstance(type.GetElementType(), length);
          for (int index = 0; index < length; ++index)
            instance.SetValue(elementReadFn(), index);
          return (object) instance;
        });
        this.m_readGenericFns.Add(type, readFn);
        return true;
      }
      if (type.IsAssignableTo<IConfig>())
      {
        PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        readFn = (Func<object>) (() =>
        {
          object uninitializedObject = FormatterServices.GetUninitializedObject(type);
          int num = this.ReadInt();
          if (num > 0)
          {
            for (int index = 0; index < num; ++index)
            {
              this.ReadNewLine(index == 0);
              string name = this.ReadString();
              object obj;
              if (this.TryReadGeneric(out obj))
              {
                MethodInfo setMethod = ((IEnumerable<PropertyInfo>) properties).FirstOrDefault<PropertyInfo>((Func<PropertyInfo, bool>) (x => x.Name == name))?.GetSetMethod(true);
                if (setMethod != (MethodInfo) null)
                  setMethod.Invoke(uninitializedObject, new object[1]
                  {
                    obj
                  });
                else
                  Log.Warning("Failed to set '" + name + "' on '" + type.Name + "'.");
              }
            }
            this.ReadNewLine(decreaseIndent: true);
          }
          return uninitializedObject;
        });
        this.m_readGenericFns.Add(type, readFn);
        return true;
      }
      if (type.IsPrimitive)
        throw new CorruptedSaveException("Unhandled primitive type '" + type.Name + "'.");
      if (type.IsAssignableTo<IDictNonGeneric>())
      {
        Type[] genericArguments = type.GetGenericArguments();
        Func<object> keyReadFn;
        Func<object> valueReadFn;
        if (!this.tryGetReadFunctionFor(genericArguments[0], out keyReadFn) || !this.tryGetReadFunctionFor(genericArguments[1], out valueReadFn))
          return false;
        readFn = (Func<object>) (() =>
        {
          IDictNonGeneric instance = (IDictNonGeneric) Activator.CreateInstance(type);
          int num = this.ReadInt();
          for (int index = 0; index < num; ++index)
            instance.Add(keyReadFn(), valueReadFn());
          return (object) instance;
        });
        this.m_readGenericFns.Add(type, readFn);
        return true;
      }
      FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
      bool flag = fields.Length == 1;
      if (flag)
      {
        string name = fields[0].Name;
        flag = name == "Value" || name == "RawValue" || name == "m_value";
      }
      readFn = !flag ? (Func<object>) (() =>
      {
        object uninitializedObject = FormatterServices.GetUninitializedObject(type);
        int num = this.ReadInt();
        if (num > 0)
        {
          for (int index = 0; index < num; ++index)
          {
            this.ReadNewLine(index == 0);
            string name = this.ReadString();
            object obj;
            if (this.TryReadGeneric(out obj))
            {
              FieldInfo fieldInfo = ((IEnumerable<FieldInfo>) fields).FirstOrDefault<FieldInfo>((Func<FieldInfo, bool>) (x => x.Name == name));
              if (fieldInfo != (FieldInfo) null)
                fieldInfo.SetValue(uninitializedObject, obj);
            }
          }
          this.ReadNewLine(decreaseIndent: true);
        }
        return uninitializedObject;
      }) : (Func<object>) (() =>
      {
        object uninitializedObject = FormatterServices.GetUninitializedObject(type);
        object obj;
        if (this.TryReadGeneric(out obj))
          fields[0].SetValue(uninitializedObject, obj);
        return uninitializedObject;
      });
      this.m_readGenericFns.Add(type, readFn);
      return true;
    }

    public void ReadProtoIdsMap(Stream stream)
    {
      if (this.m_protoIdsMap.IsNotEmpty)
        throw new CorruptedSaveException("Protos map was already loaded.");
      char[] separator = new char[2]{ ' ', '\t' };
      using (TextReader textReader = (TextReader) new StreamReader(stream))
      {
        while (textReader.Peek() >= 0)
        {
          string str1 = textReader.ReadLine();
          if (!string.IsNullOrEmpty(str1))
          {
            string str2 = str1.Trim();
            if (!str2.StartsWith("#"))
            {
              string[] strArray = str2.Split(separator, StringSplitOptions.RemoveEmptyEntries);
              if (strArray.Length != 2)
                Log.Warning("Failed to parse proto mapping from '" + str2 + "'.");
              else
                this.m_protoIdsMap.AddAndAssertNew(new Proto.ID(strArray[0]), new Proto.ID(strArray[1]));
            }
          }
        }
      }
      Log.Info(string.Format("Loaded {0} ID mappings for deserialization.", (object) this.m_protoIdsMap.Count));
    }

    public Proto.ID ReadProtoId()
    {
      if (this.m_protosDb.IsNone)
        throw new InvalidOperationException("Protos DB was not set.");
      Proto.ID id1 = new Proto.ID(this.ReadString());
      Proto.ID id2;
      if (this.m_protoIdsMap.TryGetValue(id1, out id2))
        id1 = id2;
      if (!this.m_protosDb.Value.TryGetProto<Proto>(id1, out Proto _))
        this.m_missingProtoIds.Add(id1);
      return id1;
    }

    public bool TryReadProto<TProto>(out TProto proto) where TProto : Proto
    {
      if (this.m_protosDb.IsNone)
        throw new InvalidOperationException("Protos DB was not set.");
      Proto.ID id = this.ReadProtoId();
      if (this.m_protosDb.Value.TryGetProto<TProto>(id, out proto))
        return true;
      Log.Warning(string.Format("Proto '{0}' was not found ({1}).", (object) id, (object) typeof (TProto).Name));
      return false;
    }
  }
}
