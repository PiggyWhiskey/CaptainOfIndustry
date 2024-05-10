// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.EdictRenameUtil
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Base.Prototypes
{
  internal static class EdictRenameUtil
  {
    internal static string ToOldId(this Proto.ID newId) => newId.Value.Replace("Edict_", "");
  }
}
