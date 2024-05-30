// ============================================
// Copyright (c) 2024. All rights reserved.
// File Name :     Program.cs
// Company :       mpaulosky
// Author :        teqsl
// Solution Name : BigBadBlog
// Project Name :  BigBadBlog.Service.DatabaseMigrations
// =============================================

using BigBadBlog.Common;
using BigBadBlog.Data.Postgres;
using BigBadBlog.Service.DatabaseMigrations;

using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddHostedService<Worker>();

builder.Services.AddDbContext<ApplicationDbContext>(
	options => options.UseNpgsql(builder.Configuration.GetConnectionString(ServiceNames.DatabasePosts.Name))
);

builder.Services.AddOpenTelemetry()
	.WithTracing(tracing => tracing.AddSource(Worker.ActivityName));

var host = builder.Build();

host.Run();