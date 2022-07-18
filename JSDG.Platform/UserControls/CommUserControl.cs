using System.Windows;
using System.Windows.Controls;

namespace JSDG.Platform.UserControls
{
    public class CommUserControl : Control
    {
        /// <summary>
        /// 通信连接状态
        /// </summary>
        public bool Connected
        {
            get { return (bool)GetValue(ConnectedProperty); }
            set { SetValue(ConnectedProperty, value); }
        }

        public static readonly DependencyProperty ConnectedProperty =
            DependencyProperty.Register("Connected", typeof(bool), typeof(CommUserControl), new PropertyMetadata(false));

        /// <summary>
        /// A系 主备状态
        /// </summary>
        public string IsSystemAMaster
        {
            get { return (string)GetValue(IsSystemAMasterProperty); }
            set { SetValue(IsSystemAMasterProperty, value); }
        }

        public static readonly DependencyProperty IsSystemAMasterProperty =
            DependencyProperty.Register("IsSystemAMaster", typeof(string), typeof(CommUserControl), new PropertyMetadata("Unkonwn"));

        /// <summary>
        /// B系 主备状态
        /// </summary>
        public string IsSystemBMaster
        {
            get { return (string)GetValue(IsSystemBMasterProperty); }
            set { SetValue(IsSystemBMasterProperty, value); }
        }

        public static readonly DependencyProperty IsSystemBMasterProperty =
            DependencyProperty.Register("IsSystemBMaster", typeof(string), typeof(CommUserControl), new PropertyMetadata("Unkonwn"));

        /// <summary>
        /// 同步状态
        /// </summary>
        public bool SyncConnected
        {
            get { return (bool)GetValue(SyncConnectedProperty); }
            set { SetValue(SyncConnectedProperty, value); }
        }

        public static readonly DependencyProperty SyncConnectedProperty =
            DependencyProperty.Register("SyncConnected", typeof(bool), typeof(CommUserControl), new PropertyMetadata(false));

        /// <summary>
        /// 显示内容
        /// </summary>
        public object Content
        {
            get { return (object)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(object), typeof(CommUserControl), new PropertyMetadata(string.Empty));

        /// <summary>
        /// 通信类型
        /// </summary>
        public EConnectionType ConnectionType
        {
            get { return (EConnectionType)GetValue(ConnectionTypeProperty); }
            set { SetValue(ConnectionTypeProperty, value); }
        }

        public static readonly DependencyProperty ConnectionTypeProperty =
            DependencyProperty.Register("ConnectionType", typeof(EConnectionType), typeof(CommUserControl), new PropertyMetadata(EConnectionType.ILC));
    }

    /// <summary>
    /// 通信连接类型
    /// </summary>
    public enum EConnectionType
    {
        ILC,
        HMI,
        SystemAB
    }
}
