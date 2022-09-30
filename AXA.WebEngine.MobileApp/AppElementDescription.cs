﻿// Copyright (c) 2016-2022 AXA France IARD / AXA France VIE. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Modified By: YUAN Huaxing, at: 2022-5-13 18:26
using AXA.WebEngine.Web;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.iOS;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AXA.WebEngine.MobileApp
{
    /// <summary>
    /// The Object Description how to identify the UI Elemen for native Mobile Application
    /// </summary>
    public class AppElementDescription : ElementDescription
    {

        /// <summary>
        /// Initialize the element description. If the element is not part of PageObject, you need to call <see cref="ElementDescription.UseDriver(WebDriver)"/> to indicate with WebDriver will be used.
        /// </summary>
        public AppElementDescription()
        {

        }

        /// <summary>
        /// Initialize the element description using given WebDriver
        /// </summary>
        /// <param name="driver">WebDriver instance</param>
        public AppElementDescription(WebDriver driver)
        {
            this.UseDriver(driver);
        }

        /// <summary>
        /// Identifier of the element
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The Name property of the element. Name and ContentDescription can't be used in the same time, or Name attribute will be ignored.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// To identify element via it's AccessibilityId. Id and AccessibilityId can't be used in the same time.
        /// </summary>
        public string AccessbilityId { get; set; }

        /// <summary>
        /// To identify elements via it's content-desc. Name and ContentDescription can't be used in the same time.
        /// </summary>
        public string ContentDescription { get; set; }

        /// <summary>
        /// Text of Innertext of the element
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// XPath of the element. Please avoid to use XPATH if another identification approche is available.
        /// </summary>
        public string XPath { get; set; }

        /// <summary>
        /// Classname of the element.
        /// </summary>
        public string ClassName { get; set; }


        ///<summary>
        /// Using IOS Class Chain, only avaiable for iOS applications. When using IosClassChain, other locators will be ignored..
        ///</summary>
        public string IosClassChain { get; set; }

        /// <summary>
        /// Using UIAutomator selector, only avaiable for Android applications. When using UIAutomator selector, other locators will be ignored.
        /// </summary>
        public string UIAutomatorSelector { get; set; }

        /// <summary>
        /// Shows a string representation of this AppElementDescription
        /// </summary>
        /// <returns>A string representation of this AppElementDescription</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(Id))
            {
                sb.Append($"Id={Id}");
            }
            if (!string.IsNullOrEmpty(Name))
            {
                sb.Append($"Name={Name}");
            }
            if (!string.IsNullOrEmpty(Text))
            {
                sb.AppendLine($"Text={Text}");
            }
            if (!string.IsNullOrEmpty(XPath))
            {
                sb.AppendLine($"Text={XPath}");
            }
            if (!string.IsNullOrEmpty(AccessbilityId))
            {
                sb.AppendLine($"AccessibilityId={AccessbilityId}");
            }
            if (!string.IsNullOrEmpty(ContentDescription))
            {
                sb.AppendLine($"ContentDescription={ContentDescription}");
            }
            return sb.ToString();
        }

        /// <inheritdoc/>
        protected override IWebElement InternalFindElement()
        {
            var elements = InternalFindElements();
            if (elements.Count() > 1)
            {
                throw new OpenQA.Selenium.InvalidSelectorException($"Multiple elements are found.");
            }
            else
            {
                return elements.First();
            }

        }

        /// <summary>
        /// Tires to bring current element into viewport. If the element is still not visible after scroll action, the return value will be false.
        /// </summary>
        /// <returns>True if the element is visible after scrolling. False when the element is not visible.</returns>
        public bool ScrollIntoView()
        {
            int max = 10, count = 0;
            while(!this.Exists(1) && count < max)
            {
                count++;
                ScrollDown((int)(driver.Manage().Window.Size.Width * 0.5), (int)(driver.Manage().Window.Size.Height * 0.5));
            }
            return this.Exists(1);
        }

        private void ScrollDown(int x, int y)
        {
            int startY = (int)(driver.Manage().Window.Size.Height * 0.80);
            int endY = (int)(driver.Manage().Window.Size.Height * 0.30);

            var finger = new PointerInputDevice(PointerKind.Touch);
            var actionSequence = new ActionSequence(finger, 0);

            actionSequence.AddAction(finger.CreatePointerMove(CoordinateOrigin.Viewport, x, startY, TimeSpan.Zero));
            actionSequence.AddAction(finger.CreatePointerDown(MouseButton.Touch));
            actionSequence.AddAction(finger.CreatePointerMove(CoordinateOrigin.Viewport, x, endY, new TimeSpan(0,0,1)));
            actionSequence.AddAction(finger.CreatePointerUp(MouseButton.Touch));


            driver.PerformActions(new List<ActionSequence> { actionSequence });
        }

        /// <inheritdoc />

        protected override IReadOnlyCollection<IWebElement> InternalFindElements()
        {
            IEnumerable<IWebElement> elements = null;

            if (IosClassChain != null)
            {
                var chains = driver.FindElements(MobileBy.IosClassChain(IosClassChain));
                return chains;
            }

            if(UIAutomatorSelector != null)
            {
                var locators = driver.FindElements(MobileBy.AndroidUIAutomator(UIAutomatorSelector));
                return locators;
            }

            //above locators 

            if (this.Id != null)
            {
                elements = driver.FindElements(MobileBy.Id(this.Id));
            }

            if (this.AccessbilityId != null)
            {
                var aids = driver.FindElements(MobileBy.AccessibilityId(this.AccessbilityId));
                if (elements == null)
                {
                    elements = aids;
                }
                else
                {
                    elements = elements.Where(x => aids.Contains(x));
                }
            }

            if (this.ContentDescription != null)
            {
                if (driver is AndroidDriver ad)
                {
                    var cds = ad.FindElements(MobileBy.AccessibilityId(this.ContentDescription));
                    if (elements == null)
                    {
                        elements = cds;
                    }
                    else
                    {
                        elements = elements.Where(x => cds.Contains(x));
                    }
                }
                else if (driver is IOSDriver)
                {
                    var cds = driver.FindElements(MobileBy.Name(this.ContentDescription));
                    if (elements == null)
                    {
                        elements = cds;
                    }
                    else
                    {
                        elements = elements.Where(x => cds.Contains(x));
                    }
                }
            }
            else if (this.Name != null)
            {
                var names = driver.FindElements(MobileBy.Name(this.Name));
                if (elements == null)
                {
                    elements = names;
                }
                else
                {
                    elements = elements.Where(x => names.Contains(x));
                }
            }

            if (this.ClassName != null)
            {
                var classes = driver.FindElements(MobileBy.ClassName(ClassName));
                if (elements == null)
                {
                    elements = classes;
                }
                else
                {
                    elements = elements.Where(x => classes.Contains(x));
                }

            }

            if (this.XPath != null)
            {
                var xpaths = driver.FindElements(MobileBy.XPath(this.XPath));
                if (elements == null)
                {
                    elements = xpaths;
                }
                else
                {
                    elements = elements.Where(x => xpaths.Contains(x));
                }
            }

            if (this.Text != null)
            {
                if (elements != null)
                {
                    elements = elements.Where(x => x.Text == Text);
                }
                else
                {
                    elements = driver.FindElements(MobileBy.LinkText(Text));
                }
            }

            if (elements == null || elements.Count() == 0)
            {
                throw new NoSuchElementException("No such WebElement");
            }
            else
            {
                IReadOnlyCollection<IWebElement> e = new List<IWebElement>(elements);
                return e;
            }
        }

        /// <inheritdoc />
        protected override byte[] InternalGetScreenshot()
        {
            var a = this.FindElement();
            if (a is AppiumElement ae)
            {
                return ae.GetScreenshot().AsByteArray;
            }
            else
            {
                throw new ElementNotInteractableException($"Cannot covert IWebElement to AppiumElement for screenshot. {this}");
            }
        }
    }
}
