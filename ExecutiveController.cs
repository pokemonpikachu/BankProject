using RetailBankingTeam5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Data.Entity.Core.Objects;

namespace RetailBankingTeam5.Controllers
{
    public class ExecutiveController : Controller
    {
        // GET: Executive
        DB05TMS155_1718Entities1 db = new DB05TMS155_1718Entities1();
        public ActionResult startpage()
        {
            return View();
        }

        public ActionResult Register()
        {
            bind();

            //    List<SelectListItem> dropdown = new List<SelectListItem>();
            return View("Register");
        }
        public void bind()
        {
            List<StateView_sp_Result> statelist = db.StateView_sp().ToList();
            List<SelectListItem> drplist1 = new List<SelectListItem>();
            //  TempData["stateid"] =
            foreach (StateView_sp_Result i in statelist)
            {
                drplist1.Add(new SelectListItem { Text = i.StateName.ToString(), Value = i.StateId.ToString() });



            }
            ViewBag.a = drplist1;
        }
        [HttpPost]
        public ActionResult Register(customerdetails_team c)
        {
            customerdetails_team id1 = db.customerdetails_team.Find(c.CustomerSSNId);


            if (ModelState.IsValid)
            {
                if (id1 == null)
                {
                    ObjectParameter o = new ObjectParameter("id", 0);
                    db.custreg(o, c.CustomerSSNId, c.CustomerName, c.CustomerAge, c.AddressLine1, c.AddressLine2, c.State, c.city);
                    if (Convert.ToInt64(o.Value) == 0)
                    {
                        Response.Write("<script>alert('Customer creation failed')</script>");
                    }
                    else
                    {
                        Response.Write("<script>alert('Customer creation initiated successfully')</script>");
                    }
                    db.SaveChanges();
                    ModelState.Clear();
                    int id = Convert.ToInt32(db.getstateid(c.State));
                    TempData["stateid"] = id;
                }
                else
                {
                    Response.Write("<script>alert('Already existing Customer ssn id')</script>");
                }
            }


            bind();

            //    List<SelectListItem> dropdown = new List<SelectListItem>();
            return View("Register");
        }
        [HttpGet]
        public JsonResult Getcity(int state)
        {
            DB05TMS155_1718Entities1 db = new DB05TMS155_1718Entities1();
            List<citylist_Result> cityddl = new List<citylist_Result>();
            cityddl = db.citylist(state).ToList();
            List<SelectListItem> citylist = new List<SelectListItem>();
            foreach (citylist_Result i in cityddl)
            {
                citylist.Add(new SelectListItem { Text = i.cityname.ToString(), Value = i.cityname.ToString() });
            }
            return Json(citylist, JsonRequestBehavior.AllowGet);
        }



     
        //update

        public ActionResult update1()
        {
            return View();
        }
        [HttpPost]


        public ActionResult update1(FormCollection n, customerdetails_team a)
        {
            customerdetails_team t = db.customerdetails_team.Where(x => x.CustomerId == a.CustomerId).FirstOrDefault();
            if (t == null)
            {
                Response.Write("<script>alert('Invalid Customer_ID Id')</script>)");
                return View();
            }
            else
            {
                TempData["cid"] = t.CustomerId;
                return RedirectToAction("updatecus", new { id = 0 });
            }

        }

        public ActionResult updatecus(customerdetails_team q)
        {
            customerdetails_team c = new customerdetails_team();
            DB05TMS155_1718Entities1 mn = new DB05TMS155_1718Entities1();


            c = mn.customerdetails_team.Find(Convert.ToInt32(TempData["cid"]));
            TempData["cnid"] = c.CustomerId;


            return View(c);
        }
        [HttpPost]
        public ActionResult updatecus(customerdetails_team q, FormCollection n)
        {
            try
            {
                DB05TMS155_1718Entities1 z = new DB05TMS155_1718Entities1();
          
                customerdetails_team a = z.customerdetails_team.Find(Convert.ToInt32(TempData["cnid"]));
                string s1 = n["newcname"];
                string s2 = n["newaddress1"];
                string s3 = n["newaddress2"];
                string s4 = n["newage"];
                if (s1.Length==0)
                {
                    n["newcname"] = a.CustomerName;
                }
                if (s2.Length == 0)
                {
                    n["newaddress1"] = a.AddressLine1;
                }
                if (s3.Length == 0)
                {
                    n["newaddress2"] = a.AddressLine2;
                }
                if (s4.Length == 0)
                {
                    n["newage"] = a.CustomerAge.ToString();
                }
                if (a.stats == "inactive")
                {
                    TempData["AlertMessage"] = 23;
                    return RedirectToAction("viewall");

                }
                else
                {
                    string a1 = n["newcname"];
                    string a2 = n["newaddress1"];
                    string a3 = n["newaddress2"];
                    int a4 = Convert.ToInt32(n["newage"]);

                    z.team5ud(Convert.ToInt32(TempData["cnid"]), a1, a4, a2, a3);

                    TempData["AlertMessage"] = 24;
                    return RedirectToAction("viewall");
                }
            }
            catch(Exception)
            {
                TempData["alert"] = 15;
                return RedirectToAction("update1");
            }



        }
        //view

