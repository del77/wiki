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
        private readonly IMapper mapper;
        public ArticleService(IArticleRepository articleRepository, IMapper mapper)
        {
            this.mapper = mapper;
            this.articleRepository = articleRepository;
        }



        public async Task<IEnumerable<ArticleDto>> BrowseAsync(string title, IEnumerable<int> selectedTags, int selectedCategory, int selectedStatus, int selectedArticle=0)
        {
            var articles = await articleRepository.GetAllAsync(selectedTags, title, selectedCategory, selectedStatus, selectedCategory);
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

        public async Task AddAsync(int articleId, string title, string content, int[] selectedTags, int selectedCategory, int author, double version)
        {
            author = 0;
            var article = new Article(articleId);

            var tags = new List<TextTag>();
            foreach (var tag in selectedTags)
            {
                tags.Add(new TextTag(tag));
            }

            var user = new User(author);

            var text = new Text(title, content, version);
            var status = new TextStatus(2); // waiting
            text.SetStatus(status);
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
    }
}
