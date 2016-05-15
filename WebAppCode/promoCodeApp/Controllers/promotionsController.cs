using promoCodeApp.Helpers;
using promoCodeApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace promoCodeApp.Controllers
{
    [Authorize]
    public class promotionsController : Controller
    {
        private promoCodeDbContext db = new promoCodeDbContext();

        // GET: promotions
        public async Task<ActionResult> Index()
        {
            if (User.IsInRole("Admin"))
            {
                var promotions = db.promotions.Include(p => p.gamePackage).Include(p => p.type).Where(x => x.isDeleted == false);
                return View(await promotions.ToListAsync());
            }
            else
            {
                var promotions = db.promotions.Include(p => p.gamePackage).Include(p => p.type).Where(x => x.gamePackage.game.registration.email == User.Identity.Name && x.isDeleted == false);
                return View(await promotions.ToListAsync());
            }



        }

        public ActionResult PrintView(List<promotion> promos)
        {
            return View(promos);

        }

        // GET: promotions/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            promotion promotion = await db.promotions.FindAsync(id);
            if (promotion == null)
            {
                return HttpNotFound();
            }
            return View(promotion);
        }

        // GET: promotions/Create
        public ActionResult Create()
        {
            ViewBag.gamePackageId = new SelectList(db.gamePackages.Where(x => x.game.registration.email == User.Identity.Name && x.isDeleted == false && x.isActive == true), "gamePackageId", "packageName");
            ViewBag.typeId = new SelectList(db.types, "typeId", "name");
            return View();
        }

        // POST: promotions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "promotionId,gamePackageId,typeId,startDate,endDate,promotionCode,isRedeemed,isActive,dateTime,numberOfCodesToBeGenerated,customCodeValue")] promotion promotionParameter)
        {
            if (ModelState.IsValid)
            {
                List<promotion> recordsgenerated;
                try
                {
                    recordsgenerated = new List<promotion>();
                    for (int i = 0; i < promotionParameter.numberOfCodesToBeGenerated; i++)
                    {
                        promotion loopObject = new promotion();
                        loopObject.gamePackageId = promotionParameter.gamePackageId;
                        loopObject.typeId = promotionParameter.typeId;
                        loopObject.startDate = promotionParameter.startDate;
                        loopObject.endDate = promotionParameter.endDate;
                        loopObject.isActive = true;
                        loopObject.isRedeemed = false;
                        loopObject.promotionCode = string.IsNullOrEmpty(promotionParameter.customCodeValue) ? PromoUtil.GenerateCode() : promotionParameter.customCodeValue;
                        loopObject.numberOfCodesToBeGenerated = 1;

                        db.promotions.Add(loopObject);
                        await db.SaveChangesAsync();
                        recordsgenerated.Add(loopObject);
                        loopObject = null;
                    }
                    return View("PrintView", recordsgenerated);
                }
                catch
                {
                    return View("Error", "Cannot Save your record. Please try again later or contact administrator");
                }
                finally
                {
                    recordsgenerated = null;
                }
            }

            ViewBag.gamePackageId = new SelectList(db.gamePackages.Where(x => x.game.registration.email == User.Identity.Name && x.isDeleted == false && x.isActive == true), "gamePackageId", "packageName", promotionParameter.gamePackageId);
            ViewBag.typeId = new SelectList(db.types.Where(x => x.isDeleted == false), "typeId", "name", promotionParameter.typeId);
            return View(promotionParameter);
        }

        // GET: promotions/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            promotion promotion = await db.promotions.FindAsync(id);
            if (promotion == null)
            {
                return HttpNotFound();
            }
            ViewBag.gamePackageId = new SelectList(db.gamePackages.Where(x => x.game.registration.email == User.Identity.Name && x.isDeleted == false && x.isActive == true), "gamePackageId", "packageName", promotion.gamePackageId);
            ViewBag.typeId = new SelectList(db.types, "typeId", "name", promotion.typeId);
            return View(promotion);
        }

        // POST: promotions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "promotionId,gamePackageId,typeId,startDate,endDate,promotionCode,isRedeemed,isActive,dateTime")] promotion promotion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(promotion).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.gamePackageId = new SelectList(db.gamePackages.Where(x => x.game.registration.email == User.Identity.Name && x.isDeleted == false && x.isActive == true), "gamePackageId", "packageName", promotion.gamePackageId);
            ViewBag.typeId = new SelectList(db.types, "typeId", "name", promotion.typeId);
            return View(promotion);
        }

        // GET: promotions/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            promotion promotion = await db.promotions.FindAsync(id);
            if (promotion == null)
            {
                return HttpNotFound();
            }
            return View(promotion);
        }

        // POST: promotions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            promotion promotion = await db.promotions.FindAsync(id);
            promotion.isDeleted = true;
            db.Entry(promotion).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UploadFiles(HttpPostedFileBase file)
        {
            AzureStorageFileUpload azureFileUpload = null;

            try
            {
                azureFileUpload = new AzureStorageFileUpload();

                string extension = Path.GetExtension(file.FileName);
                string fileNameWithoutExtension = file.FileName.Substring(0, file.FileName.Length - Path.GetExtension(file.FileName).Length);
                var newFileName = fileNameWithoutExtension + "-" + Guid.NewGuid().ToString() + extension;
                newFileName = newFileName.Replace(" ", "");
                newFileName = newFileName.Replace("-", "");

                string uploadedFileName = azureFileUpload.uploadFiletoStorage(newFileName, file);

                var result = new { error = 0, message = uploadedFileName };
                return Json(result);

            }
            catch (Exception ex)
            {
                var errorFileExtn = new { error = 1, message = "Error: " + ex.Message };
                return Json(errorFileExtn);
            }
            finally
            {
                azureFileUpload = null;
            }

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
