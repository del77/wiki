using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AutoMapper;
using Wiki.Core.Domain;
using Wiki.Core.Repositories;
using Wiki.Infrastructure.DTO;

namespace Wiki.Infrastructure.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository articleRepository;
        private readonly IArticleTagsRepository articleTagsRepository;
        private readonly IMapper mapper;

        public ArticleService(IArticleRepository articleRepository, IArticleTagsRepository articleTagsRepository, IMapper mapper)
        {
            this.mapper = mapper;
            this.articleRepository = articleRepository;
            this.articleTagsRepository = articleTagsRepository;
        }



        public async Task<IEnumerable<ArticleDto>> BrowseAsync(int? selectedStatus, int? selectedUser, int? selectedArticle)
        {
            var articles = await articleRepository.GetAllAsync(selectedStatus, selectedUser, selectedArticle);
            return mapper.Map<IEnumerable<ArticleDto>>(articles);
        }


        public async Task<ArticleDetailsDto> GetAsync(int textid)
        {
            var article = await articleRepository.GetAsync(textid);
            return mapper.Map<ArticleDetailsDto>(article);
        }

        public async Task<FilterInfo> GetFilterInfo()
        {
            var categories = await articleRepository.GetCategories();
            var tags = await articleRepository.GetTags();
            var statuses = await articleRepository.GetStatuses();
            var filter = new FilterInfo();
            filter.Categories = mapper.Map<IEnumerable<ArticleCategoryDto>>(categories);
            filter.Tags = mapper.Map<IEnumerable<TextTagDto>>(tags);
            filter.Statuses = mapper.Map<IEnumerable<TextStatusDto>>(statuses);
            return filter;
        }

        public async Task AddAsync(int articleId, string title, string content, int status, int[] selectedTags, int selectedCategory, int author, double version)
        {
            var article = new Article(articleId);

            var tags = new List<TextTag>();
            foreach (var tag in selectedTags)
            {

                tags.Add(new TextTag(tag));
            }

            var user = new User(author);

            var text = new Text(title, content, version);
            var textStatus = new TextStatus(status);
            text.SetStatus(textStatus);
            text.SetTags(tags);
            text.SetAuthor(user);
            article.SetText(text);

            var category = new ArticleCategory(selectedCategory);
            article.SetCategory(category);
            

            await articleRepository.AddAsync(article);
        }

        public async Task ChangeStatus(int textid, int status, string comment="")
        {
            await articleRepository.UpdateAsync(textid, status, comment);
        }

        public async Task UpdateVersion(int articleId, int textId, string title, string content, int status, int[] selectedTags, int selectedCategory, int author, double version, string textcomment)
        {
            var article = new Article(articleId);

            var tags = await articleTagsRepository.GetTags();
            var tasks = new List<Task>();

            foreach (var tag in tags)
            {
                tasks.Add(articleTagsRepository.RemoveAsync(textId, tag.Id));
            }
            await Task.WhenAll(tasks);
            tasks.Clear();
            foreach (var tag in selectedTags)
            {
                var addedTag = new TextTag(tag, textId);
                tasks.Add(articleTagsRepository.AddAsync(addedTag));
            }

            var user = new User(author);
            var text = new Text(textId, title, content, version);
            var textStatus = new TextStatus(status);
            text.SetStatus(textStatus);
            text.SetAuthor(user);
            text.SetComment(textcomment);
            article.SetText(text);

            var category = new ArticleCategory(selectedCategory);
            article.SetCategory(category);

            foreach(var tag in tags)
            tasks.Add(articleRepository.UpdateAsync(article));
            await Task.WhenAll(tasks);
        }

        public async Task SetSupervisor(int textid, int userId)
        {
            var article = await articleRepository.GetAsync(textid);
            article.Master.SetSupervisor(new User(userId));
            await articleRepository.UpdateAsync(article);
        }
    }
}