        public ActionResult viewall(customerdetails_team c)
        {

            DB05TMS155_1718Entities1 db = new DB05TMS155_1718Entities1();
            List<customerdetails_team> s1 = new List<customerdetails_team>();
            s1 = db.customerdetails_team.ToList();
            db.SaveChanges();


            return View(s1);

        }

        public ActionResult viewstatus()
        {

            DB05TMS155_1718Entities1 b = new DB05TMS155_1718Entities1();
            List<customerdetails_team> s2 = new List<customerdetails_team>();
            s2 = b.customerdetails_team.ToList();
            b.SaveChanges();


            return View(s2);

        }
        [HttpPost]
        public ActionResult viewall(FormCollection F)
        {
            DB05TMS155_1718Entities1 db = new DB05TMS155_1718Entities1();
            string searchby = F["search by"];
            string searchvalue = F["SearchValue"];
            int search = Convert.ToInt32(searchvalue);

            customerdetails_team tem = db.customerdetails_team.Find(search);
            if (searchby == "CustomerId" || searchby == "CustomerSSNId")
            {
                if (tem != null)
                {
                    if (searchby == "CustomerId")
                    {


                        List<customerdetails_team> templist = db.customerdetails_team.Where(x => x.CustomerId == search).ToList();

                        return View("viewall", templist);
                    }
                    else
                    {
                        List<customerdetails_team> templist = db.customerdetails_team.Where(x => x.CustomerSSNId == (search)).ToList();

                        return View("viewall", templist);

                    }
                }
                else
                {
                    Response.Write("<script>alert('Please enter valid customerid')</script>");
                    List<customerdetails_team> s1 = new List<customerdetails_team>();
                    s1 = db.customerdetails_team.ToList();
                    db.SaveChanges();

                    return View(s1);
                }
            }
            else
            {
                Response.Write("<script>alert('Choose search by radio button')</script>");
                List<customerdetails_team> s1 = new List<customerdetails_team>();
                s1 = db.customerdetails_team.ToList();
                db.SaveChanges();

                return View(s1);
            }

        }



        public ActionResult Account_Search(FormCollection F)
        {
            {
                DB05TMS155_1718Entities1 db = new DB05TMS155_1718Entities1();
                string searchby = F["search by"];
                string search1 = F["Search1"];
                int searchvalue1 = 0;
                if (search1 != "")
                {
                    searchvalue1 = Convert.ToInt32(search1);
                }
                int searchvalue2 = 0;


                string search2 = F["Search2"];
                if (search2 != "")
                {
                    searchvalue2 = Convert.ToInt32(search2);
                }


                if (searchby == "Customer_ID" || searchby == "Account_ID")
                {



                    if (searchby == "Customer_ID")
                    {

                        string accounttype = F["Account_Type"];
                        if (accounttype == "Current Account" || accounttype == "Savings Account")
                        {
                            // List<Account> tem1 = new List<Account>();
                            // tem1 = db.Accounts.Where(a => a.Customer_ID == searchvalue1).ToList();
                            ObjectParameter o = new ObjectParameter("id", 0);
                            db.accountsearch(searchvalue1, accounttype, o);
                            db.SaveChanges();
                            int tem1 = Convert.ToInt32(o.Value);
                            if (tem1 == 1)
                            {
                                //  string accounttype = F["Customer_ID"];
                                List<Account> templist = new List<Account>();
                                templist = db.Accounts.Where(x => x.Customer_ID == searchvalue1 && x.Account_type == accounttype).ToList();

                                return View("ViewAccountexecutive", templist);
                            }

                            return View("ViewAccountexecutive");
                        }
                        else
                        {
                            Response.Write("<script>alert('select account type')</script>");
                            List<Account> lst = new List<Account>();
                            lst = db.Accounts.ToList();
                            ViewBag.list = lst;
                            return View("ViewAccountexecutive", lst);
                        }
                    }
                    else
                    {


                        Account tem2 = db.Accounts.Find(searchvalue2);
                        if (tem2 != null)
                        {
                            List<Account> templist = new List<Account>();
                            templist = db.Accounts.Where(x => x.Account_ID == searchvalue2).ToList();
                            return View("ViewAccountexecutive", templist);
                        }
                        else
                        {
                            Response.Write("<script>alert('Invalid account id')</script>");
                            List<Account> lst = new List<Account>();
                            lst = db.Accounts.ToList();
                            ViewBag.list = lst;
                            return View("ViewAccountexecutive", lst);
                        }
                    }



                }


                else
                {
                    Response.Write("<script>alert('Choose search by radio button')</script>");
                    List<Account> lst = new List<Account>();
                    lst = db.Accounts.ToList();
                    ViewBag.list = lst;
                    return View("ViewAccountexecutive", lst);

                }

            }


        }




