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

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add Aspire service defaults
builder.AddServiceDefaults();

// Add OutputCache service	
builder.AddRedisOutputCache("outputcache");

builder.Services.AddOutputCache(options =>
{
	options.AddBasePolicy(policy => policy.Tag("ALL").Expire(TimeSpan.FromMinutes(5)));
	options.AddPolicy("Home", policy => policy.Tag("Home").Expire(TimeSpan.FromSeconds(30)));
	options.AddPolicy("Post", policy => policy.Tag("Post").SetVaryByRouteValue("slug").Expire(TimeSpan.FromSeconds(30)));
});

// Add my repository for posts
builder.Services.AddTransient<IPostRepository, MarkdownPostRepository>();

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
											 throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlite(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
	.AddEntityFrameworkStores<ApplicationDbContext>();

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

app.Run();