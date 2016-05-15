using promoCodeApp.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace promoCodeApp.Controllers
{
    [Authorize]
    public class gamePackagesController : Controller
    {
        private promoCodeDbContext db = new promoCodeDbContext();

        // GET: gamePackages
        public async Task<ActionResult> Index()
        {
            if (User.IsInRole("Admin"))
            {
                var gamePackages = db.gamePackages.Include(g => g.game).Where(x => x.isDeleted == false);
                return View(await gamePackages.ToListAsync());
            }
            else
            {
                var gamePackages = db.gamePackages.Include(g => g.game).Where(x => x.game.registration.email == User.Identity.Name && x.isDeleted == false);
                return View(await gamePackages.ToListAsync());
            }
        }

        // GET: gamePackages/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gamePackage gamePackage = await db.gamePackages.FindAsync(id);
            if (gamePackage == null)
            {
                return HttpNotFound();
            }
            return View(gamePackage);
        }

        // GET: gamePackages/Create
        public ActionResult Create()
        {
            ViewBag.gameId = new SelectList(db.games.Where(x => x.registration.email == User.Identity.Name && x.isDeleted == false && x.isActive == true), "gameId", "gameName");
            return View();
        }

        // POST: gamePackages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "gamePackageId,gameId,packageName,secretKey,isActive,datetime")] gamePackage gamePackage)
        {
            if (ModelState.IsValid)
            {
                db.gamePackages.Add(gamePackage);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.gameId = new SelectList(db.games.Where(x => x.registration.email == User.Identity.Name && x.isDeleted == false && x.isActive == true), "gameId", "gameName", gamePackage.gameId);
            return View(gamePackage);
        }

        // GET: gamePackages/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gamePackage gamePackage = await db.gamePackages.FindAsync(id);
            if (gamePackage == null)
            {
                return HttpNotFound();
            }
            ViewBag.gameId = new SelectList(db.games.Where(x => x.registration.email == User.Identity.Name && x.isDeleted == false && x.isActive == true), "gameId", "gameName", gamePackage.gameId);
            return View(gamePackage);
        }

        // POST: gamePackages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "gamePackageId,gameId,packageName,secretKey,isActive,datetime")] gamePackage gamePackage)
        {
            if (ModelState.IsValid)
            {
                List<promotion> allGamePackagePromotions = null;
                allGamePackagePromotions = db.promotions.Where(x => x.gamePackage.gamePackageId == gamePackage.gamePackageId).ToList();
                if (null != allGamePackagePromotions && allGamePackagePromotions.Count > 0)
                {
                    foreach (promotion promo in allGamePackagePromotions)
                    {
                        promo.isActive = gamePackage.isActive;
                        promo.numberOfCodesToBeGenerated = 1;
                        db.Entry(promo).State = EntityState.Modified;
                        await db.SaveChangesAsync();

                    }
                }


                db.Entry(gamePackage).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.gameId = new SelectList(db.games.Where(x => x.registration.email == User.Identity.Name && x.isDeleted == false && x.isActive == true), "gameId", "gameName", gamePackage.gameId);
            return View(gamePackage);
        }

        // GET: gamePackages/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gamePackage gamePackage = await db.gamePackages.FindAsync(id);
            if (gamePackage == null)
            {
                return HttpNotFound();
            }
            return View(gamePackage);
        }

        // POST: gamePackages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            gamePackage gamePackage = await db.gamePackages.FindAsync(id);
            gamePackage.isDeleted = true;
            db.Entry(gamePackage).State = EntityState.Modified;
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
