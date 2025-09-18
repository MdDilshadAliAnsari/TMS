namespace TMS.Authentication.Menu
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlRoot("Menus")]
    public class Menus
    {
        [XmlElement("Menu")]
        public List<Menu> MenuList { get; set; }
    }

    public class Menu
    {
        [XmlAttribute("Url")]
        public string Url { get; set; }

        [XmlAttribute("Text")]
        public string Text { get; set; }

        [XmlAttribute("Value")]
        public string Value { get; set; }

        [XmlElement("SubMenu")]
        public List<SubMenu> SubMenus { get; set; }
    }

    public class SubMenu
    {
        [XmlAttribute("Url")]
        public string Url { get; set; }

        [XmlAttribute("Text")]
        public string Text { get; set; }

        [XmlAttribute("Value")]
        public string Value { get; set; }
    }

}
