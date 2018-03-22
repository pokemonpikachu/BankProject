using RetailBankingTeam5.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;



namespace RetailBankingTeam5.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        DB05TMS155_1718Entities1 db = new DB05TMS155_1718Entities1();
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(login_tab usermodel)
        {
            using (DB05TMS155_1718Entities1 db = new DB05TMS155_1718Entities1())
                {
                if (ModelState.IsValid)
                {

                    var userdetails = db.login_tab.Where(x => x.userid == usermodel.userid && x.passwrd == usermodel.passwrd).FirstOrDefault();
                    if (userdetails == null)
                    {
                        Response.Write("<script>alert('Please enter valid credentials')</script>");
                        return View("Login");
                    }
                    else
                    {
                        ObjectParameter o = new ObjectParameter("userrole", 0);
                        db.checkrole(usermodel.userid, usermodel.passwrd, o);

                        if (o.Value.ToString() == "CAE")
                        {
                            db.login(usermodel.userid, usermodel.passwrd);
                            Session["userID"] = userdetails.userid;
                            return RedirectToAction("startpage", "Executive");
                        }
                        else
                        {
                            db.login(usermodel.userid, usermodel.passwrd);
                            Session["userID"] = userdetails.userid;
                            return RedirectToAction("startpage", "Account");
                        }
                   
                    }
                    
                  }
                }
            return View();

        }

    }
}
