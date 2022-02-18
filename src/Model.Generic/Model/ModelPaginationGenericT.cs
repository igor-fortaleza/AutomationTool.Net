using System;

namespace Model.Generic.Model
{
    public class ModelPaginationGeneric<T> : ModelPagination
    {
        public T ModelFilter { get; set; } = (T)Activator.CreateInstance(typeof(T));

        public string OrderBy { get; set; } = string.Empty;

        public bool OrderDesc { get; set; }
    }
}
