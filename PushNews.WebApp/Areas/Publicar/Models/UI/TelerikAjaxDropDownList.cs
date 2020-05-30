using System.Collections.Generic;

namespace PushNews.WebApp.Models.UI
{
    public class TelerikAjaxDropDownList
    {
        public TelerikAjaxDropDownList(string name, string action, string controller, string dataValueField, string dataTextField, IDictionary<string, object> htmlAttributes, bool valuePrimitive = true, string placeholder = "")
        {
            Name = name;
            ReadAction = action;
            ReadController = controller;
            DataTextField = dataTextField;
            DataValueField = dataValueField;
            ValuePrimitive = valuePrimitive;
            Placeholder = placeholder;
            HtmlAttributes = htmlAttributes;
        }
        public string Name { get; set; }
        public string DataTextField { get; set; }
        public string DataValueField { get; set; }
        public bool ValuePrimitive { get; set; }
        public string Placeholder { get; set; }
        public IDictionary<string, object> HtmlAttributes { get; set; }

        public string ReadAction { get; set; }
        public string ReadController { get; set; }
    }
}