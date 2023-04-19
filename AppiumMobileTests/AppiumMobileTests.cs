using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;

namespace AppiumMobileTests
{
    public class AppiumMobileTests
    {
        private AndroidDriver<AndroidElement> driver;
        private AppiumOptions options;
        private const string appLocation = @"E:\SoftUni\FE - Exam Prep\ExamPrepResources\com.android.example.github.apk";
        private const string appiumServer = "http://127.0.0.1:4723/wd/hub";

        [SetUp]
        public void PrepareApp()
        {
            this.options= new AppiumOptions() { PlatformName = "Android" };
            options.AddAdditionalCapability("app", appLocation);
            driver = new AndroidDriver<AndroidElement>(new Uri(appiumServer), options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        [TearDown]
        public void CloseApp()
        {
            driver.Quit();
        }

        [Test]
        public void Test_VerifyBarancevName()
        {
            var inputSearchField = driver.FindElementById("com.android.example.github:id/input");
            inputSearchField.Click();
            inputSearchField.SendKeys("Selenium");
            driver.PressKeyCode(AndroidKeyCode.Keycode_ENTER);

            var textSelenium = driver.FindElementByXPath("//android.view.ViewGroup/android.widget.TextView[2]");
            Assert.That(textSelenium.Text, Is.EqualTo("SeleniumHQ/selenium"));
            textSelenium.Click();

            //Assert Barancev is in the list
            var listTextBarancev = driver.FindElementByXPath("//android.widget.FrameLayout[2]/android.view.ViewGroup/android.widget.TextView");
            Assert.That(listTextBarancev.Text, Is.EqualTo("barancev"));

            listTextBarancev.Click();

            var textFieldBarancev = driver.FindElementByXPath("//android.widget.TextView[@content-desc='user name']");
            Assert.That(textFieldBarancev.Text, Is.EqualTo("Alexei Barantsev"));
        }
    }
}