using Dnet.QdrantAdmin.Application.Shared.Dtos;
using Dnet.Blazor.Components.Grid.Infrastructure.Entities;
using Dnet.Blazor.Components.Grid.Infrastructure.Enums;
using Dnet.Blazor.Infrastructure.Models.SearchModels;

namespace Dnet.QdrantAdmin.Client.Pages.Admins;

public class QpointGridConfiguration
{
    public static GridOptions<QpointDto> GetQpointGridOptions()
    {
        return new GridOptions<QpointDto>()
        {
            HeaderHeight = 40,
            RowHeight = 40,
            PaginationPageSize = 100,
            SuppressRowClickSelection = true,
            RowMultiSelectWithClick = false,
            RowSelectionType = RowSelectionType.Single,
            GridClass = "cvs-bl-grid",
            GroupDefaultExpanded = false,
            SuppressFilterRow = true,
            SuppressPaginationPanel = true,
            EnableGrouping = true,
            CheckboxSelectionColumn = true,
            NullValueSortedToEnd = true,
            UseVirtualization = true
        };
    }

    public static List<GridColumn<QpointDto>> GetQpointGridColumns()
    {
        var height = 40;
        var width = 100;
        var canGrow = 1;

        return new List<GridColumn<QpointDto>> {
                        new()
                        {
                            ColumnId = 0,
                            ColumnOrder = 0,
                            HeaderName = "QpointId",
                            DataField = "QpointId",
                            Width= 100,
                            Height= height,
                            CanGrow = canGrow,
                            Sortable = true,
                            CellDataFn = cellParam => cellParam.RowData.QpointId,
                            CellDataType = CellDataType.Text,
                            Hide = false
                        },
                        new()
                        {
                            ColumnId = 1,
                            ColumnOrder = 1,
                            HeaderName = "CollectionName",
                            DataField = "CollectionName",
                            Width= width,
                            Height= height,
                            CanGrow = canGrow,
                            CellDataFn = cellParam => cellParam.RowData.CollectionName,
                            CellDataType = CellDataType.Text,
                        },
                        new()
                        {
                            ColumnId = 1,
                            ColumnOrder = 1,
                            HeaderName = "PayloadString",
                            DataField = "PayloadString",
                            Width= width,
                            Height= height,
                            CanGrow = canGrow,
                            CellDataFn = cellParam => cellParam.RowData.PayloadString,
                            CellDataType = CellDataType.Text,
                        }

        };
    }

    public static List<GridColumn<QpointDto>> GetImportQpointGridColumns()
    {
        var height = 40;
        var width = 100;
        var canGrow = 1;

        return new List<GridColumn<QpointDto>> {
                        new()
                        {
                            ColumnId = 1,
                            ColumnOrder = 1,
                            HeaderName = "Text",
                            DataField = "Text",
                            Width= width,
                            Height= height,
                            CanGrow = canGrow,
                            CellDataFn = cellParam => cellParam.RowData.Text,
                            CellDataType = CellDataType.Text,
                        },
                        new()
                        {
                            ColumnId = 1,
                            ColumnOrder = 1,
                            HeaderName = "Payload",
                            DataField = "PayloadString",
                            Width= width,
                            Height= height,
                            CanGrow = canGrow,
                            CellDataFn = cellParam => cellParam.RowData.PayloadString,
                            CellDataType = CellDataType.Text,
                        }

        };
    }
}
