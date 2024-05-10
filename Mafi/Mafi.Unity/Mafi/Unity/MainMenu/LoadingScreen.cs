// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MainMenu.LoadingScreen
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Utils;
using Mafi.Localization;
using Mafi.Unity.UiFramework.Decorators;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MainMenu
{
  public class LoadingScreen
  {
    private static readonly Color TEXT_COLOR;
    public readonly GameObject RootGo;
    private TextMeshProUGUI m_loadingInfoText;
    private Canvas m_canvas;
    private readonly RectTransform m_overlayTransform;
    private readonly RectTransform m_progressTransform;
    private readonly LocStr[] m_tips;

    public LoadingScreen(AssetsDb assetDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_tips = new LocStr[9]
      {
        Tr.TipOnLoad__Food,
        Tr.TipOnLoad__Diesel,
        Tr.TipOnLoad__OilRig,
        Tr.TipOnLoad__BuildClose,
        Tr.TipOnLoad__BuildTransports,
        Tr.TipOnLoad__Unity,
        Tr.TipOnLoad__Unity2,
        Tr.TipOnLoad__TransportUX,
        Tr.TipOnLoad__TransportStraight
      };
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.RootGo = new GameObject(nameof (LoadingScreen));
      this.m_canvas = this.RootGo.AddComponent<Canvas>();
      this.m_canvas.renderMode = RenderMode.ScreenSpaceOverlay;
      this.m_canvas.sortingOrder = 1000;
      this.RootGo.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
      GameObject gameObject1 = new GameObject("Background");
      Sprite sharedSprite = assetDb.GetSharedSprite("Assets/Unity/UserInterface/MainMenu/MenuBackgroundAlt.jpg");
      BackgroundDecorator.DecorateWithBgImage(gameObject1, sharedSprite, color: new ColorRgba?(ColorRgba.White));
      AspectRatioFitter aspectRatioFitter1 = gameObject1.AddComponent<AspectRatioFitter>();
      aspectRatioFitter1.aspectMode = AspectRatioFitter.AspectMode.EnvelopeParent;
      AspectRatioFitter aspectRatioFitter2 = aspectRatioFitter1;
      Rect rect = sharedSprite.rect;
      double width = (double) rect.width;
      rect = sharedSprite.rect;
      double height = (double) rect.height;
      double num = width / height;
      aspectRatioFitter2.aspectRatio = (float) num;
      gameObject1.PutTo(this.RootGo);
      GameObject gameObject2 = new GameObject("Black overlay");
      this.m_overlayTransform = gameObject2.AddComponent<RectTransform>();
      gameObject2.AddComponent<Image>().color = new ColorRgba(0, 235).ToColor();
      gameObject2.PutToBottomOf(this.RootGo, 140f, Mafi.Unity.UiFramework.Offset.Bottom(100f));
      FontAsset valueOrNull = assetDb.GetSharedFontAsset("Assets/Unity/TextMeshPro/Fonts/Main-Regular/Roboto-Dynamic.asset").ValueOrNull;
      GameObject objectToPlace1 = new GameObject("TipText");
      TextMeshProUGUI textMeshProUgui1 = objectToPlace1.AddComponent<TextMeshProUGUI>();
      textMeshProUgui1.verticalAlignment = VerticalAlignmentOptions.Middle;
      textMeshProUgui1.horizontalAlignment = HorizontalAlignmentOptions.Center;
      int index = UnityEngine.Random.Range(0, this.m_tips.Length);
      textMeshProUgui1.text = Tr.TipOnLoad__Prefix.Format(string.Format("{0:00}", (object) (index + 1)), this.m_tips[index].TranslatedString).Value;
      textMeshProUgui1.fontSize = 18f;
      textMeshProUgui1.font = valueOrNull;
      textMeshProUgui1.fontStyle = TMPro.FontStyles.Bold;
      textMeshProUgui1.color = LoadingScreen.TEXT_COLOR;
      objectToPlace1.PutTo(gameObject2, new Mafi.Unity.UiFramework.Offset(50f, 0.0f, 50f, 50f));
      GameObject objectToPlace2 = new GameObject("LoadingText");
      this.m_loadingInfoText = objectToPlace2.AddComponent<TextMeshProUGUI>();
      this.m_loadingInfoText.verticalAlignment = VerticalAlignmentOptions.Bottom;
      this.m_loadingInfoText.horizontalAlignment = HorizontalAlignmentOptions.Left;
      this.m_loadingInfoText.text = "";
      this.m_loadingInfoText.fontSize = 12f;
      this.m_loadingInfoText.font = valueOrNull;
      this.m_loadingInfoText.fontStyle = TMPro.FontStyles.Bold;
      this.m_loadingInfoText.color = Color.white;
      objectToPlace2.PutTo(this.RootGo, Mafi.Unity.UiFramework.Offset.BottomLeft(30f, 30f));
      GameObject objectToPlace3 = new GameObject("LoadingProgressBar");
      this.m_progressTransform = objectToPlace3.AddComponent<RectTransform>();
      objectToPlace3.AddComponent<Image>().color = 12554844.ToColor();
      objectToPlace3.PutToLeftBottomOf(gameObject2, new Vector2(0.0f, 20f), Mafi.Unity.UiFramework.Offset.Bottom(8f));
      string latestPatchChangeLog = ChangelogUtils.GetLatestPatchChangeLog(true, false);
      if (string.IsNullOrEmpty(latestPatchChangeLog))
        return;
      GameObject gameObject3 = new GameObject("changelog overlay");
      RectTransform p = gameObject3.AddComponent<RectTransform>();
      gameObject3.AddComponent<Image>().color = new ColorRgba(0, 224).ToColor();
      p.SetParent((Transform) this.RootGo.GetComponent<RectTransform>());
      GameObject objectToPlace4 = new GameObject("ChangelogText");
      objectToPlace4.AddComponent<RectTransform>().SetParent((Transform) p);
      TextMeshProUGUI textMeshProUgui2 = objectToPlace4.AddComponent<TextMeshProUGUI>();
      textMeshProUgui2.text = latestPatchChangeLog;
      textMeshProUgui2.verticalAlignment = VerticalAlignmentOptions.Top;
      textMeshProUgui2.horizontalAlignment = HorizontalAlignmentOptions.Left;
      textMeshProUgui2.overflowMode = TextOverflowModes.Truncate;
      textMeshProUgui2.fontSize = 14f;
      textMeshProUgui2.font = valueOrNull;
      textMeshProUgui2.fontStyle = TMPro.FontStyles.Bold;
      textMeshProUgui2.color = LoadingScreen.TEXT_COLOR;
      float x = 500f;
      float y;
      try
      {
        TextStyleSheet result;
        if (assetDb.TryGetSharedAsset<TextStyleSheet>("Assets/Unity/TextMeshPro/Resources/Style Sheets/Default Style Sheet.asset", out result))
          textMeshProUgui2.styleSheet = result;
        else
          Log.Error("Failed to load default style sheet");
        y = (textMeshProUgui2.GetPreferredValues(x - 40f, float.MaxValue).y + 40f).Min(this.m_canvas.pixelRect.height - 300f);
      }
      catch (Exception ex)
      {
        y = this.m_canvas.pixelRect.height - 300f;
        Log.Exception(ex, string.Format("Failed to display changelog on loading screen (v2), text length: {0}", (object) latestPatchChangeLog.Length));
      }
      gameObject3.PutToLeftTopOf(this.RootGo, new Vector2(x, y), Mafi.Unity.UiFramework.Offset.TopLeft(20f, 20f));
      objectToPlace4.PutTo(gameObject3, Mafi.Unity.UiFramework.Offset.All(20f));
    }

    public void ShowLoadingMessage(string s)
    {
      this.m_loadingInfoText.text = s;
      ++this.m_canvas.sortingOrder;
    }

    public void SetProgress(Percent percent)
    {
      this.m_progressTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, percent.Clamp0To100().Apply(this.m_overlayTransform.rect.width));
    }

    public void Destroy()
    {
      Assert.That<GameObject>(this.RootGo).IsNotNull<GameObject>();
      this.RootGo.Destroy();
      this.m_loadingInfoText = (TextMeshProUGUI) null;
      this.m_canvas = (Canvas) null;
    }

    static LoadingScreen()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      LoadingScreen.TEXT_COLOR = 13935981.ToColor();
    }
  }
}
