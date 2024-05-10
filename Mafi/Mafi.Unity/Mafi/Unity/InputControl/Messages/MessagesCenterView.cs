// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Messages.MessagesCenterView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Input;
using Mafi.Core.Messages;
using Mafi.Core.Prototypes;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Messages
{
  public class MessagesCenterView : WindowView
  {
    private readonly IInputScheduler m_inputScheduler;
    private readonly MessagesManager m_messagesManager;
    private readonly LazyResolve<MessagesCenterController> m_controller;
    private readonly Lyst<KeyValuePair<Message, bool>> m_toCreate;
    private readonly Dict<Message, MessagesCenterView.MessageView> m_messages;
    private readonly Dict<Message, MessagesCenterView.MessageTitleView> m_titles;
    private StackContainer m_titlesContainer;
    private ScrollableContainer m_scrollableMessageContainer;
    private Option<Message> m_selectedMessage;
    private readonly Dict<MessageGroupProto, MessagesCenterView.MessageGroupView> m_categoryLabels;
    private readonly Dict<MessageGroupProto, Lyst<MessagesCenterView.MessageTitleView>> m_messagesPerGroup;

    public MessagesCenterView(
      LazyResolve<MessagesCenterController> controller,
      IInputScheduler inputScheduler,
      MessagesManager messagesManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_toCreate = new Lyst<KeyValuePair<Message, bool>>();
      this.m_messages = new Dict<Message, MessagesCenterView.MessageView>();
      this.m_titles = new Dict<Message, MessagesCenterView.MessageTitleView>();
      this.m_categoryLabels = new Dict<MessageGroupProto, MessagesCenterView.MessageGroupView>();
      this.m_messagesPerGroup = new Dict<MessageGroupProto, Lyst<MessagesCenterView.MessageTitleView>>();
      // ISSUE: explicit constructor call
      base.\u002Ector("MessagesCenter");
      this.m_inputScheduler = inputScheduler;
      this.m_messagesManager = messagesManager;
      this.m_controller = controller;
      this.ShowAfterSync = true;
    }

    protected override void BuildWindowContent()
    {
      this.SetTitle((LocStrFormatted) Tr.MessageCenter__Title);
      int num = 300;
      int allOffsets = 10;
      this.SetContentBg((ColorRgba) 3487029);
      this.SetContentSize(new Vector2((float) (num + MessagesCenterView.MessageView.WIDTH + 2 * allOffsets), this.ResolveWindowSize().y));
      this.PositionSelfToCenter(true);
      Txt txt = this.Builder.NewTxt("Messages", (IUiElement) this.GetContentPanel()).SetText((LocStrFormatted) Tr.MessageCenter__MessagesTitle);
      TextStyle title = this.Builder.Style.Global.Title;
      ref TextStyle local = ref title;
      int? nullable = new int?(14);
      ColorRgba? color = new ColorRgba?();
      FontStyle? fontStyle = new FontStyle?();
      int? fontSize = nullable;
      bool? isCapitalized = new bool?();
      TextStyle textStyle = local.Extend(color, fontStyle, fontSize, isCapitalized);
      txt.SetTextStyle(textStyle).SetAlignment(TextAnchor.MiddleCenter).PutToLeftTopOf<Txt>((IUiElement) this.GetContentPanel(), new Vector2((float) num, 40f));
      ScrollableContainer leftOf = this.Builder.NewScrollableContainer("ScrollableTitles", (IUiElement) this.GetContentPanel()).AddVerticalScrollbar().PutToLeftOf<ScrollableContainer>((IUiElement) this.GetContentPanel(), (float) (num - 20), Offset.LeftRight(10f) + Offset.Top(40f));
      this.m_titlesContainer = this.Builder.NewStackContainer("TitlesContainer", (IUiElement) this.GetContentPanel()).SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.Dynamic).SetItemSpacing(2f).PutToTopOf<StackContainer>((IUiElement) this.GetContentPanel(), 0.0f);
      leftOf.AddItemTop((IUiElement) this.m_titlesContainer);
      this.m_scrollableMessageContainer = this.Builder.NewScrollableContainer("ScrollableMessage", (IUiElement) this.GetContentPanel()).AddVerticalScrollbar().OnClick(new Action(this.onMessageViewportClick)).PutTo<ScrollableContainer>((IUiElement) this.GetContentPanel(), Offset.Left((float) num) + Offset.All((float) allOffsets));
      this.Builder.NewPanel("MessageBg", (IUiElement) this.m_scrollableMessageContainer).SetBackground((ColorRgba) 3026478).PutTo<Panel>((IUiElement) this.m_scrollableMessageContainer).SendToBack<Panel>();
      this.OnShowStart += (Action) (() =>
      {
        if (this.m_selectedMessage.IsNone && this.m_titles.IsNotEmpty)
          this.OpenMessage(this.m_titles.Keys.FirstOrDefault<Message>((Func<Message, bool>) (x => x.Proto.Id.Value == "MessageWelcome")) ?? this.m_titles.Keys.First<Message>());
        this.Builder.GameOverlay.Show<Panel>();
      });
      this.OnHide += (Action) (() => this.Builder.GameOverlay.Hide<Panel>());
    }

    protected override Option<IUiElement> GetParent(UiBuilder builder)
    {
      return (Option<IUiElement>) (IUiElement) builder.GameOverlay;
    }

    public void InitializeMessages()
    {
      foreach (Message allMessage in this.m_messagesManager.AllMessages)
      {
        if (!allMessage.Proto.IsObsolete)
          this.addMessage(allMessage, false);
      }
      this.m_messagesManager.OnNewMessageForUi += new Action<Message, bool>(this.onNewMessage);
    }

    private void onNewMessage(Message message, bool openInUi)
    {
      this.m_toCreate.Add(Make.Kvp<Message, bool>(message, openInUi));
    }

    private void onMessageViewportClick()
    {
      if (!this.m_selectedMessage.HasValue)
        return;
      this.m_messages[this.m_selectedMessage.Value].OnClick();
    }

    private void onGroupClick(MessagesCenterView.MessageGroupView group)
    {
      group.SetIsMinimized(!group.IsMinimized);
      this.rebuildMenu();
    }

    private void addMessage(Message message, bool openInUi)
    {
      Assert.That<Dict<Message, MessagesCenterView.MessageTitleView>>(this.m_titles).NotContainsKey<Message, MessagesCenterView.MessageTitleView>(message);
      MessagesCenterView.MessageTitleView title = new MessagesCenterView.MessageTitleView((IUiElement) this.m_titlesContainer, this.Builder, message, new Action<Message>(this.OpenMessage), new Action(this.updateReadStatus));
      this.m_titles.Add(message, title);
      if (message.Proto.Group.HasValue)
      {
        MessageGroupProto key = message.Proto.Group.Value;
        this.m_categoryLabels.GetOrAdd<MessageGroupProto, MessagesCenterView.MessageGroupView>(key, (Func<MessageGroupProto, MessagesCenterView.MessageGroupView>) (g => new MessagesCenterView.MessageGroupView((IUiElement) this.m_titlesContainer, this.Builder, g.Strings.Name, new Action<MessagesCenterView.MessageGroupView>(this.onGroupClick)))).AddTutorial(title);
        this.m_messagesPerGroup.GetOrAdd<MessageGroupProto, Lyst<MessagesCenterView.MessageTitleView>>(key, (Func<MessageGroupProto, Lyst<MessagesCenterView.MessageTitleView>>) (_ => new Lyst<MessagesCenterView.MessageTitleView>())).Add(title);
      }
      this.rebuildMenu();
      this.updateReadStatus();
      if (!openInUi)
        return;
      this.OpenMessage(message);
      this.m_controller.Value.ForceOpen();
    }

    private MessagesCenterView.MessageView getOrBuildMessage(Message message)
    {
      MessagesCenterView.MessageView orBuildMessage;
      if (this.m_messages.TryGetValue(message, out orBuildMessage))
        return orBuildMessage;
      MessagesCenterView.MessageView child = new MessagesCenterView.MessageView((IUiElement) this.m_scrollableMessageContainer.Viewport, this.Builder, message);
      this.m_messages.Add(message, child);
      this.m_scrollableMessageContainer.AddItemTop((IUiElement) child);
      return child;
    }

    public void ForceOpenMessageIfNotRead(MessageProto messageProto)
    {
      Message message = this.m_messages.Keys.FirstOrDefault<Message>((Func<Message, bool>) (x => (Proto) x.Proto == (Proto) messageProto));
      if (message == null || message.IsRead)
        return;
      this.OpenMessage(message);
      this.m_controller.Value.ForceOpen();
    }

    private void updateReadStatus()
    {
      this.m_controller.Value.SetMessagesReadStatus(this.m_titles.Values.All<MessagesCenterView.MessageTitleView>((Func<MessagesCenterView.MessageTitleView, bool>) (x => x.IsRead)));
    }

    private void rebuildMenu()
    {
      this.m_titlesContainer.ClearAll(true);
      this.m_titlesContainer.StartBatchOperation();
      foreach (KeyValuePair<Message, MessagesCenterView.MessageTitleView> title in this.m_titles)
      {
        if (!title.Key.Proto.Group.HasValue)
        {
          title.Value.Show<MessagesCenterView.MessageTitleView>();
          this.m_titlesContainer.Append((IUiElement) title.Value, new float?(30f));
        }
      }
      foreach (KeyValuePair<MessageGroupProto, MessagesCenterView.MessageGroupView> keyValuePair in (IEnumerable<KeyValuePair<MessageGroupProto, MessagesCenterView.MessageGroupView>>) this.m_categoryLabels.OrderBy<KeyValuePair<MessageGroupProto, MessagesCenterView.MessageGroupView>, int>((Func<KeyValuePair<MessageGroupProto, MessagesCenterView.MessageGroupView>, int>) (x => x.Key.Order)))
      {
        keyValuePair.Value.Show<MessagesCenterView.MessageGroupView>();
        this.m_titlesContainer.Append((IUiElement) keyValuePair.Value, new float?(28f));
        if (!keyValuePair.Value.IsMinimized)
        {
          foreach (MessagesCenterView.MessageTitleView element in this.m_messagesPerGroup[keyValuePair.Key])
          {
            element.Show<MessagesCenterView.MessageTitleView>();
            this.m_titlesContainer.Append((IUiElement) element, new float?(28f));
          }
        }
      }
      this.m_titlesContainer.FinishBatchOperation();
    }

    public bool OpenMessage(Proto.ID messageId)
    {
      KeyValuePair<Message, MessagesCenterView.MessageTitleView> keyValuePair = this.m_titles.FirstOrDefault<KeyValuePair<Message, MessagesCenterView.MessageTitleView>>((Func<KeyValuePair<Message, MessagesCenterView.MessageTitleView>, bool>) (x => x.Key.Proto.Id == messageId));
      if (keyValuePair.Key != null)
      {
        this.OpenMessage(keyValuePair.Key);
        return true;
      }
      this.m_messagesManager.AddMessageFromRenderThread(messageId, true);
      return false;
    }

    public void OpenMessage(Message message)
    {
      if (this.m_selectedMessage.HasValue)
      {
        this.m_messages[this.m_selectedMessage.Value].Hide<MessagesCenterView.MessageView>();
        this.m_titles[this.m_selectedMessage.Value].Deselect();
      }
      MessagesCenterView.MessageView orBuildMessage = this.getOrBuildMessage(message);
      this.m_scrollableMessageContainer.SetContentToScroll((IUiElement) orBuildMessage);
      this.m_titles[message].Select();
      orBuildMessage.Show<MessagesCenterView.MessageView>();
      this.m_selectedMessage = (Option<Message>) message;
      this.SetTitle(message.Title);
      if (message.IsRead)
        return;
      this.m_inputScheduler.ScheduleInputCmd<MarkMessageAsReadCmd>(new MarkMessageAsReadCmd(message.Proto.Id));
    }

    public override void SyncUpdate(GameTime gameTime)
    {
      base.SyncUpdate(gameTime);
      this.m_toCreate.ForEachAndClear((Action<KeyValuePair<Message, bool>>) (x => this.addMessage(x.Key, x.Value)));
    }

    private class MessageTitleView : IUiElement
    {
      private readonly Btn m_btn;
      private readonly IconContainer m_newIcon;
      private readonly IconContainer m_selectedIcon;
      private readonly UiBuilder m_builder;
      private readonly Action m_onReadChange;

      public GameObject GameObject => this.m_btn.GameObject;

      public RectTransform RectTransform => this.m_btn.RectTransform;

      public bool IsRead { get; private set; }

      public MessageTitleView(
        IUiElement parent,
        UiBuilder builder,
        Message message,
        Action<Message> onClick,
        Action onReadChange)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_builder = builder;
        this.m_onReadChange = onReadChange;
        bool hasValue = message.Proto.Group.HasValue;
        this.m_btn = builder.NewBtn("Title", parent).SetText(hasValue ? string.Format("          {0}", (object) message.Title) : "    " + message.Title.Value).SetButtonStyle(builder.Style.Global.ListMenuBtn).SetTextAlignment(TextAnchor.MiddleLeft).OnClick((Action) (() => onClick(message)));
        this.m_newIcon = builder.NewIconContainer("New").SetIcon("Assets/Unity/UserInterface/General/Circle.svg", (ColorRgba) 16756491).PutToLeftMiddleOf<IconContainer>((IUiElement) this.m_btn, 8.Vector2(), Offset.Left(hasValue ? 25f : 5f));
        this.m_newIcon.SetVisibility<IconContainer>(!message.IsRead);
        this.IsRead = message.IsRead;
        this.m_selectedIcon = builder.NewIconContainer("Selected").SetIcon("Assets/Unity/UserInterface/General/ResArrow128.png", (ColorRgba) 16777215).PutToLeftMiddleOf<IconContainer>((IUiElement) this.m_btn, 14.Vector2(), Offset.Left(hasValue ? 25f : 5f));
        this.m_selectedIcon.Hide<IconContainer>();
      }

      public void Select()
      {
        this.m_newIcon.Hide<IconContainer>();
        this.m_selectedIcon.Show<IconContainer>();
        this.m_btn.SetButtonStyle(this.m_builder.Style.Global.ListMenuBtnSelected);
        this.m_btn.SetEnabled(false);
        if (this.IsRead)
          return;
        this.IsRead = true;
        Action onReadChange = this.m_onReadChange;
        if (onReadChange == null)
          return;
        onReadChange();
      }

      public void Deselect()
      {
        this.m_btn.SetEnabled(true);
        this.m_selectedIcon.Hide<IconContainer>();
        this.m_btn.SetButtonStyle(this.m_builder.Style.Global.ListMenuBtn);
      }
    }

    private class MessageGroupView : IUiElement
    {
      private readonly LocStr m_title;
      private readonly Btn m_btn;
      private int m_tutorialsNested;
      private readonly IconContainer m_newIcon;
      private Lyst<MessagesCenterView.MessageTitleView> m_titles;

      public GameObject GameObject => this.m_btn.GameObject;

      public RectTransform RectTransform => this.m_btn.RectTransform;

      public bool IsMinimized { get; private set; }

      public MessageGroupView(
        IUiElement parent,
        UiBuilder builder,
        LocStr title,
        Action<MessagesCenterView.MessageGroupView> onClick)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.m_titles = new Lyst<MessagesCenterView.MessageTitleView>();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        MessagesCenterView.MessageGroupView messageGroupView = this;
        this.m_title = title;
        this.m_btn = builder.NewBtn("Title", parent).SetText(string.Format("    {0}", (object) this.m_title)).SetButtonStyle(builder.Style.Global.ListMenuGroupBtn).SetTextAlignment(TextAnchor.MiddleLeft).OnClick((Action) (() => onClick(messageGroupView)));
        this.m_newIcon = builder.NewIconContainer("New").SetIcon("Assets/Unity/UserInterface/General/Circle.svg", (ColorRgba) 16756491).PutToLeftMiddleOf<IconContainer>((IUiElement) this.m_btn, 8.Vector2(), Offset.Left(5f)).Hide<IconContainer>();
      }

      public void AddTutorial(MessagesCenterView.MessageTitleView title)
      {
        this.m_titles.Add(title);
        ++this.m_tutorialsNested;
        this.SetIsMinimized(false);
      }

      public void SetIsMinimized(bool isMinimized)
      {
        this.IsMinimized = isMinimized;
        this.m_btn.SetText(string.Format("    {0}", (object) this.m_title) + (this.IsMinimized ? string.Format(" ({0})", (object) this.m_titles.Count) : ""));
        this.m_newIcon.SetVisibility<IconContainer>(isMinimized && this.m_titles.Any<MessagesCenterView.MessageTitleView>((Predicate<MessagesCenterView.MessageTitleView>) (x => !x.IsRead)));
      }
    }

    private class MessageView : IUiElement
    {
      public static int WIDTH;
      private readonly StackContainer m_content;
      private readonly Lyst<IUiElement> m_elementsToShow;
      private readonly Txt m_clickForMoreInfo;

      public GameObject GameObject => this.m_content.GameObject;

      public RectTransform RectTransform => this.m_content.RectTransform;

      public MessageView(IUiElement parent, UiBuilder builder, Message message)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.m_elementsToShow = new Lyst<IUiElement>();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_content = builder.NewStackContainer("Content", parent).SetSizeMode(StackContainer.SizeMode.Dynamic).SetStackingDirection(StackContainer.Direction.TopToBottom).SetInnerPadding(Offset.LeftRight(20f) + Offset.Bottom(100f) + Offset.Top(20f));
        int num1 = 0;
        Regex regex = new Regex(" width=([0-9]+)");
        int num2 = 0;
        Option<Txt> option = Option<Txt>.None;
        this.m_content.StartBatchOperation();
        int? nullable1;
        foreach (string str1 in message.Content)
        {
          string str2 = str1;
          Offset offset1 = Offset.Bottom(15f);
          bool flag1 = false;
          TextStyle text1 = builder.Style.Global.Text;
          ref TextStyle local1 = ref text1;
          nullable1 = new int?(16);
          ColorRgba? color1 = new ColorRgba?();
          FontStyle? nullable2 = new FontStyle?();
          FontStyle? fontStyle1 = nullable2;
          int? fontSize1 = nullable1;
          bool? isCapitalized1 = new bool?();
          TextStyle textStyle = local1.Extend(color1, fontStyle1, fontSize1, isCapitalized1);
          if (str2.StartsWith("<IMAGE=") || str2.StartsWith("<DIMAGE="))
          {
            bool flag2 = false;
            string str3;
            if (str2.StartsWith("<IMAGE="))
            {
              str3 = str2.Substring("<IMAGE=".Length, str2.Length - "<IMAGE=".Length - 1);
            }
            else
            {
              str3 = str2.Substring("<DIMAGE=".Length, str2.Length - "<DIMAGE=".Length - 1);
              flag2 = true;
            }
            Match match = regex.Match(str3);
            float x;
            if (match.Success && match.Groups.Count == 2)
            {
              int result;
              x = !int.TryParse(match.Groups[1].Value, out result) ? (float) (MessagesCenterView.MessageView.WIDTH - 60) : (float) result;
              str3 = str3.Substring(0, str3.Length - match.Length);
            }
            else
              x = (float) (MessagesCenterView.MessageView.WIDTH - 60);
            Sprite sharedSprite = builder.AssetsDb.GetSharedSprite(str3);
            float num3 = x / (float) sharedSprite.texture.width;
            float y1 = (float) sharedSprite.texture.height * num3;
            float bottomOffset = 15f;
            if (option.HasValue)
            {
              Panel panel = builder.NewPanel("Frame", (IUiElement) this.m_content).SetBackground(ColorRgba.Black);
              option.Value.PutTo<Txt>((IUiElement) panel, Offset.All(10f));
              float y2 = option.Value.GetPreferedHeight(x - 20f) + 20f;
              panel.AppendTo<Panel>(this.m_content, new Vector2?(new Vector2(x, y2)), ContainerPosition.MiddleOrCenter);
              bottomOffset += 5f;
            }
            option = Option<Txt>.None;
            IconContainer iconContainer = builder.NewIconContainer("Image", (IUiElement) this.m_content).SetIcon(sharedSprite).AppendTo<IconContainer>(this.m_content, new Vector2?(new Vector2(x, y1)), ContainerPosition.MiddleOrCenter, Offset.Bottom(bottomOffset));
            if (flag2)
              iconContainer.SetColor((ColorRgba) 14737632);
          }
          else
          {
            Offset offset2;
            if (str2.StartsWith("<h>"))
            {
              str2 = str2.Substring("<h>".Length, str2.Length - "<h>".Length - "</h>".Length);
              ref TextStyle local2 = ref textStyle;
              nullable1 = new int?(18);
              nullable2 = new FontStyle?(FontStyle.Bold);
              ColorRgba? color2 = new ColorRgba?();
              FontStyle? fontStyle2 = nullable2;
              int? fontSize2 = nullable1;
              bool? isCapitalized2 = new bool?();
              textStyle = local2.Extend(color2, fontStyle2, fontSize2, isCapitalized2);
              offset2 = Offset.Bottom(4f);
            }
            else
              offset2 = offset1 + Offset.LeftRight(20f);
            if (flag1)
              ++num1;
            bool flag3 = false;
            if (str2.StartsWith("[STEP]"))
            {
              ++num2;
              str2 = str2.Replace("[STEP]", string.Format("{0})", (object) num2));
              flag3 = true;
            }
            else if (str2.StartsWith("[ATTACH]"))
            {
              str2 = str2.Replace("[ATTACH] ", "");
              flag3 = true;
            }
            string text2 = str2.Replace("<b>", "<color=#d9ae5b><b>").Replace("</b>", "</b></color>").Replace("<bc>", "<color=#d9ae5b><b>").Replace("</bc>", "</b></color>").Replace("<br>", Environment.NewLine);
            int num4 = 0;
            int startIndex1 = 0;
            for (; num4 < 100; ++num4)
            {
              int startIndex2 = text2.IndexOf("<KB=", startIndex1, StringComparison.Ordinal);
              startIndex1 = startIndex2;
              if (startIndex2 >= 0)
              {
                int num5 = text2.IndexOf(">", startIndex2, StringComparison.Ordinal);
                string oldValue = text2.Substring(startIndex2, num5 - startIndex2 + 1);
                KeyBindings byId = ShortcutsStorage.FindById(text2.Substring(startIndex2 + "<KB=".Length, num5 - (startIndex2 + "<KB=".Length)));
                text2 = !byId.IsEmpty ? text2.Replace(oldValue, "<color=#d9ae5b><b>" + byId.ToNiceStringLong() + "</b></color>") : text2.Replace(oldValue, "<b><key not mapped></b>");
              }
              else
                break;
            }
            Txt element = builder.NewTxt("Text", (IUiElement) this.m_content).SetAlignment(TextAnchor.UpperLeft).SetTextStyle(textStyle).EnableRichText().SetText(text2);
            if (flag3)
              option = (Option<Txt>) element;
            else
              this.m_content.Append((IUiElement) element, new float?(element.GetPreferedHeight((float) (MessagesCenterView.MessageView.WIDTH - 40) - offset2.LeftRightOffset)), offset2);
            if (num1 > 1 && !message.IsRead)
            {
              this.m_content.HideItem((IUiElement) element);
              this.m_elementsToShow.Add((IUiElement) element);
            }
          }
        }
        if (this.m_elementsToShow.IsNotEmpty)
        {
          Txt txt = builder.NewTxt("Text", (IUiElement) this.m_content).SetText("<< Click anywhere to continue >>").SetAlignment(TextAnchor.MiddleCenter);
          TextStyle text = builder.Style.Global.Text;
          ref TextStyle local = ref text;
          nullable1 = new int?(14);
          ColorRgba? color = new ColorRgba?(builder.Style.Global.OrangeText);
          FontStyle? fontStyle = new FontStyle?();
          int? fontSize = nullable1;
          bool? isCapitalized = new bool?();
          TextStyle textStyle = local.Extend(color, fontStyle, fontSize, isCapitalized);
          this.m_clickForMoreInfo = txt.SetTextStyle(textStyle).AppendTo<Txt>(this.m_content, new float?(40f));
        }
        this.m_content.FinishBatchOperation();
        this.Hide<MessagesCenterView.MessageView>();
        this.SetSize<MessagesCenterView.MessageView>(new Vector2(800f, this.m_content.GetDynamicHeight() + 100f));
      }

      public void OnClick()
      {
        if (this.m_elementsToShow.IsEmpty)
          return;
        this.m_content.ShowItem(this.m_elementsToShow.First);
        this.m_elementsToShow.RemoveAt(0);
        if (!this.m_elementsToShow.IsEmpty)
          return;
        this.m_content.HideItem((IUiElement) this.m_clickForMoreInfo);
      }

      static MessageView()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        MessagesCenterView.MessageView.WIDTH = 750;
      }
    }
  }
}
