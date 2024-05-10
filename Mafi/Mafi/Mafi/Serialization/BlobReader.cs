// Decompiled with JetBrains decompiler
// Type: Mafi.Serialization.BlobReader
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

#nullable disable
namespace Mafi.Serialization
{
  public class BlobReader
  {
    public readonly int LoadedSaveVersion;
    private readonly BinaryReader m_reader;
    private ImmutableArray<ISpecialSerializerFactory> m_specialSerializers;
    private readonly Dict<Type, object> m_deserializersCache;
    private readonly Lyst<object> m_readObjects;
    private readonly Lyst<Type> m_readTypes;
    private readonly Lyst<BlobReader.ResolvedMember> m_membersToResolve;
    private readonly Lyst<BlobReader.InitAfterLoad> m_objsToInit;
    private readonly Queueue<KeyValuePair<object, Action<object, BlobReader>>> m_delayedDataDeserializations;
    private readonly Dict<Type, Lyst<BlobReader.FieldData>> m_fieldsCache;

    public Stream InputStream => this.m_reader.BaseStream;

    public int DelayedDeserializationsCount => this.m_delayedDataDeserializations.Count;

    public BlobReader(
      Stream inputStream,
      int loadedSaveVersion,
      ImmutableArray<ISpecialSerializerFactory> specialSerializers = default (ImmutableArray<ISpecialSerializerFactory>))
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.m_deserializersCache = new Dict<Type, object>();
      this.m_readObjects = new Lyst<object>();
      this.m_readTypes = new Lyst<Type>();
      this.m_membersToResolve = new Lyst<BlobReader.ResolvedMember>();
      this.m_objsToInit = new Lyst<BlobReader.InitAfterLoad>();
      this.m_delayedDataDeserializations = new Queueue<KeyValuePair<object, Action<object, BlobReader>>>();
      this.m_fieldsCache = new Dict<Type, Lyst<BlobReader.FieldData>>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      Assert.That<bool>(inputStream.CanRead).IsTrue("Cannot read from given input stream!");
      this.LoadedSaveVersion = loadedSaveVersion;
      this.m_specialSerializers = specialSerializers.IsValid ? specialSerializers : ImmutableArray<ISpecialSerializerFactory>.Empty;
      this.m_reader = new BinaryReader(inputStream.CheckNotNull<Stream>(), Encoding.UTF8, true);
      this.Reset();
    }

    public void Destroy()
    {
      Assert.That<bool>(this.m_delayedDataDeserializations.IsEmpty).IsTrue("There are some objects to serialize, did you call `FinalizeLoading`?");
      Assert.That<bool>(this.m_membersToResolve.IsEmpty).IsTrue("There are some members to resolve, did you call `FinalizeLoading`?");
      Assert.That<bool>(this.m_objsToInit.IsEmpty).IsTrue("There are some classes to initialize, did you call `FinalizeLoading`?");
      this.m_reader.Dispose();
      this.m_readObjects.Clear();
      this.m_readTypes.Clear();
    }

    public void Reset()
    {
      this.m_readObjects.Clear();
      this.m_readTypes.Clear();
      this.m_readObjects.Add((object) null);
      this.m_readTypes.Add((Type) null);
    }

    public void SetSpecialSerializers(
      ImmutableArray<ISpecialSerializerFactory> specialSerializers)
    {
      Assert.That<Queueue<KeyValuePair<object, Action<object, BlobReader>>>>(this.m_delayedDataDeserializations).IsEmpty<KeyValuePair<object, Action<object, BlobReader>>>("Some objects are not finished reading when setting special serializers.");
      this.m_specialSerializers = specialSerializers;
    }

    public long GetRemainingBytes()
    {
      Stream baseStream = this.m_reader.BaseStream;
      if (baseStream is GZipStream gzipStream)
        baseStream = gzipStream.BaseStream;
      return baseStream.Length - baseStream.Position;
    }

    public Percent GetReadProgress()
    {
      Stream baseStream = this.m_reader.BaseStream;
      if (baseStream is GZipStream gzipStream)
        baseStream = gzipStream.BaseStream;
      return baseStream.Length <= 0L ? Percent.Zero : Percent.FromRatio(baseStream.Position, baseStream.Length);
    }

