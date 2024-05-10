// Decompiled with JetBrains decompiler
// Type: Mafi.Core.PropertiesDb.Property`1
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;

#nullable disable
namespace Mafi.Core.PropertiesDb
{
  [GenerateSerializer(false, null, 0)]
  public abstract class Property<T> : IProperty<T>, IProperty where T : IEquatable<T>
  {
    /// <summary>
    /// Special group that allows to apply modifier to the base value using CombineModifiersInGroup
    /// instead of via ApplyModifier. This for instance allows to redefine the initial value of Percent
    /// set to zero where multiplication wouldn't work.
    /// </summary>
    public static string BASE_GROUP;
    private static readonly ThreadLocal<Dict<string, T>> SUMS_CACHE;
    private readonly Event<T> m_onChange;
    private readonly T m_baseValue;
    private readonly Lyst<PropertyModifier<T>> m_modifiers;
    [NewInSaveVersion(140, null, "new()", null, null)]
    private readonly Lyst<Type> m_entitiesToUpdate;
    [NewInSaveVersion(140, null, null, null, null)]
    private readonly IPropertiesDbInternal m_propertiesDb;

    public string Id { get; private set; }

    public IEvent<T> OnChange => (IEvent<T>) this.m_onChange;

    public T Value { get; private set; }

    protected Property(string id, T value, IPropertiesDbInternal propertiesDb)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_onChange = new Event<T>();
      this.m_modifiers = new Lyst<PropertyModifier<T>>();
      this.m_entitiesToUpdate = new Lyst<Type>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Id = id;
      this.Value = value;
      this.m_baseValue = value;
      this.m_propertiesDb = propertiesDb;
    }

    [OnlyForSaveCompatibility(null)]
    [InitAfterLoad(InitPriority.Normal)]
    private void initDeps(int saveVersion, DependencyResolver resolver)
    {
      if (saveVersion >= 140)
        return;
      ReflectionUtils.SetField<Property<T>>(this, "m_propertiesDb", (object) (IPropertiesDbInternal) resolver.Resolve<IPropertiesDb>());
    }

    [InitAfterLoad(InitPriority.Lowest)]
    private void initSelf()
    {
      T other = this.calculateValue(Option<StringBuilder>.None);
      if (this.Value.Equals(other))
        return;
      Log.Error("Immutable modifier '" + this.Id + "' has changed?");
      this.Value = other;
      this.onChange(this.Value);
    }

    public void OverrideValue(T value)
    {
      if (typeof (T) != typeof (bool))
      {
        Log.Error("Cannot override non boolean type");
      }
      else
      {
        this.Value = value;
        this.onChange(this.Value);
      }
    }

    public void AddModifier(PropertyModifier<T> modifier)
    {
      if (this.m_modifiers.Any<PropertyModifier<T>>((Predicate<PropertyModifier<T>>) (x => x.Owner == modifier.Owner)))
      {
        Assert.Fail("Transformation of " + this.Id + " from " + modifier.Owner + " already exists!");
      }
      else
      {
        this.m_modifiers.Add(modifier);
        this.updateValue();
      }
    }

    public void AddOrSetModifier(string owner, T value, Option<string> group)
    {
      PropertyModifier<T> propertyModifier = this.m_modifiers.FirstOrDefault<PropertyModifier<T>>((Predicate<PropertyModifier<T>>) (x => x.Owner == owner));
      if (propertyModifier != null)
      {
        if (propertyModifier.Value.Equals(value) && propertyModifier.Group == group)
          return;
        this.m_modifiers.Remove(propertyModifier);
      }
      this.m_modifiers.Add(new PropertyModifier<T>(value, owner, group));
      this.updateValue();
    }

    public bool TryGetModifier(string owner, out PropertyModifier<T> modifier)
    {
      modifier = this.m_modifiers.FirstOrDefault<PropertyModifier<T>>((Predicate<PropertyModifier<T>>) (x => x.Owner == owner));
      return modifier != null;
    }

    public void RemoveModifier(PropertyModifier<T> modifier)
    {
      if (!this.m_modifiers.Remove(modifier))
        Assert.Fail("Transformation of " + this.Id + " from " + modifier.Owner + " not found!");
      else
        this.updateValue();
    }

    private void updateValue(bool doNotNotify = false)
    {
      T obj1 = this.Value;
      this.Value = this.calculateValue((Option<StringBuilder>) Option.None);
      if (doNotNotify)
        return;
      ref T local = ref obj1;
      if ((object) default (T) == null)
      {
        T obj2 = local;
        local = ref obj2;
      }
      T other = this.Value;
      if (local.Equals(other))
        return;
      this.onChange(this.Value);
    }

