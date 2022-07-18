using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Windows.Forms;

namespace JSDG.Platform
{
    public class Container
    {
        [Import("JSDG.ATS")]
        public Action<String, String[], Action<Int32, String>> Act_JSDG_ATS { get; private set; }

        [Import("JSDG.ES")]
        public Action<String, String, String, String[], Action<Int32, String>> Act_JSDG_ES { get; private set; }

        [Import("JSDG.CBI.A")]
        public Action<String, String[], Action<Int32, String>> Act_JSDG_CBI_A { get; private set; }

        [Import("JSDG.CBI.B")]
        public Action<String, String[], Action<Int32, String>> Act_JSDG_CBI_B { get; private set; }

        [Import("JSDG.OBCU.A")]
        public Action<String, String[], Action<Int32, String>> Act_JSDG_OBCU_A { get; private set; }

        [Import("JSDG.OBCU.B")]
        public Action<String, String[], Action<Int32, String>> Act_JSDG_OBCU_B { get; private set; }

        [Import("TCC_TCC.A")]
        public Action<String, String[], Action<Int32, String>> Act_TCC_TCC_A { get; private set; }

        [Import("JSDG.ZC.B")]
        public Action<String, String[], Action<Int32, String>> Act_JSDG_ZC_B { get; private set; }

        [Import("JSDG.POCT_B")]
        public Action<String, String[], Action<Int32, String>, Dictionary<string, string>> Act_JSDG_POCT_B { get; private set; }

        [Import("JSDG.POCT_A")]
        public Action<String, String[], Action<Int32, String>, Dictionary<string, string>> Act_JSDG_POCT_A { get; private set; }

        [Import("JSDG.MMS")]
        public Form Form_JSDG_MMS { get; private set; }

        [Import("JSDG.Telegram")]
        public Action<String, String, String, String, String, Action<Int32, String>> Act_JSDG_Telegram { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public Container()
        {
            DirectoryCatalog catalog = new DirectoryCatalog(AppDomain.CurrentDomain.BaseDirectory);
            CompositionContainer container = new CompositionContainer(catalog);

            this.Act_JSDG_ATS = container.GetExportedValueOrDefault<Action<String, String[], Action<Int32, String>>>("JSDG.ATS");

            this.Act_JSDG_ES = container.GetExportedValueOrDefault<Action<String, String, String, String[] ,Action<Int32, String>>>("JSDG.ES");

            this.Act_JSDG_CBI_A = container.GetExportedValueOrDefault<Action<String, String[], Action<Int32, String>>>("JSDG.CBI.A");

            this.Act_JSDG_CBI_B = container.GetExportedValueOrDefault<Action<String, String[], Action<Int32, String>>>("JSDG.CBI.B");

            this.Act_JSDG_OBCU_A = container.GetExportedValueOrDefault<Action<String, String[], Action<Int32, String>>>("JSDG.OBCU.A");

            this.Act_JSDG_OBCU_B = container.GetExportedValueOrDefault<Action<String, String[], Action<Int32, String>>>("JSDG.OBCU.B");

            this.Act_TCC_TCC_A = container.GetExportedValueOrDefault<Action<String, String[], Action<Int32, String>>>("TCC_TCC.A");

            this.Act_JSDG_ZC_B = container.GetExportedValueOrDefault<Action<String, String[], Action<Int32, String>>>("JSDG.ZC.B");

            this.Act_JSDG_POCT_B = container.GetExportedValueOrDefault<Action<String, String[], Action<Int32, String>, Dictionary<string, string>>>("JSDG.POCT_B");

            this.Act_JSDG_POCT_A = container.GetExportedValueOrDefault<Action<String, String[], Action<Int32, String>, Dictionary<string, string>>>("JSDG.POCT_A");

            this.Form_JSDG_MMS = container.GetExportedValueOrDefault<Object>("JSDG.MMS") as Form;

            this.Act_JSDG_Telegram = container.GetExportedValueOrDefault<Action<String, String, String, String, String, Action<Int32, String>>>("JSDG.Telegram");
        }
    }
}
