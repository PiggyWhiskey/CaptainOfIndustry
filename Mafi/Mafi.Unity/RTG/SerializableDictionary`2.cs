// Decompiled with JetBrains decompiler
// Type: RTG.SerializableDictionary`2
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  [Serializable]
  public class SerializableDictionary<SerializedKeyType, SerializedValueType> : 
    ISerializationCallbackReceiver
  {
    private System.Collections.Generic.Dictionary<SerializedKeyType, SerializedValueType> _dictionary;
    [SerializeField]
    private List<SerializedKeyType> _serializedKeys;
    [SerializeField]
    private List<SerializedValueType> _serializedValues;

    public System.Collections.Generic.Dictionary<SerializedKeyType, SerializedValueType> Dictionary
    {
      get => this._dictionary;
    }

    public SerializedValueType this[SerializedKeyType index]
    {
      get => this._dictionary[index];
      set => this._dictionary[index] = value;
    }

    public void OnBeforeSerialize()
    {
      this.RemoveNullKeys();
      this._serializedKeys.Clear();
      this._serializedValues.Clear();
      foreach (KeyValuePair<SerializedKeyType, SerializedValueType> keyValuePair in this._dictionary)
      {
        this._serializedKeys.Add(keyValuePair.Key);
        this._serializedValues.Add(keyValuePair.Value);
      }
    }

    public void OnAfterDeserialize()
    {
      this._dictionary = new System.Collections.Generic.Dictionary<SerializedKeyType, SerializedValueType>();
      int num = Math.Min(this._serializedKeys.Count, this._serializedValues.Count);
      for (int index = 0; index < num; ++index)
        this._dictionary.Add(this._serializedKeys[index], this._serializedValues[index]);
      this._serializedKeys.Clear();
      this._serializedValues.Clear();
    }

    public void Clear() => this._dictionary.Clear();

    public void Add(SerializedKeyType key, SerializedValueType value)
    {
      this._dictionary.Add(key, value);
    }

    public bool ContainsKey(SerializedKeyType key) => this._dictionary.ContainsKey(key);

    public void Copy(
      SerializableDictionary<SerializedKeyType, SerializedValueType> other)
    {
      this.Clear();
      foreach (KeyValuePair<SerializedKeyType, SerializedValueType> keyValuePair in other.Dictionary)
        this._dictionary.Add(keyValuePair.Key, keyValuePair.Value);
    }

    public void RemoveNullKeys()
    {
      this._dictionary = this._dictionary.Where<KeyValuePair<SerializedKeyType, SerializedValueType>>((Func<KeyValuePair<SerializedKeyType, SerializedValueType>, bool>) (keyValuePair => !EqualityComparer<SerializedKeyType>.Default.Equals(keyValuePair.Key, default (SerializedKeyType)))).ToDictionary<KeyValuePair<SerializedKeyType, SerializedValueType>, SerializedKeyType, SerializedValueType>((Func<KeyValuePair<SerializedKeyType, SerializedValueType>, SerializedKeyType>) (keyValuePair => keyValuePair.Key), (Func<KeyValuePair<SerializedKeyType, SerializedValueType>, SerializedValueType>) (keyValuePair => keyValuePair.Value));
    }

    public SerializableDictionary()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._dictionary = new System.Collections.Generic.Dictionary<SerializedKeyType, SerializedValueType>();
      this._serializedKeys = new List<SerializedKeyType>();
      this._serializedValues = new List<SerializedValueType>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
