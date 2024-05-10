// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Research.ResearchNodeProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Products;
using Mafi.Core.PropertiesDb;
using Mafi.Core.Prototypes;
using Mafi.Core.UnlockingTree;
using Mafi.Localization;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Mafi.Core.Research
{
  /// <summary>
  /// Represents a single item of research tree that can be unlocked by the player.
  /// 
  /// One ResearchProto consists of multiple <see cref="T:Mafi.Core.UnlockingTree.IUnlockNodeUnit" /> that provide the individual research
  /// improvements.
  /// 
  /// For instance node proto called Nuclear energy can have following atomic protos:
  /// - Unlock: Nuclear reactor
  /// - Unlock: Nuclear waste truck
  /// - Permanent bonus: +5% company rating
  /// 
  /// And following required products:
  /// - 1 tons of Uranium per step
  /// - 1 Advanced Turbines per step
  /// - 10 Coolers per step
  /// </summary>
  [DebuggerDisplay("Research Node: {Id}")]
  public class ResearchNodeProto : Proto, IProtoWithPropertiesUpdate, IProto
  {
    private static readonly int DIFF_MULT;
    private static readonly int DIFF_MULT_DELAYED;
    private static readonly int DIFF_MULT_STEP_DELAY;
    /// <summary>
    /// Research units of this node. These actually bring the effects of the research.
    /// </summary>
    public readonly ImmutableArray<IUnlockNodeUnit> Units;
    /// <summary>
    /// Conditions that need to be satisfied in order for this node to became available for research.
    /// State of this is not serialized thus conditions can be added or removed anytime.
    /// </summary>
    public readonly ImmutableArray<IResearchNodeUnlockingCondition> UnlockingConditions;
    private readonly int m_difficultyBase;
    /// <summary>
    /// Grid position of this node in the research tree. TODO: More documentation.
    /// </summary>
    public Vector2i GridPosition;
    /// <summary>
    /// If true, only one parent needs to be unlocked to make this node available.
    /// </summary>
    public readonly bool AnyParentCanUnlock;
    public readonly ResearchNodeProto.Gfx Graphics;
    public readonly LocStr ResolvedDescription;
    private readonly Lyst<ResearchNodeProto> m_parents;

    public ResearchNodeProto.ID Id => new ResearchNodeProto.ID(base.Id.Value);

    /// <summary>
    /// Converts difficulty to number of steps. Steps grow exponentially with difficulty. This makes defining
    /// research easier.
    /// </summary>
    public static int DifficultyToSteps(int difficulty)
    {
      Mafi.Assert.That<int>(difficulty).IsNotNegative();
      if (difficulty <= 0)
        return 0;
      return difficulty == 1 ? ResearchNodeProto.DIFF_MULT : difficulty * ResearchNodeProto.DIFF_MULT + 0.Max(difficulty - ResearchNodeProto.DIFF_MULT_STEP_DELAY) * ResearchNodeProto.DIFF_MULT_DELAYED;
    }

    /// <summary>Whether this node should be unlocked on game start.</summary>
    public bool IsUnlockedFromStart => this.TotalStepsRequired == 0;

    /// <summary>
    /// Parents of this in the research tree. All parents must be unlocked, to be able to research this.
    /// </summary>
    public ImmutableArray<ResearchNodeProto> Parents { get; private set; }

    public int Difficulty { get; private set; }

    /// <summary>
    /// Total number of step that must be done to unlock the research.
    /// 
    /// IMPORTANT: Do not cache. Can change during the runtime.
    /// </summary>
    public int TotalStepsRequired { get; private set; }

    public ResearchNodeProto(
      ResearchNodeProto.ID id,
      Proto.Str strings,
      ImmutableArray<ResearchNodeProto> parents,
      ImmutableArray<IUnlockNodeUnit> units,
      ImmutableArray<IResearchNodeUnlockingCondition> unlockingConditions,
      int difficulty,
      Vector2i gridPosition,
      bool anyParentCanUnlock,
      ResearchNodeProto.Gfx graphics)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_parents = new Lyst<ResearchNodeProto>();
      // ISSUE: explicit constructor call
      base.\u002Ector((Proto.ID) id, strings);
      this.m_parents.AddRange(parents);
      this.UnlockingConditions = unlockingConditions.CheckNotDefaultStruct<ImmutableArray<IResearchNodeUnlockingCondition>>();
      this.Units = units.CheckNotDefaultStruct<ImmutableArray<IUnlockNodeUnit>>();
      this.TotalStepsRequired = ResearchNodeProto.DifficultyToSteps(difficulty).CheckNotNegative();
      this.Difficulty = this.m_difficultyBase = difficulty;
      this.GridPosition = gridPosition;
      this.AnyParentCanUnlock = anyParentCanUnlock;
      this.Graphics = graphics;
      this.ResolvedDescription = strings.DescShort;
      if (this.ResolvedDescription.TranslatedString.IsEmpty())
      {
        Lyst<IProto> indexable = new Lyst<IProto>();
        foreach (IUnlockNodeUnit unit in this.Units)
        {
          if (unit is ProtoUnlock protoUnlock)
          {
            IEnumerable<IProto> items = protoUnlock.UnlockedProtos.Where((Func<IProto, bool>) (x => !(x is RecipeProto) && !(x is ProductProto)));
            indexable.AddRange(items);
          }
        }
        if (indexable.Count == 1)
          this.ResolvedDescription = indexable.First<IProto>().Strings.DescShort;
      }
      Mafi.Assert.That<int>(this.GridPosition.X).IsNotNegative();
      Mafi.Assert.That<int>(this.GridPosition.Y).IsNotNegative();
    }

    protected override void OnInitialize(ProtosDb protosDb)
    {
      base.OnInitialize(protosDb);
      protosDb.TrackProperty((IProtoWithPropertiesUpdate) this, IdsCore.PropertyIds.ResearchStepsMultiplier.Value);
      this.Parents = this.m_parents.ToImmutableArrayAndClear();
    }

    public void OnPropertyUpdated(IProperty property)
    {
      Percent p;
      if (!property.TryGetValueAs<Percent>(IdsCore.PropertyIds.ResearchStepsMultiplier, out p))
        return;
      this.Difficulty = this.m_difficultyBase.ScaledByRounded(p);
      this.TotalStepsRequired = ResearchNodeProto.DifficultyToSteps(this.Difficulty);
    }

    public void AddParent(ResearchNodeProto parent)
    {
      Mafi.Assert.That<bool>(this.IsInitialized).IsFalse();
      this.m_parents.Add(parent);
    }

    static ResearchNodeProto()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ResearchNodeProto.DIFF_MULT = 10;
      ResearchNodeProto.DIFF_MULT_DELAYED = 18;
      ResearchNodeProto.DIFF_MULT_STEP_DELAY = 2;
    }

    [ManuallyWrittenSerialization]
    [DebuggerStepThrough]
    [DebuggerDisplay("{Value,nq}")]
    public new readonly struct ID : 
      IEquatable<ResearchNodeProto.ID>,
      IComparable<ResearchNodeProto.ID>
    {
      /// <summary>Underlying string value of this Id.</summary>
      public readonly string Value;

      public ID(string value)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.Value = value;
      }

      /// <summary>
      /// Implicit conversion to parent <see cref="T:Mafi.Core.Prototypes.Proto.ID" />.
      /// </summary>
      public static implicit operator Proto.ID(ResearchNodeProto.ID id) => new Proto.ID(id.Value);

      public static bool operator ==(Proto.ID lhs, ResearchNodeProto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator ==(ResearchNodeProto.ID lhs, Proto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(Proto.ID lhs, ResearchNodeProto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(ResearchNodeProto.ID lhs, Proto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator ==(ResearchNodeProto.ID lhs, ResearchNodeProto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(ResearchNodeProto.ID lhs, ResearchNodeProto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public override bool Equals(object other)
      {
        return other is ResearchNodeProto.ID other1 && this.Equals(other1);
      }

      public bool Equals(ResearchNodeProto.ID other)
      {
        return string.Equals(this.Value, other.Value, StringComparison.Ordinal);
      }

      public int CompareTo(ResearchNodeProto.ID other)
      {
        return string.CompareOrdinal(this.Value, other.Value);
      }

      public override string ToString() => this.Value ?? string.Empty;

      public override int GetHashCode()
      {
        string str = this.Value;
        return str == null ? 0 : str.GetHashCode();
      }

      public static void Serialize(ResearchNodeProto.ID value, BlobWriter writer)
      {
        writer.WriteString(value.Value);
      }

      public static ResearchNodeProto.ID Deserialize(BlobReader reader)
      {
        return new ResearchNodeProto.ID(reader.ReadString());
      }

      /// <summary>
      /// Implicit conversion to parent <see cref="T:Mafi.Core.Factory.Recipes.RecipeProto.ID" />.
      /// </summary>
      public static implicit operator RecipeProto.ID(ResearchNodeProto.ID id)
      {
        return new RecipeProto.ID(id.Value);
      }
    }

    public new class Gfx : Proto.Gfx
    {
      public static readonly ResearchNodeProto.Gfx Empty;

      public ImmutableArray<KeyValuePair<Option<Proto>, string>> Icons { get; private set; }

      public Gfx(
        ImmutableArray<KeyValuePair<Option<Proto>, string>> icons)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.Icons = icons.CheckNotDefaultStruct<ImmutableArray<KeyValuePair<Option<Proto>, string>>>();
      }

      static Gfx()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        ResearchNodeProto.Gfx.Empty = new ResearchNodeProto.Gfx(ImmutableArray<KeyValuePair<Option<Proto>, string>>.Empty);
      }
    }
  }
}
