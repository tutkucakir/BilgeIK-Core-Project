using PagedList.Core.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BilgeIK.WEBUI.Infrastructure
{
    public class SitePagedListRenderOptions
    {
        public static PagedListRenderOptions Boostrap4CustomizedText
        {
            get
            {
                var option = PagedListRenderOptions.Bootstrap4PageNumbersOnly;
                option.UlElementClasses = new[] { "pagination justify-content-center" }; 
                option.MaximumPageNumbersToDisplay = 5;
                option.LinkToPreviousPageFormat = "<";
                option.LinkToNextPageFormat = ">";
                option.LinkToFirstPageFormat = "<<";
                option.LinkToLastPageFormat = ">>";

                return option;
            }
        }
    }
}
