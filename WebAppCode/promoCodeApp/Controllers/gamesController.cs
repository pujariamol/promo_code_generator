using promoCodeApp.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace promoCodeApp.Controllers
{
    [Authorize]
    public class gamesController : Controller
    {
        private promoCodeDbContext db = new promoCodeDbContext();

        // GET: games
        public async Task<ActionResult> Index()
        {
            if (User.IsInRole("Admin"))
            {
                var games = db.games.Include(g => g.registration).Where(x => x.isDeleted == false);
                return View(await games.ToListAsync());
            }
            else
            {
                var games = db.games.Include(g => g.registration).Where(x => x.registration.email == User.Identity.Name && x.isDeleted == false);
                return View(await games.ToListAsync());
            }
        }

        // GET: games/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            game game = await db.games.FindAsync(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            return View(game);
        }

        // GET: games/Create
        public ActionResult Create()
        {
            ViewBag.registrationId = new SelectList(db.registrations.Where(x => x.email == User.Identity.Name && x.isDeleted == false), "registrationId", "registrationName");
            return View();
        }

        // POST: games/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "gameId,registrationId,gameName,isActive,dateTime")] game game)
        {
            if (ModelState.IsValid)
            {
                db.games.Add(game);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.registrationId = new SelectList(db.registrations.Where(x => x.email == User.Identity.Name && x.isDeleted == false), "registrationId", "registrationName", game.registrationId);
            return View(game);
        }

        // GET: games/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            game game = await db.games.FindAsync(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            ViewBag.registrationId = new SelectList(db.registrations.Where(x => x.email == User.Identity.Name && x.isDeleted == false), "registrationId", "registrationName", game.registrationId);
            return View(game);
        }

        // POST: games/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "gameId,registrationId,gameName,isActive,dateTime")] game game)
        {
            if (ModelState.IsValid)
            {
                db.Entry(game).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.registrationId = new SelectList(db.registrations.Where(x => x.email == User.Identity.Name && x.isDeleted == false), "registrationId", "registrationName", game.registrationId);
            return View(game);
        }

        // GET: games/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            game game = await db.games.FindAsync(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            return View(game);
        }

        // POST: games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            game game = await db.games.FindAsync(id);
            game.isDeleted = true;
            db.Entry(game).State = EntityState.Modified;
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
