using Microsoft.AspNetCore.Mvc;
using ShalehPrpject.Models;
using ShalehPrpject.ViewModel;

namespace ShalehPrpject.Controllers
{
    public class AdminController : Controller
    {
        private readonly Context _context;

        public AdminController(Context context)
        {
            _context = context;
        }

        public IActionResult Index()//Admin/Index
        {
            List<Customer> customers = _context.Customers.ToList();
            return View(customers);
        }

        public IActionResult ShowContract(int id)
        {
            Customer? customer = _context.Set<Customer>()
                .FirstOrDefault(Customer => Customer.Id == id);
            return View(customer);
        }
        public IActionResult AllOffers()//Admin/AllOffers
        {
            List<Offer> offers = _context.Offers.ToList();
            return View(offers);
        }
        public IActionResult Reservations()//Admin/AllOffers
        {
            var offers = _context.Bookings.ToList();
            return View(offers);
        }

        public IActionResult MakeFree(int id)
        {
            Booking booking = _context.Bookings.FirstOrDefault(b => b.Id == id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                _context.SaveChanges();
            }
            return RedirectToAction("Reservations");
        }

        [HttpGet]
        //public IActionResult AddOffer()
        //{

        //    return View();
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOffer(Offer model)
        {
            if (ModelState.IsValid)
            {

                Offer offer = new Offer()
                {
                    Type = model.Type,
                    PriceOfTheDayInOffer = model.PriceOfTheDayInOffer

                };
                _context.Add(offer);

                _context.SaveChanges();

            }
            return RedirectToAction(nameof(AllOffers));

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Offer model)
        {
            if (ModelState.IsValid)
            {
                _context.Update(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(AllOffers));
            }
            return View(model);
        }
        public async Task<IActionResult> GetOffer(int id)
        {
            var offer = await _context.Offers.FindAsync(id);
            if (offer == null)
            {
                return NotFound();
            }
            return Json(offer);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var offer = await _context.Offers.FindAsync(id);
            if (offer == null)
            {
                return NotFound();
            }

            _context.Offers.Remove(offer);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(AllOffers));
        }

        [HttpPost]
        [Route("Admin/AddDiscount")]
        public IActionResult AddDiscount([FromBody] Discount discount)
        {
            if (discount == null)
            {
                return Json(new
                {
                    Status = false,
                    message = "Invalid data",
                    receivedData = discount
                });
            }

            _context.Discounts.Add(discount);
            _context.SaveChanges();

            return Json(new
            {
                Status = true,
                message = "Data received and saved successfully",
                receivedData = discount
            });
        }

        [HttpPost]
        [Route("Admin/AddPrice")]
        public JsonResult AddPrice([FromBody] PriceData data)
        {

            if (data == null)
            {
                return Json(new { success = false, message = "Invalid data" });
            }

            try
            {
                DateTime.TryParse(data.StartDate, out DateTime startDate);
                DateTime.TryParse(data.EndDate, out DateTime endDate);
                List<DateTime> dateTimes = GetDatesBetween(startDate, endDate);
                SavePriceData(dateTimes, data.Value);
                return Json(new { success = true, message = "Data submitted successfully" });
            }
            catch (Exception ex)
            {
                // Log the exception
                return Json(new { success = false, message = ex.Message });
            }
        }
        public JsonResult Maintenance([FromBody] PriceData data)
        {

            if (data == null)
            {
                return Json(new { success = false, message = "Invalid data" });
            }

            try
            {
                DateTime.TryParse(data.StartDate, out DateTime startDate);
                DateTime.TryParse(data.EndDate, out DateTime endDate);
                List<DateTime> dateTimes = GetDatesBetween(startDate, endDate);
                SaveData(dateTimes, "صيانه", "المالك");
                return Json(new { success = true, message = "Data submitted successfully" });
            }
            catch (Exception ex)
            {
                // Log the exception
                return Json(new { success = false, message = ex.Message });
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

        private void SavePriceData(List<DateTime> dateTimes, float value)
        {
            var newPrices = dateTimes.Select(date => new NewPrice
            {
                Day = date.Day,
                Month = date.Month,
                Year = date.Year,
                Value = value
            }).ToList();
            foreach (var price in newPrices)
            {
                var result = _context.Prices.FirstOrDefault(b => b.Year == price.Year && b.Month == price.Month && b.Day == price.Day);
                if (result == null)
                {
                    _context.Prices.Add(price);
                }
                else
                {
                    result.Value = price.Value;
                    _context.Prices.Update(result);
                }
            }
            _context.SaveChanges();
        }
        private void SaveData(List<DateTime> dateTimes, string Type)
        {
            var newBooking = dateTimes.Select(date => new Booking
            {
                Day = date.Day,
                Month = date.Month,
                Year = date.Year,
                Type = Type
            }).ToList();
            _context.Bookings.AddRange(newBooking);
            _context.SaveChanges();
        }

        [HttpGet]
        public IActionResult login() //Flora/Offers
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string userName, string password)
        {
            string currentUserName = "admin";
            string currentUserPassword = "admin123456";

            // Check if username and password are provided
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
            {
                // Check if the provided credentials match the admin credentials
                if (userName == currentUserName && password == currentUserPassword)
                {
                    // Redirect to the appropriate action if the credentials are correct
                    // For example, redirect to the Offers action
                    return RedirectToAction("index", "admin");
                }
                else
                {
                    // Credentials are incorrect, you may want to add an error message
                    ModelState.AddModelError(string.Empty, "اسم المستخدم او كلمه السر خطأ");
                }
            }
            else
            {
                // Username or password is missing, you may want to add an error message
                ModelState.AddModelError(string.Empty, "ادخل جميع الحقول");
            }
            // Return the login view with any validation errors
            return View();
        }
        // In your controller
        [HttpGet]

        public IActionResult CheckAvailability(DateTime date)
        {
            // Extract day, month, and year from the provided date
            int day = date.Day;
            int month = date.Month;
            int year = date.Year;

            // Check if any booking exists for the provided day, month, and year
            bool isBooked = _context.Bookings.Any(b => b.Day == day && b.Month == month && b.Year == year);

            // Return JSON response indicating whether the date is booked or not
            return Json(new { booked = isBooked });
        }

        [HttpPost]
        [Route("Admin/CalculateTotalPrice")]
        public IActionResult CalculateTotalPrice([FromBody] TotalPrice date)
        {
            float total = 0;

            DateTime.TryParse(date.arriveDate, out DateTime startDate);
            DateTime.TryParse(date.leaveDate, out DateTime endDate);
            List<DateTime> dateTimes = GetDatesBetween(startDate, endDate);
            foreach (DateTime dat in dateTimes)
            {
                int day = dat.Day;
                int month = dat.Month;
                int year = dat.Year;

                var result = _context.Prices.FirstOrDefault(b => b.Day == day && b.Month == month && b.Year == year);
                if (result != null)
                {
                    total += result.Value;
                }
                else
                {
                    total += 1200;
                }
            }

            return Json(new { totalPrice = total, status = true });
        }


    }
}
