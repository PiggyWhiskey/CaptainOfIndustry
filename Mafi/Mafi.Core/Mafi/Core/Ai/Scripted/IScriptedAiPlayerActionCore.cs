// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Ai.Scripted.IScriptedAiPlayerActionCore
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi.Core.Ai.Scripted
{
  public interface IScriptedAiPlayerActionCore
  {
    /// <summary>
    /// Performs this action and returns whether this action was finished. If <c>false</c> is returned, this method
    /// will be called again in the next sim step, <c>true</c> denotes that the action was finished.
    /// </summary>
    bool Perform(ScriptedAiPlayer player);
  }
}
