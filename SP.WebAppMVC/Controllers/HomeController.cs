using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using SP.DataAccess.Entities;
using SP.DataAccess.Services.Interfaces;

namespace SP.WebAppMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStudentsService _service;

        public HomeController(IStudentsService service)
        {
            _service = service;
        }

        public async Task<ActionResult> Index()
        {
            IEnumerable<Student> students = await _service.GetAllAsync();
            return View(students);
        }

        // GET: Home/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Home/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Title")] Student student)
        {
            if (ModelState.IsValid)
            {
                await _service.AddAsync(student);
                return RedirectToAction("Index");
            }

            return View(student);
        }

        // GET: Home/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            Student student = await _service.GetOneAsync(id);
            return View(student);
        }

        // POST: Home/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title")] Student student)
        {
            if (ModelState.IsValid)
            {
                await _service.UpdateAsync(student);
                return RedirectToAction("Index");
            }

            return RedirectToAction("Edit");
        }

        // POST: Home/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}