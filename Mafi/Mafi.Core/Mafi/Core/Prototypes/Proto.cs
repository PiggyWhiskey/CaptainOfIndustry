// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Prototypes.Proto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Mods;
using Mafi.Localization;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

#nullable disable
namespace Mafi.Core.Prototypes
{
  [DisableDirectCallSerialization]
  public abstract class Proto : 
    INotInitializedProto,
    IProtoInternalFriend,
    IComparable<Proto>,
    IEquatable<Proto>,
    IIsSafeAsHashKey,
    IProto
  {
    /// <summary>Common prefix for all phantom prototypes.</summary>
    protected const string PHANTOM_ID_PREFIX = "__PHANTOM__";
    private static readonly Lyst<Proto> s_phantoms;
    private static readonly Regex VALID_ID_REGEX;
    /// <summary>
    /// Whether this prototype is a phantom. Phantom is special instance that represents invalid proto and can be
    /// used un special situations to avoid nulls.
    /// </summary>
    /// <remarks>Each proto type should have at most one phantom instance.</remarks>
    public readonly bool IsPhantom;
    /// <summary>
    /// Whether this proto has been initialized. After initialization the proto must not be mutable anymore.
    /// </summary>
    public bool IsInitialized;
    private readonly Dict<Type, IProtoParam> m_params;
    private bool m_isNotAvailable;

    public static IIndexable<Proto> AllPhantoms => (IIndexable<Proto>) Proto.s_phantoms;

    /// <summary>
    /// This should be used to register all phantom protos right on their static creation.
    /// </summary>
    protected static T RegisterPhantom<T>(T proto) where T : Proto
    {
      Mafi.Assert.That<bool>(proto.IsPhantom).IsTrue("Registering non-phantom proto.");
      Mafi.Assert.That<Proto>(Proto.s_phantoms.Find((Predicate<Proto>) (x => x.Id == ((T) proto).Id))).IsNull<Proto>("Duplicate phantom registration.");
      Mafi.Assert.That<Proto>(Proto.s_phantoms.Find((Predicate<Proto>) (x => x.GetType() == ((T) proto).GetType()))).IsNull<Proto>("Duplicate phantom type.");
      Proto.s_phantoms.Add((Proto) proto);
      return proto;
    }

    /// <summary>Validates given proto ID.</summary>
    public static bool IsValidId(Proto.ID id)
    {
      return !string.IsNullOrWhiteSpace(id.Value) && Proto.VALID_ID_REGEX.Match(id.Value).Length == id.Value.Length;
    }

    /// <summary>
    /// Unique ID of this Prototype. To avoid name conflicts consider using some prefix specific to your mod.
    /// </summary>
    public Proto.ID Id { get; }

    public Proto.Str Strings { get; }

    public bool IsNotPhantom => !this.IsPhantom;

    /// <summary>Mod that (most likely) registered this Prototype.</summary>
    public IMod Mod { get; private set; }

    /// <summary>
    /// Tags of this proto. Adding new tags is allowed only during the game initialization phase.
    /// </summary>
    public HybridSet<Tag> Tags { get; private set; }

    /// <summary>
    /// Whether proto is not available and should be hidden in UI (research, menus, etc).
    /// </summary>
    public bool IsNotAvailable => this.m_isNotAvailable || this.IsObsolete;

    public virtual bool IsAvailable => !this.IsNotAvailable;

    public bool IsObsolete { get; private set; }

    protected Proto(Proto.ID id, Proto.Str strings, IEnumerable<Tag> tags = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: reference to a compiler-generated field
      this.\u003CTags\u003Ek__BackingField = HybridSet<Tag>.Empty;
      this.m_params = new Dict<Type, IProtoParam>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Id = Proto.IsValidId(id) ? id : throw new ArgumentException(string.Format("Invalid proto ID {0}.", (object) id));
      this.Strings = strings;
      this.IsPhantom = id.Value.StartsWith("__PHANTOM__");
      if (tags == null)
        return;
      this.AddTags(tags);
    }

    public Proto SetAvailability(bool isAvailable)
    {
      if (this.IsInitialized)
        Mafi.Log.Error(string.Format("Failed to set availability for proto {0}: Proto is already initialized.", (object) this.Id));
      else
        this.m_isNotAvailable = !isAvailable;
      return this;
    }

    protected void SetAvailabilityRuntime(bool isAvailable) => this.m_isNotAvailable = !isAvailable;

    public Proto MarkAsObsolete()
    {
      if (this.IsInitialized)
        Mafi.Log.Error(string.Format("Failed to mark proto obsolete {0}: Proto is already initialized.", (object) this.Id));
      else
        this.IsObsolete = true;
      return this;
    }

    public bool HasTag(Tag tag) => this.Tags.Contains(tag);

    protected void AddTags(params Tag[] tags)
    {
      if (this.IsInitialized)
        return;
      foreach (Tag tag in tags)
      {
        if (!tag.TargetType.IsAssignableFrom(this.GetType()))
          throw new ProtoBuilderException(string.Format("Tag '{0}' with target type '{1}' cannot be assigned to proto with type '{2}'", (object) tag.Id, (object) tag.TargetType.Name, (object) this.GetType()));
        if (this.Tags.Contains(tag))
          Mafi.Log.Warning(string.Format("Adding duplicate tag '{0}' to proto '{1}'.", (object) tag, (object) this.Id));
      }
      this.Tags = this.Tags.Add(tags);
    }

    protected void AddTags(IEnumerable<Tag> tags) => this.AddTags(tags.ToArray<Tag>());

    void INotInitializedProto.AddTags(params Tag[] tags) => this.AddTags(tags);

    void INotInitializedProto.AddTags(IEnumerable<Tag> tags) => this.AddTags(tags.ToArray<Tag>());

    void IProtoInternalFriend.SetMod(IMod mod) => this.Mod = mod.CheckNotNull<IMod>();

    /// <summary>
    /// One-time initialization that is called once all protos are registered int the DB.
    /// </summary>
    void IProtoInternalFriend.InitializeInternal(ProtosDb protosDb)
    {
      int num = this.IsInitialized ? 1 : 0;
      Mafi.Assert.That<IMod>(this.Mod).IsNotNull<IMod>("Mod not set!");
      this.OnInitialize(protosDb);
      this.IsInitialized = true;
    }

    /// <summary>
    /// Throws <see cref="T:Mafi.Core.Prototypes.ProtoInitException" /> if this proto was already initialized. This should be used in any
    /// method that mutates the proto.
    /// </summary>
    protected void ThrowIfInitialized()
    {
      if (this.IsInitialized)
        throw new ProtoInitException(string.Format("Proto '{0}' is already initialized and cannot be mutated.", (object) this.Id));
    }

    /// <summary>
    /// Throws <see cref="T:Mafi.Core.Prototypes.ProtoInitException" /> if this proto was not initialized. This should be used in any method
    /// uses data that may be invalid before initialization.
    /// </summary>
    protected void ThrowIfNotInitialized()
    {
      if (!this.IsInitialized)
        throw new ProtoInitException(string.Format("Proto '{0}' is not yet initialized and cannot be read.", (object) this.Id));
    }

    /// <summary>
    /// Initialization hook for derived classes. Called once all protos are registered int the DB.
    /// </summary>
    protected virtual void OnInitialize(ProtosDb protosDb)
    {
    }

    public int CompareTo(Proto other) => this.Id.CompareTo(other.Id);

    public override string ToString()
    {
      return string.Format("{0} ({1})", (object) this.Id, (object) this.GetType().Name);
    }

    public bool Equals(Proto other)
    {
      if ((object) other == null)
        return false;
      if ((object) this == (object) other)
        return true;
      if (!this.Id.Equals(other.Id))
        return false;
      Mafi.Log.Error(string.Format("Protos that are not equal references have equal IDs: {0}", (object) this.Id));
      return true;
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      if ((object) this == obj)
        return true;
      Proto proto = obj as Proto;
      if ((object) proto == null || !this.Id.Equals(proto.Id))
        return false;
      Mafi.Log.Error(string.Format("Protos that are not equal references have equal IDs: {0}", (object) this.Id));
      return true;
    }

    public static bool operator ==(Proto p1, Proto p2)
    {
      return (object) p1 == null ? (object) p2 == null : p1.Equals(p2);
    }

    public static bool operator !=(Proto p1, Proto p2) => !(p1 == p2);

    public override int GetHashCode() => this.Id.GetHashCode();

    /// <summary>
    /// Adds (ore replaces) an arbitrary parameter to the proto. This can be used to add special values to proto
    /// without using inheritance. Parameters can be only added during protos construction phase.
    /// </summary>
    public void AddOrReplaceParam(IProtoParam param)
    {
      if (this.IsInitialized)
        throw new ProtoInitException(string.Format("Cannot add parameter '{0}' to {1}. Proto was already initialized.", (object) param.GetType(), (object) this));
      this.m_params[param.GetType()] = param;
    }

    public Proto AddParam(IProtoParam param)
    {
      if (this.m_params.ContainsKey(param.GetType()))
        throw new ProtoBuilderException(string.Format("Parameter {0} was already set on {1}. ", (object) param.GetType(), (object) this) + "Use AddOrReplaceParam to replace it.");
      if (!param.AllowedProtoType.IsAssignableTo<Proto>())
        throw new ProtoBuilderException(string.Format("Parameter {0} has invalid `AllowedProtoType` ", (object) param.GetType()) + string.Format("{0} (must derive from `{1}`).", (object) param.AllowedProtoType, (object) typeof (Proto)));
      if (!this.GetType().IsAssignableTo(param.AllowedProtoType))
        throw new ProtoBuilderException(string.Format("Parameter {0} cannot be assigned to type `{1}`. ", (object) this.GetType(), (object) param.AllowedProtoType) + string.Format("Only derived types of `{0}` are allowed.", (object) param.AllowedProtoType));
      this.AddOrReplaceParam(param);
      return this;
    }

    public bool TryGetParam<T>(out T paramValue) where T : class
    {
      IProtoParam protoParam;
      if (this.m_params.TryGetValue(typeof (T), out protoParam))
      {
        paramValue = (T) protoParam;
        return true;
      }
      paramValue = default (T);
      return false;
    }

    public Option<T> GetParam<T>() where T : class
    {
      IProtoParam protoParam;
      return this.m_params.TryGetValue(typeof (T), out protoParam) ? (Option<T>) (T) protoParam : Option<T>.None;
    }

    public bool HasParam<T>() where T : class
    {
      return this.m_params.TryGetValue(typeof (T), out IProtoParam _);
    }

    /// <summary>
    /// Creates proto str and registers name and description using <see cref="M:Mafi.Localization.Loc.Str(System.String,System.String,System.String)" />.
    /// </summary>
    public static Proto.Str CreateStr(
      Proto.ID id,
      string name,
      string descShort = null,
      string translationComment = null)
    {
      translationComment = !string.IsNullOrWhiteSpace(translationComment) ? ": " + translationComment : "";
      return new Proto.Str(Loc.Str(id.Value + "__name", name, nameof (name) + translationComment), descShort != null ? Loc.Str(id.Value + "__desc", descShort, "short description" + translationComment) : LocStr.Empty);
    }

    public static Proto.Str CreateStrHidden(Proto.ID id)
    {
      return Proto.CreateStr(id, id.Value, translationComment: "HIDE");
    }

    /// <summary>
    /// Creates proto str and registers the given name but the description is not registered since it is already
    /// localized.
    /// </summary>
    public static Proto.Str CreateStr(
      Proto.ID id,
      string name,
      LocStr descShort,
      string translationComment = null)
    {
      translationComment = !string.IsNullOrWhiteSpace(translationComment) ? ": " + translationComment : "";
      return new Proto.Str(Loc.Str(id.Value + "__name", name, nameof (name) + translationComment), descShort);
    }

    /// <summary>
    /// Creates proto str with already formatted description. The formatted description is registered using
    /// <see cref="M:Mafi.Localization.LocalizationManager.CreateAlreadyLocalizedFormatted(System.String,Mafi.Localization.LocStrFormatted)" />.
    /// </summary>
    public static Proto.Str CreateStrFromLocalized(
      Proto.ID id,
      string name,
      LocStrFormatted descShortFormatted,
      string nameTrComment = null)
    {
      nameTrComment = !string.IsNullOrWhiteSpace(nameTrComment) ? ": " + nameTrComment : "";
      return new Proto.Str(Loc.Str(id.Value + "__name", name, nameof (name) + nameTrComment), LocalizationManager.CreateAlreadyLocalizedFormatted(id.Value + "__desc", descShortFormatted));
    }

    /// <summary>
    /// Creates proto str with already formatted name and description (both are registered using
    /// <see cref="M:Mafi.Localization.LocalizationManager.CreateAlreadyLocalizedFormatted(System.String,Mafi.Localization.LocStrFormatted)" />).
    /// </summary>
    public static Proto.Str CreateStrFromLocalized(
      Proto.ID id,
      LocStrFormatted nameFormatted,
      LocStrFormatted descShortFormatted)
    {
      return new Proto.Str(LocalizationManager.CreateAlreadyLocalizedFormatted(id.Value + "__name", nameFormatted), LocalizationManager.CreateAlreadyLocalizedFormatted(id.Value + "__desc", descShortFormatted));
    }

    /// <summary>
    /// Creates proto str with already formatted name, which is registered using
    /// <see cref="M:Mafi.Localization.LocalizationManager.CreateAlreadyLocalizedFormatted(System.String,Mafi.Localization.LocStrFormatted)" />. Description is just passed though.
    /// </summary>
    public static Proto.Str CreateStrFromLocalized(
      Proto.ID id,
      LocStrFormatted nameFormatted,
      LocStr descShort)
    {
      return new Proto.Str(LocalizationManager.CreateAlreadyLocalizedFormatted(id.Value + "__name", nameFormatted), descShort);
    }

    /// <summary>
    /// Creates proto str with and formats description with given argument. The raw description is registered with
    /// formatted description is registered using <see cref="M:Mafi.Localization.LocalizationManager.CreateAlreadyLocalizedFormatted(System.String,Mafi.Localization.LocStrFormatted)" />.
    /// </summary>
    public static Proto.Str CreateStrFormatDesc1(
      Proto.ID id,
      string name,
      string descShort,
      LocStrFormatted descFormatArg0,
      string descTrComment)
    {
      LocStr1 locStr1 = Loc.Str1(id.Value + "__desc", descShort, "short description: " + descTrComment);
      return new Proto.Str(Loc.Str(id.Value + "__name", name, nameof (name)), LocalizationManager.CreateAlreadyLocalizedFormatted(locStr1.Id, locStr1.Format(descFormatArg0)));
    }

    static Proto()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      Proto.s_phantoms = new Lyst<Proto>();
      Proto.VALID_ID_REGEX = new Regex("[a-zA-Z_][a-zA-Z0-9_]*", RegexOptions.Compiled);
    }

