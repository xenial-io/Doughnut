
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevExpress.Xpo;
using Microsoft.AspNetCore.Components;
using Skclusive.Core.Component;

namespace Xenial.Doughnut.Frontend.Shared
{
    public partial class ObjectDetailDialog<TItem> : ComponentBase
        where TItem : Doughnut.Model.DoughnutBaseObject
    {
        protected RenderFragment<TItem> childContent;
        [Parameter]
        public RenderFragment<TItem> ChildContent { get => childContent; set => childContent = value; }
    }
}