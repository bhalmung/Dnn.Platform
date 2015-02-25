﻿#region Copyright
// 
// DotNetNuke® - http://www.dnnsoftware.com
// Copyright (c) 2002-2014
// by DNN Corporation
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions 
// of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.
#endregion

using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dnn.Modules.Html.Models;
using DotNetNuke.Common;
using DotNetNuke.Data;
using DotNetNuke.Web.Mvc.Framework.Controllers;

namespace Dnn.Modules.Html.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class HtmlController : DnnController
    {
        private readonly IDataContext _dataContext;

        /// <summary>
        /// 
        /// </summary>
        public HtmlController() : this(DataContext.Instance()) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataContext"></param>
        public HtmlController(IDataContext dataContext)
        {
            Requires.NotNull("dataContext", dataContext);

            _dataContext = dataContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            HtmlText item;

            using (_dataContext)
            {
                var rep = _dataContext.GetRepository<HtmlText>();

                item = rep.Find("WHERE ModuleID = " + ActiveModule.ModuleID)
                                .OrderByDescending(c => c.Version)
                                .FirstOrDefault();

                if (item != null && !String.IsNullOrEmpty(item.Content))
                {
                    item.Content = HttpUtility.HtmlDecode(item.Content);
                }
            }

            return View(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit()
        {
            HtmlText item;

            using (_dataContext)
            {
                var rep = _dataContext.GetRepository<HtmlText>();

                item = rep.Find("WHERE ModuleID = " + ActiveModule.ModuleID)
                                .OrderByDescending(c => c.Version)
                                .FirstOrDefault();

                if (item == null)
                {
                    item = new HtmlText { ModuleID = ActiveModule.ModuleID };
                }
                if (!String.IsNullOrEmpty(item.Content))
                {
                    item.Content = HttpUtility.HtmlDecode(item.Content);
                }
                else
                {
                    item.Content = String.Empty;
                }
            }

            return View(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(string content)
        {
            HtmlText item;

            using (_dataContext)
            {
                var rep = _dataContext.GetRepository<HtmlText>();

                item = rep.Find("WHERE ModuleID = " + ActiveModule.ModuleID)
                                .OrderByDescending(c => c.Version)
                                .FirstOrDefault();

                if (item == null)
                {
                    item = new HtmlText { ModuleID = ActiveModule.ModuleID, ItemID = -1 };
                }

                item.Content = HttpUtility.HtmlEncode(content);

                if (item.ItemID == -1)
                {
                    rep.Insert(item);
                }
                else
                {
                    rep.Update(item);
                }
            }

            return RedirectToDefaultRoute();
        }
    }
}