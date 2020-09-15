
using Xenial.Doughnut.Model;

namespace Xenial.Doughnut.Frontend
{
    public class Selectable<T>
        where T : DoughnutBaseObject
    {
        public int Id => Item.Oid;
        public bool Selected { get; set; }
        public T Item { get; }
        public Selectable(T item)
            => Item = item;
    }
}