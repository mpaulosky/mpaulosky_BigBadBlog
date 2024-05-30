// ============================================
// Copyright (c) 2024. All rights reserved.
// File Name :     PgPost.cs
// Company :       mpaulosky
// Author :        teqsl
// Solution Name : BigBadBlog
// Project Name :  BigBadBlog.Data.Postgres
// =============================================

using System.ComponentModel.DataAnnotations;

using BigBadBlog.Common;

namespace BigBadBlog.Data.Postgres;

public class PgPost
{
	[Key] public int Id { get; set; }

	[Required, MaxLength(100)] public required string Title { get; set; }

	[Required, MaxLength(100)] public required string Author { get; set; }

	[Required] public required string Content { get; set; }

	[Required, MaxLength(150)] public required string Slug { get; set; }

	[Required] public DateTime Date { get; set; }


	// explicit conversion to PostMetadata
	public static explicit operator PostMetadata(PgPost post)
	{
		return new PostMetadata("", post.Title, post.Author, post.Date);
	}

	// explicit conversation from PostMetadata
	public static explicit operator PgPost((PostMetadata, string) post)
	{
		return new PgPost
		{
			Title = post.Item1.Title,
			Author = post.Item1.Author,
			Date = post.Item1.Date,
			Content = post.Item2,
			Slug = post.Item1.Slug
		};
	}
}