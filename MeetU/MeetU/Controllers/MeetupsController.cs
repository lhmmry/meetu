﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MeetU.Models;
using Microsoft.AspNet.Identity;

namespace MeetU.Controllers
{
    public class MeetupsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Meetups
        public async Task<ActionResult> Index()
        {
            var meetups = db.Meetups.Include(m => m.ApplicationUser);
            return View(await meetups.ToListAsync());
        }

        // GET: Meetups/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Meetup meetup = await db.Meetups.FindAsync(id);
            if (meetup == null)
            {
                return HttpNotFound();
            }
            return View(meetup);
        }

        // GET: Meetups/Create
        [Authorize]
        public ActionResult Create()
        {
            //ViewBag.Sponsor = new SelectList(db.ApplicationUsers, "Id", "Email");
            return View();
        }

        // POST: Meetups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Meetup meetup)
        {
            meetup.Sponsor = User.Identity.GetUserId();
            meetup.CreatedAt = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Meetups.Add(meetup);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            //ViewBag.Sponsor = new SelectList(db.ApplicationUsers, "Id", "Email", meetup.Sponsor);
            return View(meetup);
        }

        // GET: Meetups/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Meetup meetup = await db.Meetups.FindAsync(id);
            if (meetup == null)
            {
                return HttpNotFound();
            }
            //ViewBag.Sponsor = new SelectList(db.ApplicationUsers, "Id", "Email", meetup.Sponsor);
            return View(meetup);
        }

        // POST: Meetups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Description,When,Where,Sponsor")] Meetup meetup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(meetup).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            //ViewBag.Sponsor = new SelectList(db.ApplicationUsers, "Id", "Email", meetup.Sponsor);
            return View(meetup);
        }

        // GET: Meetups/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Meetup meetup = await db.Meetups.FindAsync(id);
            if (meetup == null)
            {
                return HttpNotFound();
            }
            return View(meetup);
        }

        // POST: Meetups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Meetup meetup = await db.Meetups.FindAsync(id);
            db.Meetups.Remove(meetup);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
