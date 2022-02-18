using FinalQuizProject.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FinalQuizProject.Controllers
{
    public class HomeController : Controller
    {

        QUIZ_DBEntities2 db = new QUIZ_DBEntities2();

        // GET: Admin

        #region Index
        public ActionResult Index()
        {
            return View();
        }
        #endregion
        
        #region Login
        public ActionResult slogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult slogin(TBL_STUDENT s)
        {
            if (ModelState.IsValid == true)
            {
                var data = db.TBL_STUDENT.Where(x => x.STD_NAME == s.STD_NAME && x.STD_PASSWORD == s.STD_PASSWORD).SingleOrDefault();
                if (data != null)
                {
                    Session["std_id"] = data.STD_ID;
                    return RedirectToAction("StudentExam");
                }
                else
                {
                    ViewBag.error = "Invalid Username or Password";
                }
            }
            return View();
        }

        public ActionResult tlogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult tlogin(TBL_ADMIN ad)
        {
            if (ModelState.IsValid == true)
            {
                var data = db.TBL_ADMIN.Where(x => x.AD_NAME == ad.AD_NAME && x.AD_PASSWORD == ad.AD_PASSWORD).SingleOrDefault();
                if (data != null)
                {
                    Session["ad_id"] = data.AD_ID;
                    return RedirectToAction("Dashboard");
                }
                else
                {
                    ViewBag.error = "Invalid Username or Password";
                }
            }
            return View();
        }

        #endregion

        #region Reister
        public ActionResult sregister()
        {
            return View();
        }

        [HttpPost]
        public ActionResult sregister(TBL_STUDENT s,HttpPostedFileBase ImgFile)
        {
            string filename = Path.GetFileNameWithoutExtension(ImgFile.FileName);
            string extension = Path.GetExtension(ImgFile.FileName);
            filename = filename + extension;
            HttpPostedFileBase PostedFile = ImgFile;
            int length = PostedFile.ContentLength;
            if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
            {
                if (length <= 1000000)
                {
                    s.STD_IMAGE = "~/images/" + filename;
                    string path = Server.MapPath("~/images/");
                    ImgFile.SaveAs(Path.Combine(path,ImgFile.FileName));
                    db.TBL_STUDENT.Add(s);
                    int a = db.SaveChanges();
                    if (a > 0)
                    {
                        return RedirectToAction("slogin");
                    }
                    else
                    {
                        ViewBag.error = "Register Failed";
                    }
                }
                else
                {
                    ViewBag.size = "<script>Image Should Be In 1 MB</script>";
                }
            }
            else
            {
                ViewBag.extension = "<script>Image Not Supported</script>";
            }
            return View();
        }

        #endregion

        #region Dashboard
        public ActionResult Dashboard()
        {
            return View();
        }
        #endregion

        #region Add Categories


        public ActionResult Addcategory()
        {
            int id = Convert.ToInt32(Session["ad_id"].ToString());
            List<TBL_CATEGORY> cat = db.TBL_CATEGORY.Where(x => x.CAT_FK_ADID == id).ToList();
            ViewBag.list = cat;
            var ad = db.TBL_ADMIN.Where(x => x.AD_ID == id).FirstOrDefault();
            ViewBag.admin_name = ad;
            return View();
        }

        [HttpPost]
        public ActionResult Addcategory(TBL_CATEGORY c)
        {
            int id = Convert.ToInt32(Session["ad_id"].ToString());
            List<TBL_CATEGORY> cat = db.TBL_CATEGORY.Where(x => x.CAT_FK_ADID == id).ToList();
            ViewBag.list = cat;
            var ad = db.TBL_ADMIN.Where(x => x.AD_ID == id).FirstOrDefault();
            ViewBag.admin_name = ad;
            if (ModelState.IsValid == true)
            {
                TBL_CATEGORY cate = new TBL_CATEGORY();
                cate.CAT_NAME = c.CAT_NAME;
                cate.CAT_FK_ADID = Convert.ToInt32(Session["ad_id"].ToString());
                Random r = new Random();
                cate.CAT_ENCRYPTEDSTRING = encrytop.Encrypt(cate.CAT_NAME.Trim() + r.Next().ToString(), true);
                db.TBL_CATEGORY.Add(cate);
                int a = db.SaveChanges();
                if (a > 0)
                {
                    ViewBag.add = "Add Successfull";
                    return RedirectToAction("Addcategory");
                }
                else
                {
                    return View();
                }
            }
            return View();
        }

        #endregion

        #region Add Question
        public ActionResult Addquestion()
        {
            int id = Convert.ToInt32(Session["ad_id"].ToString());
            List<TBL_CATEGORY> li = db.TBL_CATEGORY.Where(x => x.CAT_FK_ADID == id).ToList();
            ViewBag.cat = new SelectList(li, "CAT_ID", "CAT_NAME");
            return View();
        }

        [HttpPost]
        public ActionResult Addquestion(TBL_QUESTION q)
        {

            int id = Convert.ToInt32(Session["ad_id"].ToString());
            List<TBL_CATEGORY> li = db.TBL_CATEGORY.Where(x => x.CAT_FK_ADID == id).ToList();
            ViewBag.cat = new SelectList(li, "CAT_ID", "CAT_NAME");

            if (ModelState.IsValid == true)
            {
                TBL_QUESTION ques = new TBL_QUESTION();
                ques.QUESTION_TXT = q.QUESTION_TXT;
                ques.OPA = q.OPA;
                ques.OPB = q.OPB;
                ques.OPC = q.OPC;
                ques.OPD = q.OPD;
                ques.COP = q.COP;
                ques.QUESTION_FK_CAT = q.QUESTION_FK_CAT;
                db.TBL_QUESTION.Add(ques);
                int a = db.SaveChanges();
                if (a > 0)
                {
                    ViewBag.addquestion = "Add Successfull";
                    ModelState.Clear();
                }
                else
                {
                    return RedirectToAction("Addquestion");
                }
            }
            return View();
        }

        #endregion
      
        #region Logout
        public ActionResult Logout()
        {
            Session.Abandon();
            Session.RemoveAll();
            return RedirectToAction("Index");
        }

        #endregion

        public ActionResult StudentExam()
        {
            return View();
        }

        [HttpPost]
        public ActionResult StudentExam(string room)
        {
            List<TBL_CATEGORY> list = db.TBL_CATEGORY.ToList();
            foreach (var item in list)
            {
                if (item.CAT_ENCRYPTEDSTRING == room)
                {
                    List<TBL_QUESTION> li = db.TBL_QUESTION.Where(x => x.QUESTION_FK_CAT == item.CAT_ID).ToList();
                    Queue<TBL_QUESTION> queue = new Queue<TBL_QUESTION>();
                    foreach (TBL_QUESTION a in li)
                    {
                        queue.Enqueue(a);
                    }
                    TempData["questions"] = queue;
                    TempData["score"] = 0;
                    TempData.Keep();
                    return RedirectToAction("QuizStart");
                }
                else
                {
                    ViewBag.msg = "No Room Found.....";
                }
            }
            return View();
        }



        public ActionResult QuizStart()
        {

            TBL_QUESTION q = null;

            if (TempData["questions"] != null)
            {
                Queue<TBL_QUESTION> qlist = (Queue<TBL_QUESTION>)TempData["questions"];
                if (qlist.Count > 0)
                {
                    q = qlist.Peek();
                    qlist.Dequeue();
                    TempData["questions"] = qlist;
                    TempData.Keep();
                }
                else
                {
                    return RedirectToAction("EndExam");
                }
            }
            else
            {
                return RedirectToAction("StudentExam");
            }
            return View(q);
        }


        [HttpPost]
        public ActionResult QuizStart(TBL_QUESTION q)
        {
            string correctans = null;

            if (q.OPA != null)
            {
                correctans = "A";
            }
            else if (q.OPB != null)
            {
                correctans = "B";
            }
            else if (q.OPC != null)
            {
                correctans = "C";
            }
            else if (q.OPD != null)
            {
                correctans = "D";
            }

            if (correctans.Equals(q.COP))
            {
                TempData["score"] = Convert.ToInt32(TempData["score"]) + 1;
            }

            TempData.Keep();

            return RedirectToAction("QuizStart");
        }

        public ActionResult EndExam()
        {
            return View();
        }
    }
}