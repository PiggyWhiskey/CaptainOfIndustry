using Mafi.Localization;
using Mafi.Unity;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;

using UnityEngine;
using UnityEngine.UI;

internal static class UIBuilderBtnExtensions
{
    public static Panel NewPanel(this UiBuilder builder, string name) => new Panel(builder, name);

    public static PanelWithShadow NewPanelWithShadow(this UiBuilder builder, string name)
    {
        return new PanelWithShadow(builder, name);
    }

    public static StackContainer NewStackContainer(this UiBuilder builder, string name)
    {
        return new StackContainer(builder, name);
    }

    internal static Btn NewBtn(this UiBuilder builder, string name, IUiElement parent = null)
    {
        return new Btn(builder, name, parent?.GameObject);
    }

    internal static Panel NewPanel(this UiBuilder builder, string name, IUiElement parent)
    {
        return new Panel(builder, name, parent?.GameObject);
    }

    internal static PanelWithShadow NewPanelWithShadow(
        this UiBuilder builder,
        string name,
        IUiElement parent)
    {
        return new PanelWithShadow(builder, name, parent?.GameObject);
    }

    internal static StackContainer NewStackContainer(
                        this UiBuilder builder,
        string name,
        IUiElement parent)
    {
        return new StackContainer(builder, name, parent?.GameObject);
    }
}

internal static class UIBuilderElementExtensions
{
    public static T AddTo<T>(
        this T objectToPlace,
        StackContainer container,
        float order,
        float size,
        Offset offset = default,
        bool noSpacingAfterThis = false)
        where T : IUiElement
    {
        container.Add(order, objectToPlace, size, offset, noSpacingAfterThis);
        return objectToPlace;
    }

    public static T AddTo<T>(
        this T objectToPlace,
        StackContainer container,
        float order,
        Vector2 size,
        ContainerPosition position,
        Offset offset,
        bool noSpacingAfterThis = false)
        where T : IUiElement
    {
        container.Add(order, objectToPlace, new Vector2?(size), new ContainerPosition?(position), offset, noSpacingAfterThis);
        return objectToPlace;
    }

    public static T AppendTo<T>(
        this T objectToPlace,
        StackContainer container,
        float? size = null,
        Offset offset = default,
        bool noSpacingAfterThis = false)
        where T : IUiElement
    {
        container.Append(objectToPlace, size, offset, noSpacingAfterThis);
        return objectToPlace;
    }

    public static T AppendTo<T>(
        this T objectToPlace,
        StackContainer container,
        Vector2? size,
        ContainerPosition position,
        Offset offset = default,
        bool noSpacingAfterThis = false)
        where T : IUiElement
    {
        container.Append(objectToPlace, size, new ContainerPosition?(position), offset, noSpacingAfterThis);
        return objectToPlace;
    }

    public static T AppendTo<T>(this T objectToPlace, GridContainer container) where T : IUiElement
    {
        container.Append(objectToPlace);
        return objectToPlace;
    }

    public static void Destroy(this IUiElement element) => element.GameObject.Destroy();

    public static float GetHeight(this IUiElement element) => element.RectTransform.rect.height;

    public static Vector2 GetSize(this IUiElement element)
    {
        Rect rect = element.RectTransform.rect;
        double width = (double)rect.width;
        rect = element.RectTransform.rect;
        double height = (double)rect.height;
        return new Vector2((float)width, (float)height);
    }

    public static float GetWidth(this IUiElement element) => element.RectTransform.rect.width;

    public static T Hide<T>(this T element) where T : IUiElement
    {
        element.GameObject.SetActive(false);
        return element;
    }

    public static bool IsVisible(this IUiElement element) => element.GameObject.activeSelf;

    public static Dropdwn NewDropdown(this UiBuilder builder, string name)
    {
        return new Dropdwn(builder, name);
    }

    public static IconContainer NewIconContainer(this UiBuilder builder, string name)
    {
        return new IconContainer(builder, name);
    }

