using Radzen.Blazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ALISS.Data
{
    public class RadzenGridCustom<T>
    {
        public RadzenGrid<T> radzenGrid { get; set; } = new RadzenGrid<T>();
        public List<int> PageSizeOption { get { return new List<int> { 4, 10, 25, 50 }; } }
        public int PageSize { get; set; }
        public string FooterLabelString
        {
            get
            {
                var footerString = "0 record";
                var pageFirstRecord = (radzenGrid.CurrentPage * radzenGrid.PageSize) + 1;
                var pageLastRecord = (radzenGrid.CurrentPage * radzenGrid.PageSize) + radzenGrid.PageSize;
                if (pageLastRecord > radzenGrid.View.Count()) pageLastRecord = radzenGrid.View.Count();
                if (radzenGrid.View.Count() > 0)
                {
                    footerString = $"record ( {pageFirstRecord} to {pageLastRecord} ) of {radzenGrid.View.Count()}";
                }
                return footerString;
            }
        }

        public RadzenGridCustom()
        {
            PageSize = 4;
        }
    }
}
