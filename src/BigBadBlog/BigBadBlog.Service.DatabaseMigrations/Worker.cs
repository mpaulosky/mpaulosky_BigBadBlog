// ============================================
// Copyright (c) 2024. All rights reserved.
// File Name :     Worker.cs
// Company :       mpaulosky
// Author :        teqsl
// Solution Name : BigBadBlog
// Project Name :  BigBadBlog.Service.DatabaseMigrations
// =============================================

using System.Diagnostics;

using BigBadBlog.Data.Postgres;

using Microsoft.EntityFrameworkCore;

using OpenTelemetry.Trace;

namespace BigBadBlog.Service.DatabaseMigrations;

public class Worker : BackgroundService
{
	private readonly IServiceProvider _serviceProvider;
	private readonly IHostApplicationLifetime _hostApplicationLifetime;
	private readonly ILogger<Worker> _logger;

	internal const string ActivityName = "MigrationService";
	private static readonly ActivitySource ActivitySource = new(ActivityName);

	public Worker(IServiceProvider serviceProvider,
		IHostApplicationLifetime hostApplicationLifetime,
		ILogger<Worker> logger)
	{
		this._serviceProvider = serviceProvider;
		this._hostApplicationLifetime = hostApplicationLifetime;
		_logger = logger;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		using var activity = ActivitySource.StartActivity("Migrating database", ActivityKind.Client);

		try
		{
			using var scope = _serviceProvider.CreateScope();
			var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

			await dbContext.Database.MigrateAsync(stoppingToken);
		}
		catch (Exception ex)
		{
			activity?.RecordException(ex);
			throw;
		}

		_hostApplicationLifetime.StopApplication();
	}
}