    public void SetField<T>(T obj, string fieldName, object value) where T : class
    {
      this.setField((object) obj, typeof (T), fieldName, value);
    }

    private void setField(object obj, Type type, string fieldName, object value)
    {
      Type type1 = obj.GetType();
      Lyst<BlobReader.FieldData> lyst;
      if (!this.m_fieldsCache.TryGetValue(type1, out lyst))
      {
        lyst = new Lyst<BlobReader.FieldData>(8);
        this.m_fieldsCache.Add(type1, lyst);
      }
      for (int index = 0; index < lyst.Count; ++index)
      {
        BlobReader.FieldData fieldData = lyst[index];
        if (fieldData.Type == type && fieldData.Name == fieldName)
        {
          fieldData.Field.SetValue(obj, value);
          return;
        }
      }
      FieldInfo field = type.GetField(fieldName, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
      if (field == (FieldInfo) null)
      {
        Log.Error("Field '" + fieldName + "' on object '" + type.Name + "' does not exist.");
      }
      else
      {
        lyst.Add(new BlobReader.FieldData()
        {
          Field = field,
          Name = fieldName,
          Type = type
        });
        field.SetValue(obj, value);
      }
    }

    public void SetProperty<T>(T obj, string propName, object value) where T : class
    {
      this.setProperty((object) obj, typeof (T), propName, value);
    }

    private void setProperty(object obj, Type type, string propName, object value)
    {
      PropertyInfo property = type.GetProperty(propName, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
      if (property == (PropertyInfo) null)
        Log.Error("Property '" + propName + "' on object '" + type.Name + "' does not exist.");
      else
        property.SetValue(obj, value);
    }

    public void RegisterResolvedMember<T>(
      T obj,
      string name,
      Type resolvedType,
      bool isField,
      Func<object, object> convertBeforeAssignment = null)
      where T : class
    {
      if ((object) obj == null)
        return;
      this.m_membersToResolve.Add(new BlobReader.ResolvedMember((object) obj, typeof (T), name, resolvedType, (Option<Func<object, object>>) convertBeforeAssignment, isField));
    }

    public void RegisterInitAfterLoad<T>(T obj, string methodName, InitPriority priority) where T : class
    {
      if ((object) obj == null)
        return;
      this.m_objsToInit.Add(new BlobReader.InitAfterLoad((object) obj, typeof (T), methodName, priority));
    }

    /// <summary>
    /// Reads data for all loaded classes. This has to be called at the end of loading.
    /// </summary>
    public void FinalizeLoading(Option<DependencyResolver> resolver, Action beforeInitCalls = null)
    {
      this.FinalizeLoadingTimeSliced(resolver, int.MaxValue, beforeInitCalls).EnumerateToTheEnd<string>();
    }

    public IEnumerator<string> FinalizeLoadingTimeSliced(
      Option<DependencyResolver> resolver,
      int pauseAfterMs,
      Action beforeInitCalls = null)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator<string>) new BlobReader.\u003CFinalizeLoadingTimeSliced\u003Ed__30(0)
      {
        \u003C\u003E4__this = this,
        resolver = resolver,
        pauseAfterMs = pauseAfterMs,
        beforeInitCalls = beforeInitCalls
      };
    }

