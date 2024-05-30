// ============================================
// Copyright (c) 2024. All rights reserved.
// File Name :     Program.cs
// Company :       mpaulosky
// Author :        teqsl
// Solution Name : BigBadBlog
// Project Name :  BigBadBlog.AppHost
// =============================================

using Projects;

var builder = DistributedApplication.CreateBuilder(args);

// Add Aspire Redis Cache
var cache = builder.AddRedis("outputcache")
	.WithRedisCommander();

// Add Website
var webApp = builder.AddProject<BigBadBlog_Web>("web")
	.WithReference(cache);

builder.Build().Run();