using promoCodeApp.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace promoCodeApp.Controllers
{
    [Authorize]
    public class registrationsController : Controller
    {
        private promoCodeDbContext db = new promoCodeDbContext();

        // GET: registrations
        public async Task<ActionResult> Index()
        {
            if (User.IsInRole("Admin"))
            {
                return View(await db.registrations.Where(x => x.isDeleted == false).ToListAsync());
            }
            else
            {
                return View(await db.registrations.Where(x => x.email == User.Identity.Name && x.isDeleted == false).ToListAsync());
            }
        }

        // GET: registrations/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            registration registration = await db.registrations.FindAsync(id);
            if (registration == null)
            {
                return HttpNotFound();
            }
            return View(registration);
        }

        // GET: registrations/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: registrations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(registration registration)
        {
            registration.email = User.Identity.Name;
            if (ModelState.IsValid)
            {
                db.registrations.Add(registration);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(registration);
        }

        // GET: registrations/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            registration registration = await db.registrations.FindAsync(id);
            if (registration == null)
            {
                return HttpNotFound();
            }
            return View(registration);
        }

        // POST: registrations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(registration registration)
        {
            if (ModelState.IsValid)
            {
                db.Entry(registration).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(registration);
        }

        // GET: registrations/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            registration registration = await db.registrations.FindAsync(id);
            if (registration == null)
            {
                return HttpNotFound();
            }
            return View(registration);
        }

        // POST: registrations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            registration registration = await db.registrations.FindAsync(id);
            registration.isDeleted = true;
            db.Entry(registration).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
