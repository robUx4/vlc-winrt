﻿using System;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace VLC_WinRT.Controls
{
    public delegate void FlyoutCloseRequested(object sender, EventArgs e);
    
    [TemplatePart(Name = TopBarContentPresenterName, Type = typeof(ContentPresenter))]
    [TemplatePart(Name = InformationContentPresenterName, Type = typeof(ContentPresenter))]
    [TemplatePart(Name = ContentPresenterName, Type = typeof(ContentPresenter))]
    [TemplatePart(Name = RightFlyoutContentPresenterName, Type = typeof(ContentPresenter))]
    [TemplatePart(Name = RightFlyoutFadeInName, Type = typeof(Storyboard))]
    [TemplatePart(Name = RightFlyoutFadeOutName, Type = typeof(Storyboard))]
    [TemplatePart(Name = RightFlyoutPlaneProjectionName, Type = typeof(PlaneProjection))]
    [TemplatePart(Name = RightFlyoutGridContainerName, Type = typeof(Grid))]
    [TemplatePart(Name = FlyoutBackgroundGridName, Type = typeof(Grid))]
    [TemplatePart(Name = FooterContentPresenterName, Type = typeof(ContentPresenter))]
    public sealed class SplitShell : Control
    {
        public event FlyoutCloseRequested FlyoutCloseRequested;
        public TaskCompletionSource<bool> TemplateApplied = new TaskCompletionSource<bool>();
        
        private const string ContentPresenterName = "ContentPresenter";
        private const string TopBarContentPresenterName = "TopBarContentPresenter";
        private const string InformationContentPresenterName = "InformationContentPresenter";
        private const string RightFlyoutContentPresenterName = "RightFlyoutContentPresenter";
        private const string RightFlyoutFadeInName = "RightFlyoutFadeIn";
        private const string RightFlyoutFadeOutName = "RightFlyoutFadeOut";
        private const string RightFlyoutPlaneProjectionName = "RightFlyoutPlaneProjection";
        private const string RightFlyoutGridContainerName = "RightFlyoutGridContainer";
        private const string FlyoutBackgroundGridName = "FlyoutBackgroundGrid";
        private const string FooterContentPresenterName = "FooterContentPresenter";
        
        private Grid _rightFlyoutGridContainer;
        private Grid _flyoutBackgroundGrid;
        private ContentPresenter _contentPresenter;
        private ContentPresenter _topBarContentPresenter;
        private ContentPresenter _rightFlyoutContentPresenter;
        private ContentPresenter _footerContentPresenter;

        private PlaneProjection _rightFlyoutPlaneProjection;
        private Storyboard _rightFlyoutFadeIn;
        private Storyboard _rightFlyoutFadeOut;
        private ContentPresenter _informationGrid;
        private TextBlock _informationTextBlock;
        
        public async void SetContentPresenter(object contentPresenter)
        {
            await TemplateApplied.Task;
            _contentPresenter.Content = contentPresenter;
        }
        
        public async void SetTopbarContentPresenter(object contentPresenter)
        {
            await TemplateApplied.Task;
            _topBarContentPresenter.Content = contentPresenter;
        }

        public async void SetInformationContent(object contentPresenter)
        {
            await TemplateApplied.Task;
            _informationGrid.Content = contentPresenter;
        }

        public async void SetRightPaneContentPresenter(object content)
        {
            await TemplateApplied.Task;
            if(IsRightFlyoutOpen)
            {
                HideFlyout();
                await Task.Delay(400);
            }
            _rightFlyoutContentPresenter.Content = content;
            ShowFlyout();
        }

        public async void SetFooterContentPresenter(object content)
        {
            await TemplateApplied.Task;
            _footerContentPresenter.Content = content;
        }

        #region Content Property
        public DependencyObject Content
        {
            get { return (DependencyObject)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
            "Content", typeof(DependencyObject), typeof(SplitShell), new PropertyMetadata(default(DependencyObject), ContentPropertyChangedCallback));


        private static void ContentPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var that = (SplitShell)dependencyObject;
            that.SetContentPresenter(dependencyPropertyChangedEventArgs.NewValue);
        }
        #endregion

        #region TopBarContent Property
        public DependencyObject TopBarContent
        {
            get { return (DependencyObject)GetValue(TopBarContentProperty); }
            set { SetValue(TopBarContentProperty, value); }
        }

        public static readonly DependencyProperty TopBarContentProperty = DependencyProperty.Register(
            "TopBarContent", typeof(DependencyObject), typeof(SplitShell), new PropertyMetadata(default(DependencyObject), TopBarContentPropertyChangedCallback));


        private static void TopBarContentPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var that = (SplitShell)dependencyObject;
            that.SetTopbarContentPresenter(dependencyPropertyChangedEventArgs.NewValue);
        }
        #endregion

        #region RightPaneContent Property

        public DependencyObject RightFlyoutContent
        {
            get { return (DependencyObject)GetValue(RightFlyoutContentProperty); }
            set { SetValue(RightFlyoutContentProperty, value); }
        }

        public static readonly DependencyProperty RightFlyoutContentProperty = DependencyProperty.Register(
            "RightFlyoutContent", typeof(DependencyObject), typeof(SplitShell),
            new PropertyMetadata(default(DependencyObject), RightFlyoutContentPropertyChangedCallback));

        private static void RightFlyoutContentPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var that = (SplitShell)dependencyObject;
            that.SetRightPaneContentPresenter(dependencyPropertyChangedEventArgs.NewValue);
        }
        #endregion

        #region FooterContent Property

        public DependencyObject FooterContent
        {
            get { return (DependencyObject)GetValue(FooterContentProperty); }
            set { SetValue(FooterContentProperty, value); }
        }

        public static readonly DependencyProperty FooterContentProperty = DependencyProperty.Register(
            "FooterContent", typeof(DependencyObject), typeof(SplitShell),
            new PropertyMetadata(default(DependencyObject), FooterContentPropertyChangedCallback));

        private static void FooterContentPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var that = (SplitShell)dependencyObject;
            that.SetFooterContentPresenter(dependencyPropertyChangedEventArgs.NewValue);
        }
        #endregion

        #region InformationContent Property

        public DependencyObject InformationText
        {
            get { return (DependencyObject)GetValue(InformationTextProperty); }
            set { SetValue(InformationTextProperty, value); }
        }

        public static readonly DependencyProperty InformationTextProperty = DependencyProperty.Register("InformationText", typeof(DependencyObject), typeof(SplitShell), new PropertyMetadata(default(DependencyObject), InformationContentPresenterPropertyChangedCallback));

        private static void InformationContentPresenterPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var that = (SplitShell)dependencyObject;
            that.SetInformationContent(dependencyPropertyChangedEventArgs.NewValue);
        }
        #endregion

        public SplitShell()
        {
            DefaultStyleKey = typeof(SplitShell);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _contentPresenter = (ContentPresenter)GetTemplateChild(ContentPresenterName);
            _topBarContentPresenter = (ContentPresenter)GetTemplateChild(TopBarContentPresenterName);
            _informationGrid = (ContentPresenter)GetTemplateChild(InformationContentPresenterName);
            _rightFlyoutContentPresenter = (ContentPresenter)GetTemplateChild(RightFlyoutContentPresenterName);
            _rightFlyoutFadeIn = (Storyboard)GetTemplateChild(RightFlyoutFadeInName);
            _rightFlyoutFadeOut = (Storyboard)GetTemplateChild(RightFlyoutFadeOutName);
            _rightFlyoutPlaneProjection = (PlaneProjection)GetTemplateChild(RightFlyoutPlaneProjectionName);
            _rightFlyoutGridContainer = (Grid)GetTemplateChild(RightFlyoutGridContainerName);
            _flyoutBackgroundGrid = (Grid)GetTemplateChild(FlyoutBackgroundGridName);
            _footerContentPresenter = (ContentPresenter) GetTemplateChild(FooterContentPresenterName);

            TemplateApplied.SetResult(true);
            
            _rightFlyoutGridContainer.Visibility = Visibility.Collapsed;
            _flyoutBackgroundGrid.Tapped += RightFlyoutGridContainerOnTapped;
        }
        
        private void RightFlyoutGridContainerOnTapped(object sender, TappedRoutedEventArgs tappedRoutedEventArgs)
        {
            if (FlyoutCloseRequested != null)
                FlyoutCloseRequested.Invoke(null, new EventArgs());
        }
        
        void ShowFlyout()
        {
            _rightFlyoutFadeIn.Begin();
            IsRightFlyoutOpen = true;
        }

        public void HideFlyout()
        {
            _rightFlyoutFadeOut.Begin();
            IsRightFlyoutOpen = false;
        }

        public bool IsRightFlyoutOpen { get; set; }
    }
}
