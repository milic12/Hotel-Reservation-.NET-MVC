using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;
using HotelManagement.Models;
using HotelManagement.ViewModel;

namespace HotelManagement.Controllers
{
    public class RoomController : Controller
    {

        private HotelDBEntities1 objHotelDbEntities;
        public RoomController()
        {
            objHotelDbEntities = new HotelDBEntities1();


        }
        public ActionResult Index()
        {
            RoomViewModel objRoomViewModel = new RoomViewModel();
            objRoomViewModel.ListOfBookingStatus = (from obj in objHotelDbEntities.BookingStatus
                                                    select new SelectListItem()
                                                    {
                                                        Text = obj.BookingStatus,
                                                        Value = obj.BookingStatusId.ToString()
                                                    }).ToList();

            objRoomViewModel.ListOfRoomType = (from obj in objHotelDbEntities.RoomTypes
                                               select new SelectListItem()
                                               {
                                                   Text = obj.RoomTypeName,
                                                   Value = obj.RoomTypeId.ToString()
                                               }).ToList();
            return View(objRoomViewModel);
        }
        [HttpPost]
        public ActionResult Index(RoomViewModel objRoomViewModel)
        {
            string ImageUniqueName = Guid.NewGuid().ToString();
            string ActualImageName = ImageUniqueName + Path.GetExtension(objRoomViewModel.Image.FileName);
            //Spremanje slike u folder i bazu
            objRoomViewModel.Image.SaveAs(filename: Server.MapPath("~/RoomImages/" + ActualImageName));

            //Spremanje u Bazu podtaka HotelDBEntities
            Room objRoom = new Room()
            {
                RoomNumber = objRoomViewModel.RoomNumber,
                RoomDescription = objRoomViewModel.RoomDescription,
                RoomPrice = objRoomViewModel.RoomPrice,
                BookStatusId = objRoomViewModel.BookingStatusId,
                IsActive = true,
                RoomImage = ActualImageName,
                RoomCapacity = objRoomViewModel.RoomCapacity,
                RoomTypeID = objRoomViewModel.RoomTypeId

            };
            objHotelDbEntities.Rooms.Add(objRoom);
            objHotelDbEntities.SaveChanges();

            return Json(data:new { message = "Room Successfully Added." , success = true }, JsonRequestBehavior.AllowGet);

        }
        public PartialViewResult GetAllRooms()
        {
            IEnumerable<RoomDetailViewModel> ListOfRoomDetailsViewModels =
                (from objRoom in objHotelDbEntities.Rooms
                 join objBooking in objHotelDbEntities.BookingStatus on objRoom.BookStatusId equals objBooking.BookingStatusId
                 join objRoomType in objHotelDbEntities.RoomTypes on objRoom.RoomTypeID equals objRoomType.RoomTypeId
                 select new RoomDetailViewModel()
                 {
                     RoomNumber = objRoom.RoomNumber,
                     RoomDescription = objRoom.RoomDescription,
                     RoomCapacity = objRoom.RoomCapacity,
                     RoomPrice = objRoom.RoomPrice,
                     BookingStatus = objBooking.BookingStatus,
                     RoomType = objRoomType.RoomTypeName,
                     RoomImage = objRoom.RoomImage,
                     RoomId = objRoom.RoomId
                 }).ToList();
            return PartialView("_RoomDetailsPartial", ListOfRoomDetailsViewModels);
        }

    }
}
