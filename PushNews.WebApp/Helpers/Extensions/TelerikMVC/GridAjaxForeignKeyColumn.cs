﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Web.Mvc;
//using Kendo.Mvc.UI;
//using Kendo.Mvc.UI.Html;
//using Kendo.Mvc.Extensions;

//namespace PushNews.WebApp.Helpers.Extensions.TelerikMVC
//{
//    public class GridAjaxForeignKeyColumn<TModel, TValue> : GridBoundColumn<TModel, TValue>, IGridForeignKeyColumn where TModel : class
//    {
//        public GridAjaxForeignKeyColumn(Grid<TModel> grid, Expression<Func<TModel, TValue>> expression, ListViewAjaxDataSourceBuilder dataSource)
//            : base(grid, expression)
//        {
//        }

//        public SelectList Data
//        {
//            get
//            {
//                throw new NotImplementedException();
//            }

//            set
//            {
//                throw new NotImplementedException();
//            }
//        }

//        public Action<IDictionary<string, object>, object> SerializeSelectList
//        {
//            get
//            {
//                throw new NotImplementedException();
//            }
//        }
//    }

//// (c) Copyright 2002-2010 Telerik 
//// This source is subject to the GNU General Public License, version 2
//// See http://www.gnu.org/licenses/gpl-2.0.html. 
//// All other rights reserved.

//namespace Telerik.Web.Mvc.UI
//{

//    public class GridForeignKeyColumn<TModel, TValue> : GridBoundColumn<TModel, TValue> where TModel : class
//    {
//        public GridForeignKeyColumn(Grid<TModel> grid, Expression<Func<TModel, TValue>> expression, SelectList data)
//            : base(grid, expression)
//        {
//#if MVC2 || MVC3
//            EditorTemplateName = "GridForeignKey";
//#endif
//            Data = data;
//        }

//        public SelectList Data
//        {
//            get;
//            set;
//        }

//        public override IGridColumnSerializer CreateSerializer()
//        {
//            return new GridForeignKeyColumnSerializer(this);
//        }

//        protected override IGridDataCellBuilder CreateEditBuilderCore(IGridHtmlHelper htmlHelper)
//        {
//#if MVC2 || MVC3
//            if (!ReadOnly)
//            {
//                var builder = new GridForeignKeyEditorForCellBuilder<TModel, TValue>()
//                {
//                    Expression = Expression,
//                    AdditionalViewData = AdditionalViewData,
//                    ViewContext = Grid.ViewContext,
//                    TemplateName = EditorTemplateName,
//                    Member = Member,
//                    AppendViewData = SerializeSelectList
//                };

//                builder.HtmlAttributes.Merge(HtmlAttributes);

//                return builder;
//            }
//#endif
//            return CreateDisplayBuilder(htmlHelper);
//        }

//        protected override IGridDataCellBuilder CreateDisplayBuilderCore(Html.IGridHtmlHelper htmlHelper)
//        {
//            if (Template != null || InlineTemplate != null)
//            {
//                return base.CreateDisplayBuilderCore(htmlHelper);
//            }

//            IGridDataCellBuilder builder;

//            builder = new GridForeignKeyDataCellBuilder<TModel, TValue>
//            {
//                Encoded = Encoded,
//                Format = Format,
//                Value = Value,
//                Data = Data
//            };

//            builder.HtmlAttributes.Merge(HtmlAttributes);

//            return builder;
//        }

//        protected void AppendSelectList(IDictionary<string, object> viewData, object dataItem)
//        {
//            object selectedValue;
//            if (!Data.Any(i => i.Selected))
//            {
//                selectedValue = Expression.Compile().Invoke((TModel)dataItem);
//            }
//            else
//            {
//                selectedValue = Data.SelectedValue;
//            }

//            viewData.Add(Member + "_Data", new SelectList(Data.Items, Data.DataValueField, Data.DataTextField, selectedValue));
//        }

//        public Action<IDictionary<string, object>, object> SerializeSelectList
//        {
//            get { return AppendSelectList; }
//        }
//    }
//}