using Microsoft.AspNetCore.Mvc;

using ShalehPrpject.Models;




namespace ShalehPrpject.Controllers
{
    public class FloraController : Controller
    {
        private readonly Context _context;
        private readonly IWebHostEnvironment _hostingEnvironment;


        public FloraController(Context context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }


        public IActionResult Index() //Flora/Index
        {
            var today = DateTime.Today;
            var discount = _context.Discounts
                .FirstOrDefault(d => d.Day == today.Day && d.Month == today.Month && d.Year == today.Year);
            if (discount != null)
            {
                ViewBag.dis = discount.Value;
            }


            var latestPrice = _context.Prices
            .FirstOrDefault(d => d.Day == today.Day && d.Month == today.Month && d.Year == today.Year);
            double priceOfDay = 0;
            if (latestPrice != null)
            {
                priceOfDay = latestPrice.Value;
            }
            return View(priceOfDay);
        }
        public IActionResult Payment(double totalPrice, string arriveDate, string leaveDate)
        {
            ViewBag.TotalPrice = totalPrice;
            ViewBag.ArriveDate = arriveDate; // Pass along the arriveDate
            ViewBag.LeaveDate = leaveDate;   // Pass along the leaveDate
            return View();
        }

        [HttpPost]
        public IActionResult SubmitPayment(double totalPrice, int paymentValue, int resetValue, string arriveDate, string leaveDate)
        {
            // Redirect to Contract action and pass the parameters
            return RedirectToAction("Contract", new
            {
                amount = paymentValue,
                reset = resetValue,
                arriveDate = arriveDate,
                leaveDate = leaveDate,
                totalPrice2 = totalPrice
            });
        }


        public IActionResult Contract(double totalPrice2, int amount, int reset, string arriveDate, string leaveDate)
        {
            ViewBag.TotalPrice = totalPrice2;
            ViewBag.PaymentAmount = amount;
            ViewBag.Reset = reset;
            ViewBag.ShowModal = true;
            ViewBag.ArrivalDate = arriveDate;
            ViewBag.DepartureDate = leaveDate;
            TempData["arriveDate"] = arriveDate;
            TempData["leaveDate"] = leaveDate;
            return View();
        }

        [HttpPost]
        public IActionResult SubmitContract(string tenantName, string idNumber, string nationality,
                                    string tenantAddress, string phoneNumber)
        {
            try
            {
                // Save customer data to database
                Customer c = new Customer
                {
                    Name = tenantName,
                    Address = tenantAddress,
                    PhoneNumber = phoneNumber,
                    Nationality = nationality,
                    IdNumber = idNumber,
                };
                _context.Add(c);
                _context.SaveChanges();

                DateTime.TryParse(TempData["arriveDate"].ToString(), out DateTime startDate);
                DateTime.TryParse(TempData["leaveDate"].ToString(), out DateTime endDate);
                List<DateTime> dateTimes = GetDatesBetween(startDate, endDate);
                SaveData(dateTimes, "booking", tenantName);
                return RedirectToAction("index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Contract");
            }
        }

        private void SaveData(List<DateTime> dateTimes, string Type, string name)
        {
            var newBooking = dateTimes.Select(date => new Booking
            {
                Day = date.Day,
                Month = date.Month,
                Year = date.Year,
                Type = Type,
                Name = name
            }).ToList();
            _context.Bookings.AddRange(newBooking);
            _context.SaveChanges();
        }
        public List<DateTime> GetDatesBetween(DateTime startDate, DateTime endDate)
        {
            var dates = new List<DateTime>();
            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                dates.Add(date);
            }
            return dates;
        }

        public IActionResult Offers() //Flora/Offers
        {
            IEnumerable<Offer> offers = _context.Offers.ToList();
            return View(offers);
        }
        public IActionResult BuyOffer(int id) //Flora/Offers
        {
            Offer offer = _context.Offers.FirstOrDefault(o => o.Id == id);
            return View(offer.PriceOfTheDayInOffer);
        }





    }
}


















