using System.Collections.Generic;

namespace PushNews.WebApp.Models.UI
{
    public class TelerikAjaxComboBox: TelerikAjaxDropDownList
    {
        public TelerikAjaxComboBox(string name, string action, string controller, string dataValueField, string dataTextField, IDictionary<string, object> htmlAttributes, bool valuePrimitive = true, string placeholder = "", bool serverFiltering = false)
            :base(name, action, controller, dataTextField, dataValueField, htmlAttributes, valuePrimitive: valuePrimitive, placeholder: placeholder)
        {
            ServerFiltering = serverFiltering;
        }

        public bool ServerFiltering { get; set; }
    }
}