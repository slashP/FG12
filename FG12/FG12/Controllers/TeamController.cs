﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FG12.Models;

namespace FG12.Controllers
{
    [Authorize]
    public class TeamController : Controller
    {
        private DataContext db = new DataContext();

        //
        // GET: /Team/

        public ActionResult Index()
        {
            var teams = db.Teams.Include(t => t.Group).Include(t => t.GroupMiddleStage);
            return View(teams.ToList());
        }

        //
        // GET: /Team/Details/5

        public ActionResult Details(int id = 0)
        {
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        //
        // GET: /Team/Create

        public ActionResult Create()
        {
            ViewBag.GroupId = new SelectList(db.Groups, "Id", "Name");
            ViewBag.GroupMiddleStageId = new SelectList(db.Groups, "Id", "Name");
            return View();
        }

        //
        // POST: /Team/Create

        [HttpPost]
        public ActionResult Create(Team team)
        {
            if (ModelState.IsValid)
            {
                db.Teams.Add(team);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.GroupId = new SelectList(db.Groups, "Id", "Name", team.GroupId);
            ViewBag.GroupMiddleStageId = new SelectList(db.Groups, "Id", "Name", team.GroupMiddleStageId);
            return View(team);
        }

        //
        // GET: /Team/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            ViewBag.GroupId = new SelectList(db.Groups, "Id", "Name", team.GroupId);
            ViewBag.GroupMiddleStageId = new SelectList(db.Groups, "Id", "Name", team.GroupMiddleStageId);
            return View(team);
        }

        //
        // POST: /Team/Edit/5

        [HttpPost]
        public ActionResult Edit(Team team)
        {
            if (ModelState.IsValid)
            {
                db.Entry(team).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GroupId = new SelectList(db.Groups, "Id", "Name", team.GroupId);
            ViewBag.GroupMiddleStageId = new SelectList(db.Groups, "Id", "Name", team.GroupMiddleStageId);
            return View(team);
        }

        //
        // GET: /Team/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        //
        // POST: /Team/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Team team = db.Teams.Find(id);
            db.Teams.Remove(team);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}