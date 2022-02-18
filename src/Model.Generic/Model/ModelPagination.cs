using System;
using System.ComponentModel.DataAnnotations;

namespace Model.Generic.Model
{
    public class ModelPagination
    {
        [Display(Name = "Total de Registros")]
        public int TotalRecords { get; set; }

        [Display(Name = "Total de Páginas")]
        public int Pages { get; set; }

        [Display(Name = "Páginas Atual")]
        public int CurrentPage { get; set; }

        [Display(Name = "Quantidade por Página")]
        public int QuantityPerPage { get; set; }
    }
}
