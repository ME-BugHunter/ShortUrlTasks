using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V109.Network;

namespace SeleniumTests
{
    public class SeleniumTests
    {
        private WebDriver driver;
        private const string baseUrl = "https://shorturl.mariaemanuilova.repl.co/";

        [SetUp]
        public void OpenWebApp()
        {
            this.driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
            driver.Navigate().GoToUrl(baseUrl);      
        }

        [TearDown]
        public void closeWebApp()
        {
            driver.Quit();
        }

        [Test]
        public void Test_TableTopLeftCellContent()
        {
            var linkShortUrl = driver.FindElement(By.LinkText("Short URLs"));
            linkShortUrl.Click();

            var topLeftCellLabel = driver.FindElement(By.CssSelector("th:nth-child(1)"));

            Assert.That(topLeftCellLabel.Text, Is.EqualTo("Original URL"), "Top left cell text content");
        }

        [Test]
        public void Test_AddValidURL()
        {
            var urlToAdd = "http://url" + DateTime.Now.Ticks + ".com";

            driver.FindElement(By.LinkText("Add URL")).Click();
            driver.FindElement(By.Id("url")).SendKeys(urlToAdd);
            driver.FindElement(By.XPath("//button[@type='submit']")).Click();

            //Assertion for the URL in the page source 
            Assert.That(driver.PageSource.Contains(urlToAdd));

            var tableLastRow = driver.FindElements(By.CssSelector("table > tbody > tr")).Last();
            var firstCellLastRow = tableLastRow.FindElements(By.CssSelector("td")).First();

            //Assert URL in the table
            Assert.That(firstCellLastRow.Text, Is.EqualTo(urlToAdd), "URL text");
        }

        [Test]
        public void Test_TryAddInvalidUrl()
        {
            driver.FindElement(By.LinkText("Add URL")).Click();
            driver.FindElement(By.Id("url")).SendKeys("somerandomurl");
            driver.FindElement(By.XPath("//button[@type='submit']")).Click();

            var labelErrorMessage = driver.FindElement(By.XPath("//div[@class='err']"));

            Assert.That(labelErrorMessage.Text, Is.EqualTo("Invalid URL!"));
            Assert.True(labelErrorMessage.Displayed);
        }

        [Test]
        public void Test_TryVisitInvalidUrl()
        {
            driver.Url= "http://shorturl.nakov.repl.co/go/invalid536524";
          
            var labelErrorMessage = driver.FindElement(By.XPath("//div[@class='err']"));

            Assert.That(labelErrorMessage.Text, Is.EqualTo("Cannot navigate to given short URL"), "Check error label text");
            Assert.True(labelErrorMessage.Displayed);
        }

        [Test]
        public void Test_CounterIncrease()
        {
            var linkShortUrl = driver.FindElement(By.LinkText("Short URLs"));
            linkShortUrl.Click();

            var tableFirstRow = driver.FindElements(By.CssSelector("table > tbody > tr")).First();
            var oldCounter = int.Parse(tableFirstRow.FindElements(By.CssSelector("td")).Last().Text);

            var linkToClickCell = tableFirstRow.FindElements(By.CssSelector("td"))[1];

            var linktoClick = linkToClickCell.FindElement(By.TagName("a"));
            linktoClick.Click();

            driver.SwitchTo().Window(driver.WindowHandles[0]);
            driver.Navigate().Refresh();
            tableFirstRow = driver.FindElements(By.CssSelector("table > tbody > tr")).First();

            var newCounter = int.Parse(tableFirstRow.FindElements(By.CssSelector("td")).Last().Text);

            Assert.That(newCounter, Is.EqualTo(oldCounter + 1));
        }
    }
}