// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Recipes.RecipeProtoBuilder
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Mods;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.Factory.Recipes
{
  public sealed class RecipeProtoBuilder : IProtoBuilder
  {
    public const string ANY_COMPATIBLE_PORT = "*";
    public const string VIRTUAL_PORT = "VIRTUAL";

    public ProtosDb ProtosDb => this.Registrator.PrototypesDb;

    public ProtoRegistrator Registrator { get; }

    public RecipeProtoBuilder(ProtoRegistrator registrator)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Registrator = registrator;
    }

    public RecipeProtoBuilder.State Start(
      string name,
      RecipeProto.ID recipeId,
      MachineProto.ID machineId)
    {
      MachineProto orThrow = this.ProtosDb.GetOrThrow<MachineProto>((Proto.ID) machineId);
      return this.Start(name, recipeId, orThrow);
    }

    public RecipeProtoBuilder.State Start(
      string name,
      RecipeProto.ID recipeId,
      MachineProto machine)
    {
      return new RecipeProtoBuilder.State(this, recipeId, name.CheckNotNull<string>(), machine.CheckNotNull<MachineProto>());
    }

    public class State : 
      ProtoBuilderState<RecipeProtoBuilder.State>,
      IRecipeProtoBuilderState<RecipeProtoBuilder.State>
    {
      private readonly RecipeProto.ID m_protoId;
      private readonly MachineProto m_machine;
      private readonly Lyst<RecipeInput> m_inputs;
      private readonly Lyst<RecipeOutput> m_outputs;
      private Duration? m_duration;
      private bool m_periodicIoSet;
      private Percent m_minimumUtilization;
      private bool m_emptyRecipesEnabled;
      private bool m_disableSourceProductsConversionLoss;
      private DestroyReason m_productsDestroyReason;

      public State(
        RecipeProtoBuilder builder,
        RecipeProto.ID protoId,
        string name,
        MachineProto machine,
        string translationComment = "HIDE: recipe name")
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.m_inputs = new Lyst<RecipeInput>();
        this.m_outputs = new Lyst<RecipeOutput>();
        this.m_minimumUtilization = Percent.Hundred;
        this.m_productsDestroyReason = DestroyReason.General;
        // ISSUE: explicit constructor call
        base.\u002Ector((IProtoBuilder) builder, (Proto.ID) protoId, name, translationComment);
        this.m_protoId = protoId;
        this.m_machine = machine;
      }

      public ProtosDb ProtosDb => this.Builder.ProtosDb;

      public Duration? RecipeDuration => this.m_duration;

      public override RecipeProtoBuilder.State Description(string description, string explanation = "short description")
      {
        return this;
      }

      public RecipeProtoBuilder.State SetDuration(Duration durationTicks)
      {
        if (this.m_periodicIoSet)
          throw new ProtoBuilderException("Cannot set duration, some periodic IO was already set.");
        this.m_duration = new Duration?(durationTicks);
        return this;
      }

      public RecipeProtoBuilder.State SetDurationSeconds(int seconds)
      {
        return this.SetDuration(seconds.Seconds());
      }

      public RecipeProtoBuilder.State SetProductsDestroyReason(DestroyReason reason)
      {
        this.m_productsDestroyReason = reason;
        return this;
      }

      public RecipeProtoBuilder.State EnablePartialExecution(Percent minUtilization)
      {
        this.m_minimumUtilization = minUtilization.CheckWithinIncl(Percent.Zero, Percent.Hundred);
        return this;
      }

      public RecipeProtoBuilder.State AddInput(
        string portSelector,
        ProductProto product,
        Quantity quantity,
        bool hideInUi = false)
      {
        if (this.m_inputs.Any<RecipeInput>((Predicate<RecipeInput>) (x => (Proto) x.Product == (Proto) product)))
        {
          Assert.Fail(string.Format("Input for product {0} was already defined", (object) product.Id));
          return this;
        }
        this.m_inputs.Add(new RecipeInput(this.resolvePortSelector(portSelector, IoPortType.Input, product.Type), product, quantity, hideInUi));
        return this;
      }

      public RecipeProtoBuilder.State AddOutput(
        string portSelector,
        ProductProto product,
        Quantity quantity,
        bool triggerAtStart = false,
        bool hideInUi = false)
      {
        if (this.m_outputs.Any<RecipeOutput>((Predicate<RecipeOutput>) (x => (Proto) x.Product == (Proto) product)))
          throw new ProtoBuilderException(string.Format("Output for product {0} was already defined", (object) product.Id));
        this.m_outputs.Add(new RecipeOutput(this.resolvePortSelector(portSelector, IoPortType.Output, product.Type), product, quantity, triggerAtStart, hideInUi));
        return this;
      }

      public RecipeProtoBuilder.State EnableEmptyRecipe()
      {
        this.m_emptyRecipesEnabled = true;
        return this;
      }

      public RecipeProtoBuilder.State DisableSourceProductsConversionLoss()
      {
        this.m_disableSourceProductsConversionLoss = true;
        return this;
      }

      public RecipeProto BuildAndAdd()
      {
        this.verifyRecipeIo();
        RecipeProto db = this.AddToDb<RecipeProto>(new RecipeProto(this.m_protoId, this.Strings, this.ValueOrThrow<Duration>(this.m_duration, "Duration"), this.m_inputs.ToImmutableArray(), this.m_outputs.ToImmutableArray(), this.m_minimumUtilization, this.m_productsDestroyReason, this.m_disableSourceProductsConversionLoss));
        this.m_machine.AddRecipe(db);
        return db;
      }

      /// <summary>
      /// Verifies that the machine has adequate I/O ports for this recipe.
      /// </summary>
      private void verifyRecipeIo()
      {
        if (!this.m_emptyRecipesEnabled && this.m_inputs.Count == 0 && this.m_outputs.Count == 0)
          throw new ProtoBuilderException(string.Format("Recipe '{0}': has to have at least one input or output.", (object) this.m_protoId));
        Dictionary<ProductType, int> dictionary1 = this.m_machine.InputPorts.AsEnumerable().GroupBy<IoPortTemplate, ProductType>((Func<IoPortTemplate, ProductType>) (p => p.Shape.AllowedProductType)).ToDictionary<IGrouping<ProductType, IoPortTemplate>, ProductType, int>((Func<IGrouping<ProductType, IoPortTemplate>, ProductType>) (g => g.Key), (Func<IGrouping<ProductType, IoPortTemplate>, int>) (g => g.Count<IoPortTemplate>()));
        if (!dictionary1.ContainsKey(ProductType.ANY))
        {
          foreach (KeyValuePair<ProductType, int> keyValuePair in this.m_inputs.Where<RecipeInput>((Func<RecipeInput, bool>) (p => p.Product.Type != VirtualProductProto.ProductType)).GroupBy<RecipeInput, ProductType>((Func<RecipeInput, ProductType>) (p => p.Product.Type)).ToDictionary<IGrouping<ProductType, RecipeInput>, ProductType, int>((Func<IGrouping<ProductType, RecipeInput>, ProductType>) (g => g.Key), (Func<IGrouping<ProductType, RecipeInput>, int>) (g => g.Count<RecipeInput>())))
          {
            int num;
            if (!dictionary1.TryGetValue(keyValuePair.Key, out num))
              throw new ProtoBuilderException(string.Format("Failed to add recipe '{0}' to machine '{1}'. ", (object) this.Id, (object) this.m_machine.Id) + string.Format("The recipe needs input for '{0}' but machine does not have any ports for ", (object) keyValuePair.Key) + "that product type.");
            if (num < keyValuePair.Value)
              throw new ProtoBuilderException(string.Format("Failed to add recipe '{0}' to machine '{1}'. ", (object) this.Id, (object) this.m_machine.Id) + string.Format("The recipe needs input of '{0}' on {1} ports but machine has only ", (object) keyValuePair.Key, (object) keyValuePair.Value) + string.Format("{0} ports of that type.", (object) num));
          }
        }
        Dictionary<ProductType, int> dictionary2 = this.m_machine.OutputPorts.AsEnumerable().GroupBy<IoPortTemplate, ProductType>((Func<IoPortTemplate, ProductType>) (p => p.Shape.AllowedProductType)).ToDictionary<IGrouping<ProductType, IoPortTemplate>, ProductType, int>((Func<IGrouping<ProductType, IoPortTemplate>, ProductType>) (g => g.Key), (Func<IGrouping<ProductType, IoPortTemplate>, int>) (g => g.Count<IoPortTemplate>()));
        if (dictionary2.ContainsKey(ProductType.ANY))
          return;
        foreach (KeyValuePair<ProductType, int> keyValuePair in this.m_outputs.Where<RecipeOutput>((Func<RecipeOutput, bool>) (p => p.Product.Type != VirtualProductProto.ProductType)).GroupBy<RecipeOutput, ProductType>((Func<RecipeOutput, ProductType>) (p => p.Product.Type)).ToDictionary<IGrouping<ProductType, RecipeOutput>, ProductType, int>((Func<IGrouping<ProductType, RecipeOutput>, ProductType>) (g => g.Key), (Func<IGrouping<ProductType, RecipeOutput>, int>) (g => g.Count<RecipeOutput>())))
        {
          KeyValuePair<ProductType, int> outKvp = keyValuePair;
          int num;
          if (!dictionary2.TryGetValue(outKvp.Key, out num))
            throw new ProtoBuilderException(string.Format("Failed to add recipe '{0}' to machine '{1}'. ", (object) this.Id, (object) this.m_machine.Id) + string.Format("The recipe needs output for '{0}' but machine does not have any ports for ", (object) outKvp.Key) + "that product type.");
          if (num < outKvp.Value)
            throw new ProtoBuilderException(string.Format("Failed to add recipe '{0}' to machine '{1}'. ", (object) this.Id, (object) this.m_machine.Id) + string.Format("The recipe needs output of '{0}' on {1} ports but machine has only ", (object) outKvp.Key, (object) outKvp.Value) + string.Format("{0} ports of that type.", (object) num));
          if (num > 1 && this.m_outputs.Count<RecipeOutput>((Func<RecipeOutput, bool>) (p => p.Product.Type == outKvp.Key && p.Ports.Length > 1)) > 1)
            throw new ProtoBuilderException(string.Format("Failed to add recipe '{0}' to machine '{1}'. Output port matching is ", (object) this.Id, (object) this.m_machine.Id) + string.Format("ambiguous. The recipe has multiple products of type '{0}' and the ", (object) outKvp.Key) + string.Format("machine has {0} ports for that type.", (object) num));
        }
      }

      private ImmutableArray<IoPortTemplate> resolvePortSelector(
        string portSelector,
        IoPortType type,
        ProductType productType)
      {
        switch (portSelector)
        {
          case "VIRTUAL":
            return ImmutableArray<IoPortTemplate>.Empty;
          case "*":
            if (!(productType == VirtualProductProto.ProductType))
              break;
            goto case "VIRTUAL";
        }
        try
        {
          return this.m_machine.Layout.ResolvePortSelectorOrThrow(portSelector, type, productType).ToImmutableArray();
        }
        catch (ProtoBuilderException ex)
        {
          throw new ProtoBuilderException(string.Format("Failed to resolve build recipe '{0}', invalid port(s) of machine '{1}'.", (object) this.Id, (object) this.m_machine.Id), (Exception) ex);
        }
      }
    }
  }
}
