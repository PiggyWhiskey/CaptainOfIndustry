// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Recipes.RecipeProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Serialization;
using System;
using System.Diagnostics;

#nullable disable
namespace Mafi.Core.Factory.Recipes
{
  /// <summary>
  /// Recipe that transforms some set of products to another set of products using some machine.
  /// </summary>
  [DebuggerDisplay("Recipe: {Id}")]
  public class RecipeProto : Proto, IRecipeForUi
  {
    /// <summary>
    /// Union of <see cref="!:InputsAtStart" /> and <see cref="!:InputsPeriodical" />.
    /// </summary>
    public readonly ImmutableArray<RecipeInput> AllInputs;
    /// <summary>
    /// Union of <see cref="F:Mafi.Core.Factory.Recipes.RecipeProto.OutputsAtEnd" /> and <see cref="!:OutputsPeriodical" />.
    /// </summary>
    public readonly ImmutableArray<RecipeOutput> AllOutputs;
    /// <summary>
    /// Products, quantity and their assignments returned by the process when it ends its work.
    /// </summary>
    public readonly ImmutableArray<RecipeOutput> OutputsAtEnd;
    /// <summary>
    /// Products, quantity and their assignments returned continuously by the process while it works.
    /// </summary>
    public readonly ImmutableArray<RecipeOutput> OutputsAtStart;
    /// <summary>
    /// Minimum utilization. When set, this recipe can be executed partially.
    /// </summary>
    public readonly Percent MinUtilization;
    /// <summary>
    /// Greatest common divisor for input and output quantities.
    /// </summary>
    public readonly int QuantitiesGcd;
    /// <summary>
    /// Reason with which this recipe destroys its products. E.g. dumped for waste pump.
    /// </summary>
    public readonly DestroyReason ProductsDestroyReason;
    /// <summary>
    /// Is able to for instance bypass recycling ratio. Useful for machines such as shredder.
    /// </summary>
    public readonly bool DisableSourceProductsConversionLoss;

    Proto.ID IRecipeForUi.Id => (Proto.ID) this.Id;

    /// <summary>All inputs visible to the user.</summary>
    public ImmutableArray<RecipeInput> AllUserVisibleInputs { get; }

    /// <summary>All outputs visible to the user.</summary>
    public ImmutableArray<RecipeOutput> AllUserVisibleOutputs { get; }

    /// <summary>Number of updates it takes to transform the products</summary>
    public Duration Duration { get; }

    public RecipeProto(
      RecipeProto.ID id,
      Proto.Str strings,
      Duration duration,
      ImmutableArray<RecipeInput> allInputs,
      ImmutableArray<RecipeOutput> allOutputs,
      Percent minUtilization,
      DestroyReason productsDestroyReason = DestroyReason.General,
      bool disableSourceProductsConversionLoss = false)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector((Proto.ID) id, strings);
      this.Duration = duration.CheckPositive();
      this.AllInputs = allInputs;
      this.AllOutputs = allOutputs;
      this.DisableSourceProductsConversionLoss = disableSourceProductsConversionLoss;
      this.AllUserVisibleInputs = this.AllInputs.Filter((Predicate<RecipeInput>) (x => !x.HideInUi));
      this.AllUserVisibleOutputs = this.AllOutputs.Filter((Predicate<RecipeOutput>) (x => !x.HideInUi));
      this.OutputsAtEnd = this.AllOutputs.Filter((Predicate<RecipeOutput>) (x => !x.TriggerAtStart));
      this.OutputsAtStart = this.AllOutputs.Filter((Predicate<RecipeOutput>) (x => x.TriggerAtStart));
      this.ProductsDestroyReason = productsDestroyReason;
      foreach (RecipeProduct allInput in allInputs)
      {
        foreach (IoPortTemplate port in allInput.Ports)
        {
          if (port.Type != IoPortType.Input)
            throw new ProtoBuilderException(string.Format("Recipe '{0}': Port '{1}' is not an input type.", (object) id, (object) port.Name));
        }
      }
      foreach (RecipeProduct allOutput in allOutputs)
      {
        foreach (IoPortTemplate port in allOutput.Ports)
        {
          if (port.Type != IoPortType.Output)
            throw new ProtoBuilderException(string.Format("Recipe '{0}': Port '{1}' is not an output type.", (object) id, (object) port.Name));
        }
      }
      this.MinUtilization = minUtilization;
      if (this.MinUtilization <= Percent.Zero || this.MinUtilization > Percent.Hundred)
        throw new ProtoBuilderException(string.Format("MinUtilization '{0}' is not between (0,100]!", (object) this.MinUtilization));
      if (this.MinUtilization == Percent.Hundred)
      {
        this.QuantitiesGcd = 1;
      }
      else
      {
        ImmutableArray<RecipeProduct> immutableArray = this.AllInputs.As<RecipeProduct>().Concat(this.AllOutputs.As<RecipeProduct>());
        this.QuantitiesGcd = immutableArray.IsEmpty ? 1 : MafiMath.Gcd(immutableArray.Select<int>((Func<RecipeProduct, int>) (x => x.Quantity.Value)));
      }
      if (this.QuantitiesGcd <= 0)
        throw new ProtoBuilderException(string.Format("Invalid QuantitiesGcd '{0}'!", (object) this.QuantitiesGcd));
    }

    public override string ToString() => string.Format("{0} (recipe)", (object) this.Id);

    public RecipeProto.ID Id => new RecipeProto.ID(base.Id.Value);

    [DebuggerDisplay("{Value,nq}")]
    [DebuggerStepThrough]
    [ManuallyWrittenSerialization]
    public new readonly struct ID : IEquatable<RecipeProto.ID>, IComparable<RecipeProto.ID>
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
      public static implicit operator Proto.ID(RecipeProto.ID id) => new Proto.ID(id.Value);

      public static bool operator ==(Proto.ID lhs, RecipeProto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator ==(RecipeProto.ID lhs, Proto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(Proto.ID lhs, RecipeProto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(RecipeProto.ID lhs, Proto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator ==(RecipeProto.ID lhs, RecipeProto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(RecipeProto.ID lhs, RecipeProto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public override bool Equals(object other)
      {
        return other is RecipeProto.ID other1 && this.Equals(other1);
      }

      public bool Equals(RecipeProto.ID other)
      {
        return string.Equals(this.Value, other.Value, StringComparison.Ordinal);
      }

      public int CompareTo(RecipeProto.ID other) => string.CompareOrdinal(this.Value, other.Value);

      public override string ToString() => this.Value ?? string.Empty;

      public override int GetHashCode()
      {
        string str = this.Value;
        return str == null ? 0 : str.GetHashCode();
      }

      public static void Serialize(RecipeProto.ID value, BlobWriter writer)
      {
        writer.WriteString(value.Value);
      }

      public static RecipeProto.ID Deserialize(BlobReader reader)
      {
        return new RecipeProto.ID(reader.ReadString());
      }
    }
  }
}
