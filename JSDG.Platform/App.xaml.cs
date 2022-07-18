using System.Windows;

namespace JSDG.Platform
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// 容器
        /// </summary>
        public static Container Container { get; private set; }

        /// <summary>
        /// 初始化容器
        /// </summary>
        static App()
        {
            Container = new Container();
        }
    }
}