    private IEnumerator<string> invokeInit(
      BlobReader.InitAfterLoad initData,
      Option<DependencyResolver> resolver)
    {
      MethodInfo method = initData.Type.GetMethod(initData.MethodName, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
      if (method == (MethodInfo) null)
      {
        Log.Error("Failed to find init method '" + initData.MethodName + "' on '" + initData.Type.Name + "'.");
        return (IEnumerator<string>) null;
      }
      ParameterInfo[] parameters = method.GetParameters();
      if (parameters.Length == 0)
        return method.Invoke(initData.Obj, Array.Empty<object>()) as IEnumerator<string>;
      object[] objArray = ArrayPool<object>.Get(parameters.Length);
      for (int index = 0; index < objArray.Length; ++index)
      {
        Type parameterType = parameters[index].ParameterType;
        if (parameterType == typeof (int))
          objArray[index] = (object) this.LoadedSaveVersion;
        else if (parameterType == typeof (DependencyResolver))
        {
          if (resolver.IsNone)
          {
            Log.Error("Init method '" + initData.MethodName + "' on '" + initData.Type.Name + "' cannot be called with resolver, no resolver is available at this stage.");
            return (IEnumerator<string>) null;
          }
          objArray[index] = (object) resolver.Value;
        }
        else
        {
          Log.Error("Failed to invoke init method '" + initData.MethodName + "' on '" + initData.Type.Name + "' due to invalid parameter type '" + parameterType.Name + "'.");
          objArray.ReturnToPool<object>();
          return (IEnumerator<string>) null;
        }
      }
      object obj = method.Invoke(initData.Obj, objArray);
      objArray.ReturnToPool<object>();
      return obj as IEnumerator<string>;
    }

    public Func<BlobReader, T> GetDeserializerFor<T>()
    {
      object deserializerFor1;
      if (this.m_deserializersCache.TryGetValue(typeof (T), out deserializerFor1))
        return (Func<BlobReader, T>) deserializerFor1;
      Func<BlobReader, T> deserializerFor2 = (Func<BlobReader, T>) null;
      foreach (ISpecialSerializerFactory specialSerializer in this.m_specialSerializers)
      {
        if (specialSerializer.CanSerialize<T>())
          deserializerFor2 = specialSerializer.GetDeserializeFunction<T>();
      }
      if (deserializerFor2 == null)
        deserializerFor2 = GenericSerializersFactory.GetDeserializeFunction<T>();
      this.m_deserializersCache.Add(typeof (T), (object) deserializerFor2);
      return deserializerFor2;
    }

    public T ReadGenericAs<T>()
    {
      Func<BlobReader, T> deserializerFor;
      try
      {
        deserializerFor = this.GetDeserializerFor<T>();
      }
      catch (Exception ex)
      {
        throw new Exception("Failed to create generic deserializer for '" + typeof (T).Name + "'.", ex);
      }
      return deserializerFor(this);
    }

    public bool ReadBool() => this.m_reader.ReadBoolean();

    public byte ReadByte() => this.m_reader.ReadByte();

    public sbyte ReadSByte() => this.m_reader.ReadSByte();

    public char ReadChar() => this.m_reader.ReadChar();

    public short ReadShort() => this.m_reader.ReadInt16();

    [OnlyForSaveCompatibility(null)]
    public short ReadShortVariable()
    {
      ushort num = this.LoadedSaveVersion < 166 ? this.ReadUShort() : this.ReadUShortVariable();
      return (short) ((int) num >> 1 ^ -((int) num & 1));
    }

    public ushort ReadUShort() => this.m_reader.ReadUInt16();

    public ushort ReadUShortVariable()
    {
      byte num1 = this.m_reader.ReadByte();
      uint num2 = (uint) num1 & (uint) sbyte.MaxValue;
      if (((int) num1 & 128) == 0)
        return (ushort) num2;
      byte num3 = this.m_reader.ReadByte();
      uint num4 = num2 | (uint) (((int) num3 & (int) sbyte.MaxValue) << 7);
      if (((int) num3 & 128) == 0)
        return (ushort) num4;
      byte num5 = this.m_reader.ReadByte();
      return (ushort) (num4 | (uint) (((int) num5 & (int) sbyte.MaxValue) << 14));
    }

    public int ReadIntNonVariable() => this.m_reader.ReadInt32();

    public int ReadInt()
    {
      uint num = this.ReadUInt();
      return (int) (num >> 1) ^ -((int) num & 1);
    }

    public int ReadIntNotNegative()
    {
      int num = (int) this.ReadUInt();
      return num >= 0 ? num : throw new CorruptedSaveException(string.Format("Value {0} is negative!", (object) num));
    }

    public uint ReadUIntNonVariable() => this.m_reader.ReadUInt32();

    public uint ReadUInt()
    {
      byte num1 = this.m_reader.ReadByte();
      uint num2 = (uint) num1 & (uint) sbyte.MaxValue;
      if (((int) num1 & 128) == 0)
        return num2;
      byte num3 = this.m_reader.ReadByte();
      uint num4 = num2 | (uint) (((int) num3 & (int) sbyte.MaxValue) << 7);
      if (((int) num3 & 128) == 0)
        return num4;
      byte num5 = this.m_reader.ReadByte();
      uint num6 = num4 | (uint) (((int) num5 & (int) sbyte.MaxValue) << 14);
      if (((int) num5 & 128) == 0)
        return num6;
      byte num7 = this.m_reader.ReadByte();
      uint num8 = num6 | (uint) (((int) num7 & (int) sbyte.MaxValue) << 21);
      if (((int) num7 & 128) == 0)
        return num8;
      byte num9 = this.m_reader.ReadByte();
      if (num9 > (byte) 15)
        throw new CorruptedSaveException("Failed to read variable-encoded uint, last byte is too high.");
      return num8 | (uint) num9 << 28;
    }

    public long ReadLongNonVariable() => this.m_reader.ReadInt64();

    public long ReadLong()
    {
      ulong num = this.ReadULong();
      return (long) (num >> 1) ^ -((long) num & 1L);
    }

    public long ReadLongNotNegative()
    {
      long num = (long) this.ReadULong();
      return num >= 0L ? num : throw new CorruptedSaveException(string.Format("Value {0} is negative!", (object) num));
    }

    public ulong ReadULongNonVariable() => this.m_reader.ReadUInt64();

    public ulong ReadULong()
    {
      byte num1 = this.m_reader.ReadByte();
      ulong num2 = (ulong) num1 & (ulong) sbyte.MaxValue;
      if (((int) num1 & 128) == 0)
        return num2;
      byte num3 = this.m_reader.ReadByte();
      ulong num4 = num2 | (ulong) (((long) num3 & (long) sbyte.MaxValue) << 7);
      if (((int) num3 & 128) == 0)
        return num4;
      byte num5 = this.m_reader.ReadByte();
      ulong num6 = num4 | (ulong) (((long) num5 & (long) sbyte.MaxValue) << 14);
      if (((int) num5 & 128) == 0)
        return num6;
      byte num7 = this.m_reader.ReadByte();
      ulong num8 = num6 | (ulong) (((long) num7 & (long) sbyte.MaxValue) << 21);
      if (((int) num7 & 128) == 0)
        return num8;
      byte num9 = this.m_reader.ReadByte();
      ulong num10 = num8 | (ulong) (((long) num9 & (long) sbyte.MaxValue) << 28);
      if (((int) num9 & 128) == 0)
        return num10;
      byte num11 = this.m_reader.ReadByte();
      ulong num12 = num10 | (ulong) (((long) num11 & (long) sbyte.MaxValue) << 35);
      if (((int) num11 & 128) == 0)
        return num12;
      byte num13 = this.m_reader.ReadByte();
      ulong num14 = num12 | (ulong) (((long) num13 & (long) sbyte.MaxValue) << 42);
      if (((int) num13 & 128) == 0)
        return num14;
      byte num15 = this.m_reader.ReadByte();
      ulong num16 = num14 | (ulong) (((long) num15 & (long) sbyte.MaxValue) << 49);
      return ((int) num15 & 128) == 0 ? num16 : num16 | (ulong) this.m_reader.ReadByte() << 56;
    }

    public float ReadFloat() => this.m_reader.ReadSingle();

    public double ReadDouble() => this.m_reader.ReadDouble();

    public DateTime ReadDateTime() => DateTime.FromFileTimeUtc(this.ReadLong());

    public string ReadString()
    {
      int index = this.ReadIntNotNegative();
      if (index < this.m_readObjects.Count)
      {
        object readObject = this.m_readObjects[index];
        if (readObject == null)
        {
          Log.Warning("Deserializing null of type 'string'.");
          return (string) null;
        }
        return readObject is string str ? str : throw new CorruptedSaveException("Deserialized object has type '" + readObject.GetType().Name + "' " + string.Format("but type '{0}' was expected. ToString returned '{1}'.", (object) "String", readObject));
      }
      if (index != this.m_readObjects.Count)
        throw new CorruptedSaveException("New deserialized object was expected to have ID " + string.Format("{0} but value {1} was read.", (object) this.m_readObjects.Count, (object) index));
      string str1 = this.m_reader.ReadString();
      this.m_readObjects.Add((object) str1);
      return str1;
    }

    internal string ReadStringNoRef() => this.m_reader.ReadString();

    public T? ReadNullableStruct<T>() where T : struct
    {
      return this.ReadBool() ? new T?(this.ReadGenericAs<T>()) : new T?();
    }

    public KeyValuePair<TKey, TValue> ReadKeyValuePair<TKey, TValue>()
    {
      return new KeyValuePair<TKey, TValue>(this.ReadGenericAs<TKey>(), this.ReadGenericAs<TValue>());
    }

    public Type ReadType(bool allowNulls = false)
    {
      int index = this.ReadIntNotNegative();
      if (index < this.m_readTypes.Count)
      {
        Type readType = this.m_readTypes[index];
        if (readType == (Type) null && !allowNulls)
          Log.Error("Deserializing null of type 'Type'.");
        return readType;
      }
      if (index != this.m_readTypes.Count)
        throw new CorruptedSaveException("New deserialized type was expected to have ID " + string.Format("{0} but value {1} was read.", (object) this.m_readTypes.Count, (object) index));
      Type type = this.ReadTypeFromStrNoRef();
      this.m_readTypes.Add(type);
      return type;
    }

    public Type ReadTypeFromStrNoRef()
    {
      string typeName = this.ReadStringNoRef();
      Type type;
      try
      {
        type = Type.GetType(typeName);
      }
      catch (Exception ex)
      {
        throw new CorruptedSaveException("Failed to load type '" + typeName + "'.", ex);
      }
      return !(type == (Type) null) ? type : throw new CorruptedSaveException("Failed to load type '" + typeName + "'.");
    }

    public string ReadTypeNameAsStrNoRef() => this.ReadStringNoRef();

    public T[] ReadArray<T>()
    {
      T[] arr;
      if (this.TryStartArrayDeserialization<T>(out arr) && arr.Length != 0)
      {
        Func<BlobReader, T> deserializerFor = this.GetDeserializerFor<T>();
        for (int index = 0; index < arr.Length; ++index)
          arr[index] = deserializerFor(this);
      }
      return arr;
    }

    public T[] ReadArray<T>(int length)
    {
      T[] arr;
      if (this.TryStartArrayDeserialization<T>(out arr) && length > 0)
      {
        if (length > arr.Length)
        {
          Log.Error(string.Format("Trying to read {0} elements to array of length {1}.", (object) length, (object) arr.Length));
          length = arr.Length;
        }
        Func<BlobReader, T> deserializerFor = this.GetDeserializerFor<T>();
        for (int index = 0; index < length; ++index)
          arr[index] = deserializerFor(this);
      }
      return arr;
    }

    public ImmutableArray<byte> ReadBytesArray()
    {
      return ImmutableArray<byte>.ReadFrom(this.m_reader, this.ReadIntNotNegative());
    }

    /// <summary>
    /// Starts class deserialization by reading next class ID from the stream. If the deserialized class is new,
    /// previously unseen, creates an empty instance and returns <c>true</c>. If the class was previously seen
    /// and deserialized <c>false</c> is returned. It is sole responsibility of the caller to properly load class
    /// data when <c>true</c> was returned.
    /// 
    /// See <see cref="M:Mafi.Serialization.BlobWriter.TryStartClassSerialization``1(``0)" />.
    /// </summary>
    /// <param name="customNewObjFactory">Custom factory function. WARNING: This function cannot read new objects.</param>
    public bool TryStartClassDeserialization<T>(
      out T obj,
      Func<BlobReader, Type, T> customNewObjFactory = null)
      where T : class
    {
      int index = this.ReadIntNotNegative();
      if (index < this.m_readObjects.Count)
      {
        object readObject = this.m_readObjects[index];
        if (readObject == null)
        {
          Log.Warning("Deserializing null of type '" + typeof (T).Name + "'.");
          obj = default (T);
        }
        else
          obj = readObject is T obj1 ? obj1 : throw new CorruptedSaveException("Deserialized object has type '" + readObject.GetType().Name + "' " + string.Format("but type '{0}' was expected. ToString returned '{1}'.", (object) typeof (T).Name, readObject));
        return false;
      }
      if (index != this.m_readObjects.Count)
        throw new CorruptedSaveException("New deserialized object of type'" + typeof (T).Name + "' was expected to have ID " + string.Format("'{0}' but value '{1}' was read.", (object) this.m_readObjects.Count, (object) index));
      Type type = this.ReadType();
      object obj2 = customNewObjFactory != null ? (object) customNewObjFactory(this, type) : FormatterServices.GetUninitializedObject(type);
      this.m_readObjects.Add(obj2);
      if (obj2 == null)
      {
        Log.Warning("Deserializing null of type '" + typeof (T).Name + "' (returned by factory).");
        obj = default (T);
        return false;
      }
      obj = obj2 is T obj3 ? obj3 : throw new CorruptedSaveException("Deserialized object has type '" + obj2.GetType().Name + "' " + string.Format("but type '{0}' was expected. ToString returned '{1}'.", (object) typeof (T).Name, obj2));
      return true;
    }

    public bool TryStartArrayDeserialization<T>(out T[] arr)
    {
      int index = this.ReadIntNotNegative();
      if (index < this.m_readObjects.Count)
      {
        object readObject = this.m_readObjects[index];
        if (readObject == null)
        {
          Log.Error("Deserializing null of type '" + typeof (T[]).Name + "'.");
          arr = (T[]) null;
        }
        else
          arr = readObject is T[] objArray ? objArray : throw new CorruptedSaveException("Deserialized object has type '" + readObject.GetType().Name + "' " + string.Format("but array of type '{0}' was expected. ToString returned '{1}'.", (object) typeof (T[]).Name, readObject));
        return false;
      }
      if (index != this.m_readObjects.Count)
        throw new CorruptedSaveException("New deserialized array was expected to have ID " + string.Format("{0} but value {1} was read.", (object) this.m_readObjects.Count, (object) index));
      object instance = (object) Array.CreateInstance(this.ReadType(), this.ReadIntNotNegative());
      this.m_readObjects.Add(instance);
      arr = instance is T[] objArray1 ? objArray1 : throw new CorruptedSaveException("Deserialized array has type '" + instance.GetType().Name + "' but array of type '" + typeof (T[]).Name + "' was expected.");
      return true;
    }

    /// <summary>
    /// Opposite of <see cref="M:Mafi.Serialization.BlobWriter.EnqueueDataSerialization(System.Object,System.Action{System.Object,Mafi.Serialization.BlobWriter})" />.
    /// </summary>
    public void EnqueueDataDeserialization(object obj, Action<object, BlobReader> deserializeAction)
    {
      this.m_delayedDataDeserializations.Enqueue(Make.Kvp<object, Action<object, BlobReader>>(obj, deserializeAction));
    }

    private struct FieldData
    {
      public FieldInfo Field;
      public Type Type;
      public string Name;
    }

    private readonly struct ResolvedMember
    {
      public readonly object Obj;
      public readonly Type Type;
      public readonly string Name;
      public readonly Type ResolvedType;
      public readonly Option<Func<object, object>> ConvertBeforeAssignment;
      public readonly bool IsField;

      public ResolvedMember(
        object obj,
        Type type,
        string name,
        Type resolvedType,
        Option<Func<object, object>> convertBeforeAssignment,
        bool isField)
      {
        MBiHIp97M4MqqbtZOh.RFLpSOptx();
        this.Obj = obj;
        this.Type = type;
        this.Name = name;
        this.ResolvedType = resolvedType;
        this.ConvertBeforeAssignment = convertBeforeAssignment;
        this.IsField = isField;
      }
    }

    private readonly struct InitAfterLoad
    {
      public readonly object Obj;
      public readonly Type Type;
      public readonly string MethodName;
      public readonly InitPriority Priority;

      public InitAfterLoad(object obj, Type type, string methodName, InitPriority priority)
      {
        MBiHIp97M4MqqbtZOh.RFLpSOptx();
        this.Obj = obj;
        this.Type = type;
        this.MethodName = methodName;
        this.Priority = priority;
      }
    }
  }
}
