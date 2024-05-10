// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Components.RecipeDurationNormalizer
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Products;
using Mafi.Core.Syncers;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UserInterface.Components
{
  public class RecipeDurationNormalizer
  {
    private readonly UiBuilder m_builder;

    public bool IsNormalizationOn => this.m_builder.UiPreferences.IsRecipeNormalizationOn;

    public bool IsNormalizationOff => !this.m_builder.UiPreferences.IsRecipeNormalizationOn;

    public RecipeDurationNormalizer(UiBuilder builder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_builder = builder;
    }

    public Fix32 NormalizeQuantity(
      ProductProto product,
      Quantity standardQuantity,
      Duration recipeDuration)
    {
      return !this.CanNormalizeRecipe(recipeDuration) || !this.IsNormalizationOn || product.DoNotNormalize ? standardQuantity.Value.ToFix32() : (60.Seconds().Ticks * standardQuantity.Value).DivToFix32(recipeDuration.Ticks);
    }

    public Duration GetNormalizedDuration(Duration regularDuration)
    {
      return !this.IsNormalizationOn ? regularDuration : 60.Seconds();
    }

    public Fix32 NormalizeQuantity(RecipeProduct recipeProduct, Duration recipeDuration)
    {
      return this.NormalizeQuantity(recipeProduct.Product, recipeProduct.Quantity, recipeDuration);
    }

    public string NormalizeQuantityAsString(ProductQuantity pqIn, Duration recipeDuration)
    {
      Fix32 fix32 = this.NormalizeQuantity(pqIn.Product, pqIn.Quantity, recipeDuration);
      return fix32.IsInteger ? new ProductQuantity(pqIn.Product, fix32.IntegerPart.Quantity()).FormatNumberAndUnitOnly().Value : fix32.ToStringRounded(1);
    }

    public string NormalizeQuantityAsString(RecipeProduct recipeProduct, Duration recipeDuration)
    {
      Fix32 fix32 = this.NormalizeQuantity(recipeProduct.Product, recipeProduct.Quantity, recipeDuration);
      return fix32.IsInteger ? new ProductQuantity(recipeProduct.Product, fix32.IntegerPart.Quantity()).FormatNumberAndUnitOnly().Value : fix32.ToStringRounded(fix32 > 10 ? 1 : 2);
    }

    public bool CanNormalizeRecipe(Duration recipeDuration) => recipeDuration < 1.Years();

    public string NormalizeThroughput(PartialQuantity throughputPerTick)
    {
      Duration duration = this.IsNormalizationOn ? 60.Seconds() : 1.Seconds();
      Fix32 fix32 = throughputPerTick.Value * duration.Ticks;
      return this.IsNormalizationOn ? fix32.ToIntRounded().ToString() : fix32.ToStringRounded(1);
    }

    public void AttachPer60ToggleToTitle(
      Txt title,
      UiBuilder builder,
      UpdaterBuilder updaterBuilder)
    {
      float width;
      this.BuildPer60Toggle(builder, updaterBuilder, out width).PutToRightOf<Panel>((IUiElement) title, width, Offset.Right(20f));
    }

    public Panel BuildPer60Toggle(
      UiBuilder builder,
      UpdaterBuilder updaterBuilder,
      out float width)
    {
      Panel parent = builder.NewPanel("Switch");
      SwitchBtn btn = builder.NewSwitchBtn((IUiElement) parent).SetText(Tr.ShowPerDuration.TranslatedString + " / 60").SetOnToggleAction((Action<bool>) (isOn => this.m_builder.UiPreferences.SetRecipeNormalizationOn(isOn)));
      btn.PutToLeftOf<SwitchBtn>((IUiElement) parent, btn.GetWidth());
      builder.NewIconContainer("Icon").SetIcon("Assets/Unity/UserInterface/General/Clock.svg", builder.Style.Global.Text.Color).PutToLeftOf<IconContainer>((IUiElement) parent, 20f, Offset.Left(btn.GetWidth() + 5f));
      updaterBuilder.Observe<bool>((Func<bool>) (() => this.IsNormalizationOn)).Do((Action<bool>) (normalize => btn.SetIsOn(normalize)));
      width = (float) ((double) btn.GetWidth() + 20.0 + 5.0);
      return parent;
    }
  }
}
