using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;

namespace SeleniumTests
{
    [TestFixture]
    public class DomImport
    {
        private IWebDriver driver;
        private StringBuilder verificationErrors;
        private string baseURL;
        private bool acceptNextAlert = true;

        [SetUp]
        public void SetupTest()
        {
            driver = new InternetExplorerDriver();
            baseURL = "https://clicpltest.egroup.hu/";
            verificationErrors = new StringBuilder();
        }

        [TearDown]
        public void TeardownTest()
        {
            try
            {
                driver.Quit();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
            Assert.AreEqual("", verificationErrors.ToString());
        }

        [Test]
        public void TheDomImportTest()
        {
            driver.Navigate().GoToUrl(baseURL + "/Login/Login"); Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("span.glyphicon.glyphicon-tag")).Click(); Thread.Sleep(1000);
            driver.FindElement(By.Id("loginId")).Clear();
            driver.FindElement(By.Id("loginId")).SendKeys("100003");
            driver.FindElement(By.Id("login")).Click(); Thread.Sleep(1000);
            driver.FindElement(By.Id("submit")).Click(); Thread.Sleep(1000);
            driver.Navigate().GoToUrl(baseURL + "/PaymentList/Import"); Thread.Sleep(1000);
            driver.FindElement(By.Id("Input_ImportFile")).Clear();
            driver.FindElement(By.Id("Input_ImportFile")).SendKeys(@"C:\Test\Import\test.PLI");
            driver.FindElement(By.Id("Input_OnlyIfNoErrors")).Click();
            new SelectElement(driver.FindElement(By.Id("Input_ImportState"))).SelectByText("To edit");
            driver.FindElement(By.Id("Input_MoveToGroup")).Click();
            driver.FindElement(By.Id("actionButton_Import")).Click(); Thread.Sleep(1000);
            driver.FindElement(By.Id("actionButton_Import")).Click();
            Thread.Sleep(3000);
            try
            {
                Assert.AreEqual("PAYMENT LIST", driver.FindElement(By.XPath(".//*[@id='main']/div[3]/div")).Text);
            }
            catch (AssertionException e)
            {
                verificationErrors.Append(e.Message);
            }

        }
        private bool IsElementPresent(By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        private bool IsAlertPresent()
        {
            try
            {
                driver.SwitchTo().Alert();
                return true;
            }
            catch (NoAlertPresentException)
            {
                return false;
            }
        }

        private string CloseAlertAndGetItsText()
        {
            try
            {
                IAlert alert = driver.SwitchTo().Alert();
                string alertText = alert.Text;
                if (acceptNextAlert)
                {
                    alert.Accept();
                }
                else
                {
                    alert.Dismiss();
                }
                return alertText;
            }
            finally
            {
                acceptNextAlert = true;
            }
        }
    }
}
