using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace HareTortoiseGame.Common
{
    /// <summary>
    /// 頁面的一般實作，提供幾項重要的便利功能:
    /// <list type="bullet">
    /// <item>
    /// <description>應用程式檢視狀態對視覺狀態的對應</description>
    /// </item>
    /// <item>
    /// <description>GoBack、GoForward 和 GoHome 事件處理常式</description>
    /// </item>
    /// <item>
    /// <description>用於巡覽的滑鼠和鍵盤快速鍵</description>
    /// </item>
    /// <item>
    /// <description>用於巡覽和處理程序生命週期管理的狀態管理</description>
    /// </item>
    /// <item>
    /// <description>預設檢視模式</description>
    /// </item>
    /// </list>
    /// </summary>
    [Windows.Foundation.Metadata.WebHostHidden]
    public class LayoutAwarePage : Page
    {
        /// <summary>
        /// 識別 <see cref="DefaultViewModel"/> 相依性屬性。
        /// </summary>
        public static readonly DependencyProperty DefaultViewModelProperty =
            DependencyProperty.Register("DefaultViewModel", typeof(IObservableMap<String, Object>),
            typeof(LayoutAwarePage), null);

        private List<Control> _layoutAwareControls;

        /// <summary>
        /// 初始化 <see cref="LayoutAwarePage"/> 類別的新執行個體。
        /// </summary>
        public LayoutAwarePage()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled) return;

            // 建立空的預設檢視模式
            this.DefaultViewModel = new ObservableDictionary<String, Object>();

            // 當這個頁面是視覺化樹狀結構的一部分時，執行兩項變更:
            // 1) 將應用程式檢視狀態對應到頁面的視覺狀態
            // 2) 處理鍵盤和滑鼠巡覽要求
            this.Loaded += (sender, e) =>
            {
                this.StartLayoutUpdates(sender, e);

                // 只有佔用整個視窗時才適用鍵盤和滑鼠巡覽
                if (this.ActualHeight == Window.Current.Bounds.Height &&
                    this.ActualWidth == Window.Current.Bounds.Width)
                {
                    // 直接接聽視窗，所以不需要焦點
                    Window.Current.CoreWindow.Dispatcher.AcceleratorKeyActivated +=
                        CoreDispatcher_AcceleratorKeyActivated;
                    Window.Current.CoreWindow.PointerPressed +=
                        this.CoreWindow_PointerPressed;
                }
            };

            // 當頁面不再可見時，復原相同的變更
            this.Unloaded += (sender, e) =>
            {
                this.StopLayoutUpdates(sender, e);
                Window.Current.CoreWindow.Dispatcher.AcceleratorKeyActivated -=
                    CoreDispatcher_AcceleratorKeyActivated;
                Window.Current.CoreWindow.PointerPressed -=
                    this.CoreWindow_PointerPressed;
            };
        }

        /// <summary>
        /// <see cref="IObservableMap&lt;String, Object&gt;"/> 的實作，其設計目的在於
        /// 做為 trivial 檢視模式。
        /// </summary>
        protected IObservableMap<String, Object> DefaultViewModel
        {
            get
            {
                return this.GetValue(DefaultViewModelProperty) as IObservableMap<String, Object>;
            }

            set
            {
                this.SetValue(DefaultViewModelProperty, value);
            }
        }

        #region 巡覽支援

        /// <summary>
        /// 以事件處理常式方式叫用，以在頁面的關聯 <see cref="Frame"/>
        /// 向後巡覽，直到它到達巡覽堆疊最上方為止。
        /// </summary>
        /// <param name="sender">觸發事件的執行個體。</param>
        /// <param name="e">描述造成事件之狀況的事件資料。</param>
        protected virtual void GoHome(object sender, RoutedEventArgs e)
        {
            // 使用巡覽框架返回頁面最上面一頁
            if (this.Frame != null)
            {
                while (this.Frame.CanGoBack) this.Frame.GoBack();
            }
        }

        /// <summary>
        /// 以事件處理常式方式叫用，以向後巡覽這個頁面的
        /// <see cref="Frame"/> 關聯的巡覽堆疊。
        /// </summary>
        /// <param name="sender">觸發事件的執行個體。</param>
        /// <param name="e">描述造成事件之狀況的事件
        /// 資料。</param>
        protected virtual void GoBack(object sender, RoutedEventArgs e)
        {
            // 使用巡覽框架返回上一頁
            if (this.Frame != null && this.Frame.CanGoBack) this.Frame.GoBack();
        }

        /// <summary>
        /// 以事件處理常式方式叫用，以向前巡覽這個頁面的
        /// <see cref="Frame"/> 關聯的巡覽堆疊。
        /// </summary>
        /// <param name="sender">觸發事件的執行個體。</param>
        /// <param name="e">描述造成事件之狀況的事件
        /// 資料。</param>
        protected virtual void GoForward(object sender, RoutedEventArgs e)
        {
            // 使用巡覽框架移到下一頁
            if (this.Frame != null && this.Frame.CanGoForward) this.Frame.GoForward();
        }

        /// <summary>
        /// 當這個頁面作用中且佔用整個視窗時，在按下每個按鍵時叫用，
        /// 包括如 Alt 鍵組合等系統按鍵。用來偵測頁面之間的鍵盤巡覽，
        /// 即使頁面本身沒有焦點。
        /// </summary>
        /// <param name="sender">觸發事件的執行個體。</param>
        /// <param name="args">描述造成事件之狀況的事件資料。</param>
        private void CoreDispatcher_AcceleratorKeyActivated(CoreDispatcher sender,
            AcceleratorKeyEventArgs args)
        {
            var virtualKey = args.VirtualKey;

            // 只在按下左、右或專用的 [上一頁] 或 [下一頁] 按鍵時才
            // 進一步調查
            if ((args.EventType == CoreAcceleratorKeyEventType.SystemKeyDown ||
                args.EventType == CoreAcceleratorKeyEventType.KeyDown) &&
                (virtualKey == VirtualKey.Left || virtualKey == VirtualKey.Right ||
                (int)virtualKey == 166 || (int)virtualKey == 167))
            {
                var coreWindow = Window.Current.CoreWindow;
                var downState = CoreVirtualKeyStates.Down;
                bool menuKey = (coreWindow.GetKeyState(VirtualKey.Menu) & downState) == downState;
                bool controlKey = (coreWindow.GetKeyState(VirtualKey.Control) & downState) == downState;
                bool shiftKey = (coreWindow.GetKeyState(VirtualKey.Shift) & downState) == downState;
                bool noModifiers = !menuKey && !controlKey && !shiftKey;
                bool onlyAlt = menuKey && !controlKey && !shiftKey;

                if (((int)virtualKey == 166 && noModifiers) ||
                    (virtualKey == VirtualKey.Left && onlyAlt))
                {
                    // 按下上一頁按鍵或 Alt+Left 時，向後巡覽
                    args.Handled = true;
                    this.GoBack(this, new RoutedEventArgs());
                }
                else if (((int)virtualKey == 167 && noModifiers) ||
                    (virtualKey == VirtualKey.Right && onlyAlt))
                {
                    // 按下下一頁按鍵或 Alt+Right 時，向前巡覽
                    args.Handled = true;
                    this.GoForward(this, new RoutedEventArgs());
                }
            }
        }

        /// <summary>
        /// 當這個頁面作用中且佔用整個視窗時，在每個滑鼠點按、觸控螢幕點選
        /// 或對等的互動時叫用。用來偵測瀏覽器的下一頁和
        /// 上一頁滑鼠按鈕點選，以在頁面之間巡覽。
        /// </summary>
        /// <param name="sender">觸發事件的執行個體。</param>
        /// <param name="args">描述造成事件之狀況的事件資料。</param>
        private void CoreWindow_PointerPressed(CoreWindow sender,
            PointerEventArgs args)
        {
            var properties = args.CurrentPoint.Properties;

            // 忽略含左、右和中間按鈕的按鈕同步選取
            if (properties.IsLeftButtonPressed || properties.IsRightButtonPressed ||
                properties.IsMiddleButtonPressed) return;

            // 如果按下上一頁或下一頁 (但不是兩者都按)，即適當巡覽
            bool backPressed = properties.IsXButton1Pressed;
            bool forwardPressed = properties.IsXButton2Pressed;
            if (backPressed ^ forwardPressed)
            {
                args.Handled = true;
                if (backPressed) this.GoBack(this, new RoutedEventArgs());
                if (forwardPressed) this.GoForward(this, new RoutedEventArgs());
            }
        }

        #endregion

        #region 視覺狀態切換

        /// <summary>
        /// 以事件處理常式方式叫用，通常發生在頁面內 <see cref="Control"/> 的
        /// <see cref="FrameworkElement.Loaded"/> 事件上，以指示傳送者應
        /// 開始接收對應到應用程式檢視狀態變更的視覺狀態
        /// 管理變更。
        /// </summary>
        /// <param name="sender"><see cref="Control"/> 的執行個體，可支援對應到
        /// 檢視狀態的視覺狀態管理。</param>
        /// <param name="e">描述如何提出要求的事件資料。</param>
        /// <remarks>要求配置更新時，將立即使用目前檢視狀態來
        /// 設定對應的視覺狀態。強烈建議使用連接到
        /// <see cref="StopLayoutUpdates"/> 的對應 <see cref="FrameworkElement.Unloaded"/>
        /// 事件處理常式。<see cref="LayoutAwarePage"/> 的執行個體
        /// 會自動在 Loaded 和 Unloaded 事件中叫用
        /// 這些處理常式。</remarks>
        /// <seealso cref="DetermineVisualState"/>
        /// <seealso cref="InvalidateVisualState"/>
        public void StartLayoutUpdates(object sender, RoutedEventArgs e)
        {
            var control = sender as Control;
            if (control == null) return;
            if (this._layoutAwareControls == null)
            {
                // 當控制項有意更新時，開始接聽檢視狀態變更
                Window.Current.SizeChanged += this.WindowSizeChanged;
                this._layoutAwareControls = new List<Control>();
            }
            this._layoutAwareControls.Add(control);

            // 設定控制項的初始視覺狀態
            VisualStateManager.GoToState(control, DetermineVisualState(ApplicationView.Value), false);
        }

        private void WindowSizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            this.InvalidateVisualState();
        }

        /// <summary>
        /// 以事件處理常式方式叫用，通常發生在 <see cref="Control"/> 的
        /// <see cref="FrameworkElement.Unloaded"/> 事件上，以指示傳送者應開始接收對應到
        /// 應用程式檢視狀態變更的視覺狀態管理變更。
        /// </summary>
        /// <param name="sender"><see cref="Control"/> 的執行個體，可支援對應到
        /// 檢視狀態的視覺狀態管理。</param>
        /// <param name="e">描述如何提出要求的事件資料。</param>
        /// <remarks>要求配置更新時，將立即使用目前檢視狀態來
        /// 設定對應的視覺狀態。</remarks>
        /// <seealso cref="StartLayoutUpdates"/>
        public void StopLayoutUpdates(object sender, RoutedEventArgs e)
        {
            var control = sender as Control;
            if (control == null || this._layoutAwareControls == null) return;
            this._layoutAwareControls.Remove(control);
            if (this._layoutAwareControls.Count == 0)
            {
                // 當沒有控制項有意更新時，停止接聽檢視狀態變更
                this._layoutAwareControls = null;
                Window.Current.SizeChanged -= this.WindowSizeChanged;
            }
        }

        /// <summary>
        /// 將 <see cref="ApplicationViewState"/> 值轉譯成字串，以進行頁面內的
        /// 視覺狀態管理。預設實作會使用列舉值的名稱。
        /// 子類別可覆寫這個方法，以控制所用的對應配置。
        /// </summary>
        /// <param name="viewState">需要其視覺狀態的檢視狀態。</param>
        /// <returns>用來驅動 <see cref="VisualStateManager"/> 的
        /// 視覺狀態名稱</returns>
        /// <seealso cref="InvalidateVisualState"/>
        protected virtual string DetermineVisualState(ApplicationViewState viewState)
        {
            return viewState.ToString();
        }

        /// <summary>
        /// 更新所有接聽具有正確視覺狀態之視覺狀態
        /// 變更的控制項。
        /// </summary>
        /// <remarks>
        /// 通常搭配覆寫 <see cref="DetermineVisualState"/> 一起使用，以
        /// 指示雖然檢視狀態未變更，仍可能傳回
        /// 不同值。
        /// </remarks>
        public void InvalidateVisualState()
        {
            if (this._layoutAwareControls != null)
            {
                string visualState = DetermineVisualState(ApplicationView.Value);
                foreach (var layoutAwareControl in this._layoutAwareControls)
                {
                    VisualStateManager.GoToState(layoutAwareControl, visualState, false);
                }
            }
        }

        #endregion

        #region 處理程序生命週期管理

        private String _pageKey;

        /// <summary>
        /// 在此頁面即將顯示在框架中時叫用。
        /// </summary>
        /// <param name="e">描述如何到達此頁面的事件資料。Parameter
        /// 屬性會提供要顯示之群組。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // 透過巡覽返回快取的頁面不應該觸發狀態載入
            if (this._pageKey != null) return;

            var frameState = SuspensionManager.SessionStateForFrame(this.Frame);
            this._pageKey = "Page-" + this.Frame.BackStackDepth;

            if (e.NavigationMode == NavigationMode.New)
            {
                // 有新頁面加入巡覽堆疊時，清除向前巡覽的
                // 現有狀態
                var nextPageKey = this._pageKey;
                int nextPageIndex = this.Frame.BackStackDepth;
                while (frameState.Remove(nextPageKey))
                {
                    nextPageIndex++;
                    nextPageKey = "Page-" + nextPageIndex;
                }

                // 將巡覽參數傳遞給新頁面
                this.LoadState(e.Parameter, null);
            }
            else
            {
                // 將巡覽參數傳遞和保留的頁面狀態傳遞給頁面，且使用
                // 與載入暫停狀態和根據快取重新建立捨棄的頁面
                // 一樣的策略
                this.LoadState(e.Parameter, (Dictionary<String, Object>)frameState[this._pageKey]);
            }
        }

        /// <summary>
        /// 在框架中不再顯示這個頁面時叫用。
        /// </summary>
        /// <param name="e">描述如何到達此頁面的事件資料。Parameter
        /// 屬性會提供要顯示之群組。</param>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            var frameState = SuspensionManager.SessionStateForFrame(this.Frame);
            var pageState = new Dictionary<String, Object>();
            this.SaveState(pageState);
            frameState[_pageKey] = pageState;
        }

        /// <summary>
        /// 巡覽期間以傳遞的內容填入頁面。從之前的工作階段
        /// 重新建立頁面時，也會提供儲存的狀態。
        /// </summary>
        /// <param name="navigationParameter">最初要求這個頁面時，傳遞到
        /// <see cref="Frame.Navigate(Type, Object)"/> 的參數。
        /// </param>
        /// <param name="pageState">這個頁面在先前的工作階段期間保留的
        /// 狀態字典。第一次瀏覽頁面時，這一項是 null。</param>
        protected virtual void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
        }

        /// <summary>
        /// 在應用程式暫停或從巡覽快取中捨棄頁面時，
        /// 保留與這個頁面關聯的狀態。值必須符合
        /// <see cref="SuspensionManager.SessionState"/> 的序列化需求。
        /// </summary>
        /// <param name="pageState">即將以可序列化狀態填入的空白字典。</param>
        protected virtual void SaveState(Dictionary<String, Object> pageState)
        {
        }

        #endregion

        /// <summary>
        /// 支援重新進入的 IObservableMap 實作，以做為預設檢視
        /// 模型。
        /// </summary>
        private class ObservableDictionary<K, V> : IObservableMap<K, V>
        {
            private class ObservableDictionaryChangedEventArgs : IMapChangedEventArgs<K>
            {
                public ObservableDictionaryChangedEventArgs(CollectionChange change, K key)
                {
                    this.CollectionChange = change;
                    this.Key = key;
                }

                public CollectionChange CollectionChange { get; private set; }
                public K Key { get; private set; }
            }

            private Dictionary<K, V> _dictionary = new Dictionary<K, V>();
            public event MapChangedEventHandler<K, V> MapChanged;

            private void InvokeMapChanged(CollectionChange change, K key)
            {
                var eventHandler = MapChanged;
                if (eventHandler != null)
                {
                    eventHandler(this, new ObservableDictionaryChangedEventArgs(change, key));
                }
            }

            public void Add(K key, V value)
            {
                this._dictionary.Add(key, value);
                this.InvokeMapChanged(CollectionChange.ItemInserted, key);
            }

            public void Add(KeyValuePair<K, V> item)
            {
                this.Add(item.Key, item.Value);
            }

            public bool Remove(K key)
            {
                if (this._dictionary.Remove(key))
                {
                    this.InvokeMapChanged(CollectionChange.ItemRemoved, key);
                    return true;
                }
                return false;
            }

            public bool Remove(KeyValuePair<K, V> item)
            {
                V currentValue;
                if (this._dictionary.TryGetValue(item.Key, out currentValue) &&
                    Object.Equals(item.Value, currentValue) && this._dictionary.Remove(item.Key))
                {
                    this.InvokeMapChanged(CollectionChange.ItemRemoved, item.Key);
                    return true;
                }
                return false;
            }

            public V this[K key]
            {
                get
                {
                    return this._dictionary[key];
                }
                set
                {
                    this._dictionary[key] = value;
                    this.InvokeMapChanged(CollectionChange.ItemChanged, key);
                }
            }

            public void Clear()
            {
                var priorKeys = this._dictionary.Keys.ToArray();
                this._dictionary.Clear();
                foreach (var key in priorKeys)
                {
                    this.InvokeMapChanged(CollectionChange.ItemRemoved, key);
                }
            }

            public ICollection<K> Keys
            {
                get { return this._dictionary.Keys; }
            }

            public bool ContainsKey(K key)
            {
                return this._dictionary.ContainsKey(key);
            }

            public bool TryGetValue(K key, out V value)
            {
                return this._dictionary.TryGetValue(key, out value);
            }

            public ICollection<V> Values
            {
                get { return this._dictionary.Values; }
            }

            public bool Contains(KeyValuePair<K, V> item)
            {
                return this._dictionary.Contains(item);
            }

            public int Count
            {
                get { return this._dictionary.Count; }
            }

            public bool IsReadOnly
            {
                get { return false; }
            }

            public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
            {
                return this._dictionary.GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return this._dictionary.GetEnumerator();
            }

            public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
            {
                int arraySize = array.Length;
                foreach (var pair in this._dictionary)
                {
                    if (arrayIndex >= arraySize) break;
                    array[arrayIndex++] = pair;
                }
            }
        }
    }
}
