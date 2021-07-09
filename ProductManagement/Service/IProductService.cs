using ProductManagement.Entities;
using ProductManagement.Helpers;
using ProductManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductManagement.Service
{
    public interface IProductService
    {
        IEnumerable<Product> GetAll();
        Product GetById(int id);
        Product Create(Product productParam);
        void Update(Product productParam);
        void Delete(int id);
    }
    public class ProductService : IProductService
    {
        private UserDBContext _context;
        public ProductService(UserDBContext context)
        {
            _context = context;
        }
        public Product Create(Product productParam)
        {
            var product = new Product
            {
                ProductName = productParam.ProductName,
                Amount = productParam.Amount,
                UnitPrice = productParam.UnitPrice,
                Category = _context.Category.Find(productParam.CategoryID)

            };
            _context.Product.Add(productParam);
            _context.SaveChanges();
            return productParam;
        }

        public void Delete(int id)
        {
            var pro = _context.Product.Find(id);
            if (pro != null)
            {
                _context.Product.Remove(pro);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Product> GetAll()
        {
            var product = _context.Product.Select(s => new Product
            {
                ProductId = s.ProductId,
                ProductName = s.ProductName,
                Amount = s.Amount,
                UnitPrice = s.UnitPrice,
                CategoryID = s.CategoryID,
                Category = _context.Category.Where(a => a.CategoryId == s.Category.CategoryId).FirstOrDefault()

            }).ToList();
            return product;
        }

        public Product GetById(int id)
        {
            return _context.Product.Find(id);
        }

        public void Update(Product productParam)
        {
            var product = _context.Product.Find(productParam.ProductId);

            if (product == null)
                throw new AppException("category not found");

            // update username if it has changed
            if (!string.IsNullOrWhiteSpace(productParam.ProductName)) 
                product.ProductName = productParam.ProductName;

            product.Category = _context.Category.Find(productParam.Category);
            if (!string.IsNullOrWhiteSpace((productParam.Amount).ToString()))
                product.Amount = productParam.Amount;
            if (!string.IsNullOrWhiteSpace((productParam.UnitPrice).ToString()))
                product.UnitPrice = productParam.UnitPrice;

            _context.Product.Update(product);
            _context.SaveChanges();
        }
    }
}
