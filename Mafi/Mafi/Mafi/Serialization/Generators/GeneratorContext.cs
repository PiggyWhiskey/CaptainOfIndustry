// Decompiled with JetBrains decompiler
// Type: Mafi.Serialization.Generators.GeneratorContext
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace Mafi.Serialization.Generators
{
  public class GeneratorContext
  {
    public readonly IReadOnlySet<Type> AllTypes;
    public readonly IReadOnlySet<Type> NonSerializedGlobalDeps;
    public readonly IReadOnlyDictionary<Type, TypeSerializationSpec> TypeSerializationSpecs;
    public readonly StringBuilder GlobalLog;

    public GeneratorContext(
      StringBuilder globalLog,
      IReadOnlySet<Type> allTypes,
      IReadOnlySet<Type> nonSerializedGlobalDeps,
      IReadOnlyDictionary<Type, TypeSerializationSpec> typeSerializationSpecs)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.GlobalLog = globalLog;
      this.AllTypes = allTypes;
      this.NonSerializedGlobalDeps = nonSerializedGlobalDeps;
      this.TypeSerializationSpecs = typeSerializationSpecs;
    }
  }
}
