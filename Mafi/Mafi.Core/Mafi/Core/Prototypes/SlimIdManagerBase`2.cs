// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Prototypes.SlimIdManagerBase`2
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.Prototypes
{
  /// <summary>
  /// Handles conversion from and to SlimIDs and ensures that their order is persistent between saves.
  /// Derived class should be serializable and sealed.
  /// </summary>
  /// <remarks>
  /// To properly implement slim ID on a proto, add following code to the proto class:
  /// <code>
  /// public XxxSlimId SlimId {
  /// 	get {
  /// 		Assert.That(m_slimId.IsPhantom).IsEqualTo_DebugOnly(IsPhantom, "Getting SlimID before it was set.");
  /// 		return m_slimId;
  /// 	}
  /// }
  /// private XxxSlimId m_slimId;
  /// 
  /// void IProtoWithSlimID{XxxSlimId}.SetSlimId(XxxSlimId id) {
  /// 	if (m_slimId.Value != 0 and m_slimId != id) {
  /// 		throw new InvalidOperationException($"Slim ID of '{this}' was already set to '{m_slimId}'.");
  /// 	}
  /// 
  /// 	m_slimId = id;
  /// }
  /// </code>
  /// </remarks>
  public abstract class SlimIdManagerBase<TProto, TSlimId>
    where TProto : Proto, IProtoWithSlimID<TSlimId>
    where TSlimId : struct
  {
    /// <summary>
    /// All managed protos. Proto at index <c>i</c> has slim id equal to <c>i</c>.
    /// </summary>
    public readonly ImmutableArray<TProto> ManagedProtos;

    /// <summary>Return phantom proto.</summary>
    /// <remarks>IMPORTANT: Implement this as a constant. This is called BEFORE ctor of the derived class.</remarks>
    public abstract TProto PhantomProto { get; }

    /// <summary>Maximum valid ID (zero-based).</summary>
    /// <remarks>IMPORTANT: Implement this as a constant. This is called BEFORE ctor of the derived class.</remarks>
    public abstract int MaxIdValue { get; }

    protected SlimIdManagerBase(ProtosDb db)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      Assert.That<bool>(db.IsReadonly).IsTrue();
      Lyst<TProto> lyst = new Lyst<TProto>()
      {
        this.PhantomProto
      };
      lyst.AddRange((IEnumerable<TProto>) db.All<TProto>().OrderBy<TProto, string>((Func<TProto, string>) (x => x.Id.Value), (IComparer<string>) StringComparer.Ordinal));
      if (lyst.Count > this.MaxIdValue + 1)
        throw new ProtoInitException(string.Format("Too many {0}. Only {1} supported.", (object) typeof (TProto).Name, (object) (this.MaxIdValue + 1)));
      if (lyst.Count > this.MaxIdValue - this.MaxIdValue.CeilDiv(10))
        Log.Warning(string.Format("There are currently {0} instances of '{1}' and ", (object) lyst.Count, (object) typeof (TProto).Name) + string.Format("it is getting close to limit of {0}.", (object) this.MaxIdValue));
      this.ManagedProtos = lyst.ToImmutableArrayAndClear();
      for (int index = 0; index < this.ManagedProtos.Length; ++index)
      {
        this.ManagedProtos[index].SetSlimId(this.CreateSlimId(index));
        if (index > 0)
          Assert.That<TProto>(this.ManagedProtos[index]).IsNotNullOrPhantom<TProto>();
      }
    }

    [InitAfterLoad(InitPriority.Highest)]
    private void initAfterLoad(DependencyResolver resolver)
    {
      Set<TProto> set = resolver.Resolve<ProtosDb>().All<TProto>().ToSet<TProto>();
      // ISSUE: variable of a boxed type
      __Boxed<TProto> local = (object) this.ManagedProtos.FirstOrDefault();
      Assert.That<bool>(local != null && local.IsPhantom).IsTrue();
      ImmutableArray<TProto> immutableArray = this.ManagedProtos;
      for (int index = 1; index < immutableArray.Length; ++index)
      {
        TProto proto = immutableArray[index];
        if (!proto.IsPhantom && !set.Remove(proto))
        {
          immutableArray = immutableArray.SetItem(index, this.PhantomProto);
          Log.Warning(string.Format("Missing proto detected ({0}), replacing with phantom.", (object) proto));
        }
      }
      if (set.IsNotEmpty)
      {
        TProto[] array = set.OrderBy<TProto, string>((Func<TProto, string>) (x => x.Id.Value), (IComparer<string>) StringComparer.Ordinal).ToArray<TProto>();
        int length = immutableArray.Length;
        if (length + array.Length > this.MaxIdValue)
          throw new ProtoInitException(string.Format("Too many {0}. Only {1} supported.", (object) typeof (TProto).Name, (object) (this.MaxIdValue + 1)));
        ImmutableArrayBuilder<TProto> immutableArrayBuilder = new ImmutableArrayBuilder<TProto>(length + array.Length);
        for (int index = 0; index < length; ++index)
          immutableArrayBuilder[index] = immutableArray[index];
        for (int index = 0; index < array.Length; ++index)
        {
          Assert.That<TProto>(array[index]).IsNotNullOrPhantom<TProto>();
          immutableArrayBuilder[length + index] = array[index];
        }
        immutableArray = immutableArrayBuilder.GetImmutableArrayAndClear();
        Log.Info(string.Format("New {0} protos ({1}) after load in {2}: ", (object) array.Length, (object) typeof (TProto).Name, (object) this.GetType().Name) + ((IEnumerable<TProto>) array).Select<TProto, string>((Func<TProto, string>) (x => x.Id.Value)).JoinStrings(", "));
      }
      if (immutableArray != this.ManagedProtos)
        ReflectionUtils.SetField<SlimIdManagerBase<TProto, TSlimId>>(this, "ManagedProtos", (object) immutableArray);
      for (int index = 1; index < immutableArray.Length; ++index)
      {
        TProto proto = immutableArray[index];
        if (!proto.IsPhantom)
          proto.SetSlimId(this.CreateSlimId(index));
      }
    }

    /// <summary>Creates a slim ID from given index.</summary>
    /// <remarks>IMPORTANT: This is getting called BEFORE ctor of the derived class is called.</remarks>
    protected abstract TSlimId CreateSlimId(int index);

    protected abstract int GetIndex(TSlimId slimId);

    public TProto ResolveOrPhantom(TSlimId slimId)
    {
      int index = this.GetIndex(slimId);
      if ((long) (uint) index < (long) this.ManagedProtos.Length)
        return this.ManagedProtos[index];
      Log.Error(string.Format("Resolving invalid slim ID {0} of {1}, returning phantom.", (object) slimId, (object) typeof (TProto).Name));
      return this.PhantomProto;
    }

    public bool TryResolve(TSlimId slimId, out TProto proto)
    {
      int index = this.GetIndex(slimId);
      if ((long) (uint) index < (long) this.ManagedProtos.Length)
      {
        proto = this.ManagedProtos[index];
        return true;
      }
      proto = default (TProto);
      return false;
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      ImmutableArray<TProto>.Serialize(this.ManagedProtos, writer);
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<SlimIdManagerBase<TProto, TSlimId>>(this, "ManagedProtos", (object) ImmutableArray<TProto>.Deserialize(reader));
      reader.RegisterInitAfterLoad<SlimIdManagerBase<TProto, TSlimId>>(this, "initAfterLoad", InitPriority.Highest);
    }
  }
}
