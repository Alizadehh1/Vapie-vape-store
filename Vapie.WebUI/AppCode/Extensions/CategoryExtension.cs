using Microsoft.AspNetCore.Html;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vapie.WebUI.Models.Entities;

namespace Vapie.WebUI.AppCode.Extensions
{
    public static partial class Extension
    {
        public static HtmlString GetCategoriesRaw(this List<Category> categories)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<ul class=\"widget-body filter-items search-ul\">");
            foreach (var category in categories.Where(c=>c.DeletedById==null))
            {
                if (categories.Any(c=>c.Id == category.ParentId))
                {
                    continue;
                }
                bool hasChild = categories.Any(c => c.ParentId == category.Id);
                AppendCategory(category, sb, hasChild);
            }
            sb.Append("</ul>");
            return new HtmlString(sb.ToString());
        }
        static void AppendCategory(Category category, StringBuilder sb,bool hasChild)
        {
            //bool hasChild = category.Children.Any();
            sb.Append($"<li {(hasChild ? "class=with-ul" : "")}>" +
                $"<a href=\"#\">{category.Name}");
            if (hasChild)
                sb.Append("<i class=\"fas fa-chevron-down\"></i>");
            sb.Append("</a>");
            if (hasChild)
            {
                sb.Append("<ul style=\"display: none\">");
                foreach (var item in category.Children.Where(c => c.DeletedById == null))
                {
                    bool hasChild1 = category.Children.Any(c => c.ParentId == item.Id);
                    AppendCategory(item, sb, hasChild1);
                }
                sb.Append("</ul>");
            }
            sb.Append("</li>");
        }
        static public IEnumerable<Category> GetAllChildren(this Category category)
        {
            if (category.ParentId != null)
                yield return category;

            if (category.Children != null)
            {
                foreach (var item in category.Children.SelectMany(c => c.GetAllChildren()))
                {
                    yield return item;
                }
            }
        }
    }
}
