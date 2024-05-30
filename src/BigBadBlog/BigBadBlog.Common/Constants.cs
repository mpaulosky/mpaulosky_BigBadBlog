// ============================================
// Copyright (c) 2024. All rights reserved.
// File Name :     Constants.cs
// Company :       mpaulosky
// Author :        teqsl
// Solution Name : BigBadBlog
// Project Name :  BigBadBlog.Common
// =============================================

namespace BigBadBlog.Common;

public static class ServiceNames
{
	/// <summary>
	///   Constants for referencing the database containing blog posts
	/// </summary>
	public static class DatabasePosts
	{
		public const string Servername = "posts";
		public const string Name = "post-database";
	}

	public const string Migration = "database-migration";

	public const string Outputcache = "OUTPUTCASHE";
}