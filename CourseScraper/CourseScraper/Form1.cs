using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace CourseScraper
{
    
    public partial class Form1 : Form
    {
        ChromeDriver _driver;
        public Form1()
        {
            InitializeComponent();
            _driver = new ChromeDriver();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _driver.Navigate().GoToUrl("https://www.cs.utah.edu");
            var people_link = _driver.FindElement(By.Id("menu-item-355"));
            people_link.Click();
            Thread.Sleep(5000);
            var click_on_me = _driver.FindElement(By.Id("menu-item-356"));
            click_on_me.Click();
        }
    }
}