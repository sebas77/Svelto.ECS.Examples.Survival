# Svelto-ECS-Example
Example for Svelto ECS (https://github.com/sebas77/Svelto-ECS)

I used the Survival Shooter Unity Demo to show how an ECS framework could work inside Unity. I am not sure about the license of this demo, so use it only for learning purposes.

Most of the source code has been rewritten to work with the ECS framework. You should disregard the SveltEx folder, which has been added just for convenience. I have used some extra features from my framework, but I won't keep this folder up to date. Check my other repositories to find the most updated version.

The Survival Demo is compatible only with Unity 5.3

*Note: In this demo I extensively use explicit interface implementation. While I like them because the make the code clearer to read, I later found out that explicit implemented methods are marked as virtual. This could potentially impact the performance of the application, so it's better avoid it. It would be too much work to change the example code, so you have been warned.*
