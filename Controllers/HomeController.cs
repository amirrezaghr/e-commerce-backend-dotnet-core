using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyEshop.Data;
using MyEshop.Models;
using ZarinpalSandbox;

namespace MyEshop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private MyEshopContext _Context;
        

        public HomeController(ILogger<HomeController> logger,MyEshopContext context)
        {
            _logger = logger;
            _Context = context;
        }

        public IActionResult Index()
        {
            var products = _Context.Products.ToList();
            return View(products);
        }
        public IActionResult Detail(int id)
        {
            var product = _Context.Products
                .Include(p => p.Item)
                .SingleOrDefault(p => p.Id == id);
            if(product == null)
            {
                return NotFound();
            }

            var categories = _Context.Products
                .Where(p => p.Id == id)
                .SelectMany(c => c.CategoryToProducts)
                .Select(ca => ca.Category)
                .ToList();

            var vm = new DetailViewModel()
            {
                Product = product,
                Categories = categories
            };

            return View(vm);
        }


        //[Authorize]
        public IActionResult AddToCart(int itemId)
        {
            var product = _Context.Products.Include(p => p.Item).SingleOrDefault(p => p.ItemId == itemId);
            if(product != null)
            {
                int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString());
                var order = _Context.Orders.FirstOrDefault(o => o.UserId == userId && !o.IsFinaly);
                if (order != null)
                {
                    var orderDatail = _Context.OrderDetails.FirstOrDefault(d => d.OrderId == order.OrderId && d.ProductId == product.Id);
                    if(orderDatail != null)
                    {
                        orderDatail.Count += 1;
                    }
                    else
                    {
                        _Context.OrderDetails.Add(new OrderDetail()
                        {
                            OrderId = order.OrderId,
                            ProductId = product.Id,
                            Price = product.Item.Price,
                            Count = 1
                        });
                    }
                }
                else
                {
                    order = new Order()
                    {
                        IsFinaly = false,
                        CreateDate = DateTime.Now,
                        UserId = userId
                    };
                    _Context.Orders.Add(order);
                    _Context.SaveChanges();
                    _Context.OrderDetails.Add(new OrderDetail()
                    {
                        OrderId = order.OrderId,
                        ProductId = product.Id,
                        Price = product.Item.Price,
                        Count = 1
                    });
                }
                _Context.SaveChanges();
            }
            return RedirectToAction("ShowCart");
        }

        //[Authorize]
        public IActionResult ShowCart()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString());
            var order = _Context.Orders.Where(o => o.UserId == userId)
                .Include(o => o.OrderDetails)
                .ThenInclude(c => c.Product).FirstOrDefault();


            return View(order);
        }
        
        //[Authorize]
        public IActionResult RemoveCart(int detailId)
        {

            var orderDatail = _Context.OrderDetails.Find(detailId);
            _Context.Remove(orderDatail);
            _Context.SaveChanges();
            return RedirectToAction("ShowCart");
        }

        [Route("ContactUs")]
        public IActionResult ContactUs()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Payment()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var order = _Context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefault(o => o.UserId == userId && !o.IsFinaly);

            if (order == null)
                return NotFound();

            var payment = new Payment((int)order.OrderDetails.Sum(d => d.Price));
            var res = payment.PaymentRequest($"پرداخت فاکتور شماره {order.OrderId}",
               "http://localhost:1635/Home/OnlinePayment/" + order.OrderId, "Iman@Madaeny.com", "09197070750");
            if (res.Result.Status == 100)
            {
                return Redirect("https://sandbox.zarinpal.com/pg/StartPay/" + res.Result.Authority);
            }
            else
            {
                return BadRequest();
            }
        }


        public IActionResult OnlinePayment(int id)
        {
            if (HttpContext.Request.Query["Status"] != "" &&
                HttpContext.Request.Query["Status"].ToString().ToLower() == "ok" &&
                HttpContext.Request.Query["Authority"] != "")
            {
                string authority = HttpContext.Request.Query["Authority"].ToString();
                var order = _Context.Orders.Include(o => o.OrderDetails)
                    .FirstOrDefault(o => o.OrderId == id);
                var payment = new Payment((int)order.OrderDetails.Sum(d => d.Price));
                var res = payment.Verification(authority).Result;
                if (res.Status == 100)
                {
                    order.IsFinaly = true;
                    _Context.Orders.Update(order);
                    _Context.SaveChanges();
                    ViewBag.code = res.RefId;
                    return View();
                }
            }

            return NotFound();
        }
    }
}
