using Domain.Abstract;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Policy;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebUI.Infrastructure.Abstract;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class CartController : Controller
    {
        private IRepository repository;

        public CartController(IRepository repo)
        {
            repository = repo;
        }

        public ViewResult Index(Cart cart, string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }

        public RedirectToRouteResult AddToCart(Cart cart, int activityId, string returnUrl)
        {
            Activity activity = repository.Activities
                .FirstOrDefault(g => g.ActivityId == activityId);

            if (activity != null)
            {
                cart.AddItem(activity, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart, int activityId, string returnUrl)
        {
            Activity activity = repository.Activities
                .FirstOrDefault(g => g.ActivityId == activityId);

            if (activity != null)
            {
                cart.RemoveLine(activity);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }

        public ViewResult Checkout()
        {
            return View(new Participant());
        }

        StringBuilder idActivity = new StringBuilder(10);

        [HttpPost]
        public ViewResult Checkout(Cart cart, Participant participant)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Извините, ваша корзина пуста!");
            }

            if (ModelState.IsValid)
            {
                int id = 0;
                var idActivitys = cart.Lines.GroupBy(s => s.Activity.ActivityId).Select(g => g.Key);
                foreach (int activity in idActivitys)
                {
                    idActivity.Append(activity + ".");
                }

                if (repository.Participants.Any(o => o.Email == participant.Email))
                {
                    id = repository.Participants.Where(c => c.Email == participant.Email)
                        .GroupBy(s => s.ParticipantId).Select(g => g.Key).First();
                }
                else
                {
                    repository.SaveParticipants(participant);
                    id = participant.ParticipantId;
                }

                SendEmail(new MailAddress(participant.Email), id, idActivity);
                cart.Clear();
                return View("Completed");
            }
            else
            {
                return View(participant);
            }           
        }

        Subscription subscription = new Subscription();
        public ViewResult Registr(string returnUrl)
        {
            string[] words = returnUrl.TrimEnd('.').Split(new char[] {'/'});
            string[] b = words[1].Split(new char[] { '.' });
            for (int i=0; i < b.Length; i++)
            {
                subscription.ActivityId = int.Parse(b[i]);
                subscription.ParticipantId = int.Parse(words[0]);
                repository.SaveSubcription(subscription);
            }

            return View();
        }

        MailAddress fromMailAddress = new MailAddress("sa4kovsky@gmail.com", "EventPlanning");

        public void SendEmail(MailAddress toMailAddress, int idParticipant, StringBuilder idActivity)
        {
            using (MailMessage m = new MailMessage(fromMailAddress, toMailAddress))
            using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                Url callbackUrl = new Url("http://localhost:59993/Cart/Registr?returnUrl=" + idParticipant + "/" + idActivity);

                m.Subject = "Подтверждения бронирования";
                m.Body = "<html><body> <br><img src=\"http://prekrasnij-mir.ru/uploads/gallery_photos/36515057192961jpg1024x768.jpg\" alt =\"Super Game!\">" + @" 
                          <br>Здравствуйте уважаемый(я) " + toMailAddress + @" !
                          <br>Вы получили это письмо для активации брони мест на мероприятия на нашем сайте: http://localhost:59993/.
                          <br>Высылаем Вам секретную ссылку для активации вашей брони. 
                          <br>Без активации бронь не будет учтена!!!
                          <br>                                                                                              
                          <br>Перейдите по ссылке:       <b>" + callbackUrl + @"</b>
                          <br>
                          <br>Мы будем рады видеть Вас на нашем сайте и желаем Вам приятого времяпрепровождения!</body></html>";
                m.IsBodyHtml = true;
                smtp.Credentials = new NetworkCredential(fromMailAddress.Address, ";tyz24043670;tyz");
                smtp.EnableSsl = true;
                smtp.Send(m);
            }
        }
    }
}