using System;
using System.Collections.Generic;
using System.Threading;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;
using System.Runtime.InteropServices;
using Windows.UI.Xaml.Markup;
using ManagedJupiterHost;
using JupiterSUnitDrts.DXInterop;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Popups;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.System.Threading;
using Windows.Media;
using Windows.Graphics.Display;
using Windows.UI.Xaml.Data;
using System.Runtime.InteropServices.ComTypes;

namespace DxInterop
{

    public partial class DXInteropApp : Application
    {
        public bool TickPriorityTestingMode = false; //flag to test tick priority changes to address input message starvation.
        CoreDispatcherPriority[] priorities = new CoreDispatcherPriority[] { CoreDispatcherPriority.High, CoreDispatcherPriority.Low, CoreDispatcherPriority.Normal };
        public static Window CurrentWindow = null;

        public DXInteropApp()
        {
        }


        protected override void OnLaunched(Windows.ApplicationModel.Activation.LaunchActivatedEventArgs args)
        {
            base.OnLaunched(args);
            Application.Current.DebugSettings.IsOverdrawHeatMapEnabled = false;
            MainPage firstPage = new MainPage();
            Window.Current.Content = firstPage;
            CurrentWindow = Window.Current;

            Window.Current.Activate();
        }

        public static void ActivateWindow(int index)
        {
            CoreWindow cw = DXInteropApp.WindowList[index];
            var nowait = cw.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                cw.Activate();
                Window.Current.Activate();
            });
        }

        public static void CloseWindow(int index)
        {
            CoreWindow cw = DXInteropApp.WindowList[index];
            var nowait = cw.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                cw.Close();
            });
        }

        private static List<CoreWindow> _windows = new List<CoreWindow>();
        public static List<CoreWindow> WindowList
        {
            get
            {
                return _windows;
            }
        }
    }


    public sealed partial class MainPage : Page
    {
        ID3D11Device d3d11Device = null;
        IDXGIDevice dxgiDevice = null;
        IDXGISwapChain1 dxgiSwapChain = null;
        SwapChainBackgroundPanelInterop customSCBP = null;
        Random random = new Random();
        UserControl VSMControl;
        enum BrushType { SIS, VSIS };
        List<SurfaceImageSourceWrapper> SurfaceImageSourceList = new List<SurfaceImageSourceWrapper>();
        MediaElement mediaElement = null;
        Popup popup = null;
        TextBlock numberOfChildrenTextBlock = null;
        TextBlock errorText = null;
        TextBlock WindowIndex = null;
        ScrollViewer scrollViewer = null;
        Slider slider = null;
        Button switchRoot = null;
        Button toggleTickPriorityTestingMode = null;
        int priorityIndex = 0;
        CoreDispatcherPriority[] priorities = new CoreDispatcherPriority[] { CoreDispatcherPriority.High, CoreDispatcherPriority.Low, CoreDispatcherPriority.Normal };
        DXInteropApp App = null;
        Microsoft.Interop.D2D.DeviceContext d2dDeviceContext = null;

        public MainPage()
        {
            InitializeD3D11();
            InitializeComponent();
            ((SwapChainBackgroundPanel)customSCBP).PointerMoved += SCBP_PointerMoved;
            Window.Current.SizeChanged += Window_SizeChanged;
        }

        public void InitializeD3D11()
        {
            DXInteropHelper.InitializeD3D(D3D_DRIVER_TYPE.D3D_DRIVER_TYPE_HARDWARE, out d3d11Device, D3D11_CREATE_DEVICE_FLAG.D3D11_CREATE_DEVICE_BGRA_SUPPORT);
            DXInteropHelper.InitializeD2D(d3d11Device, out d2dDeviceContext);
            dxgiDevice = DXInteropHelper.GetDXGIDevice(d3d11Device);
            dxgiSwapChain = DXInteropHelper.CreateSwapChainForComposition(d3d11Device, (int)Window.Current.Bounds.Width, (int)Window.Current.Bounds.Height, true);

        }
        public void InitializeComponent()
        {
            Application.LoadComponent(this, new System.Uri("ms-resource:Files/MainPage.xaml"));

            App = (DXInteropApp)Application.Current;

            SwapChainBackgroundPanel customSCBP = this.SetupSCBPTree();

            this.Content = customSCBP;

            // On SurfaceContentsLost we create a new device, set a new SC, and recover SIS/VSIS contents, but apperentlly the device will not be ready for the next CompositionTarget_Rendering.
            CompositionTarget.SurfaceContentsLost += new System.EventHandler<object>(CompositionTarget_SurfaceContentsLost);
            CompositionTarget.Rendering += new System.EventHandler<object>(CompositionTarget_Rendering);

            DXInteropApp.WindowList.Add(Window.Current.CoreWindow);
            Window.Current.Activated += new WindowActivatedEventHandler(Current_Window_Activated);
        }

        StackPanel stackPanel = null;
        public SwapChainBackgroundPanel SetupSCBPTree()
        {
            customSCBP = new CustomSwapChainBackgroundPanel();
            customSCBP.SCBP.CompositeMode = (ElementCompositeMode)2;

            customSCBP.SetSwapChain(dxgiSwapChain);
            customSCBP.UpdateSurface(new float[] { 1f, 1f, 1f, 1f });

            popup = new Popup() { Width = 200, Height = 100, IsOpen = false, Child = new TextBlock() { Width = 200, Height = 100, Text = "Popup", Foreground = new SolidColorBrush(Colors.Black), FontSize = 20 }, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Top };

            mediaElement = new MediaElement() { Width = 200, Height = 200, AutoPlay = false, Source = new Uri(@"http://mftest_2:81/dxaml/batman.mp4"), Visibility = Visibility.Visible, IsLooping = true, AudioCategory = AudioCategory.BackgroundCapableMedia, AreTransportControlsEnabled = true };
            ((SwapChainBackgroundPanel)customSCBP).Children.Add(mediaElement);

            scrollViewer = new ScrollViewer() { Width = 500, Height = 1000, HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Center };
            stackPanel = new StackPanel() { Background = new SolidColorBrush(Colors.Brown) };

            AddScrollViewerDimensionsContent(stackPanel);
            stackPanel.Children.Add(CreateSCPContent((ElementCompositeMode)3, DXContentType.D2DTiles, null, null));
            stackPanel.Children.Add(CreateSCPContent(ElementCompositeMode.SourceOver, DXContentType.D2DTiles, null, null));
            stackPanel.Children.Add(CreateSCPContent(ElementCompositeMode.Inherit, DXContentType.D2DClock, new float[] { (float)random.Next(255) / 255, (float)random.Next(255) / 255, (float)random.Next(255) / 255, (float)random.Next(255) / 255 }, null));
            stackPanel.Children.Add(CreateSCPContent(ElementCompositeMode.MinBlend, DXContentType.D2DTiles, null, null));

            AddSISContent(stackPanel);

            scrollViewer.Content = stackPanel;
            ((SwapChainBackgroundPanel)customSCBP).Children.Add(scrollViewer);

            VSMControl = (UserControl)XamlReader.Load(vsmXamlControl);
            ((SwapChainBackgroundPanel)customSCBP).Children.Add(VSMControl);

            switchRoot = (Button)VSMControl.FindName("_SwitchRoot");
            switchRoot.Click += new RoutedEventHandler(SwitchRoot);

            Button _VSMInvokeButton = (Button)VSMControl.FindName("_VSMInvokeButton");
            _VSMInvokeButton.Click += new RoutedEventHandler(_VSMInvokeButton_Click);

            Button _ChangeVisibilityButton = (Button)VSMControl.FindName("_ChangeVisibility");
            _ChangeVisibilityButton.Click += new RoutedEventHandler(_ChangeVisibilityButton_Click);

            Button _ChangePopup = (Button)VSMControl.FindName("_ChangePopup");
            _ChangePopup.Click += new RoutedEventHandler(_ChangePopup_Click);

            Button _AddChild = (Button)VSMControl.FindName("_AddChild");
            _AddChild.Click += new RoutedEventHandler(_AddChild_Click);

            Button _RemoveChild = (Button)VSMControl.FindName("_RemoveChild");
            _RemoveChild.Click += new RoutedEventHandler(_RemoveChild_Click);

            Button _AddRemoveMedia = (Button)VSMControl.FindName("_AddRemoveMedia");
            _AddRemoveMedia.Click += new RoutedEventHandler(_AddRemoveMedia_Click);

            Button _OverDrawHeatMapButton = (Button)VSMControl.FindName("_OverdawHeatMapButton");
            _OverDrawHeatMapButton.Click += (s, e) => { Application.Current.DebugSettings.IsOverdrawHeatMapEnabled = !Application.Current.DebugSettings.IsOverdrawHeatMapEnabled; };

            Button _CreateNewWindow = (Button)VSMControl.FindName("_CreateNewWindow");
            _CreateNewWindow.Click += new RoutedEventHandler(_createWindowButton_Click);

            Button _CloseWindow = (Button)VSMControl.FindName("_CloseWindow");
            _CloseWindow.Click += new RoutedEventHandler(_closeWindowButton_Click);

            Button _PreviousWindow = (Button)VSMControl.FindName("_PreviousWindow");
            _PreviousWindow.Click += new RoutedEventHandler(_previousWindowButton_Click);

            Button _NextWindow = (Button)VSMControl.FindName("_NextWindow");
            _NextWindow.Click += new RoutedEventHandler(_nextWindowButton_Click);

            WindowIndex = (TextBlock)VSMControl.FindName("_WindowIndex");

            toggleTickPriorityTestingMode = (Button)VSMControl.FindName("_ToggleTickPriorityTestingMode");
            toggleTickPriorityTestingMode.Click += new RoutedEventHandler(toggleTickPriorityTestingMode_Click);
            toggleTickPriorityTestingMode.Content = "Toggle TickPriorityTestingMode (" + App.TickPriorityTestingMode.ToString() + ")";

            numberOfChildrenTextBlock = (TextBlock)VSMControl.FindName("_NumOfChildrenTextBox");
            errorText = (TextBlock)VSMControl.FindName("_errorText");
            errorText.TextWrapping = TextWrapping.Wrap;
            errorText.Text = "Page is now the Main Root";

            slider = (Slider)VSMControl.FindName("_Slider");
            slider.ValueChanged += new RangeBaseValueChangedEventHandler(Slider_ValueChanged);

            ((SwapChainBackgroundPanel)customSCBP).Children.Add(new Button() { Content = "Below the AppBar", VerticalAlignment = VerticalAlignment.Bottom, Width = 200 });

            return (SwapChainBackgroundPanel)customSCBP;
        }


        private SwapChainPanelInterop CreateSCPContent(ElementCompositeMode compositeMode, DXContentType dxContentType, float[] color, Rect? rect, int height = 200)
        {
            double scpWidth = scrollViewer.Width;
            double scpHeight = height;
            SwapChainPanel scp = new SwapChainPanel() { Width = scpWidth, Height = scpHeight, CompositeMode = compositeMode };
            SwapChainPanelInterop scpInterop = scp;
            IDXGISwapChain1 swapChain = DXInteropHelper.CreateSwapChainForComposition(d3d11Device, (int)scpWidth, (int)scpHeight, false);
            scpInterop.SetSwapChain(swapChain);
            DrawDXContent(dxContentType, scpInterop, color, rect);
            scpInterop.CopyBuffers(1, 0);

            float compositionScaleX = 1; float compositionScaleY = 1;
            scp.CompositionScaleChanged += (s, e) =>
            {
                compositionScaleX = scp.CompositionScaleX;
                compositionScaleY = scp.CompositionScaleY;

                try
                {
                    scpInterop.ResizeBuffers((int)scp.Width, (int)scp.Height);
                }
                catch (System.Runtime.InteropServices.COMException ex)
                {
                    if (ex.HResult == unchecked((int)0x887A0005)) /* DXGI_ERROR_DEVICE_REMOVED */
                    {
                        swapChain = DXInteropHelper.CreateSwapChainForComposition(d3d11Device, (int)scp.Width, (int)scp.Height, false);
                        scpInterop.SetSwapChain(swapChain);
                    }
                    else
                        throw ex;
                }

                DrawDXContent(dxContentType, scpInterop, color, rect);
                scpInterop.CopyBuffers(1, 0);
            };

            StackPanel scpStackPanel = new StackPanel() { Orientation = Orientation.Vertical };
            StackPanel sp = new StackPanel() { Orientation = Orientation.Horizontal };
            TextBox scpWidthTB = new TextBox() { Height = 60, Header = "SCPWidth" };
            TextBox scpHeightTB = new TextBox() { Height = 60, Header = "SCPHeight" };
            Binding binding = new Binding() { Source = scp, Path = new PropertyPath("Width"), Mode = BindingMode.TwoWay };
            scpWidthTB.SetBinding(TextBox.TextProperty, binding);
            binding = new Binding() { Source = scp, Path = new PropertyPath("Height"), Mode = BindingMode.TwoWay };
            scpHeightTB.SetBinding(TextBox.TextProperty, binding);
            sp.Children.Add(scpWidthTB);
            sp.Children.Add(scpHeightTB);
            scpStackPanel.Children.Add(sp);

            StackPanel sp1 = new StackPanel() { Orientation = Orientation.Horizontal };
            sp1.Children.Add(new TextBlock() { Text = "CoreInput" });
            CheckBox coreinputEnabledCB = new CheckBox() { IsChecked = true, IsThreeState = false };
            sp1.Children.Add(coreinputEnabledCB);

            sp1.Children.Add(new TextBlock() { Text = "Touch" });
            CheckBox coreinputTouchCB = new CheckBox() { IsChecked = true, IsThreeState = false };
            sp1.Children.Add(coreinputTouchCB);

            sp1.Children.Add(new TextBlock() { Text = "Mouse" });
            CheckBox coreinputMouseCB = new CheckBox() { IsChecked = true, IsThreeState = false };
            sp1.Children.Add(coreinputMouseCB);

            scpStackPanel.Children.Add(sp1);

            scp.Children.Add(scpStackPanel);


            bool enableTouchCoreInput = true;
            bool enableMouseCoreInput = true;



            CoreDispatcher coreInputDispatcher = null;
            CoreDispatcher xamlDispatcher = Window.Current.Dispatcher;
            Action coreInputThreadAction = () =>
            {
                CoreInputDeviceTypes deviceTypes = CoreInputDeviceTypes.Pen;
                if (enableTouchCoreInput) deviceTypes |= CoreInputDeviceTypes.Touch;
                if (enableMouseCoreInput) deviceTypes |= CoreInputDeviceTypes.Mouse;

                CoreIndependentInputSource coreInput = scp.CreateCoreIndependentInputSource(deviceTypes);
                coreInputDispatcher = coreInput.Dispatcher;

                xamlDispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    coreinputEnabledCB.Unchecked += (s4, e4) =>
                    {
                        coreInputDispatcher.RunAsync(CoreDispatcherPriority.High, () => { coreInput.IsInputEnabled = false; });
                    };
                    coreinputEnabledCB.Checked += (s4, e4) =>
                    {
                        coreInputDispatcher.RunAsync(CoreDispatcherPriority.High, () => { coreInput.IsInputEnabled = true; });
                    };
                });

                coreInput.PointerPressed += (s, e) =>
                {
                    scpInterop.UpdateSurfaceWithD2DEllipse(new Point(e.CurrentPoint.Position.X * compositionScaleX, e.CurrentPoint.Position.Y * compositionScaleY), 10.0f, new float[] { 1f, 0f, 1f, 1f }, true /*copyBuffers*/);
                };
                coreInput.PointerMoved += (s1, e1) =>
                {
                    scpInterop.UpdateSurfaceWithD2DEllipse(new Point(e1.CurrentPoint.Position.X * compositionScaleX, e1.CurrentPoint.Position.Y * compositionScaleY), 10.0f, new float[] { 0f, 0f, 1f, 1f }, true /*copyBuffers*/);
                };
                coreInput.PointerReleased += (s2, e2) =>
                {
                    scpInterop.UpdateSurfaceWithD2DEllipse(new Point(e2.CurrentPoint.Position.X * compositionScaleX, e2.CurrentPoint.Position.Y * compositionScaleY), 10.0f, new float[] { 0f, 0f, 1f, 1f }, true /*copyBuffers*/);
                    // Marking this unhandled so that AppBar can be invoked by letting the right click go through CoreInput which then raises WM_ContextMenu
                    // that calls handles in xaml framework which marshalles the call to the UIthread to toggle AppBar.
                    e2.Handled = false;
                };

                coreInputDispatcher.ProcessEvents(CoreProcessEventsOption.ProcessUntilQuit);
            };

            Action startCoreInputThread = () =>
            {
                Windows.System.Threading.ThreadPool.RunAsync((asyncAction) =>
                {
                    if (coreInputDispatcher != null)
                    {
                        coreInputDispatcher.StopProcessEvents();
                        coreInputDispatcher = null;
                    }

                    coreInputThreadAction();
                }, WorkItemPriority.High);
            };

            startCoreInputThread();

            coreinputTouchCB.Checked += (x, y) => { enableTouchCoreInput = true; startCoreInputThread(); };
            coreinputTouchCB.Unchecked += (x, y) => { enableTouchCoreInput = false; startCoreInputThread(); };
            coreinputMouseCB.Checked += (x, y) => { enableMouseCoreInput = true; scp.CompositeMode = (ElementCompositeMode)(((int)scp.CompositeMode + 1) % 4); startCoreInputThread(); };
            coreinputMouseCB.Unchecked += (x, y) => { enableMouseCoreInput = false; startCoreInputThread(); };

            return scpInterop;
        }

        enum DXContentType
        {
            D3DClear,
            D2DClear,
            D2DTiles,
            D2DClock,
            D2DRectangle,
        }

        private void DrawDXContent(DXContentType dxContentType, SwapChainPanelInteropBase scpBase, float[] color, Rect? rect)
        {
            switch (dxContentType)
            {
                case DXContentType.D2DTiles:
                    scpBase.UpdateSurfaceWithD2DTiles();
                    break;
                case DXContentType.D3DClear:
                    scpBase.UpdateSurface(color);
                    break;
                case DXContentType.D2DClear:
                    scpBase.UpdateSurfaceWithD2D(color);
                    break;
                case DXContentType.D2DClock:
                    scpBase.UpdateSurfaceWithD2DClock(color);
                    break;
                case DXContentType.D2DRectangle:
                    scpBase.UpdateSurfaceWithD2DRectangle(color, rect.HasValue ? rect.Value : new Rect());
                    break;

                default:
                    throw new InvalidOperationException("Cannot draw this dx content type " + dxContentType.ToString());
            }
        }

        private void AddScrollViewerDimensionsContent(StackPanel stackPanel)
        {
            StackPanel sp = new StackPanel() { Orientation = Orientation.Horizontal };
            TextBox svWidth = new TextBox() { Height = 60, Header = "ScrollViewerWidth" };
            TextBox svHeight = new TextBox() { Height = 60, Header = "ScrollViewerHeight" };

            Binding binding = new Binding() { Source = scrollViewer, Path = new PropertyPath("Width"), Mode = BindingMode.TwoWay };
            svWidth.SetBinding(TextBox.TextProperty, binding);

            binding = new Binding() { Source = scrollViewer, Path = new PropertyPath("Height"), Mode = BindingMode.TwoWay };
            svHeight.SetBinding(TextBox.TextProperty, binding);

            sp.Children.Add(svWidth);
            sp.Children.Add(svHeight);

            stackPanel.Children.Add(sp);
        }


        void _RemoveChild_Click(object sender, RoutedEventArgs e)
        {
            foreach (UIElement element in ((SwapChainBackgroundPanel)customSCBP).Children)
            {
                if (element is Rectangle)
                {
                    ((SwapChainBackgroundPanel)customSCBP).Children.Remove(element);
                    break;
                }
            }
        }

        void SwitchRoot(object sender, RoutedEventArgs e)
        {

            if (Window.Current.Content is Page)
            {
                this.Content = null;
                switchRoot.Content = "Switch to Page Root";
                Window.Current.Content = customSCBP;
                errorText.Text = "SwapChainBackgroundPanel is now the Main Root";
            }
            else
            {
                Window.Current.Content = new MainPage();
            }
        }

        void _AddChild_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Rectangle rect = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.Red), HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top };
                ((SwapChainBackgroundPanel)customSCBP).Children.Insert(0, rect);
                rect.RenderTransform = CreateTransform();
                CreateAnimation(rect);
            }
            catch (Exception ee)
            {
                errorText.Text = ee.Message;
            }
        }

        void _AddRemoveMedia_Click(object sender, RoutedEventArgs e)
        {
            if (((SwapChainBackgroundPanel)customSCBP).Children.Contains(mediaElement))
            {
                ((SwapChainBackgroundPanel)customSCBP).Children.Remove(mediaElement);
            }
            else
            {
                ((SwapChainBackgroundPanel)customSCBP).Children.Add(mediaElement);
            }
        }

        void _ChangePopup_Click(object sender, RoutedEventArgs e)
        {
            popup.IsOpen = !popup.IsOpen;
        }

        void _ChangeVisibilityButton_Click(object sender, RoutedEventArgs e)
        {
            scrollViewer.Visibility = scrollViewer.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            scrollViewer.Opacity = slider.Value;
        }

        void CompositionTarget_SurfaceContentsLost(object sender, object args)
        {
            InitializeD3D11();
            //SetupSCBPTree();
            customSCBP.SetSwapChain(dxgiSwapChain);

            foreach (SurfaceImageSourceWrapper surfaceImageSourceWrapper in SurfaceImageSourceList)
            {
                surfaceImageSourceWrapper.UpdateSurfaceImageSource(dxgiDevice);
            }
        }

        Visibility visibleState = Visibility.Visible;
        void _VSMInvokeButton_Click(object sender, RoutedEventArgs e)
        {
            if (visibleState == Visibility.Visible)
            {
                VisualStateManager.GoToState(VSMControl, "Collapsed", true);
                visibleState = Visibility.Collapsed;
            }
            else
            {
                VisualStateManager.GoToState(VSMControl, "Visible", true);
                visibleState = Visibility.Visible;
            }
        }

        void toggleTickPriorityTestingMode_Click(object sender, RoutedEventArgs e)
        {
            App.TickPriorityTestingMode = !App.TickPriorityTestingMode;
            ((Button)sender).Content = "Toggle TickPriorityTestingMode (" + App.TickPriorityTestingMode.ToString() + ")";

            if (App.TickPriorityTestingMode)
            {
                ThreadPoolTimer.CreatePeriodicTimer(TimerElapsedHandler1, TimeSpan.FromMilliseconds(100));
            }
        }


        void TimerElapsedHandler1(ThreadPoolTimer timer)
        {
            if (App.TickPriorityTestingMode)
            {
                var nowait = DXInteropApp.CurrentWindow.Dispatcher.RunAsync(priorities[priorityIndex++ % 3], () =>
                {
                    int i = 0;
                    while (i > 10) i++;
                });
            }
            else
            {
                timer.Cancel();
            }
        }

        private static void DoSomeWork(int iterations)
        {
            bool flag = false;
            for (int i = 0; i < iterations; i++)
            {
                flag = !flag;
            }
        }

        Color[] colors = new Color[] { Colors.Red, Colors.Green, Colors.Blue };
        static int colorIndex = 0;
        void CompositionTarget_Rendering(object sender, object args)
        {
            if (App.TickPriorityTestingMode)
            {
                DoSomeWork(1000000000);
                stackPanel.Background = new SolidColorBrush(colors[colorIndex++ % 3]);
            }
            else
            {
                UpdateSCBPContent();
            }
        }

        // scbp user is expected to listen for the window size/orientation changes and always create 2x1 swapchain for Dflip.
        void Window_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            //DisplayProperties.CurrentOrienation takes sometime to return correct orientation value. As a workaround, delaying this by .5 sec.
            DispatcherTimer timer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(500) };
            timer.Tick += (s1, e1) =>
            {
                dxgiSwapChain = DXInteropHelper.CreateSwapChainForComposition(d3d11Device, (int)e.Size.Width, (int)e.Size.Height, true);

                customSCBP.SetSwapChain(dxgiSwapChain);
                timer.Stop();
            };

            timer.Start();
        }

        private void UpdateSCBPContent()
        {
            if (customSCBP != null)
            {
                if (((CustomSwapChainBackgroundPanel)customSCBP).KeepUpdatingSurface)
                {
                    try
                    {
                        //customSCBP.UpdateSurfaceWithD2DClock(new float[] { (float)random.Next(255) / 255, (float)random.Next(255) / 255, (float)random.Next(255) / 255, (float)random.Next(255) / 255 }, true);
                    }
                    catch (Exception exception)
                    {
                        errorText.Text = exception.Message;
                    }
                }
                if (int.Parse(numberOfChildrenTextBlock.Text) != ((CustomSwapChainBackgroundPanel)customSCBP).Children.Count)
                    numberOfChildrenTextBlock.Text = ((CustomSwapChainBackgroundPanel)customSCBP).Children.Count.ToString();
            }
        }

        void SCBP_PointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (App.TickPriorityTestingMode)
            {
                DoSomeWork(50000000);

                UpdateSCBPContent();
            }
        }


        #region SIS/VSIS content
        private void AddSISContent(StackPanel stackPanel)
        {
            string[] brushTypes = { "SIS", "VSIS" };
            foreach (string brushType in brushTypes)
            {
                for (int strechEnumIndex = 0; strechEnumIndex < 4; strechEnumIndex++)
                {
                    ImageBrush foregroundbrush = new ImageBrush();
                    ImageBrush fillBrush = new ImageBrush();
                    ImageBrush strokeBrush = new ImageBrush();
                    if (brushType == "SIS")
                    {
                        foregroundbrush.ImageSource = (SurfaceImageSourceInterop)CreateSurfaceImageSource(BrushType.SIS, new Rect() { X = 0, Y = 0, Width = 200, Height = 400 }, new float[] { 1f, 0f, 0.8f, 0.5f }, new Rect() { X = 200, Y = 0, Width = 200, Height = 400 }, new float[] { 1f, 0.8f, 1f, 1.0f });
                        fillBrush.ImageSource = (SurfaceImageSourceInterop)CreateSurfaceImageSource(BrushType.SIS, new Rect() { X = 0, Y = 0, Width = 200, Height = 400 }, new float[] { 1f, 0f, 0.8f, 0.5f }, new Rect() { X = 200, Y = 0, Width = 200, Height = 400 }, new float[] { 1f, 0f, 1f, 1.0f });
                        strokeBrush.ImageSource = (SurfaceImageSourceInterop)CreateSurfaceImageSource(BrushType.SIS, new Rect() { X = 0, Y = 0, Width = 200, Height = 400 }, new float[] { 1f, .2f, .5f, 0.8f }, new Rect() { X = 200, Y = 0, Width = 200, Height = 400 }, new float[] { 1f, 0.8f, 0.6f, 0.9f });
                    }
                    else
                    {
                        foregroundbrush.ImageSource = (VirtualSurfaceImageSourceInterop)CreateSurfaceImageSource(BrushType.VSIS, new Rect() { X = 0, Y = 0, Width = 200, Height = 400 }, new float[] { 1f, 0f, 0.8f, 0.5f }, new Rect() { X = 200, Y = 0, Width = 200, Height = 400 }, new float[] { 1f, 0.8f, 1f, 1.0f });
                        fillBrush.ImageSource = (VirtualSurfaceImageSourceInterop)CreateSurfaceImageSource(BrushType.VSIS, new Rect() { X = 0, Y = 0, Width = 200, Height = 400 }, new float[] { 1f, 0f, 0.8f, 0.5f }, new Rect() { X = 200, Y = 0, Width = 200, Height = 400 }, new float[] { 1f, 0f, 1f, 1.0f });
                        strokeBrush.ImageSource = (VirtualSurfaceImageSourceInterop)CreateSurfaceImageSource(BrushType.VSIS, new Rect() { X = 0, Y = 0, Width = 200, Height = 400 }, new float[] { 1f, .2f, .5f, 0.8f }, new Rect() { X = 200, Y = 0, Width = 200, Height = 400 }, new float[] { 1f, 0.8f, 0.6f, 0.9f });
                    }

                    foregroundbrush.Stretch = fillBrush.Stretch = strokeBrush.Stretch = Stretch.None + strechEnumIndex;

                    TextBlock myText = new TextBlock();
                    myText.Text = brushType + " with " + foregroundbrush.Stretch.ToString() + " StretchMode";
                    myText.FontSize = 20;
                    myText.Height = 60;
                    myText.Foreground = foregroundbrush;
                    Rectangle rectangle = new Rectangle();
                    rectangle.Width = 200;
                    rectangle.Height = 100;
                    rectangle.Stroke = strokeBrush;
                    rectangle.Fill = fillBrush;
                    rectangle.StrokeThickness = 20;
                    rectangle.Margin = new Thickness(10);
                    Polygon polygon = new Polygon();
                    polygon.Points.Add(new Point(0, 0));
                    polygon.Points.Add(new Point(100, 0));
                    polygon.Points.Add(new Point(120, 120));
                    polygon.Points.Add(new Point(100, 100));
                    polygon.Points.Add(new Point(0, 100));
                    polygon.Fill = fillBrush;
                    polygon.Stroke = strokeBrush;
                    polygon.StrokeThickness = 20;
                    polygon.Margin = new Thickness(10);

                    stackPanel.Children.Add(myText);
                    stackPanel.Children.Add(rectangle);
                    stackPanel.Children.Add(polygon);
                }
            }
        }

        private SurfaceImageSourceInterop CreateSurfaceImageSource(BrushType type, Rect r1, float[] args1, Rect r2, float[] args2)
        {
            SurfaceImageSourceInterop sis = null;

            if (type == BrushType.SIS)
            {
                sis = new SurfaceImageSourceInterop(400, 400);
            }
            else
            {
                sis = new VirtualSurfaceImageSourceInterop(400, 400);
            }
            sis.SetDevice(dxgiDevice);
            SurfaceImageSourceWrapper surfaceImageSourceWrapper = new SurfaceImageSourceWrapper(sis, r1, args1, r2, args2, dxgiDevice);

            // Add a call back to redraw VSIS
            if (type == BrushType.VSIS)
            {
                ((VirtualSurfaceImageSourceInterop)sis).RegisterForUpdatesNeeded(new DXInteropVirtualSurfaceUpdatesCallback(surfaceImageSourceWrapper));
            }
            else
            {
                // Update SIS only,since the call back will update VSIS.
                sis.UpdateSurface(r1, args1);
                sis.UpdateSurface(r2, args2);
            }

            SurfaceImageSourceList.Add(surfaceImageSourceWrapper);

            return sis;
        }

        #endregion SIS/VSIS content


        #region Multiple Window Handlers.

        void _createWindowButton_Click(object sender, RoutedEventArgs e)
        {
            CoreApplicationView cav = CoreApplication.CreateNewView("", "content");
            var nowait = cav.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Window.Current.Content = new MainPage();
                List<CoreWindow> windowList = DXInteropApp.WindowList;
                int index = windowList.FindIndex((cw) =>
                {
                    return cw.GetHashCode() == Window.Current.CoreWindow.GetHashCode();
                });
                DXInteropApp.ActivateWindow(index);
            });
        }

        void _closeWindowButton_Click(object sender, RoutedEventArgs e)
        {
            List<CoreWindow> windowList = DXInteropApp.WindowList;
            if (windowList.Count == 1)
            {
                this.WindowIndex.Text = "This is the only window, and it can't be closed";
                return;
            }

            int index = windowList.FindIndex((cw) =>
            {
                return cw.GetHashCode() == Window.Current.CoreWindow.GetHashCode();
            });
            if (index > 0)
            {
                DXInteropApp.ActivateWindow(index - 1);
                this.WindowIndex.Text = (index - 1).ToString();
            }
            else
            {
                DXInteropApp.ActivateWindow(index + 1);
                this.WindowIndex.Text = (index + 1).ToString();
            }

            DXInteropApp.CloseWindow(index);
            lock (DXInteropApp.WindowList)
            {
                DXInteropApp.WindowList.Remove(windowList[index]);
            }
        }

        void _nextWindowButton_Click(object sender, RoutedEventArgs e)
        {
            List<CoreWindow> windowList = DXInteropApp.WindowList;

            int index = windowList.FindIndex((cw) =>
            {
                return cw.GetHashCode() == Window.Current.CoreWindow.GetHashCode();
            });
            if (index == windowList.Count - 1)
            {
                this.WindowIndex.Text = "No next window";
            }
            else
            {
                DXInteropApp.ActivateWindow(index + 1);
                this.WindowIndex.Text = (index + 1).ToString();
            }
        }

        void _previousWindowButton_Click(object sender, RoutedEventArgs e)
        {
            List<CoreWindow> windowList = DXInteropApp.WindowList;
            int index = windowList.IndexOf(Window.Current.CoreWindow);
            if (index == 0)
            {
                this.WindowIndex.Text = "No previous window";
            }
            else
            {
                DXInteropApp.ActivateWindow(index - 1);
                this.WindowIndex.Text = (index - 1).ToString();
            }
        }

        void Current_Window_Activated(object sender, WindowActivatedEventArgs e)
        {
            List<CoreWindow> windowList = DXInteropApp.WindowList;
            int index = windowList.FindIndex((cw) =>
            {
                return cw.GetHashCode() == Window.Current.CoreWindow.GetHashCode();
            });

            if (index != -1)
                this.WindowIndex.Text = index.ToString();
        }

        private void CreateAnimation(Rectangle rect)
        {
            Storyboard storyboard = new Storyboard();
            DoubleAnimation scaleTransformX = new DoubleAnimation();
            scaleTransformX.From = 0;
            scaleTransformX.To = 8;
            scaleTransformX.AutoReverse = true;
            scaleTransformX.RepeatBehavior = RepeatBehavior.Forever;
            scaleTransformX.Duration = new Duration(new TimeSpan(0, 0, 1));
            Storyboard.SetTargetProperty(scaleTransformX, "(Rectangle.RenderTransform).(ScaleTransform.ScaleX)");
            Storyboard.SetTarget(scaleTransformX, rect);

            DoubleAnimation scaleTransformY = new DoubleAnimation();
            scaleTransformY.From = 0;
            scaleTransformY.To = 8;
            scaleTransformY.AutoReverse = true;
            scaleTransformY.RepeatBehavior = RepeatBehavior.Forever;
            scaleTransformY.Duration = new Duration(new TimeSpan(0, 0, 1));
            Storyboard.SetTargetProperty(scaleTransformY, "(Rectangle.RenderTransform).(ScaleTransform.ScaleY)");
            Storyboard.SetTarget(scaleTransformY, rect);

            storyboard.Children.Add(scaleTransformX);
            storyboard.Children.Add(scaleTransformY);

            ((SwapChainBackgroundPanel)customSCBP).Resources.Add(Guid.NewGuid().ToString().GetHashCode().ToString("x"), storyboard);
            storyboard.Begin();
        }

        private Transform CreateTransform()
        {

            ScaleTransform scaleTransform = new ScaleTransform();
            scaleTransform.ScaleX = 0.8;
            scaleTransform.ScaleY = 0.8;
            return scaleTransform;
        }

        #endregion Multiple Window Handlers.

        #region Xamls
        string vsmXamlControl = @"  
         <UserControl  xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'  xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml' 
         Width='420' Margin='20'  HorizontalAlignment='Left'  VerticalAlignment='Top'>
