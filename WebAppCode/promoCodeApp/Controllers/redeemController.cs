using promoCodeApp.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace promoCodeApp.Controllers
{
    public class redeemController : ApiController
    {
        private promoCodeDbContext db = new promoCodeDbContext();
        public async Task<HttpResponseMessage> postCode(RedeemApiModel parameter)
        {
            try
            {
                if (parameter == null || string.IsNullOrEmpty(parameter.clientSecretKey) || string.IsNullOrEmpty(parameter.promotionCode))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }

                if (db.registrations.Count(x => x.clientServerKey == parameter.clientSecretKey) > 0)
                {
                    promotion promo = db.promotions.Where(x => x.promotionCode == parameter.promotionCode && x.gamePackage.game.registration.clientServerKey == parameter.clientSecretKey && x.isRedeemed == false && x.gamePackage.isActive == true && x.isActive == true).FirstOrDefault();
                    if (promo != null)
                    {
                        promo.isRedeemed = true;
                        promo.numberOfCodesToBeGenerated = 1;
                        db.Entry(promo).State = EntityState.Modified;
                        await db.SaveChangesAsync();

                        return Request.CreateResponse(HttpStatusCode.Created, promo.gamePackage.secretKey);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "Sorry, either code is alreay redeemed or not found.");
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Bad Request, Please check all parameters.");
                }
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Internal Server Error, Please contact admin.");
            }
            finally { }

        }
    }
}
