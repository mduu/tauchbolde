using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tauchbolde.Common.Model;
using Tauchbolde.Common.Repositories;
using Tauchbolde.Web.Models.ViewComponentModels;

namespace Tauchbolde.Web.ViewComponents
{
    public class RecentNewsViewComponent : ViewComponent
    {
        private readonly IPostRepository _postRepository;

        public RecentNewsViewComponent(IPostRepository postRepository)
        {
            if (postRepository == null) throw new ArgumentNullException(nameof(postRepository));

            _postRepository = postRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new RecentNewsViewModel
            {
                RecentNews = await _postRepository.GetNewestPostsForCategoryAsync(PostCategory.News, 15),
            };

            return View(model);
        }
    }
}
