/**
* Author:    Xia Li
* Date:      11/22/2022
* Course:    CS 4540, University of Utah, School of Computing
* Copyright: CS 4540 and Xia Li and Wenlin Li - This work may not be copied for use in Academic Coursework.
*
* I, Xia Li, certify that I wrote this code from scratch and did 
* not copy it in part or whole from another source.  Any references used 
* in the completion of the assignment are cited in my README file and in
* the appropriate method header.
*
* Web Scraper for University of Utah course page
*
*/
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Diagnostics.Metrics;
using System.Net.Http.Json;
using System.Security.Policy;
using System.Text;
using Application = System.Windows.Forms.Application;
using Keys = OpenQA.Selenium.Keys;


namespace CourseScraper
{

    
    public partial class Form1 : Form
    {
        ChromeDriver _driver;
        string year;
        string semester;
        int limit;
        List<string> classNumber;
        List<string> subject;
        List<string> catagorieNumber;
        List<string> section;
        List<string> title;
        List<string> enrollment;
        List<string> waitList;
        List<string> currentEnrolled;
        List<string> seatingAvailiable;
        List<string> pID;
        List<string> credits;
        List<string> courseDescription;
        string description;
        string credit;
        

        public Form1()
        {
            InitializeComponent();
            if (_driver == null)
            {
                _driver = new ChromeDriver();
                limit = 0;
            }
            
            comboBox1.Items.Add("Spring");
            comboBox1.Items.Add("Fall");
            comboBox1.Items.Add("Summer");
 
            limit = 0;
            button3.Enabled = false;
            credit = "";
            description = "";
            classNumber = new List<string>();
            subject = new List<string>();
            catagorieNumber = new List<string>();
            section = new List<string>();
            title = new List<string>();
            enrollment = new List<string>();
            waitList = new List<string>();
            currentEnrolled = new List<string>();
            seatingAvailiable = new List<string>();
            pID = new List<string>();
            credits = new List<string>();
            courseDescription = new List<string>();
            Application.ApplicationExit += new EventHandler(this.OnApplicationExit);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            
        }

        /*Scrape Single Class
         * 
         */
        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text == "" || courseText.Text == "")
            {
                MessageBox.Show("The course or semester is missing, please make sure you input them");
                return;
            }

            if (limit == 100)
            {
                richTextBox1.Text += "The limit of scraping has been exceeded" + "\r\n";
                return;
            }
            string[] courseName = courseText.Text.ToUpper().Split("CS");
            if (Int32.Parse(courseName[1]) >= 6999 || Int32.Parse(courseName[1]) <= 1000)
            {
                richTextBox1.Text += "Please search the course between 1000 to 6999" + "\r\n";
                return;
            }
            string url = "https://catalog.utah.edu/#/home";

