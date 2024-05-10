// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.ObjectEditor.Editors.ObjEditorForImage
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UiToolkit.Component;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library.ObjectEditor.Editors
{
  public class ObjEditorForImage : StretchedImg, IObjEditor
  {
    public UiComponent Component => (UiComponent) this;

    public ObjEditorForImage()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public void SetData(ObjEditorData data)
    {
      this.Image<ObjEditorForImage>((data.Value as Texture2D).CreateOption<Texture2D>());
    }

    public bool TryGetValue(out object value)
    {
      value = (object) this.InnerElement.style.backgroundImage.value.texture;
      return value != null;
    }
  }
}
