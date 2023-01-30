using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        public ProductController(IUnitOfWork unitOfWork , IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        ////GET
        //public IActionResult Create()
        //{
        //    return View();
        //}

        ////POST
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Create(Product obj)
        //{
        //    if (obj.Name.Length>50)
        //    {
        //        ModelState.AddModelError("Name", "Name should be less than 50 characters");
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.CoverType.Add(obj);
        //        _unitOfWork.Save();

        //        TempData["success"] = "Cover Type created successfully";

        //        return RedirectToAction("Index"); //redirect to action within the same controller. for a diff controller it goes ("actionname", "controllername)
        //    }
        //    return View(obj);

        //}

        //GET
        public IActionResult Upsert(int id)
        {
            ProductVM productVM = new()
            {
                Product = new(),
                CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                CoverTypeList = _unitOfWork.CoverType.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })


            };
            if (id == null || id == 0)
            {
                //create
                //ViewBag.CategoryList = CategoryList;
                //ViewData["CoverTypeList"] = CoverTypeList;
                return View(productVM);
            }
            else
            {
                //update
            }
            return View(productVM);

        }


        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj, IFormFile? file)
        {
           

               if (ModelState.IsValid)
               {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if(file != null)
                {

                    string fileName= Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\products");
                    var extension = Path.GetExtension(file.FileName);
                    using(var fileStreams = new FileStream(Path.Combine(uploads , fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    obj.Product.ImageUrl = @"images\products\" + fileName + extension;
                }
                        
                    _unitOfWork.Product.Add(obj.Product);
                    _unitOfWork.Save();

                    TempData["success"] = "Product created successfully";

                    return RedirectToAction("Index"); //redirect to action within the same controller. for a diff controller it goes ("actionname", "controllername)
               }

            return View(obj);

            }

        ////GET
        //public IActionResult Delete(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    var coverTypeFromDbFirst = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);

        //    if (coverTypeFromDbFirst == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(coverTypeFromDbFirst);

        //}

        ////POST
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public IActionResult DeletePOST(int id)
        //{

        //    var obj = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);
        //    if (obj == null)
        //    {
        //        return NotFound();
        //    }
        //    _unitOfWork.CoverType.Remove(obj);
        //    _unitOfWork.Save();

        //    TempData["success"] = "Cover Type deleted successfully";

        //    return RedirectToAction("Index");

        //}

        #region API CALLS
        [HttpGet]

        public IActionResult GetAll()
        {
            var productList = _unitOfWork.Product.GetAll();
            return Json(new { data = productList });
        }
        #endregion
    }
}