            scrape(url);
        }

        /*
         * This is a private helper method for scraping class.
        */
        private void scrape(String url)
        {
            _driver.Navigate().Refresh();
            _driver.Navigate().GoToUrl(url);
            
                try
                {
                    _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
                    var input_the_search = _driver.FindElement(By.XPath("//*[@id=\"top\"]/div/div[2]/span/div/div/div/input"));
                    _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
                    input_the_search.SendKeys(courseText.Text.ToString());
                    _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
                    input_the_search.SendKeys(Keys.Return);
                    var click_on_course = _driver.FindElement(By.PartialLinkText(courseText.Text.ToUpper()));
                    _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
                    click_on_course.Click();
                    var course_Division = _driver.FindElements(By.ClassName("course-view__pre___2VF54"));
                    int index = 0;
                    var course_Description_Text = "";
                    credit = _driver.FindElement(By.XPath("//*[@id=\"__KUALI_TLP\"]/div/div[2]/div[2]/span/div/div/div/div/div/div")).Text;
                    int preCheck = 0;
                    foreach (var c in course_Division)
                    {
                    
                    
                    if (index == 2)
                    {
                        var course_Description = c.FindElement(By.TagName("div")).Text;
                        if (course_Description.Contains("Prerequisites"))
                        {
                            preCheck = 1;
                        }
                        else
                        {
                            description = course_Description;
                            course_Description_Text = courseText.Text + " Course Description: " + "\r\n" + course_Description + "\r\n";
                        }
                    }

                    if(preCheck == 1)
                    {
                        if (index == 3)
                        {
                            var course_Description = c.FindElement(By.TagName("div")).Text;
                            description = course_Description;
                            course_Description_Text = courseText.Text + " Course Description: " + "\r\n" + course_Description + "\r\n";
                            preCheck = 0;
                        }
                    }
                       
                        
                        if (index == 4)
                        {
                            var components = c.FindElements(By.TagName("div"));
                            foreach (var component in components)
                            {
                                if (component.Text.ToUpper().Contains("LECTURE") || component.Text.ToUpper().Contains("SEMINAR"))
                                {
                                    course_Description_Text += "Course Component: " + "\r\n" + component.Text + "\r\n";
                                    break;
                                }
                                else
                                {
                                richTextBox1.Text += "This course is not Lecture or Seminar";
                                return;
                                }
                            }      
                        }
                        index++;
                    }
                    richTextBox1.Text += course_Description_Text;
                    limit++;
                    textBox3.Text = limit.ToString();
                }
                catch
                {
                    richTextBox1.Text += "The course you are looking for is not found";
                }
            }
         
        /**
         * private helper method to get full enrollment
         */
        private void getFullEnroll(String url)
        {
            _driver.Navigate().GoToUrl(url);
            _driver.Manage().Window.Size = new Size(1920, 1080);//get full size window so we can scrape
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
            var seating_link_click = _driver.FindElement(By.LinkText("Seating availability for all CS classes"));
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
            seating_link_click.Click();
            var table = _driver.FindElement(By.TagName("table"));
            var body = table.FindElements(By.XPath("//tbody//tr"));
            int i = 1; 
            foreach (var r in body)
            {
                string section = _driver.FindElement(By.XPath("//*[@id=\"seatingAvailabilityTable\"]/tbody/tr[" + i.ToString() + "]" + "/td[4]")).Text;
                string cl = _driver.FindElement(By.XPath("//*[@id=\"seatingAvailabilityTable\"]/tbody/tr[" + i.ToString() + "]" + "/td[3]")).Text;

                var classNum = _driver.FindElement(By.XPath("//*[@id=\"seatingAvailabilityTable\"]/tbody/tr[" + i.ToString() + "]" + "/td[1]")).Text;
                _driver.Navigate().GoToUrl(url);
                var component = _driver.FindElement(By.XPath("//*[@id=\"" + classNum + "\"]/div[1]/div[2]/ul/li[3]/span")).Text;

                if (section == "001" && (component == "Lecture" || component == "Seminar") && (Int32.Parse(cl) > 1000 || Int32.Parse(cl) < 6999))
                {
                   
                    //Course c = new Course();
                    seating_link_click = _driver.FindElement(By.LinkText("Seating availability for all CS classes"));
                    seating_link_click.Click();
                    string cNum = _driver.FindElement(By.XPath("//*[@id=\"seatingAvailabilityTable\"]/tbody/tr[" + i.ToString() + "]" + "/td[1]")).Text;
                    string sbj = _driver.FindElement(By.XPath("//*[@id=\"seatingAvailabilityTable\"]/tbody/tr[" + i.ToString() + "]" + "/td[2]")).Text;
                    string t = _driver.FindElement(By.XPath("//*[@id=\"seatingAvailabilityTable\"]/tbody/tr[" + i.ToString() + "]" + "/td[5]")).Text;
                    string e = _driver.FindElement(By.XPath("//*[@id=\"seatingAvailabilityTable\"]/tbody/tr[" + i.ToString() + "]" + "/td[6]")).Text;
                    string wList = _driver.FindElement(By.XPath("//*[@id=\"seatingAvailabilityTable\"]/tbody/tr[" + i.ToString() + "]" + "/td[7]")).Text;
                    string curr = _driver.FindElement(By.XPath("//*[@id=\"seatingAvailabilityTable\"]/tbody/tr[" + i.ToString() + "]" + "/td[8]")).Text;
                    string seats = _driver.FindElement(By.XPath("//*[@id=\"seatingAvailabilityTable\"]/tbody/tr[" + i.ToString() + "]" + "/td[9]")).Text;
                    classNumber.Add(cNum);
                    subject.Add(sbj);
                    catagorieNumber.Add(cl);
                    this.section.Add(section);
                    this.title.Add(t);
                    enrollment.Add(e);
                    waitList.Add(wList);
                    currentEnrolled.Add(curr);
                    seatingAvailiable.Add(seats);

                    courseText.Text = sbj + cl;

                    button1.PerformClick();

                    credits.Add(credit);
                    courseDescription.Add(description);

                    _driver.Navigate().GoToUrl(url);

                    var courseNum = _driver.FindElement(By.XPath("//*[@id=\"" + cNum + "\"]/div[1]/div[2]/ul/li[2]/span/a"));
                    string pid = courseNum.GetAttribute("href").Split("/")[3];
                    pID.Add(pid);
                    richTextBox1.Text += comboBox1.SelectedItem.ToString() + "," + textBox1.Text + "," + sbj + "," + cl + "," + t + "," + pid + "," + credit + "," + e + "," + description + "\r\n";
                    seating_link_click = _driver.FindElement(By.LinkText("Seating availability for all CS classes"));
                    seating_link_click.Click();
                    credit = "";
                    description = "";
                }
                else
                {
                    seating_link_click = _driver.FindElement(By.LinkText("Seating availability for all CS classes"));
                    seating_link_click.Click();
                    credit = "";
                    description = "";
                }
                i++;
            }
            button3.Enabled = true;
        }

        /**Find Enrollment
         * 
         */
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == ""  || comboBox1.Text == "")
            {
                MessageBox.Show("The semester or year is missing, please make sure you input them");
                return;
            }
            string semesters = comboBox1.SelectedItem.ToString();
            switch (semesters)
            {
                case "Spring":
                    semester = "4";
                    break;
                case "Summer":
                    semester = "6";
                    break;
                case "Fall":
                    semester = "8";
                    break;
            }
            year = textBox1.Text.Trim().Substring(textBox1.Text.Length - 3, textBox1.Text.Length - 1).ToString();
            year = year.Substring(year.Length - 2, year.Length - 1);
            string url = "https://student.apps.utah.edu/uofu/stu/ClassSchedules/main/" + "1" + year + semester + "/class_list.html?subject=CS";
            getFullEnroll(url);
             
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

       

        

        /**Save
         */
        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog savefile = new SaveFileDialog();
            var sb = new StringBuilder();
            savefile.Filter = "CSV file (*.csv)|*.csv| All Files (*.*)|*.*";
            savefile.FilterIndex = 2;
            savefile.RestoreDirectory = true;
            sb.AppendLine("Course Semester,Course Year,Course Dept,Course Number,Course Title,Course Instructors UNID,Course Credits,Course Enrollment,Course Description");
            for (int i = 0; i < classNumber.Count; i++)
            {
                sb.AppendLine(comboBox1.SelectedItem.ToString() + "," + textBox1.Text + "," + subject[i] + "," + catagorieNumber[i] + "," + title[i] + "," + pID[i] + "," + credit[i] + "," + enrollment[i] + "," + courseDescription[i]);
            }

            if (savefile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    File.WriteAllText(savefile.FileName, sb.ToString());
                }catch(Exception error)
                {
                    MessageBox.Show(error.ToString());
                }
                
            }
        }
        private void OnApplicationExit(object sender, EventArgs e)
        {
            _driver.Quit();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                HttpClient client = new HttpClient();
                List<Course> courses = new List<Course>();
                for(int i = 0; i < classNumber.Count; i++)
                {
                    courses.Add(new Course
                    {
                        credit = credits[i],
                        title = title[i],
                        catalogNumber = catagorieNumber[i],
                        section = section[i],
                        semester = comboBox1.SelectedText.ToString(),
                        year = textBox1.Text,
                        pID = pID[i],
                        Enroll = enrollment[i],
                        subject = subject[i]
                    }) ;
                }
               // HttpResponseMessage response = await client.PostAsJsonAsync<List<Course>>("https://localhost:7008/Courses/UploadCourses", courses);
                //

            }
            catch
            {
                richTextBox1.Text += "The data can not be posted, please try another way" + "\r\n";
            }
        }


    }

    public class Course
    {
        public string credit { get; set; }
        public string title { get; set; }
        public string catalogNumber { get; set; }
        public string section { get; set; }
        public string semester { get; set; }
        public string year { get; set; }
        public string pID { get; set; }
        public string Enroll { get; set; }
        public string subject { get; set; }
       
    }
}