using Dapper;
using EngJobMgmt.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace EngJobMgmt.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Login login)
        {
            if (ModelState.IsValid)
            {
                bool success = WebSecurity.Login(login.username, login.password, false);
                if (success)
                {
                    var role = Roles.GetRolesForUser(login.username);
                    string returnUrl = Request.QueryString["ReturnUrl"];
                    if (returnUrl == null)
                    {
                        if (role[0]== "Admin")
                        {
                            Response.Redirect("~/Home/index");
                        }
                        else if (role[0] == "Engineer")
                        {
                            Response.Redirect("~/Home/About");
                        }
                        else if (role[0] == "Admin")
                        {
                            Response.Redirect("~/Home/index");
                        }
                        else
                        {
                            Response.Redirect("~/Home/index");
                        }
                    }
                    else
                    {
                        Response.Redirect(returnUrl);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("Error", "Please enter Username and Password");
            }
            return View(login);

        }


        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Register(Register register)
        {
            if (ModelState.IsValid)
            {
                if (!WebSecurity.UserExists(register.username))
                {
                    WebSecurity.CreateUserAndAccount(register.username, register.password
                       );
                    Response.Redirect("~/account/login");
                }
            }
            else
            {
                ModelState.AddModelError("Error", "Please enter all details");
            }
            return View();
        }

        [HttpGet]
        public ActionResult RoleCreate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RoleCreate(Role role)
        {
            if (ModelState.IsValid)
            {
                if (Roles.RoleExists(role.RoleName))
                {
                    ModelState.AddModelError("Error", "Rolename already exists");
                    return View(role);
                }
                else
                {
                    Roles.CreateRole(role.RoleName);
                    return RedirectToAction("RoleIndex", "Account");
                }
            }
            else
            {
                ModelState.AddModelError("Error", "Please enter Username and Password");
            }
            return View(role);
        }

        public ActionResult RoleIndex()
        {
            var roles = Roles.GetAllRoles();
            return View(roles);
        }

        [HttpGet]
        public ActionResult RoleAddToUser()
        {
            AssignRoleVM objvm = new AssignRoleVM();

            List<SelectListItem> listrole = new List<SelectListItem>(); //list 1  

            listrole.Add(new SelectListItem { Text = "Select", Value = "0" });

            foreach (var item in Roles.GetAllRoles())
            {
                listrole.Add(new SelectListItem { Text = item, Value = item });
            };

            objvm.RolesList = listrole;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnection1"].ToString()))
            {
                var Userlist = con.Query("SELECT * FROM Users").ToList();

                List<SelectListItem> listuser = new List<SelectListItem>(); //list 2  

                listuser.Add(new SelectListItem { Text = "Select", Value = "0" });

                foreach (var item in Userlist)
                {
                    listuser.Add(new SelectListItem { Text = item.UserName, Value = item.UserName });
                }

                objvm.Userlist = listuser;
            }

            return View(objvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RoleAddToUser(AssignRoleVM objvm)
        {

            if (objvm.RoleName == "0")
            {
                ModelState.AddModelError("RoleName", "Please select RoleName");
            }

            if (objvm.UserName == "0")
            {
                ModelState.AddModelError("UserName", "Please select Username");
            }

            if (ModelState.IsValid)
            {

                if (Roles.IsUserInRole(objvm.UserName, objvm.RoleName))
                {
                    ViewBag.ResultMessage = "This user already has the role specified !";
                }
                else
                {
                    Roles.AddUserToRole(objvm.UserName, objvm.RoleName);

                    ViewBag.ResultMessage = "Username added to the role successfully !";
                }


                List<SelectListItem> lirole = new List<SelectListItem>();
                lirole.Add(new SelectListItem { Text = "Select", Value = "0" });

                foreach (var item in Roles.GetAllRoles())
                {
                    lirole.Add(new SelectListItem { Text = item, Value = item });
                }

                objvm.RolesList = lirole;

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnection1"].ToString()))
                {
                    var Userlist = con.Query("SELECT * FROM Users").ToList();
                    List<SelectListItem> listuser = new List<SelectListItem>();
                    listuser.Add(new SelectListItem { Text = "Select", Value = "0" });

                    foreach (var item in Userlist)
                    {
                        listuser.Add(new SelectListItem { Text = item.UserName, Value = item.UserName });
                    }
                    objvm.Userlist = listuser;
                }

                return View(objvm);

            }

            else
            {
                List<SelectListItem> lirole = new List<SelectListItem>();
                lirole.Add(new SelectListItem { Text = "Select", Value = "0" });

                foreach (var item in Roles.GetAllRoles())
                {
                    lirole.Add(new SelectListItem { Text = item, Value = item });
                }

                objvm.RolesList = lirole;

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnection"].ToString()))
                {
                    var Userlist = con.Query("SELECT * FROM Users").ToList();
                    List<SelectListItem> listuser = new List<SelectListItem>();
                    listuser.Add(new SelectListItem { Text = "Select", Value = "0" });

                    foreach (var item in Userlist)
                    {
                        listuser.Add(new SelectListItem { Text = item.UserName, Value = item.UserName });
                    }

                    objvm.Userlist = listuser;
                }
                ModelState.AddModelError("Error", "Please enter Username and Password");
            }
            return View(objvm);
        }
    }
}