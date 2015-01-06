﻿using Excess.RuntimeProject;
using Excess.Web.Entities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Excess.Web.Controllers
{
    public class ProjectController : ExcessControllerBase
    {
        public ProjectController(IProjectManager manager)
        {
            _manager = manager;
        }

        public ActionResult LoadProject(int projectId)
        {
            ProjectRepository repo = new ProjectRepository();
            Project project = repo.LoadProject(projectId);
            if (project == null)
                return HttpNotFound();

            if (!project.IsSample)
            {
                if (!User.Identity.IsAuthenticated)
                    return HttpNotFound(); //td: right error

                if (User.Identity.GetUserId() != project.UserID)
                    return HttpNotFound(); //td: right error
            }

            var runtime = _manager.createRuntime(project.ProjectType);
            foreach (var file in project.ProjectFiles)
                runtime.add(file.Name, file.ID, file.Contents);

            Session["project"]   = runtime;

            if (!project.IsSample)
                Session["projectId"] = project.ID;

            return Json(new
            {
                defaultFile = runtime.defaultFile(),
                tree        = new[] { projectTree(project) }
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadFile(string file)
        {
            var project = Session["project"] as IRuntimeProject;
            if (project == null)
                return HttpNotFound(); //td: right error

            string contents = project.fileContents(file);
            if (contents == null)
                return HttpNotFound(); //td: right error

            return Content(contents);
        }

        public ActionResult SaveFile(string file, string contents)
        {
            var project = Session["project"] as IRuntimeProject;
            if (project == null)
                return HttpNotFound(); //td: right error

            project.modify(file, contents);

            if (Session["projectId"] != null)
            {
                int fileIdx = project.fileId(file);
                if (fileIdx < 0)
                    return HttpNotFound(); //td: right error

                ProjectRepository repo = new ProjectRepository();
                repo.SaveFile(fileIdx, contents);
            }

            return Content("ok"); 
        }

        public ActionResult CreateClass(string className)
        {
            var project = Session["project"] as IRuntimeProject;
            if (project == null)
                return HttpNotFound(); //td: right error

            var contents = string.Format("class {0} \n{{\n}}", className);
            var fileId   = -1;
            if (Session["projectId"] != null)
            {
                ProjectRepository repo = new ProjectRepository();
                fileId = repo.CreateFile((int)Session["projectId"], className, contents);
            }

            project.add(className, fileId, contents);
            return Content("ok");
        }

        public ActionResult Compile()
        {
            var project = Session["project"] as IRuntimeProject;
            if (project == null)
                return HttpNotFound(); //td: right error

            project.compile();
            return Content("ok");
        }

        public ActionResult Execute()
        {
            var project = Session["project"] as IRuntimeProject;
            if (project == null)
                return HttpNotFound(); //td: right error

            project.run();
            return Content("ok");
        }

        public ActionResult Notifications()
        {
            var project = Session["project"] as IRuntimeProject;
            if (project == null)
                return HttpNotFound(); //td: right error

            var notifications = project.notifications();
            return Json(notifications, JsonRequestBehavior.AllowGet);
        }

        //Project tree
        private class TreeNodeAction
        {
            public string id { get; set; }
            public string icon { get; set; }
        }

        private class TreeNode
        {
            public string label { get; set; }
            public string icon { get; set; }
            public string action { get; set; }
            public dynamic data { get; set; }
            public IEnumerable<TreeNodeAction> actions { get; set; }
            public IEnumerable<TreeNode> children { get; set; }
        }

        private TreeNode projectTree(Project project)
        {
            TreeNode result = new TreeNode
            {
                label   = project.Name,
                icon    = "fa-sitemap",
                actions = new[]
                {
                    new TreeNodeAction { id = "add-class", icon = "fa-plus-circle" }
                } ,
                children = project.ProjectFiles.Select<ProjectFile, TreeNode>( file =>
                {
                    return new TreeNode
                    {
                        label   = file.Name,
                        icon    = "fa-code",
                        action  = "select-file",
                        data    = file.Name,
                        actions = new[]
                        {
                            new TreeNodeAction { id = "remove-file", icon = "fa-times-circle-o"       },
                            new TreeNodeAction { id = "open-tab",    icon = "fa-arrow-circle-o-right" },
                        }
                    };
                })
            };

            return result;
        }

        private IProjectManager _manager;
    }
}