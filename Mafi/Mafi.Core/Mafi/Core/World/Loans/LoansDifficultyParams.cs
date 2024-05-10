// Decompiled with JetBrains decompiler
// Type: Mafi.Core.World.Loans.LoansDifficultyParams
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.World.Loans
{
  public static class LoansDifficultyParams
  {
    public static readonly Fix32 StartingScore;
    public static readonly Fix32 MinScore;
    public static readonly Fix32 MaxScore;
    private static readonly ImmutableArray<KeyValuePair<int, Percent>> SCORE_TO_INTEREST_EASY;
    private static readonly ImmutableArray<KeyValuePair<int, Percent>> SCORE_TO_INTEREST_MEDIUM;
    private static readonly ImmutableArray<KeyValuePair<int, Percent>> SCORE_TO_INTEREST_HARD;
    private static readonly ImmutableArray<KeyValuePair<int, int>> SCORE_TO_MAX_ACTIVE_LOANS_EASY;
    private static readonly ImmutableArray<KeyValuePair<int, int>> SCORE_TO_MAX_ACTIVE_LOANS_MEDIUM_HARD;
    /// <summary>
    /// Sets how much of annual production can go to payments. This determines the max loan.
    /// Be careful that this is applied before SCORE_TO_MULTIPLIER. So if you set this to 60%
    /// and SCORE_TO_MULTIPLIER reaches 2.0, player can borrow 120% of their production track
    /// record.
    /// </summary>
    public static readonly Percent MaxAnnualPaymentToProductionRatio;
    private static readonly ImmutableArray<KeyValuePair<int, Fix32>> SCORE_TO_MULTIPLIER;
    private static readonly ImmutableArray<KeyValuePair<int, Percent>> SCORE_TO_FEE;

    public static Percent GetFee(LoansDifficulty difficulty, Fix32 creditScore)
    {
      ImmutableArray<KeyValuePair<int, Percent>> scoreToFee = LoansDifficultyParams.SCORE_TO_FEE;
      Percent fee = scoreToFee.First.Value;
      for (int index = 0; index < scoreToFee.Length; ++index)
      {
        KeyValuePair<int, Percent> keyValuePair = scoreToFee[index];
        fee = keyValuePair.Value;
        Fix32 fix32 = creditScore;
        keyValuePair = scoreToFee[index];
        int key = keyValuePair.Key;
        if (fix32 < key)
          break;
      }
      return fee;
    }

    public static Percent GetInterestRate(LoansDifficulty difficulty, Fix32 creditScore)
    {
      ImmutableArray<KeyValuePair<int, Percent>> scoreToInterest = LoansDifficultyParams.getScoreToInterest(difficulty);
      Percent interestRate = scoreToInterest.First.Value;
      for (int index = 0; index < scoreToInterest.Length; ++index)
      {
        interestRate = scoreToInterest[index].Value;
        if (creditScore < scoreToInterest[index].Key)
          break;
      }
      return interestRate;
    }

    private static ImmutableArray<KeyValuePair<int, Percent>> getScoreToInterest(
      LoansDifficulty difficulty)
    {
      switch (difficulty)
      {
        case LoansDifficulty.Medium:
          return LoansDifficultyParams.SCORE_TO_INTEREST_MEDIUM;
        case LoansDifficulty.Hard:
          return LoansDifficultyParams.SCORE_TO_INTEREST_HARD;
        default:
          return LoansDifficultyParams.SCORE_TO_INTEREST_EASY;
      }
    }

    public static Fix32 GetMultiplier(LoansDifficulty difficulty, Fix32 creditScore)
    {
      ImmutableArray<KeyValuePair<int, Fix32>> scoreToMultiplier = LoansDifficultyParams.getScoreToMultiplier(difficulty);
      Fix32 multiplier = scoreToMultiplier.First.Value;
      for (int index = 0; index < scoreToMultiplier.Length; ++index)
      {
        multiplier = scoreToMultiplier[index].Value;
        if (creditScore.IntegerPart < scoreToMultiplier[index].Key)
          break;
      }
      return multiplier;
    }

    private static ImmutableArray<KeyValuePair<int, Fix32>> getScoreToMultiplier(
      LoansDifficulty difficulty)
    {
      return LoansDifficultyParams.SCORE_TO_MULTIPLIER;
    }

    public static int GetMaxActiveLoans(LoansDifficulty difficulty, Fix32 creditScore)
    {
      ImmutableArray<KeyValuePair<int, int>> scoreToMaxLoans = LoansDifficultyParams.getScoreToMaxLoans(difficulty);
      int maxActiveLoans = scoreToMaxLoans.First.Value;
      for (int index = 0; index < scoreToMaxLoans.Length; ++index)
      {
        maxActiveLoans = scoreToMaxLoans[index].Value;
        if (creditScore < scoreToMaxLoans[index].Key)
          break;
      }
      return maxActiveLoans;
    }

    private static ImmutableArray<KeyValuePair<int, int>> getScoreToMaxLoans(
      LoansDifficulty difficulty)
    {
      switch (difficulty)
      {
        case LoansDifficulty.Medium:
        case LoansDifficulty.Hard:
          return LoansDifficultyParams.SCORE_TO_MAX_ACTIVE_LOANS_MEDIUM_HARD;
        default:
          return LoansDifficultyParams.SCORE_TO_MAX_ACTIVE_LOANS_EASY;
      }
    }

    static LoansDifficultyParams()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      LoansDifficultyParams.StartingScore = 25.ToFix32();
      LoansDifficultyParams.MinScore = 0.ToFix32();
      LoansDifficultyParams.MaxScore = 100.ToFix32();
      LoansDifficultyParams.SCORE_TO_INTEREST_EASY = ((ICollection<KeyValuePair<int, Percent>>) new KeyValuePair<int, Percent>[7]
      {
        Make.Kvp<int, Percent>(15, 10.Percent()),
        Make.Kvp<int, Percent>(25, 8.Percent()),
        Make.Kvp<int, Percent>(30, 7.Percent()),
        Make.Kvp<int, Percent>(40, 6.Percent()),
        Make.Kvp<int, Percent>(55, 5.Percent()),
        Make.Kvp<int, Percent>(75, 5.Percent()),
        Make.Kvp<int, Percent>(95, 4.Percent())
      }).ToImmutableArray<KeyValuePair<int, Percent>>();
      LoansDifficultyParams.SCORE_TO_INTEREST_MEDIUM = ((ICollection<KeyValuePair<int, Percent>>) new KeyValuePair<int, Percent>[7]
      {
        Make.Kvp<int, Percent>(15, 12.Percent()),
        Make.Kvp<int, Percent>(25, 10.Percent()),
        Make.Kvp<int, Percent>(30, 8.Percent()),
        Make.Kvp<int, Percent>(40, 7.Percent()),
        Make.Kvp<int, Percent>(55, 6.Percent()),
        Make.Kvp<int, Percent>(75, 5.Percent()),
        Make.Kvp<int, Percent>(95, 4.Percent())
      }).ToImmutableArray<KeyValuePair<int, Percent>>();
      LoansDifficultyParams.SCORE_TO_INTEREST_HARD = ((ICollection<KeyValuePair<int, Percent>>) new KeyValuePair<int, Percent>[7]
      {
        Make.Kvp<int, Percent>(15, 15.Percent()),
        Make.Kvp<int, Percent>(25, 12.Percent()),
        Make.Kvp<int, Percent>(30, 10.Percent()),
        Make.Kvp<int, Percent>(40, 9.Percent()),
        Make.Kvp<int, Percent>(55, 8.Percent()),
        Make.Kvp<int, Percent>(75, 6.Percent()),
        Make.Kvp<int, Percent>(95, 5.Percent())
      }).ToImmutableArray<KeyValuePair<int, Percent>>();
      LoansDifficultyParams.SCORE_TO_MAX_ACTIVE_LOANS_EASY = ((ICollection<KeyValuePair<int, int>>) new KeyValuePair<int, int>[7]
      {
        Make.Kvp<int, int>(15, 1),
        Make.Kvp<int, int>(25, 2),
        Make.Kvp<int, int>(30, 2),
        Make.Kvp<int, int>(40, 3),
        Make.Kvp<int, int>(55, 4),
        Make.Kvp<int, int>(75, 5),
        Make.Kvp<int, int>(95, 6)
      }).ToImmutableArray<KeyValuePair<int, int>>();
      LoansDifficultyParams.SCORE_TO_MAX_ACTIVE_LOANS_MEDIUM_HARD = ((ICollection<KeyValuePair<int, int>>) new KeyValuePair<int, int>[7]
      {
        Make.Kvp<int, int>(15, 1),
        Make.Kvp<int, int>(25, 1),
        Make.Kvp<int, int>(30, 2),
        Make.Kvp<int, int>(40, 3),
        Make.Kvp<int, int>(55, 4),
        Make.Kvp<int, int>(75, 5),
        Make.Kvp<int, int>(95, 6)
      }).ToImmutableArray<KeyValuePair<int, int>>();
      LoansDifficultyParams.MaxAnnualPaymentToProductionRatio = 40.Percent();
      LoansDifficultyParams.SCORE_TO_MULTIPLIER = ((ICollection<KeyValuePair<int, Fix32>>) new KeyValuePair<int, Fix32>[7]
      {
        Make.Kvp<int, Fix32>(15, 0.5.ToFix32()),
        Make.Kvp<int, Fix32>(25, 0.8.ToFix32()),
        Make.Kvp<int, Fix32>(30, 1.ToFix32()),
        Make.Kvp<int, Fix32>(40, 1.2.ToFix32()),
        Make.Kvp<int, Fix32>(55, 1.4.ToFix32()),
        Make.Kvp<int, Fix32>(75, 1.6.ToFix32()),
        Make.Kvp<int, Fix32>(95, 1.8.ToFix32())
      }).ToImmutableArray<KeyValuePair<int, Fix32>>();
      LoansDifficultyParams.SCORE_TO_FEE = ((ICollection<KeyValuePair<int, Percent>>) new KeyValuePair<int, Percent>[7]
      {
        Make.Kvp<int, Percent>(15, 15.Percent()),
        Make.Kvp<int, Percent>(25, 12.Percent()),
        Make.Kvp<int, Percent>(30, 10.Percent()),
        Make.Kvp<int, Percent>(40, 8.Percent()),
        Make.Kvp<int, Percent>(55, 6.Percent()),
        Make.Kvp<int, Percent>(75, 5.Percent()),
        Make.Kvp<int, Percent>(95, 4.Percent())
      }).ToImmutableArray<KeyValuePair<int, Percent>>();
    }
  }
}
