# Math Center and Science Center Sign Ins
Created by: _Tiffany Jansen_

Current Version: Implemented Fall 2019.

## Features Include:
1. Ability to remember students after they have signed in the first time.
2. Ability for students to select their class on 1 page instead of 4.
3. Ability for students to sign into multiple classes.
4. Ability for faculty to download sign in data in an excel file.
5. Ability for faculty to upload current classes by uploading an excel file instead of copying into a text field.

## Steps to Deploy
1. Pull the repo into the folder the repo exists on the server.
    * The repo should exist in the folder with the .git folder in it.
        * It will also contain the MathTutor folder that won't be found anywhere else.
    * Open Git Bash in that folder.
        * You can right click in the folder and select "Open Git Bash Here"
    * Run _git pull origin master_ in the console.
        * If it asks for a password email me and I'll give it to you. 
    * Wait for it to load. You now have the current code versions of the math and science center websites.
2. Get the connection strings found in the current deployed versions.
    * Open the folder with the deployed versions (/var/www/ ??)
        * Find the files called _Web.config_.
            * There will be one for each project, so make sure you find both.
        * Open it and find the section _connectionString_.
        * Copy the entire tag.
        * Paste them into Notepad or another place you will be able to find it easily.
            * Make sure you keep the math and science ones separate.
3. Place the connection strings into the pulled versions of the code.
    * Follow the same steps as above, but with the code you just got from git.
        * Instead of copying the connection strings, you want to replace them with the ones that were deployed.
4. Move new code to where the old code is.
5. Open the _.sln_ file, this will automatically open the project in Visual Studio.
6. Deploy
    * Click on the build tab.
    * Click on the publish link.
    * Click deploy.
        * Note: I don't know if the "clicking deploy" will work... 
7. Do a quick check to make sure it's up and working.
8. You should be done.

*Side Note:* If you find any steps are missing please write them down or email them to me so I can update this so in the future when we have to deploy (cause I know there will be more updates...) we have easy to follow steps that will be easy to follow. Also, if you can give exact paths to the files/folders mentioned here that would be great. :)
