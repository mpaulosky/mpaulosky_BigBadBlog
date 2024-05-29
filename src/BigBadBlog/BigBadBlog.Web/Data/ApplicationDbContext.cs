// ============================================
// Copyright (c) 2024. All rights reserved.
// File Name :     ApplicationDbContext.cs
// Company :       mpaulosky
// Author :        teqsl
// Solution Name : BigBadBlog
// Project Name :  BigBadBlog.Web
// =============================================

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BigBadBlog.Web.Data;

public class ApplicationDbContext : IdentityDbContext
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
		: base(options)
	{
	}
}