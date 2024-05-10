// Decompiled with JetBrains decompiler
// Type: Mafi.Core.SaveGame.CustomSerializer.CustomTextWriter
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
using System.Text;

#nullable disable
namespace Mafi.Core.SaveGame.CustomSerializer
{
  public class CustomTextWriter : IDisposable
  {
    public readonly int SaveVersion;
    private readonly StreamWriter m_writer;
    private readonly SaveCompressionType m_compressionType;
    private readonly Assembly m_executingAssembly;
    private readonly Assembly m_mscorLibAssembly;
    private readonly Dict<Type, Action<object>> m_writeGenericActions;

    public int Level { get; private set; }

    public CustomTextWriter(Stream stream, int saveVersion)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_mscorLibAssembly = typeof (int).Assembly;
      this.m_writeGenericActions = new Dict<Type, Action<object>>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.SaveVersion = saveVersion;
      this.m_writer = new StreamWriter(stream, (Encoding) new UTF8Encoding(false), 4096, true);
      this.m_writeGenericActions.Add(typeof (bool), (Action<object>) (x => this.WriteBool((bool) x)));
      this.m_writeGenericActions.Add(typeof (sbyte), (Action<object>) (x => this.WriteInt((int) (sbyte) x)));
      this.m_writeGenericActions.Add(typeof (byte), (Action<object>) (x => this.WriteInt((int) (byte) x)));
      this.m_writeGenericActions.Add(typeof (short), (Action<object>) (x => this.WriteInt((int) (short) x)));
      this.m_writeGenericActions.Add(typeof (ushort), (Action<object>) (x => this.WriteInt((int) (ushort) x)));
      this.m_writeGenericActions.Add(typeof (char), (Action<object>) (x => this.WriteInt((int) (char) x)));
      this.m_writeGenericActions.Add(typeof (int), (Action<object>) (x => this.WriteInt((int) x)));
      this.m_writeGenericActions.Add(typeof (uint), (Action<object>) (x => this.WriteUInt((uint) x)));
      this.m_writeGenericActions.Add(typeof (long), (Action<object>) (x => this.WriteLong((long) x)));
      this.m_writeGenericActions.Add(typeof (ulong), (Action<object>) (x => this.WriteULong((ulong) x)));
      this.m_writeGenericActions.Add(typeof (string), (Action<object>) (x => this.WriteString((string) x)));
      this.m_writeGenericActions.Add(typeof (LocStr), (Action<object>) (x =>
      {
        LocStr locStr = (LocStr) x;
        this.WriteString(locStr.Id);
        this.WriteString(locStr.TranslatedString);
      }));
      this.m_executingAssembly = Assembly.GetExecutingAssembly();
    }

    public void WriteHeader()
    {
      this.m_writer.Flush();
      this.m_writer.BaseStream.Write(SaveHeaders.HEADER_CUSTOM_SAVE, 0, 8);
      this.WriteInt(this.SaveVersion);
      this.WriteNewLine();
      this.WriteInt((int) this.m_compressionType);
      this.WriteNewLine();
    }

    public void Dispose() => this.m_writer.Dispose();

    public void WriteSectionStart(string name)
    {
      if (name.Contains(' '.ToString()))
        throw new Exception("Failed to write section start, name contains new line.");
      this.m_writer.Write(name);
      ++this.Level;
      this.WriteNewLine();
    }

    public void WriteSectionEnd()
    {
      if (this.Level <= 0)
        throw new Exception("Failed to write section end, indent level is not positive.");
      --this.Level;
      this.WriteNewLine();
    }

    public void WriteNewLine(bool increaseIndent = false, bool decreaseIndent = false)
    {
      this.m_writer.Write('\n');
      if (increaseIndent)
        ++this.Level;
      if (decreaseIndent)
      {
        --this.Level;
        Assert.That<int>(this.Level).IsNotNegative();
      }
      for (int index = 0; index < this.Level; ++index)
        this.m_writer.Write(' ');
    }