    [ManuallyWrittenSerialization]
    [DebuggerDisplay("{Value,nq}")]
    [DebuggerStepThrough]
    public readonly struct ID : IEquatable<Proto.ID>, IComparable<Proto.ID>
    {
      /// <summary>Underlying string value of this Id.</summary>
      public readonly string Value;

      public ID(string value)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.Value = value;
      }

      public static bool operator ==(Proto.ID lhs, Proto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(Proto.ID lhs, Proto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public override bool Equals(object other) => other is Proto.ID other1 && this.Equals(other1);

      public bool Equals(Proto.ID other)
      {
        return string.Equals(this.Value, other.Value, StringComparison.Ordinal);
      }

      public int CompareTo(Proto.ID other) => string.CompareOrdinal(this.Value, other.Value);

      public override string ToString() => this.Value ?? string.Empty;

      public override int GetHashCode()
      {
        string str = this.Value;
        return str == null ? 0 : str.GetHashCode();
      }

      public static void Serialize(Proto.ID value, BlobWriter writer)
      {
        writer.WriteString(value.Value);
      }

      public static Proto.ID Deserialize(BlobReader reader) => new Proto.ID(reader.ReadString());
    }

    public struct Str
    {
      public static readonly Proto.Str Empty;
      /// <summary>Player-friendly name.</summary>
      public readonly LocStr Name;
      /// <summary>Short description of the proto.</summary>
      public readonly LocStr DescShort;

      public Str(LocStr name)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.Name = name;
        this.DescShort = LocStr.Empty;
      }

      public Str(LocStr name, LocStr descShort)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.Name = name;
        this.DescShort = descShort;
      }

      static Str()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        Proto.Str.Empty = new Proto.Str(LocStr.Empty);
      }
    }

    /// <summary>
    /// Base class for all graphics information that is NOT used in game simulation.
    /// </summary>
    public abstract class Gfx
    {
      /// <summary>Path in empty graphics instance.</summary>
      public const string EMPTY_PATH = "EMPTY";
      public const string GENERATED_ICON_PATH_PREFIX = "Assets/Unity/Generated/Icons";
      public const string GENERATED_ANIMATION_PATH_PREFIX = "Assets/Unity/Generated/Animations";

      protected Gfx()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }
  }
}
