using ProductManagement.Entities;
using ProductManagement.Helpers;
using ProductManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductManagement.Service
{
    public interface ICateService
    {
        IEnumerable<Category> GetAll();
        Category GetById(int id);
        Category Create(Category cateParam);
        void Update(Category cateParam);
        void Delete(int id);
    }
    public class CateService : ICateService
    {
        private UserDBContext _context;
        public CateService(UserDBContext context)
        {
            _context = context;
        }

        public Category Create(Category cateParam)
        {
            var cate = new Category
            {
                CategoryName = cateParam.CategoryName
            };
            _context.Category.Add(cateParam);
            _context.SaveChanges();
            return cateParam;

        }

        public void Delete(int id)
        {
            var cate = _context.Category.Find(id);
            if (cate != null)
            {
                _context.Category.Remove(cate);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Category> GetAll()
        {
            var cate = _context.Category.Select(s => new Category
            {
                CategoryId = s.CategoryId,
                CategoryName = s.CategoryName,
               // Products = _context.Product.Where(x => x.CategoryID == s.CategoryId).ToList()
            });
            return cate;
        }

        public Category GetById(int id)
        {
            return _context.Category.Find(id);
        }

        public void Update(Category cateParam)
        {
            var category = _context.Category.Find(cateParam.CategoryId);

            if (category == null)
                throw new AppException("category not found");

            // update username if it has changed
            if (!string.IsNullOrWhiteSpace(cateParam.CategoryName) && cateParam.CategoryName != category.CategoryName)
            {
                // throw error if the new username is already taken
                if (_context.Category.Any(x => x.CategoryName == cateParam.CategoryName))
                    throw new AppException("CategoryName " + cateParam.CategoryName + " is already taken");

                category.CategoryName = cateParam.CategoryName;
            }

           

            _context.Category.Update(category);
            _context.SaveChanges();
        }

    }

}
