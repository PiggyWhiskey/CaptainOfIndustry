// Decompiled with JetBrains decompiler
// Type: Mafi.Serialization.InitPriority
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

#nullable disable
namespace Mafi.Serialization
{
  public enum InitPriority
  {
    /// <summary>
    /// Method is called immediately after non-ref members are deserialized. Use this only for setting values, do not
    /// read any references as they will be all null at this point.
    /// </summary>
    ImmediatelyAfterSelfDeserialized = 0,
    /// <summary>
    /// Use only for self-init of classes that must initialize before anything else, like collections or resolver.
    /// Resolver should NOT be used during this stage if possible.
    /// </summary>
    Highest = 1,
    /// <summary>
    /// Initialize before other classes but after critical components such as collections.
    /// </summary>
    High = 5,
    /// <summary>
    /// Default priority, use this when you need to initialize self but don't need to rely on other classes being
    /// initialized.
    /// </summary>
    Normal = 10, // 0x0000000A
    /// <summary>
    /// Initialize after most of other classes. Use this if you need some other classes already initialized as part
    /// of your own initialization.
    /// </summary>
    Low = 20, // 0x00000014
    /// <summary>This is usually used for data validation.</summary>
    Lowest = 100, // 0x00000064
  }
}
