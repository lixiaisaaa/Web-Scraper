# PS7-CourseScraper
```
Author:     Robert Li
Date:       11/22/2022
Course:     CS 4540, University of Utah, School of Computing
GitHub ID:  lixiaisaaa
Repo:       https://github.com/uofu-cs4540-fall2022/ps7---web-scraper-lixiaisaaa
Commit Tag: Version 1.2
Project:    CourseScraper
Copyright:  CS 4540 and Robert Li - This work may not be copied for use in Academic Coursework.
```
# Overview of the Course Scraper Functionality 

This course scraper has 4 bottons mainly use, srape class can support you to scrape a single class, find enrollment can find all course in specific semester, and provide spicific information, like courseID, pID.. The ohter 2 bottons can support you save it as a CSV file and post it on web.

# Comments to Evaluators:

Hi, Evaluators! We are developing the Course Scrape for students to get courses information.


# Assignment Specific Topics


Above and Beyond paragraph - describe what you did that is more involved then the basic requirements for the assignment.  If you followed external tutorials/advice, make sure you cite your sources.
N/A
Improvements paragraph - If you have done any other work to improve your application over previous assignments, describe them here.  If not, type: -na-
N/A

# Consulted Peers:

List any peers (or other people) in the class (or outside for that matter) that you talked with about the project for more than one minute.

 - None

# Peers Helped:

List any students for whom you gave aid/instruction/tutoring/advice/etc.

  - None

# Acknowledgements:

  Course Scraper

# References:

  Lecture slides and videos   

# Time Expenditures:

  Predicted Hours: 10 
  Actual Hours: 15

# Time Out Value
  
  I chose 15 second as time out value for page load and implicit wait. I chose 15 second because loading pages and finding elements on the page will depend on the        speed of the browser and network connections. if someone has slow network connection will have majority impact.
  
# HtmlAgilityPack vs. Selenium 
  
  I think the selenium library is easier to use and the code lines from lecture is helpful. It works pretty well at most time, however, when I try to use FindElement()
  over 10 times on a same page, it will cause a lot of questions. I think the most useful key word is FindElement() function in selenium, we can easily scrape the       information from webpage. Otherwise, if we have source code, HtmlAgilityPack will be a lot of faster with FindNode() function.

# Thoughts on Selenium/What was easy/What was hard. 
  
  I think Selenium is easy to use. It can just search element from a web page directly by Xpath just like a real user are using it. 
  I think it will be slow when a lot of elements are needed.
  
