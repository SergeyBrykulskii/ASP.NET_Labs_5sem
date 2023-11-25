using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Web_153501_Brykulskii.TagHelpers;

[HtmlTargetElement("pager")]
public class PagerTagHelper : TagHelper
{
	private readonly LinkGenerator _linkGenerator;
	private readonly HttpContext _httpContext;

	public PagerTagHelper(LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor)
	{
		_linkGenerator = linkGenerator;
		_httpContext = httpContextAccessor.HttpContext!;
	}

	[HtmlAttributeName("current-page")]
	public int CurrentPage { get; set; }

	[HtmlAttributeName("total-pages")]
	public int TotalPages { get; set; }

	[HtmlAttributeName("genre")]
	public string? Genre { get; set; }

	[HtmlAttributeName("admin")]
	public bool Admin { get; set; }

	private RouteValueDictionary GetRouteValues(int pageNumber)
	{
		if (Admin)
		{
			return new RouteValueDictionary(new
			{
				pageNo = pageNumber
			});
		}
		else
		{
			return new RouteValueDictionary(new
			{
				pageNo = pageNumber,
				genre = Genre
			});
		}
	}

	private string? GetUrl(int pageNo)
	{
		return _linkGenerator.GetPathByPage(_httpContext, values: GetRouteValues(pageNo));
	}
	public override void Process(TagHelperContext context, TagHelperOutput output)
	{
		output.TagName = "nav";
		output.Attributes.SetAttribute("class", "col-sm-4 offset-2");

		var ulTag = new TagBuilder("ul");
		ulTag.AddCssClass("pagination");

		var previousAvailable = CurrentPage > 1;
		var nextAvailable = CurrentPage < TotalPages;

		var previousLiTag = new TagBuilder("li");
		previousLiTag.AddCssClass(previousAvailable ? "page-item" : "page-item disabled");

		var previousLink = new TagBuilder("a");
		previousLink.AddCssClass("page-link");
		previousLink.Attributes["aria-label"] = "Previous";

		if (previousAvailable)
		{
			var previousUrl = GetUrl(CurrentPage - 1);
			previousLink.Attributes["href"] = previousUrl;
		}

		var previousSpan = new TagBuilder("span");
		previousSpan.InnerHtml.Append("\u00AB");

		previousLink.InnerHtml.AppendHtml(previousSpan);
		previousLiTag.InnerHtml.AppendHtml(previousLink);
		ulTag.InnerHtml.AppendHtml(previousLiTag);

		for (int i = 1; i <= TotalPages; i++)
		{
			var liTag = new TagBuilder("li");
			liTag.AddCssClass("page-item");
			if (CurrentPage == i)
			{
				liTag.AddCssClass("active");
			}

			var link = new TagBuilder("a");
			link.AddCssClass("page-link");

			var url = GetUrl(i);
			link.Attributes["href"] = url;
			link.InnerHtml.Append(i.ToString());

			liTag.InnerHtml.AppendHtml(link);
			ulTag.InnerHtml.AppendHtml(liTag);
		}

		var nextLiTag = new TagBuilder("li");
		nextLiTag.AddCssClass(nextAvailable ? "page-item" : "page-item disabled");

		var nextLink = new TagBuilder("a");
		nextLink.AddCssClass("page-link");
		nextLink.Attributes["aria-label"] = "Next";

		if (nextAvailable)
		{
			var nextUrl = GetUrl(CurrentPage + 1);
			nextLink.Attributes["href"] = nextUrl;
		}

		var nextSpan = new TagBuilder("span");
		nextSpan.InnerHtml.Append("\u00BB");

		nextLink.InnerHtml.AppendHtml(nextSpan);
		nextLiTag.InnerHtml.AppendHtml(nextLink);
		ulTag.InnerHtml.AppendHtml(nextLiTag);

		output.Content.AppendHtml(ulTag);
	}
}
