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
            sb.Append("<div class=\"sidenav\">");
            foreach (var category in categories.Where(c => c.ParentId == null))
            {
                AppendCategory(category, sb);
            }
            sb.Append("</div>");
            return new HtmlString(sb.ToString());
        }
        static void AppendCategory(Category category, StringBuilder sb)
        {
            if (category.Children == null)
            {
                return;
            }
            bool hasChild = category.Children.Any();
            if (hasChild)
            {
                sb.Append("<button class=\"dropdown-btn\">" +
                    $"<a style=\"display:contents;\" href=\"/shop/category?categoryId={category.Id}\">{category.Name}</a>" +
                    "<i class=\"fas fa-caret-down\"></i>" +
                    "</button>");
                sb.Append("<div class=\"dropdown-container\">");
                foreach (var item in category.Children)
                {
                    AppendCategory(item, sb);
                }
                sb.Append("</div>");
            }
            else
            {
                sb.Append($"<a href=\"/shop/category?categoryId={category.Id}\">{category.Name}</a>");
            }
            
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
