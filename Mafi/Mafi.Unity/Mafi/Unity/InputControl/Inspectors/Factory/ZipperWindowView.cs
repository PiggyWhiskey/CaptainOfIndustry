// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Factory.ZipperWindowView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Factory.Zippers;
using Mafi.Core.Syncers;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Factory
{
  internal class ZipperWindowView : StaticEntityInspectorBase<Zipper>
  {
    private static readonly Color NOT_CONNECTED_NUMBER_BG;
    private static readonly Color NON_PRIORITY_NUMBER_BG;
    private static readonly Color PRIORITY_NUMBER_BG;
    private readonly ZipperInspector m_controller;
    private readonly Lyst<IUiElement> m_portNumberPanels;

    protected override Zipper Entity => this.m_controller.SelectedEntity;

    public IIndexable<IUiElement> PortNumberPanels
    {
      get => (IIndexable<IUiElement>) this.m_portNumberPanels;
    }

    public ZipperWindowView(ZipperInspector controller)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_portNumberPanels = new Lyst<IUiElement>();
      // ISSUE: explicit constructor call
      base.\u002Ector((IEntityInspector) controller);
      this.m_controller = controller.CheckNotNull<ZipperInspector>();
    }

    protected override void AddCustomItems(StackContainer itemContainer)
    {
      base.AddCustomItems(itemContainer);
      UpdaterBuilder updater = UpdaterBuilder.Start();
      this.SetWidth(500f);
      this.AddClearButton(new Action(((EntityInspector<Zipper, ZipperWindowView>) this.m_controller).Clear));
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.BalancerRatios__Title, new LocStrFormatted?((LocStrFormatted) Tr.BalancerRatios__Tooltip));
      this.AddSwitch(itemContainer, Tr.BalancerRatios__Inputs.TranslatedString, (Action<bool>) (x => this.m_controller.InputScheduler.ScheduleInputCmd<ZipperSetForceEvenInputsCmd>(new ZipperSetForceEvenInputsCmd(this.Entity.Id, x))), updater, (Func<bool>) (() => this.Entity.ForceEvenInputs));
      this.AddSwitch(itemContainer, Tr.BalancerRatios__Outputs.TranslatedString, (Action<bool>) (x => this.m_controller.InputScheduler.ScheduleInputCmd<ZipperSetForceEvenOutputsCmd>(new ZipperSetForceEvenOutputsCmd(this.Entity.Id, x))), updater, (Func<bool>) (() => this.Entity.ForceEvenOutputs));
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.BalancerPrioritization__Title, new LocStrFormatted?((LocStrFormatted) Tr.BalancerPrioritization__Tooltip));
      RawTextureContainer parent = this.Builder.NewRawTextureContainer("zipTex").SetTexture((Texture) this.m_controller.ZipperViewRenderTexture).AppendTo<RawTextureContainer>(itemContainer, new Vector2?(new Vector2(448f, 448f)), ContainerPosition.MiddleOrCenter);
      for (int index = 0; index < this.m_controller.MaxPortsCount; ++index)
      {
        int capturedI = index;
        Btn btn = this.Builder.NewBtn("PrioBtn").SetButtonStyle(this.Builder.Style.Global.GeneralBtn).OnClick((Action) (() => this.m_controller.InputScheduler.ScheduleInputCmd<ZipperSetPriorityPortsCmd>(new ZipperSetPriorityPortsCmd(this.Entity.Id, this.Entity.Ports[capturedI].Name, new bool?())))).PutRelativeTo<Btn>((IUiElement) parent, new Vector2(90f, 34f), HorizontalPosition.Left, VerticalPosition.Bottom);
        IconContainer icon = this.Builder.NewIconContainer("Icon").SetIcon("Assets/Unity/UserInterface/General/Priority.svg").PutToLeftTopOf<IconContainer>((IUiElement) btn, 18.Vector2(), Offset.Left(4f) + Offset.Top(6f));
        Txt txt1 = this.Builder.NewTxt("PrioLabel").SetText((LocStrFormatted) Tr.Priority);
        TextStyle boldText1 = this.Builder.Style.Global.BoldText;
        ref TextStyle local1 = ref boldText1;
        int? nullable1 = new int?(10);
        bool? nullable2 = new bool?(true);
        ColorRgba? color1 = new ColorRgba?();
        FontStyle? fontStyle1 = new FontStyle?();
        int? fontSize1 = nullable1;
        bool? isCapitalized1 = nullable2;
        TextStyle textStyle1 = local1.Extend(color1, fontStyle1, fontSize1, isCapitalized1);
        Txt prioLabel = txt1.SetTextStyle(textStyle1).SetAlignment(TextAnchor.MiddleLeft).PutToTopOf<Txt>((IUiElement) btn, 18f, Offset.Left(26f) + Offset.Top(1f));
        Txt txt2 = this.Builder.NewTxt("IoLabel");
        TextStyle boldText2 = this.Builder.Style.Global.BoldText;
        ref TextStyle local2 = ref boldText2;
        nullable1 = new int?(10);
        ColorRgba? color2 = new ColorRgba?();
        FontStyle? fontStyle2 = new FontStyle?();
        int? fontSize2 = nullable1;
        nullable2 = new bool?();
        bool? isCapitalized2 = nullable2;
        TextStyle textStyle2 = local2.Extend(color2, fontStyle2, fontSize2, isCapitalized2);
        Txt ioLabel = txt2.SetTextStyle(textStyle2).PutToBottomOf<Txt>((IUiElement) btn, 18f, Offset.Left(26f));
        float num = (float) ((double) prioLabel.GetPreferedWidth() + 18.0 + 12.0);
        float x = num.Clamp(80f, 110f);
        if ((double) x < (double) num)
          prioLabel.BestFitEnabled(10);
        btn.PutRelativeTo<Btn>((IUiElement) parent, new Vector2(x, 34f), HorizontalPosition.Left, VerticalPosition.Bottom);
        btn.RectTransform.pivot = new Vector2(0.5f, 0.5f);
        this.m_portNumberPanels.Add((IUiElement) btn);
        updater.Observe<bool>((Func<bool>) (() => capturedI < this.Entity.Ports.Length && this.Entity.Ports[capturedI].IsConnected)).Observe<bool>((Func<bool>) (() => capturedI < this.Entity.Ports.Length && this.Entity.IsPortPrioritizedArray[capturedI])).Observe<bool>((Func<bool>) (() => capturedI < this.Entity.Ports.Length && this.Entity.Ports[capturedI].IsConnectedAsInput)).Do((Action<bool, bool, bool>) ((isConnected, isPrioritized, isInput) =>
        {
          if (isConnected)
          {
            ioLabel.SetText((LocStrFormatted) (isInput ? Tr.IoLabel__IN : Tr.IoLabel__OUT));
            ioLabel.SetColor(isInput ? this.Builder.Style.Global.GreenForDark : this.Builder.Style.Global.DangerClr);
          }
          else
          {
            ioLabel.SetText("");
            ioLabel.SetColor(ColorRgba.Gray);
          }
          ColorRgba colorRgba = (ColorRgba) 16762624;
          prioLabel.SetColor(isPrioritized ? colorRgba : ColorRgba.Gray);
          icon.SetColor(isPrioritized ? colorRgba : ColorRgba.Gray);
        }));
      }
      this.AddUpdater(updater.Build());
    }

    static ZipperWindowView()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      ZipperWindowView.NOT_CONNECTED_NUMBER_BG = new Color(0.5f, 0.5f, 0.5f, 0.5f);
      ZipperWindowView.NON_PRIORITY_NUMBER_BG = new Color(1f, 1f, 1f, 0.5f);
      ZipperWindowView.PRIORITY_NUMBER_BG = new Color(1f, 0.7f, 0.0f, 0.8f);
    }
  }
}