    public static TxtField NewTxtField(this UiBuilder builder, string name)
    {
        return new TxtField(builder, name);
    }

    public static T PlaceViaEnvelope<T>(this T objectToPlace, IUiElement parent, float aspectRatio) where T : IUiElement
    {
        LayoutHelper.Fill(parent.GameObject, objectToPlace.GameObject);
        AspectRatioFitter aspectRatioFitter = objectToPlace.GameObject.AddComponent<AspectRatioFitter>();
        aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.EnvelopeParent;
        aspectRatioFitter.aspectRatio = aspectRatio;
        return objectToPlace;
    }

    public static T PutRelativeTo<T>(
        this T objectToPlace,
        IUiElement parent,
        Vector2 size,
        HorizontalPosition horizontalPosition,
        VerticalPosition verticalPosition,
        Offset offset = default)
        where T : IUiElement
    {
        LayoutHelper.PositionRelative(parent.GameObject, objectToPlace.GameObject, size, horizontalPosition, verticalPosition, offset);
        return objectToPlace;
    }

    public static T PutTo<T>(this T objectToPlace, IUiElement parent, Offset offset = default) where T : IUiElement
    {
        LayoutHelper.Fill(parent.GameObject, objectToPlace.GameObject, offset);
        return objectToPlace;
    }

    public static T PutToBottomOf<T>(
        this T objectToPlace,
        IUiElement parent,
        float size,
        Offset offset = default)
        where T : IUiElement
    {
        LayoutHelper.FillHorizontally(parent.GameObject, objectToPlace.GameObject, size, VerticalPosition.Bottom, offset);
        return objectToPlace;
    }

    public static T PutToCenterBottomOf<T>(
        this T objectToPlace,
        IUiElement parent,
        Vector2 size,
        Offset offset = default)
        where T : IUiElement
    {
        LayoutHelper.PositionRelative(parent.GameObject, objectToPlace.GameObject, size, HorizontalPosition.Center, VerticalPosition.Bottom, offset);
        return objectToPlace;
    }

    public static T PutToCenterMiddleOf<T>(
        this T objectToPlace,
        IUiElement parent,
        Vector2 size,
        Offset offset = default)
        where T : IUiElement
    {
        LayoutHelper.PositionRelative(parent.GameObject, objectToPlace.GameObject, size, HorizontalPosition.Center, VerticalPosition.Middle, offset);
        return objectToPlace;
    }

    public static T PutToCenterOf<T>(
        this T objectToPlace,
        IUiElement parent,
        float size,
        Offset offset = default)
        where T : IUiElement
    {
        LayoutHelper.FillVertically(parent.GameObject, objectToPlace.GameObject, size, HorizontalPosition.Center, offset);
        return objectToPlace;
    }

    public static T PutToCenterTopOf<T>(
        this T objectToPlace,
        IUiElement parent,
        Vector2 size,
        Offset offset = default)
        where T : IUiElement
    {
        LayoutHelper.PositionRelative(parent.GameObject, objectToPlace.GameObject, size, HorizontalPosition.Center, VerticalPosition.Top, offset);
        return objectToPlace;
    }

    public static T PutToLeftBottomOf<T>(
        this T objectToPlace,
        IUiElement parent,
        Vector2 size,
        Offset offset = default)
        where T : IUiElement
    {
        LayoutHelper.PositionRelative(parent.GameObject, objectToPlace.GameObject, size, HorizontalPosition.Left, VerticalPosition.Bottom, offset);
        return objectToPlace;
    }

    public static T PutToLeftBottomOf<T>(
        this T objectToPlace,
        GameObject parent,
        Vector2 size,
        Offset offset = default)
        where T : IUiElement
    {
        LayoutHelper.PositionRelative(parent, objectToPlace.GameObject, size, HorizontalPosition.Left, VerticalPosition.Bottom, offset);
        return objectToPlace;
    }

