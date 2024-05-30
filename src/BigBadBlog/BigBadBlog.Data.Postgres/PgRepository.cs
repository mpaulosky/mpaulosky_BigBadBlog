// ============================================
// Copyright (c) 2024. All rights reserved.
// File Name :     PgRepository.cs
// Company :       mpaulosky
// Author :        teqsl
// Solution Name : BigBadBlog
// Project Name :  BigBadBlog.Data.Postgres
// =============================================

using BigBadBlog.Common;

namespace BigBadBlog.Data.Postgres;

internal class PgRepository(ApplicationDbContext dbContext) : IPostRepository
{
	public Task<IEnumerable<(PostMetadata, string)>> GetPostsAsync(int count, int page)
	{
		var posts = dbContext.Posts
			.OrderByDescending(p => p.Date)
			.Skip((page - 1) * count)
			.Take(count)
			.ToList();

		return Task.FromResult(posts
			.Select(p => ((PostMetadata)p, p.Content)));
	}

	public Task<(PostMetadata, string)> GetPostAsync(string slug)
	{
		var post = dbContext.Posts
			.FirstOrDefault(p => p.Slug == slug);

		return Task.FromResult(((PostMetadata)post, post.Content));
	}

	public async Task AddPostAsync(PostMetadata metadata, string content)
	{
		var post = (PgPost)(metadata, content);

		post.Date = new DateTime(DateTime.Now.Ticks, DateTimeKind.Utc);
		dbContext.Posts.Add(post);

		await dbContext.SaveChangesAsync();
	}
}