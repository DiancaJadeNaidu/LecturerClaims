# LecturerClaims

Welcome to the Contract Monthly Claim System (CMCS).
This application streamlines the submission, verification, and approval of lecturer claims, ensuring efficient processing for academic institutions.
Lecturers can submit claims, upload supporting documents, and track claim status, while Programme Coordinators and Academic Managers can verify and approve these claims easily.

# Getting Started
## Prerequisites
Development Environment: Visual Studio 2019 or later

Framework: .NET Core 3.1 or later

Operating System: Windows, macOS, or Linux

## Installation and Setup
Download Visual Studio: Get Visual Studio 2019 or later from the official Microsoft website.

Install .NET Core: Download and install the .NET Core SDK from the official .NET website or via the Visual Studio installer.

Clone Repository: Clone the CMCS repository from GitHub or download the source code archive.

Open Solution: Open the solution file (.sln) in Visual Studio.

Build Solution: Build the solution to restore NuGet packages and compile the application.

Run Application: Start the application to launch the interface and begin using CMCS.

# Specifications
## Functional Specifications
User Roles: Lecturers, Programme Coordinators, and Academic Managers

Claim Submission: Lecturers can submit monthly claims with details like hours worked, hourly rate, and notes

Document Upload: Upload PDF, DOCX, or XLSX files as supporting documents

Claim Tracking: Track claim status in real time (Submitted, Verified, Approved, Rejected)

Approval Workflow: Claims reviewed by Programme Coordinators and Academic Managers

Real-Time Status Updates: Immediate updates after verification or approval

## Non-Functional Specifications
Performance: Optimized for quick load times and responsive updates

Reliability: Stable operations with robust error handling

Compatibility: Works across Windows, macOS, and Linux

Scalability: Supports multiple users and claims efficiently

Security: Role-based authentication and secure file handling

# Functionality
## Features
Submit Claims: Lecturers can submit monthly claims with details of hours worked and rates

Upload Supporting Documents: Attach files (PDF, DOCX, XLSX) to claims

Claim Tracking: View real-time claim status and comments

Approve/Reject Claims: Coordinators and Managers can verify, approve, or reject claims

Real-Time Updates: Instant feedback when claim status changes

Error Handling: User-friendly messages and graceful handling of errors

## Plugins and Packages
Microsoft.EntityFrameworkCore: Database management

Microsoft.AspNetCore.Identity: Role-based authentication and authorization

AutoMapper: Simplifies data transfer between models and views

System.IO: File handling for document uploads

Toastr Notifications: Real-time user notifications

jQuery: Enhances client-side interactivity

## Non-Functional Requirements
MVC Design: Uses Model-View-Controller architecture

System Testing: Includes tests for claim processing and error handling

Error Handling: Displays clear messages on failure for better user experience

# User Guide
## Lecturer View
Submit a Claim:

Enter hours worked, hourly rate, and additional information

Attach supporting documents using the ‘Upload’ button

Click ‘Submit’ to send the claim for verification

Track Claim:

View the real-time status of submitted claims and related comments

## Programme Coordinator & Academic Manager View
Verify Claims:

View a list of pending claims with details

Approve or reject claims by clicking the respective button

# Frequently Asked Questions (FAQs)
Q: Can I submit multiple claims at once?
A: Yes, you can submit multiple claims, but each must be submitted separately.

Q: What file types can I upload?
A: PDF, DOCX, and XLSX files.

Q: How do I track claim status?
A: Use the claim tracking section to see real-time updates.

Q: Can I edit a claim after submission?
A: No, but you can submit a new claim if needed.

Q: How are claims verified?
A: Programme Coordinators and Academic Managers review and decide whether to approve or reject claims.

# License
This project is licensed under the MIT License. See the LICENSE file for details.

# Author
Full Name: Dianca Jade Naidu

Email: diancanaidu@gmail.com


# Acknowledgements
OpenAI. (2024). ChatGPT. Retrieved from https://openai.com/

FreeCodeCamp. (2023). ASP.NET Core Tutorial [Video]. YouTube. https://youtu.be/EdRzfKSM2Xk?si=JwK2ChgAlTPHqtg3

CodeWithMosh. (2023). ASP.NET MVC Crash Course [Video]. YouTube. https://youtu.be/1XXY_5k8Nok?si=7E9GvP2JGZIa6ml0

Programming with Mosh. (2023). Entity Framework Core Tutorial [Video]. YouTube. https://youtu.be/vRqz_zUiJTw?si=fZOEKn9nCo6ieUJ4
