# BeeLog Web Application

Welcome to the BeeLog web application! BeeLog is a social media platform that enables users to share posts, interact with other users, and more.

## Table of Contents

1. [Introduction](#introduction)
2. [Sign In and Sign Up](#sign-in-and-sign-up)
3. [Home Page](#home-page)
4. [Profile Page](#profile-page)
5. [Search Users](#search-users)
6. [Tech Stack](#tech-stack)
7. [Usage](#usage)
8. [Contributing](#contributing)
9. [License](#license)

## Introduction <a name="introduction"></a>

BeeLog is a social media platform that provides users with the ability to share posts, interact with posts, and connect with other users.

## Sign In and Sign Up <a name="sign-in-and-sign-up"></a>

### User Authentication

BeeLog features a secure user authentication system. Users can create an account or sign in to access the platform's features.

### Sign In Form

The Sign In page allows registered users to log in using their email and password. The form ensures the user enters a valid email and a corresponding password.
![Sign In](images/img1.jpg)

### Sign Up Form

New users can create an account using the Sign Up page. The form collects the user's name, email, and password. Additionally, the user's profile picture can be uploaded during the sign-up process.
![Sign Up](images/img2.jpg)

### Password Matching Feature

Both the Sign In and Sign Up forms ensure that the entered passwords match by displaying a password mismatch message if the passwords do not match.
![Password Matching Feature](images/img3.jpg)

## Home Page <a name="home-page"></a>

### Add a Post

The home page features a text area where users can write and post updates. Users can express their thoughts, ideas, or share content with others.
![Add a Post](images/img4.jpg)

### Display Posts

The home page displays posts from different users. Each post includes the user's profile picture, username, and the post content.
![Display Posts](images/img5.jpg)

If the user doesn’t follow anyone, BeeLog ‘ll suggest users to follow.
![Suggestions](images/img6.jpg)

### Like and Unlike Posts

Users can like or unlike posts by clicking the "Like" and "Unlike" buttons. The like count for each post updates in real-time.
![Like and Unlike Post](images/img7.jpg)

### View Comments

Users can view comments on each post by clicking the "View Comments" button. A modal window opens, displaying comments for the respective post.
![View Comments](images/img8.jpg)

## Profile Page <a name="profile-page"></a>

### View Profile Information

Users can view their own or other users' profiles. The profile page displays the user's profile picture, name, email, and bio.
![View Profile Information](images/img9.jpg)

### Profile Picture Management

Users can upload or change their profile picture by clicking the "Upload Profile Picture" button. This opens a modal window where users can select an image to upload.
![Profile Picture Management](images/img10.jpg)

### Edit Bio

Users with edit profile permissions can modify their bio. Clicking the "Edit bio" button opens a modal window where users can update their bio information.
![Edit Bio](images/img11.jpg)

### Follow and Unfollow Users

Users can follow or unfollow other users by clicking the respective buttons on the profile page. The page dynamically updates to show the current follower count.
![Follow and Unfollow Users](images/img12.jpg)

## Search Users <a name="search-users"></a>

### Search Input Field

The search feature allows users to find other users by entering a name in the search box. As users type, the application suggests similar names.

### Display Similar Names

After entering a search query, the application displays a list of users with names that closely match the search term.

### Direct Profile Links

Each displayed user's name is a clickable link that takes users to the respective profile page.

![Search Users](images/img13.jpg)

## Tech Stack <a name="tech-stack"></a>

1. ASP.NET MVC
2. SQL Server
3. jQuery
4. Bootstrap

## Usage <a name="usage"></a>

1. Sign up for a BeeLog account or log in if you already have an account.
2. Explore the home page to view posts from other users.
3. Like posts, view comments, and interact with other users.
4. Navigate to your profile page to manage your profile and posts.

## Contributing <a name="contributing"></a>

Contributions to BeeLog are welcome! If you find any issues or have suggestions for improvements, feel free to submit a pull request.

## License <a name="license"></a>

This project is licensed under the [MIT License](LICENSE).
