using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ContactsApp.Models;
using System.Net;
using System.Data.Entity;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI.WebControls;

namespace ContactsApp.Controllers
{
    public class ContactController : Controller
    {
        private DatabaseEntities db = new DatabaseEntities();

        public ActionResult SeeContacts()               //róźnica pomiędzy SeeContacts a ManageContacts jest taka, że SeeContacts pozwala jedynie na przeglądanie kontaktów (opcja przed zalogowaniem), a ManageContacts pozwala również na ich edycję, usuwanie oraz dodawanie nowych (po zalogowaniu)
        {
            return View(db.Contact.ToList());
        }

        public ActionResult ManageContacts()
        {
            return View(db.Contact.ToList());
        }


        public ActionResult Create()
        {
           
            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind(Include = "ID,FirstName,LastName,Email,Password,Category,Subcategory,PhoneNumber,DateOfBirth")] Contact newContact, string ddlSelectValue, string tbSelectValue, FormCollection fmc)    //dziala jak INSERT INTO w SQL
        {
            if (ModelState.IsValid)
            {

                bool isCapitalLetter = false;
                bool isDigit = false;


               
                if (db.Contact.Any(i => i.Email == newContact.Email))        //LINQ sprawdzanie czy adres email jest unikatowy
                {
                    ViewBag.email = "incorrect";
                    return View();
                }

                if (newContact.Password.Length < 8)                 //sprawdzanie minimalnej długości hasła
                {
                    ViewBag.password = "incorrect";
                    return View();
                }

                for (int i = 0; i < newContact.Password.Length; i++)     //sprawdzanie czy w hasle wystapila minimum 1 wielka litera
                {
                    if ((int)newContact.Password[i] >= 65 && (int)newContact.Password[i] <= 90)     //zamiana poszczegołnych znaków hasła na kod ascii, zeby zobaczyc czy znajduje sie tam wielka litera
                        isCapitalLetter = true;

                    if ((int)newContact.Password[i] >= 48 && (int)newContact.Password[i] <= 57)     //zamiana poszczegołnych znaków hasła na kod ascii, zeby zobaczyc czy znajduje sie tam cyfra
                        isDigit = true;
                }

                if (!isCapitalLetter || !isDigit)                   //gdy brakuje wielkiej litery badz liczby to powrot do poprzedniego widoku
                {
                    ViewBag.password = "incorrect";
                    return View();
                }


                string selectedRB = fmc["radioButton"].ToString();      

                if (selectedRB=="Other")                //przypisanie kategorii Other
                    newContact.Category = 3;
                else if (selectedRB == "Business")      //przypisanie kategorii Business
                    newContact.Category = 1;


                if (ddlSelectValue != "")
                    newContact.Subcategory = db.Subcategory.First(i => i.SubcategoryName == ddlSelectValue).SubcategoryId;  //przypisanie podkategorii nalezacej do Business
                if (tbSelectValue != "")
                {
                    if(db.Subcategory.Any(i => i.SubcategoryName==tbSelectValue))       
                        newContact.Subcategory = db.Subcategory.First(i => i.SubcategoryName == tbSelectValue).SubcategoryId;  //jesli slowo znajduje sie juz w slowniku
                    else
                    {

                        int maxIndex = db.Subcategory.OrderByDescending(i => i.SubcategoryId).FirstOrDefault().SubcategoryId;//znalezienie kolejnego indeksu dla wstawianego do slownika wyrazu - tutaj jakis blad
                                 
                        Subcategory subCat = new Subcategory();
                        subCat.SubcategoryId = maxIndex+1;
                        subCat.SubcategoryName = tbSelectValue;
                        subCat.SubcategoryParentId = 3;
                        db.Subcategory.Add(subCat);                                       //dodanie wyrazu do słownika
                        newContact.Subcategory = subCat.SubcategoryId;
                                                                                                
                    }
                    
                }
                    


                /*  Próby z hashowniem hasła
                var md5 = MD5.Create();
                
                var hashedPassword = md5.ComputeHash(Encoding.ASCII.GetBytes(newContact.Password));

                newContact.Password = Encoding.ASCII.GetString(hashedPassword);
                */

                db.Contact.Add(newContact);     //dodanie nowego rekordu do bazy danych

                db.SaveChanges();
                return RedirectToAction("ManageContacts");
            }

            return View("ManageContacts");
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contact.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Include = "ID,FirstName,LastName,Email,Password,Category,Subcategory,PhoneNumber,DateOfBirth")] Contact updateContact)       //dziala jak UPDATE w SQL
        {
            if (ModelState.IsValid)
            {
                db.Entry(updateContact).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ManageContacts");
            }
            return View("ManageContacts");
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contact.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Contact contact = db.Contact.Find(id);
            db.Contact.Remove(contact);
            db.SaveChanges();
            return RedirectToAction("ManageContacts");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public void CheckEmailAndPassword()
        {

        }
    }
}