using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;
using RaabbitMQWeb.ExcelCreate.Models;
using RaabbitMQWeb.ExcelCreate.Services;
using Shared;

namespace RaabbitMQWeb.ExcelCreate.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManger;
        private readonly RabbitMQPublisher _rabbitMqPublisher;

        public ProductController(UserManager<IdentityUser> userManger, AppDbContext context, RabbitMQPublisher rabbitMqPublisher)
        {
            _userManger = userManger;
            _context = context;
            _rabbitMqPublisher = rabbitMqPublisher;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CreateProductExcel()
        {
            var user =await _userManger.FindByNameAsync(User.Identity.Name);

            var fileName = $"product-excel-{Guid.NewGuid().ToString().Substring(1, 10)}";

            UserFile userfile = new()
            {
                UserId = user.Id,
                FileName = fileName,
                FileStatus = FileStatus.Creating
            };

           await _context.UserFiles.AddAsync(userfile);
           await _context.SaveChangesAsync();
           //rabbitMQ mesaj gonderimi
           _rabbitMqPublisher.Publish(new CreateExcelMessage(){FileId = userfile.Id });
           TempData["StartCreatingExcel"] = true;
           return RedirectToAction(nameof(Files));


        }

        public async Task<IActionResult> Files()
        {
            var user = await _userManger.FindByNameAsync(User.Identity.Name);
            return View(await _context.UserFiles.Where(x => x.UserId == user.Id).OrderByDescending(x => x.Id).ToListAsync());
        }

    }
}
