using System.Windows;
using System.Windows.Controls;

namespace JSDG.Platform.UserControls
{
    /// <summary>
    /// 自定义ComboBox
    /// </summary>
    public class CustomComboBox : ComboBox
    {
        /// <summary>
        /// 名称显示
        /// </summary>
        public string DisplayName
        {
            get { return (string)GetValue(DisplayNameProperty); }
            set { SetValue(DisplayNameProperty, value); }
        }

        public static readonly DependencyProperty DisplayNameProperty =
            DependencyProperty.Register("DisplayName", typeof(string), typeof(CustomComboBox), new PropertyMetadata(string.Empty));
    }
}
