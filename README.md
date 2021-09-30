# SystemAnalysisGroupProject
Group Project with Cody, Brian, and Christian

# Scope
## In scope:
- User types
- Different web page depending on the logged in user
- 30 Students per staff
- Student information
	-name, birthday, address, major, first year enrolled, highschool attended, highschool transcript (PDF), SAT/ACT score, undergraduate school attended, MCAT scores, TOEFL scores (if applicable), health data
- Database storage for maintaining state
- “Check off” system for validating student information (for admin and staff)
- Admin login:
	- should be able to add student IDs and an initial password and the student interface should let the user change the password on the student's first login.
	- should be able to specify various artifacts that need to be collected.
		- enter number of documents needed to submit and then the name of each document
	- should be able to make reports of what has and hasn’t been submitted for students
- Staff login:
	- the “Check off” system for validating submitted student artifacts
	- should be able to submit artifacts to save to the database
- Student login:
	- should be able to submit artifacts to save to the database
- Security
	- Exclusively use database stored procedures to avoid SQL injection
	- Database encryption using Amazon AWS

## Out of scope:
- sending emails/texts to students and staff
- Functionality for mobile application