    public void WriteInt(int i) => this.WriteLong((long) i);

    public void WriteUInt(uint i) => this.WriteULong((ulong) i);

    public void WriteLong(long i)
    {
      this.m_writer.Write(i.ToString());
      this.m_writer.Write(' ');
    }

    public void WriteULong(ulong i)
    {
      this.m_writer.Write(i.ToString());
      this.m_writer.Write(' ');
    }

    public void WriteFix32(Fix32 f) => this.WriteInt(f.RawValue);

    public void WriteString(string str)
    {
      if (str == null)
      {
        Log.Error("Writing null string, it will be written as empty string.");
        str = "";
      }
      this.WriteInt(str.Length);
      this.m_writer.Write(str);
      this.m_writer.Write(' ');
    }

    public void WriteProtoId(Proto.ID id) => this.WriteString(id.Value);

    public void WriteProto(Proto proto)
    {
      if (proto == (Proto) null)
        throw new Exception("Writing null proto");
      this.WriteString(proto.Id.Value);
    }

    public void WriteVector2i(Vector2i vec)
    {
      this.WriteInt(vec.X);
      this.WriteInt(vec.Y);
    }

    public void WriteVector2f(Vector2f vec)
    {
      this.WriteFix32(vec.X);
      this.WriteFix32(vec.Y);
    }

    public void WriteVector3i(Vector3i vec)
    {
      this.WriteInt(vec.X);
      this.WriteInt(vec.Y);
      this.WriteInt(vec.Z);
    }

    public void WriteBool(bool b) => this.WriteInt(b ? 1 : 0);

    public void WriteArray<T>(T[] arr, Action<CustomTextWriter, T> writeFunc)
    {
      this.WriteInt(arr.Length);
      foreach (T obj in arr)
        writeFunc(this, obj);
    }

    public void WriteArray<T>(ImmutableArray<T> arr, Action<CustomTextWriter, T> writeFunc)
    {
      this.WriteInt(arr.Length);
      foreach (T obj in arr)
        writeFunc(this, obj);
    }

    public void WriteType(Type type)
    {
      this.WriteString(getShortTypeName(type));

      string getShortTypeName(Type t)
      {
        return !(t.Assembly == this.m_mscorLibAssembly) && !(t.Assembly == this.m_executingAssembly) ? t.AssemblyQualifiedName : t.FullName;
      }
    }

    public void WriteGeneric(object value)
    {
      if (value == null)
      {
        this.WriteBool(false);
      }
      else
      {
        Type type = value.GetType();
        Action<object> writeAction;
        if (this.tryGetWriteActionFor(type, out writeAction))
        {
          this.WriteBool(true);
          this.WriteType(type);
          writeAction(value);
        }
        else
        {
          this.WriteBool(false);
          Log.Warning(string.Format("Failed to write value '{0}', unhandled type {1}.", value, (object) value.GetType().Name));
        }
      }
    }

