// ============================================
// Copyright (c) 2024. All rights reserved.
// File Name :     IPostRepository.cs
// Company :       mpaulosky
// Author :        teqsl
// Solution Name : BigBadBlog
// Project Name :  BigBadBlog.Common
// =============================================

namespace BigBadBlog.Common;

/// <summary>
///   A definition of a simple interaction with blog posts
/// </summary>
public interface IPostRepository
{
	Task<IEnumerable<(PostMetadata, string)>> GetPostsAsync(int count, int page);

	Task<(PostMetadata, string)> GetPostAsync(string slug);

	Task AddPostAsync(PostMetadata metadata, string content);
}

public record PostMetadata(string Filename, string Title, string Author, DateTime Date)
{
	public string Slug => Uri.EscapeDataString(Title.ToLower());
}