<StackPanel>
 <VisualStateManager.VisualStateGroups>
                <VisualStateGroup x:Name='VisibiltyStates'>
                    <VisualState x:Name='Visible'>
                        <Storyboard>
                            <DoubleAnimation Storyboard.TargetName='_TargetButton' Storyboard.TargetProperty='(UIElement.Opacity)' Duration='00:00:00.25' To='1'>
                                <DoubleAnimation.EasingFunction>
                                    <CubicEase EasingMode='EaseIn' />
                                </DoubleAnimation.EasingFunction>
                            </DoubleAnimation>
                            <ObjectAnimationUsingKeyFrames Storyboard.TargetProperty='Visibility' Storyboard.TargetName='_TargetButton'>
                                <DiscreteObjectKeyFrame KeyTime='0:0:0' Value='Visible' />
                            </ObjectAnimationUsingKeyFrames>
                        </Storyboard>
                    </VisualState>
                    <VisualState x:Name='Collapsed'>
                        <Storyboard>
                            <DoubleAnimation Storyboard.TargetName='_TargetButton' Storyboard.TargetProperty='(UIElement.Opacity)' Duration='00:00:00.25' To='0'>
                                <DoubleAnimation.EasingFunction>
                                    <CubicEase EasingMode='EaseIn' />
                                </DoubleAnimation.EasingFunction>
                            </DoubleAnimation>
                            <ObjectAnimationUsingKeyFrames Storyboard.TargetProperty='Visibility' Storyboard.TargetName='_TargetButton'>
                                <DiscreteObjectKeyFrame KeyTime='0:0:1' Value='Collapsed' />
                            </ObjectAnimationUsingKeyFrames>
                        </Storyboard>
                    </VisualState>
                </VisualStateGroup>
            </VisualStateManager.VisualStateGroups>          

            <!-- The VisualStateManager hides/shows the UI controls in response to the application being snapped -->
           
            <Button x:Name='_SwitchRoot' Content='Switch To SCBP Root'/>
            <StackPanel Orientation='Horizontal'>
                <Button x:Name='_VSMInvokeButton' Content='VSM Invoke Button'/>
                <Button x:Name='_TargetButton' Content='VSM Target Button'/>
            </StackPanel>
            <Button x:Name='_OverdawHeatMapButton' Content='Toggle OverdrawHeatMap'/>
            <Button x:Name='_ChangeVisibility' Content='Change Stack Visibility'/>
            <Button x:Name='_ChangePopup' Content='Show/Hide Popup'/>
            <Button x:Name='_AddRemoveMedia' Content='Add/Remove Media'/>
            <StackPanel Orientation='Horizontal'>
                <Button x:Name='_AddChild' Content='Add Rectangle To SCBP'/>
                <Button x:Name='_RemoveChild' Content='Remove Rectangle From SCBP'/>
            </StackPanel>
            <StackPanel Orientation='Vertical'>
                <StackPanel Orientation='Horizontal'>
                    <Button x:Name='_CreateNewWindow' Content='Create New Window'/>
                    <Button x:Name='_CloseWindow' Content='Close Window'/>
                </StackPanel>
                <StackPanel Orientation='Horizontal'>
                    <Button x:Name='_NextWindow' Content='Next Window'/>
                    <Button x:Name='_PreviousWindow' Content='Previous Window'/>
                </StackPanel>
                <StackPanel Orientation='Horizontal'>
                    <TextBlock Text='WindowIndex:'/>
                    <TextBlock x:Name='_WindowIndex'/>
                </StackPanel>
            </StackPanel>
            <Button x:Name='_ToggleTickPriorityTestingMode' Content='Toggle TickPriorityTestingMode'/>
            <StackPanel Orientation='Horizontal'>
                <TextBlock Text='SIS/VSIS StackPanel Opacity'/>
                <Slider x:Name='_Slider' Minimum='0' Maximum ='1' Width='80' Height='40' SmallChange='0.1' LargeChange='0.1' Value='1' /> 
            </StackPanel>
            <StackPanel Orientation='Horizontal'>
                <TextBlock Text='Number of SCBP Children: '/>
                <TextBlock x:Name='_NumOfChildrenTextBox' Text='0' />
            </StackPanel>
            <TextBlock Text='0' Width='500' Height='100' x:Name='_errorText'/>
            <ListView Width='150' Background='LightGreen' CanDragItems='True' CanReorderItems='True' Height='400'>
                   <TextBox Text='Item 1'/>
                   <ComboBox>
                       <TextBlock Text='Combobox Item 1'/>
                       <TextBlock Text='Combobox Item 1'/>
                       <TextBlock Text='Combobox Item 1'/>
                   </ComboBox>                                                   
                   <TextBlock Text='Item 3'/>
                   <TextBlock Text='Item 4'/>
                   <TextBlock Text='Item 5'/>
                   <TextBlock Text='Item 6'/>
                   <TextBlock Text='Item 7'/>
                   <TextBlock Text='Item 8'/>
              </ListView>
            <StackPanel Orientation='Horizontal'>
                <Image Width='200' Height='150' Source='Logo.png' >
                  <Image.Triggers>
                    <EventTrigger RoutedEvent='Image.Loaded'>
                        <BeginStoryboard >
                             <Storyboard x:Name='SB_ImageRotate1' TargetName='ImageRotate1' TargetProperty='RotateTransform.Angle'>
                                <DoubleAnimation Duration='0:0:5' RepeatBehavior='Forever' From='360' To='0'/>
                              </Storyboard>
                        </BeginStoryboard >
                     </EventTrigger>
                 </Image.Triggers>
   
                 <Image.RenderTransform>
                     <RotateTransform Angle='0' x:Name='ImageRotate1'/>
                 </Image.RenderTransform>
                </Image>
                <Image Width='200' Height='150' Source='Logo.png' CacheMode='BitmapCache'>
                    <Image.Triggers>
                    <EventTrigger RoutedEvent='Image.Loaded'>
                         <BeginStoryboard>
                             <Storyboard x:Name='SB_ImageRotate2' TargetName='ImageRotate2' TargetProperty='RotateTransform.Angle'>
                                 <DoubleAnimation Duration='0:0:5' RepeatBehavior='Forever' From='0' To='360'/>
                             </Storyboard>
                        </BeginStoryboard>
                     </EventTrigger>
                 </Image.Triggers>
                   
                 <Image.RenderTransform>
                     <RotateTransform Angle='0' x:Name='ImageRotate2'/>
                 </Image.RenderTransform>
                </Image>
            </StackPanel>
           
           </StackPanel>
         </UserControl>
