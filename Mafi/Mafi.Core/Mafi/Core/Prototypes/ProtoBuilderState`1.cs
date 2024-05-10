// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Prototypes.ProtoBuilderState`1
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Localization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Prototypes
{
  public abstract class ProtoBuilderState<TState> where TState : ProtoBuilderState<TState>
  {
    public readonly IProtoBuilder Builder;
    protected readonly Proto.ID Id;
    protected readonly LocStr Name;
    protected LocStr DescShort;
    protected bool LockedOnInit;
    private readonly Lyst<Tag> m_tags;

    protected Proto.Str Strings => new Proto.Str(this.Name, this.DescShort);

    protected ProtoBuilderState(
      IProtoBuilder builder,
      Proto.ID id,
      string name,
      string translationComment)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.DescShort = LocStr.Empty;
      this.m_tags = new Lyst<Tag>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Builder = builder.CheckNotNull<IProtoBuilder>();
      this.Id = id;
      this.Name = Loc.Str(id.Value + "__name", name, translationComment);
    }

    [MustUseReturnValue]
    public TState AddTags(Tag tags)
    {
      this.m_tags.Add(tags);
      return (TState) this;
    }

    [MustUseReturnValue]
    public TState AddTags(params Tag[] tags)
    {
      this.m_tags.AddRange(tags);
      return (TState) this;
    }

    [MustUseReturnValue]
    public virtual TState Description(string description, string explanation = "short description")
    {
      this.DescShort = Loc.Str(this.Id.Value + "__desc", description, explanation);
      return (TState) this;
    }

    [MustUseReturnValue]
    public virtual TState Description(LocStr desc)
    {
      this.DescShort = desc;
      return (TState) this;
    }

    [MustUseReturnValue]
    public TState SetAsLockedOnInit()
    {
      this.LockedOnInit = true;
      return (TState) this;
    }

    protected TProto AddToDb<TProto>(TProto proto) where TProto : Proto
    {
      ((INotInitializedProto) proto).AddTags((IEnumerable<Tag>) this.m_tags);
      this.Builder.ProtosDb.Add<TProto>(proto, this.LockedOnInit);
      return proto;
    }

    protected T ValueOrThrow<T>(T value, string name, Predicate<T> isValid) where T : struct
    {
      return isValid(value) ? value : throw new ProtoBuilderException(string.Format("Value '{0}' of proto '{1}' is invalid.", (object) name, (object) this.Id));
    }

    protected T ValueOrThrow<T>(T? value, string name) where T : struct
    {
      return value.HasValue ? value.Value : throw new ProtoBuilderException(string.Format("Value '{0}' of proto '{1}' was not set.", (object) name, (object) this.Id));
    }

    protected T ValueOrThrow<T>(Option<T> value, string name) where T : class
    {
      return value.HasValue ? value.Value : throw new ProtoBuilderException(string.Format("Value '{0}' of proto '{1}' was not set.", (object) name, (object) this.Id));
    }

    protected string ValueOrThrow(string value, string name)
    {
      return !string.IsNullOrEmpty(value) ? value : throw new ProtoBuilderException(string.Format("Value '{0}' of proto '{1}' was not set or is empty.", (object) name, (object) this.Id));
    }

    protected ImmutableArray<T> NotEmptyOrThrow<T>(ImmutableArray<T> value, string name)
    {
      if (value.IsNotValid || value.IsEmpty)
        throw new ProtoBuilderException(string.Format("Value '{0}' of proto '{1}' was not set or is empty.", (object) name, (object) this.Id));
      return value;
    }
  }
}