    public static T PutToLeftMiddleOf<T>(
        this T objectToPlace,
        IUiElement parent,
        Vector2 size,
        Offset offset = default)
        where T : IUiElement
    {
        LayoutHelper.PositionRelative(parent.GameObject, objectToPlace.GameObject, size, HorizontalPosition.Left, VerticalPosition.Middle, offset);
        return objectToPlace;
    }

    public static T PutToLeftOf<T>(
        this T objectToPlace,
        IUiElement parent,
        float size,
        Offset offset = default)
        where T : IUiElement
    {
        LayoutHelper.FillVertically(parent.GameObject, objectToPlace.GameObject, size, HorizontalPosition.Left, offset);
        return objectToPlace;
    }

    public static T PutToLeftTopOf<T>(
        this T objectToPlace,
        IUiElement parent,
        Vector2 size,
        Offset offset = default)
        where T : IUiElement
    {
        LayoutHelper.PositionRelative(parent.GameObject, objectToPlace.GameObject, size, HorizontalPosition.Left, VerticalPosition.Top, offset);
        return objectToPlace;
    }

    public static T PutToLeftTopOf<T>(
        this T objectToPlace,
        GameObject parent,
        Vector2 size,
        Offset offset = default)
        where T : IUiElement
    {
        LayoutHelper.PositionRelative(parent, objectToPlace.GameObject, size, HorizontalPosition.Left, VerticalPosition.Top, offset);
        return objectToPlace;
    }

    public static T PutToMiddleOf<T>(
        this T objectToPlace,
        IUiElement parent,
        float size,
        Offset offset = default)
        where T : IUiElement
    {
        LayoutHelper.FillHorizontally(parent.GameObject, objectToPlace.GameObject, size, VerticalPosition.Middle, offset);
        return objectToPlace;
    }

    public static T PutToRightBottomOf<T>(
        this T objectToPlace,
        IUiElement parent,
        Vector2 size,
        Offset offset = default)
        where T : IUiElement
    {
        LayoutHelper.PositionRelative(parent.GameObject, objectToPlace.GameObject, size, HorizontalPosition.Right, VerticalPosition.Bottom, offset);
        return objectToPlace;
    }

    public static T PutToRightMiddleOf<T>(
        this T objectToPlace,
        IUiElement parent,
        Vector2 size,
        Offset offset = default)
        where T : IUiElement
    {
        LayoutHelper.PositionRelative(parent.GameObject, objectToPlace.GameObject, size, HorizontalPosition.Right, VerticalPosition.Middle, offset);
        return objectToPlace;
    }

    public static T PutToRightOf<T>(
        this T objectToPlace,
        IUiElement parent,
        float size,
        Offset offset = default)
        where T : IUiElement
    {
        LayoutHelper.FillVertically(parent.GameObject, objectToPlace.GameObject, size, HorizontalPosition.Right, offset);
        return objectToPlace;
    }

    public static T PutToRightTopOf<T>(
        this T objectToPlace,
        IUiElement parent,
        Vector2 size,
        Offset offset = default)
        where T : IUiElement
    {
        LayoutHelper.PositionRelative(parent.GameObject, objectToPlace.GameObject, size, HorizontalPosition.Right, VerticalPosition.Top, offset);
        return objectToPlace;
    }

    public static T PutToTopOf<T>(
        this T objectToPlace,
        IUiElement parent,
        float size,
        Offset offset = default)
        where T : IUiElement
    {
        LayoutHelper.FillHorizontally(parent.GameObject, objectToPlace.GameObject, size, VerticalPosition.Top, offset);
        return objectToPlace;
    }

    public static T SendToBack<T>(this T element) where T : IUiElement
    {
        element.GameObject.transform.SetAsFirstSibling();
        return element;
    }

    public static T SendToFront<T>(this T element) where T : IUiElement
    {
        element.GameObject.transform.SetAsLastSibling();
        return element;
    }

    public static T SetAnchoredPosition<T>(this T element, Vector2 position) where T : IUiElement
    {
        element.RectTransform.anchoredPosition = position;
        return element;
    }

    public static T SetHeight<T>(this T element, float height) where T : IUiElement
    {
        element.RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        return element;
    }

