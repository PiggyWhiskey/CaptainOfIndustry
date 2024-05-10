// Decompiled with JetBrains decompiler
// Type: Mafi.BuildInfoData
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi
{
  /// <summary>
  /// Represents build data that stores details about how current assembly was built.
  /// <see cref="T:DefinesExplanations" /> for overview.
  /// </summary>
  public readonly struct BuildInfoData
  {
    public readonly bool IsDebug;
    public readonly bool IsDevOnly;
    public readonly bool IsAlphaOnly;
    public readonly bool AssertionsEnabled;
    public readonly bool TracingEnabled;

    public BuildInfoData(
      bool isDebug,
      bool isDevOnly,
      bool isAlphaOnly,
      bool assertionsEnabled,
      bool tracingEnabled)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.IsDebug = isDebug;
      this.IsDevOnly = isDevOnly;
      this.IsAlphaOnly = isAlphaOnly;
      this.AssertionsEnabled = assertionsEnabled;
      this.TracingEnabled = tracingEnabled;
    }

    public override string ToString()
    {
      return "Debug=" + this.IsDebug.ToCSharp() + "; DevOnly=" + this.IsDevOnly.ToCSharp() + "; AlphaOnly=" + this.IsAlphaOnly.ToCSharp() + "; Assertions=" + this.AssertionsEnabled.ToCSharp() + "; Tracing=" + this.TracingEnabled.ToCSharp();
    }
  }
}
