using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using MostaStoreWebApp.Models;

namespace MostaStoreWebApp.Models
{
    public class PagingModel
    {
        
        public int CurrentPage { get; set; } = 1;
        public int Count { get; set; }
        public int PageSize { get; set; } = 2;

        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));
    }
    
}