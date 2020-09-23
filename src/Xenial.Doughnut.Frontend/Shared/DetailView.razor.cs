
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevExpress.Data.Filtering;
using DevExpress.Xpo;
using Microsoft.AspNetCore.Components;

namespace Xenial.Doughnut.Frontend.Shared
{
    public partial class DetailView<TItem> : ComponentBase
        where TItem : Doughnut.Model.DoughnutBaseObject
    {
        [Inject]
        private UnitOfWork uow { get; set; }

        [Parameter]
        public bool ShowNew { get; set; }

        [Parameter]
        public bool ShowDelete { get; set; }

        public bool IsLoading { get; set; } = true;

        [Parameter]
        public int? EditId { get; set; }

        public TItem CurrentObject { get; set; }

        [Parameter]
        public RenderFragment<TItem> ChildContent { set; get; }

        [Parameter]
        public string Title { get; set; }
        [Parameter]
        public string Name { get; set; }

        [Parameter]
        public RenderFragment Icon { get; set; }

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
            IsLoading = true;
            try
            {
                uow.DropIdentityMap();
                var info = uow.GetClassInfo<TItem>().FindAttributeInfo(typeof(Xenial.Doughnut.Model.SingletonAttribute));
                if(info != null)
                {
                    var item = await uow.FindObjectAsync<TItem>((CriteriaOperator)null);
                    CurrentObject = item;
                    if(CurrentObject == null)
                    {
                        CurrentObject = (TItem)uow.GetClassInfo<TItem>().CreateNewObject(uow);
                    }
                }
                else
                {
                    var item = await uow.GetObjectByKeyAsync<TItem>(EditId.HasValue ? EditId.Value : null);
                    CurrentObject = item;
                    if(CurrentObject == null)
                    {
                        CurrentObject = (TItem)uow.GetClassInfo<TItem>().CreateNewObject(uow);
                    }
                }
                
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task Save()
        {
            try
            {
                await uow.CommitChangesAsync();
                if(CurrentObject != null)
                {
                    EditId = (int?)uow.GetKeyValue(CurrentObject);
                }
                await Refresh();
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
                if(CurrentObject != null)
                {
                    await uow.DeleteAsync(CurrentObject);
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
            CurrentObject = (TItem)uow.GetClassInfo<TItem>().CreateNewObject(uow);
        }
    }
}
