// ============================================
// Copyright (c) 2024. All rights reserved.
// File Name :     ProgramExtensions.cs
// Company :       mpaulosky
// Author :        teqsl
// Solution Name : BigBadBlog
// Project Name :  BigBadBlog.Data.Postgres
// =============================================

using BigBadBlog.Common;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BigBadBlog.Data.Postgres;

public static class ProgramExtensions
{
	public static IHostApplicationBuilder? AddPostgresDatabaseServices(this IHostApplicationBuilder? host)
	{
		host.AddNpgsqlDbContext<ApplicationDbContext>(ServiceNames.DatabasePosts.Name);

		host.Services.AddScoped<IPostRepository, PgRepository>();

		return host;
	}

	public static IHostApplicationBuilder? AddIdentityServices(this IHostApplicationBuilder? builder)
	{
		builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
			.AddEntityFrameworkStores<ApplicationDbContext>();

		return builder;
	}
}