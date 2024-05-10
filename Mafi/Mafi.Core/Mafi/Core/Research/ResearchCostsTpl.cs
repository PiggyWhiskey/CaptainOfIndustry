// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Research.ResearchCostsTpl
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Products;
using System;

#nullable disable
namespace Mafi.Core.Research
{
  public class ResearchCostsTpl
  {
    public readonly int Difficulty;

    public static ResearchCostsTpl.Builder Build => new ResearchCostsTpl.Builder();

    public static ResearchCostsTpl UnlockedFromStart => new ResearchCostsTpl(0);

    public ResearchCostsTpl(int difficulty)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Difficulty = difficulty;
    }

    public class Builder
    {
      private int m_difficulty;
      private readonly Lyst<Tuple<ProductProto.ID, Quantity>> m_productQuantities;

      [MustUseReturnValue]
      public ResearchCostsTpl.Builder SetDifficulty(int difficulty)
      {
        this.m_difficulty = difficulty;
        return this;
      }

      [MustUseReturnValue]
      public ResearchCostsTpl.Builder AddProduct(int quantity, ProductProto.ID productId)
      {
        this.m_productQuantities.Add(new Tuple<ProductProto.ID, Quantity>(productId, new Quantity(quantity)));
        return this;
      }

      public static implicit operator ResearchCostsTpl(ResearchCostsTpl.Builder builder)
      {
        return new ResearchCostsTpl(builder.m_difficulty.CheckPositive());
      }

      public Builder()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.m_productQuantities = new Lyst<Tuple<ProductProto.ID, Quantity>>();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }
  }
}
