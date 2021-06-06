using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Aqua.Wpf.Controls
{
    public partial class NavigationDrawer : UserControl
	{
		#region Свойства зависимости

		public static readonly DependencyProperty TopBarProperty;
		public static readonly DependencyProperty MenuHeaderProperty;
		public static readonly DependencyProperty MenuProperty;
		public static readonly DependencyProperty MenuFooterProperty;
		public new static readonly DependencyProperty ContentProperty;
		
		public static readonly DependencyProperty TopBarBackgroundProperty;
		public static readonly DependencyProperty MenuBackgroundProperty;
		public static readonly DependencyProperty ShadowColorProperty;
		public static readonly DependencyProperty ButtonMenuColorProperty;
		public static readonly DependencyProperty ButtonMenuHoverColorProperty;

		public static readonly DependencyProperty IsMenuOpenProperty;
		public static readonly DependencyProperty ButtonMenuColorChangeDurationProperty;

		public static readonly DependencyProperty MinMenuWidthProperty;
		public static readonly DependencyProperty MaxMenuWidthProperty;
		public static readonly DependencyProperty MenuHeaderVisibilityProperty;
		public static readonly DependencyProperty MenuFooterVisibilityProperty;

		#endregion

		#region Регистрация свойств зависимости

		static NavigationDrawer()
		{
			TopBarProperty = DependencyProperty.Register(
				nameof(TopBar),
				typeof(object),
				typeof(NavigationDrawer),
				new PropertyMetadata
				{
					PropertyChangedCallback = OnTopBarChanged
				});

			MenuHeaderProperty = DependencyProperty.Register(
				nameof(MenuHeader),
				typeof(object),
				typeof(NavigationDrawer),
				new PropertyMetadata
				{
					PropertyChangedCallback = OnMenuHeaderChanged
				});

			MenuProperty = DependencyProperty.Register(
				nameof(Menu),
				typeof(object),
				typeof(NavigationDrawer),
				new PropertyMetadata
				{
					PropertyChangedCallback = OnMenuChanged
				});

			MenuFooterProperty = DependencyProperty.Register(
				nameof(MenuFooter),
				typeof(object),
				typeof(NavigationDrawer),
				new PropertyMetadata
				{
					PropertyChangedCallback = OnMenuFooterChanged
				});

			ContentProperty = DependencyProperty.Register(
				nameof(Content),
				typeof(object),
				typeof(NavigationDrawer),
				new PropertyMetadata
				{
					PropertyChangedCallback = OnContentChanged
				});

			TopBarBackgroundProperty = DependencyProperty.Register(
				nameof(TopBarBackground),
				typeof(Brush),
				typeof(NavigationDrawer),
				new PropertyMetadata
				{
					PropertyChangedCallback = OnTopBarBackgroundChanged
				});

			MenuBackgroundProperty = DependencyProperty.Register(
				nameof(MenuBackground),
				typeof(Brush),
				typeof(NavigationDrawer),
				new PropertyMetadata
				{
					PropertyChangedCallback = OnMenuBackgroundChanged
				});

			ShadowColorProperty = DependencyProperty.Register(
				nameof(ShadowColor),
				typeof(Color),
				typeof(NavigationDrawer),
				new PropertyMetadata
				{
					PropertyChangedCallback = OnShadowColorChanged
				});

			ButtonMenuColorProperty = DependencyProperty.Register(
				nameof(ButtonMenuColor),
				typeof(Color),
				typeof(NavigationDrawer),
				new PropertyMetadata
				{
					PropertyChangedCallback = OnButtonMenuColorChanged
				});

			ButtonMenuHoverColorProperty = DependencyProperty.Register(
				nameof(ButtonMenuHoverColor),
				typeof(Color),
				typeof(NavigationDrawer));

			IsMenuOpenProperty = DependencyProperty.Register(
				nameof(IsMenuOpen),
				typeof(bool),
				typeof(NavigationDrawer),
				new PropertyMetadata
				{
					PropertyChangedCallback = OnIsMenuOpenChanged
				});

			ButtonMenuColorChangeDurationProperty = DependencyProperty.Register(
				nameof(ButtonMenuColorChangeDuration),
				typeof(Duration),
				typeof(NavigationDrawer));

			MinMenuWidthProperty = DependencyProperty.Register(
				nameof(MinMenuWidth),
				typeof(double),
				typeof(NavigationDrawer),
				new PropertyMetadata
				{
					PropertyChangedCallback = OnMinMenuWidthChanged
				});

			MaxMenuWidthProperty = DependencyProperty.Register(
				nameof(MaxMenuWidth),
				typeof(double),
				typeof(NavigationDrawer),
				new PropertyMetadata
				{
					PropertyChangedCallback = OnMaxMenuWidthChanged
				});

			MenuHeaderVisibilityProperty = DependencyProperty.Register(
				nameof(MenuHeaderVisibility),
				typeof(Visibility),
				typeof(NavigationDrawer),
				new PropertyMetadata
				{
					PropertyChangedCallback = OnMenuHeaderVisibilityChanged
				});

			MenuFooterVisibilityProperty = DependencyProperty.Register(
				nameof(MenuFooterVisibility),
				typeof(Visibility),
				typeof(NavigationDrawer),
				new PropertyMetadata
				{
					PropertyChangedCallback = OnMenuFooterVisibilityChanged
				});
		}

		#endregion

		public NavigationDrawer()
		{
			InitializeComponent();
						
			TopBarBackground = new SolidColorBrush(Color.FromArgb(0xff, 0x4a, 0x76, 0xc9));
			MenuBackground = new SolidColorBrush(Color.FromArgb(0xff, 0x4a, 0x76, 0xc9));
			ShadowColor = Color.FromArgb(0x7f, 0x22, 0x22, 0x22);
			ButtonMenuColor = Color.FromArgb(0xff, 0x4a, 0x76, 0xc9);
			ButtonMenuHoverColor = Colors.DeepSkyBlue;

			ButtonMenuColorChangeDuration = TimeSpan.FromMilliseconds(250);
			MaxMenuWidth = Double.NaN;

			ButtonMenu.Click += OpenMenuButton_Click;
			ShadowArea.MouseDown += Shadow_MouseDown;
			ShadowArea.Visibility = Visibility.Collapsed;
		}

		#region Свойства-обертки

		public object TopBar
		{
			get => GetValue(TopBarProperty);
			set => SetValue(TopBarProperty, value);
		}

		public object MenuHeader
		{
			get => GetValue(MenuHeaderProperty);
			set => SetValue(MenuHeaderProperty, value);
		}

		public object Menu
		{
			get => GetValue(MenuProperty);
			set => SetValue(MenuProperty, value);
		}
		
		public object MenuFooter
		{
			get => GetValue(MenuFooterProperty);
			set => SetValue(MenuFooterProperty, value);
		}

		public new object Content
		{
			get => GetValue(ContentProperty);
			set => SetValue(ContentProperty, value);
		}

		public Brush TopBarBackground
		{
			get => (Brush)GetValue(TopBarBackgroundProperty);
			set => SetValue(TopBarBackgroundProperty, value);
		}

		public Brush MenuBackground
		{
			get => (Brush)GetValue(MenuBackgroundProperty);
			set => SetValue(MenuBackgroundProperty, value);
		}

		public Color ShadowColor
		{
			get => (Color)GetValue(ShadowColorProperty);
			set => SetValue(ShadowColorProperty, value);
		}

		public Color ButtonMenuColor
		{
			get => (Color)GetValue(ButtonMenuColorProperty);
			set => SetValue(ButtonMenuColorProperty, value);
		}

		public Color ButtonMenuHoverColor
		{
			get => (Color)GetValue(ButtonMenuHoverColorProperty);
			set => SetValue(ButtonMenuHoverColorProperty, value);
		}

		public bool IsMenuOpen
		{
			get => (bool)GetValue(IsMenuOpenProperty);
			set => SetValue(IsMenuOpenProperty, value);
		}

		public Duration ButtonMenuColorChangeDuration
		{
			get => (Duration)GetValue(ButtonMenuColorChangeDurationProperty);
			set => SetValue(ButtonMenuColorChangeDurationProperty, value);
		}

		public double MinMenuWidth
		{
			get => (double)GetValue(MinMenuWidthProperty);
			set => SetValue(MinMenuWidthProperty, value);
		}

		public double MaxMenuWidth
		{
			get => (double)GetValue(MaxMenuWidthProperty);
			set => SetValue(MaxMenuWidthProperty, value);
		}

		public Visibility MenuHeaderVisibility
		{
			get => (Visibility)GetValue(MenuHeaderVisibilityProperty);
			set => SetValue(MenuHeaderVisibilityProperty, value);
		}

		public Visibility MenuFooterVisibility
		{
			get => (Visibility)GetValue(MenuFooterVisibilityProperty);
			set => SetValue(MenuFooterVisibilityProperty, value);
		}

		#endregion

		#region Методы изменения свойств зависимости

		private static void OnTopBarChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			if (sender is NavigationDrawer navigationDrawer)
			{
				navigationDrawer.TopBarArea.Content = navigationDrawer.TopBar;
			}
		}

		private static void OnMenuHeaderChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			if (sender is NavigationDrawer navigationDrawer)
			{
				navigationDrawer.MenuHeaderArea.Content = navigationDrawer.MenuHeader;
			}
		}

		private static void OnMenuChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			if (sender is NavigationDrawer navigationDrawer)
			{
				navigationDrawer.MenuArea.Content = navigationDrawer.Menu;
			}
		}

		private static void OnMenuFooterChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			if (sender is NavigationDrawer navigationDrawer)
			{
				navigationDrawer.MenuFooterArea.Content = navigationDrawer.MenuFooter;
			}
		}

		private static void OnContentChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			if (sender is NavigationDrawer navigationDrawer)
			{
				navigationDrawer.ContentArea.Content = navigationDrawer.Content;
			}
		}

		private static void OnTopBarBackgroundChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			if (sender is NavigationDrawer navigationDrawer)
			{
				navigationDrawer.TopBarAreaBorder.Background = navigationDrawer.TopBarBackground;
			}
		}

		private static void OnMenuBackgroundChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			if (sender is NavigationDrawer navigationDrawer)
			{
				navigationDrawer.MenuAreaBorder.Background = navigationDrawer.MenuBackground;
			}
		}

		private static void OnShadowColorChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			if (sender is NavigationDrawer navigationDrawer)
			{
				navigationDrawer.ShadowArea.Background = new SolidColorBrush(navigationDrawer.ShadowColor);
			}
		}

		private static void OnButtonMenuColorChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			if (sender is NavigationDrawer navigationDrawer)
			{
				navigationDrawer.ButtonMenu.Background = new SolidColorBrush(navigationDrawer.ButtonMenuColor);
			}
		}

		private static void OnIsMenuOpenChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{		
			if (sender is NavigationDrawer navigationDrawer)
			{
				if (navigationDrawer.IsMenuOpen)
				{
					navigationDrawer.MenuAreaBorder.Width = navigationDrawer.MaxMenuWidth;
					navigationDrawer.ShadowArea.Visibility = Visibility.Visible;
				}
				else
				{
					navigationDrawer.MenuAreaBorder.Width = navigationDrawer.MinMenuWidth;
					navigationDrawer.ShadowArea.Visibility = Visibility.Collapsed;
				}
			}			
		}

		private static void OnMinMenuWidthChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			if (sender is NavigationDrawer navigationDrawer)
			{
				navigationDrawer.MenuColumn.Width = new GridLength(navigationDrawer.MinMenuWidth);
				navigationDrawer.ButtonMenu.Width = navigationDrawer.MinMenuWidth;

				if (!navigationDrawer.IsMenuOpen)
				{
					navigationDrawer.MenuAreaBorder.Width = navigationDrawer.MinMenuWidth;
				}				
			}
		}

		private static void OnMaxMenuWidthChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			if (sender is NavigationDrawer navigationDrawer)
			{
				if (navigationDrawer.IsMenuOpen)
				{
					navigationDrawer.MenuAreaBorder.Width = navigationDrawer.MaxMenuWidth;
				}
			}
		}

		private static void OnMenuHeaderVisibilityChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			if (sender is NavigationDrawer navigationDrawer)
			{
				navigationDrawer.MenuHeaderArea.Visibility = navigationDrawer.MenuHeaderVisibility;
			}
		}

		private static void OnMenuFooterVisibilityChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			if (sender is NavigationDrawer navigationDrawer)
			{
				navigationDrawer.MenuFooterArea.Visibility = navigationDrawer.MenuFooterVisibility;
			}
		}

		#endregion

		private void OpenMenuButton_Click(object sender, RoutedEventArgs e)
		{
			IsMenuOpen = !IsMenuOpen;
		}

		private void Shadow_MouseDown(object sender, MouseButtonEventArgs e)
		{
			IsMenuOpen = false;
		}
	}
}