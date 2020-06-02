using System;
using Kendo.Mvc.UI.Fluent;
using Kendo.Mvc.UI;
using Txt = PushNews.WebApp.App_LocalResources;

namespace PushNews.WebApp.Models.UI
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Arguments",
        "JustCode_LiteralArgumentIsNotNamedDiagnostic:The used literal argument is not named",
        Justification = "Suficientemente autodocumentado, y añadir los nombres de parámetros empeora la legibilidad en este caso.")]
    public static class Grillas
    {
        
        public static GridBuilder<T> ConfigurarGrilla<T>(this GridBuilder<T> builder, string id,
            string controller, Action<DataSourceModelDescriptorFactory<T>> model,

            bool edicionInline = true, bool seleccionar = false, bool edicionMultiple = false) where T : class
        {
            return ConfigurarGrilla(builder, id, controller, model, new object(), edicionInline, 
                seleccionar: seleccionar, edicionMultiple:edicionMultiple);
        }

        public static GridBuilder<T> ConfigurarGrilla<T>(this GridBuilder<T> builder, string id,
            string controller, Action<DataSourceModelDescriptorFactory<T>> model,
            object readRouteValues, bool edicionInline = true, int tamPagina = 25,
            bool seleccionar = false, bool edicionMultiple = false) where T : class
        {
            Action<DataSourceBuilder<T>> dataSource = ds => ds
                    .Ajax()
                    .Batch(edicionMultiple)
                    .PageSize(tamPagina)
                    .Model(model)
                    .Create(create => create.Action("Nuevo", controller))
                    .Destroy("Eliminar", controller)
                    .Update("Modificar", controller)
                    .Read(r => r.Action("Leer", controller, readRouteValues));

            Action<GridExcelBuilder> excel = ex => ex
                    .AllPages(true)
                    .FileName(DateTime.Now.ToString("yyyyMMddhhmmss") + "_PushNews_" + controller + ".xlsx")
                    .Filterable(true)
                    .ProxyURL(controller + "/ExcelExportSave");

            return ConfigurarGrilla(builder, id, dataSource, excel, edicionInline, tamPagina, seleccionar, edicionMultiple);
        }

        public static GridBuilder<T> ConfigurarGrilla<T>(this GridBuilder<T> builder, string id,
            Action<DataSourceBuilder<T>> dataSource, Action<GridExcelBuilder> excel,
            bool edicionInline = true, int tamPagina = 25, bool seleccionar = false,
            bool edicionMultiple = false) where T : class
        {
            return builder
                .Name(id)
                .Sortable()
                .Filterable()
                .AutoBind(true)
                .Excel(excel)
                .Pageable(pageable => pageable
                    .Refresh(true)
                    .PageSizes(new[] { 10, 25, 50, 100 })
                    .ButtonCount(5)
                    .Messages(mm => mm
                        .ItemsPerPage(Txt.Kendo.ItemsPerPage)
                        .First(Txt.Kendo.First)
                        .Last(Txt.Kendo.Last)
                        .Previous(Txt.Kendo.Previous)
                        .Next(Txt.Kendo.Next)))
                        .Selectable(c => c
                            .Enabled(seleccionar)
                            .Mode(edicionMultiple ? GridSelectionMode.Multiple : GridSelectionMode.Single)
                        )
                .Editable(editable =>
                {
                    editable.DisplayDeleteConfirmation(false);
                    editable.Mode(GridEditMode.InLine);
                    editable.Enabled(edicionInline);
                })
                .DataSource(dataSource);
        }
   }
}