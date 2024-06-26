// ============================================
// Copyright (c) 2024. All rights reserved.
// File Name :     Program.cs
// Company :       mpaulosky
// Author :        teqsl
// Solution Name : BigBadBlog
// Project Name :  BigBadBlog.Web
// =============================================

global using BigBadBlog.Web.Data;

using System.Net;

using BigBadBlog.Common;
using BigBadBlog.Data.Postgres;

var builder = WebApplication.CreateBuilder(args);

// Add Aspire service defaults
builder.AddServiceDefaults();

builder.AddPostgresDatabaseServices();

// Add OutputCache service	
builder.AddRedisOutputCache(ServiceNames.Outputcache);
builder.Services.AddOutputCache(options =>
{
	options.AddBasePolicy(policy => policy.Tag("ALL").Expire(TimeSpan.FromMinutes(5)));
	options.AddPolicy("Home", policy => policy.Tag("Home").Expire(TimeSpan.FromSeconds(30)));
	options.AddPolicy("Post", policy => policy.Tag("Post").SetVaryByRouteValue("slug").Expire(TimeSpan.FromSeconds(30)));
});

// Add services to the container.
builder.AddIdentityServices();

builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseMigrationsEndPoint();
}
else
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.Map("/Posts", (HttpContext ctx) =>
{
	return HttpStatusCode.NotFound;
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseOutputCache();

app.MapRazorPages();

var mdRepo = new MarkdownPostRepository();
var pgRepo = app.Services.CreateScope().ServiceProvider.GetRequiredService<IPostRepository>();

var pgPosts = await pgRepo.GetPostsAsync(10, 1);

if (!pgPosts.Any())
{
	var existingPosts = await mdRepo.GetPostsAsync(10, 1);
	foreach (var post in existingPosts)
	{
		await pgRepo.AddPostAsync(post.Item1, post.Item2);
	}
}

app.Run();