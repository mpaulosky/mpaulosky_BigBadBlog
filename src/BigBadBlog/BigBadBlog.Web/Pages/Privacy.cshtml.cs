// ============================================
// Copyright (c) 2024. All rights reserved.
// File Name :     Privacy.cshtml.cs
// Company :       mpaulosky
// Author :        teqsl
// Solution Name : BigBadBlog
// Project Name :  BigBadBlog.Web
// =============================================

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BigBadBlog.Web.Pages;

public class PrivacyModel : PageModel
{
	private readonly ILogger<PrivacyModel> _logger;

	public PrivacyModel(ILogger<PrivacyModel> logger)
	{
		_logger = logger;
	}

	public void OnGet()
	{
	}
}