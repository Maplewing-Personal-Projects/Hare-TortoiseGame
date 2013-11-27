using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;

namespace HareTortoiseGame.Common
{
    /// <summary>
    /// <see cref="RichTextBlock"/> 的包裝函式，可視需要建立足夠容納可用內容所需的
    /// 其他溢位欄。
    /// </summary>
    /// <example>
    /// 以下建立間隔 50 像素的 400 像素寬欄集合，
    /// 以包含任意的資料繫結內容:
    /// <code>
    /// <RichTextColumns>
    ///     <RichTextColumns.ColumnTemplate>
    ///         <DataTemplate>
    ///             <RichTextBlockOverflow Width="400" Margin="50,0,0,0"/>
    ///         </DataTemplate>
    ///     </RichTextColumns.ColumnTemplate>
    ///     
    ///     <RichTextBlock Width="400">
    ///         <Paragraph>
    ///             <Run Text="{Binding Content}"/>
    ///         </Paragraph>
    ///     </RichTextBlock>
    /// </RichTextColumns>
    /// </code>
    /// </example>
    /// <remarks>通常用在水平捲動區域，在這個區域中有無限量的空間
    /// 可容納要建立的所有必要欄。用在垂直捲動的
    /// 空間時，則絕不會有任何其他欄。</remarks>
    [Windows.UI.Xaml.Markup.ContentProperty(Name = "RichTextContent")]
    public sealed class RichTextColumns : Panel
    {
        /// <summary>
        /// 識別 <see cref="RichTextContent"/> 相依性屬性。
        /// </summary>
        public static readonly DependencyProperty RichTextContentProperty =
            DependencyProperty.Register("RichTextContent", typeof(RichTextBlock),
            typeof(RichTextColumns), new PropertyMetadata(null, ResetOverflowLayout));

        /// <summary>
        /// 識別 <see cref="ColumnTemplate"/> 相依性屬性。
        /// </summary>
        public static readonly DependencyProperty ColumnTemplateProperty =
            DependencyProperty.Register("ColumnTemplate", typeof(DataTemplate),
            typeof(RichTextColumns), new PropertyMetadata(null, ResetOverflowLayout));

        /// <summary>
        /// 初始化 <see cref="RichTextColumns"/> 類別的新執行個體。
        /// </summary>
        public RichTextColumns()
        {
            this.HorizontalAlignment = HorizontalAlignment.Left;
        }

        /// <summary>
        /// 取得或設定要做為第一欄的初始 RTF 文字內容。
        /// </summary>
        public RichTextBlock RichTextContent
        {
            get { return (RichTextBlock)GetValue(RichTextContentProperty); }
            set { SetValue(RichTextContentProperty, value); }
        }

        /// <summary>
        /// 取得或設定範本，以用來建立其他
        /// <see cref="RichTextBlockOverflow"/> 執行個體。
        /// </summary>
        public DataTemplate ColumnTemplate
        {
            get { return (DataTemplate)GetValue(ColumnTemplateProperty); }
            set { SetValue(ColumnTemplateProperty, value); }
        }

        /// <summary>
        /// 在內容或溢位範本變更以重新建立欄配置時叫用。
        /// </summary>
        /// <param name="d">發生變更的 <see cref="RichTextColumns"/>
        /// 執行個體。</param>
        /// <param name="e">描述特定變更的事件資料。</param>
        private static void ResetOverflowLayout(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // 發生重大變更時，從頭開始重建欄配置
            var target = d as RichTextColumns;
            if (target != null)
            {
                target._overflowColumns = null;
                target.Children.Clear();
                target.InvalidateMeasure();
            }
        }

        /// <summary>
        /// 列出已建立的溢位欄。必須與初始 RichTextBlock 子系
        /// 後的 <see cref="Panel.Children"/> 集合中的執行個體
        /// 保持 1:1 的關係。
        /// </summary>
        private List<RichTextBlockOverflow> _overflowColumns = null;

        /// <summary>
        /// 判斷是否需要其他溢位欄，以及能否移除
        /// 現有欄。
        /// </summary>
        /// <param name="availableSize">可用空間大小，用來限制
        /// 可建立的其他欄數。</param>
        /// <returns>原始內容產生的大小加上任何額外的欄。</returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            if (this.RichTextContent == null) return new Size(0, 0);

            // 確定 RichTextBlock 是子系，利用沒有
            // 其他欄清單，來表示這一項
            // 尚未完成
            if (this._overflowColumns == null)
            {
                Children.Add(this.RichTextContent);
                this._overflowColumns = new List<RichTextBlockOverflow>();
            }

            // 以測量原始 RichTextBlock 內容做為開始
            this.RichTextContent.Measure(availableSize);
            var maxWidth = this.RichTextContent.DesiredSize.Width;
            var maxHeight = this.RichTextContent.DesiredSize.Height;
            var hasOverflow = this.RichTextContent.HasOverflowContent;

            // 確定有足夠的溢位欄
            int overflowIndex = 0;
            while (hasOverflow && maxWidth < availableSize.Width && this.ColumnTemplate != null)
            {
                // 使用現有溢位欄，直到用完為止，之後根據
                // 所提供的範本建立更多欄
                RichTextBlockOverflow overflow;
                if (this._overflowColumns.Count > overflowIndex)
                {
                    overflow = this._overflowColumns[overflowIndex];
                }
                else
                {
                    overflow = (RichTextBlockOverflow)this.ColumnTemplate.LoadContent();
                    this._overflowColumns.Add(overflow);
                    this.Children.Add(overflow);
                    if (overflowIndex == 0)
                    {
                        this.RichTextContent.OverflowContentTarget = overflow;
                    }
                    else
                    {
                        this._overflowColumns[overflowIndex - 1].OverflowContentTarget = overflow;
                    }
                }

                // 測量新欄，並準備視需要重複
                overflow.Measure(new Size(availableSize.Width - maxWidth, availableSize.Height));
                maxWidth += overflow.DesiredSize.Width;
                maxHeight = Math.Max(maxHeight, overflow.DesiredSize.Height);
                hasOverflow = overflow.HasOverflowContent;
                overflowIndex++;
            }

            // 將額外的欄從溢位鏈結中斷連接，將它們從我們的欄私用清單
            // 移除，並以子系方式移除
            if (this._overflowColumns.Count > overflowIndex)
            {
                if (overflowIndex == 0)
                {
                    this.RichTextContent.OverflowContentTarget = null;
                }
                else
                {
                    this._overflowColumns[overflowIndex - 1].OverflowContentTarget = null;
                }
                while (this._overflowColumns.Count > overflowIndex)
                {
                    this._overflowColumns.RemoveAt(overflowIndex);
                    this.Children.RemoveAt(overflowIndex + 1);
                }
            }

            // 報告最終決定的大小
            return new Size(maxWidth, maxHeight);
        }

        /// <summary>
        /// 排列原始內容和所有額外的欄。
        /// </summary>
        /// <param name="finalSize">定義用以排列子系的區域
        /// 大小。</param>
        /// <returns>子系實際需要的區域大小。</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            double maxWidth = 0;
            double maxHeight = 0;
            foreach (var child in Children)
            {
                child.Arrange(new Rect(maxWidth, 0, child.DesiredSize.Width, finalSize.Height));
                maxWidth += child.DesiredSize.Width;
                maxHeight = Math.Max(maxHeight, child.DesiredSize.Height);
            }
            return new Size(maxWidth, maxHeight);
        }
    }
}