    private bool tryGetWriteActionFor(Type type, out Action<object> writeAction)
    {
      if (this.m_writeGenericActions.TryGetValue(type, out writeAction))
        return true;
      if (type.IsAssignableTo<Type>())
      {
        writeAction = (Action<object>) (value => this.WriteType((Type) value));
        this.m_writeGenericActions.Add(type, writeAction);
        return true;
      }
      if (type.IsValueType)
      {
        if (type.IsEnum)
        {
          writeAction = (Action<object>) (value => this.WriteInt((int) value));
          this.m_writeGenericActions.Add(type, writeAction);
          return true;
        }
        if (type.FullName.EndsWith("Proto+ID"))
        {
          FieldInfo valueField = type.GetField("Value", BindingFlags.Instance | BindingFlags.Public);
          writeAction = !(valueField == (FieldInfo) null) && !(valueField.FieldType != typeof (string)) ? (Action<object>) (value => this.WriteString((string) valueField.GetValue(value))) : throw new CorruptedSaveException("Missing or invalid 'Value' field on " + type.Name + ".");
          this.m_writeGenericActions.Add(type, writeAction);
          return true;
        }
      }
      if (type.IsArray)
      {
        Type elementType = type.GetElementType();
        if (elementType.IsAbstract)
        {
          writeAction = (Action<object>) (value =>
          {
            Array array = (Array) value;
            int length = array.Length;
            this.WriteInt(length);
            for (int index = 0; index < length; ++index)
              this.WriteGeneric(array.GetValue(index));
          });
          this.m_writeGenericActions.Add(type, writeAction);
          return true;
        }
        Action<object> elementWriteAction;
        if (!this.tryGetWriteActionFor(elementType, out elementWriteAction))
          return false;
        writeAction = (Action<object>) (value =>
        {
          Array array = (Array) value;
          int length = array.Length;
          this.WriteInt(length);
          for (int index = 0; index < length; ++index)
            elementWriteAction(array.GetValue(index));
        });
        this.m_writeGenericActions.Add(type, writeAction);
        return true;
      }
      if (type.IsAbstract)
        throw new CorruptedSaveException("Trying to write abstract type '" + type.Name + "'.");
      if (type.IsAssignableTo<IConfig>())
      {
        PropertyInfo[] properties = ((IEnumerable<PropertyInfo>) type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)).Where<PropertyInfo>((Func<PropertyInfo, bool>) (x => x.GetSetMethod(true) != (MethodInfo) null)).ToArray<PropertyInfo>();
        writeAction = (Action<object>) (value =>
        {
          this.WriteInt(properties.Length);
          if (properties.Length == 0)
            return;
          for (int index = 0; index < properties.Length; ++index)
          {
            this.WriteNewLine(index == 0);
            PropertyInfo propertyInfo = properties[index];
            this.WriteString(propertyInfo.Name);
            this.WriteGeneric(propertyInfo.GetValue(value));
          }
          this.WriteNewLine(decreaseIndent: true);
        });
        this.m_writeGenericActions.Add(type, writeAction);
        return true;
      }
      if (type.IsPrimitive)
        throw new CorruptedSaveException("Unhandled primitive type '" + type.Name + "'.");
      if (type.IsAssignableTo<IDictNonGeneric>())
      {
        Type[] genericArguments = type.GetGenericArguments();
        Action<object> keyWriteAction;
        Action<object> valueWriteAction;
        if (!this.tryGetWriteActionFor(genericArguments[0], out keyWriteAction) || !this.tryGetWriteActionFor(genericArguments[1], out valueWriteAction))
          return false;
        writeAction = (Action<object>) (value =>
        {
          IDictNonGeneric dictNonGeneric = (IDictNonGeneric) value;
          if (!dictNonGeneric.HasDefaultComparer)
            throw new CorruptedSaveException("Dict without default comparer cannot be saved.");
          this.WriteInt(dictNonGeneric.Count);
          foreach (KeyValuePair<object, object> keyValuePair in dictNonGeneric)
          {
            keyWriteAction(keyValuePair.Key);
            valueWriteAction(keyValuePair.Value);
          }
        });
        this.m_writeGenericActions.Add(type, writeAction);
        return true;
      }
      FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
      bool flag = fields.Length == 1;
      if (flag)
      {
        string name = fields[0].Name;
        flag = name == "Value" || name == "RawValue" || name == "m_value";
      }
      writeAction = !flag ? (Action<object>) (value =>
      {
        this.WriteInt(fields.Length);
        if (fields.Length == 0)
          return;
        for (int index = 0; index < fields.Length; ++index)
        {
          this.WriteNewLine(index == 0);
          FieldInfo fieldInfo = fields[index];
          this.WriteString(fieldInfo.Name);
          this.WriteGeneric(fieldInfo.GetValue(value));
        }
        this.WriteNewLine(decreaseIndent: true);
      }) : (Action<object>) (value => this.WriteGeneric(fields[0].GetValue(value)));
      this.m_writeGenericActions.Add(type, writeAction);
      return true;
    }
  }
}
