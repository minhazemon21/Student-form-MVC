using Common.Service.StoredProcedure;
using ERP.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Accounts.Controllers
{
    public class StdInfoController : Controller
    {
        // GET: StdInfo
        private readonly ISPService spService;
        public StdInfoController(ISPService spService)
        {
            this.spService = spService;
        }
        string sessionName = "STR_VoucherDataTable" + SessionHelper.LoggedInUserId.ToString();
        StringBuilder sb = new StringBuilder();
        public ActionResult Index()
        {
            var DepartmentList = spService.GetDataWithoutParameter("USP_GET_Department_For_DropdownList").Tables[0];
            ViewBag.DepartmentList = DepartmentList;
            return View();
        }

        public JsonResult SaveStudent(int stdDeptId, int stdSubInfoId, string STudentName, string StudentContactNo, int Id = 0)
        {
            try
            {
                var data = spService.GetDataWithParameter(new { stdDeptId = stdDeptId, stdSubInfoId = stdSubInfoId, STudentName = STudentName, StudentContactNo = StudentContactNo, CreatedUserId = SessionHelper.LoggedInUserId, id = Id }, "USP_INSERT_StudentList").Tables[0].AsEnumerable().Select(r => new
                {
                    msg = r.Field<string>("Message")
                }).FirstOrDefault();


                return Json(new { Message = data.msg, Status = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Message = ex.InnerException.Message, Status = false }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult GetStudentDetails()
        {
            try
            
            {
                var stdList = spService.GetDataWithoutParameter("USP_GET_StdntList").Tables[0]
                    .AsEnumerable().Select(x => new
                    {
                        Id = x.Field<int>("Id"),
                        RowSl = x.Field<string>("RowSl"),
                        STudentName = x.Field<string>("STudentName"),
                        StudentContactNo = x.Field<string>("StudentContactNo"),
                        DeptName = x.Field<string>("DeptName"),
                        SubjectName = x.Field<string>("SubjectName"),
                        stdDeptId = x.Field<int>("stdDeptId"),
                        stdSubInfoId = x.Field<int>("stdSubInfoId"),
                        SubjectShortName = x.Field<string>("SubjectShortName")


                    }).ToList();
                return Json(stdList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, data = "", Message = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetChangeSubjectDropdown(int Id = 0)
        {
            try
            {
                var param = new { stdDeptId = Id };
                var subList = spService.GetDataWithParameter(param, "USP_GET_StdSubjectInfo_For_DropdownList");

                var bankAccListByOrg = subList.Tables[0].AsEnumerable()
                .Select(x => new
                {
                    Id = x.Field<int>("Id"),
                    SubjectShortName = x.Field<string>("SubjectShortName")
                }).ToList();

                return Json(new { status = true, data = bankAccListByOrg, message = "" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { status = false, data = "", message = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult DeleteStudent(int Id)
        {
            try
            {

                var data = spService.GetDataWithParameter(new { Id = Id }, "USP_DELETE_Student").Tables[0].AsEnumerable().Select(R => new
                {
                    meg = R.Field<string>("Message")
                }).FirstOrDefault();

                return Json(new { Message = data.meg, Status = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Message = ex.InnerException == null ? ex.Message : ex.InnerException.Message,
                    Status = false
                }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}