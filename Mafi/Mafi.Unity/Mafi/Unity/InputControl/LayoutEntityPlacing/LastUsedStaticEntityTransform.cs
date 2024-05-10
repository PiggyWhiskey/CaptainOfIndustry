// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.LayoutEntityPlacing.LastUsedStaticEntityTransform
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.LayoutEntityPlacing
{
  /// <summary>
  /// Contains the TileTransform that was used on previous entity placing, using this class it is shared among all
  /// instantiated <see cref="!:StaticEntityPlacer" />s. <see cref="!:StaticEntityPlacer" /> sets the transform when
  /// placing an entity and reads it again when it initializes entity placing.
  /// </summary>
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class LastUsedStaticEntityTransform
  {
    public Rotation90 LastRotation { get; private set; }

    public bool LastReflection { get; private set; }

    public ThicknessTilesI LastHeight { get; private set; }

    public void SetLastTransform(
      Rotation90 lastRotation,
      bool lastReflection,
      ThicknessTilesI lastHeight)
    {
      this.LastRotation = lastRotation;
      this.LastReflection = lastReflection;
      this.LastHeight = lastHeight;
    }

    public void SetLastTransform(TileTransform lastTransform, ThicknessTilesI lastHeight)
    {
      this.SetLastTransform(lastTransform.Rotation, lastTransform.IsReflected, lastHeight);
    }

    public LastUsedStaticEntityTransform()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: reference to a compiler-generated field
      this.\u003CLastRotation\u003Ek__BackingField = Rotation90.Deg0;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