    public static T SetLocalPosition<T>(this T element, Vector3 position) where T : IUiElement
    {
        element.RectTransform.localPosition = position;
        return element;
    }

    public static T SetParent<T>(this T element, IUiElement parent, bool worldPositionStays = true) where T : IUiElement
    {
        element.RectTransform.SetParent(parent.RectTransform, worldPositionStays);
        return element;
    }

    public static T SetPivot<T>(this T element, Vector2 pivot) where T : IUiElement
    {
        element.RectTransform.pivot = pivot;
        return element;
    }

    public static T SetPosition<T>(this T element, Vector3 position) where T : IUiElement
    {
        element.RectTransform.position = position;
        return element;
    }

    public static T SetScale<T>(this T element, Vector2 scale) where T : IUiElement
    {
        element.RectTransform.localScale = new Vector3(scale.x, scale.y, 0.0f);
        return element;
    }

    public static T SetSize<T>(this T element, Vector2 size) where T : IUiElement
    {
        element.RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
        element.RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
        return element;
    }

    public static T SetVisibility<T>(this T element, bool visibility) where T : IUiElement
    {
        element.GameObject.SetActive(visibility);
        return element;
    }

    public static T SetWidth<T>(this T element, float width) where T : IUiElement
    {
        element.RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        return element;
    }

    public static T Show<T>(this T element) where T : IUiElement
    {
        element.GameObject.SetActive(true);
        return element;
    }

    public static T ToggleVisibility<T>(this T element) where T : IUiElement
    {
        element.GameObject.SetActive(!element.GameObject.activeSelf);
        return element;
    }

    internal static Dropdwn NewDropdown(this UiBuilder builder, string name, IUiElement parent)
    {
        return new Dropdwn(builder, name, parent?.GameObject);
    }

    internal static IconContainer NewIconContainer(
        this UiBuilder builder,
        string name,
        IUiElement parent)
    {
        return new IconContainer(builder, name, parent.GameObject);
    }

    internal static TxtField NewTxtField(this UiBuilder builder, string name, IUiElement parent)
    {
        return new TxtField(builder, name, parent?.GameObject);
    }
}

internal static class UIBuilderTxtExtensions
{
    public static Txt NewTitle(this UiBuilder builder, string name)
    {
        return new Txt(builder, name).SetTextStyle(builder.Style.Global.Title).SetAlignment(TextAnchor.MiddleLeft);
    }

    public static Txt NewTitle(this UiBuilder builder, LocStr name)
    {
        return new Txt(builder, "title").SetText((LocStrFormatted)name).SetTextStyle(builder.Style.Global.Title).SetAlignment(TextAnchor.MiddleLeft);
    }

    public static Txt NewTitleBig(this UiBuilder builder, string name)
    {
        return new Txt(builder, name).SetTextStyle(builder.Style.Global.TitleBig).SetAlignment(TextAnchor.MiddleLeft);
    }

    public static Txt NewTxt(this UiBuilder builder, string name) => new Txt(builder, name);

    internal static Txt NewTitle(this UiBuilder builder, string name, IUiElement parent)
    {
        return new Txt(builder, name, parent?.GameObject).SetTextStyle(builder.Style.Global.Title).SetAlignment(TextAnchor.MiddleLeft);
    }

    internal static Txt NewTitle(this UiBuilder builder, LocStr name, IUiElement parent)
    {
        return new Txt(builder, "title", parent?.GameObject).SetText((LocStrFormatted)name).SetTextStyle(builder.Style.Global.Title).SetAlignment(TextAnchor.MiddleLeft);
    }

    internal static Txt NewTitleBigCentered(this UiBuilder builder, IUiElement parent)
    {
        return new Txt(builder, "Title", parent?.GameObject).SetTextStyle(builder.Style.Global.TitleBig).SetAlignment(TextAnchor.MiddleCenter);
    }

    internal static Txt NewTxt(this UiBuilder builder, string name, IUiElement parent)
    {
        return new Txt(builder, name, parent?.GameObject);
    }
}