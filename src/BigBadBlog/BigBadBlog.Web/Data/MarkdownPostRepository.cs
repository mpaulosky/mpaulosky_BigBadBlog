// ============================================
// Copyright (c) 2024. All rights reserved.
// File Name :     MarkdownPostRepository.cs
// Company :       mpaulosky
// Author :        teqsl
// Solution Name : BigBadBlog
// Project Name :  BigBadBlog.Web
// =============================================

using BigBadBlog.Common;

using Markdig;
using Markdig.Extensions.Yaml;
using Markdig.Syntax;

namespace BigBadBlog.Web.Data;

/// <summary>
///   A simple repository for blog posts that are saved as markdown files in the Posts folder
/// </summary>
public class MarkdownPostRepository : IPostRepository
{
	private static readonly Dictionary<PostMetadata, string> Posts = new();
	private readonly MarkdownPipeline _markdownPipeline;

	public MarkdownPostRepository()
	{
		_markdownPipeline = new MarkdownPipelineBuilder()
			.UseYamlFrontMatter()
			.Build();

		if (!Posts.Any())
		{
			var files = Directory.GetFiles("Posts", "*.md");
			foreach (var file in files)
			{
				var newPost = ExtractMetadataFromFile(file);
				Posts.Add(newPost.Item1, newPost.Item2);
			}
		}
	}

	public Task<(PostMetadata, string)> GetPostAsync(string slug)
	{
		var toCheck = Uri.EscapeDataString(slug.ToLower());
		var thePost = Posts.FirstOrDefault(p => p.Key.Slug == toCheck);

		return Task.FromResult((thePost.Key, thePost.Value));
	}

	public Task AddPostAsync(PostMetadata metadata, string content)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<(PostMetadata, string)>> GetPostsAsync(int count, int page)
	{
		count = count < 1 ? 10 : count;
		page = page < 1 ? 1 : page;

		return Posts.Any()
			? Task.FromResult(Posts
				.OrderByDescending(p => p.Key.Date)
				.Skip((page - 1) * count)
				.Take(count)
				.Select(p => (p.Key, p.Value))
				.AsEnumerable())
			: Task.FromResult(Enumerable.Empty<(PostMetadata, string)>());
	}

	private (PostMetadata, string) ExtractMetadataFromFile(string fileName)
	{
		// Read all content from the file
		var content = File.ReadAllText(fileName);

		var yamlBlock = Markdown.Parse(content, _markdownPipeline)
			.Descendants<YamlFrontMatterBlock>()
			.FirstOrDefault();

		var yamlDictionary = new Dictionary<string, string>();
		foreach (var line in yamlBlock.Lines)
		{
			if (line.ToString().Contains(": "))
			{
				var values = line.ToString().Split(": ", StringSplitOptions.TrimEntries);
				yamlDictionary.Add(values[0].ToLower(), values[1]);
			}
		}

		return (new PostMetadata(
			fileName,
			yamlDictionary["title"],
			yamlDictionary["author"],
			DateTime.Parse(yamlDictionary["date"])
		), content);
	}
}