    private T calculateValue(Option<StringBuilder> sb)
    {
      T obj1 = this.m_baseValue;
      if (sb.HasValue)
        sb.Value.AppendLine(string.Format("{0}: {1} (initial: {2})", (object) this.Id, (object) this.Value, (object) this.m_baseValue));
      Option<string> group;
      foreach (PropertyModifier<T> modifier in this.m_modifiers)
      {
        group = modifier.Group;
        if (group.ValueOrNull == Property<T>.BASE_GROUP)
        {
          T obj2 = obj1;
          obj1 = this.CombineModifiersInGroup(obj1, modifier.Value);
          if (sb.HasValue)
            sb.Value.AppendLine(string.Format(" |- {0}: {1}: {2} -> {3}", (object) Property<T>.BASE_GROUP, (object) modifier.Owner, (object) obj2, (object) obj1));
        }
      }
      Dict<string, T> dict = Property<T>.SUMS_CACHE.Value;
      dict.Clear();
      foreach (PropertyModifier<T> modifier in this.m_modifiers)
      {
        group = modifier.Group;
        if (!(group.ValueOrNull == Property<T>.BASE_GROUP))
        {
          group = modifier.Group;
          if (group.HasValue)
          {
            string key = modifier.Group.Value;
            T first;
            dict[key] = !dict.TryGetValue(key, out first) ? modifier.Value : this.CombineModifiersInGroup(first, modifier.Value);
          }
          else
          {
            T obj3 = obj1;
            obj1 = this.ApplyModifier(obj1, modifier.Value);
            if (sb.HasValue)
              sb.Value.AppendLine(string.Format(" |- {0}: {1} -> {2}", (object) modifier.Owner, (object) obj3, (object) obj1));
          }
        }
      }
      foreach (KeyValuePair<string, T> keyValuePair in dict)
      {
        T obj4 = obj1;
        obj1 = this.ApplyModifier(obj1, keyValuePair.Value);
        if (sb.HasValue)
          sb.Value.AppendLine(string.Format(" |- {0}: {1} -> {2} [group sum: {3}]", (object) keyValuePair.Key, (object) obj4, (object) obj1, (object) keyValuePair.Value));
      }
      return obj1;
    }

    public void RegisterEntityTypeForUpdate(Type type)
    {
      if (this.m_entitiesToUpdate.Contains(type))
        return;
      foreach (Type type1 in this.m_entitiesToUpdate)
      {
        if (type1.IsAssignableTo(type))
          return;
      }
      this.m_entitiesToUpdate.Add(type);
    }

    private void onChange(T newVal)
    {
      this.m_onChange.Invoke(newVal);
      this.m_propertiesDb.OnPropertyChanged((IProperty) this, this.m_entitiesToUpdate.ToImmutableArray());
    }

    protected abstract T ApplyModifier(T baseValue, T modifier);

    protected abstract T CombineModifiersInGroup(T first, T second);

    /// <summary>
    /// Throws exceptions if parsing fails! Only use in debug/testing code!
    /// </summary>
    public void SetValueFromString(string valueString)
    {
      object obj;
      if (typeof (T).IsAssignableTo(typeof (Percent)))
        obj = (object) Percent.FromPercentVal(int.Parse(valueString.TrimEnd('%')));
      else
        obj = !typeof (T).IsAssignableTo(typeof (bool)) ? (object) TypeDescriptor.GetConverter(typeof (T)) : (object) bool.Parse(valueString.Trim());
      this.Value = (T) obj;
      this.onChange(this.Value);
    }

    public void DumpInfo(StringBuilder sb) => this.calculateValue((Option<StringBuilder>) sb);

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteString(this.Id);
      writer.WriteGeneric<T>(this.m_baseValue);
      Lyst<Type>.Serialize(this.m_entitiesToUpdate, writer);
      Lyst<PropertyModifier<T>>.Serialize(this.m_modifiers, writer);
      Event<T>.Serialize(this.m_onChange, writer);
      writer.WriteGeneric<IPropertiesDbInternal>(this.m_propertiesDb);
      writer.WriteGeneric<T>(this.Value);
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.Id = reader.ReadString();
      reader.SetField<Property<T>>(this, "m_baseValue", (object) reader.ReadGenericAs<T>());
      reader.SetField<Property<T>>(this, "m_entitiesToUpdate", reader.LoadedSaveVersion >= 140 ? (object) Lyst<Type>.Deserialize(reader) : (object) new Lyst<Type>());
      reader.SetField<Property<T>>(this, "m_modifiers", (object) Lyst<PropertyModifier<T>>.Deserialize(reader));
      reader.SetField<Property<T>>(this, "m_onChange", (object) Event<T>.Deserialize(reader));
      reader.SetField<Property<T>>(this, "m_propertiesDb", reader.LoadedSaveVersion >= 140 ? (object) reader.ReadGenericAs<IPropertiesDbInternal>() : (object) (IPropertiesDbInternal) null);
      this.Value = reader.ReadGenericAs<T>();
      reader.RegisterInitAfterLoad<Property<T>>(this, "initDeps", InitPriority.Normal);
      reader.RegisterInitAfterLoad<Property<T>>(this, "initSelf", InitPriority.Lowest);
    }

    static Property()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      Property<T>.BASE_GROUP = "BaseGroup";
      Property<T>.SUMS_CACHE = new ThreadLocal<Dict<string, T>>((Func<Dict<string, T>>) (() => new Dict<string, T>()));
    }
  }
}
