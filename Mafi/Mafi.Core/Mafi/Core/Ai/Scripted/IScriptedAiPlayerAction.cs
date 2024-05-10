// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Ai.Scripted.IScriptedAiPlayerAction
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using System;

#nullable disable
namespace Mafi.Core.Ai.Scripted
{
  public interface IScriptedAiPlayerAction
  {
    /// <summary>A shot description of this action.</summary>
    string Description { get; }

    /// <summary>
    /// Returns a type of a type that inherits <see cref="T:Mafi.Core.Ai.Scripted.IScriptedAiPlayerActionCore" /> which will be instantiated
    /// via resolver and called. The first argument of the action core must be this class. Other arguments may be
    /// anything that resolver can provide.
    /// </summary>
    Type ActionCoreType { get; }
  }
}
