using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RetailBankingTeam5.Models;
using System.Data.Entity.Core.Objects;

namespace RetailBankingTeam5.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        DB05TMS155_1718Entities1 db = new DB05TMS155_1718Entities1();

        public ActionResult startpage()
        {
            return View();
        }

        public ActionResult accountregister()
        {
            return View();
        }
        [HttpPost]
        public ActionResult accountregister(Account a)
        {
            if (ModelState.IsValid)
            {
                ObjectParameter o = new ObjectParameter("id", 0);
                db.checkaccount(a.Customer_ID, o);
                if (Convert.ToInt32(o.Value) == 0)
                {
                    Response.Write("<script>alert('Invalid Customer Id')</script>)");
                    return View();
                }
                else
                {
                    DB05TMS155_1718Entities1 bd = new DB05TMS155_1718Entities1();
                    ObjectParameter r = new ObjectParameter("id", 0);
                    db.account_reg1(r, a.Customer_ID, a.Account_type, a.Deposit_Amount);
                    if (Convert.ToInt32(r.Value) == 0)
                    {
                        Response.Write("<script>alert('This ID already holds" + a.Account_type + "')</script>)");

                    }
                    else
                    {
                        Response.Write("<script>alert('Account created successfully with ID" + Convert.ToInt32(r.Value) + "')</script>)");
                    }
                    db.SaveChanges();
                    ModelState.Clear();
                }
            }
            return View();
        }







        public ActionResult ViewAccount()
        {
            List<Account> lst = new List<Account>();
            lst = db.Accounts.ToList();
            ViewBag.list = lst;
            return View(lst);
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

                                return View("ViewAccount", templist);
                            }
                           
                            return View("ViewAccount");
                        }
                        else
                        {
                            Response.Write("<script>alert('select account type')</script>");
                            List<Account> lst = new List<Account>();
                            lst = db.Accounts.ToList();
                            ViewBag.list = lst;
                            return View("ViewAccount", lst);
                        }
                    }
                    else
                    {


                        Account tem2 = db.Accounts.Find(searchvalue2);
                        if (tem2 != null)
                        {
                            List<Account> templist = new List<Account>();
                            templist = db.Accounts.Where(x => x.Account_ID == searchvalue2).ToList();
                            return View("ViewAccount", templist);
                }
                else
                {
                            Response.Write("<script>alert('Invalid account id')</script>");
                            List<Account> lst = new List<Account>();
                            lst = db.Accounts.ToList();
                            ViewBag.list = lst;
                            return View("ViewAccount", lst);
                        }
                    }



                }


                else
                {
                    Response.Write("<script>alert('Choose search by radio button')</script>");
                    List<Account> lst = new List<Account>();
                    lst = db.Accounts.ToList();
                    ViewBag.list = lst;
                    return View("ViewAccount", lst);

                }

            }


        }



        public ActionResult Depositmoney1()
        {

            return View();

        }
      
        [HttpPost]
        public ActionResult Depositmoney1(Account a)
        {
            Account t = db.Accounts.Where(x => x.Customer_ID == a.Customer_ID && x.Account_type == a.Account_type).FirstOrDefault();
            if (t == null)
            {
                Response.Write("<script>alert('Invalid Customer_ID Id or Account type')</script>)");
                return View();
            }
            else
            {
                TempData["aid"] = t.Account_ID;
                return RedirectToAction("Depositmoney", new { id = 0 });
            }



        }



        public ActionResult Depositmoney(int id)
        {
            
                if (id == 0)
                {
                    DB05TMS155_1718Entities1 db = new DB05TMS155_1718Entities1();
            Account a = new Account();
                    a = db.Accounts.Find(Convert.ToInt32(TempData["aid"]));
                TempData["accountid"] = a.Account_ID;
                    return View(a);
                }
                else
                {
                TempData["aid"] = id;
                    Account a = new Account();
            a = db.Accounts.Find(id);
            return View(a);
                } 

        }
        [HttpPost]
        public ActionResult Depositmoney(FormCollection f, Account c)
        {
            try
            {


                ObjectParameter o = new ObjectParameter("flag", 0);
                int amount = Convert.ToInt32(f["depositmoney"]);
                int s = Convert.ToInt32(TempData["accountid"]);
                db.amountdeposit5(s, amount, o);
                if (Convert.ToInt32(o.Value) == 0)
                {
                    TempData["AlertMessage"] = 3;
                    return RedirectToAction("Depositmoney1");
                }
                else
                {
                    TempData["AlertMessage"] = 4;
                    return RedirectToAction("ViewAccount");
                }
            }
            catch(Exception)
            {
                TempData["alert"] = 36;
                return RedirectToAction("Depositmoney1");
            }


        }
           


        //Withdraw
        public ActionResult withdraw_1()
        {
            return View();

        }
        [HttpPost]
        public ActionResult withdraw_1(Account a)
        {

            Account t = db.Accounts.Where(x => x.Customer_ID == a.Customer_ID && x.Account_type == a.Account_type).FirstOrDefault();
            if (t == null)
            {
                Response.Write("<script>alert('Invalid Customer_ID Id or Account type')</script>)");
                return View();
            }
            else
            {
                TempData["aid"] = t.Account_ID;
                return RedirectToAction("withdraw", new { id = 0 });
            }
        }


        public ActionResult withdraw(int id)
        {
            if (id == 0)
            {
                DB05TMS155_1718Entities1 db = new DB05TMS155_1718Entities1();
            Account a = new Account();
                a = db.Accounts.Find(Convert.ToInt32(TempData["aid"]));
                TempData["accountid"] = a.Account_ID;
                return View(a);
            }
            else
            {
                TempData["id"] = id;
                Account a = new Account();
                DB05TMS155_1718Entities1 db = new DB05TMS155_1718Entities1();
            a = db.Accounts.Find(id);
                TempData["accountid"] = a.Account_ID;
            return View(a);

        }

        }

        [HttpPost]
        public ActionResult withdraw(FormCollection f, Account c)
        {
            try
            { 
            ObjectParameter o = new ObjectParameter("flag", 0);
            int amount = Convert.ToInt32(f["withdraw"]);
            Account t = db.Accounts.Find(Convert.ToInt32(TempData["accountid"]));
            if (t.accountstatus == "Inactive")
            {
                TempData["AlertMessage"] = 20;
                return RedirectToAction("ViewAccount");
            }
            else
            {
                db.withdraw_amount(Convert.ToInt32(TempData["accountid"]), amount, o);
            if (Convert.ToInt32(o.Value) == 0)
            {
                TempData["AlertMessage"] = 1;
                return RedirectToAction("withdraw_1");
            }
            else
            {
                TempData["AlertMessage"] = 2;
                return RedirectToAction("ViewAccount");
            }
            }
            }
            catch (Exception)
            {
                TempData["alert"] = 37;
                return RedirectToAction("withdraw_1");
            }

        }

        public ActionResult Viewtransactions()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Viewtransactions1(transaction t, FormCollection f)
        {
            Account a = db.Accounts.Find(t.Account_ID);
            if (a == null)
            {
                TempData["alert"] = 5;
                return RedirectToAction("Viewtransactions");
            }
            else
            {
                if (f["searchcriteria"] == null)
                {
                    TempData["alert"] = 1;
                    return RedirectToAction("Viewtransactions");

                }
                else
                {


                    List<viewtransc_1_Result> lst_1 = new List<viewtransc_1_Result>();
                    if (f["searchcriteria"] == "Number of transactions")
                    {
                        lst_1 = db.view_trans1(t.Account_ID, Convert.ToInt32(f["nooftrans"])).ToList();

                        return View(lst_1);
                    }
                    else
                    {
                        TempData["accid"] = t.Account_ID;
                        TempData["startdate"] = f["startdate"];
                        TempData["enddate"] = f["enddate"];
                        return RedirectToAction("Viewtransaction2");
                    }

                }
            }
        }


        public ActionResult Viewtransaction2()
        {
            try
            {
                if (TempData["startdate"] != null && TempData["enddate"] != null)
                {
                    DateTime d1 = Convert.ToDateTime(TempData["startdate"]);
                    DateTime d2 = Convert.ToDateTime(TempData["enddate"]);
                    if (d2 < d1)
                    {
                        TempData["alert"] = 6;
                        return RedirectToAction("Viewtransactions");
                    }
                    else
                    {
                        List<viewtransc_2_Result> lst_2 = new List<viewtransc_2_Result>();
                        lst_2 = db.viewtransc_2(Convert.ToInt32(TempData["accid"]), Convert.ToDateTime(TempData["startdate"]), Convert.ToDateTime(TempData["enddate"])).ToList();
                        return View(lst_2);
                    }
                }

                else
                {
                    TempData["alert"] = 2;
                    return RedirectToAction("Viewtransactions");
                }
            }
            catch (Exception ex)
            {

                TempData["alert"] = 3;
                return RedirectToAction("Viewtransactions");
            }

        }


        public ActionResult transfer()
        {
      
            return View();
        }

        [HttpPost]
        public ActionResult transfer(Account c, FormCollection f)
        {
            try
            { 
            DB05TMS155_1718Entities1 db = new DB05TMS155_1718Entities1();
            TempData["act"] = f["act"];
            TempData["trt"] = f["trt"];
            ObjectParameter r = new ObjectParameter("id", 0);
            db.check123(Convert.ToInt32(f["act"]), Convert.ToInt32(f["trt"]), r);

            if (Convert.ToInt32(r.Value) == 0)
            {
                Response.Write("<script>alert('Invalid account Id ')</script>)");
                return View();
            }
            else
            {

                long a1 = Convert.ToInt64(f["act"]);
                long a2 = Convert.ToInt64(f["trt"]);
                int a3 = Convert.ToInt32(f["transamount"]);
                DB05TMS155_1718Entities1 m = new DB05TMS155_1718Entities1();
                ObjectParameter n = new ObjectParameter("flag", 0);
                ObjectParameter k = new ObjectParameter("temp1", 0);
                ObjectParameter v = new ObjectParameter("temp2", 0);
                m.transq(a1, a2, a3, n,k,v);

                if (a1 == a2)
                {
                    TempData["AlertMessage"] = 33;
                    return RedirectToAction("transfer");
                }
                else
                {

                    if (Convert.ToInt32(k.Value) > 0)
                    {
                        if (Convert.ToInt32(v.Value) > 0)
                        {
                            if (Convert.ToInt32(n.Value) > 0)
                            {
                                m.SaveChanges();
                                TempData["AlertMessage"] = 22;
                                ModelState.Clear();
                                return RedirectToAction("ViewAccount");

                            }
                            else
                            {
                                TempData["AlertMessage"] = 21;
                                return RedirectToAction("transfer");

                            }
                        }
                        else
                        {

                            TempData["AlertMessage"] = 32;
                            return RedirectToAction("transfer");

                        }
                    }
                    else
                    {

                        TempData["AlertMessage"] = 31;
                        return RedirectToAction("transfer");

                    }
                        }
                    }



            }
            catch (Exception)
            {
                TempData["alert"] = 38;
                return RedirectToAction("transfer");
            }



        }  
                 
        }
    



    }
