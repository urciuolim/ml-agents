This branch is a result of Mike and Agnes' project for NYU's Deep RL course for the Fall 2020 semester. It is a fork off of Unity's ML Agents, as we used the SoccerTwos environment that seemed to have a lot of dependencies on various files on their master branch. Instead of trying to figure out which files we could and could not delete, we simply built upon their project.

After cloning this repo, first download Unity Hub: https://unity3d.com/get-unity/download

Within Unity Hub, go to the "installs" tab and click the Add button. It will ask you to select a Unity version, choose the 2019 LTS version (at the time of this writting this is a more updated version that what was used to create our project, but it works).

Next, go back to Projects, click Add, navigate to the cloned repo, and select the "Project" folder. An example filepath would be the following:
C:\Users\Michael\Documents\GitHub\ml-agents\Project

Next, select the 2019 Unity version. You might get a warning here, choose the option that accepts the newer version of Unity. Finally, click the project in the list of projects to open it within Unity.

Upon opening, the IDE will most likely be empty. In the project file explorer at the bottom of the screen, navigate to Assets > ML-Agents > Examples > Soccer > Scenes > _SoccerTwosDeepRLDemo.unity.

This is our demo scene. Upon clicking the play button, various models will exhibit the behavior shown in our presentation. Selected displays 1-4 in the upper left hand corner of the Game camera (middle screen) switches between each stage of our project. You can control the agent in stage 2, labeled "Expert" demo.
