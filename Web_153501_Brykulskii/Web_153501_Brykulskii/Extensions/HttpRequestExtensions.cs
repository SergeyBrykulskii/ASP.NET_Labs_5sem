namespace Web_153501_Brykulskii.Extensions;

public static class HttpRequestExtensions
{
	public static bool isAjaxRequest(this HttpRequest request)
	{
		if (request == null)
		{
			throw new ArgumentNullException(nameof(request));
		}

		return request.Headers["X-Requested-With"] == "XMLHttpRequest";
	}
}
