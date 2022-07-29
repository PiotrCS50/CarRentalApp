using CarRental.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CarRental.Domain.Extentions
{
    public static class HtmlHelperExtentions
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html, Paging paging, Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();
            TagBuilder firstTag = new TagBuilder("a");
            firstTag.MergeAttribute("href", pageUrl(1));
            TagBuilder firstIcon = new TagBuilder("i");
            firstTag.AddCssClass("bi bi-chevron-double-left");
            firstTag.InnerHtml = firstIcon.ToString();
            firstTag.AddCssClass("btn btn-default");
            result.Append(firstTag.ToString());

            for (int i = 1; i <= paging.TotalPages; i++)
            {
                TagBuilder tag = new TagBuilder("a");
                tag.MergeAttribute("href", pageUrl(i));
                tag.InnerHtml = i.ToString();
                if(i == paging.CurrentPage)
                {
                    tag.AddCssClass("selected");
                    tag.AddCssClass("btn-primary");
                }
                tag.AddCssClass("btn btn-default");
                result.Append(tag.ToString());
            }

            TagBuilder lastTag = new TagBuilder("a");
            lastTag.MergeAttribute("href", pageUrl(paging.TotalPages));
            TagBuilder lastIcon = new TagBuilder("i");
            lastIcon.AddCssClass("bi bi-chevron-double-right");
            lastTag.InnerHtml = lastIcon.ToString();
            lastTag.AddCssClass("btn btn-default");
            result.Append(lastTag.ToString());

            return MvcHtmlString.Create(result.ToString());
        }
    }
}
