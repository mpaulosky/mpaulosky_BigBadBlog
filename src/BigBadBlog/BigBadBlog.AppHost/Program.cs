// ============================================
// Copyright (c) 2024. All rights reserved.
// File Name :     Program.cs
// Company :       mpaulosky
// Author :        teqsl
// Solution Name : BigBadBlog
// Project Name :  BigBadBlog.AppHost
// =============================================

using BigBadBlog.Common;

using Projects;

var builder = DistributedApplication.CreateBuilder(args);

#region Add Aspire Redis Cache

var cache = builder.AddRedis("outputcache")
	.WithRedisCommander();

#endregion

#region Add Postgres Database

var postgresPassword =
	builder.AddParameter("postgrespassword", secret: true);

var db =
	builder.AddPostgres(ServiceNames.DatabasePosts.Servername, password: postgresPassword)
		.WithDataVolume()
		.WithPgAdmin()
		.AddDatabase(ServiceNames.DatabasePosts.Name);

#endregion

#region Add Website

var webApp = builder.AddProject<BigBadBlog_Web>("web")
	.WithReference(cache)
	.WithReference(db)
	.WithExternalHttpEndpoints();

#endregion

var migrationService =
	builder.AddProject<BigBadBlog_Service_DatabaseMigrations>(ServiceNames.Migration)
		.WithReference(db);

builder.Build().Run();