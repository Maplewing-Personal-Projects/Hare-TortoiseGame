using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HareTortoiseGame.Common
{
    /// <summary>
    /// SuspensionManager 會擷取全域工作階段狀態，以簡化應用程式的處理程序
    /// 生命週期管理。請注意，在各種不同情況下，工作階段狀態會自動清除，
    /// 所以應該只用來儲存方便在各工作階段之間傳遞、
    /// 而且應用程式關閉或升級時就應捨棄的
    /// 資訊。
    /// </summary>
    internal sealed class SuspensionManager
    {
        private static Dictionary<string, object> _sessionState = new Dictionary<string, object>();
        private static List<Type> _knownTypes = new List<Type>();
        private const string sessionStateFilename = "_sessionState.xml";

        /// <summary>
        /// 提供存取目前工作階段的全域工作階段狀態。這個狀態
        /// 由 <see cref="SaveAsync"/> 序列化，並由
        /// <see cref="RestoreAsync"/> 還原，所以值必須可由
        /// <see cref="DataContractSerializer"/> 序列化，而且應該盡可能精簡。強烈
        /// 建議使用字串和其他獨立的資料型別。
        /// </summary>
        public static Dictionary<string, object> SessionState
        {
            get { return _sessionState; }
        }

        /// <summary>
        /// 讀取和寫入工作階段狀態時，提供給 <see cref="DataContractSerializer"/>
        /// 的自訂型別清單。最開始是空白，可加入其他型別
        /// 以自訂序列化程序。
        /// </summary>
        public static List<Type> KnownTypes
        {
            get { return _knownTypes; }
        }

        /// <summary>
        /// 儲存目前的 <see cref="SessionState"/>。任何 <see cref="Frame"/> 執行個體
        /// 若在 <see cref="RegisterFrame"/> 中註冊，也會保留其目前
        /// 導覽堆疊，從而使它們的使用中 <see cref="Page"/> 有機會
        /// 儲存狀態。
        /// </summary>
        /// <returns>反映何時儲存工作階段狀態的非同步工作。</returns>
        public static async Task SaveAsync()
        {
            try
            {
            // 儲存所有已註冊框架的導覽狀態
            foreach (var weakFrameReference in _registeredFrames)
            {
                Frame frame;
                if (weakFrameReference.TryGetTarget(out frame))
                {
                    SaveFrameNavigationState(frame);
                }
            }

            // 同步序列化工作階段狀態，以免非同步存取共用
            // 狀態
            MemoryStream sessionData = new MemoryStream();
            DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<string, object>), _knownTypes);
            serializer.WriteObject(sessionData, _sessionState);
            
                // 取得 SessionState 檔的輸出資料流，並以非同步方式寫入狀態
                StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(sessionStateFilename, CreationCollisionOption.ReplaceExisting);
                using (Stream fileStream = await file.OpenStreamForWriteAsync())
                {
                    sessionData.Seek(0, SeekOrigin.Begin);
                    await sessionData.CopyToAsync(fileStream);
                    await fileStream.FlushAsync();
                }
            }
            catch (Exception e)
            {
                throw new SuspensionManagerException(e);
            }
        }

        /// <summary>
        /// 還原之前儲存 <see cref="SessionState"/>。任何 <see cref="Frame"/> 執行個體
        /// 若在 <see cref="RegisterFrame"/> 中註冊，也會還原其之前的導覽
        /// 狀態，從而使它們的使用中 <see cref="Page"/> 有機會還原
        /// 狀態。
        /// </summary>
        /// <returns>反映何時讀取工作階段狀態的非同步工作。
        /// 不應依賴 <see cref="SessionState"/> 的內容，直到這個工作
        /// 完成。</returns>
        public static async Task RestoreAsync()
        {
            _sessionState = new Dictionary<String, Object>();

            try
            {
                // 取得 SessionState 檔的輸入資料流
                StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(sessionStateFilename);
                using (IInputStream inStream = await file.OpenSequentialReadAsync())
                {
                    // 還原序列化工作階段狀態
                    DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<string, object>), _knownTypes);
                    _sessionState = (Dictionary<string, object>)serializer.ReadObject(inStream.AsStreamForRead());
                }

                // 將任何註冊的框架還原回儲存的狀態
                foreach (var weakFrameReference in _registeredFrames)
                {
                    Frame frame;
                    if (weakFrameReference.TryGetTarget(out frame))
                    {
                        frame.ClearValue(FrameSessionStateProperty);
                        RestoreFrameNavigationState(frame);
                    }
                }
            }
            catch (Exception e)
            {
                throw new SuspensionManagerException(e);
            }
        }

        private static DependencyProperty FrameSessionStateKeyProperty =
            DependencyProperty.RegisterAttached("_FrameSessionStateKey", typeof(String), typeof(SuspensionManager), null);
        private static DependencyProperty FrameSessionStateProperty =
            DependencyProperty.RegisterAttached("_FrameSessionState", typeof(Dictionary<String, Object>), typeof(SuspensionManager), null);
        private static List<WeakReference<Frame>> _registeredFrames = new List<WeakReference<Frame>>();

        /// <summary>
        /// 註冊 <see cref="Frame"/> 執行個體，讓它的導覽記錄能夠儲存到
        /// <see cref="SessionState"/> 或從這裡還原。框架應在建立後立即註冊，
        /// 這樣才能參與工作階段狀態管理。註冊後，
        /// 如果為指定的索引鍵還原狀態，
        /// 就會立即還原導覽記錄。之後引動
        /// <see cref="RestoreAsync"/> 也會還原導覽記錄。
        /// </summary>
        /// <param name="frame">執行個體，其導覽記錄應由
        /// <see cref="SuspensionManager"/> 管理</param>
        /// <param name="sessionStateKey"><see cref="SessionState"/> 的唯一索引鍵，用來
        /// 儲存導覽相關資訊。</param>
        public static void RegisterFrame(Frame frame, String sessionStateKey)
        {
            if (frame.GetValue(FrameSessionStateKeyProperty) != null)
            {
                throw new InvalidOperationException("Frames can only be registered to one session state key");
            }

            if (frame.GetValue(FrameSessionStateProperty) != null)
            {
                throw new InvalidOperationException("Frames must be either be registered before accessing frame session state, or not registered at all");
            }

            // 使用相依性屬性將工作階段索引鍵與框架產生關聯，並保存應管理
            // 導覽狀態的框架清單
            frame.SetValue(FrameSessionStateKeyProperty, sessionStateKey);
            _registeredFrames.Add(new WeakReference<Frame>(frame));

            // 檢查能否還原導覽狀態
            RestoreFrameNavigationState(frame);
        }

        /// <summary>
        /// 將之前由 <see cref="RegisterFrame"/> 註冊的 <see cref="Frame"/>
        /// 與 <see cref="SessionState"/> 取消關聯。之前擷取的任何導覽狀態都將
        /// 移除。
        /// </summary>
        /// <param name="frame">不應該再管理之導覽記錄的
        /// 執行個體。</param>
        public static void UnregisterFrame(Frame frame)
        {
            // 移除工作階段狀態，並將框架從儲存導覽狀態的
            // 框架清單移除 (以及不能夠再使用的任何弱式參考)
            SessionState.Remove((String)frame.GetValue(FrameSessionStateKeyProperty));
            _registeredFrames.RemoveAll((weakFrameReference) =>
            {
                Frame testFrame;
                return !weakFrameReference.TryGetTarget(out testFrame) || testFrame == frame;
            });
        }

        /// <summary>
        /// 針對與指定之 <see cref="Frame"/> 關聯的工作階段狀態，提供儲存體。
        /// 之前在 <see cref="RegisterFrame"/> 中註冊的框架會
        /// 將工作階段狀態自動與全域 <see cref="SessionState"/> 一起
        /// 儲存和還原。未註冊的框架有暫時狀態，這些狀態
        /// 在還原已從導覽快取捨棄的頁面時
        /// 仍然有用。
        /// </summary>
        /// <remarks>應用程式可選擇依賴 <see cref="LayoutAwarePage"/> 來管理
        /// 頁面特定狀態，而不直接處理框架工作階段狀態。</remarks>
        /// <param name="frame">需要工作階段狀態的執行個體。</param>
        /// <returns>狀態集合，受限於與
        /// <see cref="SessionState"/> 相同的序列化機制。</returns>
        public static Dictionary<String, Object> SessionStateForFrame(Frame frame)
        {
            var frameState = (Dictionary<String, Object>)frame.GetValue(FrameSessionStateProperty);

            if (frameState == null)
            {
                var frameSessionKey = (String)frame.GetValue(FrameSessionStateKeyProperty);
                if (frameSessionKey != null)
                {
                    // 註冊的框架會反映對應的工作階段狀態
                    if (!_sessionState.ContainsKey(frameSessionKey))
                    {
                        _sessionState[frameSessionKey] = new Dictionary<String, Object>();
                    }
                    frameState = (Dictionary<String, Object>)_sessionState[frameSessionKey];
                }
                else
                {
                    // 未註冊的框架有暫時狀態
                    frameState = new Dictionary<String, Object>();
                }
                frame.SetValue(FrameSessionStateProperty, frameState);
            }
            return frameState;
        }

        private static void RestoreFrameNavigationState(Frame frame)
        {
            var frameState = SessionStateForFrame(frame);
            if (frameState.ContainsKey("Navigation"))
            {
                frame.SetNavigationState((String)frameState["Navigation"]);
            }
        }

        private static void SaveFrameNavigationState(Frame frame)
        {
            var frameState = SessionStateForFrame(frame);
            frameState["Navigation"] = frame.GetNavigationState();
        }
    }
    public class SuspensionManagerException : Exception
    {
        public SuspensionManagerException()
        {
        }

        public SuspensionManagerException(Exception e) : base("SuspensionManager failed", e)
        {
            
        }
    }
}
