
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevExpress.Xpo;
using Microsoft.AspNetCore.Components;
using Skclusive.Core.Component;

namespace Xenial.Doughnut.Frontend.Shared
{
    public partial class ListView<TItem> : ComponentBase
        where TItem : Doughnut.Model.DoughnutBaseObject
    {
        [Inject]
        private UnitOfWork uow { get; set; }

        public bool ShowNew { get; set; }
        public bool ShowEdit { get; set; }
        public bool IsLoading { get; set; } = true;
        public int? EditId { get; set; }

        [Parameter]
        public string OrderBy { set; get; }

        [Parameter]
        public Sort Direction { set; get; } = Sort.Ascending;

        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public RenderFragment Icon { get; set; }

        [Parameter]
        public RenderFragment<ListView<TItem>> TableHeaderCells { get; set; }

        [Parameter]
        public RenderFragment<(TItem, ListView<TItem>)> TableRows { get; set; }

        [Parameter]
        public RenderFragment<ListView<TItem>> AddDialog { get; set; }

        [Parameter]
        public RenderFragment<ListView<TItem>> EditDialog { get; set; }

        private EventCallback<EventArgs> CreateOnSortClick(string name) => EventCallback.Factory.Create<System.EventArgs>(this, (_) =>
        {
            HandleSortClick(name);
        });

        private void HandleSortClick(string name)
        {
            if (OrderBy == name)
            {
                Direction = Direction == Sort.Ascending ? Sort.Descending : Sort.Ascending;
            }
            else
            {
                Direction = Sort.Ascending;
            }

            OrderBy = name;

            StateHasChanged();
        }

        private bool Dense { set; get; } = false;

        private Size Size => Dense ? Size.Small : Size.Medium;

        private bool AnySelected => HasSelection && SelectedCount < Rows.Count;

        private bool AllSelected => Rows.All(row => row.Selected);

        private int SelectedCount => Rows.Count(row => row.Selected);

        private bool HasSelection => SelectedCount > 0;

        private int RowsPerPage => 6;

        private int EmptyRows => RowsPerPage - Rows.Count;

        private int EmptyHeight => (Dense ? 33 : 53) * EmptyRows;

        private void OnDenseChanged(EventArgs args)
        {
            Dense = !Dense;

            StateHasChanged();
        }

        private void OnSelectAllClick(EventArgs args)
        {
            var allSelected = AllSelected;

            foreach (var row in Rows)
            {
                row.Selected = !allSelected;
            }

            StateHasChanged();
        }

        private IList<Selectable<TItem>> Rows = new List<Selectable<TItem>>();

        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;
            try
            {
                await uow.UpdateSchemaAsync();
                await Refresh();
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task Refresh()
        {
            ShowNew = false;
            ShowEdit = false;
            EditId = null;
            IsLoading = true;
            try
            {
                uow.DropIdentityMap();
                var items = await uow.Query<TItem>().ToListAsync();
                Rows = items.Select(i => new Selectable<TItem>(i)).ToList();
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task Delete()
        {
            try
            {
                var items = Rows.Where(row => row.Selected).Select(row => row.Item);

                foreach (var item in items)
                {
                    await uow.DeleteAsync(item);
                }

                await uow.CommitChangesAsync();
                await Refresh();
            }
            finally
            {
                IsLoading = false;
            }
        }

        public void New()
        {
            ShowNew = true;
        }

        public void EditRow(TItem item)
        {
            EditId = item.Oid;
            ShowEdit = true;
        }
    }
}