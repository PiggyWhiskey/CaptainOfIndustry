// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Research.IResearchNodeFriend
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Buildings.ResearchLab;

#nullable disable
namespace Mafi.Core.Research
{
  public interface IResearchNodeFriend
  {
    void SetState(ResearchNodeState state);

    void SetIsLockedByParents(bool isLockedByParents);

    void IncStepsDone(Fix32 steps);

    void BuildGraph(ImmutableArray<ResearchNode> nodes);

    void PopulateRequiredLab(Option<ResearchLabProto> labRequired);

    void ClearRequiredLab();

    void ForceStepsToDone();
  }
}
