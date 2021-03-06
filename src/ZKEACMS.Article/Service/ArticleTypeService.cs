/* http://www.zkea.net/ Copyright 2016 ZKEASOFT http://www.zkea.net/licenses */
using Easy.Extend;
using Easy.RepositoryPattern;
using System.Collections.Generic;
using ZKEACMS.Article.Models;
using Easy;
using Microsoft.EntityFrameworkCore;
using System;

namespace ZKEACMS.Article.Service
{
    public class ArticleTypeService : ServiceBase<ArticleType, ArticleDbContext>, IArticleTypeService
    {
        private IArticleService _articleService;

        public ArticleTypeService(IApplicationContext applicationContext, IArticleService articleService) : base(applicationContext)
        {
            _articleService = articleService;
        }

        public override DbSet<ArticleType> CurrentDbSet
        {
            get
            {
                return DbContext.ArticleType;
            }
        }

        public override void Add(ArticleType item)
        {
            item.ParentID = item.ParentID ?? 0;
            base.Add(item);
        }

        public IEnumerable<ArticleType> GetChildren(long id)
        {
            return Get(m => m.ParentID == id);
        }
        public override void Remove(ArticleType item, bool saveImmediately = true)
        {
            if (item != null)
            {
                GetChildren(item.ID).Each(m =>
                {
                    _articleService.Remove(n => n.ArticleTypeID == m.ID);
                    Remove(m.ID);
                });
                _articleService.Remove(n => n.ArticleTypeID == item.ID);
            }
            base.Remove(item, saveImmediately);
        }

    }
}