using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrangWebBTNhom.Models;

namespace TrangWebBTNhom.Controllers
{
    public class NewsController : Controller
    {
		// GET: News
		MollaEntities1 db = new MollaEntities1();
		public ActionResult Index(Guid? CategoryId, int Page = 1)
        {
			int itemPerPage = 6;
			var categoties = db.Categories.Where(c => c.ParentId.ToString() == "15C03BF0-CBDD-462E-B254-0257A5B3EF7E").Select(c => new List<string> { c.Id.ToString(), c.Name, db.News_Category.Where(n => n.CategoryId == c.Id).Count().ToString() }).ToList();
			ViewBag.Categories = categoties;
			var items = db.News.ToList();
			if (CategoryId != null)
				items = items.Where(n => db.News_Category.Where(c => c.CategoryId == CategoryId).Select(c => c.NewId).Contains(n.Id)).ToList();
			items = items.OrderByDescending(n => n.CreatedDate).Skip((Page -1)*itemPerPage).Take(itemPerPage).ToList();

			var news = items.Select(i => new List<string>
			{
				i.Picture,
				db.Members.Find(i.CreatedBy).Name,
				i.CreatedDate.Value.ToString("dd-MM-yyyy"),
				db.Comments.Where(c=>c.NewsId==i.Id).Count().ToString(),
				i.Title,
				String.Join(", ",db.Categories.Where(b=>db.News_Category.Where(d=>d.NewId==i.Id).Select(d=>d.CategoryId).Contains(b.Id)).Select(b=>b.Name).ToArray()),
				i.Description,
				i.Id.ToString()
			}).ToList(); 
			ViewBag.News = news;
			return View();
        }
    }
}