";

        #endregion Xamls
    }

    public class DXInteropVirtualSurfaceUpdatesCallback : IVirtualSurfaceUpdatesCallbackNative
    {
        private SurfaceImageSourceWrapper surfaceImageSourceWrapper;

        public DXInteropVirtualSurfaceUpdatesCallback(SurfaceImageSourceWrapper surfaceImageSourceWrapperArg)
        {
            this.surfaceImageSourceWrapper = surfaceImageSourceWrapperArg;
        }

        public void UpdatesNeeded()
        {
            surfaceImageSourceWrapper.UpdateSurfaceImageSource(null);
        }
    }

    public class SurfaceImageSourceWrapper
    {
        public SurfaceImageSourceInterop surfaceImageSource;
        float[] color1;
        float[] color2;
        Rect rect1;
        Rect rect2;
        IDXGIDevice device;
        public SurfaceImageSourceWrapper(SurfaceImageSourceInterop v_sis, Rect rect1arg, float[] colorargs1, Rect rectarg2, float[] colorargs2, IDXGIDevice device)
        {
            surfaceImageSource = v_sis;
            color1 = colorargs1;
            color2 = colorargs2;
            rect1 = rect1arg;
            rect2 = rectarg2;
            this.device = device;
        }

        public void UpdateSurfaceImageSource(IDXGIDevice device)
        {
            if (device != null)
                this.device = device;
            surfaceImageSource.SetDevice(this.device);
            surfaceImageSource.UpdateSurface(rect1, color1);
            surfaceImageSource.UpdateSurface(rect2, color2);
        }
    }

    class CustomSwapChainBackgroundPanel : SwapChainBackgroundPanel
    {
        public bool KeepUpdatingSurface { get; set; }

        public CustomSwapChainBackgroundPanel()
        {
            this.PointerPressed += new Windows.UI.Xaml.Input.PointerEventHandler(CustomSwapChainBackgroundPanel_PointerPressed);
            KeepUpdatingSurface = true;

            MediaControl.PlayPressed += MediaControl_PlayPressed;
            MediaControl.PausePressed += MediaControl_PausePressed;
            MediaControl.PlayPauseTogglePressed += MediaControl_PlayPauseTogglePressed;
            MediaControl.StopPressed += MediaControl_StopPressed;
        }



        void CustomSwapChainBackgroundPanel_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (((DXInteropApp)Application.Current).TickPriorityTestingMode)
            {
                this.KeepUpdatingSurface = true;
            }
            else
                this.KeepUpdatingSurface = !this.KeepUpdatingSurface;
        }


        private void MediaControl_StopPressed(object sender, object e)
        {

        }

        private void MediaControl_PlayPauseTogglePressed(object sender, object e)
        {
        }

        private void MediaControl_PausePressed(object sender, object e)
        {
        }

        private void MediaControl_PlayPressed(object sender, object e)
        {
        }

    }

    class Program
    {
        public void Run()
        {
            Application.Start((p) => new DXInteropApp());
        }

        static void Main(string[] args)
        {
            Program p = new Program();
            p.Run();
        }
    }


}
