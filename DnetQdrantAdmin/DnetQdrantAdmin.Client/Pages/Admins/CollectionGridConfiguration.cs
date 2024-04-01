using Dnet.QdrantAdmin.Application.Shared.Dtos;
using Dnet.Blazor.Components.Grid.Infrastructure.Entities;
using Dnet.Blazor.Components.Grid.Infrastructure.Enums;
using Dnet.Blazor.Infrastructure.Models.SearchModels;

namespace Dnet.QdrantAdmin.Client.Pages.Admins;

public static class CollectionGridConfiguration
{
    public static GridOptions<CollectionDto> GetCollectionGridOptions()
    {
        return new GridOptions<CollectionDto>()
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

    public static List<GridColumn<CollectionDto>> GetCollectionGridColumns()
    {
        var height = 40;
        var width = 100;
        var canGrow = 1;

        return new List<GridColumn<CollectionDto>> {
                        new()
                        {
                            ColumnId = 0,
                            ColumnOrder = 0,
                            HeaderName = "CollectionId",
                            DataField = "CollectionId",
                            Width= 100,
                            Height= height,
                            CanGrow = canGrow,
                            Sortable = true,
                            CellDataFn = cellParam => cellParam.RowData.CollectionId,
                            CellDataType = CellDataType.Number,
                            Hide = true
                        },
                        new()
                        {
                            ColumnId = 1,
                            ColumnOrder = 1,
                            HeaderName = "Name",
                            DataField = "Name",
                            Width= width,
                            Height= height,
                            CanGrow = canGrow,
                            CellDataFn = cellParam => cellParam.RowData.Name,
                            CellDataType = CellDataType.Text,
                        },

        };
    }
}