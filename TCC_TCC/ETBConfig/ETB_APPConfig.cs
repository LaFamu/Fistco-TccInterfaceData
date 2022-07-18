using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCC_TCC.A.ETBConfig
{
    public class ETB_APPConfig
    {
        /// <summary>
        /// etb.config中的配置
        /// </summary>
        public static Configuration AppConfig { get; set; }

        /// <summary>
        /// 获取配置 只运行一次
        /// </summary>
        static ETB_APPConfig()
        {
            AppConfig = ConfigFileHelper.InitConfigurationManager(Path.GetFullPath(@"ETBConfig\ConfigFile\TCC_TCC_A.config"));
        }

        /// <summary>
        /// 根据节点名称获取字典
        /// </summary>
        /// <param name="SectionGroupAndSectionName">节点名称</param>
        /// <returns></returns>
        public static Dictionary<String, UInt32> GetConfigDic(string SectionGroupAndSectionName)
        {
            if (string.IsNullOrEmpty(SectionGroupAndSectionName))
            {
                return new Dictionary<String, UInt32>();
            }
            var config = AppConfig.GetSection(String.Format("{0}", SectionGroupAndSectionName)) as ConfigSection;
            if (config != null)
            {
                return config.KeyValues.Cast<TheKeyValue>().ToDictionary(x => 
                    x.Key, y => UInt32.Parse(y.Value, NumberStyles.HexNumber));
            }
            return new Dictionary<String, UInt32>();
        }
        /// <summary>
        /// 根据节点名称获取字典
        /// </summary>
        /// <param name="key">节点名称</param>
        /// <returns></returns>
        public static ICollection<String> GetEnums(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return new List<string>();
            }
            var config = ETB_APPConfig.AppConfig.AppSettings.Settings[key].Value;
            if (config != null)
            {
                return config.Split(',');
            }
            return new List<string>();
        }
    }

    public sealed class ConfigFileHelper
    {
        public static Configuration InitConfigurationManager(String fileName)
        {
            var map = new ExeConfigurationFileMap() { ExeConfigFilename = fileName };
            var config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
            if (config == null)
            {
                throw new Exception(String.Format("未能找到 '{0}' 配置文件", fileName));
            }
            return config;
        }
    }
    public class ConfigSection : ConfigurationSection
    {
        private static readonly ConfigurationProperty Property
            = new ConfigurationProperty("", typeof(TheKeyValueCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);

        [ConfigurationProperty("", Options = ConfigurationPropertyOptions.IsDefaultCollection)]
        public TheKeyValueCollection KeyValues
        {
            get { return (TheKeyValueCollection)base[Property]; }
        }
    }

    [ConfigurationCollection(typeof(TheKeyValue))]
    public class TheKeyValueCollection : ConfigurationElementCollection
    {
        public TheKeyValue this[String name]
        {
            get { return (TheKeyValue)base.BaseGet(name); }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new TheKeyValue();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TheKeyValue)element).Key;
        }
    }

    public class TheKeyValue : ConfigurationElement
    {
        [ConfigurationProperty("key", IsRequired = true)]
        public String Key
        {
            get { return base["key"].ToString(); }
            set { base["key"] = value; }
        }

        [ConfigurationProperty("value", IsRequired = true)]
        public String Value
        {
            get { return base["value"].ToString(); }
            set { base["value"] = value; }
        }
    }
    public class MSIOModelSection : ConfigurationSection
    {
        [ConfigurationProperty("ID", IsRequired = true)]
        public String ID
        {
            get { return this["ID"].ToString(); }
            set { this["ID"] = value; }
        }

        [ConfigurationProperty("Index", IsRequired = true)]
        public String Index
        {
            get { return this["Index"].ToString(); }
            set { this["Index"] = value; }
        }

        [ConfigurationProperty("Name", IsRequired = true)]
        public String Name
        {
            get { return this["Name"].ToString(); }
            set { this["Name"] = value; }
        }

        [ConfigurationProperty("Byte_F", IsRequired = true)]
        public String Byte_F
        {
            get { return this["Byte_F"].ToString(); }
            set { this["Byte_F"] = value; }
        }

        [ConfigurationProperty("Bit_F", IsRequired = true)]
        public String Bit_F
        {
            get { return this["Bit_F"].ToString(); }
            set { this["Bit_F"] = value; }
        }

        [ConfigurationProperty("TypeInOrOut", IsRequired = true)]
        public String TypeInOrOut
        {
            get { return this["TypeInOrOut"].ToString(); }
            set { this["TypeInOrOut"] = value; }
        }
    }
}
