using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using NetCore3MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace NetCore3MVC.Services
{
    public static class HtmlHelpers
    {
        private static long subtraction(DateTime timestamp) => DateTime.Now.Ticks - timestamp.Ticks;
        delegate long SubtractionDelegate(DateTime timestamp);


        public static HtmlString ListArticles(this IHtmlHelper html, List<Article> source)
        {
            int lenght = 200;

            SubtractionDelegate c = subtraction;
            string result = "<div class='list-group'>";
            source.ForEach(x =>
            {
                result += $"<a href='/Title/Article/{x.Id}' class='list-group-item list-group-item-action active my-2'>" +
                    "<div class='d-flex w-100 justify-content-between'>" +
                        "<h5 class='mb-1'>" + x.Title + "</h5>" +
                        "<small>" + 
                            (x.CreateTimestamp.HasValue ? TimeRounding(new TimeSpan(c(x.CreateTimestamp.Value))) : "") +
                        "</small>" +
                    "</div>" +
                    "<p class='mb-1'>" + (x.Content.Length > lenght ? x.Content.Substring(0, lenght) + "..." : x.Content) + "</p>" +
                "</a>";
            });

            result += "</div>";
            return new HtmlString(result);
        }

        static string TimeRounding(TimeSpan time)
        {
            var monthsDays = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            if (time.TotalMinutes < 60)
            {
                return Math.Ceiling(time.TotalMinutes).ToString() + " minutes ago";
            }
            else if (time.TotalHours < 24)
            {
                var hour = Math.Ceiling(time.TotalHours);
                return hour.ToString() + (hour == 1 ? " hour" : " hours") + " ago";
            }
            else if (time.TotalDays < monthsDays)
            {
                var days = Math.Ceiling(time.TotalDays);
                return days.ToString() + (days == 1 ? " day" : " days") + " ago";
            }
            else
            {
                var months = Math.Ceiling(time.TotalDays / monthsDays);
                return months.ToString() + (months == 1? " month" : " months") + " ago";
            }
        }

        public static HtmlString AccountButtonToggle(this IHtmlHelper html, List<Claim> claims)
        {
            var login = claims.FirstOrDefault(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value;
            var firstName = claims.FirstOrDefault(x => x.Type == "FirstName")?.Value;
            var secondName = claims.FirstOrDefault(x => x.Type == "SecondName")?.Value;

            string result = "<div class='dropdown'>" +
                                    "<button class='btn btn-secondary dropdown-toggle' type='button' id='SignInOutBtn' data-toggle='dropdown' aria-expanded='false'>" +
                                        "Sign In/Out" +
                                    "</button>" +
                                    "<div class='dropdown-menu' aria-labelledby='SignInOutBtn'>" +
                                        "<div class='container dropdown-header bg-dark'> " +
                                            "<h5><span class='text-white'>" + $"{firstName} {secondName}" + "</span></h5>" +
                                            "<h5 class='middle'><span class='text-white'>" + $"{login}" + "</span></h5>" +
                                        "</div>" +
                                        "<a class='dropdown-item' href='/Account/Person'>LK</a>" +
                                        "<div class='dropdown-divider'></div>" +
                                        "<a class='dropdown-item' href='/Account/Logout'>Sign out</a>" +
                                    "</div>" +
                                    "</div>";
            return new HtmlString(result);   
        }

        public static HtmlString AccountButtonSignIn(this IHtmlHelper html)
        {
            string result = "<a class='nav-link text-dark' asp-area='' href='/Account/Login'>Sign In</a>";
            return new HtmlString(result);
        }

        /// <summary>
        /// this method creates news posts in horizontal scrollable panel.
        /// created by concatinating strings 
        /// </summary>
        /// <param name="html"></param>
        /// <param name="news"></param>
        /// <returns></returns>
        public static HtmlString NewsCreate(this IHtmlHelper html, List<New> news)
        {
            var result = string.Empty;
            foreach(var item in news)
            {
                result += "<div class='col-md-4 mr-4'>" +
                        "<div class='my-4'>" +
                            $"<h5>{item.Title}</h5>" +
                        "</div>" +
                        $"<p>{item.Content}</p>" +
                    "</div>";
            }

            return new HtmlString(result);
        }

        public static TagBuilder NewsCarouselWithHref(this IHtmlHelper html, List<New> news)
        {
            bool active = true;

            TagBuilder carousel = new TagBuilder("div");
            carousel.AddCssClass("carousel slide");
            carousel.MergeAttribute("data-ride", "carousel");
            carousel.MergeAttribute("id", "carouselCaptions");

            TagBuilder ol = new TagBuilder("ol");
            ol.AddCssClass("carousel-indicators");
            for(int i = 0; i < news.Count; i++)
            {
                TagBuilder li = new TagBuilder("li");
                li.MergeAttribute("data-target", "#carouselCaptions");
                li.MergeAttribute("data-slide-to", i.ToString());
                if (i == 0)
                {
                    li.AddCssClass("active");
                }
                ol.InnerHtml.AppendHtml(li);
            }
            carousel.InnerHtml.AppendHtml(ol);

            TagBuilder carouselInner = new TagBuilder("div");
            carouselInner.AddCssClass("carousel-inner");

            foreach(var newsItem in news)
            {
                TagBuilder carouselItem = new TagBuilder("div");
                carouselItem.AddCssClass("carousel-item");
                if (active)
                {
                    carouselItem.AddCssClass("active");
                }

                TagBuilder image = new TagBuilder("img");
                string src = newsItem.FilesPaths?.FirstOrDefault()?.Path != null ? newsItem.FilesPaths[0].Path : "/Resources/oneMore.png"; 
                image.MergeAttribute("src", src);
                image.AddCssClass("d-block w-100");

                TagBuilder h5 = new TagBuilder("h5");
                h5.InnerHtml.Append(newsItem.Title);

                TagBuilder p = new TagBuilder("p");
                p.InnerHtml.Append(newsItem.Content);

                TagBuilder caption = new TagBuilder("div");
                caption.AddCssClass("carousel-caption d-none d-md-block");
                caption.InnerHtml.AppendHtml(h5);
                caption.InnerHtml.AppendHtml(p);

                TagBuilder a = new TagBuilder("a");
                a.AddCssClass("text-dark");
                a.MergeAttribute("href", $"/News/NewsPage/{newsItem.Id}");
                a.InnerHtml.AppendHtml(image);
                a.InnerHtml.AppendHtml(caption);

                carouselItem.InnerHtml.AppendHtml(a);
                carouselInner.InnerHtml.AppendHtml(carouselItem);
                active = false;
            }
            carousel.InnerHtml.AppendHtml(carouselInner);

            return carousel;
        }


        public static TagBuilder NewsCarousel(this IHtmlHelper html, List<New> news)
        {
            bool active = true;
            TagBuilder tag = new TagBuilder("div");
            tag.GenerateId("carouselSlideOnly", "id");
            tag.AddCssClass("carousel slide");
            tag.MergeAttribute("data-ride", "carousel");

            TagBuilder carouselInner = new TagBuilder("div");
            carouselInner.AddCssClass("carousel-inner");

            foreach(var newItem in news)
            {
                TagBuilder carouselItem = new TagBuilder("div");
                carouselItem.AddCssClass("carousel-item");
                if (active)
                {
                    carouselItem.AddCssClass("active");
                }

                TagBuilder title = new TagBuilder("div");
                title.AddCssClass("my-4 d-flex flex-column h-100 d-inline-block");

                TagBuilder h5 = new TagBuilder("h5");
                h5.InnerHtml.Append(newItem.Title);

                TagBuilder p = new TagBuilder("p");
                p.InnerHtml.Append(newItem.Content);

                title.InnerHtml.AppendHtml(h5);
                title.InnerHtml.AppendHtml(p);

                carouselItem.InnerHtml.AppendHtml(title);

                carouselInner.InnerHtml.AppendHtml(carouselItem);
                active = false;
            }
            tag.InnerHtml.AppendHtml(carouselInner);
            return tag;
        }

        public static TagBuilder ArticleCategories(this IHtmlHelper html, List<ArticleСategory> categories)
        {
            TagBuilder tag = new TagBuilder("div");
            tag.AddCssClass("row");

            if (categories != null)
            {
                TagBuilder ul = new TagBuilder("ul");
                foreach(var category in categories)
                {
                    TagBuilder li = new TagBuilder("li");
                    li.InnerHtml.Append(category.Name);
                    ul.InnerHtml.AppendHtml(li);
                }
                tag.InnerHtml.AppendHtml(ul);

            }

            return tag;
        }

        public static HtmlString ListNews (this IHtmlHelper html, List<New> source)
        {
            int lenght = 200;

            SubtractionDelegate c = subtraction;
            string result = "<div class='list-group'>";
            source.ForEach(x =>
            {
                result += $"<a href='/News/NewsPage/{x.Id}' class='list-group-item list-group-item-action active my-2'>" +
                    "<div class='d-flex w-100 justify-content-between'>" +
                        "<h5 class='mb-1'>" + x.Title + "</h5>" +
                        "<small>" +
                            (x.CreateTimestamp.HasValue ? TimeRounding(new TimeSpan(c(x.CreateTimestamp.Value))) : "") +
                        "</small>" +
                    "</div>" +
                    "<p class='mb-1'>" + (x.Content.Length > lenght ? x.Content.Substring(0, lenght) + "..." : x.Content) + "</p>" +
                "</a>";
            });

            result += "</div>";
            return new HtmlString(result);
        }

        public static TagBuilder Breadcrumb(this IHtmlHelper html, string[] actions, string controller)
        {
            TagBuilder nav = new TagBuilder("nav");
            nav.MergeAttribute("aria-label", "breadcrumb");
            nav.AddCssClass("h-50");

            TagBuilder ol = new TagBuilder("ol");
            ol.AddCssClass("breadcrumb");

            for(int i =0; i < actions.Length; i++)
            {
                TagBuilder li = new TagBuilder("li");
                li.AddCssClass("breadcrumb-item");

                TagBuilder a = new TagBuilder("a");
                a.MergeAttribute("href", $"/{controller}/{actions[i]}");
                a.InnerHtml.Append(actions[i]);
                if (i == actions.Length - 1)
                    a.AddCssClass("active");

                li.InnerHtml.AppendHtml(a);
                ol.InnerHtml.AppendHtml(li);
            }
            nav.InnerHtml.AppendHtml(ol);
            return nav;
        }
    }
}
