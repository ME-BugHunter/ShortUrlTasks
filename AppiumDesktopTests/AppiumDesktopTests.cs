using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;

namespace AppiumDesktopTests
{
    public class AppiumDesktopTests
    {
        private WindowsDriver<WindowsElement> driver;
        private AppiumOptions options;
        private const string appLocation = @"E:\SoftUni\FE - Exam Prep\ExamPrepResources\ShortURL-DesktopClient-v1.0.net6\ShortURL-DesktopClient.exe";
        private const string appiumServer = "http://127.0.0.1:4723/wd/hub";
        private const string appServer = "https://shorturl.mariaemanuilova.repl.co/api";

        [SetUp]
        public void PrepareApp()
        {
            this.options = new AppiumOptions();
            options.AddAdditionalCapability("app", appLocation);
            driver = new WindowsDriver<WindowsElement>(new Uri(appiumServer), options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        [TearDown]
        public void CloseApp()
        {
            driver.Quit();
        }

        [Test]
        public void Test_AddNewUrl()
        {
            var urlToAdd = "https://url" + DateTime.Now.Ticks + ".com";
            //Change the url of the backend
            var inputAppUrl = driver.FindElementByAccessibilityId("textBoxApiUrl");
            inputAppUrl.Clear();
            inputAppUrl.SendKeys(appServer);

            var buttonConnect = driver.FindElementByAccessibilityId("buttonConnect");
            buttonConnect.Click();

            Thread.Sleep(2000);

            var buttonAdd = driver.FindElementByAccessibilityId("buttonAdd");
            buttonAdd.Click();

            var inputUrl = driver.FindElementByAccessibilityId("textBoxURL");
            inputUrl.SendKeys(urlToAdd);
            var buttonCreate = driver.FindElementByAccessibilityId("buttonCreate");
            buttonCreate.Click();

            var createdUrl = driver.FindElementByName(urlToAdd);
            Assert.IsNotEmpty(createdUrl.Text);
            Assert.That(createdUrl.Text, Is.EqualTo(urlToAdd));
        }
    }
}