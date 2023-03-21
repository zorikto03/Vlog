using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace NetCore3MVC.Services
{
    public class PageLinkTagHelper : TagHelper
    {
        private IUrlHelperFactory UrlHelperFactory;
        public PageLinkTagHelper(IUrlHelperFactory helperFactory)
        {
            UrlHelperFactory = helperFactory;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }
        public PageViewModel PageModel { get; set; }
        public string PageAction { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            IUrlHelper urlHelper = UrlHelperFactory.GetUrlHelper(ViewContext);
            output.TagName = "div";

            TagBuilder tag = new TagBuilder("ul");
            tag.AddCssClass("pagination");

            //TagBuilder currentItem = CreateTag(PageModel.CurrentPage, PageModel.PageSize, urlHelper);

            for(int i = 1; i <= PageModel.TotalPages; i++)
            {
                TagBuilder prevItem = CreateTag(i, urlHelper);
                tag.InnerHtml.AppendHtml(prevItem);
            }

            //tag.InnerHtml.AppendHtml(currentItem);

            //for(int i = 1; i <= PageModel.CountNextPages; i++)
            //{
            //    TagBuilder prevItem = CreateTag(PageModel.CurrentPage + i, PageModel.PageSize, urlHelper);
            //    tag.InnerHtml.AppendHtml(prevItem);
            //}

            output.Content.AppendHtml(tag);
        }

        TagBuilder CreateTag(int currentPage, IUrlHelper urlHelper)
        {
            TagBuilder item = new TagBuilder("li");
            TagBuilder link = new TagBuilder("a");
            if (currentPage == this.PageModel.CurrentPage)
            {
                item.AddCssClass("active");
            }
            else
            {
                link.Attributes["href"] = urlHelper.Action(PageAction, new { page = currentPage, pageSize = PageModel.PageSize });
            }
            item.AddCssClass("page-item");
            link.AddCssClass("page-link");
            link.InnerHtml.Append(currentPage.ToString());
            item.InnerHtml.AppendHtml(link);
            return item;
        }
    }
}