        public ActionResult ViewAccountexecutive()
        {
            List<Account> lst = new List<Account>();
            lst = db.Accounts.ToList();
            ViewBag.list = lst;
            return View(lst);
        }
        public ActionResult Deleteaccount()
        {


            return View();


        }
        [HttpPost]
        public ActionResult Deleteaccount(Account a)

        {
            Account t = db.Accounts.Where(x => x.Customer_ID == a.Customer_ID && x.Account_type == a.Account_type).FirstOrDefault();
            if (t == null)
            {
                Response.Write("<script>alert('Invalid Customer_ID Id or Account type')</script>)");
                return View();
            }
            else
            {
                TempData["actid"] = t.Account_ID;
                return RedirectToAction("Deleteaccount1", new { id = 0 });
            }

        }

        public ActionResult Deleteaccount1(int id)
        {
            if (id == 0)
            {
                DB05TMS155_1718Entities1 df = new DB05TMS155_1718Entities1();
                Account c = new Account();
                c = df.Accounts.Find(TempData["actid"]);
                TempData["accountid"] = c.Account_ID;
                return View(c);
            }
            else
            {
                TempData["actid"] = id;
                DB05TMS155_1718Entities1 df = new DB05TMS155_1718Entities1();
                Account c = new Account();
                c = df.Accounts.Find(id);
                TempData["accountid"] = c.Account_ID;
                return View(c);

            }
        }

        [HttpPost]
        public ActionResult Deleteaccount1(Account a)
        {

            DB05TMS155_1718Entities1 bd = new DB05TMS155_1718Entities1();
            ObjectParameter o = new ObjectParameter("succes", 0);
            Account t = db.Accounts.Find(Convert.ToInt32(TempData["accountid"]));
            if (t.accountstatus == "Inactive")
            {
                TempData["AlertMessage"] = 20;
                return RedirectToAction("ViewAccountexecutive");
            }
            else

            {
                int s1 = Convert.ToInt32(TempData["accountid"]);
                db.accountdelete5(s1, o);


                if (Convert.ToInt32(o.Value) == 0)
                {
                    TempData["AlertMessage"] = 7;

                    return RedirectToAction("ViewAccountexecutive");

                }
                else
                {
                    TempData["AlertMessage"] = 8;

                    return RedirectToAction("ViewAccountexecutive");

                }
            }

        }



        public ActionResult delete()
        {
            return View();
        }
        [HttpPost]


        public ActionResult delete(FormCollection n, customerdetails_team a)
        {
            customerdetails_team t = db.customerdetails_team.Where(x => x.CustomerId == a.CustomerId).FirstOrDefault();
            if (t == null)
            {
                Response.Write("<script>alert('Invalid Customer_ID Id')</script>)");
                return View();
            }
            else
            {
                TempData["cid"] = t.CustomerId;
                return RedirectToAction("cdelete", new { id = 0 });
            }
        }
        public ActionResult cdelete(customerdetails_team m)
        {
            customerdetails_team d = new customerdetails_team();
            DB05TMS155_1718Entities1 mn = new DB05TMS155_1718Entities1();

            d = mn.customerdetails_team.Find(Convert.ToInt32(TempData["cid"]));
            TempData["cnid"] = d.CustomerId;
            return View(d);
        }
        [HttpPost]
        public ActionResult cdelete(customerdetails_team n, FormCollection a)
        {

            DB05TMS155_1718Entities1 z = new DB05TMS155_1718Entities1();
            customerdetails_team s = z.customerdetails_team.Find(Convert.ToInt32(TempData["cnid"]));
            if (s.stats == "inactive")
            {
                TempData["AlertMessage"] = 23;
                return RedirectToAction("viewall");

            }
            else
            {
                z.team5delete(Convert.ToInt32(TempData["cnid"]));

                TempData["AlertMessage"] = 30;
                return RedirectToAction("viewstatus");
            }
        }
    }
}
      





