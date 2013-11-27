using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml.Data;

namespace HareTortoiseGame.Common
{
    /// <summary>
    /// 實作 <see cref="INotifyPropertyChanged"/> 以簡化模型。
    /// </summary>
    [Windows.Foundation.Metadata.WebHostHidden]
    public abstract class BindableBase : INotifyPropertyChanged
    {
        /// <summary>
        /// 屬性變更通知的多點傳送事件。
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 檢查屬性是否已符合所需值。只在需要時設定
        /// 屬性和通知接聽程式。
        /// </summary>
        /// <typeparam name="T">屬性的型別。</typeparam>
        /// <param name="storage">參考到具有 getter 和 setter 的屬性。</param>
        /// <param name="value">屬性的所需值。</param>
        /// <param name="propertyName">用來通知接聽程式的屬性名稱。這是
        /// 選擇性值，可在從編譯器叫用時自動提供，但該編譯器必須
        /// 支援 CallerMemberName。</param>
        /// <returns>如果值已變更，即為 True，如果現有值符合所需值，
        /// 則為 false。</returns>
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] String propertyName = null)
        {
            if (object.Equals(storage, value)) return false;

            storage = value;
            this.OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// 通知接聽程式有屬性值已變更。
        /// </summary>
        /// <param name="propertyName">用來通知接聽程式的屬性名稱。這是
        /// 選擇性值，可在從編譯器叫用時自動提供，但該編譯器必須
        /// 支援 <see cref="CallerMemberNameAttribute"/>。</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var eventHandler = this.PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
